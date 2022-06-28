var initialDate;
var endDate;
var serviceName;
var serviceType = $("#ServiceID").val();
var CustomField1;
var CustomField2;
var CustomField3;
var CustomField4;
var CustomField5;
var CustomDateField;
var IsDateFieldCustomField1;
var ServiceTypeID;
var Period;
var FilingTypeID;

$(document).ready(function () {
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
        onInit: function (event, current) {
            if (serviceType == 1 || serviceType == 3)
                $('.actions > ul > li:first-child').addClass("disabled").attr("aria-disabled", "true");
        },
        onStepChanging: function (event, currentIndex, newIndex) {
            // Always allow going backward even if the current step contains invalid fields!
            if (currentIndex > newIndex) {
                return true;
            }
            var form = $(this);
            if (currentIndex === 0) {
                if (!$(".serviceGroup .active").length) {
                    if ($("li.current").length) {
                        $("li.current").addClass('error');
                    }
                    return false;
                }
            }

            if (currentIndex === 1) {
                if (!$(".serviceType .active").length) {
                    if ($("li.current").length) {
                        $("li.current").addClass('error');
                    }
                    return false;
                }
            }
            // Clean up if user went backward before
            if (currentIndex < newIndex) {
                // To remove error styles
                $(".body:eq(" + newIndex + ") label.error", form).remove();
                $(".body:eq(" + newIndex + ") .error", form).removeClass("error");

                if (newIndex === 1) {
                    if ($(".serviceType i.active").attr('id') != $("#SelectedServiceTypeId"))
                        GetServiceType();
                }
                if (newIndex === 2) {
                    if (!$("input[name='ServiceIDs']").length)
                        GetService();
                }
                if (newIndex === 3) {
                    var cnt = 'input[name=ServiceIDs]:checked';
                    $("#ServiceID").val($(cnt).val());
                    serviceName = $(cnt).next('label').text().trim();
                    initialDate = $(cnt).attr('data-InitialDate');
                    endDate = $(cnt).attr('data-EndDate');
                    CustomField1 = $(cnt).attr('data-customfield1');
                    CustomField2 = $(cnt).attr('data-customfield2');
                    CustomField3 = $(cnt).attr('data-customfield3');
                    CustomField4 = $(cnt).attr('data-customfield4');
                    CustomField5 = $(cnt).attr('data-customfield5');
                    CustomDateField = $(cnt).attr('data-customdatefield');
                    IsDateFieldCustomField1 = $(cnt).attr('data-isdatefieldcustomfield');
                    $("#IsDateFieldCustomField1").val(IsDateFieldCustomField1);
                    ServiceTypeID = $(cnt).attr('data-servicetypeid');
                    FilingTypeID = $(cnt).attr('data-FilingType');
                    Period = $(cnt).attr('data-period');
                    GetFiscalYear();
                    GetServiceException();
                    ShowHideDueDatesByFilingType($(cnt).attr('data-FilingType'));
                    if ($("#SelectedServiceTypeId").val() == 26 || ($("#SelectedServiceTypeId").val() == 16 && !$("#dvFilingDate").hasClass("hide"))) {
                        $("#FilingDueDate").attr('readonly', 'readonly');
                        $("#PaymentDueDate").attr('readonly', 'readonly');
                        $('#ddlProperty').prop('disabled', true);
                        $('#ddlProperty').select2();
                        $(".divLicenseAccountService").show();
                    }
                    else {
                        $("#FilingDueDate").removeAttr("readonly");
                        $("#PaymentDueDate").removeAttr("readonly");
                        $('#ddlProperty').prop('disabled', false);
                        $('#ddlProperty').select2();
                        $(".divLicenseAccountService").hide();
                        InitializeFlexibleDate();
                    }

                }
                if (newIndex === 4) {
                    GetSummery();
                }
            }
            // Disable validation on fields that are disabled or hidden.
            form.validate().settings.ignore = ":disabled,:hidden";
            // Start validation; Prevent going forward if false
            return form.valid();
        },
        onStepChanged: function (event, current, next) {
            if ((serviceType == 1 || serviceType == 3) && current <= 1) {
                $('.actions > ul > li:first-child').addClass("disabled").attr("aria-disabled", "true");
            } else {
                $('.actions > ul > li:first-child').removeClass("disabled").attr("aria-disabled", "false");
            }
        },
        onFinishing: function (event, currentIndex) {
            var form = $(this);
            // Disable validation on fields that are disabled.
            // At this point it's recommended to do an overall check (mean ignoring only disabled fields)
            form.validate().settings.ignore = ":disabled,:hidden";
            // Start validation; Prevent form submission if false
            return form.valid();
        },
        onFinished: function (event, currentIndex) {

            //24-feb-2020
            if (isFromRight == "1" && $("#AccountTypeID").val() == "6")//View Right-->Non Property Service
            {
                if ($("#GroupID").val() != "9") {
                    $("#AccountID").val($("#OwnerID").val());
                }
            }
            //


            $("#hfCustomDateField").val($("#CustomDateField").val());

            var form = $('#form');
            // Submit form input            
            var attr = $("#form").find(".actions li:last a").attr('disabled');
            if (!(typeof attr !== typeof undefined && attr !== false)) {
                form.submit();
                $("#form").find(".actions li:last a").attr('disabled', 'disabled');
            }
        }
    });
    $('.full-height-scroll-group').slimscroll({
        height: '250px'
    });
    if (serviceType == 1) {
        $(".serviceGroup i[id='" + 1 + "']").toggleClass("active");
        $("#form").steps("next");
    }
    if (serviceType == 3) {
        $(".serviceGroup i[id='" + 2 + "']").toggleClass("active");
        $("#form").steps("next");
    }
});

