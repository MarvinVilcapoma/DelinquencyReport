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
        language: __culture,
    });
    var dt = new Date();
    $('#FutureDate').datepicker("setDate", new Date(dt.getFullYear(), dt.getMonth(), dt.getDate()));
}

function getAccountInfo() {
    var retval = true;
    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/Reports/Report/BusinessLicenseDebtDetailGet",
        data: { 'accountId': $("#AccountId").val(), 'futureDate': new Date($("#FutureDate").datepicker('getDate')).toDateString() },
        success: function (response) {
            if (response.status) {
                $("#dvUpdate").html(response.data);
                $("#advanceSearchModal").modal('hide');
                showLoading();
                InitializeDataTable("tblPatentStatement");
            }
            else {
                retval = false;
                $("#dvUpdate").html('');
                showAlert('error', response.data);
            }
            hideLoading();
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
            "url": ROOTPath + "/Reports/Report/BusinessLicenseDebitStatement",
            "type": "POST",
            "data": function (data) {
                data.futureDate = new Date($("#FutureDate").datepicker('getDate')).toDateString();
                data.accountId = $("#AccountId").val();
            }
        },
        destroy: true,
        "columns": [
            {
                name: "IVUPeriod", title: PatentPeriod, "data": "FormattedFiscalYearID", className: "col-lg-2"
            },
            {
                name: "Principal1", title: FirstSemesterbalance, "data": "FormattedPrincipal1", className: "col-lg-2 text-right"
            },
            {
                name: "Principal2", title: SecondSemesterBalance, "data": "FormattedPrincipal2", className: "col-lg-2 text-right"
            },
            {
                name: "Penalty", title: Penalty, "data": "FormattedPenalties", className: "col-lg-2 text-right"
            },
            {
                name: "Surcharges", title: Surcharges, "data": "FormattedCharges", className: "col-lg-1 text-right"
            },
            {
                name: "Interests", title: Interests, "data": "FormattedInterest", className: "col-lg-1 text-right"
            },
            {
                name: "TotalBalance", title: TotalBalanceResource, "data": "FormattedBalanceamount", className: "col-lg-2 text-right"
            }
        ],
        "lengthMenu": [[15, 25, 50, 100], [15, 25, 50, 100]],
        "order": [[1, "asc"]]
        , "footerCallback": function (row, data, start, end, display) {
            var totalBalance = 0;
            for (var i = 0; i < display.length; i++) {
                totalBalance = totalBalance + data[i].Balance;
            }

            $(row).find("th").eq("1").text(NumberToCultureFormat(parseFloat(totalBalance)));
            $("#tblFooter").removeClass("hide");

            hideLoading();
        }
    });
}

function Print() {
    if (checkAccountIdInput()) {
        $.ajax({
            type: "GET",
            async: false,
            url: ROOTPath + "/Reports/Report/PrintBusinessLicenseDebtDetail",
            data: { 'accountId': $("#AccountId").val(), 'futureDate': new Date($("#FutureDate").datepicker('getDate')).toDateString() },
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
        //isvalid
    }
    return isvalid;
}
