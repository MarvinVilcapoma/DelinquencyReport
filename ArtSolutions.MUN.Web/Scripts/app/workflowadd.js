$(document).ready(function () {
    $("#form1").steps({
        bodyTag: "fieldset",
        stepsOrientation: "vertical",
        onStepChanging: function (event, currentIndex, newIndex) {

            if (currentIndex > newIndex) {
                return true;
            }
            var form = $(this);


            if (currentIndex < newIndex) {

                $(".body:eq(" + newIndex + ") label.error", form).remove();
                $(".body:eq(" + newIndex + ") .error", form).removeClass("error");
            }

            form.validate().settings.ignore = ":disabled,:hidden";
            var isvalid = form.valid();
            if (currentIndex == 1) {
                if (isvalid) {
                    if ($("#tblStatus").find("tbody").find("tr").length == 0) {
                        isvalid = false;
                        showAlert("error", statusValidataion);
                    }
                }
            }
            if (newIndex == 2 && isvalid) {
                isvalid = true;
                Summary();
            }
            if (newIndex == 1) {
                $("#txtWorkflowStatus").focus();
            }
            return isvalid;
        },
        onStepChanged: function (event, currentIndex, priorIndex) {
            if (currentIndex === 2 && priorIndex === 3) {
                $(this).steps("previous");
            }
        },
        onStepChanged: function (event, currentIndex, priorIndex) {
            if (currentIndex === 4) {
                $(this).steps("previous");
            }
        },
        onFinishing: function (event, currentIndex) {
            var form = $(this);

            form.validate().settings.ignore = ":disabled";

            return form.valid();
        },
        onFinished: function (event, currentIndex) {
            var form = $(this);

            Save();
        },
        onCanceled: function (event) {
            window.location.href = ROOTPath + '/Workflows/Workflow/List';
        },
        labels: {
            next: next,
            previous: previous,
            cancel: cancel,
            finish: finish
        },
    });
    $(".select2dropdown").select2({
        width: "100%"
    });
    $("#txtWorkflowStatus").keyup(function (e) {
        if (e.keyCode == 13) {
            OpenStatusAddPopup();
        }
    });
});
function OpenStatusAddPopup() {
    var data = {
        id: 0,
        actionType: 1,
        Name: $("#txtWorkflowStatus").val()
    };
    $.get(ROOTPath + "/Workflows/WorkflowStatus/Get", data, function (result) {
        $("#dvWorkFlowModalContent").html(result);
        $('.i-checks').iCheck({
            checkboxClass: 'icheckbox_square-blue'
        });
        $('.select2dropdown').select2({
            width: "100%"
        });
        $("#dvWorkFlowModal").modal("show");
    });
}
function OnAddStatus() {
    if (ValidateStatus()) {
        var groups = $("#ddlGroup").val() == null || $("#ddlGroup").val() == "" ? "" : $("#ddlGroup").val().join(",");
        var row = "<tr>";
        row += "<td>";
        row += $("#txtStatusName").val().trim();
        row += "</td>";
        row += "<td>";
        row += $("#txtStatusActivity").val().trim();
        row += "</td>";
        row += "<td>";
        var reasons = $("#ddlReasons").select2('data');
        var tempString = "";
        var tempReasons = new Array();
        if (reasons) {
            for (var i in reasons) {
                tempReasons.push(reasons[i].id);
                tempString += "<span class=\"tag label label-primary m-r-xss\" data-val=\"" + reasons[i].id + "\">" + reasons[i].text + "</span>";
            }
        }
        row += tempString;
        row += "</td>";
        row += "<td class=\"text-right\">";
        row += " <button type=\"button\" onclick=\"OnRemoveStatusRow(this)\" class=\"btn btn-sm btn-default\" title=\"" + deletes + "\"><i class=\"fa fa-trash\"></i></button>";
        row += "<input type=\"hidden\" name=\"hdnStatusProp\" data-InitialStatus=\"" + $("#InitialStatus").is(":checked") + "\" " +
            "data-name=\"" + $("#txtStatusName").val() + "\" data-Activity=\"" + $("#txtStatusActivity").val() + "\" " +
            "data-AllowEdit=\"" + $("#AllowEdit").is(":checked") + "\" data-AllowDelete=\"" + $("#AllowDelete").is(":checked") + "\" " +
            "data-Post=\"" + $("#Post").is(":checked") + "\" data-Void=\"" + $("#Void").is(":checked") + "\" data-IsFull=\"" + $("#IsFull").is(":checked") + "\" " +
            "data-IsPartial=\"" + $("#IsPartial").is(":checked") + "\" data-Description=\"" + $("#txtstatusDescription").val() + "\"" +
            "data-Groups=\"" + groups + "\" data-reasons=\"" + tempReasons.join(",") + "\"/>";
        row += "</td>";
        row += "</tr>";
        $("#tblStatus").find("tbody").append(row);
        $(".select2dropdown").select2({
            width: '100%',
            containerCss: { "display": "block" }
        });
        $("#txtWorkflowStatus").val("");
        $("#dvStatusForm").find("input[type=text]").val("");
        $("#dvStatusForm").find("input[type=checkbox]").prop('checked', false);

        $("#dvWorkFlowModal").modal("hide");
    }
}
function OnRemoveStatusRow(elmnt) {
    var row = $(elmnt).closest("tr");
    $(row).remove();
}
function OnEditStatusRow(elmnt) {
    var row = $(elmnt).closest("tr");
    var statusProperties = $(row).find("input[name=hdnStatusProp]");
    $("#txtStatusName").val($(statusProperties).data("name"));
    $("#txtStatusActivity").val($(statusProperties).data("activity"));
    $("#txtstatusDescription").val($(statusProperties).data("description"));
    if ($(statusProperties).data("initialstatus") == true) {
        $("#InitialStatus").iCheck("check");
        $("#InitialStatus").iCheck("update");
    }
    if ($(statusProperties).data("allowedit") == true) {
        $("#AllowEdit").iCheck("check");
        $("#AllowEdit").iCheck("update");
    }
    if ($(statusProperties).data("allowdelete") == true) {
        $("#AllowDelete").iCheck("check");
        $("#AllowDelete").iCheck("update");
    }
    if ($(statusProperties).data("post") == true) {
        $("#Post").iCheck("check");
        $("#Post").iCheck("update");
    }
    if ($(statusProperties).data("void") == true) {
        $("#Void").iCheck("check");
        $("#Void").iCheck("update");
    }
    if ($(statusProperties).data("isfull") == true) {
        $("#IsFull").iCheck("check");
        $("#IsFull").iCheck("update");
    }
    if ($(statusProperties).data("ispartial") == true) {
        $("#IsPartial").iCheck("check");
        $("#IsPartial").iCheck("update");
    }
    $("#dvWorkFlowModal").modal("show");
}
function Summary() {
    $("#lblDocumentType").text($("#DocumentTypeID option:selected").text());
    $("#lblName").text($("#Name").val());
    $("#lblDescription").text($("#Description").val());
    $("#tblConfirmStatus").find("thead").empty().html($("#tblStatus").find("thead").html());
    $("#tblConfirmStatus").find("tbody").empty();
    $("#tblStatus").find("tbody").find("tr").each(function (i, elmnt) {
        var row = "";
        var colneRow = $(elmnt).clone();
        row += "<tr>";
        row += $(colneRow).html();
        row += "</tr>";
        $("#tblConfirmStatus").find("tbody").append(row);
    });

}
function OnCloseStatusModal() {
    $("#dvWorkFlowModal").modal("hide");
}
function CreateWorkflowModel() {
    var statusList = new Array();
    $("#tblStatus").find("tbody").find("tr").each(function (i, elmnt) {
        var statusProp = $(elmnt).find("input[name=hdnStatusProp]");
        statusList.push({
            Name: $(statusProp).data("name"),
            Activity: $(statusProp).data("activity"),
            Description: $(statusProp).data("description"),
            AllowEdit: $(statusProp).data("allowedit"),
            AllowDelete: $(statusProp).data("allowdelete"),
            InitialStatus: $(statusProp).data("initialstatus"),
            Post: $(statusProp).data("post"),
            Void: $(statusProp).data("void"),
            IsPartial: $(statusProp).data("ispartial"),
            IsFull: $(statusProp).data("isfull"),
            Reasons: $(statusProp).data("reasons"),
            Groups: $(statusProp).data("groups") == null ? "" : $(statusProp).data("groups")
        });
    });
    return {
        DocumentTypeID: parseInt($("#DocumentTypeID").val()),
        Name: $("#Name").val(),
        Description: $("#Description").val(),
        WorkflowStatusList: statusList
    };
}
function Save() {
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        type: 'POST',
        url: (ROOTPath + "/Workflows/Workflow/Add"),
        data: JSON.stringify(CreateWorkflowModel()),
        success: function (result) {
            if (result.Status) {
                showAlert("success", result.Message);
                setTimeout(function () {
                    window.location.href = ROOTPath + '/Workflows/Workflow/List';
                }, 2000);
            }
            else {
                showAlert("error", result.Message);
            }
        }
    });
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