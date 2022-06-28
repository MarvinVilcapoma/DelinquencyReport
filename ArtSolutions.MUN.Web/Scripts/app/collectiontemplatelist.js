$(window).on("load", function () {
    $('#txtSearch').focus();
});
$(document).ready(function () {
    InitializeDataTable();
});

$(document).on('click', '#btnSearch', function () {
    $('#tblTemplateList').DataTable().search($('#txtSearch').val()).draw();
    $("#txtSearch").focus();
});

$("#btnrefresh").click(function () {
    $("#txtSearch").focus();
    $('#txtSearch').val('');
    InitializeDataTable();
});

function InitializeDataTable(tableName) {
    $('#tblTemplateList').dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg,
            oPaginate: {
                sPrevious: previous,
                sNext: next
            }
        },
        "fnDrawCallback": function () {
            $('#tblTemplateList tbody tr td').each(function () {
                if ($(this).index() == 0 || $(this).index() == 1)
                    this.setAttribute('title', $(this).text());
            });

            /* Apply the tooltips */
            $('#tblTemplateList tbody tr td').tooltip({
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
        "order": [[1, "asc"]],
        "conditionalPaging": true,
        "ajax": {
            "url": ROOTPath + '/CollectionTemplate/Get',
            "type": "POST",
            "data": function (data) {
                data.filterText = $("#txtSearch").val();
            }
        },
        destroy: true,
        "columns": [
            {
                name: "Name", title: name, "data": "Name", className: "col-lg-3"
            },
            {
                name: "Description", title: description, "data": "Description", className: "col-lg-7 table-description-field"
            },
            {
                name: "Actions",
                title: actions,
                className: "col-lg-2 text-right",
                "bSortable": false,
                "data": function (row) {
                    var str = "";
                    str = '<a href=' + ROOTPath + "/Services/CollectionTemplate/View/" + row.ID + ' class="btn btn-white btn-sm text-right"><i class="fa fa-folder"></i> ' + view + ' </a>';
                    if (row.IsPublic!=true)
                        str = str + '<a href=' + ROOTPath + "/Services/CollectionTemplate/Edit/" + row.ID + '  class="btn btn-white btn-sm m-l-xs"><i class="fa fa-pencil"></i> ' + edit + ' </a>';
                    return str;
                }
            }
        ]
    });
}
