$(window).on("load", function () {
    $('#btnGo').focus();
});
$(document).ready(function () {
    
    $('.periodDate').datepicker({
        defaultDate: new Date(),
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture
    });
    $('.periodDate').datepicker('update', new Date());
    $(".select2dropdown").select2({ width: '100%' });
    InitializeDataTable();
    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });
});

$(document).on('click', '#btnGo', function () {
    if (validateDates()) {
        InitializeDataTable();
        return true;
    }
    return false;
});
var hdnServiceType;
var hdnBalanceFrom;
var hdnBalanceTo;
var hdnBankAccount;
function SetResetFilterOption() {
    var PreviousSelectedData = [];
    if (hdnServiceType != undefined && hdnServiceType.length > 0) {
        PreviousSelectedData = hdnServiceType.split(",");        
        $('#ServiceTypeIDs').val(PreviousSelectedData).trigger('change');
    }
    if (hdnBalanceFrom != undefined && hdnBalanceFrom.length > 0) {
        $("#txtNetDepositFrom").val(hdnBalanceFrom);
        $("#txtNetDepositTo").val(hdnBalanceTo);
    }
    if (hdnBankAccount != undefined && hdnBankAccount.length > 0) {
        PreviousSelectedData = hdnBankAccount.split(",");        
        $('#BankAccountIDs').val(PreviousSelectedData).trigger('change');
    }
}
function validateDates() {
    var isvalid = false;
    if ($("#StartPeriodDate").val() == '')
        showAlert('error', $("#StartPeriodDate").attr("data-required-msg"));
    else if ($("#EndPeriodDate").val() == '')
        showAlert('error', $("#EndPeriodDate").attr("data-required-msg"));
    else if (new Date($("#EndPeriodDate").val()) < new Date($("#StartPeriodDate").val()))
        showAlert('error', $("#EndPeriodDate").attr("data-compare-val-msg"));
    else
        isvalid = true;

    if (isvalid) {
        hdnBankAccount = getBankAccountIDs();
        hdnServiceType = getServiceTypeIDs();
        hdnBalanceFrom = $("#txtNetDepositFrom").val();
        hdnBalanceTo = $("#txtNetDepositTo").val();
        $("#FilterServiceTypeID").val(hdnServiceType);
        $("#FilterBankAccountID").val(hdnBankAccount);
    }
    return isvalid;
}
function getServiceTypeIDs() {
    var selectServiceTypeList = [];
    if ($("#ServiceTypeIDs").select2('data').length) {
        $.each($("#ServiceTypeIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectServiceTypeList += "," + item.id;
        });
    }
    var selectServiceTypeId = "";
    if (selectServiceTypeList.length > 0)
        selectServiceTypeId = selectServiceTypeList.substring(1, selectServiceTypeList.length);
    return selectServiceTypeId;
}

