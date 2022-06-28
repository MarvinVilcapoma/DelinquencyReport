$(window).on("load", function () {
    $('#txtSearch').focus();
});
$(document).ready(function () {
    InitializeDataTable();   
});

$(document).on('click', '#btnSearch', function () {
    $('#DatatableList').DataTable().search($('#txtSearch').val()).draw();
    $("#txtSearch").focus();
});

$("#btnrefresh").click(function () {
    $("#txtSearch").focus();
    $('#txtSearch').val('');
    InitializeDataTable();
});

function InitializeDataTable(tableName) {
    $('#DatatableList').dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg,
            oPaginate: {
                sPrevious: previous,
                sNext: next
            }
        },
        "fnDrawCallback": function () {
            $('#DatatableList tbody tr td').each(function () {
                if ($(this).index() == 2)
                    this.setAttribute('title', $(this).text());
            });

            /* Apply the tooltips */
            $('#DatatableList tbody tr td').tooltip({
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
            "url": ROOTPath + '/CreditNote/Get',
            "type": "POST",
            "data": function (data) {
                data.filterText = $("#txtSearch").val();
            }
        },
        destroy: true,
        "columns": [
            {
                name: "Date", title: date, "data": "FormattedDate", className: "col-lg-2"
            },
            {
                name: "Number", title: number, "data": "Number", className: "col-lg-3"
            },
            {
                name: "AccountDisplayName", title: accountName, "data": "AccountDisplayName", className: "col-lg-3 table-description-field"
            },
            {
                name: "NetPayment", title: amount, "data": "FormattedAmount", className: "col-lg-2 text-right"
            },
            {
                name: "AvailableNetPayment", title: availableAmount, "data": "FormattedAvailableAmount", className: "col-lg-2 text-right"
            },
            {
                name: "Actions",
                title: actions,
                className: "col-lg-1 text-right",
                "bSortable": false,
                "data": function (row) {     
                    var str = "";
                    str = '<a href=' + ROOTPath + "/Collections/CreditNote/View?ID=" + row.ID + '&IsCH=' + row.IsCreditHistory+' class="btn btn-white btn-sm text-right"><i class="fa fa-folder"></i> ' + view + ' </a>';
                    return str;
                }
            }
        ]        ,
        "createdRow": function (row, data, dataIndex) {
            //if (data.IsVoid === true) {
            //    $(row).children("td").not(":last-child").addClass("text-danger");
            //}
        }
    });
}
