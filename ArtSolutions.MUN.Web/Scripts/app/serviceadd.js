/*********[START] - Global Variable Declaration ******************/
var collectionRule = {};
var collectionRuleList = [];
var ruleDetail = {};
var ruleDetailList = [];
var ruleSeq = 1, ruledetailSeq = 1;
var defaultFromAmount = 0;
var defaultToAmount = 9999999999999;
var maxRangeAmount = 9999999999999;
var calculationDateList = [];
var calculationDate = {};
var service = {};
var isDatesFilled = false;
var maxRuleDetailRange = 0;
var exceptionModel = {};
var exceptionList = [];
var exceptionSeqId = 1;
var _ruleID = 1;

/*******************[START] - On DOM Ready Functions  *******************************/
$(window).on("load", function () {
    $('#txtServiceName').focus();
});

$(document).ready(function () {
    $("#txtServiceName").focus();
    initializeDate('startdate', "MM dd", true);
    initializeDate('effectivedate', __dateFormat, false);
    extendStringPrototype();
    $("#txtServiceName").keyup(function () { return false; });
    $("#txtServiceName").blur(function () { return false; });

    if ($("#ChkCustomField1").is(":checked")) {
        $("#CustomField1").removeAttr("disabled");
        $("#chkAllowDuplicateCustomField1").removeAttr("disabled");
    }
    else {
        $("#CustomField1").attr("disabled", "disabled");
        $("#chkAllowDuplicateCustomField1").attr("disabled", "disabled");
    }

    if ($("#ChkCustomField2").is(":checked")) {
        $("#CustomField2").removeAttr("disabled");
        $("#chkAllowDuplicateCustomField2").removeAttr("disabled");
    }
    else {
        $("#CustomField2").attr("disabled", "disabled");
        $("#chkAllowDuplicateCustomField2").attr("disabled", "disabled");
    }

    if ($("#ChkCustomField3").is(":checked")) {
        $("#CustomField3").removeAttr("disabled");
        $("#chkAllowDuplicateCustomField3").removeAttr("disabled");
    }
    else {
        $("#CustomField3").attr("disabled", "disabled");
        $("#chkAllowDuplicateCustomField3").attr("disabled", "disabled");
    }

    if ($("#ChkCustomField4").is(":checked")) {
        $("#CustomField4").removeAttr("disabled");
        $("#chkAllowDuplicateCustomField4").removeAttr("disabled");
    }
    else {
        $("#CustomField4").attr("disabled", "disabled");
        $("#chkAllowDuplicateCustomField4").attr("disabled", "disabled");
    }

    if ($("#ChkCustomField5").is(":checked")) {
        $("#CustomField5").removeAttr("disabled");
        $("#chkAllowDuplicateCustomField5").removeAttr("disabled");
    }
    else {
        $("#CustomField5").attr("disabled", "disabled");
        $("#chkAllowDuplicateCustomField5").attr("disabled", "disabled");
    }

    if ($("#ChkCustomDateField").is(":checked")) {
        $("#CustomDateField").removeAttr("disabled");
        $("#chkAllowDuplicateCustomDateField").removeAttr("disabled");
    }
    else {
        $("#CustomDateField").attr("disabled", "disabled");
        $("#chkAllowDuplicateCustomDateField").attr("disabled", "disabled");
    }

    $("#ChkCustomField1").on("click", function () {
        if ($(this).is(":checked")) {
            $("#CustomField1").removeAttr("disabled");
            $("#chkAllowDuplicateCustomField1").removeAttr("disabled");
        }
        else {
            $("#CustomField1").attr("disabled", "disabled");
            $("#chkAllowDuplicateCustomField1").attr("disabled", "disabled");
        }
    });

    $("#ChkCustomField2").on("click", function () {
        if ($(this).is(":checked")) {
            $("#CustomField2").removeAttr("disabled");
            $("#chkAllowDuplicateCustomField2").removeAttr("disabled");
        }
        else {
            $("#CustomField2").attr("disabled", "disabled");
            $("#chkAllowDuplicateCustomField2").attr("disabled", "disabled");
        }
    });

    $("#ChkCustomField3").on("click", function () {
        if ($(this).is(":checked")) {
            $("#CustomField3").removeAttr("disabled");
            $("#chkAllowDuplicateCustomField3").removeAttr("disabled");
        }
        else {
            $("#CustomField3").attr("disabled", "disabled");
            $("#chkAllowDuplicateCustomField3").attr("disabled", "disabled");
        }
    });

    $("#ChkCustomField4").on("click", function () {
        if ($(this).is(":checked")) {
            $("#CustomField4").removeAttr("disabled");
            $("#chkAllowDuplicateCustomField4").removeAttr("disabled");
        }
        else {
            $("#CustomField4").attr("disabled", "disabled");
            $("#chkAllowDuplicateCustomField4").attr("disabled", "disabled");
        }
    });

    $("#ChkCustomField5").on("click", function () {
        if ($(this).is(":checked")) {
            $("#CustomField5").removeAttr("disabled");
            $("#chkAllowDuplicateCustomField5").removeAttr("disabled");
        }
        else {
            $("#CustomField5").attr("disabled", "disabled");
            $("#chkAllowDuplicateCustomField5").attr("disabled", "disabled");
        }
    });

    $("#ChkCustomDateField").on("click", function () {
        if ($(this).is(":checked")) {
            $("#CustomDateField").removeAttr("disabled");
            $("#chkAllowDuplicateCustomDateField").removeAttr("disabled");
        }
        else {
            $("#CustomDateField").attr("disabled", "disabled");
            $("#chkAllowDuplicateCustomDateField").attr("disabled", "disabled");
        }
    });
});
$(function () {
    $(".select2dropdown").select2({ width: '100%' });
});
/*******************[START] - Utility Functions  *******************************/
function initializeDate(className, dateFormat, setcurrentDate) {

    $('.' + className).datepicker({
        defaultDate: new Date(),
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: dateFormat,
        language: __culture
    }).on("change", function (e) {
        if ($(this).attr("id") == "txtStartDate") {
            var startDate = $(this).datepicker('getDate');
            startDate.setMonth(startDate.getMonth() + 12);
            startDate.setDate(startDate.getDate() - 1);
            $("#txtEndDate").datepicker({ format: dateFormat, language: __culture });
            $('#txtEndDate').datepicker('update', startDate);
        }
        confirmServiceInputChange(e, this);
    });
    if (setcurrentDate)
        $('.' + className).datepicker('update', new Date());
    else {
        $('.' + className).datepicker('clearDates');
    }
}
function initializePopupDate(className, dateFormat) {

    $('.' + className).datepicker({
        defaultDate: new Date(),
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: dateFormat,
        language: __culture
    });
}
function extendStringPrototype() {
    String.prototype.format = function () {
        var formatted = this;
        for (var arg in arguments) {
            formatted = formatted.replace("{" + arg + "}", arguments[arg]);
        }
        return formatted;
    };
}
function findIndex(arr, key, value) {
    for (var i = 0; i < arr.length; i++) {

        if (arr[i][key] == value) {
            return i;
            break;
        }
    }
    return -1;
}
/*******************[START] - Service Page Functions  *******************************/
$("#btnSave").click(function (event) {
    if (Save() == false)
        return false;
});
$("#btnSaveNew").click(function (event) {
    if (Save() == false)
        return false;
});
function Save() {
    if ($("#form").valid()) {
        if (validateForm()) {
            ClearFilingDateByFilingType();
            $("#ServiceCollectionRuleListJson").val(JSON.stringify(collectionRuleList));
            $("#ServiceCalculationDateListJson").val(JSON.stringify(calculationDateList));
            $("#txtEndDate").attr("disabled", false);
            $($("#hdnFormattedStartDate")).val(new Date($("#txtStartDate").datepicker('getDate')).toDateString());
            $($("#hdnFormattedEndDate")).val(new Date($("#txtEndDate").datepicker('getDate')).toDateString());
            $("#ServiceExceptionListJSON").val(JSON.stringify(exceptionList));
            var ListAccountType;

            $.each($("#AccountType").select2('data'), function (index, item) {
                if (index !== 0) {
                    ListAccountType = ListAccountType + "," + item.id;
                }
                else {
                    ListAccountType = item.id;
                }
            });
            $("#AccountTypeIDs").val(ListAccountType);

            return true;
        }
        else
            return false;
    }
    else
        return false;
}