function InitializeFlexibleDate() {
    $('.filingDate').datepicker({
        defaultDate: new Date(),
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture
    });
}

$(document).on('click', ".actions > ul > li:first-child", function (e) {
    if ((serviceType == 1 || serviceType == 3) && $("#form").steps("getCurrentIndex") === 0)
        $("#form").steps("next");
});

function OnFailure(xhr, status, error) {
    $("#form").find(".actions li:last a").removeAttr('disabled');
    showAjaxAlert(xhr);
}

function OnSuccess(response, status, xhr) {
    if (response.status == false) {
        showAlert("error", response.message);
        $("#form").find(".actions li:last a").removeAttr('disabled');
    }
    else {
        window.location.href = ROOTPath + '/AccountService/View?accountServiceID=' + response.id;
    }
}

function selectServiceGroup(id) {
    $(".serviceGroup i").removeClass("active");
    $(".serviceGroup i[id='" + id + "']").toggleClass("active");
};

function selectServiceType(id) {
    $(".serviceType i").removeClass("active");
    $(".serviceType i[id='" + id + "']").toggleClass("active");
    $("#SelectedServiceTypeId").val($(".serviceType i.active").attr('id'));
    $("#divService").empty();
};

function GetServiceType() {
    $.ajax({
        url: ROOTPath + "/AccountService/GetServiceType?groupId=" + $(".serviceGroup i.active").attr('id'),
        success: function (data) {
            $("#divServiceType").empty();
            $("#divServiceType").html(data);
            $(".serviceType i[id='" + $("#SelectedServiceTypeId").val() + "']").addClass("active");
            $('.full-height-scroll-type').slimscroll({
                height: '300px'
            });
        }
    });
}

function GetService() {
    var AccountTypeId = getUrlVars()["accountTypeId"];
    $.ajax({
        url: ROOTPath + "/AccountService/GetService?serviceTypeId=" + $("#SelectedServiceTypeId").val() + "&groupId=" + $(".serviceGroup i.active").attr('id') + "&AccountTypeID=" + AccountTypeId,
        success: function (data) {
            $("#divService").empty();
            $("#divService").html(data);
            $('.full-height-scroll-service').slimscroll({
                height: '300px'
            });

            if (!$("input[name='ServiceIDs']").length)
                $('.actions > ul > li:first-child').next().attr('style', 'display:none');
        }
    });
}

