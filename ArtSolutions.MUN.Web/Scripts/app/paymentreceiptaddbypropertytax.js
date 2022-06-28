$(window).on("load", function () {
    $('#AccountPropertyID').next().find('.select2-selection').focus();
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

    GetAccountPropertyForSelect('AccountPropertyID', dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg);
});

function initSelect2($selector) {
    if ($selector == null || $selector == "")
        $('.select2dropdown').not('#AccountPropertyID').select2({ width: '100%' });
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
function loadAccountPropertyRight(source) {
    if ($(source).val() > 0) {
        $.ajax({
            url: ROOTPath + "/Payment/GetAccountPropertyRight",
            data: { "accountPropertyID": $(source).val() },
            success: function (response) {
                if (response.status == undefined) {
                    $("#dvPropertyRight select").select2('destroy');
                    $("#dvPropertyRight").html('').html(response);
                    initSelect2(null);
                    loadAccountServiceCollectionDetail();
                    $('#AccountPropertyID').next().find('.select2-selection').focus();
                }
                else
                    showAlert("error", response.message);
            }
        });

    }
    else {
        $("#AccountPropertyRightID").find('option').not(':first').remove();        
        $('#AccountPropertyRightID').val(null).trigger('change');
        $("#CreditAmount").val(GlobalFormatWithText(GlobalConvertToDecimal("0")));
        $("#CreditAmount").data("credit", GlobalFormatWithText(GlobalConvertToDecimal("0")));
    }
}

$("#dvPropertyRight").on("change", "#AccountPropertyRightID", function () {
    loadAccountServiceCollectionDetail();
    if ($(this).val() > 0) {
        var AccountID = $("#AccountPropertyRightID option:selected").attr('data-accountID');
        $("#AccountID").val(AccountID);
        $.ajax({
            url: ROOTPath + "/Payment/GetCreditNoteBalance",
            data: { "accountID": AccountID },
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
    }
    else {
        $("#CreditAmount").val(GlobalFormatWithText(GlobalConvertToDecimal("0")));
        $("#CreditAmount").data("credit", GlobalFormatWithText(GlobalConvertToDecimal("0")));
    }
});
$(document).on("focusout", ".amountinput", function (e) {
    loadAccountServiceCollectionDetail();
    setDefaultValue(this);

});
function getPaymentOptionTotal() {
    var total = 0;
    $(".amountinput").each(function (index) {
        if (GlobalConvertToDecimal($(this).val()) > 0)
            total = total + GlobalConvertToDecimal($(this).val());
    });
    $("#paymentoptionTotal").attr("data-total", GlobalFormat(total));
    $("#paymentoptionTotal").html(GlobalFormatWithText(total));
    return total;
}
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
function loadAccountServiceCollectionDetail() {
    var AccountPropertyRightID = $("#AccountPropertyRightID").val();
    var total = getPaymentOptionTotal();
    if (total == undefined || isNaN(total) || total == "")
        total = 0;
    if (AccountPropertyRightID == undefined || AccountPropertyRightID == "") {
        AccountPropertyRightID = 0;
        selectedItems = []
    }
    var AccountPropertyID = $("#AccountPropertyID").val();
    if (AccountPropertyID == undefined || AccountPropertyID == "") {
        AccountPropertyID = 0;
        selectedItems = [];
    }
    pushSelectedItems();
    $.ajax({
        url: ROOTPath + "/Payment/GetAccountServiceCollectionPropertyTaxDetail",
        data: { "accountPropertyID": AccountPropertyID, "accountPropertyRightID": AccountPropertyRightID, "paymentDate": $("#Date").val(), 'TotalAmount': total, 'selectedItemIds': JSON.stringify(selectedItems) },
        success: function (response) {
            if (response.status == undefined) {
                $("#collectiondetailRows").html(response);
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
function removePaymentOption(ele) {
    $(ele).parents("tr").remove();
    loadAccountServiceCollectionDetail();
}
function confirmPayment() {
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
            if (confirmed)
                $("#btnSave").prop("type", "submit").click().prop("type", "button");
            else
                return false;
        });
    }
    else
        return false;
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
function paymentSuccessCallback(response) {
    if (!response.status) {
        var errmsg = response.message;
        if (response.message.indexOf('|') > -1) { errmsg = errmsg.replace(/\|/g, "<br />"); }
        showAlert("error", errmsg);
    } else {
        var url = (ROOTPath + "/Collections/Payment/Index");
        if (response.paymentID != undefined && response.paymentID != null && response.paymentID > 0) {
            url = (ROOTPath + "/Collections/Payment/View?ID=" + response.paymentID + "&Type=PropertyTax&TypeID=20");
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