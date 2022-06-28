var isValid = false, is_async_step = false;

$(document).ready(function () {

    $("#frmImportMeasuredWaterFiling").steps({
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
                //if (!isValid) return false;
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

                var other_data = $('.divFileUpload *').serializeArray();
                $.each(other_data, function (key, input) {
                    formdata.append(input.name, input.value);
                });

                // 8-Feb-2020
                showAlert('info', importMeasuredWaterFillingProcessInfoMsg, 0);
                //

                $.ajax({
                    //async: false,
                    url: ROOTPath + '/AccountService/UploadMeasuredWaterFile',
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
                                //InitSelect2();
                                reInitvalidator(form);
                                InitFooTable($('#tblValidationResult'));
                            }

                            // 8-Feb-2020                           
                            $(".toast-info").hide();
                            //

                            is_async_step = true;
                            //TRIGGER NAVIGATION EVENT
                            $(form).steps("next");
                        }
                    }
                });
            }
            //STEP #2 Validation
            else if (newIndex === 2) {

                if (!form.valid() || $("#IsValidFile").val() == 'False' || parseInt($('#ValidOk').val()) > 0)
                    return false;

                // Submit form input
                formdata = new FormData();

                var other_data = $('.divValidation *').serializeArray();
                $.each(other_data, function (key, input) {
                    formdata.append(input.name, input.value);
                });

                // 8-Feb-2020
                showAlert('info', importMeasuredWaterFillingProcessInfoMsg, 0);
                //

                $.ajax({
                    // async: false,
                    url: ROOTPath + '/AccountService/ImportMeasuredWaterValidation',
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

                            // 8-Feb-2020                           
                            $(".toast-info").hide();
                            //

                            is_async_step = true;
                            //TRIGGER NAVIGATION EVENT
                            $(form).steps("next");
                        }
                    }
                });
            }

            //CLEAN UP IF USER WENT BACKWARD BEFORE
            if (currentIndex < newIndex) {
                //TO REMOVE ERROR STYLES
                $(".body:eq(" + newIndex + ") label.error", form).remove();
                $(".body:eq(" + newIndex + ") .error", form).removeClass("error");
            }

            //DISABLE VALIDATION ON FIELDS THAT ARE DISABLED OR HIDDEN.
            form.validate().settings.ignore = ":disabled,:hidden";

            //START VALIDATION; PREVENT GOING FORWARD IF FALSE
            //if (isValid)
            //    return form.valid();
            //FORBID THE NEXT STEP BECAUSE $.POST IS AN ASYNCHRONOUS FUNCTION
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


            var mapped_data = $('.divValidation *').serializeArray();
            //var mapped_data = $('#frmImportMeasuredWaterFiling').serializeArray();
            $.each(mapped_data, function (key, input) {
                formdata.append(input.name, input.value);
            });

            // 8-Feb-2020
            showAlert('info', importMeasuredWaterFillingProcessInfoMsg, 0);
            //

            $.ajax({
                ///async: false,
                url: ROOTPath + '/AccountService/FinishMeasuredWaterImport',
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

                        // 8-Feb-2020                           
                        $(".toast-info").hide();
                        //

                        isValid = true;
                        window.location.href = ROOTPath + "/Accounts/AccountService/List";
                    }
                }
            });
        }
    }).validate({
        errorPlacement: function (error, element) {
            element.before(error);
        }
    });

    //InitSelect2();
    InitializeDate();

    $("#btnClose").click(function () {
        window.location.href = ROOTPath + "/Accounts/AccountService/List";
    });
});

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
    $('.periodDate').datepicker({
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: "MM yyyy",
        language: __culture
    });

    $('.periodDate').datepicker('update', new Date());
}