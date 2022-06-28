var attachmentDropzone;
var imageIds = "";

function Initialize() {
    imageIds = "";
    initializeFillingDate();
    initializeDropzone();
    $.validator.unobtrusive.parse('#frmDebitNote');
}

function initializeFillingDate() {
    $('#Date').datepicker({
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture,
    });
    $('#Date').datepicker('update', new Date());
}

//========= DropZone Control Functions============= *@
function initializeDropzone(NoOfFilesToUpload) {
    $('#attachmentDropzone').dropzone({
        url: ROOTPath + "/File/UploadFile",
        autoProcessQueue: true,
        addRemoveLinks: true,
        maxFiles: NoOfFilesToUpload > 0 ? NoOfFilesToUpload : MaxFilesToUpload,
        maxFilesize: maxFileLength,
        acceptedFiles: allowedFileTypes,
        parallelUploads: 1,
        uploadMultiple: false,
        dictRemoveFile: removefile,
        dictDefaultMessage: dropzonemessage,
        init: function () {
            attachmentDropzone = this;
            imageIds = "";

            this.on("addedfile", function (file) {
                if (this.files.length > this.options.maxFiles) {
                    this.removeFile(file);
                    if (NoOfFilesToUpload == 1)
                        showAlert("warning", onlyonefileallowed);
                    else
                        showAlert("warning", Uploadedfilemaxlimitmessage);
                    return false;
                }
                if (file.size > this.options.maxFilesize) {
                    showAlert("warning", Uploadedfilemaxsizemessage + file.name);
                    this.removeFile(file);
                    return false;
                }
                if (allowedFileTypes.indexOf(file.type) < 0) {
                    showAlert("warning", uploadfiletypemsg);
                    this.removeFile(file);
                    return false;
                }
            });
            this.on("removedfile", function (file) {
                if (NoOfFilesToUpload == 1) {
                    $("#ImageID").val(null);
                }
                else {
                    imageIds = imageIds.replace(file.myCustomName, "");
                    RemoveAttachmentRow(file.name);
                }
            });
        },
        success: function (file, response) {
            if (NoOfFilesToUpload == 1) {
                $("#ImageID").val(response.id);
                file.previewElement.classList.add("dz-success");
            }
            else {
                if (response.id <= 0) {
                    showAlert("error", response.message);
                    this.removeFile(file);
                    return false;
                }
                else {
                    if (imageIds === "")
                        imageIds = response.id;
                    else
                        imageIds = imageIds + ',' + response.id;
                    file.myCustomName = response.id;
                    $("#ImageIds").val(imageIds);
                }
            }
        }
    });
}

function RemoveAttachmentRow(fileID) {
    $('#tblAttachmentRow').find("tr[id='" + fileID + "']").remove();

    if ($("#tblAttachement > tbody > tr").length === 0)
        $("#btnAddFile").prop("disabled", true);
}


//========= Tax POPUP Functions============= *@
$(document).on('click', '#btnCreditNote', function (e) {
    $.ajax({
        url: ROOTPath + "/Payment/CreditNote",
        async: false,
        data: { PaymentID: $("#Payment_ID").val() },
        dataType: 'json',
        success: function (data) {
            if (data.status === true) {
                $(".divCreditNote").html(data.returnData);
                Initialize();
                $('#frmCreditNote #Amount').on("cut copy paste", function (e) {
                    e.preventDefault();
                });
                $("#creditNoteModal").modal("show");
            }
            else {
                showAlert('error', data.msg);
            }
        }
    });
});

$(document).on('click', '#btnCreditNote', function (e) {
    $.ajax({
        url: ROOTPath + "/Payment/CreditNote",
        async: false,
        data: { PaymentID: $("#Payment_ID").val() },
        dataType: 'json',
        success: function (data) {
            if (data.status === true) {
                $(".divCreditNote").html(data.returnData);
                Initialize();
                $('#frmCreditNote #Amount').on("cut copy paste", function (e) {
                    e.preventDefault();
                });
                $("#creditNoteModal").modal("show");
            }
            else {
                showAlert('error', data.msg);
            }
        }
    });
});

function SuccessCallback(response) {
    if (response.status) {
        window.location.reload();
    }
    else
        showAlert("error", response.message);
}

function OpenViewJornalPopup(JournalId, DocumentTypeID, Number) {
    $.ajax({
        url: ROOTPath + "/Report/ViewJournal/",
        data: { id: JournalId, DocumentTypeID: DocumentTypeID, ReceiptNumber: Number },
        success: function (data) {
            if (data.status) {
                $("#journalpopup").html(data.RedirectUrl);
                $("#modalviewjournal").modal("show");
            }
            else
                showAlert("error", data.message);
        }
    });
}
function ValidateCreditNote() {
    var retVal = true;
    if ($("#frmCreditNote #Amount").val() != "" && GlobalConvertToDecimal($("#frmCreditNote #Amount").val()) < 0) {
        showAlert("warning", ValCreditBalancePositive);
        retVal = false;
    }
    return retVal;
}

function ValidateDebitNote() {
    var retVal = true;
    if ($("#frmDebitNote #Amount").val() != "" && GlobalConvertToDecimal($("#frmDebitNote #Amount").val()) < 0) {
        showAlert("warning", ValCreditBalancePositive);
        retVal = false;
    }
    return retVal;
}

$(document).on('click', '#btnDebitNote', function (e) {
    $.ajax({
        url: ROOTPath + "/Payment/DebitNote",
        async: false,
        data: { PaymentID: $("#Payment_ID").val() },
        dataType: 'json',
        success: function (data) {
            if (data.status === true) {
                $(".divDebitNote").html(data.returnData);
                Initialize();
                $('#frmDebitNote #Amount').on("cut copy paste", function (e) {
                    e.preventDefault();
                });
                $("#DebitNoteModal").modal("show");
            }
            else {
                showAlert('error', data.msg);
            }
        }
    });
});