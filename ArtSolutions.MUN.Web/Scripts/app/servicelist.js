$(document).ready(function () {
    $("#txtSearch").focus();
    $(".select2dropdown").select2({ width: '100%' });
    InitializeDataTable();
});

$(document).on('click', '#btnSearch', function () {
    $('#tblService').DataTable().search($('#txtSearch').val()).draw();
});

$("#btnrefresh").click(function () {
    $("#txtSearch").focus();
    $('#txtSearch').val('');    
    $('#ddlGroupID').val(null).trigger('change');    
    $('#ddlServiceType').val(null).trigger('change');
    InitializeDataTable();
});

$(document).on('change', '#ddlGroupID', function () {
    $("#ddlServiceType").empty();
    $("#ddlServiceType").append("<option value=''>" + ALL + "</option>");
    $.ajax({
        url: ROOTPath + "/Service/GetServiceTypesByGroup?groupId=" + $('#ddlGroupID').val(),
        success: function (data) {
            $.each(data, function (index, optiondata) {
                $("#ddlServiceType").append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
            });           
            $('#ddlServiceType').val(null).trigger('change');
        }
    });  
});

$(document).on('change', '#ddlServiceType', function () {
    InitializeDataTable();
});

function InitializeDataTable(tableName) {
    $('#tblService').dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg,
            oPaginate: {
                sPrevious: previous,
                sNext: next
            }
        },
        "fnDrawCallback": function () {
            $('#tblService tbody tr td').each(function () {
                if ($(this).index() == 3)
                    this.setAttribute('title', $(this).text());
            });

            /* Apply the tooltips */
            $('#tblService tbody tr td').tooltip({
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
        "order": [[0, "asc"]],
        "conditionalPaging": true,
        "ajax": {
            "url": ROOTPath + '/Service/Get',
            "type": "POST",
            "data": function (data) {
                data.filterText = $("#txtSearch").val();
                data.GroupID = $("#ddlGroupID").val();
                data.ServiceTypeID = $("#ddlServiceType").val();
            }
        },
        destroy: true,
        "columns": [
            {
                name: "Group", title: group, "data": "Group", width:"12%"
            },
            {
                name: "Type", title: type, "data": "Type", width: "14%"
            },
            {
                name: "Code", title: serviceCode, "data": "Code", width: "12%"
            },
            {
                name: "Service", title: service, "data": "Service", className: "table-description-field", width: "21%"
            },
            {
                name: "EffectiveFromByCulture", title: effectiveFrom, "data": "EffectiveFromByCulture", width: "12%"
            },
            {
                name: "EffectiveToByCulture", title: effectiveTo, "data": "EffectiveToByCulture", width: "12%"
            },
            {
                name: "Actions",
                title: actions,
                className: "text-right", width: "15%",
                "bSortable": false,
                "data": function (row) {
                    var str = "";
                    str = '<a href=' + ROOTPath + "/Service/View?serviceId=" + row.ID + '  class="btn btn-white btn-sm m-b-5"><i class="fa fa-folder"></i> ' + view + ' </a>';
                    
                    //if (!row.IsLocked && !row.IsPublic) {
                    //    str = str + '<a href=' + ROOTPath + "/Service/Edit?serviceID=" + row.ID + '  class="btn btn-white btn-sm m-l-xs"><i class="fa fa-pencil"></i> ' + edit + ' </a>';
                    //}
                    if (!row.IsLocked) {
                        str = str + '<a href=' + ROOTPath + "/Service/Edit?serviceID=" + row.ID + '  class="btn btn-white btn-sm m-l-xs"><i class="fa fa-pencil"></i> ' + edit + ' </a>';
                    }
                    return str;
                }
            }
        ]
    });
}