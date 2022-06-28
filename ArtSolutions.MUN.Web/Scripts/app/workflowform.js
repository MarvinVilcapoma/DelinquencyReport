$(document).ready(function () {
    $("#btnrefresh").click(function () {
        $("#txtSearch").val('');
        WorkflowformGet();
    });
    $("#btnSearch").click(function () {
        WorkflowformGet();
    });
    $('#txtSearch').bind("keypress", function (e) {
        if (e.keyCode == 13) {
            WorkflowformGet();
            return false;
        }
    });
    WorkflowformGet();
});

function WorkflowformGet() {
    var model = {
        FilterText: $("#txtSearch").val()
    };
    $('#tblFroms').dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg,
            oPaginate: {
                "sFirst": first,
                "sLast": last,
                "sNext": next,
                "sPrevious": previous
            }
        },
        "serverSide": true,
        "processing": false,
        "bFilter": false,
        "bInfo": false,
        "bLengthChange": false,
        "autoWidth": false,
        "ajax": {
            "url": ROOTPath + "/Workflows/WorkflowForm/List",
            "type": "POST",
            "data": function (data) {
                data.model = model;
                return data;
            }
        },
        "destroy": true,
        "columns": [
            {
                name: "Name", title: name, "data": "Name", className: "col-lg-3"
            },
            {
                name: "UsePopup", title: usePopup, "data": function (data, type) {
                    var checked = "";
                    if (data.UsePopup) {
                        checked = "checked";
                    }
                    return "<input type=\"checkbox\" class=\"i-checks\" " + checked + " />";
                }, className: "col-lg-2"
            },
            {
                name: "C.Description", title: description, "data": function (data, type) {
                    return data.Description;
                }, className: "col-lg-5"
            },
            {
                "data": function (data, type) {
                    return "<a href=\"#\" class=\"btn btn-white btn-sm\"><i class=\"fa fa-folder\"></i> " + view + " </a>";
                },
                className: "text-right col-lg-2", sortable: false,
                title: action
            }
        ],
        "order": [[0, "asc"]],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $(nRow).find("td").each(function (index) {
                if (index != $(nRow).find("td").length - 1) {
                    $(this).addClass("table-description-field");
                    this.setAttribute('title', $(this).text().trim());
                }
            });
        }
    });
    $('#tblFroms').dataTable().on('draw.dt', function () {
        $('.i-checks').iCheck({
            checkboxClass: 'icheckbox_square-blue'
        });
    });
}