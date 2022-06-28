$(document).ready(function () {
    
    initializeDatePicker();
    InitializeDataTable();
    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });
});

var hdnPaymentPlan;
var hdnBalanceFrom;
var hdnBalanceTo;
var hdnBankAccount;

function SetResetFilterOption() {
    var PreviousSelectedData = [];
    if (hdnPaymentPlan !== undefined && hdnPaymentPlan.length > 0) {
        PreviousSelectedData = hdnPaymentPlan.split(",");        
        $('#PaymentPlanIDs').val(PreviousSelectedData).trigger('change');
    }
    if (hdnBalanceFrom !== undefined && hdnBalanceFrom.length > 0) {
        $("#txtNetDepositFrom").val(hdnBalanceFrom);
        $("#txtNetDepositTo").val(hdnBalanceTo);
    }
    if (hdnBankAccount !== undefined && hdnBankAccount.length > 0) {
        PreviousSelectedData = hdnBankAccount.split(",");        
        $('#BankAccountIDs').val(PreviousSelectedData).trigger('change');
    }
}

function initializeDatePicker() {
    $('.periodDate').datepicker({
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture
    });
    $('.periodDate').datepicker('update', new Date());
    $(".select2dropdown").select2({ width: '100%' });
}

$(window).on("load", function () {
    $('#btnGo').focus();
});

$(document).on('click', '#btnGo', function () {
    if (validateDates()) {
        InitializeDataTable();
        return true;
    }
    return false;
});