function GetFiscalYear() {
    if ($('input[name=ServiceIDs]:checked').val() > 0 && $("#SelectedServiceId").val() != $('input[name=ServiceIDs]:checked').val()) {
        $.ajax({
            url: ROOTPath + "/AccountService/GetFiscalYear?serviceID=" + $('input[name=ServiceIDs]:checked').val() + "&accountID=" + $('#AccountID').val(),
            success: function (data) {
                $("#SelectedServiceId").val($('input[name=ServiceIDs]:checked').val());
                $(".initialDate").val('');
                $(".filingDate").val('');

                $("#ddlYear").empty();
                $.each(data, function (index, optiondata) {
                    $("#ddlYear").append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
                });

                //CO-884
                $("#ddlYear").val(parseInt((new Date).getFullYear())).trigger('change');
                //

                $("#ddlProperty").empty();
                $("#dvProperty").hide();

                $(".select2dropdown").select2({ width: '100%' });
                $.validator.unobtrusive.parse('#form');
                $("#ddlYear option:first").val("");

                if (CustomField1 != "" && CustomField1 != undefined) {
                    $(".divCustomField1").show();
                    //$(".lblCustomField1").html(CustomField1);
                    if (ServiceTypeID == 17 && FilingTypeID != 4) {
                        $(".lblCustomField1").html(CustomField1 + ' <span class="text-danger">*</span>')
                        $("#CustomField1").addClass("required");
                    }
                    else {
                        $(".lblCustomField1").html(CustomField1)
                        $("#CustomField1").removeClass("required");
                        $("#CustomField1").removeClass("input-validation-error");
                    }
                    if (IsDateFieldCustomField1)
                        $("#CustomField1").addClass("inputdatefield");
                    else
                        $("#CustomField1").removeClass("inputdatefield");
                }
                else {
                    $(".divCustomField1").hide();
                }
                if (CustomField2 != "" && CustomField2 != undefined) {
                    $(".divCustomField2").show();
                    if (ServiceTypeID == 17 && FilingTypeID != 4) {
                        $(".lblCustomField2").html(CustomField2 + ' <span class="text-danger">*</span>')
                        $("#CustomField2").addClass("required");
                    }
                    else {
                        $(".lblCustomField2").html(CustomField2)
                        $("#CustomField2").removeClass("required");
                        $("#CustomField2").removeClass("input-validation-error");
                    }
                    //$(".lblCustomField2").html(CustomField2);
                }
                else {
                    $(".divCustomField2").hide();
                }
                if (CustomField3 != "" && CustomField3 != undefined) {
                    $(".divCustomField3").show();
                    $(".lblCustomField3").html(CustomField3);
                }
                else {
                    $(".divCustomField3").hide();
                }
                if (CustomField4 != "" && CustomField4 != undefined) {
                    $(".divCustomField4").show();
                    $(".lblCustomField4").html(CustomField4);
                    $(".lblCustomField4").append(" <span class='text-danger'>*</span>");
                }
                else {
                    $("#CustomField4").removeClass("input-validation-error");
                    $(".divCustomField4").hide();
                }
                if (CustomField5 != "" && CustomField5 != undefined) {
                    $(".divCustomField5").show();
                    $(".lblCustomField5").html(CustomField5);
                    $(".lblCustomField5").append(" <span class='text-danger'>*</span>");
                }
                else {
                    $("#CustomField5").removeClass("input-validation-error");
                    $(".divCustomField5").hide();
                }
                if (CustomDateField != "" && CustomDateField != undefined) {
                    $(".divCustomDateField").show();
                    $(".lblCustomDateField").html(CustomDateField);
                }
                else {
                    $(".divCustomDateField").hide();
                }

                //CO-1238
                $('.inputdatefield').datepicker({
                    todayHighlight: true,
                    keyboardNavigation: false,
                    forceParse: false,
                    calendarWeeks: true,
                    autoclose: true,
                    format: __dateFormat,
                    language: __culture
                });
                //

                //Clear Custom Field 
                $(".clearinput").val(null);
                //

                $("#ddlServiceStartPeriod option:not(:first)").remove();
                $("#ddlServiceStartPeriod").removeClass("required");
                if (ServiceTypeID != "" && ServiceTypeID != undefined) {
                    var i;
                    for (i = 1; i <= Period; i++) {
                        $("#ddlServiceStartPeriod").append("<option value='" + i + "'>" + i + "</option>");
                    }
                    $(".select2dropdown").select2({ width: '100%' });
                    $.validator.unobtrusive.parse('#form');
                    $("#ddlServiceStartPeriod option:first").val("");
                    $("#ddlServiceStartPeriod").addClass("required");
                    $(".divServiceStartPeriod").show();
                }
                else {
                    $(".divServiceStartPeriod").hide();
                }
                $('.full-height-scroll-additional').slimscroll({
                    height: '300px'
                });
            }
        });
    }
}

