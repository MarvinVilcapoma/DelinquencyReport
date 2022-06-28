$(document).ready(function () {
    $('#txtSearch').focus();
    //if ($(".select2dropdown").length)
    $(".select2dropdown").select2();
    InitializeDataTable();
});

$(document).on('click', '#btnSearch', function () {
    $('#DatatableList').DataTable().search($('#txtSearch').val()).draw();
    $("#txtSearch").focus();
});

$("#btnrefresh").click(function () {
    $("#txtSearch").focus();
    $('#txtSearch').val('');
    $("#DocumentTypeID").val("").trigger("change.select2");
    InitializeDataTable();
});

function getDocumentTypeIDs() {
    var selectDocumentTypeList = [];
    if ($("#DocumentTypeID").select2('data').length) {
        $.each($("#DocumentTypeID").select2('data'), function (key, item) {
            if (item.id != 0)
                selectDocumentTypeList += "," + item.id;
        });
    }
    var selectedDocumentTypeIDs = "";
    if (selectDocumentTypeList.length > 0)
        selectedDocumentTypeIDs = selectDocumentTypeList.substring(1, selectDocumentTypeList.length);
    return selectedDocumentTypeIDs;
}

function InitializeDataTable(tableName) {
    $('#DatatableList').dataTable({
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
            "url": ROOTPath + '/PostingProcess/Get',
            "type": "POST",
            "data": function (data) {
                data.filterText = $("#txtSearch").val();
                data.DocumentTypeID = getDocumentTypeIDs();
            }
        },
        destroy: true,
        "columns": [
            {
                name: "SequenceID", title: number, "data": "SequenceID", className: "col-lg-2"
            },
            {
                name: "AsOfDate", title: date, "data": "FormattedDate", className: "col-lg-2"
            },
            {
                name: "DocumentType", title: documenttype, "data": "DocumentType", className: "col-lg-4"
            },
            {
                name: "Status", title: status, "data": "Status", className: "col-lg-2"
            },
            {
                name: "Amount", title: amount, "data": "FormattedAmount", className: "col-lg-2 text-right"
            },
            {
                name: "Actions",
                title: actions,
                className: "col-lg-1 text-right",
                "bSortable": false,
                "data": function (row) {                    
                    var str = "";
                    str = '<a href=' + ROOTPath + "/Collections/PostingProcess/View?ID=" + row.ID + ' class="btn btn-white btn-sm text-right"><i class="fa fa-folder"></i> ' + view + ' </a>';
                    return str;
                }
            }
        ],
        "createdRow": function (row, data, dataIndex) {
        }
    });
}
