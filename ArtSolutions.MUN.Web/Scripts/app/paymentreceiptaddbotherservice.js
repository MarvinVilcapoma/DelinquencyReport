$(window).on("load", function () {
    $('#AccountID').next().find('.select2-selection').focus();
});

$(document).ready(function () {
    $('#ApplyCreditAmount').on("cut copy paste", function (e) {
        e.preventDefault();
    });
    $('#Date').datepicker({
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture,
    });
    $('#Date').datepicker('update', new Date());
    initSelect2(null);
    if ($(".DetailAmount").length <= 0)
        addServiceTypeOption();

    //Initialize Dropzone
    Dropzone.autoDiscover = false;

    if ($("#paymentFileDropzone").length) {
        Dropzone.autoDiscover = false;
        var dropzone = new Dropzone("#paymentFileDropzone", {
            paramName: "PaymentFile",
            url: ROOTPath + "/File/UploadFile",
            maxFiles: 1,
            maxFilesize: maxFileLength,
            acceptedFiles: allowedFileTypes,
            parallelUploads: 1,
            uploadMultiple: false,
            addRemoveLinks: true,
            thumbnailWidth: 80,
            thumbnailHeight: 80,
            init: function () {
                this.on("maxfilesexceeded", function (file) {
                    this.removeAllFiles();
                    this.addFile(file);
                });
                this.on("addedfile", function (file) {
                    if (this.files.length > this.options.maxFiles) {
                        this.removeFile(file);
                        return false;
                    }
                    // check for size                 
                    if (file.size > (this.options.maxFilesize / this.options.maxFiles) * 1024 * 1024) {
                        showAlert("warning", uploadmaxsizemsg + file.name);
                        this.removeFile(file);
                        return false;
                    }
                });
                this.on("removedfile", function (file) {
                    $("#AttachmentID").val(null);
                });
            },
            error: function (file, response) {
                file.previewElement.classList.add("dz-error");
                $(file.previewElement).find('.dz-error-message').text(response);
            },
            sending: function (file, xhr, data) {
            },
            success: function (file, response) {
                $("#AttachmentID").val(response.id);
                file.previewElement.classList.add("dz-success");
            }
        });
    }

    GetAccountForSelect('AccountID', null, '4,5,6', dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg);
});

function initSelect2($selector) {
    if ($selector == null || $selector == "")
        $('.select2dropdown').not('#AccountID').select2({ width: '100%' });
    else
        $("#" + $selector).select2({ width: '100%' });
}

//Set Zero Value If Input text is empty
function setDefaultValue(obj) {

    if ($(obj).val().trim().length <= 0) {
        $(obj).val(CurrencyGlobalFormat(0));
    }
    if (isNaN(GlobalConvertToDecimal($(obj).val())))
        $(obj).val(CurrencyGlobalFormat(0));
    else {
        $(obj).val(CurrencyGlobalFormat(GlobalConvertToDecimal($(obj).val())));
    }
    $(obj).attr('maxlength', 15);
}

//Return String Value
function fixedDecimal(values) {
    return (Globalize.numberFormatter({ minimumFractionDigits: __decimalPoints })(values));
}

//Return Number
function convertToDecimal(values) {
    return Number(values.trim());
}

//Total Amount Change Event
function onTotalAmountChange(obj) {
    setDefaultValue(obj);
    var values = CurrencyGlobalFormat(obj.value);
    $(obj).val(values);
}

function loadPostedInvoices(source) {
    if (!($("#AccountID").val() > 0)) {
        $("#invoiceRows").html('');
        var amount = "0";
        $("#spnNetPayment").text(CurrencyGlobalFormatWithText(GlobalConvertToDecimal(amount)));
        $("#NetPayment").val(CurrencyGlobalFormat(amount));
        $(".amountinput:first").val(CurrencyGlobalFormat(GlobalConvertToDecimal(amount)));
        $("#paymentoptionTotal").attr("data-total", CurrencyGlobalFormat(amount));
        $("#paymentoptionTotal").html(CurrencyGlobalFormatWithText(amount));
    }
    $.ajax({
        url: ROOTPath + "/Payment/GetAvailableCreditBalance",
        data: { "accountID": $(source).val() },
        success: function (response) {
            if (response.status) {
                $("#CreditAmount").val(GlobalFormatWithText(response.Balance));
                $("#CreditAmount").data("credit", CurrencyGlobalFormat(response.Balance));
            }
            else
                showAlert("error", response.message);
        }
    });
}

