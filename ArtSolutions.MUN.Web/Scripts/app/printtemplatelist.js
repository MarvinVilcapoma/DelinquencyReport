$(document).ready(function () {
    GetCases(SetSearchModel());

    $("#btnrefresh").click(function () {
        ClearControls();
        GetCases(SetSearchModel());
    });

    $("#btnSearch").click(function () {
        GetCases(SetSearchModel());
    });

    $('#txtSearch').keypress(function (e) {
        if (e.keyCode == 13) {
            $("#btnSearch").click();
        }
    });

    $("#btnNew").click(function () {
        window.location = ROOTPath + "/PrintTemplates/PrintTemplate/Add";
    });
});

function SetSearchModel() {
    return {
        FilterText: $("#txtSearch").val()
    };
}

function ClearControls() {
    $("#txtSearch").val('');
}

function GetCases(model) {
    $('#ListTable').dataTable({
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
        "processing": true,
        "bFilter": false,
        "bInfo": false,
        "bLengthChange": false,
        "autoWidth": false,
        "pageLength": pageSize,
        "ajax": {
            "url": ROOTPath + "/PrintTemplate/List",
            "type": "POST",
            "data": function (data) {
                data.model = model
            }
        },
        destroy: true,
        "columns": [
            {
                name: "Code", title: code, "data": "Code", className: "col-lg-1"
            },
            {
                name: "C.Name", title: name, "data": "TemplateName", className: "col-lg-2"
            },
            {
                name: "C.Description", title: description, "data": "Description", className: "col-lg-5"
            },
            {
                name: "D.Name", title: workflow, "data": "WorkflowName", className: "col-lg-2"
            },
            {
                "data": function (data, type) {
                    var str = "";
                    str = "<a href=\"" + ROOTPath + "/PrintTemplates/PrintTemplate/View/" + data.ID + "\" class=\"btn btn-white btn-sm\"><i class=\"fa fa-folder\"></i> " + view + " </a>";
                    str = str + "<a href=\"" + ROOTPath + "/PrintTemplates/PrintTemplate/Edit/" + data.ID + "\" class=\"btn btn-white btn-sm m-l-xs\"><i class=\"fa fa-pencil\"></i> " + edit + " </a>";
                    return str;
                },
                className: "text-right col-lg-2", sortable: false, title: actions
            }
        ],
        "order": [[0, "asc"]],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $(nRow).find("td").each(function (index) {
                if (index != 4) {
                    $(this).addClass("table-description-field");
                    this.setAttribute('title', $(this).text().trim());
                }
            });
        },
    });
}