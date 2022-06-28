var attachmentDropzone;
var table = null;
$(document).ready(function () {
    $(".select2dropdown").select2({ width: '100%' });

    $.validator.addMethod('selectRule', function (value, element) {
        return parseInt($(element).val()) > 0; // return bool here if valid or not.
    }, requiredValidationMsg);

    $('#form').validate();

    InitializeDropzone();

    $("#btnNew").click(function () {
        var isFormValid = $('#form').valid();
        if (isFormValid) {
            isFormValid = DropzoneValidation();
            if (isFormValid) {
                SaveTemplate();
            }
        }
        return isFormValid;
    });

    $("#btnCancel").click(function () {
        window.location.href = ROOTPath + "/PrintTemplates/PrintTemplate/List";
    });

});

function InitializeDropzone() {
    var accept = ".doc,.docx,.odt";
    $('#attachmentDropzone').dropzone({
        url: ROOTPath + "/PrintTemplates/PrintTemplate/UploadFileTemporary",
        autoProcessQueue: true,
        addRemoveLinks: true,
        maxFiles: 1,
        parallelUploads: 1,
        uploadMultiple: false,
        dictRemoveFile: removefile,
        dictDefaultMessage: dropzonemessage,
        acceptedFiles: accept,
        init: function () {
            attachmentDropzone = this;
            attachmentDropzone.hiddenFileInput.removeAttribute('multiple');
            attachmentDropzone.on("addedfile", function (file) {
                if (attachmentDropzone.files[1] != null) {
                    attachmentDropzone.removeFile(attachmentDropzone.files[1]);
                    showAlert("error", uploadedfilelimitmessage);
                }
            });
            attachmentDropzone.on("removedfile", function (file) {
                var datFileID = $("#txtOldFileName").attr("data-fileId");
                if (file.name == $("#txtOldFileName").val() && datFileID == file.id) {
                    $("#txtNewFileName").val("");
                    $("#txtOldFileName").val("");
                    $("#txtOldFileName").attr({
                        title: ""
                    });
                    $("#dvFileUpload").addClass("hidden");
                }
            });
            attachmentDropzone.on("error", function (file) {
                if (!file.accepted && file.status == "added") {
                    this.removeFile(file);
                    showAlert("error", (uploadedfiletypemessage + " " + allowOnly.format(accept)));
                }
            });
            $(".dz-hidden-input").attr("data-rule-selectRule", "true");
        },
        success: function (file, response) {
            file.id = response.ID;
            CreateDataTable(response);
        }
    });
}
function CreateDataTable(response) {
    $("#txtNewFileName").val(response.FileName);
    $("#txtOldFileName").val(response.FileNameWithExtention);
    $("#txtOldFileName").attr({
        title: response.FileNameWithExtention
    });
    $("#txtOldFileName").attr({
        "data-fileId": response.ID
    });

    if (!$.fn.dataTable.isDataTable("#tblFileUpload")) {
        $("#tblFileUpload").dataTable({
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
    $("#dvFileUpload").removeClass("hidden");
    $("#UploadFile-error").addClass("hidden");
}
function DropzoneValidation() {
    var valid = true;
    if (!$("#dvFileUpload").is(":visible")) {
        $("#UploadFile-error").removeClass("hidden");
        valid = false;
    }
    return valid;
}

function SaveTemplate() {
    $.post(ROOTPath + "/PrintTemplates/PrintTemplate/Add", CreateDataModel(), function (responce) {
        if (!responce.Status) {
            showAlert("error", responce.Message);
        }
        else {
            window.location = ROOTPath + "/PrintTemplates/PrintTemplate/List";
        }
    });
}

function CreateDataModel() {
    return {
        Code: $("#Code").val(),
        TemplateName: $("#TemplateName").val(),
        Description: $("#Description").val(),
        WorkflowID: $("#WorkflowID").val(),
        StatusID: $("#StatusID").val(),
        DataSourceID: $("#DataSourceID").val(),
        FileName: $("#txtNewFileName").val()
    };
}