$(document).on('change', '#PaymentPlanIDs', function () {
    if ($(this).val() && $(this).val()[0] === 0) {
        $(this).val("").click();
        $("#PaymentPlanIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

$(document).on('change', '#BankAccountIDs', function () {
    if ($(this).val() && $(this).val()[0] === 0) {
        $(this).val("").click();
        $("#BankAccountIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

function clearSearch(filterCriteria) {
    if (validateDates()) {
        if (filterCriteria === 'balance') {
            $("#txtNetDepositFrom").val('');
            $("#txtNetDepositTo").val('');
            $("#NetDepositFrom").val('');
            $("#NetDepositTo").val('');
            $("#spnBalanceRange").addClass('hide');
            hdnBalanceFrom = "";
            hdnBalanceTo = "";
        }
        if (filterCriteria === 'bankaccount') {            
            $('#BankAccountIDs').val([]).trigger('change');
            $("#spnSelectedBankAccount").addClass('hide');
            hdnBankAccount = "";
        }
        if (filterCriteria === 'paymentplan') {            
            $('#PaymentPlanIDs').val([]).trigger('change');
            $("#spnSelectedPaymentPlan").addClass('hide');
            hdnPaymentPlan = "";
        }
        InitializeDataTable();
    }
}

function validateDates() {
    var isvalid = true;
    $("#StartPeriodDate,#EndPeriodDate").removeClass("error");
    if ($("#StartPeriodDate").val() === '') {
        showAlert('error', $("#StartPeriodDate").attr("data-required-msg"));
        $("#StartPeriodDate").addClass("error");
        isvalid = false;
    }
    if ($("#EndPeriodDate").val() === '') {
        showAlert('error', $("#EndPeriodDate").attr("data-required-msg"));
        $("#EndPeriodDate").addClass("error");
        isvalid = false;
    }
    if (new Date($("#EndPeriodDate").datepicker('getDate')) < new Date($("#StartPeriodDate").datepicker('getDate'))) {
        showAlert('error', $("#EndPeriodDate").attr("data-compare-val-msg"));
        isvalid = false;
    }

    if (isvalid) {
        hdnBankAccount = getBankAccountIDs();
        hdnPaymentPlan = getPaymentPlanIDs();
        hdnBalanceFrom = $("#txtNetDepositFrom").val();
        hdnBalanceTo = $("#txtNetDepositTo").val();
        $("#FilterPaymentPlanID").val(hdnPaymentPlan);
        $("#FilterBankAccountID").val(hdnBankAccount);
        $("#txtNetDepositFrom").val(hdnBalanceFrom);
        $("#txtNetDepositTo").val(hdnBalanceTo);
    }
    return isvalid;
}

function validateForm() {
    if ($("#txtNetDepositTo").val() !== "" && $("#txtNetDepositFrom").val() === "")
        showAlert('error', balanceFromRequiredMsg);
    else if ($("#txtNetDepositFrom").val() !== "" && $("#txtNetDepositTo").val() === "")
        showAlert('error', balanceToRequiredMsg);
    else if (
        $("#txtNetDepositTo").val() !== "" && $("#txtNetDepositFrom").val() !== ""
        &&
        parseInt($("#txtNetDepositFrom").val()) > parseInt($("#txtNetDepositTo").val())
    )
        showAlert('error', compareDepositValidationMsg);
    else {
        $("#NetDepositFrom").val($("#txtNetDepositFrom").val());
        $("#NetDepositTo").val($("#txtNetDepositTo").val());
        hdnPaymentPlan = getPaymentPlanIDs();
        hdnBalanceFrom = $("#txtNetDepositFrom").val();
        hdnBalanceTo = $("#txtNetDepositTo").val();
        hdnBankAccount = getBankAccountIDs();
        refreshData();
        $("#FilterPaymentPlanID").val(hdnPaymentPlan);
        $("#FilterBankAccountID").val(hdnBankAccount);
        return true;
    }
    return false;
}

function refreshData() {
    var selectedBankAccountTexts = "";
    var selectedPaymentPlanTexts = "";
    InitializeDataTable();
    $("#advanceSearchModal").modal('hide');
    if ($("#txtNetDepositFrom").val() !== '' && $("#txtNetDepositTo").val() !== '' && parseFloat($("#txtNetDepositFrom").val()) >= 0 && parseFloat($("#txtNetDepositTo").val()) > 0) {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnBalanceRange").removeClass('hide');
        $("#SearchText").text(BalanceRange + " : " + $("#txtNetDepositFrom").val() + " - " + $("#txtNetDepositTo").val());
    }
    else {
        $("#spnBalanceRange").addClass('hide');
        $("#SearchText").text('');
    }
    selectedBankAccountTexts = getBankAccountText();
    if (selectedBankAccountTexts !== '') {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnSelectedBankAccount").removeClass('hide');
        $("#BankAccountIdSearchText").text(SelectedBankAccount + " : " + selectedBankAccountTexts);
    }
    else {
        $("#spnSelectedBankAccount").addClass('hide');
        $("#BankAccountIdSearchText").text('');
    }
    selectedPaymentPlanTexts = getPaymentPlanText();
    if (selectedPaymentPlanTexts !== '') {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnSelectedPaymentPlan").removeClass('hide');
        $("#PaymentPlanIdSearchText").text(SelectedPaymentPlan + " : " + selectedPaymentPlanTexts);
    }
    else {
        $("#spnSelectedPaymentPlan").addClass('hide');
        $("#PaymentPlanIdSearchText").text('');
    }
}

function loadAdvanceSearch() {
    $("#txtNetDepositFrom").val("");
    $("#txtNetDepositTo").val("");   
    $('#PaymentPlanIDs').val([]).trigger('change');
    $('#BankAccountIDs').val([]).trigger('change');

    SetResetFilterOption();
    if (!validateDates()) {
        return false;
    }
    else {
        $("#advanceSearchModal").modal('show');
        $("#txtNetDepositFrom").focus();
    }
}

function getPaymentPlanIDs() {
    var selectPaymentPlanList = [];
    if ($("#PaymentPlanIDs").select2('data').length) {
        $.each($("#PaymentPlanIDs").select2('data'), function (key,item) {
            if (item.id !== 0)
                selectPaymentPlanList += "," + item.id;
        });
    }
    var selectPaymentPlanId = "";
    if (selectPaymentPlanList.length > 0)
        selectPaymentPlanId = selectPaymentPlanList.substring(1, selectPaymentPlanList.length);
    return selectPaymentPlanId;
}

function getPaymentPlanText() {
    var selectPaymentPlanList = [];
    if ($("#PaymentPlanIDs").select2('data').length) {
        $.each($("#PaymentPlanIDs").select2('data'), function (key,item) {
            if (item.id !== 0)
                selectPaymentPlanList += "," + item.text;
        });
    }
    var selectedPaymentPlanTexts = "";
    if (selectPaymentPlanList.length > 0)
        selectedPaymentPlanTexts = selectPaymentPlanList.substring(1, selectPaymentPlanList.length);
    return selectedPaymentPlanTexts;
}

function getBankAccountIDs() {
    var selectBankAccountList = [];
    if ($("#BankAccountIDs").select2('data').length) {
        $.each($("#BankAccountIDs").select2('data'), function (key,item) {
            if (item.id !== 0)
                selectBankAccountList += "," + item.id;
        });
    }
    var selectedBankAccountIDs = "";
    if (selectBankAccountList.length > 0)
        selectedBankAccountIDs = selectBankAccountList.substring(1, selectBankAccountList.length);
    return selectedBankAccountIDs;
}

function getBankAccountText() {
    var selectBankAccountList = [];
    if ($("#BankAccountIDs").select2('data').length) {
        $.each($("#BankAccountIDs").select2('data'), function (key,item) {
            if (item.id !== 0)
                selectBankAccountList += "," + item.text;
        });
    }
    var selectedBankAccountTexts = "";
    if (selectBankAccountList.length > 0)
        selectedBankAccountTexts = selectBankAccountList.substring(1, selectBankAccountList.length);
    return selectedBankAccountTexts;
}

var depositTotal = 0;
function InitializeDataTable() {
    $('#tblDepositSummeryByPaymentPlanList').dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg,
            oPaginate: {
                sPrevious: previous,
                sNext: next
            }
        },
        "responsive": true,
        "serverSide": true,
        "processing": true,
        "bFilter": false,
        "bInfo": false,
        "bLengthChange": false,
        "autoWidth": false,
        "pageLength": pageSize,
        "ordering": false,
        "paging": false,
        "scrollY": "100vh",
        "scrollX": true,
        "scrollCollapse": true,
        "ajax": {
            "url": ROOTPath + "/Reports/Report/CollectionDepositSummaryByPaymentPlan",
            "type": "POST",
            "data": function (data) {
                data.startPeriod = new Date($("#StartPeriodDate").datepicker('getDate')).toDateString();
                data.endPeriod = new Date($("#EndPeriodDate").datepicker('getDate')).toDateString();
                data.netDepositFrom = $("#txtNetDepositFrom").val();
                data.netDepositTo = $("#txtNetDepositTo").val();
                data.paymentPlanIDs = getPaymentPlanIDs();
                data.bankAccountIDs = getBankAccountIDs();
            }
            , "dataSrc": function (json) {
                depositTotal = json.depositTotal;
                return json.data;
            }
        },
        destroy: true,
        "columns": [
            {
                name: "Number", title: number, "data": "Number", className: "col-lg-1"
            },
            {
                name: "Date", title: date, "data": "FormattedDate", className: "col-lg-1 text-left"
            },
            {
                name: "DepositTypeName", title: depositType, "data": "DepositTypeName", className: "col-lg-1 text-left"
            },
            {
                name: "Bank", title: bank, "data": "BankName", className: "col-lg-2 text-left"
            },
            {
                name: "BankAccount", title: bankaccount, "data": "BankAccountName", className: "col-lg-2 text-left"
            },
            {
                name: "Description", title: description, "data": "Description", className: "col-lg-2 text-left"
            },
            {
                name: "ClosingCount", title: closingcount, "data": "ClosingCount", className: "col-lg-1 text-right"
            },
            {
                name: "NetDeposit", title: netdeposit, "data": "FormattedNetDeposit", className: "col-lg-2 text-right"
            }
        ],
        "lengthMenu": [[15, 25, 50, 100], [15, 25, 50, 100]],
        "order": [[1, "asc"]],
        "fnRowCallback": function (iDisplayIndex) {
            var row = $("#tblDepositSummeryByPaymentPlanList").DataTable().row(iDisplayIndex);
            var data = row.data();
            createChild(iDisplayIndex, data.ClosedEntryList);
        },
        "footerCallback": function (row) {
            $(row).find("td").eq("1").text(depositTotal);
            $("#tblFooter").removeClass("hide");
        }
    });

}

function createChild(idx, d) {
    $.post(ROOTPath + "/Reports/Report/GetClosingPaymentPlanHtml", { "closingListJSON": JSON.stringify(d) }, function (response) {
        if (response.status === false) {
            showAlert("error", response.message);
        }
        else {
            $("#tblDepositSummeryByPaymentPlanList").DataTable().row(idx).child(response).show();
        }
    });
}

function Print() {
    if (validateDates()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintDepositSummaryByPaymentPlan",
            data: { 'startPeriod': $("#StartPeriodDate").val(), 'endPeriod': $("#EndPeriodDate").val(), 'netDepositFrom': $("#txtNetDepositFrom").val() === '' ? null : $("#txtNetDepositFrom").val(), 'netDepositTo': $("#txtNetDepositTo").val() === '' ? null : $("#txtNetDepositTo").val(), 'paymentPlanIDs': getPaymentPlanIDs(), 'bankAccountIDs': getBankAccountIDs() },
            beforeSend: function () {
                showLoading();
            },
            success: function (response) {
                if (response.status) {
                    hideLoading();
                    var printWindow = window.open('', '_blank');
                    printWindow.document.write(response.data);
                    var fun = function () {
                        printWindow.document.close();
                        setTimeout(function () { printWindow.print(); }, 20);
                        printWindow.onfocus = function () { setTimeout(function () { printWindow.close(); }, 20); };
                    };
                    var img = printWindow.document.getElementById('img-responsive');
                    if (img.complete) {
                        fun.call(img);
                    } else {
                        img.onload = fun
                    }
                    return false;
                }
                else {
                    showAlert('error', response.data);
                    return false;
                }
            }
        });
    }
    else
        return false;
    return true;
}