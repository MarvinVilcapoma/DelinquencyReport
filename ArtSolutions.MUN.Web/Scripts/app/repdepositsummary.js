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
        language: __culture,
    });
    $('.periodDate').datepicker('update', new Date());
    $(".select2dropdown").select2({ width: '100%' });
    InitializeDataTable();

    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });

});
var hdnBalanceFrom;
var hdnBalanceTo;
var hdnBankAccount;
function SetResetFilterOption() {
    if (hdnBankAccount != undefined && hdnBankAccount.length > 0) {
        PreviousSelectedData = hdnBankAccount.split(",");        
        $('#BankAccountIDs').val(PreviousSelectedData).trigger('change');
    }
    if (hdnBalanceFrom != undefined && hdnBalanceTo > 0 && hdnBalanceFrom.length > 0) {
        $("#txtNetDepositFrom").val(hdnBalanceFrom)
        $("#txtNetDepositTo").val(hdnBalanceTo)
    }
}
$(document).on('click', '#btnGo', function () {
    if (validateDates()) {
        InitializeDataTable();
        return true;
    }
    return false;
});

$(document).on('change', '#BankAccountIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#BankAccountIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

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
        hdnBalanceFrom = $("#txtNetDepositFrom").val();
        hdnBalanceTo = $("#txtNetDepositTo").val();
        $("#FilterBankAccountID").val(hdnBankAccount)
        $("#NetClosingFrom").val(hdnBalanceFrom)
        $("#NetClosingTo").val(hdnBalanceTo)
    }
    return isvalid;
}

var depositTotal = 0;
function InitializeDataTable() {
    var table = $('#tblDepositSummeryList').dataTable({
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
            "url": ROOTPath + "/Reports/Report/CollectionDepositSummary",
            "type": "POST",
            "data": function (data) {
                data.startPeriod = new Date($("#StartPeriodDate").datepicker('getDate')).toDateString();
                data.endPeriod = new Date($("#EndPeriodDate").datepicker('getDate')).toDateString();
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
                name: "BankAccount", title: bankAccount, "data": "BankAccountName", className: "col-lg-2 text-left"
            },
            {
                name: "Description", title: description, "data": "Description", className: "col-lg-2 text-left table-description-field"
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
            var row = $("#tblDepositSummeryList").DataTable().row(iDisplayIndex);
            var data = row.data();
            createChild(iDisplayIndex, data.ClosedEntryList);
        },
        "footerCallback": function (row, data, start, end, display) {
            $(row).find("th").eq("1").text(depositTotal);
            $("#tblFooter").removeClass("hide");
        }
    });

}
function createChild(idx, d) {
    $.post(ROOTPath + "/Reports/Report/GetClosingHtml", { "closingListJSON": JSON.stringify(d) }, function (response) {
        if (response.status == false) {
            showAlert("error", response.message);
        }
        else {
            $("#tblDepositSummeryList").DataTable().row(idx).child(response).show();
        }
    });
}

function loadAdvanceSearch() {    
    $('#BankAccountIDs').val(null).trigger('change');
    $("#txtNetDepositFrom").val("");
    $("#txtNetDepositTo").val("");
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
        showAlert('error', compareBalanceValidationMsg);
    else {
        $("#NetDepositFrom").val($("#txtNetDepositFrom").val());
        $("#NetDepositTo").val($("#txtNetDepositTo").val());
        hdnBalanceFrom = $("#txtNetDepositFrom").val();
        hdnBalanceTo = $("#txtNetDepositTo").val();
        hdnBankAccount = getBankAccountIDs();
        refreshData();
        $("#FilterBankAccountID").val(hdnBankAccount);
        return true;
    }
    return false;
}
function refreshData() {
    var selectedBankAccountTexts = "";
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
            $('#BankAccountIDs').val(null).trigger('change');
            $("#spnSelectedBankAccount").addClass('hide');
            hdnBankAccount = "";
        }
        InitializeDataTable();
    }
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

function Print() {
    if (validateDates()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintDepositSummary",
            data: {
                'startPeriod': $("#StartPeriodDate").val()
                , 'endPeriod': $("#EndPeriodDate").val()
                , 'NetDepositFrom': $("#txtNetDepositFrom").val() == '' ? null : $("#txtNetDepositFrom").val()
                , 'NetDepositTo': $("#txtNetDepositTo").val() == '' ? null : $("#txtNetDepositTo").val()
                , 'bankAccountIDs': getBankAccountIDs()
            },
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