function ClearFilingDateByFilingType() {
    if ($('input[name=FilingTypeID]:checked').val() != 3 && //Fix Days/Dates
        $('input[name=FilingTypeID]:checked').val() != 4 && //Auto Filing // Updated
        $('input[name=FilingTypeID]:checked').val() != 5 //Auto Filing with Edit  
    ) {
        $(".duedays").val(null);
        calculationDateList = [];
    }
}

function validateForm() {
    var filingTypeID = $('input[name=FilingTypeID]:checked').val();

    if (filingTypeID == 3 && $("#FillingDueDays").val() == '') {
        showAlert("error", $("#FillingDueDays").attr("data-val-msg")); return false;
    }
    else if (filingTypeID == 3 && $("#PaymentDueDays").val() == '') {
        showAlert("error", $("#PaymentDueDays").attr("data-val-msg")); return false;
    }
    else if (findIndex(collectionRuleList, "IsActive", 1) == -1) {
        showAlert("error", $("#ServiceCollectionRuleListJson").attr("data-val-msg")); return false;
    }
    return true;
}

function ServiceSuccessCallback(response) {
    if (response.status == false) {
        $("#txtEndDate").attr("disabled", true);
        showAlert("error", response.message);
    }
    else {
        resolveRedirectURL(response.actionType);
    }
}

