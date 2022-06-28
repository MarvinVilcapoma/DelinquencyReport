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
    $.ajax({
        url: ROOTPath + "/Payment/GetPostedInvoices",
        data: { "accountID": $(source).val(), "type": "invoice" },
        success: function (response) {
            if (response.status == undefined)
                $("#invoiceRows").html(response);
            else
                showAlert("error", response.message);

        }
    }).then(function () {
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
    });
}

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
    total = getPaymentOptionTotal();
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

//Checkbox Check/Uncheck Event And Net Amount Calculation
function onInvoiceSelect(obj) {
    $row = $($(obj).parent().parent());
    $balance = $($row.find("td .balance"));
    $selectedinvoice = $($row.find("td .selectedinvoice"));
    $(".selectedinvoice").val(false);
    $selectedinvoice.val(true);
    $("#spnNetPayment").text(CurrencyGlobalFormatWithText(GlobalConvertToDecimal($balance.val())));
    $("#NetPayment").val(CurrencyGlobalFormat(GlobalConvertToDecimal($balance.val())));
}

function onPaymentBegin() {
    var retVal = true;
    var totAmount = $("#NetPayment").val() == "" || $("#NetPayment").val() == null ? 0 : convertToDecimal($("#NetPayment").val());
    if (totAmount <= 0) {
        showAlert("warning", ValTotalAmountRequired);
        retVal = false;
    }
    else {
        $row = $('table').find('tbody#rowdetail')
            .find('tr')
            .has('input[type=radio]:checked');
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
            if (actionType == "invoicesave") {
                $("#btnSave").prop("type", "submit").click().prop("type", "button");
            }
            if (actionType == "invoicesaveandcontinue") {
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
        if (response.actionType == "invoicesaveandcontinue") {
            url = (ROOTPath + "/Collections/Payment/Add?ActionType=invoice&InvoiceID=0");
        }
        else {
            url = (ROOTPath + "/Collections/Payment/Index");

        }
        if (Isprint == true) {
            if (response.paymentID != undefined && response.paymentID != null && response.paymentID > 0) {
                //Printurl = (ROOTPath + "/Collections/Payment/Print?ID=" + response.paymentID + "&Type=Service&TypeID=" + response.serviceTypeId);
                Printurl = (ROOTPath + "/Collections/Payment/RollPrint?ID=" + response.paymentID + "&Type=Service&TypeID=" + response.serviceTypeId);
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