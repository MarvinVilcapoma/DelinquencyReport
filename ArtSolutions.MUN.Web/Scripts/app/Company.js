var companyPictureUpload;
var companyPictureId = 0;
var numSuccess = 0;

$(document).ready(function () {         
    //Hide on First Setup
    HideLeftMenu();

    $(function () {
        $('#txtBusinessName').focus();
         // Disable events on CompanyCode
        $("#txtCode").keyup(function () { return false });
        $("#txtCode").blur(function () { return false });     
    });

    $(document).on('change', 'input.TermsAndConditionsWizard', function () {
        var chk = $(this).prop('checked');
    });

    $("#wizard").steps();
    $("#form").steps({
        bodyTag: "fieldset",
        stepsOrientation: "vertical",
        enableCancelButton: false,
        labels: {
            finish: finishStepButton,
            next: nextStepButton,
            previous: previousStepButton,
        },
        onStepChanging: function (event, currentIndex, newIndex) {
            // Always allow going backward even if the current step contains invalid fields!
            if (currentIndex > newIndex) {
                return true;
            }
            var form = $(this);

            // Clean up if user went backward before
            if (currentIndex < newIndex) {
                // To remove error styles
                $(".body:eq(" + newIndex + ") label.error", form).remove();
                $(".body:eq(" + newIndex + ") .error", form).removeClass("error");
            }

            //Fill confirmation step
            if (newIndex == 3) Filldetails();

            // Disable validation on fields that are disabled or hidden.
            form.validate().settings.ignore = ":disabled,:hidden";

            // Start validation; Prevent going forward if false
            return form.valid();
        },
        onStepChanged: function (event, currentIndex, priorIndex) {
            // Suppress (skip) "Warning" step if the user is old enough and wants to the previous step.
            if (currentIndex === 2 && priorIndex === 3) {
                $(this).steps("previous");
            }
        },
        onStepChanged: function (event, currentIndex, priorIndex) {
            // Suppress (skip) "Warning" step if the user is old enough and wants to the previous step.
            if (currentIndex === 4) {
                $(this).steps("previous");
            }
        },
        onFinishing: function (event, currentIndex) {
            var form = $(this);
            // Disable validation on fields that are disabled.
            // At this point it's recommended to do an overall check (mean ignoring only disabled fields)
            form.validate().settings.ignore = ":disabled";

            // Start validation; Prevent form submission if false
            return form.valid();
        },
        onFinished: function (event, currentIndex) {
            var form = $('#form');
            // Submit form input
            form.submit();
        }
    });
    $('.full-height-scroll-detail').slimscroll({
        height: '500px'
    });
    $('#companyPictureDropzone').dropzone({
        url: '/',
        autoProcessQueue: false,
        addRemoveLinks: true,
        parallelUploads: 1000,
        uploadMultiple: false,
        acceptedFiles: allowedFileTypes,
        maxFiles: 1,
        maxFilesize: maxFileLength,
        dictDefaultMessage: dropzonemessage,
        dictRemoveFile: removefile,
        init: function () {
            companyPictureUpload = this;
            this.on("addedfile", function () {
                //  Allow maxFiles = 1
                if (this.files[1] != null) {
                    this.removeFile(this.files[1]);
                    showAlert("warning", Uploadedfilelimitmessage);
                    return false;
                }
                // Check for file type
                if (this.files[0] !== null) {
                    var type = this.files[0].type;
                    if (allowedFileTypes.indexOf(type) < 0) {
                        showAlert("warning", Uploadedfiletypemessage);
                        this.removeFile(this.files[0]);
                        return false;
                    }
                }
                // Check for max file size
                if (this.files[0] !== null) {
                    if (this.files[0].size > this.options.maxFilesize ) {
                        showAlert("warning", Uploadedfilemaxsizemessage);
                        this.removeFile(this.files[0]);
                        return false;
                    }
                }
            });
        },
        error: function (file, response) {
            file.previewElement.classList.add("dz-error");
        },
        sending: function (file, xhr, data) {
            data.append("id", companyPictureId);
        },
        success: function (file, response) {
            if (response.id <= 0) {
                showAlert("error", response.message);
                this.removeFile(file);
                return false;
            }
            else {
                companyPictureId = response.id;
                numSuccess++;
                SaveForm();
            }
        }
    });
});

function OnFailure(xhr, status, error) {
    showAjaxAlert(xhr);
}

function Filldetails() {
    $('#lblBusinessName').html($('#txtBusinessName').val());
    $('#lblLegalName').html($('#txtLegalName').val());
    $('#lblPhoneNumber').html($('#txtPhoneNumber').val());
    $('#lblWebSite').html($('#txtWebSite').val());
    $('#lblCompanySize').html($("#CompanySize").val());
    $('#lblCountry').html($("#CountryID option:selected").text());
    $('#lblCurrency').html($("#ddlCurrency option:selected").text());
    $('#lblModules').html($('.modulecheckbox:checkbox:checked').map(function (index) {
        return [$(this).next('label').text()] + ",";
    }).get());

    if ($('#lblModules')[0].innerHTML.slice(-1) == ',') {
        $('#lblModules')[0].innerHTML = $('#lblModules')[0].innerHTML.slice(0, -1);
    }
    $('#lblPrecision').html($('#txtPrecision').val());
    $('#lblUseLeapYear').html($('#chkUseLeapYear').prop('checked') == true ? YesMessage : NoMessage);
    $('#lblCompanyCode').html($('#txtCode').val());
}

function SaveForm() {
    $('#LogoID').val(companyPictureId);
    if (numSuccess == (companyPictureUpload.getAcceptedFiles().length)) {
        //Update Profile Image and Company Logo
        $.ajax({
            type: "GET",
            url: ROOTPath + '/Company/SaveCompanyImage',
            data: { companyLogoID: companyPictureId },
            success: function (data) {
                if (data == "success")
                    window.location.href = homeURL;
                else
                    showAlert("error", data);
            },
            error: function () {
                showAlert("error", response);
            }
        });
    }
}

function OnSuccess(response) {
    //If no image redirect to List of Applications
    if (companyPictureUpload.getAcceptedFiles().length == 0) {
        window.location.href = homeURL;
    }
    else {
        //Save uploaded Images
        var fURL = ROOTPath + "/File/UploadFile";
        if (companyPictureUpload.getAcceptedFiles().length > 0) {
            companyPictureUpload.options.url = fURL;
            companyPictureUpload.processQueue();
        }
    }
}