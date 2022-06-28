$(document).ready(function () {
    if ($('#tblGroups').length > 0)
        WorkflowGroupGet();
    $("#btnrefresh").click(function () {
        $("#txtSearch").val('');
        WorkflowGroupGet();
    });
    $("#btnSearch").click(function () {
        WorkflowGroupGet();
    });
    $('#txtSearch').bind("keypress", function (e) {
        if (e.keyCode == 13) {
            WorkflowGroupGet();
            return false;
        }
    });
    $("#txtName").focus();
    $("span[data-valmsg-for=Group]").empty();
});

function WorkflowGroupGet() {
    var url = "";
    var model = {};
    if (isEditorPage !== undefined && isEditorPage) {
        url = ROOTPath + "/Workflows/WorkflowGroup/GetByWorkflowID";
        model = {
            WorkflowID: $("#hdnWorkflowID").val(),
            WorkflowVersionID: $("#hdnWorkflowVersionID").val()
        };
    }
    else {
        url = ROOTPath + "/Workflows/WorkflowGroup/List";
        model = {
            FilterText: $("#txtSearch").val()
        };
    }
    $('#tblGroups').dataTable({
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
            "url": url,
            "type": "POST",
            "contentType": "application/json",
            "data": function (data) {
                data.groupModel = model;
                return JSON.stringify(data);
            }
        },
        "destroy": true,
        "columns": [
            {
                name: "Name", title: name, "data": "Name", className: "col-lg-2"
            },
            {
                name: "Description", title: description, "data": function (data, type) {
                    return data.Description;
                }, className: "col-lg-4"
            },
            {
                "data": function (data, type) {
                    var str = "";
                    if (isEditorPage) {
                        str = "<a href=\"" + ROOTPath + "/Workflows/WorkflowGroup/UserAdd/" + data.ID + "?workflowID=" + model.WorkflowID + "&&documentTypeID=" + $("#hdnDocumentTypeID").val() + "\" class=\"btn btn-white btn-sm m-l-xs\"><i class=\"fa fa-pencil\"></i> " + edit + " </a>";
                    }
                    else {
                        if (data.IsPublic) {
                            str = "<a href=\"" + ROOTPath + "/Workflows/WorkflowGroup/Get/" + data.ID + "/?actionType=3\" class=\"btn btn-white btn-sm\"><i class=\"fa fa-folder\"></i> " + view + " </a>";
                        }
                        else {
                            str = "<a href=\"" + ROOTPath + "/Workflows/WorkflowGroup/Get/" + data.ID + "/?actionType=3\" class=\"btn btn-white btn-sm\"><i class=\"fa fa-folder\"></i> " + view + " </a>";
                            str += "<a href=\"" + ROOTPath + "/Workflows/WorkflowGroup/Get/" + data.ID + "/?actionType=2\" class=\"btn btn-white btn-sm m-l-xs\"><i class=\"fa fa-pencil\"></i> " + edit + " </a>";
                        }
                    }
                    return str;
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
    $("#tblGroups").on('draw.dt', function () {
        $("#btnNewGroup").click(function () {
            GroupEdit(0, 1, function (result) {
                if (result) {
                    setTimeout(function () {
                        $("#txtName").focus();
                    }, 500);
                }
            });
        });
    });
}
function GroupEdit(id, actionType, callback) {
    var data = {
        id: parseInt(id),
        actionType: actionType
    };
    $.get(ROOTPath + "/Workflows/WorkflowGroup/Get", data, function (result) {
        if (result) {
            $("#dvWorkFlowModalContent").html(result);
            $("#dvWorkFlowModal").modal("show");
            $("#dvWorkflowModaldialog").removeClass("modal-lg").addClass("modal-md");
            if (callback)
                callback(true);
        }
    });
}
function CreateWorkflowGroupModal() {
    var model = {
        Name: $("#txtName").val().trim(),
        Description: $("#txtGroupDescription").val().trim(),
        ID: parseInt($("#hdnGroupID").val())
    };
    return model;
}
function OnAddGroup() {
    if ($("#frmGroup").valid()) {
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            type: 'POST',
            url: (ROOTPath + "/Workflows/WorkflowGroup/Add"),
            data: JSON.stringify(CreateWorkflowGroupModal()),
            success: function (result) {
                if (result.Status) {
                    showAlert("success", result.Message);
                    setTimeout(function () {
                        window.location.href = ROOTPath + "/workflows/workflowgroup/list";
                    }, 2000);
                }
                else {
                    showAlert("error", result.Message);
                }
            }
        });
    }
}
