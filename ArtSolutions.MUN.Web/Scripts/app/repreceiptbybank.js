$(document).ready(function () {
    
    $('#dvUpdate').hide();
    $(".select2dropdown").not('#AccountIDs').select2({ width: '300px' });
    GetAccountForSelect('AccountIDs', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px', true);
    initializeDatePicker();
    $("#btnCancel").on("click", function () {
        SetResetFilterOption();

    });
});
var hdnAccount;
var hdnBankAccount;
var hdnBalanceFrom;
var hdnBalanceTo;
function SetResetFilterOption() {
    var PreviousSelectedData = [];
    if (hdnAccount != undefined && hdnAccount.length > 0) {
        PreviousSelectedData = hdnAccount.split(",");        
        $('#AccountIDs').val(PreviousSelectedData).trigger('change');
    }
    if (hdnBankAccount != undefined && hdnBankAccount.length > 0) {
        PreviousSelectedData = hdnBankAccount.split(",");
        $('#BankAccountIDs').val(PreviousSelectedData).trigger('change');
    }
    if (hdnBalanceFrom != undefined && hdnBalanceFrom.length > 0) {
        $("#txtBalanceFrom").val(hdnBalanceFrom);
        $("#txtBalanceTo").val(hdnBalanceTo);
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
    var dt = new Date();
    $('#StartDate').datepicker("setDate", new Date(dt.getFullYear(), dt.getMonth(), dt.getDate() - 1));
    $('#EndDate').datepicker('setDate', new Date(dt.getFullYear(), dt.getMonth(), dt.getDate()));
}

$(document).on('click', '#btnGo', function () {
    if (validateForm()) {
        InitializeDataTable("tblDatatable");
        return true;
    }
    return false;
});

$(document).on('change', '#AccountIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#AccountIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

$(document).on('change', '#BankAccountIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#BankAccountIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

function clearSearch(filterCriteria) {
    if (filterCriteria == 'balance') {
        $("#txtBalanceFrom").val('');
        $("#txtBalanceTo").val('');
        $("#spnBalanceRange").addClass('hide');
        hdnBalanceFrom = "";
        hdnBalanceTo = "";
    }
    if (filterCriteria == 'accounts') {
        $("#AccountIDs").val(null).trigger('change');
        $("#spnSelectedAccount").addClass('hide');
        hdnAccount = "";
    }
    if (filterCriteria == 'bankaccount') {
        $("#BankAccountIDs").val(null).trigger('change');
        $("#spnSelectedBankAccount").addClass('hide');
        hdnBankAccount = "";
    }
    InitializeDataTable("tblDatatable");
}

function validateForm() {
    var isvalid = true;
    $("#StartDate,#EndDate").removeClass("error");
    if ($("#StartDate").val() == undefined || $("#StartDate").val() == '') {
        showAlert('error', $("#StartDate").attr("data-required-msg"));
        $("#StartDate").addClass("error");
        isvalid = false;
    }
    if ($("#EndDate").val() == undefined || $("#EndDate").val() == '') {
        showAlert('error', $("#EndDate").attr("data-required-msg"));
        $("#EndDate").addClass("error");
        isvalid = false;
    }
    if (new Date($("#EndDate").datepicker('getDate')) < new Date($("#StartDate").datepicker('getDate'))) {
        showAlert('error', $("#EndDate").attr("data-compare-val-msg"));
        isvalid = false;
    }
    if (isvalid) {
        hdnAccount = getAccountIDs();
        hdnBankAccount = getBankAccountIDs();
        hdnBalanceFrom = $("#txtBalanceFrom").val();
        hdnBalanceTo = $("#txtBalanceTo").val();
        $("#FilterAccountID").val(hdnAccount);
        $("#FilterBankAccountID").val(hdnBankAccount);
        $("#BalanceFrom").val(hdnBalanceFrom);
        $("#BalanceTo").val(hdnBalanceTo);
    }
    return isvalid;
}

function validateAdvanceSearch() {
    var selectedAccountTexts = "";
    var selectedBankAccountTexts = "";
    var isvalid = false;
    if ($("#txtBalanceTo").val() != "" && $("#txtBalanceFrom").val() == "")
        showAlert('error', balanceFromRequiredMsg);
    else if ($("#txtBalanceFrom").val() != "" && $("#txtBalanceTo").val() == "")
        showAlert('error', balanceToRequiredMsg);
    else if (
        ($("#txtBalanceTo").val() != "" && $("#txtBalanceFrom").val() != "")
        &&
        (parseInt($("#txtBalanceFrom").val()) > parseInt($("#txtBalanceTo").val()))
    )
        showAlert('error', compareBalanceValidationMsg);
    else {
        isvalid = true;
        InitializeDataTable("tblDatatable");
        $("#advanceSearchModal").modal('hide');

        if ($("#txtBalanceFrom").val() != '' && $("#txtBalanceTo").val() != '') {
            $("#dvsearchinfo").removeClass('hide');
            $("#spnBalanceRange").removeClass('hide');
            $("#SearchText").text(BalanceRange + " : " + $("#txtBalanceFrom").val() + " - " + $("#txtBalanceTo").val());
        }
        else {
            $("#spnBalanceRange").addClass('hide');
        }
        selectedAccountTexts = getAccountText();
        if (selectedAccountTexts != '') {
            $("#dvsearchinfo").removeClass('hide');
            $("#spnSelectedAccount").removeClass('hide');
            $("#AccountIdSearchText").text(SelectedAccount + " : " + selectedAccountTexts);
        }
        else {
            $("#spnSelectedAccount").addClass('hide');
            $("#AccountIdSearchText").text('');
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
    if (isvalid) {
        hdnAccount = getAccountIDs();
        hdnBankAccount = getBankAccountIDs();
        hdnBalanceFrom = $("#txtBalanceFrom").val();
        hdnBalanceTo = $("#txtBalanceTo").val();
    }
    return isvalid;
}

function loadAdvanceSearch() {
    if (validateForm()) {
        $("#txtBalanceFrom").val('')
        $("#txtBalanceTo").val('')
        $("#AccountIDs").val(null).trigger('change');
        $("#BankAccountIDs").val(null).trigger('change');
        SetResetFilterOption();
        $("#advanceSearchModal").modal('show');
        $('#BankAccountIDs').select2('open').select2('close');
    }
}

function getAccountIDs() {
    var selectAccountList = [];
    if ($("#AccountIDs").select2('data').length) {
        $.each($("#AccountIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectAccountList += "," + item.id;
        });
    }
    var selectedAccountIDs = "";
    if (selectAccountList.length > 0)
        selectedAccountIDs = selectAccountList.substring(1, selectAccountList.length);
    return selectedAccountIDs;
}

function getAccountText() {
    var selectAccountList = [];
    if ($("#AccountIDs").select2('data').length) {
        $.each($("#AccountIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectAccountList += "," + item.text;
        });
    }
    var selectedAccountTexts = "";
    if (selectAccountList.length > 0)
        selectedAccountTexts = selectAccountList.substring(1, selectAccountList.length);
    return selectedAccountTexts;
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

function InitializeDataTable(tableName) {
    var totalAmountAuxiliary = 0;
    var totalAmountOfficial = 0;

    $('#dvUpdate').show();

    var table = $('#' + tableName).dataTable({
        "language": {
            "emptyTable": nodatamsg,
            "zeroRecords": nodatamsg,
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
            "url": ROOTPath + "/Reports/Report/ReceiptByBank",
            "type": "POST",
            "data": function (data) {
                data.startDate = new Date($("#StartDate").datepicker('getDate')).toDateString();
                data.endDate = new Date($("#EndDate").datepicker('getDate')).toDateString();
                data.balanceFrom = $("#txtBalanceFrom").val() ? parseFloat($("#txtBalanceFrom").val()) : null;
                data.balanceTo = $("#txtBalanceTo").val() ? parseFloat($("#txtBalanceTo").val()) : null;
                data.accountIDs = getAccountIDs();
                data.bankAccountIDs = getBankAccountIDs();
            }
            , "dataSrc": function (json) {
                depositTotal = json.recordsFiltered;
                return json.data;
            }
        },
        destroy: true,
        "columns": [

            {
                name: "BankName", title: bank, "data": "BankName", className: "col-lg-3"
            },
            {
                name: "BankAccount", title: bankAccount, "data": "BankAccount", className: "col-lg-9"
            }
        ],
        "lengthMenu": [[15, 25, 50, 100], [15, 25, 50, 100]],
        "order": [[0, "asc"]],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            var row = $("#tblDatatable").DataTable().row(iDisplayIndex);
            var data = row.data();
            createChild(iDisplayIndex, data.BankAccountList);
        }
    });
}

function createChild(idx, d) {
    $.post(ROOTPath + "/Reports/Report/GetReceiptByBankHtml", { "DataListJSON": JSON.stringify(d) }, function (response) {
        if (response.status == false) {
            showAlert("error", response.message);
        }
        else {
            $("#tblDatatable").DataTable().row(idx).child(response).show();
        }
    });
}

function Print() {
    if (validateForm()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintReceiptByBank",
            data: {
                'startDate': new Date($("#StartDate").datepicker('getDate')).toDateString()
                , 'endDate': new Date($("#EndDate").datepicker('getDate')).toDateString()
                , 'balanceFrom': $("#txtBalanceFrom").val() ? parseFloat($("#txtBalanceFrom").val()) : null
                , 'balanceTo': $("#txtBalanceTo").val() ? parseFloat($("#txtBalanceTo").val()) : null
                , 'accountIDs': getAccountIDs()
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
                }
                else {
                    showAlert('error', response.data);
                }
            }
        });
    }
    else
        return false;
    return true;
}