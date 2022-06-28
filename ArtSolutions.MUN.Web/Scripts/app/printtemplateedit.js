var attachmentDropzone;
var table = null;
$(document).ready(function () {
    $(".select2dropdown").select2({ width: '100%' });

    $.validator.addMethod('selectRule', function (value, element) {
        return parseInt($(element).val()) > 0; // return bool here if valid or not.
    }, requiredValidationMsg);

    $('#form').validate();

    InitializeDropzone();

    $("#btnUpdate").click(function () {
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
            if (fileModel.FileName && fileModel.FileExtension) {
                var mockFile = {
                    name: fileModel.FileName + "" + fileModel.FileExtension,
                    size: fileModel.Length
                };
                // Call the default addedfile event handler
                attachmentDropzone.emit("addedfile", mockFile);
                attachmentDropzone.emit("complete", mockFile);
                attachmentDropzone.files.push(mockFile);

                $("#txtNewFileName").val(fileModel.FileName);
                $("#txtOldFileName").val(fileModel.FileName + "" + fileModel.FileExtension);
                $("#txtOldFileName").attr({
                    title: fileModel.FileName
                });
                $("#dvFileUpload").removeClass("hidden");
            }

            this.hiddenFileInput.removeAttribute('multiple');
            this.on("addedfile", function (file) {
                if (this.files[1] != null) {
                    this.removeFile(this.files[1]);
                    showAlert("error", uploadedfilelimitmessage);
                    return false;
                }
            });
            this.on("removedfile", function (file) {
                if (file.name === $("#txtOldFileName").val()) {
                    $("#txtNewFileName").val("");
                    $("#txtOldFileName").val("");
                    $("#txtOldFileName").attr({
                        title: ""
                    });
                    $("#dvFileUpload").addClass("hidden");
                }
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
            CreateDataTable(response);
        }
    });
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
    $.post(ROOTPath + "/PrintTemplates/PrintTemplate/Edit", CreateDataModel(), function (responce) {
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
        ID: $("#hdnID").val(),
        Code: $("#Code").val(),
        TemplateName: $("#TemplateName").val(),
        Description: $("#Description").val(),
        WorkflowID: $("#WorkflowID").val(),
        StatusID: $("#StatusID").val(),
        DataSourceID: $("#DataSourceID").val(),
        FileName: $("#txtNewFileName").val(),
        FileID: fileModel.ID
    };
}

function CreateDataTable(response) {
    $("#txtNewFileName").val(response.FileName);
    $("#txtOldFileName").val(response.FileNameWithExtention);
    $("#txtOldFileName").attr({
        title: response.FileNameWithExtention
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