function GetServiceException() {
    if ($('input[name=ServiceIDs]:checked').val() > 0 && $("#SelectedServiceId").val() != $('input[name=ServiceIDs]:checked').val()) {
        $.ajax({
            url: ROOTPath + "/AccountService/GetServiceException?serviceID=" + $('input[name=ServiceIDs]:checked').val(),
            success: function (data) {
                $("#ddlServiceException").empty();
                if (data.length > 1) {
                    $.each(data, function (index, optiondata) {
                        $("#ddlServiceException").append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
                    });
                    $("#dvException").removeClass("hide");
                    $(".select2dropdown").select2({ width: '100%' });
                    $.validator.unobtrusive.parse('#form');
                    $("#ddlServiceException option:first").val("");
                }
                else {
                    $("#dvException").addClass("hide");
                }
                $('.full-height-scroll-additional').slimscroll({
                    height: '300px'
                });
            }
        });
    }
}

function GetProperty() {

    if ($('input[name=ServiceIDs]:checked').val() > 0 && $("#ddlYear").val() > 0) {

        var _ownerID = $("#AccountTypeID").val() == 6 ? $("#OwnerID").val() : $("#AccountID").val();

        $.ajax({
            url: ROOTPath + "/AccountService/GetAccountPropertyRightByOwner",
            data: {
                ownerID: _ownerID,
                serviceID: $('input[name=ServiceIDs]:checked').val(),
                fiscalYearID: $("#ddlYear").val(),
                ID: $("#AccountServiceAccountPropertyID").val()
            },
            success: function (data) {
                $("#ddlProperty").empty();

                if (data.length > 1 && $("#SelectedServiceTypeId").val() != 21)  //CO-960 (Hide Right DropDown For CEMETERY Services)
                {
                    $.each(data, function (index, optiondata) {
                        $("#ddlProperty").append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
                    });
                    $("#dvProperty").show();
                    $(".select2dropdown").select2({ width: '100%' });
                    $.validator.unobtrusive.parse('#form');
                    $("#ddlProperty option:first").val("");
                }
                else {
                    $("#dvProperty").hide();
                }

                $('.full-height-scroll-additional').slimscroll({
                    height: '300px'
                });
            }
        }).then(function () {
            if ($("#SelectedServiceTypeId").val() == 26 || ($("#SelectedServiceTypeId").val() == 16 && !$("#dvFilingDate").hasClass("hide"))) {
                GetAccountServiceForTmibre();
            }
        });
    }
}


function GetAccountServiceForTmibre() {
    if ($('input[name=ServiceIDs]:checked').val() > 0) {
        var accID = $("#AccountTypeID").val() == 6 ? $("#OwnerID").val() : $("#AccountID").val();
        $.ajax({
            url: ROOTPath + "/AccountService/GetAccountServiceForTimbre?fiscalYearID=" + $('#ddlYear').val() + "&accountID=" + accID + "&ServiceID=" + $("#ServiceID").val() + "&accountPropertyID=" + $("#AccountServiceAccountPropertyID").val(),
            success: function (data) {
                $("#ddlLicenseAccountServiceId option:not(:first)").remove();
                $("#ddlLicenseAccountServiceId option:first").val("");
                $.each(data, function (index, optiondata) {
                    $("#ddlLicenseAccountServiceId").append("<option value='" + optiondata.ID + "' data-paymentduedate='" + optiondata.FormattedPaymentDueDate + "' data-fillingduedate='" + optiondata.FormattedFillingDueDate + "' data-accountpropertyid='" + optiondata.AccountPropertyID + "'>" + optiondata.ServiceName + "</option>");
                });

                $(".select2dropdown").select2({ width: '100%' });
                $.validator.unobtrusive.parse('#form');
                $("#ddlLicenseAccountServiceId option:first").val("");
            }
        });
    }
}

$(document).on("change", "#ddlLicenseAccountServiceId", function () {
    if ($(this).val() != "") {
        $('#FilingDueDate').val($("#ddlLicenseAccountServiceId option:selected").attr("data-fillingduedate"));
        $('#PaymentDueDate').val($("#ddlLicenseAccountServiceId option:selected").attr("data-paymentduedate"));
        $('#ddlProperty').val($("#ddlLicenseAccountServiceId option:selected").attr("data-accountpropertyid")).trigger('change');
    }
    else {
        $('#FilingDueDate').val('');
        $('#PaymentDueDate').val('');
    }
});

