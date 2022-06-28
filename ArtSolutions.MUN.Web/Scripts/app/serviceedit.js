/*********[START] - Global Variable Declaration ******************/
var serviceID;
var collectionRule = {};
var collectionRuleList = [];
var ruleDetail = {};
var ruleDetailList = [];
var ruleSeq = 1, ruledetailSeq = 1, mode = 'add', ruleId = 0;
var defaultFromAmount = 0;
var defaultToAmount = 9999999999999;
var maxRangeAmount = 9999999999999;
var calculationDateList = [];
var calculationDateList_append = [];
var calculationDateList_original = [];
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
    extendStringPrototype();
    editServiceDefaultSetUp();
    //EnableDisableFilingDatesByFilingType($('input[name=FilingTypeID]:checked').val());
    $("#txtServiceName").focus();
    initializeDate('startdate', "MM dd", true);
    initializeDate('EffectiveFrom', __dateFormat, true);
    initializeDate('EffectiveTo', __dateFormat, true);

    $("#txtServiceName").keyup(function () { return false });
    $("#txtServiceName").blur(function () { return false });

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
    if ($("#AccountTypeIDs").val() !== "") {
        $('#AccountType').val($("#AccountTypeIDs").val().split(",")).trigger('change');
    }

    //if ($('input[name=FilingTypeID]:checked').val() == 3 || $('input[name=FilingTypeID]:checked').val() == 2 || $('input[name=FilingTypeID]:checked').val() == 5) {
    //    $("#ddlfilingForm").attr("disabled", false);
    //}
    //else {
    //    $("#ddlfilingForm").val("").trigger("change");
    //    $("#ddlfilingForm").attr("disabled", true);

    //}

});
/*******************[START] - Utility Functions  *******************************/
function editServiceDefaultSetUp() {
    serviceID = $("#Id").val();
    if (serviceID > 0) {
        calculationDateList_original = calculationDateList = $.parseJSON($("#ServiceCalculationDateListJson").val());
        collectionRuleList = $.parseJSON($("#ServiceCollectionRuleListJson").val());
        ruleDetailList = $.parseJSON($("#ServiceCollectionRuleDetailListJson").val());
        initializeServiceException();
        linkRuleAndDetailList();
        isDatesFilled = true;
        if ($("#IsTestMode").val() == "False") {
            $(".disable").attr("disabled", true);
            $(".hideRule").addClass("hide");
        }
    }
}
function initializeDate(className, dateFormat, onLoad) {
    var startdate = null;
    if (className == 'EffectiveTo' && $("#IsTestMode").val() == "False")
        startdate = $("#txtEffectiveTo").val();
    else if (className == 'EffectiveTo' && $("#IsTestMode").val() == "True")
        startdate = new Date();

    $('.' + className).datepicker({
        defaultDate: new Date(),
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: dateFormat,
        language: __culture,
        startDate: startdate
    }).on("change", function (e) {
        if ($(this).attr("id") == "txtStartDate") {
            var startDate = $(this).datepicker('getDate');
            startDate.setMonth(startDate.getMonth() + 12);
            startDate.setDate(startDate.getDate() - 1);
            $("#txtEndDate").datepicker({ format: dateFormat, language: __culture });
            $('#txtEndDate').datepicker('update', startDate);
        }

        if (!onLoad)
            confirmServiceInputChange(e, this);
        onLoad = false;
    });

    if (className == 'startdate')
        $('.' + className).datepicker('update', startDateFormatted);
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

//19-Feb-2020 - Copy Rule - Effective From -  disable previous date 
function initializeCopyPopupDate(className, dateFormat) {

    var effectiveFromStartDate = $("#txtruleEffectiveFrom").val();

    $('.' + className).datepicker({        
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: dateFormat,
        language: __culture,
        startDate: effectiveFromStartDate       
    });
}
//

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
function UpdateCalculationDateListForAutoFiling() {
    $.each(calculationDateList, function (key, value) {
        if (calculationDateList[key].FillingDueDate == null)
            calculationDateList[key].FillingDueDate = calculationDateList[key].CurrentPeriodStartDate;
        if (calculationDateList[key].PaymentDueDate == null)
            calculationDateList[key].PaymentDueDate = calculationDateList[key].CurrentPeriodEndDate;
    });
}

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

        //17-Feb-2020     
        if ($('input[name=FilingTypeID]:checked').val() == "4" || $('input[name=FilingTypeID]:checked').val() == "5") { // Updated
            UpdateCalculationDateListForAutoFiling();
        }
        //

        if (validateForm()) {
            ClearFilingDateByFilingType();
            $("#ServiceCollectionRuleListJson").val(JSON.stringify(collectionRuleList));
            $("#ServiceCalculationDateListJson").val(JSON.stringify(calculationDateList));
            $("#ServiceExceptionListJSON").val(JSON.stringify(exceptionList));
            $("#txtEndDate").attr("disabled", false);
            $($("#hdnFormattedStartDate")).val(new Date($("#txtStartDate").datepicker('getDate')).toDateString());
            $($("#hdnFormattedEndDate")).val(new Date($("#txtEndDate").datepicker('getDate')).toDateString());
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
        $(".rdDueDays").val(null);
        calculationDateList = [];
    }

    //14-Feb-2020 - CO-1349
    if ($('input[name=FilingTypeID]:checked').val() != 3) { //3=Fix Days/Dates
        $("#UseFixedFillingDueDate").val(null);
        $("#FillingDueDays").val(null);
        $("#UseFixedPaymentDueDate").val(null);
        $("#PaymentDueDays").val(null);
        $("#UseFixedDiscountDate").val(null);
        $("#DiscountDueDays").val(null);
        $("#UseFixedPaymentGracePeriod").val(null);
        $("#PaymentGracePeriod").val(null);
        $("#FilingDueType").val(null);
        $("#PaymentDueType").val(null);
        $("#DiscountDueType").val(null);
        $("#PaymentGraceType").val(null);
    }
    //
}

