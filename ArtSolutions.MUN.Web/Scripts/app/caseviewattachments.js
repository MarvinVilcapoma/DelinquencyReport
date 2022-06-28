$(document).ready(function () {
    InitializeAttachmentDropzone();

    CaseAttachments(caseID);

    $("#btnUploadAttachment").click(function () {
        var isFormValid = $("#formAttachment").valid();
        if (isFormValid) {
            Save();
        }
        return isFormValid;
    });

    $("#btnCancelAttachment").click(function () {
        Dropzone.forElement("#attachmentDropzone").removeAllFiles(true);
        $("#dvAttachmentsModel").modal("hide");
    });
});

function Save() {
    $.post(ROOTPath + "/cases/case/addattachment", CreateModel(), function (responce) {
        $("#dvAttachmentsModel").modal("hide");
        if (responce.Status) {
            Dropzone.forElement("#attachmentDropzone").removeAllFiles(true);
            $('#tblAttachments').dataTable().api().ajax.reload();
        }
        showAlert((responce.Status ? "success" : "error"), responce.Message);
    });
}

function CreateModel() {
    return {
        id: caseID,
        fileName: $("#txtNewFileName").val()
    };
}

function InitializeAttachmentDropzone() {
    $('#attachmentDropzone').dropzone({
        url: ROOTPath + "/PrintTemplates/PrintTemplate/UploadFileTemporary",
        autoProcessQueue: true,
        addRemoveLinks: true,
        maxFiles: 1,
        parallelUploads: 1,
        uploadMultiple: false,
        dictRemoveFile: removefile,
        dictDefaultMessage: dropzonemessage,
        init: function () {
            this.hiddenFileInput.removeAttribute('multiple');
            this.on("addedfile", function (file) {
                if (this.files[1] != null) {
                    this.removeFile(this.files[1]);
                }
            });
            this.on("removedfile", function (file) {
                $("#tbldvAttachments").find("tbody").empty();
            });
        },
        success: function (file, response) {
            $("#tbldvAttachments").find("tbody").empty();
            var tempString = "";
            tempString += "<tr>";
            tempString += "<td>";
            tempString += "<input type=\"text\" name=\"txtNewFileName\" id=\"txtNewFileName\" class=\"form-control required\" maxlength=\"50\" value=\"" + response.FileName + "\" />";
            tempString += "<span class=\"field-validation-valid\" data-valmsg-for=\"txtNewFileName\" data-valmsg-replace=\"true\"></span>";
            tempString += "</td>";
            tempString += "<td>";
            tempString += "<label class=\"form-control no-borders p-l-none table-description-field\" data-fileId=\"" + response.ID + "\"  style=\"max-width:380px !important;\" title=\"" + response.FileNameWithExtention + "\">" + response.FileNameWithExtention + "</label>";
            tempString += "</td>";
            tempString += "</tr>";
            $("#tbldvAttachments").find("tbody").html(tempString);
            if (!$.fn.dataTable.isDataTable("#tbldvAttachments")) {
                $("#tbldvAttachments").dataTable({
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
            $('#dvAttachmentsModel').modal({ backdrop: 'static', keyboard: false });
            $("#dvAttachmentsModel").modal("show");
        }
    });
}

function CaseAttachments(caseid) {
    var customSearch = {
        CaseID: caseid
    };
    $('#tblAttachments').dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg,
            oPaginate: {
                "sFirst": first,
                "sLast": last,
                "sNext": next,
                "sPrevious": previous
            }
        },
        "pageLength": pageSize,
        "serverSide": true,
        "processing": true,
        "bFilter": false,
        "bInfo": false,
        "bLengthChange": false,
        "autoWidth": false,
        "ajax": {
            "url": ROOTPath + "/Cases/Case/CaseImagesList",
            "type": "POST",
            "data": function (data) {
                data.customSearch = customSearch;
            }
        },
        destroy: true,
        "columns": [
            {
                name: "SrNo", title: id, "data": "SrNo", width:"7%", "orderable": false
            },
            {
                name: "FileName", title: fileName, "data": "FileName", width: "43%"
            },
            {
                name: "B.CreatedUserID", title: byUser, "data": "ByUser", width: "25%"
            },
            {
                name: "B.CreatedDate", title: date, "data": "Date", width: "25%"
            }
        ],
        "order": [[3, "asc"]],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $(nRow).find("td").each(function (index) {
                $(this).addClass("table-description-field");
                this.setAttribute('title', $(this).text().trim());
            });
        }
    });
}