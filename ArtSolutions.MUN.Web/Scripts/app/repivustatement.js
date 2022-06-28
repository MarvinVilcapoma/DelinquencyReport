$(document).ready(function () {
      
    GetAccountForSelect('AccountId', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px');

    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });

});
var hdnBalanceFrom;
var hdnBalanceTo;
function SetResetFilterOption() {
    if (hdnBalanceFrom != undefined && hdnBalanceFrom.length > 0) {
        $("#txtBalanceFrom").val(hdnBalanceFrom);
        $("#txtBalanceTo").val(hdnBalanceTo);
    }
}
$(document).on('click', '#btnGo', function () {
    if ($("#form").valid()) {
        getAccountInfo();
    }
    return false;
});
function getAccountInfo() {
    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/Reports/Report/AccountDetailGet",
        data: { 'accountId': $("#AccountId").val(), 'balanceFrom': $("#txtBalanceFrom").val() == '' ? null : $("#txtBalanceFrom").val(), 'balanceTo': $("#txtBalanceTo").val() == '' ? null : $("#txtBalanceTo").val() },
        success: function (response) {
            if (response.status) {
                $("#dvUpdate").html(response.data);
                $("#advanceSearchModal").modal('hide');
                if ($("#txtBalanceFrom").val() != '' && $("#txtBalanceTo").val() != '') {
                    $("#dvsearchinfo").removeClass('hide');
                    $("#SearchText").text(BalanceRange + " : " + $("#txtBalanceFrom").val() + " - " + $("#txtBalanceTo").val());
                }
                else {
                    $("#dvsearchinfo").addClass('hide');
                }
                InitializeDataTable("tblIVUStatement");
            }
            else {
                $("#dvUpdate").html('');
                showAlert('error', response.data);
            }
        }
    });
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
            "url": ROOTPath + "/Reports/Report/IVUStatement",
            "type": "POST",
            "data": function (data) {
                data.accountId = $("#AccountId").val();
                data.balanceFrom = $("#txtBalanceFrom").val() == '' ? null : $("#txtBalanceFrom").val();
                data.balanceTo = $("#txtBalanceTo").val() == '' ? null : $("#txtBalanceTo").val();
            }
        },
        destroy: true,
        "columns": [
            {
                name: "IVUPeriod", title: IVUPeriod, "data": "FormattedIVUPeriod", className: "col-lg-1"
            },
            {
                name: "IVUTaxBalance", title: IVUTaxBalance, "data": "FormattedIVUTaxBalance", className: "col-lg-1 text-right"
            },
            {
                name: "Penalty", title: Penalty, "data": "FormattedPenalties", className: "col-lg-2 text-right"
            },
            {
                name: "Interests", title: Interests, "data": "FormattedInterest", className: "col-lg-2 text-right"
            },
            {
                name: "Surcharges", title: Surcharges, "data": "FormattedCharges", className: "col-lg-2 text-right"
            },
            {
                name: "Payments", title: Payments, "data": "FormattedPayments", className: "col-lg-2 text-right"
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
        }
    });
}
function Print(data) {
    if (checkAccountIdInput()) {
        $.ajax({
            type: "GET",
            async: false,
            url: ROOTPath + "/Reports/Report/PrintIVUStatement",
            data: { 'accountId': $("#AccountId").val(), 'balanceFrom': $("#txtBalanceFrom").val() == '' ? null : $("#txtBalanceFrom").val(), 'balanceTo': $("#txtBalanceTo").val() == '' ? null : $("#txtBalanceTo").val(), 'statementType': data },
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
        hdnBalanceFrom = $("#txtBalanceFrom").val();
        hdnBalanceTo = $("#txtBalanceTo").val();
        $("#BalanceFrom").val(hdnBalanceFrom);
        $("#BalanceTo").val(hdnBalanceTo);
        getAccountInfo();
        return true;
    }
    return false;
}
function loadAdvanceSearch() {

    if ($("#AccountId").val() == '') {
        showAlert('error', $("#AccountId").attr("data-val-msg"));
        $("#AccountId").focus();
        return false;
    }
    else {
        $("#txtBalanceFrom").val('');
        $("#txtBalanceTo").val('');
        SetResetFilterOption();
        $("#advanceSearchModal").modal('show');
    }
}
function checkAccountIdInput() {
    if ($("#AccountId").val() == '') {
        showAlert('error', $("#AccountId").attr("data-val-msg"));
        $("#AccountId").focus();
        return false;
    }
    else
        return true;
}

function clearSearch() {
    if ($("#AccountId").val() != '') {
        $("#txtBalanceFrom").val('');
        $("#txtBalanceTo").val('');
        getAccountInfo();
        $("#dvsearchinfo").addClass('hide');
        hdnBalanceFrom = "";
        hdnBalanceTo = "";
    }
}