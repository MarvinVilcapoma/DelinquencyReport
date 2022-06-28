var isValid = false;
is_async_step = false;

$(document).ready(function () {
    $("#frmImportBankPayments").steps({
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

            //SUBMIT FORM TO UPLOAD FILE
            var formdata;

            //USED TO SEND USER TO NEXT STEP IS ASYNC REQUEST IS PRESENT - FOR AJAX CALL 
            if (is_async_step) {
                is_async_step = false;

                //ALLOW NEXT STEP
                return true;
            }

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

                showAlert('info', importBankPaymentProcessInfoMsg, 0);

                $.ajax({
                    url: ROOTPath + '/Payment/UploadFile',
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
                                $(".divValidation").html(response.nextStepData);
                                reInitvalidator(form);
                                InitFooTable($('#tblValidationResult'));
                            }

                            $(".toast-info").hide();

                            is_async_step = true;
                            //TRIGGER NAVIGATION EVENT
                            $(form).steps("next");

                        }

                    }
                });
            }
            //STEP #2 Validation
            else if (newIndex === 2) {

                if (!form.valid() || $("#IsValidFile").val() == 'False')
                    return false;

                // Submit form input
                formdata = new FormData();
                var mapped_data = $('form .divValidation').serializeArray();
                $.each(mapped_data, function (key, input) {
                    formdata.append(input.name, input.value);
                });

                showAlert('info', importBankPaymentProcessInfoMsg, 0);

                $.ajax({
                    url: ROOTPath + '/Payment/ImportValidation',
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
                                InitFooTable($('#tblFinish'));
                            }

                            $(".toast-info").hide();

                            is_async_step = true;
                            //TRIGGER NAVIGATION EVENT
                            $(form).steps("next");
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

            return false;
        },
        onStepChanged: function (event, currentIndex, priorIndex) {

            if (isValid)
                return true;

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
            var mapped_data = $('form .divValidation').serializeArray();
            $.each(mapped_data, function (key, input) {
                formdata.append(input.name, input.value);
            });

            showAlert('info', importBankPaymentProcessInfoMsg, 0);

            $.ajax({
                url: ROOTPath + '/Payment/FinishImport',
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
                        $(".toast-info").hide();
                        isValid = true;
                        window.location.href = ROOTPath + "/Collections/Payment/Index";
                    }
                }
            });
        }
    }).validate({
        errorPlacement: function (error, element) {
            element.before(error);
        }
    });

    InitSelect2();
    InitializeDate();

    $("#btnClose").click(function () {
        window.location.href = ROOTPath + "/Collections/Payment/Index";
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

function InitFooTable(table) {
    table.footable({
        "sorting": {
            "enabled": false
        },
        "paging": {
            "enabled": true,
            "position": "right",
            "size": 20
        }
    });
}

function InitializeDate() {
    $('.importDate').datepicker({
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture,
    });

    $('.importDate').datepicker('update', new Date());
}