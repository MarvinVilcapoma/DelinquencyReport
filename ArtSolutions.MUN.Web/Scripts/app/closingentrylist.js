$(window).on("load", function () {
    $('#txtSearch').focus();
});
$(document).ready(function () {
    InitializeDataTable();
});

$(document).on('click', '#btnSearch', function () {
    $('#tblclosingentryList').DataTable().search($('#txtSearch').val()).draw();
    $("#txtSearch").focus();
});

$("#btnrefresh").click(function () {
    $("#txtSearch").focus();
    $('#txtSearch').val('');
    InitializeDataTable();
});

function InitializeDataTable(tableName) {
    $('#tblclosingentryList').dataTable({
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
        "ordering": true,
        "order": [[0, "desc"]],
        "conditionalPaging": true,
        "ajax": {
            "url": ROOTPath + '/ClosingEntry/Get',
            "type": "POST",
            "data": function (data) {
                data.filterText = $("#txtSearch").val();
            }
        },
        destroy: true,
        "columns": [
            {
                name: "Date", title: date, "data": "FormattedDate", className: "col-lg-1"
            },
            {
                name: "Number", title: number, "data": "Number", className: "col-lg-1"
            },
            {
                name: "Description", title: description, "data": "Description", className: "col-lg-2 table-description-field"
            },
            {
                name: "CashierName", title: cashier, "data": "CashierName", className: "col-lg-2 table-description-field"
            },
            {
                name: "PaymentReceiptCount", title: receipts, "data": "PaymentReceiptCount", className: "col-lg-1 text-right"
            },            
            {
                name: "IsDeposited", title: deposited, "data": "DepositedText", className: "col-lg-1 text-left"
            },            
            {
                name: "NetClosing", title: amount, "data": "FormattedNetClosing", className: "col-lg-2 text-right"
            },
            {
                name: "Actions",
                title: actions,
                className: "col-lg-1 text-right",
                "bSortable": false,
                "data": function (row) {
                    var str = "";
                    str = '<a href=' + ROOTPath + "/Collections/ClosingEntry/View?ID=" + row.ID + ' class="btn btn-white btn-sm text-right"><i class="fa fa-folder"></i> ' + view + ' </a>';
                    return str;
                }
            }
        ],
        "createdRow": function (row, data, dataIndex) {
            //if (data.IsVoid === true) {
            //    $(row).children("td").not(":last-child").addClass("text-danger");
            //}
        }
    });
}
