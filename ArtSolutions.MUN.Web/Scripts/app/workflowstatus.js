$(document).ready(function () {
    WorkflowStatusGet();
});

function WorkflowStatusGet() {
    $('#tblStatuses').dataTable({
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
            "url": ROOTPath + "/Workflows/WorkflowStatus/List",
            "type": "POST",
            "contentType": "application/json",
            "data": function (data) {
                data.workflowID = parseInt($("#hdnWorkflowID").val());
                return JSON.stringify(data);
            }
        },
        "destroy": true,
        "columns": [
            {
                name: "C.Activity", title: activity, "data": "Activity", className: "col-lg-2"
            },
            {
                name: "Name", title: name, "data": "Name", className: "col-lg-2"
            },
            {
                name: "Description", title: description, "data": function (data, type) {
                    return data.Description;
                }, className: "col-lg-3"
            },
            {
                name: "Reason", title: reason, "data": function (data, type) {
                    var tempString = "";
                    if (data.Reasons != null) {
                        var reason = data.Reasons.split(",");
                        for (var i in reason) {
                            tempString += "<span class=\"tag label label-primary m-r-xss\">" + reason[i].split("^")[0] + "</span>";
                        }
                    }
                    return tempString;
                }, className: "col-lg-3", sortable: false
            },
            {
                "data": function (data, type) {
                    var str = "";
                    if (data.IsPublic) {
                        str = "<a onclick=\"StatusEdit(" + data.ID + ",3)\" class=\"btn btn-white btn-sm\"><i class=\"fa fa-folder\"></i> " + view + " </a>";
                    }
                    else {
                        str = "<a onclick=\"StatusEdit(" + data.ID + ",3)\" class=\"btn btn-white btn-sm\"><i class=\"fa fa-folder\"></i> " + view + " </a>";
                        str += "<a onclick=\"StatusEdit(" + data.ID + ",2)\" class=\"btn btn-white btn-sm m-l-xs\"><i class=\"fa fa-pencil\"></i> " + edit + " </a>";
                    }
                    return str;
                },
                className: "text-right col-lg-2", sortable: false,
                title: "<button type=\"button\" id=\"btnNewStatus\" name=\"btnNewStatus\" class=\"btn btn-primary btn-sm\">" + add + "</button>"
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
    $("#tblStatuses").on('draw.dt', function () {
        $("#btnNewStatus").click(function () {
            StatusEdit(0, 1, function (result) {
                if (result) {
                    setTimeout(function () {
                        $("#txtStatusName").focus();
                    }, 500);
                }
            });
        });
    });
}
function StatusEdit(id, actionType, callback) {
    var data = {
        id: parseInt(id),
        actionType: actionType,
        workflowID: parseInt($("#hdnWorkflowID").val())
    };
    $.get(ROOTPath + "/Workflows/WorkflowStatus/Get", data, function (result) {
        if (result) {
            $("#dvWorkFlowModalContent").html(result);
            $('.i-checks').iCheck({
                checkboxClass: 'icheckbox_square-blue'
            });
            $(".select2dropdown").select2({
                width: "100%"
            });
            $("#dvWorkFlowModal").modal("show");
            $("#dvWorkflowModaldialog").removeClass("modal-md").addClass("modal-lg");
            if (callback)
                callback(true);
        }
    });
}
function CreateWorkflowStatuModal() {
    var groups = $("#ddlGroup").val() == null || $("#ddlGroup").val() == "" ? "" : $("#ddlGroup").val().join(",");
    var model = {
        Name: $("#txtStatusName").val().trim(),
        Activity: $("#txtStatusActivity").val().trim(),
        Description: $("#txtstatusDescription").val().trim(),
        AllowEdit: $("#AllowEdit").is(":checked"),
        AllowDelete: $("#AllowDelete").is(":checked"),
        InitialStatus: $("#InitialStatus").is(":checked"),
        Post: $("#Post").is(":checked"),
        Void: $("#Void").is(":checked"),
        IsPartial: $("#IsPartial").is(":checked"),
        IsFull: $("#IsFull").is(":checked"),
        Reasons: $("#ddlReasons").val() == null || $("#ddlReasons").val() == "" ? "" : $("#ddlReasons").val().join(","),
        Groups: groups,
        WorkflowID: parseInt($("#hdnWorkflowID").val()),
        WorkflowVersionID: parseInt($("#hdnWorkflowVersionID").val()),
        ID: parseInt($("#hdnStatusID").val())
    };
    return model;
}
function OnAddStatus() {
    if (ValidateStatus()) {
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            type: 'POST',
            url: (ROOTPath + "/Workflows/WorkflowStatus/Add"),
            data: JSON.stringify(CreateWorkflowStatuModal()),
            success: function (result) {
                if (result.Status) {
                    showAlert("success", result.Message);
                    if ($('#tblStatuses').length > 0)
                        WorkflowStatusGet();
                    if ($('#tblStatusSequence').length > 0)
                        WorkflowStatusSequence();
                    if ($('#tblReasons').length > 0)
                        WorkflowReasonGet();
                    if ($('#tblGroups').length > 0)
                        WorkflowGroupGet();
                    if ($('#tblUsers').length > 0)
                        WorkflowGroupUsersGet();
                    $("#dvWorkFlowModal").modal("hide");
                }
                else {
                    showAlert("error", result.Message);
                }
            }
        });
    }
}
function ValidateStatus() {
    var isvalid = true;
    var formControl = null;
    if ($("#txtStatusName").val() == null || $("#txtStatusName").val() == "") {
        isvalid = false;
        formControl = $("#txtStatusName");
        $("#txtStatusName").next(".field-validation-error").removeClass("hidden");
    }
    else {
        $("#txtStatusName").next(".field-validation-error").addClass("hidden");
    }
    if ($("#txtStatusActivity").val() == null || $("#txtStatusActivity").val() == "") {
        isvalid = false;
        if (formControl != null)
            formControl = $("#txtStatusActivity");
        $("#txtStatusActivity").next(".field-validation-error").removeClass("hidden");
    }
    else {
        $("#txtStatusActivity").next(".field-validation-error").addClass("hidden");
    }
    if (formControl != null) {
        $(formControl).focus();
    }
    return isvalid;
}
function OnCloseStatusModal() {
    $("#dvWorkFlowModal").modal("hide");
}