function addServiceTypeOption() {
    if ($("#addInvoiceRow").attr("disabled") == undefined) {
        $.ajax({
            url: ROOTPath + "/Invoice/GetServiceTypeOption",
            success: function (response) {
                if (response.status == undefined) {
                    $("#invoiceRows").append(response);
                    if ($(".checkbooklist").length > 0) {
                        var CheckbookHtml = '<td class="col-lg-2 checkbooklist"></td>';
                        $("td:eq(1)", $(".table-multi-row tbody tr:last")).after(CheckbookHtml);

                        $("td:lt(3)", $(".table-multi-row tbody tr")).removeClass("col-lg-3").addClass("col-lg-2");
                        $("th:lt(3)", $(".itemTypeHead").closest('tr')).removeClass("col-lg-3").addClass("col-lg-2");
                    }
                    InitializeDropDown();
                }
                else
                    showAlert("error", response.message);

            }
        });

    }
}

function removeRule(e) {
    $(e).closest('tr').remove();
    UpdateTotalAmount();
    $("#addInvoiceRow").removeAttr("disabled");
}

function UpdateTotalAmount() {
    var amount = 0;
    $(".DetailAmount").each(function (item, index) {
        if ($(this).val() != "") {
            amount = amount + GlobalConvertToDecimal($(this).val());
            $(this).val(CurrencyGlobalFormat(GlobalConvertToDecimal($(this).val())));

        }
    });
    $("#spnNetPayment").text(CurrencyGlobalFormatWithText(amount));
    $("#NetPayment").val(CurrencyGlobalFormat(amount));
    $(".amountinput:first").val(CurrencyGlobalFormat(amount));
    $("#paymentoptionTotal").attr("data-total", CurrencyGlobalFormat(amount));
    $("#paymentoptionTotal").html(CurrencyGlobalFormatWithText(amount));
}

function FillFINGranName(e) {
    $(e).closest('td').find("input.FinGrantName").val($("option:selected", $(e)).text().trim());
    $(e).closest('td').find("input.FinGrantCode").val($("option:selected", $(e)).data('code'));
    $(e).closest('td').find("input.ReceivableAccountID").val($("option:selected", $(e)).data('receivableaccountid'));
    $(e).closest('td').find("input.ReceivableCode").val($("option:selected", $(e)).data('receivableaccountcode'));
    $(e).closest('td').find("input.ReceivableName").val($("option:selected", $(e)).data('receivableaccountname'));
    $(e).closest('td').find("input.RevenueAccountID").val($("option:selected", $(e)).data('revenueaccountid'));
    $(e).closest('td').find("input.RevenueCode").val($("option:selected", $(e)).data('revenueaccountcode'));
    $(e).closest('td').find("input.RevenueName").val($("option:selected", $(e)).data('revenueaccountname'));
    $(e).closest('td').find("input.CreditAccountID").val($("option:selected", $(e)).data('creditaccountid'));
    $(e).closest('td').find("input.CreditAccountCode").val($("option:selected", $(e)).data('creditaccountcode'));
    $(e).closest('td').find("input.CreditAccountName").val($("option:selected", $(e)).data('creditaccountname'));
    if ($(e).val() > 0) {
        $.ajax({
            url: ROOTPath + "/Invoice/GetCheckbookList",
            dataType: 'json',
            data: { grantId: $(e).val(), FormValue: $(e).closest('tr').find('input[name="InvoiceDetail.index"]').val() },
            type: 'GET',
            success: function (result) {
                if (result.status) {
                    if ($(".CheckbookHead").length == 0) {
                        var CheckbookHeadHtml = '<th class="col-lg-2 CheckbookHead">\
                   '+ CheckBook + '</th >';

                        $(".itemTypeHead").after(CheckbookHeadHtml);
                        $(".FotterSpan").attr("colspan", "3");
                    }
                    if ($(".checkbooklist", $(e).closest('tr')).length > 0)
                        $(".checkbooklist", $(e).closest('tr')).remove();
                    $("td:lt(2)", $(e).closest('tr')).removeClass("col-lg-3").addClass("col-lg-2");
                    $("th:lt(3)", $(".itemTypeHead").closest('tr')).removeClass("col-lg-3").addClass("col-lg-2");
                    $(".itemtypelist", $(e).closest('tr')).after(result.collectiontemlate);
                    InitializeDropDown();
                    $('#InvoiceDetail_CollectionTemplateID').next().find('.select2-selection').focus();
                }
                else {
                    showAlert("error", result.message);
                }
            }
        });
    }
    else {
        if ($(".checkbooklist").length > 1) {
            $(".checkbooklist", $(e).closest('tr')).html('');
        }
        else {
            $(".CheckbookHead").remove();
            $(".checkbooklist").remove();
            $(".FotterSpan").attr("colspan", "2");
        }

    }
}