function ShowHideDueDatesByFilingType(filingTypeID) {
    if (filingTypeID == 2) {
        $("#dvFilingDate").removeClass("hide");
        $("#dvPaymentDueDate").removeClass("hide");
        $("#FilingDueDate").addClass("required");
        $("#PaymentDueDate").addClass("required");
    }
    else {
        $("#dvFilingDate").addClass("hide");
        $("#dvPaymentDueDate").addClass("hide");
        $("#FilingDueDate").removeClass("required");
        $("#PaymentDueDate").removeClass("required");
    }
}

function GetAdditionalInfo() {
    $("#CustomField4").removeClass("input-validation-error");
    $("#CustomField5").removeClass("input-validation-error");

    $(".initialDate").val('');
    $(".filingDate").val('');

    var _serviceGroupID = $(".serviceGroup i.active").attr('id');

    if ($('#ddlYear').val() > 0) {
        $.ajax({
            url: ROOTPath + "/AccountService/GetAdditionalInfo",
            data:
            {
                serviceID: $("#ServiceID").val(),
                fiscalyear: $('#ddlYear').val(),
                initialDate: initialDate,
                endDate: endDate,
                accountID: $("#AccountID").val(),
                serviceGroupID: _serviceGroupID
            },
            success: function (data) {
                if (data) {
                    var jsonData = JSON.parse(data);

                    $('#InitialDate').val(jsonData.InitialDate);
                    $('#EndDate').val(jsonData.EndDate);
                    $("#Year").val($('#ddlYear').val());

                    //
                    $(".clearinput").val(null);
                    $('#ddlServiceStartPeriod').val($("#ddlServiceStartPeriod option:first").val()).trigger('change');
                    $("#select2-ddlServiceStartPeriod-container").parent('span').removeClass('select2-validation-error');
                    //

                    if (_serviceGroupID == 1) {
                        $('#CustomField1').val(jsonData.CustomField1);
                        $('#CustomField2').val(jsonData.CustomField2);
                        $('#CustomField3').val(jsonData.CustomField3);
                        $('#CustomField4').val(jsonData.CustomField4);
                        $('#CustomField5').val(jsonData.CustomField5);
                        /* $('#CustomDateField').val(jsonData.CustomDateField);*/
                        $('#CustomDateField').datepicker('update', jsonData.CustomDateField);
                    }
                }

                if ($("#ServiceID").val() >= 9 && $("#ServiceID").val() <= 13) {
                    $(".divPreviousMeasure").show();
                    $("#PreviousMeasure").val(null);
                }
                else {
                    $(".divPreviousMeasure").hide();
                }
            }
        }).then(function () {
            GetProperty();
        });
    }
    else {
        $("#ddlProperty").empty();
        $("#ddlLicenseAccountServiceId").empty();
        $("#dvProperty").hide();
    }
}

