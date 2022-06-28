var PaymentReceiptCount = 0;
$(window).on("load", function () {
    $('#CashierID').next().find('.select2-selection').focus();
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
});
function initSelect2($selector) {
    if ($selector == null || $selector == "")
        $(".select2dropdown").select2({ width: '100%' });
    else
        $("#" + $selector).select2({ width: '100%' });
}

function onClosingBegin() {
    var closingAmount = $("#NetClosing").val() == "" || $("#NetClosing").val() == null ? 0 : convertToDecimal($("#NetClosing").val());
    if (closingAmount == 0) {
        showAlert("warning", ValClosingAmount);
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

function closingSuccessCallback(response) {
    if (!response.status) {
        var errmsg = response.message;
        if (response.message.indexOf('|') > -1) { errmsg = errmsg.replace(/\|/g, "<br />"); }
        showAlert("error", errmsg);
    } else {
        var url = (ROOTPath + "/Collections/ClosingEntry/Index");
        if (response.closingentryID != undefined && response.closingentryID != null && response.closingentryID > 0) {
            url = (ROOTPath + "/Collections/ClosingEntry/View/" + response.closingentryID);
        }
        window.location.href = url;
    }
}

function confirmClosing() {
    if ($("#frmClosing").valid()) {
        swal({
            title: swalTitleText,
            type: "warning",
            showCancelButton: true,
            cancelButtonText: cancelBtnText,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: CloseEntry,
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

// Utility 
function convertToDecimal(values) {
    return Number(GlobalConvertToDecimal(values.trim()));
}
function fixedDecimal(values) {
    return (Globalize.numberFormatter({ minimumFractionDigits: __decimalPoints })(values));
}

function loadPaymentReceipts() {
    $("#CashierName").val($("#CashierID option:selected").text());

    $.ajax({      
        url: ROOTPath + "/ClosingEntry/GetPostedPaymentReceipts",
        data: {
            "cashierID": $("#CashierID").val(),
            "closingDate": new Date($("#Date").datepicker('getDate')).toDateString(),
            "paymentOptionID": $("#PaymentOptionID").val()
        },
        success: function (response) {
            if (response.status == undefined) {

                //Clear existing detail
                $('#tblClosingEntry tr input[type="checkbox"]').each(function () {
                    $(this).prop('checked', false);
                });

                $("#NetClosing").val(GlobalFormat(0));
                $("#spnNetClosing").text(CurrencyGlobalFormatWithTextAndThousandSeparator(0));
                $("#TotalCash").val(GlobalFormat(0));
                $("#spnTotalCash").text(CurrencyGlobalFormatWithTextAndThousandSeparator(0));
                $("#TotalChequedelBancoNacional").val(GlobalFormat(0));
                $("#spnTotalChequedelBancoNacional").text(CurrencyGlobalFormatWithTextAndThousandSeparator(0));
                $("#TotalCreditCard").val(GlobalFormat(0));
                $("#spnTotalCreditCard").text(CurrencyGlobalFormatWithTextAndThousandSeparator(0));
                $("#TotalBankTransfer").val(GlobalFormat(0));
                $("#spnTotalBankTransfer").text(CurrencyGlobalFormatWithTextAndThousandSeparator(0));
                $("#TotalChequedelBancodeCostaRica").val(GlobalFormat(0));
                $("#spnTotalChequedelBancodeCostaRica").text(CurrencyGlobalFormatWithTextAndThousandSeparator(0));
                $("#TotalAdjustment").val(GlobalFormat(0));
                $("#spnTotalAdjustment").text(CurrencyGlobalFormatWithTextAndThousandSeparator(0));
                $("#TotalBank").val(GlobalFormat(0));
                $("#spnTotalBank").text(CurrencyGlobalFormatWithTextAndThousandSeparator(0));
                $("#TotalBankTransferByBancoNacionaldeCostaRica").val(GlobalFormat(0));
                $("#spnTotalBankTransferByBancoNacionaldeCostaRica").text(CurrencyGlobalFormatWithTextAndThousandSeparator(0));
                $("#PaymentReceiptCount").val(0);
                $("#spnPaymentReceiptCount").text(0);

                PaymentReceiptCount = 0;
                //

                $("#receiptRows").html(response.html);
                $("#SelectedReceiptAll").closest('th').attr('data-netclosing', response.netClosing);
                $("#SelectedReceiptAll").closest('th').attr('data-receiptcount', response.receiptCount);

                $("#SelectedReceiptAll").closest('th').attr('data-totalCash', response.totalCash);
                $("#SelectedReceiptAll").closest('th').attr('data-totalChequedelBancoNacional', response.totalChequedelBancoNacional);
                $("#SelectedReceiptAll").closest('th').attr('data-totalCreditCard', response.totalCreditCard);
                $("#SelectedReceiptAll").closest('th').attr('data-totalBankTransfer', response.totalBankTransfer);
                $("#SelectedReceiptAll").closest('th').attr('data-totalChequedelBancodeCostaRica', response.totalChequedelBancodeCostaRica);
                $("#SelectedReceiptAll").closest('th').attr('data-totalAdjustment', response.totalAdjustment);
                $("#SelectedReceiptAll").closest('th').attr('data-totalBank', response.totalBank);
                $("#SelectedReceiptAll").closest('th').attr('data-totalBankTransferByBancoNacionaldeCostaRica', response.totalBankTransferByBancoNacionaldeCostaRica);

                if (parseInt(paymentreceiptlistCount) > parseInt(pagesize)) {
                    $('.footable').footable();
                }
                $('.footable').footable();
            }
            else
                showAlert("error", response.message);

        }
    });
}

function onReceiptSelect(obj) {
    $row = $($(obj).parent().parent());

    $receiptNetPayment = $($row.find(".netPayment"));
    receiptNetPayment = convertToDecimal($receiptNetPayment.val());
    NetClosing = convertToDecimal($("#NetClosing").val());

    $receiptTotalCash = $($row.find(".totalCash"));
    receiptTotalCash = convertToDecimal($receiptTotalCash.val());
    TotalCash = convertToDecimal($("#TotalCash").val());

    $receiptTotalChequedelBancoNacional = $($row.find(".totalChequedelBancoNacional"));
    receiptTotalChequedelBancoNacional = convertToDecimal($receiptTotalChequedelBancoNacional.val());
    TotalChequedelBancoNacional = convertToDecimal($("#TotalChequedelBancoNacional").val());

    $receiptTotalCreditCard = $($row.find(".totalCreditCard"));
    receiptTotalCreditCard = convertToDecimal($receiptTotalCreditCard.val());
    TotalCreditCard = convertToDecimal($("#TotalCreditCard").val());

    $receiptTotalBankTransfer = $($row.find(".totalBankTransfer"));
    receiptTotalBankTransfer = convertToDecimal($receiptTotalBankTransfer.val());
    TotalBankTransfer = convertToDecimal($("#TotalBankTransfer").val());

    $receiptTotalChequedelBancodeCostaRica = $($row.find(".totalChequedelBancodeCostaRica"));
    receiptTotalChequedelBancodeCostaRica = convertToDecimal($receiptTotalChequedelBancodeCostaRica.val());
    TotalChequedelBancodeCostaRica = convertToDecimal($("#TotalChequedelBancodeCostaRica").val());

    $receiptTotalAdjustment = $($row.find(".totalAdjustment"));
    receiptTotalAdjustment = convertToDecimal($receiptTotalAdjustment.val());
    TotalAdjustment = convertToDecimal($("#TotalAdjustment").val());

    $receiptTotalBank = $($row.find(".totalBank"));
    receiptTotalBank = convertToDecimal($receiptTotalBank.val());
    TotalBank = convertToDecimal($("#TotalBank").val());

    $receiptTotalBankTransferByBancoNacionaldeCostaRica = $($row.find(".totalBankTransferByBancoNacionaldeCostaRica"));
    receiptTotalBankTransferByBancoNacionaldeCostaRica = convertToDecimal($receiptTotalBankTransferByBancoNacionaldeCostaRica.val());
    TotalBankTransferByBancoNacionaldeCostaRica = convertToDecimal($("#TotalBankTransferByBancoNacionaldeCostaRica").val());

    if ($(obj).is(":checked")) {
        NetClosing = (NetClosing + receiptNetPayment);
        TotalCash = (TotalCash + receiptTotalCash);
        TotalChequedelBancoNacional = (TotalChequedelBancoNacional + receiptTotalChequedelBancoNacional);
        TotalCreditCard = (TotalCreditCard + receiptTotalCreditCard);
        TotalBankTransfer = (TotalBankTransfer + receiptTotalBankTransfer);
        TotalChequedelBancodeCostaRica = (TotalChequedelBancodeCostaRica + receiptTotalChequedelBancodeCostaRica);
        TotalAdjustment = (TotalAdjustment + receiptTotalAdjustment);
        TotalBank = (TotalBank + receiptTotalBank);
        TotalBankTransferByBancoNacionaldeCostaRica = (TotalBankTransferByBancoNacionaldeCostaRica + receiptTotalBankTransferByBancoNacionaldeCostaRica);

        PaymentReceiptCount++;

        if ($('.chkSelectedReceipt:checked').length == $('.chkSelectedReceipt').length) {
            $('#SelectedReceiptAll').prop('checked', true);
        }
    } else {
        NetClosing = convertToDecimal(GlobalFormat((NetClosing - receiptNetPayment)));
        TotalCash = convertToDecimal(GlobalFormat((TotalCash - receiptTotalCash)));
        TotalChequedelBancoNacional = convertToDecimal(GlobalFormat((TotalChequedelBancoNacional - receiptTotalChequedelBancoNacional)));
        TotalCreditCard = convertToDecimal(GlobalFormat((TotalCreditCard - receiptTotalCreditCard)));
        TotalBankTransfer = convertToDecimal(GlobalFormat((TotalBankTransfer - receiptTotalBankTransfer)));
        TotalChequedelBancodeCostaRica = convertToDecimal(GlobalFormat((TotalChequedelBancodeCostaRica - receiptTotalChequedelBancodeCostaRica)));
        TotalAdjustment = convertToDecimal(GlobalFormat((TotalAdjustment - receiptTotalAdjustment)));
        TotalBank = convertToDecimal(GlobalFormat((TotalBank - receiptTotalBank)));
        TotalBankTransferByBancoNacionaldeCostaRica = convertToDecimal(GlobalFormat((TotalBankTransferByBancoNacionaldeCostaRica - receiptTotalBankTransferByBancoNacionaldeCostaRica)));

        PaymentReceiptCount--;

        if ($('#SelectedReceiptAll').prop('checked')) {
            $('#SelectedReceiptAll').prop('checked', false);
        }
    }

    $("#NetClosing").val(GlobalFormat(NetClosing));
    $("#spnNetClosing").text(CurrencyGlobalFormatWithTextAndThousandSeparator(NetClosing));

    $("#TotalCash").val(GlobalFormat(TotalCash));
    $("#spnTotalCash").text(CurrencyGlobalFormatWithTextAndThousandSeparator(TotalCash));

    $("#TotalChequedelBancoNacional").val(GlobalFormat(TotalChequedelBancoNacional));
    $("#spnTotalChequedelBancoNacional").text(CurrencyGlobalFormatWithTextAndThousandSeparator(TotalChequedelBancoNacional));

    $("#TotalCreditCard").val(GlobalFormat(TotalCreditCard));
    $("#spnTotalCreditCard").text(CurrencyGlobalFormatWithTextAndThousandSeparator(TotalCreditCard));

    $("#TotalBankTransfer").val(GlobalFormat(TotalBankTransfer));
    $("#spnTotalBankTransfer").text(CurrencyGlobalFormatWithTextAndThousandSeparator(TotalBankTransfer));

    $("#TotalChequedelBancodeCostaRica").val(GlobalFormat(TotalChequedelBancodeCostaRica));
    $("#spnTotalChequedelBancodeCostaRica").text(CurrencyGlobalFormatWithTextAndThousandSeparator(TotalChequedelBancodeCostaRica));

    $("#TotalAdjustment").val(GlobalFormat(TotalAdjustment));
    $("#spnTotalAdjustment").text(CurrencyGlobalFormatWithTextAndThousandSeparator(TotalAdjustment));

    $("#TotalBank").val(GlobalFormat(TotalBank));
    $("#spnTotalBank").text(CurrencyGlobalFormatWithTextAndThousandSeparator(TotalBank));

    $("#TotalBankTransferByBancoNacionaldeCostaRica").val(GlobalFormat(TotalBankTransferByBancoNacionaldeCostaRica));
    $("#spnTotalBankTransferByBancoNacionaldeCostaRica").text(CurrencyGlobalFormatWithTextAndThousandSeparator(TotalBankTransferByBancoNacionaldeCostaRica));

    $("#PaymentReceiptCount").val(PaymentReceiptCount);
    $("#spnPaymentReceiptCount").text(PaymentReceiptCount);
}

function onReceiptSelectAll(obj) {

    var NetClosing = 0;
    var TotalCash = 0;
    var TotalChequedelBancoNacional = 0;
    var TotalCreditCard = 0;
    var TotalBankTransfer = 0;
    var TotalChequedelBancodeCostaRica = 0;
    var TotalAdjustment = 0;
    var TotalBank = 0;
    var TotalBankTransferByBancoNacionaldeCostaRica = 0;

    PaymentReceiptCount = 0;

    if ($(obj).is(":checked")) {

        NetClosing = convertToDecimal($("#tblClosingEntry #SelectedReceiptAll").closest('th').attr('data-netclosing'));
        TotalCash = convertToDecimal($("#tblClosingEntry #SelectedReceiptAll").closest('th').attr('data-totalCash'));
        TotalChequedelBancoNacional = convertToDecimal($("#tblClosingEntry #SelectedReceiptAll").closest('th').attr('data-totalChequedelBancoNacional'));
        TotalCreditCard = convertToDecimal($("#tblClosingEntry #SelectedReceiptAll").closest('th').attr('data-totalCreditCard'));
        TotalBankTransfer = convertToDecimal($("#tblClosingEntry #SelectedReceiptAll").closest('th').attr('data-totalBankTransfer'));
        TotalChequedelBancodeCostaRica = convertToDecimal($("#tblClosingEntry #SelectedReceiptAll").closest('th').attr('data-totalChequedelBancodeCostaRica'));
        TotalAdjustment = convertToDecimal($("#tblClosingEntry #SelectedReceiptAll").closest('th').attr('data-totalAdjustment'));
        TotalBank = convertToDecimal($("#tblClosingEntry #SelectedReceiptAll").closest('th').attr('data-totalBank'));
        TotalBankTransferByBancoNacionaldeCostaRica = convertToDecimal($("#tblClosingEntry #SelectedReceiptAll").closest('th').attr('data-totalBankTransferByBancoNacionaldeCostaRica'));

        PaymentReceiptCount = convertToDecimal($("#tblClosingEntry #SelectedReceiptAll").closest('th').attr('data-receiptcount'));
        $(".chkSelectedReceipt").prop("checked", true);

    } else {
        $(".chkSelectedReceipt").prop("checked", false);
    }

    $("#NetClosing").val(GlobalFormat(NetClosing));
    $("#spnNetClosing").text(CurrencyGlobalFormatWithTextAndThousandSeparator(NetClosing));

    $("#TotalCash").val(GlobalFormat(TotalCash));
    $("#spnTotalCash").text(CurrencyGlobalFormatWithTextAndThousandSeparator(TotalCash));

    $("#TotalChequedelBancoNacional").val(GlobalFormat(TotalChequedelBancoNacional));
    $("#spnTotalChequedelBancoNacional").text(CurrencyGlobalFormatWithTextAndThousandSeparator(TotalChequedelBancoNacional));

    $("#TotalCreditCard").val(GlobalFormat(TotalCreditCard));
    $("#spnTotalCreditCard").text(CurrencyGlobalFormatWithTextAndThousandSeparator(TotalCreditCard));

    $("#TotalBankTransfer").val(GlobalFormat(TotalBankTransfer));
    $("#spnTotalBankTransfer").text(CurrencyGlobalFormatWithTextAndThousandSeparator(TotalBankTransfer));

    $("#TotalChequedelBancodeCostaRica").val(GlobalFormat(TotalChequedelBancodeCostaRica));
    $("#spnTotalChequedelBancodeCostaRica").text(CurrencyGlobalFormatWithTextAndThousandSeparator(TotalChequedelBancodeCostaRica));

    $("#TotalAdjustment").val(GlobalFormat(TotalAdjustment));
    $("#spnTotalAdjustment").text(CurrencyGlobalFormatWithTextAndThousandSeparator(TotalAdjustment));

    $("#TotalBank").val(GlobalFormat(TotalBank));
    $("#spnTotalBank").text(CurrencyGlobalFormatWithTextAndThousandSeparator(TotalBank));

    $("#TotalBankTransferByBancoNacionaldeCostaRica").val(GlobalFormat(TotalBankTransferByBancoNacionaldeCostaRica));
    $("#spnTotalBankTransferByBancoNacionaldeCostaRica").text(CurrencyGlobalFormatWithTextAndThousandSeparator(TotalBankTransferByBancoNacionaldeCostaRica));

    $("#PaymentReceiptCount").val(PaymentReceiptCount);
    $("#spnPaymentReceiptCount").text(PaymentReceiptCount);
}