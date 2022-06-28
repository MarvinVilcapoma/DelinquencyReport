$(document).ready(function () {
    
    initializeDatePicker();
    GetAccountForSelect('AccountId', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px');
});

$(document).on('click', '#btnGo', function () {
    if (checkAccountIdInput() && $("#form").valid()) {
        getAccountInfo();
    }
    return false;
});
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
    $('#FutureDate').datepicker("setDate", new Date(dt.getFullYear(), dt.getMonth(), dt.getDate()));
}
function getAccountInfo() {
    var retval = true;
    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/Reports/Report/SalesTaxDebtDetailGet",
        data: { 'accountId': $("#AccountId").val(), 'balanceFrom': $("#txtBalanceFrom").val() == '' ? null : $("#txtBalanceFrom").val(), 'balanceTo': $("#txtBalanceTo").val() == '' ? null : $("#txtBalanceTo").val(), 'futureDate': new Date($("#FutureDate").datepicker('getDate')).toDateString() },
        success: function (response) {
            if (response.status) {
                $("#dvUpdate").html(response.data);
                $("#advanceSearchModal").modal('hide');
                if ($("#txtBalanceFrom").val() != '' && $("#txtBalanceTo").val() != '') {
                    $("#dvsearchinfo").removeClass('hide');
                    $("#SearchText").text(BalanceRange + " : " + $("#txtBalanceFrom").val() + " - " + $("#txtBalanceTo").val());
                }
                showLoading();
                InitializeDataTable("tblIVUStatement");
            }
            else {
                $("#dvUpdate").html('');
                showAlert('error', response.data);
            }
        },
        error: function (error) {
            retval = false;
        }
    });
    if (retval) {
        showLoading();
    }
}
function InitializeDataTable(tableName) {
    $('#' + tableName).dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg,
            oPaginate: {
                sPrevious: previous,
                sNext: next
            }
        },
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
        "scrollCollapse": true,
        "ajax": {
            "url": ROOTPath + "/Reports/Report/SalesTaxDebitStatement",
            "type": "POST",
            "data": function (data) {
                data.futureDate = new Date($("#FutureDate").datepicker('getDate')).toDateString();
                data.accountId = $("#AccountId").val();
                data.balanceFrom = $("#txtBalanceFrom").val() == '' ? null : $("#txtBalanceFrom").val();
                data.balanceTo = $("#txtBalanceTo").val() == '' ? null : $("#txtBalanceTo").val();
            }
        },
        destroy: true,
        "columns": [
            {
                name: "IVUPeriod", title: IVUPeriod, "data": "FormattedIVUPeriod", className: "col-lg-2"
            },
            {
                name: "IVUTaxBalance", title: IVUTaxBalance, "data": "FormattedIVUTaxBalance", className: "col-lg-2 text-right"
            },
            {
                name: "Penalty", title: Penalty, "data": "FormattedPenalties", className: "col-lg-2 text-right"
            },
            {
                name: "Surcharges", title: Surcharges, "data": "FormattedCharges", className: "col-lg-2 text-right"
            },
            {
                name: "Interests", title: Interests, "data": "FormattedInterest", className: "col-lg-2 text-right"
            },
            {
                name: "TotalBalance", title: TotalBalanceResource, "data": "FormattedTotalBalance", className: "col-lg-2 text-right"
            }
        ],
        "lengthMenu": [[15, 25, 50, 100], [15, 25, 50, 100]],
        "order": [[1, "asc"]]
        , "footerCallback": function (row, data, start, end, display) {
            var totalBalance = 0;
            for (var i = 0; i < display.length; i++) {
                totalBalance = totalBalance + data[i].TotalBalance;
            }
            $(row).find("th").eq("1").text(NumberToCultureFormat(parseFloat(totalBalance)));
            $("#tblFooter").removeClass("hide");
            hideLoading();
        }
    });
}
function Print(data) {
    if (checkAccountIdInput()) {
        $.ajax({
            type: "GET",
            async: false,
            url: ROOTPath + "/Reports/Report/PrintIVUStatement",
            data: { 'accountId': $("#AccountId").val(), 'balanceFrom': $("#txtBalanceFrom").val() == '' ? null : $("#txtBalanceFrom").val(), 'balanceTo': $("#txtBalanceTo").val() == '' ? null : $("#txtBalanceTo").val(), 'statementType': data, 'futureDate': new Date($("#FutureDate").datepicker('getDate')).toDateString() },
            success: function (response) {
                if (response.status) {
                    hideLoading();
                    var printWindow = window.open('', '_blank');
                    printWindow.document.write(response.data);
                    printWindow.document.close();
                    setTimeout(function () { printWindow.print(); }, 20);
                    printWindow.onfocus = function () { setTimeout(function () { printWindow.close(); }, 20); };
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

function validateForm() {
    if (!$("#txtBalanceFrom").val()) {
        showAlert('error', $("#txtBalanceFrom").attr("data-val-msg"));
        return false;
    }
    else if (!$("#txtBalanceTo").val()) {
        showAlert('error', $("#txtBalanceTo").attr("data-val-msg"));
        return false;
    }
    else if (parseInt($("#txtBalanceFrom").val()) > parseInt($("#txtBalanceTo").val())) {
        showAlert('error', compareBalanceValidationMsg);
        return false;
    }
    else {
        $("#BalanceFrom").val($("#txtBalanceFrom").val());
        $("#BalanceTo").val($("#txtBalanceTo").val());
        getAccountInfo();
    }
    return true;
}
function loadAdvanceSearch() {
    if (checkAccountIdInput()) {
        $("#advanceSearchModal").modal('show');
    }
}
function checkAccountIdInput() {
    var isvalid = true;
    $("#FutureDate").removeClass("error");
    if ($("#FutureDate").val() == undefined || $("#FutureDate").val() == '') {
        $("#FutureDate").addClass("error");
        isvalid = false;
    }
    if ($("#AccountId").val() == '') {
        $("#AccountId").focus();
        //isvalid;
    }
    return isvalid;
}

function clearSearch() {
    if ($("#AccountId").val() != '') {
        $("#txtBalanceFrom").val('');
        $("#txtBalanceTo").val('');
        getAccountInfo();
        $("#dvsearchinfo").addClass('hide');
    }
}