function resolveRedirectURL(actionType) {
    if (actionType == 1 || actionType == 3) // Save OR Cancel
    {
        window.location = ROOTPath + "/Service/List";
    }
    else if (actionType == 2)// Save & Add New
    {
        window.location = ROOTPath + "/Service/Add";
    }
}

function GetTypesByGroup() {
    var grpId = $('#ddlGroup').val();
    $("#ddlServiceType").empty();
    $("#ddlServiceType").append("<option value='0'>" + DropDownSelectMessage + "</option>");
    if (grpId != '') {
        $.ajax({
            url: ROOTPath + "/Service/GetServiceTypesByGroup?groupId=" + $('#ddlGroup').val(),
            success: function (data) {
                $.each(data, function (index, optiondata) {
                    $("#ddlServiceType").append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
                });
            }
        });
    }
    $('#ddlServiceType').val($("#ddlServiceType option:first").val()).trigger('change');
    $("#ddlServiceType option:first").val("");
}

$("#ddlFrequency").on('change', function (e) {
    confirmServiceInputChange(e, this);
});

function EnableDisableFilingDatesByFilingType(filingTypeID) {
    if (filingTypeID == 3) {
        $('#fillingdueon').css({ "pointer-events": "" });
        $('#paymentdueon').css({ "pointer-events": "" });
        $('#discounton').css({ "pointer-events": "" });
        $('#paymentgraceperiod').css({ "pointer-events": "" });
        $('#fillingdueon').removeClass("disabled");
        $('#paymentdueon').removeClass("disabled");
        $('#discounton').removeClass("disabled");
        $('#paymentgraceperiod').removeClass("disabled");
    }
    else {
        $('#fillingdueon').css({ "pointer-events": "none" });
        $('#paymentdueon').css({ "pointer-events": "none" });
        $('#discounton').css({ "pointer-events": "none" });
        $('#paymentgraceperiod').css({ "pointer-events": "none" });
        $('#fillingdueon').addClass("disabled");
        $('#paymentdueon').addClass("disabled");
        $('#discounton').addClass("disabled");
        $('#paymentgraceperiod').addClass("disabled");
    }

    if (filingTypeID == 4 || filingTypeID == 5) { // Updated
        $('#discounton').css({ "pointer-events": "" });
        $('#paymentgraceperiod').css({ "pointer-events": "" });
        $('#discounton').addClass("disabled");
        $('#paymentgraceperiod').addClass("disabled");
    }
    if (filingTypeID == 3 || filingTypeID == 2 || filingTypeID == 5) {// Updated
        $("#ddlfilingForm").attr("disabled", false);
        $("#ddlfilingForm").addClass("required");
    }
    else {
        $("#ddlfilingForm").val("").trigger("change");
        $("#ddlfilingForm").attr("disabled", true);
        $("#ddlfilingForm").removeClass("required input-validation-error");
        $("#ddlfilingForm").closest(".col-lg-8 ").find(".select2-selection").removeClass("select2-validation-error");

    }
};

