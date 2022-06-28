$(document).ready(function () {
    $('#dvUpdate').hide();
    InitializeDataTable("tblRoutaMissingServices");
});

$(window).resize(function () {
    InitializeDataTable("tblRoutaMissingServices");
});

function InitializeDataTable(tableName) {
    $('#dvUpdate').show();
    $('#' + tableName).dataTable({
        "language": {
            "emptyTable": nodatamsg,
            "zeroRecords": nodatamsg,
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
        "conditionalPaging": true,
        "scrollY": "100vh",
        "scrollX": true,
        "scrollCollapse": true,
        "ajax": {
            "url": ROOTPath + "/Reports/Report/RoutaMissingServices",
            "type": "POST"
        },
        destroy: true,
        "columns": [
            {
                name: "Meter", title: meter, "data": "Meter", className: "col-lg-2 text-center"
            },
            {
                name: "TaxNumber", title: taxNumber, "data": "TaxNumber", className: "col-lg-2 text-center"
            },
            {
                name: "DisplayName", title: name, "data": "DisplayName", className: "col-lg-3 text-center text-description-field"
            },
            {
                name: "FiscalYearID", title: year, "data": "FiscalYearID", className: "col-lg-2 text-center"
            },
            {
                name: "ServiceName", title: accountService, "data": "ServiceName", className: "col-lg-3 text-center text-description-field"
            }
        ],
        "fnRowCallback": function (nRow, aData) {

            if (aData.Meter != null) {
                $('td:eq(0)', nRow).html('<a id="viewAccountService" target="_blank" href=' + ROOTPath + "/AccountService/View?accountServiceID=" + aData.ID + ' class="text-underline">' + aData.Meter + '</a>');
            }
            else {
                $('td:eq(0)', nRow).html('-');
            }                        
            $('td:eq(4)', nRow).html('<a id="viewAccountService" target="_blank" href=' + ROOTPath + "/AccountService/View?accountServiceID=" + aData.ID + ' class="text-underline">' + aData.ServiceName + '</a>');
        },
        "lengthMenu": [[15, 25, 50, 100], [15, 25, 50, 100]],
        "order": [[1, "asc"]]
    });
}

function Print() {
    $.ajax({
        type: "GET",
        url: ROOTPath + "/Reports/Report/PrintRoutaMissingServices",
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