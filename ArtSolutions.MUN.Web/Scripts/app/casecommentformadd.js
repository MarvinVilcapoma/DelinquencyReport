
$(document).ready(function () {
    InitializeCommentDropzone();
    $("#btnCancelComments,#aCancelCommentForm").click(function () {
        $.post(ROOTPath + "/cases/case/RemoveTemporaryFile", null, function (response) {
            if (response.Status) {
                ClearCommentFormControl();
                $('#dvActivityModel').modal('hide');
            }
            else {
                $('#dvActivityModel').modal('hide');
            }
        });
        
    });

    $('#vertical-timeline').slimscroll({
        height: "250px"
    });

    $('#dvActivityModel').on('shown.bs.modal', function () {
        $("#tblCommentFileUpload").find("tbody").empty();
        $("#dvCommentFileUpload").addClass("hidden");
        Dropzone.forElement("#commentDropzone").removeAllFiles(true);
    });
});
function ClearCommentFormControl() {
    $("#ddlCommentAssignTo").prop('selectedIndex', 0).trigger('change');
    $("#ddlCommentReasons").prop('selectedIndex', 0).trigger('change');
    $("#Comments").val("");
}
function DocumentWorkflowHistoryLogGet(caseID, callback) {
    var data = {
        caseIDs: caseID
    };
    $.get(ROOTPath + "/Cases/Case/DocumentWorkflowHistoryLogGet", data, function (responce) {
        callback(responce);
    });
}
function WorkflowFormGet(formID, callback) {
    var data = {
        id: formID
    };
    $.get(ROOTPath + "/Cases/Case/WorkflowFormGet", data, function (responce) {
        callback(responce);
    });
}
function FillReasonsByStatusID(statusID, workflowID, isJsTree, isSelected, callback) {
    var data = {
        statusid: statusID,
        workflowID: parseInt(workflowID),
        isJsTree: isJsTree
    };
    $.get(ROOTPath + "/Case/GetWorkflowStatusReason", data, function (result) {
        if (result.length > 0) {
            var tempString = "";
            if (isSelected) {
                tempString += "<option value=\"-1\" selected='selected'>" + dropDownSelectMessage + "</option>";
            }
            for (var i in result) {
                tempString += "<option value=\"" + result[i].ID + "\">" + result[i].Name + "</option>";
            }
        }
        callback(tempString);
    });
}
function GetTimeline(logs) {
    var tempString = "";
    if (logs) {
        if (logs.length == 1) {
            tempString = GetLogs(logs[(logs.length - 1)].Logs);
        }
        else if (logs.length > 1) {
            tempString = "";
            for (var i in logs) {
                tempString += "<div class=\"ibox-title\">";
                tempString += "<h5>" + logs[i].Name + "</h5>";
                tempString += "</div>";
                tempString += "<div class=\"ibox-content\">";
                tempString += GetLogs(logs[i].Logs);
                tempString += "</div>";
            }
        }
    }
    return tempString;
}
function GetLogs(userDetails) {
    var tempString = "";
    for (var j in userDetails) {
        tempString += "<div class=\"vertical-timeline-block\">";
        tempString += "<div class=\"vertical-timeline-icon gray-bg\">";
        tempString += "<i class=\"fa fa-briefcase\"></i>";
        tempString += "</div>";
        tempString += "<div class=\"vertical-timeline-content\">";
        tempString += "<p class=\"no-margin-bottom\"><strong>" + userDetails[j].StatusName + "</strong> " + userDetails[j].AssignedTo + "</p>";
        tempString += "<span class=\"vertical-date small text-muted\">" + userDetails[j].CreatedDateInString + "</span>";
        tempString += "</div>";
        tempString += "</div>";
    }
    return tempString;
}
function InitializeCommentDropzone() {
    $('#commentDropzone').dropzone({
        url: ROOTPath + "/Cases/Case/UploadFileTemporary",
        autoProcessQueue: true,
        addRemoveLinks: true,
        maxFiles: 10,
        parallelUploads: 1,
        uploadMultiple: false,
        dictRemoveFile: removefile,
        dictDefaultMessage: dropzonemessage,
        init: function () {
            frmcommentDropZone = this;
            this.on("removedfile", function (file) {
                var data = {
                    fileName: file.name
                };
                $.post(ROOTPath + "/Cases/Case/RemoveTemporaryFile", data, function (removeFileResponse) {
                    if (removeFileResponse.responseMessage.Status) {
                        $("#tblCommentFileUpload").find("tbody").find("tr").each(function (index, row) {
                            var removeFileName = $(row).find("td").find("label[name='lblOldFileName']").text();
                            var fileId = $(row).find("td").find("label[name='lblOldFileName']").attr("data-fileid");
                            if ((removeFileName.toLowerCase() == file.name.toLowerCase()) && file.id == fileId) {
                                $(row).remove();
                                return false; 
                            }
                        });
                        if ($("#tblCommentFileUpload").find("tbody").find("tr").length == 0) {
                            $("#dvCommentFileUpload").addClass("hidden");
                        }
                    }
                });

            });
            this.on("error", function (file) {
                if (!file.accepted) {
                    this.removeFile(file);
                    showAlert("error", (uploadedfiletypemessage + " " + allowOnly.format(accept)));
                }
                file.previewElement.classList.add("dz-error");
            });

        },
        success: function (file, response) {
            file.id = response.ID;
            var tempString = "";
            tempString += "<tr>";
            tempString += "<td>";
            tempString += "<input type=\"text\" name=\"txtNewFileName\" id=\"txtNewFileName\" class=\"form-control required\" maxlength=\"50\" value=\"" + response.FileName + "\" />";
            tempString += "<span class=\"field-validation-valid\" data-valmsg-for=\"txtNewFileName\" data-valmsg-replace=\"true\"></span>";
            tempString += "</td>";
            tempString += "<td>";
            tempString += "<label name=\"lblOldFileName\" data-fileId=\"" + response.ID+ "\" style=\"max-width:380px !important;\" class=\"no-borders table-description-field text-left\" title=\"" + response.FileNameWithExtention + "\">" + response.FileNameWithExtention + "</label>";
            tempString += "</td>";
            tempString += "</tr>";
            $("#tblCommentFileUpload").find("tbody").append(tempString);
            if (!$.fn.dataTable.isDataTable("#tblCommentFileUpload")) {
                $("#tblCommentFileUpload").dataTable({
                    searching: false,
                    paging: false,
                    "ordering": false,
                    "bPaginate": false,
                    "bLengthChange": false,
                    "bFilter": true,
                    "bInfo": false,
                    "bAutoWidth": false,
                    destroy: true
                });
            }
            $("#dvCommentFileUpload").removeClass("hidden");
            $("#UploadFile-error").addClass("hidden");
        }
    });
}