function InitializeDropDown() {
    $(".select2dropdown").not('#AccountID').select2({ width: '100%' });
}

function FillCheckbookName(e) {
    $(e).closest('td').find("input.CheckbookCode").val($("option:selected", $(e)).data('checkbookcode'));
    $(e).closest('td').find("input.CheckbookName").val($("option:selected", $(e)).data('checkbookname'));
    $(e).closest('td').find("input.CashAccountID").val($("option:selected", $(e)).data('cashaccountid'));
    $(e).closest('td').find("input.CashAccountCode").val($("option:selected", $(e)).data('cashaccountcode'));
    $(e).closest('td').find("input.CashAccountName").val($("option:selected", $(e)).data('cashaccountname'));
}

$("#AccountID").change(function () {
    $.ajax({
        url: ROOTPath + "/Payment/DebitNoteAlreadyExist",
        data: { 'AccountID': $(this).val() },
        success: function (response) {
            if (!response.status) {
                showAlert("error", response.message);
            }
        }
    });
});

function addPaymentOption() {
    $.ajax({
        url: ROOTPath + "/Payment/GetPaymentOption",
        success: function (response) {
            if (response.status == undefined) {
                $("#optionrows").append(response);
                initSelect2(null);
            }
            else
                showAlert("error", response.message);

        }
    });
}

function removePaymentOption(ele) {
    $(ele).parents("tr").remove();
    var total = 0;
    if ($("#ApplyCreditAmount").val() > 0) {

        if ($("#NetPayment").val() != "" && $("#tblPaymentOptions .amountinput").length == 1)
            $(".amountinput:first").val(CurrencyGlobalFormat(GlobalConvertToDecimal($("#NetPayment").val())));
    }
    total = getPaymentOptionTotal();
    if ($("#ApplyCreditAmount").val() > 0 && $("#NetPayment").val() != "" && $("#tblPaymentOptions .amountinput").length == 1)
        $(".amountinput:first").val(CurrencyGlobalFormat(total));
}

$(document).on("focusout", ".amountinput", function (e) {
    var total = 0;
    if ($("#ApplyCreditAmount").val() > 0) {

        if ($("#NetPayment").val() != "" && $("#tblPaymentOptions .amountinput").length == 1)
            $(".amountinput:first").val(CurrencyGlobalFormat(GlobalConvertToDecimal($("#NetPayment").val())));
    }
    var total = getPaymentOptionTotal();
    if ($("#ApplyCreditAmount").val() > 0 && $("#NetPayment").val() != "" && $("#tblPaymentOptions .amountinput").length == 1)
        $(".amountinput:first").val(CurrencyGlobalFormat(total));
    setDefaultValue(this);
});

function getPaymentOptionTotal() {
    var total = 0;
    $(".amountinput").each(function (index) {
        if (GlobalConvertToDecimal($(this).val()) > 0)
            total = total + GlobalConvertToDecimal($(this).val());
    });
    if ($("#ApplyCreditAmount").val() > 0 && $("#tblPaymentOptions .amountinput").length == 1) {
        total = total - GlobalConvertToDecimal($("#ApplyCreditAmount").val())
    }
    $("#paymentoptionTotal").attr("data-total", CurrencyGlobalFormat(total));
    $("#paymentoptionTotal").html(CurrencyGlobalFormatWithText(total));

    total = 0;
    if ($("#ApplyCreditAmount").val() > 0 && $("#tblPaymentOptions .amountinput").length == 1 && $("#NetPayment").val() != "") {
        total = GlobalConvertToDecimal($("#NetPayment").val()) - GlobalConvertToDecimal($("#ApplyCreditAmount").val())
    }
    return total;
}