/*******************[START] - POPUP - DueDates Functions  *******************************/
function loadCalculationDatesPopup(datetype, popupTitle) {
    unbindFieldValidation("ddltemplate");
    if ($("#form").valid()) {
        bindFieldValidation("ddltemplate");
        setServiceValues();
        if (validateDates()) {
            var DueType = "";
            if (datetype == 0) {
                DueType = $("#FilingDueType").val();
                service.FilingDueType = DueType;
            }
            else if (datetype == 1) {
                DueType = $("#PaymentDueType").val();
                service.PaymentDueType = DueType;
            }
            else if (datetype == 2) {
                DueType = $("#DiscountDueType").val();
                service.DiscountDueType = DueType;
            }
            else if (datetype == 3) {
                DueType = $("#PaymentGraceType").val();
                service.PaymentGraceType = DueType;
            }
            $.ajax({
                type: "POST",
                //async: true,
                url: ROOTPath + "/Service/AddCalculationDates",
                data: { 'service': service, 'dateType': datetype },
                success: function (response) {
                    if (response.status) {

                        $("#dvCalculationDates").html(response.returnData);
                        $.validator.unobtrusive.parse('#frmCalDates');
                        $(".popupinputcaldate").each(function () {
                            $(this).datepicker({
                                defaultViewDate: new Date($(this).attr("data-val-startdate")),
                                todayHighlight: true,
                                keyboardNavigation: false,
                                forceParse: false,
                                calendarWeeks: true,
                                autoclose: true,
                                format: __dateFormat,
                                language: __culture
                            });
                        });

                        initializeCalculationDatesList(response.datesList, DueType, datetype);
                        $("#txtDays").focus();
                        $("input[data-duetype='" + DueType + "']").prop("checked", true);
                        disableUncheckedInput(datetype);
                        if (datetype == 2 && DueType == 1) {
                            $("#txtStartDiscountPercentage").val(GlobalFormat(calculationDateList[0].DiscountPercentage));
                        }
                        else if (datetype == 2 && DueType == 2) {
                            $("#txtEndDiscountPercentage").val(GlobalFormat(calculationDateList[0].DiscountPercentage));
                        }
                        $("#datesmodal").modal("show");
                        $("#titleId").html(popupTitle);
                        return true;
                    }
                    else {
                        showAlert("error", response.message);
                        return false;
                    }
                }
            });
        }
    }
    bindFieldValidation("ddltemplate");
    return false;
}

function setServiceValues() {
    service.ID = $("#Id").val();
    service.EffectiveFrom = new Date($("#txtEffectiveFrom").datepicker('getDate')).toDateString();
    service.EffectiveTo = new Date($("#txtEffectiveTo").datepicker('getDate')).toDateString();
    service.StartDate = new Date($("#txtStartDate").datepicker('getDate')).toDateString();
    service.EndDate = new Date($("#txtEndDate").datepicker('getDate')).toDateString();
    service.FrequencyID = $("#ddlFrequency").val();
    service.UseFixedFillingDueDate = $("#UseFixedFillingDueDate").val();
    service.FillingDueDays = $("#FillingDueDays").val();
    service.PaymentDueDays = $("#PaymentDueDays").val();
    service.UseFixedPaymentDueDate = $("#UseFixedPaymentDueDate").val();
    service.DiscountDueDays = $("#DiscountDueDays").val();
    service.DiscountPercentage = $("#DiscountPercentage").val();
    service.UseFixedDiscountDate = $("#UseFixedDiscountDate").val();
    service.PaymentGracePeriod = $("#PaymentGracePeriod").val();
    service.UseFixedPaymentGracePeriod = $("#UseFixedPaymentGracePeriod").val();
    if (!isDatesFilled)
        calculationDateList = [];
    service.ServiceCalculationDateList = calculationDateList;
}
function validateDates() {
    var _effFrom = new Date(service.EffectiveFrom);
    _effFrom.setDate(_effFrom.getDate());
    _effFrom.setFullYear(_effFrom.getFullYear() + 1);
    _effFrom.setDate(_effFrom.getDate() - 1);
    if (new Date(service.EffectiveTo) < _effFrom) {
        showAlert("error", CompareEffectiveDatesValidationMsg);
        return false;
    }
    return true;
}

