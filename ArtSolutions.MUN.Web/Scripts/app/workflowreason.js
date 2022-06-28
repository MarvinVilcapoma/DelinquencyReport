$(document).ready(function () {
    if ($('#tblReasons').length > 0)
        WorkflowReasonGet();
    if ($(".select2dropdown").length > 0)
        $(".select2dropdown").select2({
            width: "100%"
        });
    $("#btnrefresh").click(function () {
        $("#txtSearch").val('');
        WorkflowReasonGet();
    });
    $("#btnSearch").click(function () {
        WorkflowReasonGet();
    });
    $('#txtSearch').bind("keypress", function (e) {
        if (e.keyCode == 13) {
            WorkflowReasonGet();
            return false;
        }
    });
});

function WorkflowReasonGet() {
    var url = "";
    var model = {};
    if (isEditorPage !== undefined && isEditorPage) {
        url = ROOTPath + "/Workflows/WorkflowReason/GetByWorkflowID";
        model = {
            WorkflowID: $("#hdnWorkflowID").val(),
            WorkflowVersionID: $("#hdnWorkflowVersionID").val()
        };
    }
    else {
        url = ROOTPath + "/Workflows/WorkflowReason/List";
        model = {
            FilterText: $("#txtSearch").val()
        };
    }
    $('#tblReasons').dataTable({
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
                data.model = model;
                return JSON.stringify(data);
            }
        },
        "destroy": true,
        "columns": [
            {
                name: "Name", title: name, "data": "Name", className: "col-lg-4"
            },
            {
                name: "Description", title: description, "data": function (data, type) {
                    return data.Description;
                }, className: "col-lg-4"
            },
            {
                "data": function (data, type) {
                    var str = "";
                    if (data.IsPublic) {
                        str = "<a onclick=\"ReasonEdit(" + data.ID + ",3)\" class=\"btn btn-white btn-sm\"><i class=\"fa fa-folder\"></i> " + view + " </a>";
                    }
                    else {
                        str = "<a onclick=\"ReasonEdit(" + data.ID + ",3)\" class=\"btn btn-white btn-sm\"><i class=\"fa fa-folder\"></i> " + view + " </a>";
                        str += "<a onclick=\"ReasonEdit(" + data.ID + ",2)\" class=\"btn btn-white btn-sm m-l-xs\"><i class=\"fa fa-pencil\"></i> " + edit + " </a>";
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
    $("#tblReasons").on('draw.dt', function () {
        $("#btnNewReason").click(function () {
            ReasonEdit(0, 1, function (result) {
                if (result) {
                    setTimeout(function () {
                        $("#ddlStatus").focus();
                    }, 500);
                }
            });
        });
    });
}
function ReasonEdit(id, actionType, callback) {
    if (!isEditorPage) {
        window.location.href = ROOTPath + "/Workflows/WorkflowReason/Get/" + id + "?actionType=" + actionType + "&&workflowID=0";
        return false;
    }
    var data = {
        id: parseInt(id),
        actionType: actionType,
        workflowID: parseInt($("#hdnWorkflowID").val()),
        isEditor: true
    };
    $.get(ROOTPath + "/Workflows/WorkflowReason/Get", data, function (result) {
        if (result) {
            $("#dvWorkFlowModalContent").html(result);
            $(".select2dropdown").select2({
                width: "100%"
            });
            $("#dvWorkFlowModal").modal("show");
            $("#dvWorkflowModaldialog").removeClass("modal-lg").addClass("modal-md");
            if (callback)
                callback(true);
        }
    });
}
function CreateWorkflowReasonModal() {
    var model = {
        Name: $("#txtReasonName").val().trim(),
        Description: $("#txtReasonDescription").val().trim(),        
        ID: parseInt($("#hdnReasonID").val())        
    };
    return model;
}
function OnAddReason() {
    if ($("#frmReason").valid()) {
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            type: 'POST',
            url: (ROOTPath + "/Workflows/WorkflowReason/Add"),
            data: JSON.stringify(CreateWorkflowReasonModal()),
            success: function (result) {
                if (result.Status) {
                    showAlert("success", result.Message);
                    if (isEditorPage !== undefined && isEditorPage) {
                        $("#dvWorkFlowModal").modal("hide");
                        $('#tblReasons').dataTable().api().ajax.reload();
                    }
                    else {
                        setTimeout(function () {
                            window.location.href = ROOTPath + "/workflows/workflowreason/List";
                        }, 2000);
                    }
                }
                else {
                    showAlert("error", result.Message);
                }
            }
        });
    }
}
