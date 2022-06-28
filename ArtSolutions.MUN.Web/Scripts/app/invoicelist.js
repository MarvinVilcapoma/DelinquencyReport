$(window).on("load", function () {
    $('#txtSearch').focus();
});

$(document).ready(function () {
    InitializeDataTable();
    $("#txtSearch").focus();
});

$(document).on('click', '#btnSearch', function () {
    $('#tblInvoiceList').DataTable().search($('#txtSearch').val()).draw();
});

$("#btnrefresh").click(function () {
    $("#txtSearch").focus();
    $('#txtSearch').val('');
    InitializeDataTable();
});

function InitializeDataTable(tableName) {
    $('#tblInvoiceList').dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg,
            oPaginate: {
                sPrevious: previous,
                sNext: next
            }
        },
        "fnDrawCallback": function () {
            $('#tblInvoiceList tbody tr td').each(function () {
                if ($(this).index() == 2 || $(this).index() == 3)
                    this.setAttribute('title', $(this).text());
            });

            /* Apply the tooltips */
            $('#tblInvoiceList tbody tr td').tooltip({
                container: "body",
                html: true
            });
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
            "url": ROOTPath + '/Invoice/Get',
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
                name: "Number", title: number, "data": "Number", className: "col-lg-2"
            },
            {
                name: "Reference", title: reference, "data": "Reference", className: "col-lg-2"
            },
            {
                name: "AccountDisplayName", title: accountName, "data": "AccountDisplayName", className: "col-lg-2 table-description-field"
            },
            {
                name: "Status", title: status, "data": "Status", className: "col-lg-1"
            },
            {
                name: "Total", title: amount, "data": "FormattedTotal", className: "col-lg-2 text-right"
            },
            {
                name: "Actions",
                title: actions,
                className: "col-lg-2 text-right",
                "bSortable": false,
                "data": function (row) {
                    var str = "";
                    str = '<a href=' + ROOTPath + "/Collections/Invoice/View?ID=" + row.ID + ' class="btn btn-white btn-sm"><i class="fa fa-folder"></i> ' + view + ' </a>';
                    if ((row.AccountServiceID == null || row.AccountServiceID == 0) && row.IsPost == 1 && row.Payments == 0)
                        str = str + '<a href=' + ROOTPath + "/Collections/Invoice/Edit?ID=" + row.ID + ' class="btn btn-white btn-sm m-l-xs"><i class="fa fa-pencil"></i> ' + edit + ' </a>';
                    return str;
                }
            }
        ],
        "createdRow": function (row, data, dataIndex) {

            if (data.IsVoid === true) {
                $(row).children("td").not(":last-child").addClass("text-danger");
            }
        }
    });
}
