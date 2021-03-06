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

    $(document).on("blur", ".amountinput", function (e) {
        loadAccountServiceCollectionDetail();
        setDefaultValue(this);

    });

    $('#Date').datepicker({
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture
    }).on("changeDate", function () {
        loadAccountServiceCollectionDetail();
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
        $(obj).val(GlobalFormat(0));
    }
    else if (isNaN(GlobalConvertToDecimal($(obj).val())))
        $(obj).val(GlobalFormat(0));
    else
        $(obj).val(GlobalFormat(GlobalConvertToDecimal($(obj).val())));
    $(obj).attr('maxlength', 15);
}

function fixedDecimal(values) {
    return (Globalize.numberFormatter({ minimumFractionDigits: __decimalPoints })(values));
}

function loadAccountServiceType(source) {
    if ($(source).val() > 0) {

        var accountId = $("#AccountID").val();
        if (accountId == undefined || accountId == "") {
            accountId = 0;
            selectedItems = [];
        }
        pushSelectedItems();
        $.ajax({
            url: ROOTPath + "/Payment/GetAccountServiceCollectionDetailOnly",
            data:
            {
                "accountId": accountId,
                "paymentDate": $("#Date").val(),
                "IsIvaApply": $("#IVAPayment").prop("checked"),
                "ApplybyAmnesty": $("#ApplybyAmnesty").prop("checked")
            },
            success: function (response) {
                if (response.status == undefined) {
                    $("#collectiondetailRows").html(response);
                    //if ($("#hdnTotal").val() != null && $("#hdnTotal").val() != "" && $("#tblPaymentOptions .amountinput").length == 1)
                    //    $(".amountinput:first").val(CurrencyGlobalFormat(GlobalConvertToDecimal($("#hdnTotal").val())));
                    //var total = getPaymentOptionTotal();
                    //if ($("#ApplyCreditAmount").val() > 0) {
                    //    if ($("#hdnTotal").val() != null && $("#hdnTotal").val() != "" && $("#tblPaymentOptions .amountinput").length == 1)
                    //        $(".amountinput:first").val(CurrencyGlobalFormat(total));
                    //}
                    $('#AccountID').next().find('.select2-selection').focus();
                }
                else
                    showAlert("error", response.message);
            }
        }).then(function () {
            $.ajax({
                url: ROOTPath + "/Payment/GetAvailableCreditBalance",
                data: { "accountID": $(source).val() },
                success: function (response) {
                    if (response.status) {
                        $("#CreditAmount").val(GlobalFormat(response.Balance));
                        $("#CreditAmount").val(GlobalFormatWithText(response.Balance));
                        $("#CreditAmount").data("credit", GlobalFormat(response.Balance));
                    }
                    else
                        showAlert("error", response.message);
                }
            });
        }).then(function () {
            $.ajax({
                url: ROOTPath + "/Payment/IsAccountHavePaymentPlan",
                data: { "accountID": $(source).val() },
                success: function (response) {
                    if (response.status) {
                        if (response.IsAccountPayemntPlan) {
                            $(".lblPlanWarning").html(isHaveAccountPaymentPlanMsg);
                        }
                        else {
                            $(".lblPlanWarning").html("");
                        }
                    }
                    else
                        showAlert("error", response.message);
                }
            });
        });


        //loadAccountServiceCollectionDetailOnly();
        //$('#AccountID').next().find('.select2-selection').focus();

        //$.ajax({
        //    url: ROOTPath + "/Payment/GetAvailableCreditBalance",
        //    data: { "accountID": $(source).val() },
        //    success: function (response) {
        //        if (response.status) {
        //            $("#CreditAmount").val(GlobalFormat(response.Balance));
        //            $("#CreditAmount").val(GlobalFormatWithText(response.Balance));
        //            $("#CreditAmount").data("credit", GlobalFormat(response.Balance));
        //        }
        //        else
        //            showAlert("error", response.message);
        //    }
        //});

        //$.ajax({
        //    url: ROOTPath + "/Payment/IsAccountHavePaymentPlan",
        //    data: { "accountID": $(source).val() },
        //    success: function (response) {
        //        if (response.status) {
        //            if (response.IsAccountPayemntPlan) {
        //                $(".lblPlanWarning").html(isHaveAccountPaymentPlanMsg);
        //            }
        //            else {
        //                $(".lblPlanWarning").html("");
        //            }
        //        }
        //        else
        //            showAlert("error", response.message);

        //    }
        //});
    }
    else {
        $("#CreditAmount").val(GlobalFormatWithText(GlobalConvertToDecimal("0")));
        $("#CreditAmount").data("credit", GlobalFormatWithText(GlobalConvertToDecimal("0")));
    }
}

$(document).on("click", ".chkAll", function (e) {
    var isChecked = $(this).is(":checked");
    if (isChecked) {
        $(this).closest("table").find("input[type='checkbox']").prop("checked", true);
    }
    else {
        $(this).closest("table").find("input[type='checkbox']").prop("checked", false);
    }
    loadAccountServiceCollectionDetail();
});

var selectedItems = [];
function pushSelectedItems() {
    selectedItems = [];
    if ($(".collectionItem").length > 0) {
        $(".collectionItem").each(function (index) {
            if ($(this).is(':checked'))
                selectedItems.push($(this).attr("data-itemid"));
        });
    }
}

