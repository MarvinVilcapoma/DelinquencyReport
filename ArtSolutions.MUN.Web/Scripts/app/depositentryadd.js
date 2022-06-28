var closingReceiptCount = 0;

$(window).on("load", function () {
    $('#DepositTypeID').next().find('.select2-selection').focus();
});

$(document).ready(function () {
    $('.inputdate').datepicker({
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture,
    });
    $('.inputdate').datepicker('update', new Date());
    initSelect2(null);
    if (parseInt(ClosedEntryListCount) > parseInt(pagesize)) {
        $('.footable').footable();
    }


    $("#dvdepositentry").on("change", "#BankAccountID", function () {
        if ($(this).val() != "") {
            var financeAccountID = $(this).find("option:selected").data('financeaccountid');
            var financeAccountName = $(this).find("option:selected").data('financeaccountname');
            $("#AccountID").val(financeAccountID);
            $("#FinanceAccount").val(financeAccountName);

            var accountCode = $(this).find("option:selected").data('accountcode');
            var accountName = $(this).find("option:selected").data('accountname');
            var bankName = $(this).find("option:selected").data('name');
            $("#BankAccountCode").val(accountCode);
            $("#BankName").val(bankName);
            $("#BankAccountName").val(accountName);
        }
    });
});

function initSelect2($selector) {
    if ($selector == null || $selector == "")
        $(".select2dropdown").select2({ width: '100%' });
    else
        $("#" + $selector).select2({ width: '100%' });
}

function onDepositBegin() {
    var depositAmount = $("#NetDeposit").val() == "" || $("#NetDeposit").val() == null ? 0 : convertToDecimal($("#NetDeposit").val());
    if (depositAmount == 0) {
        showAlert("warning", ValDepositAmount);
        return false;
    }
    $row = $('table').find('tbody#receiptRows')
        .find('tr')
        .has('input[type=checkbox]:checked');
    if ($row.length <= 0) {
        showAlert("warning", valMinOneReceipt);
        return false;
    }
    return true;
}

function depositSuccessCallback(response) {
    if (!response.status) {
        showAlert("error", response.message);
    } else {
        var url = (ROOTPath + "/Collections/DepositEntry/Index");
        if (response.depositentryID != undefined && response.depositentryID != null && response.depositentryID > 0) {
            url = (ROOTPath + "/Collections/DepositEntry/View/" + response.depositentryID);
        }
        window.location.href = url;
    }

}

function confirmDeposit() {
    if ($("#frmDeposit").valid()) {
        swal({
            title: swalTitleText,
            type: "warning",
            showCancelButton: true,
            cancelButtonText: cancelBtnText,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: depositEntry,
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

function onEntrySelect(obj) {
    $row = $($(obj).parent().parent());
    $receiptClosingPayment = $($row.find(".netClosing"));
    receiptClosingPayment = convertToDecimal($receiptClosingPayment.val());

    NetDeposit = convertToDecimal($("#NetDeposit").val());

    if ($(obj).is(":checked")) {
        NetDeposit = (NetDeposit + receiptClosingPayment);
        closingReceiptCount++;

        if ($('.chkSelectedEntry:checked').length == $('.chkSelectedEntry').length) {
            $('#SelectedEntrytAll').prop('checked', true);
        }
    } else {
        NetDeposit = convertToDecimal(GlobalFormat((NetDeposit - receiptClosingPayment)));
        closingReceiptCount--;

        if ($('#SelectedEntrytAll').prop('checked')) {
            $('#SelectedEntrytAll').prop('checked', false);
        }
    }

    $("#NetDeposit").val(GlobalFormat(NetDeposit));
    $("#spnNetDeposit").text(GlobalFormatWithText(NetDeposit));
    $("#ClosingCount").val(closingReceiptCount);
    $("#spnClosingReceiptCount").text(closingReceiptCount);
}

function onEntrySelectAll(obj) {
    receiptClosingPayment = convertToDecimal($("#SelectedEntrytAll").closest('th').attr('data-netclosing'));//$(obj).closest('th').data('netclosing');
    closingReceiptCount = convertToDecimal($("#SelectedEntrytAll").closest('th').attr('data-receiptcount')); //$(obj).closest('th').data('receiptcount');
    NetDeposit = convertToDecimal($("#NetDeposit").val());

    if ($(obj).is(":checked")) {
        $(".chkSelectedEntry").prop("checked", true);
        NetDeposit = receiptClosingPayment;
    } else {
        $(".chkSelectedEntry").prop("checked", false);
        NetDeposit = 0;
        closingReceiptCount = 0;
    }
    $("#NetDeposit").val(GlobalFormat(NetDeposit));
    $("#spnNetDeposit").text(GlobalFormatWithText(NetDeposit));
    $("#ClosingCount").val(closingReceiptCount);
    $("#spnClosingReceiptCount").text(closingReceiptCount);
}

// Utility 
function convertToDecimal(values) {
    return Number(GlobalConvertToDecimal(values.trim()));
}

function fixedDecimal(values) {
    values = Number(values.toString().trim());
    return values.toFixed(__decimalPoints);
}