function getBankAccountIDs() {
    var selectBankAccountList = [];
    if ($("#BankAccountIDs").select2('data').length) {
        $.each($("#BankAccountIDs").select2('data'), function (key, item) {
            if (item.id != 0)
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
        $.each($("#BankAccountIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectBankAccountList += "," + item.text;
        });
    }
    var selectedBankAccountTexts = "";
    if (selectBankAccountList.length > 0)
        selectedBankAccountTexts = selectBankAccountList.substring(1, selectBankAccountList.length);
    return selectedBankAccountTexts;
}

function getServiceTypeText() {
    var selectServiceTypeList = [];
    if ($("#ServiceTypeIDs").select2('data').length) {
        $.each($("#ServiceTypeIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectServiceTypeList += "," + item.text;
        });
    }
    var selectedServiceTypeTexts = "";
    if (selectServiceTypeList.length > 0)
        selectedServiceTypeTexts = selectServiceTypeList.substring(1, selectServiceTypeList.length);
    return selectedServiceTypeTexts;
}


var depositTotal = 0;
function InitializeDataTable() {
    var table = $('#tblDepositSummeryByServiceTypeList').dataTable({
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
            "url": ROOTPath + "/Reports/Report/CollectionDepositSummaryByServiceType",
            "type": "POST",
            "data": function (data) {
                data.startPeriod = new Date($("#StartPeriodDate").datepicker('getDate')).toDateString();
                data.endPeriod = new Date($("#EndPeriodDate").datepicker('getDate')).toDateString();
                data.serviceTypeIDs = getServiceTypeIDs();
                data.NetDepositFrom = $("#txtNetDepositFrom").val();
                data.NetDepositTo = $("#txtNetDepositTo").val();
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
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            var row = $("#tblDepositSummeryByServiceTypeList").DataTable().row(iDisplayIndex);
            var data = row.data();
            createChild(iDisplayIndex, data.ClosedEntryList);
        },
        "footerCallback": function (row, data, start, end, display) {
            $(row).find("td").eq("1").text(depositTotal);
            $("#tblFooter").removeClass("hide");
        }
    });    

}
function createChild(idx, d) {
    $.post(ROOTPath + "/Reports/Report/GetClosingServiceTypeHtml", { "closingListJSON": JSON.stringify(d) }, function (response) {
        if (response.status == false) {
            showAlert("error", response.message);
        }
        else {
            $("#tblDepositSummeryByServiceTypeList").DataTable().row(idx).child(response).show();
        }
    });
}

function loadAdvanceSearch() {
    $("#txtNetDepositFrom").val("");
    $("#txtNetDepositTo").val("");
    $("#ServiceTypeIDs").val([]).trigger('change');
    $("#BankAccountIDs").val([]).trigger('change');
    SetResetFilterOption();
    if (!validateDates()) {
        return false;
    }
    else {        
        $("#advanceSearchModal").modal('show');
        $("#txtNetDepositFrom").focus();
    }
}
function validateForm() {
    if ($("#txtNetDepositTo").val() != "" && $("#txtNetDepositFrom").val() == "")
        showAlert('error', balanceFromRequiredMsg);
    else if ($("#txtNetDepositFrom").val() != "" && $("#txtNetDepositTo").val() == "")
        showAlert('error', balanceToRequiredMsg);
    else if (
        ($("#txtNetDepositTo").val() != "" && $("#txtNetDepositFrom").val() != "")
        &&
        (parseInt($("#txtNetDepositFrom").val()) > parseInt($("#txtNetDepositTo").val()))
    )
        showAlert('error', compareDepositValidationMsg);
    else {
        $("#NetDepositFrom").val($("#txtNetDepositFrom").val());
        $("#NetDepositTo").val($("#txtNetDepositTo").val());
        hdnServiceType = getServiceTypeIDs();
        hdnBalanceFrom = $("#txtNetDepositFrom").val();
        hdnBalanceTo = $("#txtNetDepositTo").val();
        hdnBankAccount = getBankAccountIDs();
        refreshData();
        $("#FilterServiceTypeID").val(hdnServiceType);
        $("#FilterBankAccountID").val(hdnBankAccount);
        return true;
    }
    return false;
}
function refreshData() {
    var selectedBankAccountTexts = "";
    var selectedServiceTypeTexts = "";
    InitializeDataTable();
    $("#advanceSearchModal").modal('hide');
    if ($("#txtNetDepositFrom").val() != '' && $("#txtNetDepositTo").val() != '' && parseFloat($("#txtNetDepositFrom").val()) >= 0 && parseFloat($("#txtNetDepositTo").val()) > 0) {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnBalanceRange").removeClass('hide');
        $("#SearchText").text(BalanceRange + " : " + $("#txtNetDepositFrom").val() + " - " + $("#txtNetDepositTo").val());
    }
    else {
        $("#spnBalanceRange").addClass('hide');
        $("#SearchText").text('');
    }
    selectedBankAccountTexts = getBankAccountText();
    if (selectedBankAccountTexts != '') {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnSelectedBankAccount").removeClass('hide');
        $("#BankAccountIdSearchText").text(SelectedBankAccount + " : " + selectedBankAccountTexts);
    }
    else {
        $("#spnSelectedBankAccount").addClass('hide');
        $("#BankAccountIdSearchText").text('');
    }
    selectedServiceTypeTexts = getServiceTypeText();
    if (selectedServiceTypeTexts != '') {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnSelectedServiceType").removeClass('hide');
        $("#ServiceTypeIdSearchText").text(SelectedServiceType + " : " + selectedServiceTypeTexts);
    }
    else {
        $("#spnSelectedServiceType").addClass('hide');
        $("#ServiceTypeIdSearchText").text('');
    }
}
function clearSearch(filterCriteria) {
    if (validateDates()) {
        if (filterCriteria == 'balance') {
            $("#txtNetDepositFrom").val('');
            $("#txtNetDepositTo").val('');
            $("#NetDepositFrom").val('');
            $("#NetDepositTo").val('');
            $("#spnBalanceRange").addClass('hide');
            hdnBalanceFrom = "";
            hdnBalanceTo = "";
        }
        if (filterCriteria == 'bankaccount') {
            $("#BankAccountIDs").val([]).trigger('change');
            $("#spnSelectedBankAccount").addClass('hide');
            hdnBankAccount = "";
        }
        if (filterCriteria == 'servicetype') {
            $("#ServiceTypeIDs").val([]).trigger('change');
            $("#spnSelectedServiceType").addClass('hide');
            hdnServiceType = "";
        }
        InitializeDataTable();
    }
}
function Print() {
    if (validateDates()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintDepositSummaryByServiceType",
            data: { 'startPeriod': $("#StartPeriodDate").val(), 'endPeriod': $("#EndPeriodDate").val(), 'serviceTypeIDs': getServiceTypeIDs(), 'NetDepositFrom': $("#txtNetDepositFrom").val() == '' ? null : $("#txtNetDepositFrom").val(), 'NetDepositTo': $("#txtNetDepositTo").val() == '' ? null : $("#txtNetDepositTo").val(), 'bankAccountIDs': getBankAccountIDs() },
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