function onPaymentBegin() {
    var retVal = true;
    var totAmount = $("#NetPayment").val() == "" || $("#NetPayment").val() == null ? 0 : convertToDecimal($("#NetPayment").val());
    if (totAmount <= 0) {
        showAlert("warning", ValTotalAmountRequired);
        retVal = false;
    }
    else {
        $row = $('table').find('tbody#invoiceRows')
            .find('tr')
            .has('.DetailAmount');
        if ($row.length <= 0) {
            showAlert("warning", SelectInvoice);
            retVal = false;
        }
    }

    if (GlobalConvertToDecimal($("#CreditAmount").data("credit")) < GlobalConvertToDecimal($("#ApplyCreditAmount").val())) {
        showAlert("warning", ValCreditBalance);
        retVal = false;
    }
    else if ($("#ApplyCreditAmount").val() != "" && GlobalConvertToDecimal($("#ApplyCreditAmount").val()) < 0) {
        showAlert("warning", ValCreditBalancePositive);
        retVal = false;
    }

    return retVal;
}

function confirmPayment(actionType) {
    setDefaultValue($("#ApplyCreditAmount"));
    if ($("#frmPayment").valid()) {
        swal({
            title: $("#Number").val() != "" && $("#Number").val() != undefined ? ManualReceiptConfirmMsg + " " + $("#Number").val() : swalTitleText,
            type: "warning",
            showCancelButton: true,
            cancelButtonText: cancelBtnText,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: postPayment,
            closeOnConfirm: true
        }, function (confirmed) {
            if (confirmed) {
                if (actionType == "invoicesave") {
                    $("#btnSave").prop("type", "submit").click().prop("type", "button");
                }
                if (actionType == "invoicesaveandcontinue") {
                    $("#btnSaveNew").prop("type", "submit").click().prop("type", "button");
                }
            }
            else
                return false;
        });
    }
    else
        return false;
}

function confirmprint(actionType) {
    var textmessage = "";
    var confMessage = "";
    setDefaultValue($("#ApplyCreditAmount"));
    if ($("#frmPayment").valid()) {
        swal({
            title: printmsg,
            text: textmessage,
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: yesBtnText,
            cancelButtonText: noBtnText,
            closeOnConfirm: true
        }, function (confirmed) {
            if (confirmed) {
                //$("#btnSave").prop("type", "submit").click().prop("type", "button");                
                Isprint = true;
            }
            else {
                Isprint = false;
            }
            if (actionType == "otherservicesave") {
                $("#btnSave").prop("type", "submit").click().prop("type", "button");
            }
            if (actionType == "otherservicesaveandcontinue") {
                $("#btnSaveNew").prop("type", "submit").click().prop("type", "button");
            }
        });
    }
    else
        return false;
}

function paymentSuccessCallback(response) {
    if (!response.status) {
        var errmsg = response.message;
        if (response.message.indexOf('|') > -1) { errmsg = errmsg.replace(/\|/g, "<br />"); }
        showAlert("error", errmsg);
    } else {
        var url = null;
        if (response.actionType == "otherservicesaveandcontinue") {
            url = (ROOTPath + "/Collections/Payment/Add?ActionType=otherservice&InvoiceID=0");
        }
        else {
            url = (ROOTPath + "/Collections/Payment/Index");

        }
        if (Isprint == true) {
            if (response.paymentID != undefined && response.paymentID != null && response.paymentID > 0) {
                //Printurl = (ROOTPath + "/Collections/Payment/Print?ID=" + response.paymentID + "&Type=Service&TypeID=" + response.serviceTypeId);
                Printurl = (ROOTPath + "/Collections/Payment/RollPrint?ID=" + response.paymentID + "&Type=OtherService&TypeID=" + response.serviceTypeId);
                window.open(Printurl, 'name');
            }
        }
        window.location.href = url;
    }
}

function toggleVisibility(isChecked, target) {
    if (isChecked)
        $("#" + target).removeClass("hide");
    else {
        $("#" + target).addClass("hide");
        $("#" + target + " input").val("");
        $("#" + target + " input").removeClass("input-validation-error");
        $("#" + target + " span.field-validation-error span").remove();
    }
}