function initializeCalculationDatesList(datesList, DueType, datetype) {
    calculationDateList = [];
    $.each(datesList, function (key, value) {
        calculationDate = {};
        calculationDate.FiscalYearID = datesList[key].FiscalYearID;
        calculationDate.StartPeriodID = datesList[key].StartPeriodID;
        calculationDate.EndPeriodID = datesList[key].EndPeriodID;
        calculationDate.SequenceID = datesList[key].SequenceID;
        calculationDate.FillingDueDate = datesList[key].FillingDueDate;
        calculationDate.PaymentDueDate = datesList[key].PaymentDueDate;
        calculationDate.DiscountDate = datesList[key].DiscountDate;
        calculationDate.PaymentGraceDate = datesList[key].PaymentGraceDate;
        calculationDate.ValStartDate = datesList[key].ValStartDate;
        calculationDate.CurrentPeriodStartDate = datesList[key].CurrentPeriodStartDate;
        calculationDate.CurrentPeriodEndDate = datesList[key].CurrentPeriodEndDate;
        calculationDate.DiscountPercentage = datesList[key].DiscountPercentage;
        calculationDateList.push(calculationDate);
    });
}
function saveCalculationDates(datetype) {
    var duetype = $("input[name=" + datetype + "]:checked").attr("data-duetype");
    if ($("input[name=" + datetype + "]:checked").val() == undefined) {
        showAlert("error", SelectOptionErrorMsg);
        return false;
    }
    var isFixDate = $("input[name=" + datetype + "]:checked").val() == "True" ? true : false;
    addremoveValidation(isFixDate, datetype, duetype);
    if ($("#frmCalDates").valid()) {
        var tempList = [];

        if (datetype == 0) {
            $("#FilingDueType").val(duetype);
            service.FilingDueType = duetype;
        }
        else if (datetype == 1) {
            $("#PaymentDueType").val(duetype);
            service.PaymentDueType = duetype;
        }
        else if (datetype == 2) {
            $("#DiscountDueType").val(duetype);
            service.DiscountDueType = duetype;
        }
        else if (datetype == 3) {
            $("#PaymentGraceType").val(duetype);
            service.PaymentGraceType = duetype;
        }
        if (datetype == 0) // FillingDueDates
        {
            service.UseFixedFillingDueDate = isFixDate;
            $("#UseFixedFillingDueDate").val(service.UseFixedFillingDueDate);

            if (!service.UseFixedFillingDueDate) {  // Update Days
                service.FillingDueDays = $("#txtDays").val();
                $("#FillingDueDays").val(service.FillingDueDays);
                clearDates(datetype);
            }
            else {                                // Update Due Date in existing [calculationDateList]
                service.FillingDueDays = 0;
                $("#FillingDueDays").val(0);
                if (duetype == 4) {
                    $("#tblRow tr").each(function () {
                        var rowseqId = $(this).find("#SequenceID").val();
                        tempList = jQuery.grep(calculationDateList, function (item) {
                            return item.SequenceID == rowseqId;
                        });
                        tempList[0].FillingDueDate = $(this).find("#" + rowseqId).val();
                    });
                }
            }
            showHideDueDatesButtonLabel('spFillingChange', 'spFillingAdd');
        }
        else if (datetype == 1) // PaymentDueDates
        {
            service.UseFixedPaymentDueDate = isFixDate;
            $("#UseFixedPaymentDueDate").val(service.UseFixedPaymentDueDate);

            if (!service.UseFixedPaymentDueDate) {  // Update Days
                service.PaymentDueDays = $("#txtDays").val();
                $("#PaymentDueDays").val(service.PaymentDueDays);
                clearDates(datetype);
            }
            else {                                 // Update Due Date in existing [calculationDateList]
                service.PaymentDueDays = 0;
                $("#PaymentDueDays").val(0);
                if (duetype == 4) {
                    $("#tblRow tr").each(function () {
                        var rowseqId = $(this).find("#SequenceID").val();
                        tempList = jQuery.grep(calculationDateList, function (item) {
                            return item.SequenceID == rowseqId;
                        });
                        tempList[0].PaymentDueDate = $(this).find("#" + rowseqId).val();
                    });
                }
            }
            showHideDueDatesButtonLabel('spPaymentChange', 'spPaymentAdd');
        }
        else if (datetype == 2) // DiscountDates
        {
            service.UseFixedDiscountDate = isFixDate;
            $("#UseFixedDiscountDate").val(service.UseFixedDiscountDate);

            if (!service.UseFixedDiscountDate) {  // Update Days
                service.DiscountDueDays = $("#txtDays").val();
                service.DiscountPercentage = $("#txtDiscountPercentage").val();
                $("#DiscountDueDays").val(service.DiscountDueDays);
                $("#DiscountPercentage").val(service.DiscountPercentage);
                clearDates(datetype);
            }
            else {                                 // Update Due Date in existing [calculationDateList]
                service.DiscountDueDays = 0;
                service.DiscountPercentage = 0;
                $("#DiscountDueDays").val(0);
                $("#DiscountPercentage").val(0);
                if (duetype == 4) {
                    $("#tblRow tr").each(function () {
                        var rowseqId = $(this).find("#SequenceID").val();
                        tempList = jQuery.grep(calculationDateList, function (item) {
                            return item.SequenceID == rowseqId;
                        });
                        tempList[0].DiscountDate = $(this).find("#" + rowseqId).val();
                        tempList[0].DiscountPercentage = $(this).find("#txtpercentage" + rowseqId).val();
                    });
                }
                else if (duetype == 1) {
                    $.each(calculationDateList, function (key, value) {
                        calculationDateList[key].DiscountPercentage = $("#txtStartDiscountPercentage").val();
                    });
                }
                else if (duetype == 2) {
                    $.each(calculationDateList, function (key, value) {
                        calculationDateList[key].DiscountPercentage = $("#txtEndDiscountPercentage").val();
                    });
                }
            }
            showHideDueDatesButtonLabel('spDiscountChange', 'spDiscountAdd');
        }
        else if (datetype == 3) // PaymentGraceDates
        {
            service.UseFixedPaymentGracePeriod = isFixDate;
            $("#UseFixedPaymentGracePeriod").val(service.UseFixedPaymentGracePeriod);

            if (!service.UseFixedPaymentGracePeriod) {  // Update Days
                service.PaymentGracePeriod = $("#txtDays").val();
                $("#PaymentGracePeriod").val(service.PaymentGracePeriod);
                clearDates(datetype);
            }
            else {                                 // Update Due Date in existing [calculationDateList]
                service.PaymentGracePeriod = 0;
                $("#PaymentGracePeriod").val(0);
                if (duetype == 4) {
                    $("#tblRow tr").each(function () {
                        var rowseqId = $(this).find("#SequenceID").val();
                        tempList = jQuery.grep(calculationDateList, function (item) {
                            return item.SequenceID == rowseqId;
                        });
                        tempList[0].PaymentGraceDate = $(this).find("#" + rowseqId).val();
                    });
                }
            }
            showHideDueDatesButtonLabel('spGraceChange', 'spGraceAdd');
        }
        $("#dvCalculationDates").html('');
        $("#datesmodal").modal('hide');
        isDatesFilled = true;
    }
    return false;
}
function clearDates(datetype) {
    $.each(calculationDateList, function (key, value) {

        if (datetype == 0) {
            calculationDateList[key].FillingDueDate = null;
        }
        else if (datetype == 1) {
            calculationDateList[key].PaymentDueDate = null;
        }
        else if (datetype == 2) {
            calculationDateList[key].DiscountDate = null;
            calculationDateList[key].DiscountPercentage = null;
        }
        else if (datetype == 3) {
            calculationDateList[key].PaymentGraceDate = null;
        }
    });
}
function addremoveValidation(fixDate, datetype, duetype) {
    if (fixDate && duetype == 4) {
        $("#txtDays").removeClass("required input-validation-error");
        $("#txtDiscountPercentage").removeClass("required input-validation-error");
        $("#txtStartDiscountPercentage").removeClass("required input-validation-error");
        $("#txtEndDiscountPercentage").removeClass("required input-validation-error");
        $('#tblRow input[type="text"]').addClass("required");
    }
    else {
        if (duetype == 1) {
            $("#txtStartDiscountPercentage").addClass("required");
        }
        else if (duetype == 2) {
            $("#txtEndDiscountPercentage").addClass("required");
        }
        else if (duetype == 3) {
            $("#txtDays").addClass("required");
            $('#tblRow input[type="text"]').removeClass("required input-validation-error");
            $("#txtDiscountPercentage").addClass("required");
        }
    }

}
function confirmServiceInputChange(evt, source) {
    if ($("#FillingDueDays").val() != '' || $("#PaymentDueDays").val() != '' || $("#DiscountDueDays").val() != '' || $("#PaymentGracePeriod").val() != '') {
        swal({
            title: suremsg,
            text: ConfirmChangeInputValidationMsg.format($(source).attr("data-label")),
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: continueMessage,
            cancelButtonText: cancel,
            closeOnConfirm: true
        }, function (status) {
            if (status == true) {
                isDatesFilled = false;
                resetCalculationDatesList();
            }
            else {
                var type = $(source).attr("data-type");
                if (type == "datepicker")
                    $(source).datepicker('update', $(source).attr("data-previousvalue"));
                else if (type == "dropdown") {
                    $(source).val($(source).attr("data-previousvalue"));
                    $(source).trigger("change");
                }
            }
            $(source).attr("data-previousvalue", $(source).val());
        });
    }
    else {
        $(source).attr("data-previousvalue", $(source).val());
    }

}
function resetCalculationDatesList() {
    calculationDateList = [];
    showHideDueDatesButtonLabel('spFillingAdd', 'spFillingChange');
    showHideDueDatesButtonLabel('spPaymentAdd', 'spPaymentChange');
    showHideDueDatesButtonLabel('spDiscountAdd', 'spDiscountChange');
    showHideDueDatesButtonLabel('spGraceAdd', 'spGraceChange');
    $(".duedays").val('');
    return false;
}
function showHideDueDatesButtonLabel(showElement, hideElement) {
    $("#" + hideElement).addClass("hide");
    $("#" + showElement).removeClass("hide");
}
$(document).on("change", "#rbFixDate", function () {
    $("#tblRow input[type=text]").removeClass("input-validation-error");
    $("#tblRow input[type=text]").next(".error").hide();
    $(".CalculationDueDateView").hide();
    disableUncheckedInput($(this).attr("name"));
});
$(document).on("change", "#rbFixDate1,#rbFixStartDate1,#rbFixendEndDate1", function () {
    $("#txtDays").removeClass("input-validation-error");
    $("#txtDays").next(".field-validation-error").hide();
    $("#txtDiscountPercentage").removeClass("input-validation-error");
    $("#txtDiscountPercentage").next(".field-validation-error").hide();
    $("#txtStartDiscountPercentage").removeClass("input-validation-error");
    $("#txtStartDiscountPercentage").next(".field-validation-error").hide();
    $("#txtEndDiscountPercentage").removeClass("input-validation-error");
    $("#txtEndDiscountPercentage").next(".field-validation-error").hide();
    if ($(this).attr("data-duetype") == 4) {
        $(".CalculationDueDateView").show();
    }
    disableUncheckedInput($(this).attr("name"));
    CalculationDueDatePopUp($(this).attr("name"), $(this).attr("data-duetype"));
});
function disableUncheckedInput(datetype) {
    if ($("input[name=" + datetype + "]:checked").val() == undefined)
        $("#frmCalDates input[type=text]").attr("disabled", true);
    else if ($("input[name=" + datetype + "]:checked").val() == "True") // disable days
    {
        $("#txtDays").attr("disabled", true);
        $("#txtDiscountPercentage").attr("disabled", true);
        $("#txtStartDiscountPercentage").attr("disabled", true);
        $("#txtEndDiscountPercentage").attr("disabled", true);
        $("#tblRow input[type=text]").removeAttr("disabled");
    }
    else if ($("input[name=" + datetype + "]:checked").val() == "False") // disable dates
    {
        $("#txtDays").removeAttr("disabled");
        $("#txtDiscountPercentage").removeAttr("disabled");
        $("#tblRow input[type=text]").attr("disabled", true);
        $("#txtStartDiscountPercentage").attr("disabled", true);
        $("#txtEndDiscountPercentage").attr("disabled", true);
    }
}
function unbindFieldValidation(eleId) {
    $("#" + eleId).removeClass("required");
}
function bindFieldValidation(eleId) {
    $("#" + eleId).addClass("required");
}
function setCollectionTemplateLink() {
    if ($("#ddltemplate").val() > 0)
        $("#lnktemplate").attr("href", ROOTPath + "/Services/CollectionTemplate/View/" + $("#ddltemplate").val());
    else
        $("#lnktemplate").removeAttr("href");
}
/*******************[START] - POPUP - Exception Functions  *******************************/
//Moved code to ServiceExceptionAddEdit.js
/*******************[START] - POPUP - CollectionRule Functions  *******************************/
//Moved code to servicecollectionrule.js