function GetSummery() {
    $.ajax({
        url: ROOTPath + "/AccountService/GetSummary",
        success: function (data) {
            $("#divSummery").empty();
            $("#divSummery").html(data);
            $("#lblServiceGroup").html($(".serviceGroup i.active").attr('data-val'));
            $("#lblServiceType").html($(".serviceType i.active").attr('data-val'));
            $("#lblService").html(serviceName);
            $("#lblFiscalYear").html($("#ddlYear").val());
            $('#GroupID').val($(".serviceGroup i.active").attr('id'));
            if ($("#ddlServiceException option:selected").val() > 0) {
                $("#dvSummaryException").removeClass("hide");
                $("#lblServiceExceptionValue").html($("#ddlServiceException option:selected").text());
                $("#ServiceExceptionID").val($("#ddlServiceException option:selected").val());
                $("#ServiceExceptionValue").val($("#ddlServiceException option:selected").text());
            }
            else {
                $("#dvSummaryException").addClass("hide");
                $("#ServiceExceptionID").val("");
                $("#ServiceExceptionValue").val("");
            }
            if ($("#ddlProperty option:selected").val() > 0) {
                $("#dvSummaryProperty").removeClass("hide");
                $("#lblPropertyValue").html($("#ddlProperty option:selected").text());
                $("#AccountPropertyID").val($("#ddlProperty option:selected").val());
                $("#Property").val($("#ddlProperty option:selected").text());
            }
            else {
                $("#dvSummaryProperty").addClass("hide");
                $("#AccountPropertyID").val("");
                $("#Property").val("");
            }
            if (CustomField1 != "" && CustomField1 != undefined) {
                $(".divCustomField1").show();
                //$(".lblCustomField1").html(CustomField1);
                if (ServiceTypeID == 17 && FilingTypeID != 4) {
                    if ($("#CustomField1").val() == '')
                        $(".lblCustomField1").html(CustomField1 + ' <span class="text-danger">*</span>')
                    else
                        $(".lblCustomField1").html(CustomField1)
                    $("#CustomField1").addClass("required");
                }
                else {
                    $(".lblCustomField1").html(CustomField1)
                    $("#CustomField1").removeClass("required");
                    $("#CustomField1").removeClass("input-validation-error");
                }
                $("#lblCustomField1Value").html($("#CustomField1").val());
            }
            else {
                $(".divCustomField1").hide();
            }
            if (CustomField2 != "" && CustomField2 != undefined) {
                $(".divCustomField2").show();
                //$(".lblCustomField2").html(CustomField2);
                if (ServiceTypeID == 17 && FilingTypeID != 4) {
                    if ($("#CustomField2").val() == '')
                        $(".lblCustomField2").html(CustomField2 + ' <span class="text-danger">*</span>')
                    else
                        $(".lblCustomField2").html(CustomField2)
                    $("#CustomField2").addClass("required");
                }
                else {
                    $(".lblCustomField2").html(CustomField2)
                    $("#CustomField2").removeClass("required");
                    $("#CustomField2").removeClass("input-validation-error");
                }
                $("#lblCustomField2Value").html($("#CustomField2").val());
            }
            else {
                $(".divCustomField2").hide();
            }
            if (CustomField3 != "" && CustomField3 != undefined) {
                $(".divCustomField3").show();
                $(".lblCustomField3").html(CustomField3);
                $("#lblCustomField3Value").html($("#CustomField3").val());
            }
            else {
                $(".divCustomField3").hide();
            }
            if (CustomField4 != "" && CustomField4 != undefined) {
                $(".divCustomField4").show();
                $(".lblCustomField4").html(CustomField4);
                $(".lblCustomField4").append(" <span class='text-danger'>*</span>");
                $(".summaryCustomField4").html(CustomField4);
                $("#lblCustomField4Value").html($("#CustomField4").val());
            }
            else {
                $("#CustomField4").removeClass("input-validation-error");
                $(".divCustomField4").hide();
            }
            if (CustomField5 != "" && CustomField5 != undefined) {
                $(".divCustomField5").show();
                $(".lblCustomField5").html(CustomField5);
                $(".lblCustomField5").append(" <span class='text-danger'>*</span>");
                $(".summaryCustomField5").html(CustomField5);
                $("#lblCustomField5Value").html($("#CustomField5").val());
            }
            else {
                $("#CustomField5").removeClass("input-validation-error");
                $(".divCustomField5").hide();
            }
            if (CustomDateField != "" && CustomDateField != undefined) {
                $(".divCustomDateField").show();
                $(".lblCustomDateField").html(CustomDateField);
                $("#lblCustomDateFieldValue").html($("#CustomDateField").val());
            }
            else {
                $(".divCustomDateField").hide();
            }

            if ($("#ServiceID").val() >= 9 && $("#ServiceID").val() <= 13) {
                $("#dvSummaryPreviousMeasure").removeClass("hide");
                $("#lblPreviousMeasureValue").html($("#PreviousMeasure").val());
            }
            else {
                $("#dvSummaryPreviousMeasure").addClass("hide");
            }

            if ($("#ddlServiceStartPeriod option:selected").val() > 0) {
                $("#divServiceStartPeriod").removeClass("hide");
                $("#lblServiceStartPeriod").html($("#ddlServiceStartPeriod option:selected").text());
                $("#ServiceStartPeriod").val($("#ddlServiceStartPeriod option:selected").val());
            }
            else {
                $("#divServiceStartPeriod").addClass("hide");
                $("#ServiceStartPeriod").val("");
            }
            if ($("#SelectedServiceTypeId").val() == 26 || ($("#SelectedServiceTypeId").val() == 16 && !$("#dvFilingDate").hasClass("hide"))) {
                $("#divLicenseAccountService").show();
                $("#lblLicenseAccountService").html($("#ddlLicenseAccountServiceId option:selected").text());
                $("#LicenseAccountServiceID").val($("#ddlLicenseAccountServiceId option:selected").val());
            }
            else {
                $("#divLicenseAccountService").hide();
                $("#lblLicenseAccountService").val("");
            }
        }
    });
}

function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}