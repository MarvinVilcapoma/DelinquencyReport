$(window).on("load", function () {
    $('#txtSearch').focus();  
});
$(document).ready(function () {  
    InitializeDataTable();   
});

$(document).on('click', '#btnSearch', function () {   
    $('#tblServiceTransferList').DataTable().search($('#txtSearch').val()).draw();
    $("#txtSearch").focus();
});

$("#btnrefresh").click(function () {  
    $("#txtSearch").focus();
    $('#txtSearch').val('');
    InitializeDataTable();
});


function InitializeDataTable(tableName) {
    $('#tblServiceTransferList').dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg,
            oPaginate: {
                sPrevious: previous,
                sNext: next
            }
        },
        "fnDrawCallback": function (oSettings) {
            $('#tblServiceTransferList tbody tr td').each(function () {
                if ($(this).index() == 1 || $(this).index() == 3 || $(this).index() == 4)
                    this.setAttribute('title', $(this).text());
            });

            /* Apply the tooltips */
            $('#tblServiceTransferList tbody tr td').tooltip({
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
            "url": ROOTPath + '/PropertyTransfer/Get',
            "type": "POST",
            "data": function (data) {                     
                data.filterText = $("#txtSearch").val();
            }
        },
        destroy: true,
        "columns": [
            {
                name: "TransferDate", title: date, "data": "FormattedTransferDate", className: "col-lg-1"
            },            
            {
                name: "OldAccountName", title: fromaccountName, "data": "OldAccountName", className: "col-lg-2 table-description-field"
            },
            {
                name: "TransferType", title: transferType, "data": "TransferType", className: "col-lg-2"
            },    
            {
                name: "Property", title: property, "data": "Property", className: "col-lg-2"
            },            
            {
                name: "NewAccountName", title: toaccountName, "data": "NewAccountName", className: "col-lg-2 table-description-field"
            }, 
            {
                name: "Status", title: status, "data": "Status", className: "col-lg-2 table-description-field"
            }, 
            {
                name: "Actions",
                title: actions,
                className: "col-lg-1 text-right",
                "bSortable": false,
                "data": function (row) {
                    if (row.TransferTypeID == 3)
                    {
                        if(row.workflowStatusID == 43)
                            str = '<a href=' + ROOTPath + "/Collections/PropertyTransfer/ViewSplit?ID=" + row.TransferID + ' class="btn btn-white btn-sm text-right"><i class="fa fa-folder"></i> ' + view + ' </a>';
                        else
                            str = '<a href=' + ROOTPath + "/Collections/PropertyTransfer/EditSplit?ID=" + row.TransferID + ' class="btn btn-white btn-sm text-right"><i class="fa fa-folder"></i> ' + view + ' </a>';
                    }
                    else if (row.TransferTypeID == 4)
                            str = '<a href=' + ROOTPath + "/Collections/PropertyTransfer/ViewMerge?ID=" + row.TransferID + ' class="btn btn-white btn-sm text-right"><i class="fa fa-folder"></i> ' + view + ' </a>';
                    else  
                        str = '<a href=' + ROOTPath + "/Collections/PropertyTransfer/View?ID=" + row.TransferID + ' class="btn btn-white btn-sm text-right"><i class="fa fa-folder"></i> ' + view + ' </a>';
                    return str;
                }
            }
        ]
    });
}