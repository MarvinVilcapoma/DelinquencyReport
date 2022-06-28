var selectedItemsAccountPaymentPlanDetailIIDArray = [];
var Isprint = false;
$(window).on("load", function () {
    $('#AccountID').next().find('.select2-selection').focus();
});

$("#btnSave").click(function (event) {
    $("#hfAccountID").val($("#AccountID").val());
});

$("#btnSaveNew").click(function (event) {
    $("#hfAccountID").val($("#AccountID").val());
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
    }).on("changeDate", function () {
        loadAccountPaymentPlan();
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

function setDefaultValue(obj) {
    if ($(obj).val().trim().length <= 0) {
        $(obj).val(CurrencyGlobalFormat(0));
    }
    else if (isNaN(GlobalConvertToDecimal($(obj).val())))
        $(obj).val(CurrencyGlobalFormat(0));
    else
        $(obj).val(CurrencyGlobalFormat(GlobalConvertToDecimal($(obj).val())));
    $(obj).attr('maxlength', 15);
}

function fixedDecimal(values) {
    return (Globalize.numberFormatter({ minimumFractionDigits: __decimalPoints })(values));
}

function loadAccountPaymentPlan() {
    if ($("#AccountID").val() > 0) {
        $.ajax({
            url: ROOTPath + "/Payment/GetAccountPaymentPlan",
            data: { "accountID": $("#AccountID").val(), "receiptDate": $('#Date').val() },
            success: function (response) {
                if (response.status == undefined) {
                    $("#dvAccountPlan").html(response);
                    initSelect2(null);
                    loadAccountPaymentPlanDetail();
                    $('#AccountID').next().find('.select2-selection').focus();
                }
                else
                    showAlert("error", response.message);
            }
        }).then(function () {
            $.ajax({
                url: ROOTPath + "/Payment/GetAvailableCreditBalance",
                data: { "accountID": $("#AccountID").val() },
                success: function (response) {
                    if (response.status) {
                        $("#CreditAmount").val(CurrencyGlobalFormat(response.Balance));
                        $("#CreditAmount").val(GlobalFormatWithText(response.Balance));
                        $("#CreditAmount").data("credit", CurrencyGlobalFormat(response.Balance));
                    }
                    else
                        showAlert("error", response.message);
                }
            });
        });
    }
    else {
        $("#AccountPaymentPlanIDs").find('option').remove();
        $("#AccountPaymentPlanIDs").val(null).trigger('change');
        $("#CreditAmount").val(GlobalFormatWithText(GlobalConvertToDecimal("0")));
        $("#CreditAmount").data("credit", CurrencyGlobalFormat(GlobalConvertToDecimal("0")));
    }
}

function getAccountPaymentPlanIDs() {
    var selectAccountPlanList = [];
    if ($("#AccountPaymentPlanIDs").select2('data').length) {
        $.each($("#AccountPaymentPlanIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectAccountPlanList += "," + item.id;
        });
    }
    var selectedAccountPlanIDs = "";
    if (selectAccountPlanList.length > 0)
        selectedAccountPlanIDs = selectAccountPlanList.substring(1, selectAccountPlanList.length);
    return selectedAccountPlanIDs;
}

function loadAccountPaymentPlanDetail() {
    if ($("#AccountPaymentPlanIDs").val() && $("#AccountPaymentPlanIDs").val()[0] == 0) {
        $("#AccountPaymentPlanIDs").val("").click();
        $("#AccountPaymentPlanIDs option:gt(0)").prop('selected', true);
        $("#AccountPaymentPlanIDs").trigger('change');
    }
    //var paymentplanId = $("#AccountPaymentPlanID").val();
    var paymentplanId = getAccountPaymentPlanIDs();
    //if (paymentplanId == undefined || paymentplanId == "")
    //    paymentplanId = 0;
    var accountId = $("#AccountID").val();
    if (accountId == undefined || accountId == "")
        accountId = 0;
    //if (paymentplanId == 0) {   
    if (paymentplanId == "") {
        selectedItemsAccountPaymentPlanDetailIIDArray = [];
    }
   
    $.ajax({
        url: ROOTPath + "/Payment/GetAccountPaymentPlanDetail",
        type: "post",
        data:
        {
            "accountId": accountId,
            'accountpaymentplanIds': paymentplanId,
            "AccountPaymentPlanDetailIDs": selectedItemsAccountPaymentPlanDetailIIDArray.join(","),
            "isRemoveInterest": $('#IsRemoveInterest').is(':checked'),
        },
        success: function (response) {
            if (response.status == undefined) {
                $("#paymemtplandetailRows").html(response);

                if ($("#hdnTotal").val() != undefined && $("#hdnTotal").val() != null && $("#hdnTotal").val() != "") {
                    $(".amountinput:first").val(CurrencyGlobalFormat(GlobalConvertToDecimal($("#hdnTotal").val())));
                    //$("#paymentoptionTotal").html(CurrencyGlobalFormatWithText(GlobalConvertToDecimal($("#hdnTotal").val())));
                }
                var total = getPaymentOptionTotal();
                if ($("#ApplyCreditAmount").val() > 0) {
                    if ($("#hdnTotal").val() != null && $("#hdnTotal").val() != "" && $("#tblPaymentOptions .amountinput").length == 1)
                        $(".amountinput:first").val(CurrencyGlobalFormat(total));
                }
            }
            else
                showAlert("error", response.message);
         
        }
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

$(document).on("focusout", ".amountinput", function (e) {
    if ($("#ApplyCreditAmount").val() > 0) {
        var total = $("#hdnTotal").val()
        if ($("#hdnTotal").val() != null && $("#hdnTotal").val() != "" && $("#tblPaymentOptions .amountinput").length == 1)
            $(".amountinput:first").val(CurrencyGlobalFormat(GlobalConvertToDecimal(total)));
    }
    total = getPaymentOptionTotal();
    if ($("#hdnTotal").val() != null && $("#hdnTotal").val() != "" && $("#tblPaymentOptions .amountinput").length == 1)
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
    if ($("#ApplyCreditAmount").val() > 0 && $("#tblPaymentOptions .amountinput").length == 1 && $("#hdnTotal").val() > 0) {
        total = $("#hdnTotal").val() - GlobalConvertToDecimal($("#ApplyCreditAmount").val())
    }
    return total;
}

function removePaymentOption(ele) {
    $(ele).parents("tr").remove();
    var total = getPaymentOptionTotal();
    if ($("#ApplyCreditAmount").val() > 0) {
        if ($("#hdnTotal").val() != null && $("#hdnTotal").val() != "" && $("#tblPaymentOptions .amountinput").length == 1)
            $(".amountinput:first").val(CurrencyGlobalFormat(total));
    }
}

function confirmPayment(actionType) {
    var retVal = true;
    setDefaultValue($("#ApplyCreditAmount"));
    if (GlobalConvertToDecimal($("#CreditAmount").data("credit")) < GlobalConvertToDecimal($("#ApplyCreditAmount").val())) {
        showAlert("warning", ValCreditBalance);
        retVal = false;
    }
    else if ($("#ApplyCreditAmount").val() != "" && GlobalConvertToDecimal($("#ApplyCreditAmount").val()) < 0) {
        showAlert("warning", ValCreditBalancePositive);
        retVal = false;
    }
    if (retVal && $("#frmPayment").valid()) {
        swal({
            title: swalTitleText,
            type: "warning",
            showCancelButton: true,
            cancelButtonText: cancelBtnText,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: postPayment,
            closeOnConfirm: true
        }, function (confirmed) {
            if (confirmed) {
                var accServiceCollectionDetailServiceIDs = "";
                selectedItemsAccountPaymentPlanDetailIIDArray = [];
                $(".chkSelectedItem:checked").each(function (index) {
                    accServiceCollectionDetailServiceIDs = $(this).attr("dataAccountPaymentPlanDetailID");
                    selectedItemsAccountPaymentPlanDetailIIDArray.push(accServiceCollectionDetailServiceIDs);
                });
                $("#AccountPaymentPlanDetailIDs").val(selectedItemsAccountPaymentPlanDetailIIDArray.join(","))
                if (actionType == "paymentplansave") {
                    $("#btnSave").prop("type", "submit").click().prop("type", "button");
                }
                if (actionType == "paymentplansaveandcontinue") {
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
            var accServiceCollectionDetailServiceIDs = "";
            selectedItemsAccountPaymentPlanDetailIIDArray = [];
            $(".chkSelectedItem:checked").each(function (index) {
                accServiceCollectionDetailServiceIDs = $(this).attr("dataAccountPaymentPlanDetailID");
                selectedItemsAccountPaymentPlanDetailIIDArray.push(accServiceCollectionDetailServiceIDs);
            });
            $("#AccountPaymentPlanDetailIDs").val(selectedItemsAccountPaymentPlanDetailIIDArray.join(","))
            if (confirmed) {
                //$("#btnSave").prop("type", "submit").click().prop("type", "button");                
                Isprint = true;
            }
            else {
                Isprint = false;
            }
            if (actionType == "paymentplansave") {
                $("#btnSave").prop("type", "submit").click().prop("type", "button");
            }
            if (actionType == "paymentplansaveandcontinue") {
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
        if (response.actionType == "paymentplansaveandcontinue") {
            url = (ROOTPath + "/Collections/Payment/Add?ActionType=paymentplan");
        }
        else {
            url = (ROOTPath + "/Collections/Payment/Index");

        }
        if (Isprint == true) {
            if (response.paymentID != undefined && response.paymentID != null && response.paymentID > 0) {
                //Printurl = (ROOTPath + "/Collections/Payment/Print?ID=" + response.paymentID + "&Type=PaymentPlan");
                Printurl = (ROOTPath + "/Collections/Payment/RollPrint?ID=" + response.paymentID + "&Type=PaymentPlan");
                window.open(Printurl, 'name');
            }
        }
        window.location.href = url;
    }
}

function GetAccountPaymentPlanServiceDetailSelectedItems(e) {
    var accServiceCollectionDetailServiceIDs = "";
    selectedItemsAccountPaymentPlanDetailIIDArray = [];
    $(".chkSelectedItem:checked").each(function (index) {
        accServiceCollectionDetailServiceIDs = $(this).attr("dataAccountPaymentPlanDetailID");
        selectedItemsAccountPaymentPlanDetailIIDArray.push(accServiceCollectionDetailServiceIDs);
    });



    //if ($(e).is(':checked')) {                
    //    selectedItemsAccountPaymentPlanDetailIIDArray.push(accServiceCollectionDetailServiceIDs);                
    //}
    //else {       
    //    selectedItemsAccountPaymentPlanDetailIIDArray = jQuery.grep(selectedItemsAccountPaymentPlanDetailIIDArray, function (value) {
    //        return value != accServiceCollectionDetailServiceIDs;
    //    });
    //}
    loadAccountPaymentPlanDetail();
}

function onPaymentBegin() {
    var retVal = true;
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