function CalculationDueDatePopUp(datetype, duetype) {
    if (datetype == 0) {
        $("#FilingDueType").val(duetype);
        service.FilingDueType = duetype;
    }
    else if (datetype == 1) {
        $("#PaymentDueType").val(duetype);
        service.PaymentDueType = duetype;
    }
    else if (datetype == 2) {
        $("#DiscountDueType").val(duetype);
        service.DiscountDueType = duetype;
    }
    else if (datetype == 3) {
        $("#PaymentGraceType").val(duetype);
        service.PaymentGraceType = duetype;
    }

    $.ajax({
        type: "POST",
        //async: false,
        url: ROOTPath + "/Service/AddCalculationDueDates",
        data: { 'service': service, 'dateType': datetype, 'duetype': duetype },
        success: function (response) {
            if (response.status) {
                $(".CalculationDueDateView").html(response.returnData);
                if (duetype == 4) {
                    $.validator.unobtrusive.parse('#frmCalDates');
                    $(".popupinputcaldate").each(function () {
                        $(this).datepicker({
                            defaultViewDate: new Date($(this).attr("data-val-startdate")),
                            todayHighlight: true,
                            keyboardNavigation: false,
                            forceParse: false,
                            calendarWeeks: true,
                            autoclose: true,
                            format: __dateFormat,
                            language: __culture
                        });
                    });
                }
                initializeCalculationDatesList(response.datesList, duetype, datetype);
                if ($("#datesmodal").attr("data-mode") == 'add' || service.IsTestMode == "True")
                    disableUncheckedInput(datetype);
                if (datetype == 2 && (duetype == 1)) {
                    $("#txtStartDiscountPercentage").attr("disabled", false);
                    $("#txtStartDiscountPercentage").removeClass("input-validation-error");
                }
                else if (datetype == 2 && (duetype == 2)) {
                    $("#txtEndDiscountPercentage").attr("disabled", false);
                    $("#txtEndDiscountPercentage").removeClass("input-validation-error");
                }
                return true;
            }
            else {
                showAlert("error", response.message);
                return false;
            }
        }
    });
}