function validateForm() {

    if ($('input[name=FilingTypeID]:checked').val() == 3) {

        if ($("#FillingDueDays").val() == '') {
            showAlert("error", $("#FillingDueDays").attr("data-val-msg")); return false;
        }
        if ($("#PaymentDueDays").val() == '') {
            showAlert("error", $("#PaymentDueDays").attr("data-val-msg")); return false;
        }
        if ($("#UseFixedFillingDueDate").val().toLowerCase() == "true") {
            if (findIndex(calculationDateList, "FillingDueDate", null) > -1) {
                showAlert("error", $("#FillingDueDays").attr("data-val-msg")); return false;
            }
        }
        if ($("#UseFixedFillingDueDate").val().toLowerCase() == "false") {
            if ($("#FillingDueDays").val() < 0) { showAlert("error", $("#FillingDueDays").attr("data-val-msg")); return false; }
        }

        if ($("#UseFixedPaymentDueDate").val().toLowerCase() == "true") {
            if (findIndex(calculationDateList, "PaymentDueDate", null) > -1) {
                showAlert("error", $("#PaymentDueDays").attr("data-val-msg")); return false;
            }
        }
        if ($("#UseFixedPaymentDueDate").val().toLowerCase() == "false") {
            if ($("#PaymentDueDays").val() < 0) {
                showAlert("error", $("#PaymentDueDays").attr("data-val-msg")); return false;
            }
        }

        if ($("#UseFixedDiscountDate").val().toLowerCase() == "true") {
            if (findIndex(calculationDateList, "DiscountDate", null) > -1) {
                showAlert("error", $("#DiscountDueDays").attr("data-val-msg")); return false;
            }
        }

        if ($("#PaymentGracePeriod").val().toLowerCase() == "true") {
            if (findIndex(calculationDateList, "PaymentGraceDate", null) > -1) {
                showAlert("error", $("#PaymentGracePeriod").attr("data-val-msg")); return false;
            }
        }
        
    }
    else if ($('input[name=FilingTypeID]:checked').val() == 4 || $('input[name=FilingTypeID]:checked').val() == 5) { // Updated

        if ($("#UseFixedPaymentDueDate").val().toLowerCase() == "true") {
            if (findIndex(calculationDateList, "PaymentDueDate", null) > -1) {
                showAlert("error", $("#PaymentDueDays").attr("data-val-msg")); return false;
            }
        }
        if ($("#UseFixedPaymentDueDate").val().toLowerCase() == "false") {
            if ($("#PaymentDueDays").val() < 0) {
                showAlert("error", $("#PaymentDueDays").attr("data-val-msg")); return false;
            }
        }

        if ($("#UseFixedDiscountDate").val().toLowerCase() == "true") {
            if (findIndex(calculationDateList, "DiscountDate", null) > -1) {
                showAlert("error", $("#DiscountDueDays").attr("data-val-msg")); return false;
            }
        }

        if ($("#PaymentGracePeriod").val().toLowerCase() == "true") {
            if (findIndex(calculationDateList, "PaymentGraceDate", null) > -1) {
                showAlert("error", $("#PaymentGracePeriod").attr("data-val-msg")); return false;
            }
        }
    }
    if (findIndex(collectionRuleList, "IsActive", 1) == -1) {
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
        window.location = ROOTPath + "/Service/List";
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

    if (filingTypeID == 4 || filingTypeID == 5) {  // Updated
        $('#discounton').css({ "pointer-events": "" });
        $('#paymentgraceperiod').css({ "pointer-events": "" });
        $('#discounton').addClass("disabled");
        $('#paymentgraceperiod').addClass("disabled");
    }

    if (filingTypeID == 3 || filingTypeID == 2 || filingTypeID == 5) {
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
function loadCalculationDatesPopup(ele, datetype) {
    unbindFieldValidation("ddltemplate");
    if ($("#form").valid()) {
        bindFieldValidation("ddltemplate");
        setServiceValues();
        var loadUrl = getLoadPopupURL($(ele).attr("data-mode"));
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
                //async: false,
                url: loadUrl,
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
                        initializeCalculationDatesList(response.datesList, response.datesList_append, DueType, datetype);
                        $("#txtDays").focus();

                        ////13-Feb-2020
                        var fillingDueDateCount = 0;
                        var paymentDueDateCount = 0;
                        var discountDateCount = 0;
                        var paymentGraceDate = 0;

                        $.each(calculationDateList, function (key, value) {
                            if (calculationDateList[key].FillingDueDate != null)
                                fillingDueDateCount++;
                            if (calculationDateList[key].PaymentDueDate != null)
                                paymentDueDateCount++;
                            if (calculationDateList[key].DiscountDate != null)
                                discountDateCount++;
                            if (calculationDateList[key].PaymentGraceDate != null)
                                paymentGraceDate++;
                        });

                        if (
                            DueType == 3 // Use Fix Days
                            ||
                            (
                                (datetype == 0 && fillingDueDateCount > 0) ||
                                (datetype == 1 && paymentDueDateCount > 0) ||
                                (datetype == 2 && discountDateCount > 0) ||
                                (datetype == 3 && paymentGraceDate > 0)
                            )
                        ) {
                            $("input[data-duetype='" + DueType + "']").prop("checked", true);
                        }
                        else {
                            $("input[data-duetype='" + 1 + "']").prop("checked", false);
                            $("input[data-duetype='" + 2 + "']").prop("checked", false);
                            $("input[data-duetype='" + 3 + "']").prop("checked", false);
                            $("input[data-duetype='" + 4 + "']").prop("checked", false);
                            $(".CalculationDueDateView").html(null);
                        }
                        //

                        $("#datesmodal").attr("data-mode", $(ele).attr("data-mode"));
                        $("#datesmodal").modal("show");
                        $("#titleId").html($(ele).attr("data-popup-title"));
                        if ($(ele).attr("data-mode") == 'add' || service.IsTestMode == "True")
                            disableUncheckedInput(datetype);

                        if (datetype == 2 && calculationDateList[0].DiscountPercentage != null) {
                            if (DueType == 1) {
                                $("#txtStartDiscountPercentage").val(GlobalFormat(calculationDateList[0].DiscountPercentage));
                            }
                            else if (DueType == 2) {
                                $("#txtEndDiscountPercentage").val(GlobalFormat(calculationDateList[0].DiscountPercentage));
                            }
                            else if (DueType == 3) {
                                $("#txtDiscountPercentage").val(GlobalFormat(calculationDateList[0].DiscountPercentage));
                            }
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
    }
    bindFieldValidation("ddltemplate");
    return false;
}

function getLoadPopupURL(mode) {
    if (mode == 'add')
        return ROOTPath + "/Service/AddCalculationDates";
    else if (mode == 'edit')
        return ROOTPath + "/Service/EditCalculationDates";
}
function getDueLoadPopupURL(mode) {
    if (mode == 'add')
        return ROOTPath + "/Service/AddCalculationDueDates";
    else if (mode == 'edit')
        return ROOTPath + "/Service/EditCalculationDueDates";
}
function setServiceValues() {
    service.ID = $("#Id").val();
    service.EffectiveFrom = new Date($("#txtEffectiveFrom").datepicker('getDate')).toDateString();
    service.EffectiveTo = new Date($("#txtEffectiveTo").datepicker('getDate')).toDateString();
    service.StartDate = new Date($("#txtStartDate").datepicker('getDate')).toDateString();
    service.EndDate = new Date($("#txtEndDate").datepicker('getDate')).toDateString();
    service.EffectiveTo_Original = $("#EffectiveTo_Original").val();
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
    service.IsTestMode = $("#IsTestMode").val();

    if (!isDatesFilled) {
        calculationDateList_append = [];
        calculationDateList = calculationDateList_original;
    }
    service.ServiceCalculationDateList = calculationDateList;
    service.ServiceCalculationDateList_Append = calculationDateList_append;

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
function initializeCalculationDatesList(datesList, datesList_append, duetype, datetype) {
    if (calculationDateList_append != undefined && calculationDateList_append.length == 0) {
        calculationDateList_append = datesList_append;
        calculationDateList = datesList;
    }
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
            else {// Update Due Date in existing [calculationDateList]
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
                else if (duetype == 1 || duetype == 2)//14-Feb-2020 -testing
                {
                    $.each(calculationDateList, function (key, value) {
                        if (duetype == 1)
                            calculationDateList[key].FillingDueDate = calculationDateList[key].CurrentPeriodStartDate;
                        else
                            calculationDateList[key].FillingDueDate = calculationDateList[key].CurrentPeriodEndDate;
                    });
                }
                //

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
            else {// Update Due Date in existing [calculationDateList]
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
                else if (duetype == 1 || duetype == 2) //14-Feb-2020 -testing
                {
                    $.each(calculationDateList, function (key, value) {
                        if (duetype == 1)
                            calculationDateList[key].PaymentDueDate = calculationDateList[key].CurrentPeriodStartDate;
                        else
                            calculationDateList[key].PaymentDueDate = calculationDateList[key].CurrentPeriodEndDate;
                    });
                }
                //
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
                        tempList[0].DiscountPercentage = $(this).find("#txtpercentage" + rowseqId).val() != '' ? $(this).find("#txtpercentage" + rowseqId).val() : $(this).find("#txtpercentage" + rowseqId).text().trim();
                    });
                }
                else if (duetype == 1) {
                    $.each(calculationDateList, function (key, value) {
                        //17-Feb-2020 - CO-1348
                        calculationDateList[key].DiscountPercentage = GlobalConvertToDecimal($("#txtStartDiscountPercentage").val());

                        //14-Feb-2020 - CO-1349
                        calculationDateList[key].DiscountDate = calculationDateList[key].CurrentPeriodStartDate;
                    });
                }
                else if (duetype == 2) {
                    $.each(calculationDateList, function (key, value) {
                        //17-Feb-2020 - CO-1348
                        calculationDateList[key].DiscountPercentage = GlobalConvertToDecimal($("#txtEndDiscountPercentage").val());

                        //14-Feb-2020 - CO-1349
                        calculationDateList[key].DiscountDate = calculationDateList[key].CurrentPeriodEndDate;
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
                else if (duetype == 1 || duetype == 2)//14-Feb-2020 -testing
                {
                    $.each(calculationDateList, function (key, value) {
                        if (duetype == 1)
                            calculationDateList[key].PaymentGraceDate = calculationDateList[key].CurrentPeriodStartDate;
                        else
                            calculationDateList[key].PaymentGraceDate = calculationDateList[key].CurrentPeriodEndDate;
                    });
                }
                //
            }
            showHideDueDatesButtonLabel('spGraceChange', 'spGraceAdd');
        }
        $("#dvCalculationDates").html('');
        $("#datesmodal").modal('hide');
        isDatesFilled = true;
    }
    return false;
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
            $("#txtDiscountPercentage").addClass("required");
            $('#tblRow input[type="text"]').removeClass("required input-validation-error");
        }
    }

}

function confirmServiceInputChange(evt, source) {
    if (
        $("#FillingDueDays").val() != '' || $("#PaymentDueDays").val() != '' ||
        $("#DiscountDueDays").val() != '' || $("#PaymentGracePeriod").val() != '' ||
        $('input[name=FilingTypeID]:checked').val() == 4 || //Auto Filing
        $('input[name=FilingTypeID]:checked').val() == 5 //Auto Filing with Edit
    ) {
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
                resetCalculationDatesList(source);
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
function resetCalculationDatesList(source) {
    calculationDateList = calculationDateList_original;
    calculationDateList_append = [];

    if ($(source).attr("id") != "txtEffectiveTo") {
        $(".duedates").attr("data-mode", "add");
        calculationDateList = calculationDateList_original = [];
    }
    regenerateCalculationDateList();
    return false;
}
function regenerateCalculationDateList() {
    setServiceValues();
    service.FilingDueType = $("#FilingDueType").val();
    service.PaymentDueType = $("#PaymentDueType").val();
    service.DiscountDueType = $("#DiscountDueType").val();
    service.PaymentGraceType = $("#PaymentGraceType").val();

    $.ajax({
        type: "POST",
        //async: false,
        url: ROOTPath + "/Service/RegenerateCalculationDateList",
        data: { 'service': service },
        success: function (response) {
            if (response.status) {
                calculationDateList = response.datesList;
                calculationDateList_append = response.datesList_append;
                isDatesFilled = true;
                return true;
            }
        }
    });
}
function showHideDueDatesButtonLabel(showElement, hideElement) {
    $("#" + hideElement).addClass("hide");
    $("#" + showElement).removeClass("hide");
}
$(document).on("change", "#rbFixDate", function () {
    $("#tblRow input[type=text]").removeClass("input-validation-error");
    $("#tblRow input[type=text]").next().removeClass("field-validation-error").addClass("field-validation-valid");
    $("#txtDays").next().addClass("field-validation-error");
    $("#txtDiscountPercentage").next().addClass("field-validation-error");
    $(".CalculationDueDateView").hide();
    disableUncheckedInput($(this).attr("name"));
});
$(document).on("change", "#rbFixDate1,#rbFixStartDate1,#rbFixendEndDate1", function () {
    $("#txtDays").removeClass("input-validation-error");
    $("#txtDays").next().removeClass("field-validation-error").addClass("field-validation-valid");
    $("#txtDiscountPercentage").removeClass("input-validation-error");
    $("#txtDiscountPercentage").next().removeClass("field-validation-error").addClass("field-validation-valid");
    $("#txtStartDiscountPercentage").removeClass("input-validation-error");
    $("#txtStartDiscountPercentage").next(".field-validation-error").addClass("field-validation-valid");
    $("#txtEndDiscountPercentage").removeClass("input-validation-error");
    $("#txtEndDiscountPercentage").next(".field-validation-error").addClass("field-validation-valid");
    $("#tblRow input[type=text]").next().addClass("field-validation-error");
    if ($(this).attr("data-duetype") == 4) {
        $(".CalculationDueDateView").show();
    }
    disableUncheckedInput($(this).attr("name"));
    CalculationDueDatePopUp($(this).attr("name"), $(this).attr("data-duetype"));
});
function disableUncheckedInput(datetype) {
    //if ($("input[name=" + datetype + "]:checked").val() == undefined)
    //    $("#frmCalDates input[type=text]").attr("disabled", true);
    //else if ($("input[name=" + datetype + "]:checked").val() == "True") // disable days
    //{
    //    $("#txtDays").attr("disabled", true);
    //    $("#txtDiscountPercentage").attr("disabled", true);
    //    $("#txtStartDiscountPercentage").attr("disabled", true);
    //    $("#txtEndDiscountPercentage").attr("disabled", true);
    //    $("#tblRow input[type=text]").removeAttr("disabled");
    //}
    //else if ($("input[name=" + datetype + "]:checked").val() == "False") // disable dates
    //{
    //    $("#txtDays").removeAttr("disabled");
    //    $("#txtDiscountPercentage").removeAttr("disabled");
    //    $("#tblRow input[type=text]").attr("disabled", true);
    //    $("#txtStartDiscountPercentage").attr("disabled", true);
    //    $("#txtEndDiscountPercentage").attr("disabled", true);
    //}

    //17-Feb-2020 - CO-1348
    if ($("input[name=" + datetype + "]:checked").val() == undefined)
        $("#frmCalDates input[type=text]").attr("disabled", true);
    else if ($("input[data-duetype='1']").prop("checked")) {
        $("#txtStartDiscountPercentage").attr("disabled", false);
        $("#txtEndDiscountPercentage").attr("disabled", true);
        $("#txtDays").attr("disabled", true);
        $("#txtDiscountPercentage").attr("disabled", true);
        $("#tblRow input[type=text]").attr("disabled", true);
    }
    else if ($("input[data-duetype='2']").prop("checked")) {
        $("#txtStartDiscountPercentage").attr("disabled", true);
        $("#txtEndDiscountPercentage").attr("disabled", false);
        $("#txtDays").attr("disabled", true);
        $("#txtDiscountPercentage").attr("disabled", true);
        $("#tblRow input[type=text]").attr("disabled", true);
    }
    else if ($("input[data-duetype='3']").prop("checked")) {
        $("#txtStartDiscountPercentage").attr("disabled", true);
        $("#txtEndDiscountPercentage").attr("disabled", true);
        $("#txtDays").attr("disabled", false);
        $("#txtDiscountPercentage").attr("disabled", false);
        $("#tblRow input[type=text]").attr("disabled", true);
    }
    else if ($("input[data-duetype='4']").prop("checked")) {
        $("#txtStartDiscountPercentage").attr("disabled", true);
        $("#txtEndDiscountPercentage").attr("disabled", true);
        $("#txtDays").attr("disabled", true);
        $("#txtDiscountPercentage").attr("disabled", true);
        $("#tblRow input[type=text]").attr("disabled", false);
    }
    clearUncheckedInput(datetype);
    //
}

//17-Feb-2020 - CO-1348
function clearUncheckedInput(datetype) {
    if ($("input[name=" + datetype + "]:checked").val() == undefined)
        $("#frmCalDates input[type=text]").val(null);
    else if ($("input[data-duetype='1']").prop("checked")) {
        $("#txtEndDiscountPercentage #txtDays #txtDiscountPercentage #tblRow input[type=text]").val(null);
    }
    else if ($("input[data-duetype='2']").prop("checked")) {
        $("#txtStartDiscountPercentage #txtDays #txtDiscountPercentage #tblRow input[type=text]").val(null);
    }
    else if ($("input[data-duetype='3']").prop("checked")) {
        $("#txtStartDiscountPercentage #txtEndDiscountPercentage #tblRow input[type=text]").val(null);
    }
    else if ($("input[data-duetype='4']").prop("checked")) {
        $("#txtStartDiscountPercentage #txtEndDiscountPercentage #txtDays #txtDiscountPercentage").val(null);
    }
}
function unbindFieldValidation(eleId) {
    $("#" + eleId).removeClass("required");
}
function bindFieldValidation(eleId) {
    $("#" + eleId).addClass("required");
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
function setCollectionTemplateLink() {
    if ($("#ddltemplate").val() > 0)
        $("#lnktemplate").attr("href", ROOTPath + "/Services/CollectionTemplate/View/" + $("#ddltemplate").val());
    else
        $("#lnktemplate").removeAttr("href");
}
/*******************[START] - POPUP - Service Exception  *******************************/
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
    var loadUrl = getDueLoadPopupURL($("#datesmodal").attr("data-mode"));

    $.ajax({
        type: "POST",
        //async: false,
        url: loadUrl,
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
                initializeCalculationDatesList(response.datesList, response.datesList_append, duetype, datetype);
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