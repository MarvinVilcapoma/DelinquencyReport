var isValid = false;
var validationStep = false;
$(document).ready(function () {
    $("#frmImportAccountServiceFiling").steps({
        bodyTag: "fieldset",
        enableAllSteps: false,
        enableCancelButton: false,
        labels: {
            next: nextText,
            previous: previousText,
            cancel: cancelText,
            finish: finishText
        },
        onStepChanging: function (event, currentIndex, newIndex) {
            //ALWAYS ALLOW GOING BACKWARD EVEN IF THE CURRENT STEP CONTAINS INVALID FIELDS!           
            if (currentIndex > newIndex) {                
                return true;
            }
            var form = $(this);
            var $Retval = false;
            validationStep = false;

            //SUBMIT FORM TO UPLOAD FILE
            var formdata;

            //STEP #1 Upload File
            if (newIndex === 1) {
                if (!form.valid())
                    return false;

                //SUBMIT FORM INPUT
                formdata = new FormData();

                var file_data = $('input[type="file"]')[0].files[0];
                formdata.append("UploadFile", file_data);

                var other_data = $('form .divFileUpload').serializeArray();
                $.each(other_data, function (key, input) {
                    formdata.append(input.name, input.value);
                });
                $.ajax({
                    async: false,
                    url: ROOTPath + '/AccountService/UploadFile',
                    data: formdata,
                    contentType: false,
                    processData: false,
                    type: 'POST',
                    success: function (response) {
                        if (!response.status) {
                            showAlert("error", response.message);
                            isValid = false;
                        }
                        else {
                            isValid = true;
                            if (response.nextStepData != undefined && response.nextStepData != null && response.nextStepData != "") {
                                $(".divMapping").html(response.nextStepData);
                                InitSelect2();
                                reInitvalidator(form);
                            }
                            $Retval = true;
                        }

                    }
                });
            }
            //STEP #2 Mapping
            else if (newIndex === 2) {
                if (!form.valid())
                    return false;
                // Submit form input
                formdata = new FormData();

                var other_data = $('form .divMapping').serializeArray();
                $.each(other_data, function (key, input) {
                    formdata.append(input.name, input.value);
                });
                $.ajax({
                    async: false,
                    url: ROOTPath + '/AccountService/ColumnMapping',
                    data: formdata,
                    contentType: false,
                    processData: false,
                    type: 'POST',
                    success: function (response) {
                        if (!response.status) {
                            showAlert("warning", response.message);
                            isValid = false;
                        }
                        else {
                            isValid = true;
                            if (response.nextStepData != undefined && response.nextStepData != null && response.nextStepData != "") {
                                $(".divValidation").html(response.nextStepData);
                                $('.tblImportError').footable();                                
                                validationStep = true;
                            }
                            $Retval = true;
                        }
                    }
                });
            }
            //STEP #3 Validation
            else if (newIndex === 3) {
                if (!form.valid() || $("#IsValidFile").val() == 'False' || parseInt($('#ValidOk').val()) > 0)
                    return false;

                // Submit form input
                formdata = new FormData();
                var mapped_data = $('form .divMapping').serializeArray();
                $.each(mapped_data, function (key, input) {
                    formdata.append(input.name, input.value);
                });
                $.ajax({
                    async: false,
                    url: ROOTPath + '/AccountService/ImportValidation',
                    data: formdata,
                    contentType: false,
                    processData: false,
                    type: 'POST',
                    success: function (response) {
                        if (!response.status) {
                            showAlert("warning", response.message);
                            isValid = false;
                        }
                        else {
                            isValid = true;
                            if (response.nextStepData != undefined && response.nextStepData != null && response.nextStepData != "") {
                                $(".divFinish").html(response.nextStepData);
                            }
                            $Retval = true;
                        }
                    }
                });
            }

            // Clean up if user went backward before
            if (currentIndex < newIndex) {
                // To remove error styles
                $(".body:eq(" + newIndex + ") label.error", form).remove();
                $(".body:eq(" + newIndex + ") .error", form).removeClass("error");
            }

            // Disable validation on fields that are disabled or hidden.
            form.validate().settings.ignore = ":disabled,:hidden";

            return $Retval;
        },
        onStepChanged: function (event, currentIndex, priorIndex) {
            if (validationStep) {
                $(this).steps("next");
            }

        },
        onCanceled: function (event) { },
        onFinishing: function (event, currentIndex) {
            var form = $(this);
            //Disable validation on fields that are disabled.
            //At this point it's recommended to do an overall check (mean ignoring only disabled fields)
            form.validate().settings.ignore = ":disabled";

            //Start validation; Prevent form submission if false
            return form.valid();
        },
        onFinished: function (event, currentIndex) {
            var form = $(this);
            //SUBMIT FORM INPUT
            formdata = new FormData();
            var mapped_data = $('form .divMapping').serializeArray();
            $.each(mapped_data, function (key, input) {
                formdata.append(input.name, input.value);
            });
            showAlert('info', ImportFillingProcessInfoMsg, 0);
            $.ajax({
                //async: false,
                url: ROOTPath + '/AccountService/FinishImport',
                data: formdata,
                contentType: false,
                processData: false,
                type: 'POST',
                success: function (response) {
                    if (!response.status) {
                        showAlert("error", response.message);
                        isValid = false;
                    }
                    else {
                        isValid = true;
                        window.location.href = ROOTPath + "/AccountService/List";
                    }
                }
            });
        }
    });
});
function InitSelect2() {
    $(".select2dropdown").select2({ width: '100%' });
}
function reInitvalidator(form) {
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
}