function pullSelectedItems() {
    $.each(selectedItems, function (index, el) {
        $("tr[id='" + selectedItems[index] + "']").find(".collectionItem").prop("checked", true);
    });
}

function getPaymentOptionTotal() {

    var total = 0;
    $(".amountinput").each(function (index) {
        if (GlobalConvertToDecimal($(this).val()) > 0)
            total = total + GlobalConvertToDecimal($(this).val());
    });
    if ($("#ApplyCreditAmount").val() > 0 && $("#tblPaymentOptions .amountinput").length == 1) {
        total = total - GlobalConvertToDecimal($("#ApplyCreditAmount").val())
    }

    $("#paymentoptionTotal").attr("data-total", GlobalFormat(total));
    $("#paymentoptionTotal").html(CurrencyGlobalFormatWithText(total));

    ////pending = test multiple time same code commented
    total = 0;
    $(".collectionItem:checked").each(function (index) {
        if ($(this).closest('tr').find('.hdnTotalAmount').val() > 0)
            total = total + parseFloat($(this).closest('tr').find('.hdnTotalAmount').val());
    });
    if ($("#ApplyCreditAmount").val() > 0 && $("#tblPaymentOptions .amountinput").length == 1) {
        total = total - GlobalConvertToDecimal($("#ApplyCreditAmount").val())
    }
    return total;
}

function loadAccountServiceCollectionDetail() {
    var total = getPaymentOptionTotal();
    if (total == undefined || isNaN(total) || total == "")
        total = 0;
    var accountId = $("#AccountID").val();
    if (accountId == undefined || accountId == "") {
        accountId = 0;
        selectedItems = [];
    }
    pushSelectedItems();
    $.ajax({
        url: ROOTPath + "/Payment/GetAccountServiceCollectionDetail",
        type: "post",
        data:
        {
            "accountId": accountId,
            "paymentDate": $("#Date").val(),
            "selectedItemIds": selectedItems.join(","),
            "IsIvaApply": $("#IVAPayment").prop("checked"),
            "ApplybyAmnesty": $("#ApplybyAmnesty").prop("checked")
        },
        success: function (response) {
            if (response.status == undefined) {
                $("#collectiondetailRows").html('').html(response);
                if ($("#hdnTotal").val() != null && $("#hdnTotal").val() != "" && $("#tblPaymentOptions .amountinput").length == 1)
                    $(".amountinput:first").val(CurrencyGlobalFormat(GlobalConvertToDecimal($("#hdnTotal").val())));

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

function loadAccountServiceCollectionDetailOnly() {
    var accountId = $("#AccountID").val();
    if (accountId == undefined || accountId == "") {
        accountId = 0;
        selectedItems = [];
    }
    pushSelectedItems();
    $.ajax({
        url: ROOTPath + "/Payment/GetAccountServiceCollectionDetailOnly",
        data:
        {
            "accountId": accountId,
            "paymentDate": $("#Date").val(),
            "IsIvaApply": $("#IVAPayment").prop("checked"),
            "ApplybyAmnesty": $("#ApplybyAmnesty").prop("checked")
        },
        success: function (response) {
            if (response.status == undefined) {
                $("#collectiondetailRows").html(response);
                if ($("#hdnTotal").val() != null && $("#hdnTotal").val() != "" && $("#tblPaymentOptions .amountinput").length == 1)
                    $(".amountinput:first").val(CurrencyGlobalFormat(GlobalConvertToDecimal($("#hdnTotal").val())));
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
            closeOnConfirm: false
        }, function (confirmed) {
            if (confirmed) {
                console.log(actionType)
                if (actionType == "servicesave") {
                    $("#btnSave").prop("type", "submit").click().prop("type", "button");
                }
                if (actionType == "servicesaveandcontinue") {
                    $("#btnSaveNew").prop("type", "submit").click().prop("type", "button");
                }
            }
            else {
                return false;
            }
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
                Isprint = true;
            }
            else {
                Isprint = false;
            }
            if (actionType == "servicesave") {
                $("#btnSave").prop("type", "submit").click().prop("type", "button");
            }
            if (actionType == "servicesaveandcontinue") {
                $("#btnSaveNew").prop("type", "submit").click().prop("type", "button");
            }
        });
    }
    else
        return false;
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
    loadAccountServiceCollectionDetail();
}

function paymentSuccessCallback(response) {
    if (!response.status) {
        var errmsg = response.message;
        if (response.message.indexOf('|') > -1) { errmsg = errmsg.replace(/\|/g, "<br />"); }
        showAlert("error", errmsg);
    } else {
        var url = null;

        if (response.actionType == "servicesaveandcontinue") {
            url = (ROOTPath + "/Collections/Payment/Add?ActionType=service");
        }
        else {
            url = (ROOTPath + "/Collections/Payment/Index");
        }
        if (Isprint == true) {
            if (response.paymentID != undefined && response.paymentID != null && response.paymentID > 0) {
                var Printurl = (ROOTPath + "/Collections/Payment/RollPrint?ID=" + response.paymentID + "&Type=Service&TypeID=" + response.serviceTypeId);
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