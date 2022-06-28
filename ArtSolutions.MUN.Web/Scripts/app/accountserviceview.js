var confMsgcustomFieldUpdateForReplace = null;
var attachmentDropzone;
var imageIds = "";
var LicensePercentage = 0.0

$(document).ready(function () {
    $(".select2dropdown").select2({ width: '100%' });
    $('.footable').footable();
});

function initializeFillingDate() {
    $('.fillingDate').datepicker({
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture
    });
    $('.fillingDate').datepicker('update', new Date());
}

function FillingSuccessCallback(response) {
    $("#licensemodal").modal('hide');
    $("#taxmodal").modal('hide');
    $("#Unitmodal").modal('hide');
    $("#measuredwatermodal").modal('hide');
    $("#propertytaxmodal").modal('hide');

    if (response.status === false) {
        showAlert("error", response.message);
    }
    else {
        window.location.href = accountserviceRedirectURL;
    }
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

//========= Filing Function ================== *@
function GetExemptionAmount() {
    var exemptionAmount = ((GlobalConvertToDecimal($('#ExceptionValue').val()) * GlobalConvertToDecimal($('#txtGrossVolume').val())) / 100);
    $('#txtExemptionAmount').val(isNaN(exemptionAmount) ? 0 : CurrencyGlobalFormat(exemptionAmount));
}

function GetExemptionAmountForProperty() {
    if ($('#ExceptionValue').val() != "" && $('#txtSubTotal').val() != "") {
        var exemptionAmount = (CurrencyGlobalFormat((GlobalConvertToDecimal($('#ExceptionValue').val()) * GlobalConvertToDecimal($('#txtSubTotal').val())) / 100));
        $('#txtExemptionAmount').val(exemptionAmount);
    }
}

//========= Tax POPUP Functions============= *@
$(document).on('click', '#viewFillingTax', function (e) {
    var collectionDetail = {};
    var element = this;
    collectionDetail.AccountServiceCollectionDetailId = $(this).attr("data-detailid");
    collectionDetail.ServiceName = $(this).attr("data-servicename");
    collectionDetail.ServicePeriod = $(this).attr("data-serviceperiod");
    collectionDetail.ExceptionValue = $(this).attr("data-exceptionValue");

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/FillingTax",
        data: { 'collectionDetail': collectionDetail, 'isViewMode': true, 'isSummary': $(this).attr("data-isSummary") },
        success: function (data) {
            $("#dvTax").html(data);
            $("#FillingPeriod").val(collectionDetail.ServicePeriod.split('-').pop().trim());
            $("#btnAddTax").hide();
            $("#taxmodal").modal('show');
            $("#frmFillingTax input").prop("disabled", true);
        }, error: function () { }
    }).always(function () {
    });
});

//=========Auto Filing POPUP Functions
$(document).on('click', '#viewAutoFilling', function (e) {
    var collectionDetail = {};
    var element = this;
    collectionDetail.AccountServiceCollectionDetailId = $(this).attr("data-detailid");
    collectionDetail.ServiceName = $(this).attr("data-servicename");
    collectionDetail.ServicePeriod = $(this).attr("data-serviceperiod");
    var SequenceID = $(this).attr("data-sequenceid");
    var ServiceStartPeriod = $(this).attr("data-startperiodid");

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/AutoFilling",
        data: {
            'collectionDetail': collectionDetail, 'isSummary': $(this).attr("data-isSummary")
        },
        success: function (data) {
            $("#dvAutoFilling").html(data);
            $("#autofillingmodal").modal('show');
            $("#dvAutoFilling input").prop("disabled", true);
        }, error: function () { }
    }).always(function () {
    });
});

$(document).on('click', '.addFillingTax', function (e) {
    var collectionDetail = {};
    var element = this;
    collectionDetail.AccountServiceCollectionDetailId = $(this).attr("data-detailid");
    collectionDetail.ServiceName = $(this).attr("data-servicename");
    collectionDetail.ServicePeriod = $(this).attr("data-serviceperiod");
    collectionDetail.ExceptionValue = $(this).attr("data-exceptionValue");
    collectionDetail.RowVersion64 = $(this).attr("data-rowVersion64");
    collectionDetail.GroupID = $(this).attr("data-groupid");
    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/FillingTax",
        data: { 'collectionDetail': collectionDetail, 'isViewMode': false, 'isSummary': $(this).attr("data-isSummary") },
        success: function (data) {
            $("#dvTax").html(data);
            $("#FillingPeriod").val(collectionDetail.ServicePeriod.split('-').pop().trim());
            $('#txtImportsUse').focus();
            $('.clearTax').val(null);
            initializeFillingDate();
            initializeDropzone();
        }, error: function () { }
    }).always(function () {
    });
});

$(document).on('click', '.editFillingTax', function (e) {
    var collectionDetail = {};
    var element = this;
    collectionDetail.AccountServiceCollectionDetailId = $("#dvTax #AccountServiceCollectionDetailId").val();
    collectionDetail.ServiceName = $("#dvTax #ServiceName").val();
    collectionDetail.ServicePeriod = $("#dvTax #ServicePeriod").val();
    collectionDetail.ExceptionValue = $("#dvTax #ExceptionValue").val();
    collectionDetail.RowVersion64 = $("#dvTax #RowVersion").val();
    collectionDetail.GroupID = $("#dvTax #GroupID").val();

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/EditFillingTax",
        data: { 'collectionDetail': collectionDetail, 'isViewMode': false, 'isSummary': $("#dvTax #isSummary").val() },
        success: function (data) {
            $("#dvTax").html(data);
            $("#FillingPeriod").val(collectionDetail.ServicePeriod.split('-').pop().trim());
            $('#txtImportsUse').focus();
            initializeDropzone();
        }, error: function () { }
    }).always(function () {
    });
});

function calculateTaxTotal() {
    var grossAmount = $("#txtGrossVolume").val();
    var excemptionAmount = $("#txtExemptionAmount").val();
    var returnAmount = $("#txtReturnAmount").val();
    var purchasesubjectToTax = $("#txtPurchasesubjectToTax").val();
    if (!isNaN(GlobalConvertToDecimal(excemptionAmount)) && GlobalConvertToDecimal(excemptionAmount) > GlobalConvertToDecimal(grossAmount)) {
        showAlert('error', $("#txtExemptionAmount").attr("datatitle") + ' ' + RequiredFillingMaxValueErrMsg);
        return false;
    }
    if (!isNaN(GlobalConvertToDecimal(returnAmount)) && GlobalConvertToDecimal(returnAmount) > GlobalConvertToDecimal(grossAmount)) {
        showAlert('error', $("#txtReturnAmount").attr("datatitle") + ' ' + RequiredFillingMaxValueErrMsg);
        return false;
    }
    if (!isNaN(GlobalConvertToDecimal(purchasesubjectToTax)) && GlobalConvertToDecimal(purchasesubjectToTax) > GlobalConvertToDecimal(grossAmount)) {
        showAlert('error', $("#txtPurchasesubjectToTax").attr("datatitle") + ' ' + RequiredFillingMaxValueErrMsg);
        return false;
    }

    var total = (isNaN(GlobalConvertToDecimal(grossAmount)) === true ? 0 : GlobalConvertToDecimal(grossAmount))
        - (isNaN(GlobalConvertToDecimal(excemptionAmount)) === true ? 0 : GlobalConvertToDecimal(excemptionAmount))
        - (isNaN(GlobalConvertToDecimal(returnAmount)) === true ? 0 : GlobalConvertToDecimal(returnAmount))
        + (isNaN(GlobalConvertToDecimal(purchasesubjectToTax)) === true ? 0 : GlobalConvertToDecimal(purchasesubjectToTax));

    $("#txtTotal").val(CurrencyGlobalFormat(total));
    return true;
}

$(document).on("focusout", ".calculate", function (e) {
    if ($(e.relatedTarget).hasClass("closemodal")) {
        return true;
    }
    else {
        calculateTaxTotal();
    }

});

function calculateUseTotal() {
    var totalCalculateUse = 0;
    $(".calculateUse:not(:last)").each(function () {
        if ($(this).val()) {
            if (isNaN(GlobalConvertToDecimal($(this).val()))) {
                showAlert('error', $(this).attr("datatitle") + ' ' + RequiredFillingMaxValueErrMsg);
                return false;
            }
            else {
                totalCalculateUse += GlobalConvertToDecimal($(this).val());
            }
        }
    });
    $(".calculateUse:last").val(CurrencyGlobalFormat(totalCalculateUse));
    return true;
}

$(document).on("focusout", ".calculateUse", function (e) {
    if ($(e.relatedTarget).hasClass("closemodal")) {
        return true;
    }
    else {
        calculateUseTotal();
        calculateTotalSubject();
    }
});

function calculateExemptTotal() {
    var totalCalculateUse = 0;
    $(".calculateExempt:not(:last)").each(function () {
        if ($(this).val()) {
            if (isNaN(GlobalConvertToDecimal($(this).val()))) {
                showAlert('error', $(this).attr("datatitle") + ' ' + RequiredFillingMaxValueErrMsg);
                return false;
            }
            else {
                totalCalculateUse += GlobalConvertToDecimal($(this).val());
            }
        }
    });
    $(".calculateExempt:last").val(CurrencyGlobalFormat(totalCalculateUse));
    return true;
}

$(document).on("focusout", ".calculateExempt", function (e) {
    if ($(e.relatedTarget).hasClass("closemodal")) {
        return true;
    }
    else {
        calculateExemptTotal();
    }
});

function calculateTaxableTotal() {
    var totalCalculateUse = 0;
    $(".calculateTaxable:not(:last)").each(function () {
        if ($(this).val()) {
            if (isNaN(GlobalConvertToDecimal($(this).val()))) {
                showAlert('error', $(this).attr("datatitle") + ' ' + RequiredFillingMaxValueErrMsg);
                return false;
            }
            else {
                totalCalculateUse += GlobalConvertToDecimal($(this).val());
            }
        }
    });
    $(".calculateTaxable:last").val(CurrencyGlobalFormat(totalCalculateUse));
    return true;
}

$(document).on("focusout", ".calculateTaxable", function (e) {
    if ($(e.relatedTarget).hasClass("closemodal")) {
        return true;
    }
    else {
        calculateTaxableTotal();
        calculateTotalSubject();
    }
});

$(document).on("focusout", ".calculateTotalSubject", function (e) {
    if ($(e.relatedTarget).hasClass("closemodal")) {
        return true;
    }
    else {
        calculateTotalSubject();
    }
});

function calculateTotalSubject() {
    var TotalUseSubject = $(".calculateUse:last").val();
    var TotalTaxable = $(".calculateTaxable:last").val();
    var TotalSubject = GlobalConvertToDecimal(TotalUseSubject) + GlobalConvertToDecimal(TotalTaxable == "" ? "0" : TotalTaxable);
    $(".calculateTotalSubject").val(CurrencyGlobalFormat(TotalSubject));
}

function SaveTax() {

    $.validator.unobtrusive.parse('#frmFillingTax');
    if ($("#frmFillingTax").valid()) {
        if (imageIds != "" && $("#ImageIds").val() != "")
            $("#ImageIds").val($("#ImageIds").val() + "," + imageIds);
        else
            $("#ImageIds").val(imageIds);

        if (calculateUseTotal() && calculateExemptTotal() && calculateTaxableTotal()) {
            showAlert('info', FillingProcessInfoMsg, 0);
            return true;
        }
        else
            return false;
    }
    return false;
}

function EditSaveTax() {

    $.validator.unobtrusive.parse('#frmFillingTax');
    if ($("#frmFillingTax").valid()) {
        if (imageIds != "" && $("#ImageIds").val() != "")
            $("#ImageIds").val($("#ImageIds").val() + "," + imageIds);
        else
            $("#ImageIds").val(imageIds);

        if (calculateUseTotal() && calculateExemptTotal() && calculateTaxableTotal()) {

            swal({
                title: UpdateWaringMsg,
                type: "warning",
                showCancelButton: true,
                cancelButtonText: cancelBtnText,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: updateBtnText,
                closeOnConfirm: true
            }, function (confirmed) {
                if (confirmed) {
                    showAlert('info', FillingProcessInfoMsg, 0);
                    $("#frmFillingTax").submit();
                }
                else
                    return false;
            });
        }
        else
            return false;
    }
    return false;
}

//=========License POPUP Functions============= 
$(document).on('click', '#viewFillingLicense', function (e) {
    var collectionDetail = {};
    var element = this;
    collectionDetail.AccountServiceCollectionDetailId = $(this).attr("data-detailid");
    collectionDetail.ServiceName = $(this).attr("data-servicename");
    collectionDetail.ServicePeriod = $(this).attr("data-serviceperiod");
    collectionDetail.FilingFormID = FilingFormID;
    collectionDetail.ServiceTypeID = $(this).attr("data-servicetypeid");

    if (FilingFormID == 6) // Liquor license
        collectionDetail.PercentageValue = $(this).attr("data-percentagevalue");
    else
        collectionDetail.ExceptionValue = $(this).attr("data-exceptionValue");

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/FillingLicense",
        data: {
            'collectionDetail': collectionDetail,
            'filingFormID': FilingFormID,
            'isViewMode': true,
            'isSummary': $(this).attr("data-isSummary")
        },
        success: function (data) {
            $("#dvLicense").html(data);
            $("#btnAddLicense").hide();
            $("#licensemodal").modal('show');
            $("#frmFillingLicense input").prop("disabled", true);
        }, error: function () { }
    }).always(function () {
    });
});

$(document).on('click', '.addFillingLicense', function (e) {
    var collectionDetail = {};
    var element = this;
    collectionDetail.AccountServiceCollectionDetailId = $(this).attr("data-detailid");
    collectionDetail.ServiceName = $(this).attr("data-servicename");
    collectionDetail.ServicePeriod = $(this).attr("data-serviceperiod");
    collectionDetail.ServiceTypeID = $(this).attr("data-servicetypeid");
    collectionDetail.FilingFormID = FilingFormID;

    if (FilingFormID == 6) // Liquor license
        collectionDetail.PercentageValue = $(this).attr("data-percentagevalue");
    else
        collectionDetail.ExceptionValue = $(this).attr("data-exceptionValue");

    collectionDetail.RowVersion64 = $(this).attr("data-rowVersion64");
    collectionDetail.GroupID = $(this).attr("data-groupid");

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/FillingLicense",
        data: {
            'collectionDetail': collectionDetail,
            'filingFormID': FilingFormID,
            'isViewMode': false,
            'isSummary': $(this).attr("data-isSummary")
        },
        success: function (data) {
            $("#dvLicense").html(data);
            $('#txtGrossVolume').focus();
            $('.clearLicence').val(null); //CO-910
            initializeFillingDate();
            initializeDropzone();
            if ($("#ServiceTypeID").val() == 1)
                GetBusinessFillingRule(collectionDetail.AccountServiceCollectionDetailId);
        }, error: function () { }
    }).always(function () {
    });
});

$(document).on('click', '.editFillingLicense', function (e) {
    var collectionDetail = {};
    var element = this;
    collectionDetail.AccountServiceCollectionDetailId = $("#dvLicense #AccountServiceCollectionDetailId").val();
    collectionDetail.ServiceName = $("#dvLicense #ServiceName").val();
    collectionDetail.ServicePeriod = $("#dvLicense #ServicePeriod").val();
    collectionDetail.ServiceTypeID = $("#dvLicense #ServiceTypeID").val();
    collectionDetail.FilingFormID = FilingFormID;

    if (FilingFormID == 6) // Liquor license
        collectionDetail.PercentageValue = $("#dvLicense #PercentageValue").val();
    else
        collectionDetail.ExceptionValue = $("#dvLicense #ExceptionValue").val();

    collectionDetail.RowVersion64 = $("#dvLicense #RowVersion").val();
    collectionDetail.GroupID = $("#dvLicense #GroupID").val();

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/EditFillingLicense",
        data: {
            'collectionDetail': collectionDetail,
            'filingFormID': FilingFormID,
            'isViewMode': false,
            'isSummary': $("#dvLicense #isSummary").val()
        },
        success: function (data) {
            $("#dvLicense").html(data);
            $('#txtGrossVolume').focus();
            initializeDropzone();
            if ($("#ServiceTypeID").val() == 1)
                GetBusinessFillingRule(collectionDetail.AccountServiceCollectionDetailId);
        }, error: function () { }
    }).always(function () {
    });
});

function calculateLicenseTotal() {
    var grossAmount = $("#txtGrossVolume").val();
    var total = 0;

    if (FilingFormID == 6) { //Liquor license
        var percentageValue = $("#txtPercentageValue").val();

        if (percentageValue > 0) {
            total = (GlobalConvertToDecimal(grossAmount) * GlobalConvertToDecimal(percentageValue) / 100);
        }
    }
    else {
        var excemptionAmount = $("#txtExemptionAmount").val();
        if (!isNaN(GlobalConvertToDecimal(excemptionAmount)) && GlobalConvertToDecimal(excemptionAmount) > GlobalConvertToDecimal(grossAmount)) {
            showAlert('error', $("#txtExemptionAmount").attr("datatitle") + ' ' + RequiredFillingMaxValueErrMsg);
            return false;
        }

        total = (isNaN(GlobalConvertToDecimal(grossAmount)) === true ? 0 : GlobalConvertToDecimal(grossAmount))
            - (isNaN(GlobalConvertToDecimal(excemptionAmount)) === true ? 0 : GlobalConvertToDecimal(excemptionAmount));

        if ($("#ServiceTypeID").val() == 1)
            total = (GlobalConvertToDecimal(grossAmount) / (LicensePercentage / 100));
        //total = GlobalConvertToDecimal(grossAmount) / 0.005;
    }

    $("#txtTotal").val(CurrencyGlobalFormat(total));
    return true;
}

$(document).on("focusout", ".calculateLicence", function (e) {
    if ($(e.relatedTarget).hasClass("closemodal")) {
        return true;
    }
    else {
        if ($("#txtGrossVolume").val() != "") {
            $("#txtGrossVolume").val(CurrencyGlobalFormat(GlobalConvertToDecimal($("#txtGrossVolume").val())));
        }
        calculateLicenseTotal();
    }
});

function SaveLicense() {
    $.validator.unobtrusive.parse('#frmFillingLicense');
    if ($("#frmFillingLicense").valid()) {
        $("#ImageIds").val(imageIds);
        var grossAmount = $("#txtGrossVolume").val();
        if (!isNaN(GlobalConvertToDecimal(grossAmount)) && GlobalConvertToDecimal(grossAmount) <= 0) {
            showAlert('error', GrossAmountRequiredMessage);
            return false;
        }
        // Check for Total Value        
        if (calculateLicenseTotal()) {
            var total = $("#txtTotal").val();
            if (isNaN(GlobalConvertToDecimal(total)) === true || GlobalConvertToDecimal(total) < 0) {
                showAlert('error', RequiredTotalValueErrMsg);
                return false;
            }
            else {
                showAlert('info', FillingProcessInfoMsg, 0);
                return true;
            }
        }
        else
            return false;
    }
    return false;
}

function SaveLicenseEdit() {
    $.validator.unobtrusive.parse('#frmFillingLicense');
    if ($("#frmFillingLicense").valid()) {
        $("#ImageIds").val(imageIds);
        var grossAmount = $("#txtGrossVolume").val();
        if (!isNaN(GlobalConvertToDecimal(grossAmount)) && GlobalConvertToDecimal(grossAmount) <= 0) {
            showAlert('error', GrossAmountRequiredMessage);
            return false;
        }
        // Check for Total Value        
        if (calculateLicenseTotal()) {
            var total = $("#txtTotal").val();
            if (isNaN(GlobalConvertToDecimal(total)) === true || GlobalConvertToDecimal(total) < 0) {
                showAlert('error', RequiredTotalValueErrMsg);
                return false;
            }
            else {
                swal({
                    title: UpdateWaringMsg,
                    type: "warning",
                    showCancelButton: true,
                    cancelButtonText: cancelBtnText,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: updateBtnText,
                    closeOnConfirm: true
                }, function (confirmed) {
                    if (confirmed) {
                        showAlert('info', FillingProcessInfoMsg, 0);
                        $("#frmFillingLicense").submit();
                    }
                    else
                        return false;
                });
            }
            return true;
        }
        else
            return false;
    }
    return false;
}

function GetBusinessFillingRule(AccountServiceCollectionDetailId) {

    $.ajax({
        url: ROOTPath + "/AccountService/GetFillingLicenseRule",
        data: { "AccountServiceCollectionDetailID": AccountServiceCollectionDetailId },
        success: function (response) {
            if (response.status) {
                LicensePercentage = response.RuleValue;
            }
            else
                showAlert("error", response.message);

        }
    });
}

//=========Unit POPUP Functions============= 
$(document).on('click', '#viewFillingUnit', function (e) {
    var collectionDetail = {};
    var element = this;
    collectionDetail.AccountServiceCollectionDetailId = $(this).attr("data-detailid");
    collectionDetail.ServiceName = $(this).attr("data-servicename");
    collectionDetail.ServicePeriod = $(this).attr("data-serviceperiod");
    collectionDetail.ServiceID = $(this).attr("data-serviceid");
    collectionDetail.ServiceTypeID = $(this).attr("data-servicetypeid");

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/FillingUnit",
        data: {
            'collectionDetail': collectionDetail, 'isViewMode': true, 'isSummary': $(this).attr("data-isSummary")
        },
        success: function (data) {
            $("#dvUnit").html(data);
            $("#btnAddUnit").hide();
            $("#Unitmodal").modal('show');
            $("#frmFillingUnit input").prop("disabled", true);
        }, error: function () { }
    }).always(function () {
    });
});

$("#dvUnit").on('blur', '#txtArea', function (e) {
    if ($("#txtArea").val() != "") {
        var UnitCost = GlobalConvertToDecimal($("#txtUnitCost").val());
        var Area = GlobalConvertToDecimal($("#txtArea").val());
        $("#txtArea").val(CurrencyGlobalFormat(GlobalConvertToDecimal($("#txtArea").val())));
        var TotalCost = Area * UnitCost;
        $("#txtTotal").val(GlobalFormat(TotalCost));
    }
    else {
        $("#txtTotal").val('');
    }
});

$(document).on('click', '.addFillingUnit', function (e) {
    var collectionDetail = {};
    var element = this;
    collectionDetail.AccountServiceCollectionDetailId = $(this).attr("data-detailid");
    collectionDetail.ServiceName = $(this).attr("data-servicename");
    collectionDetail.ServicePeriod = $(this).attr("data-serviceperiod");
    collectionDetail.RowVersion64 = $(this).attr("data-rowVersion64");
    collectionDetail.GroupID = $(this).attr("data-groupid");
    collectionDetail.ServiceID = $(this).attr("data-serviceid");
    collectionDetail.ServiceTypeID = $(this).attr("data-servicetypeid");

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/FillingUnit",
        data: { 'collectionDetail': collectionDetail, 'isViewMode': false, 'isSummary': $(this).attr("data-isSummary") },
        success: function (data) {
            $("#dvUnit").html(data);
            $('#txtArea').focus();
            $(".clearUnit").val(null);
            initializeFillingDate();
            initializeDropzone();
        }, error: function () { }
    }).always(function () {

    });
    GetUnitFillingRule(collectionDetail.AccountServiceCollectionDetailId);
});

$(document).on('click', '.editFillingUnit', function (e) {
    var collectionDetail = {};
    var element = this;
    collectionDetail.AccountServiceCollectionDetailId = $("#dvUnit #AccountServiceCollectionDetailId").val();
    collectionDetail.ServiceName = $("#dvUnit #AccountServiceCollectionDetailId").val();
    collectionDetail.ServicePeriod = $("#dvUnit #ServiceName").val();
    collectionDetail.RowVersion64 = $("#dvUnit #RowVersion").val();
    collectionDetail.GroupID = $("#dvUnit #GroupID").val();
    collectionDetail.ServiceID = $("#dvUnit #ServiceID").val();
    collectionDetail.ServiceTypeID = $("#dvUnit #ServiceTypeID").val();

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/EditFillingUnit",
        data: { 'collectionDetail': collectionDetail, 'isViewMode': false, 'isSummary': $("#dvLicense #isSummary").val() },
        success: function (data) {
            $("#dvUnit").html(data);
            $('#txtArea').focus();
            initializeDropzone();
        }, error: function () { }
    }).always(function () {
    });
});

function GetUnitFillingRule(AccountServiceCollectionDetailId) {

    $.ajax({
        url: ROOTPath + "/AccountService/GetFillingUnitRule",
        data: { "AccountServiceCollectionDetailID": AccountServiceCollectionDetailId },
        success: function (response) {
            if (response.status) {
                $("#txtUnitCost").val(CurrencyGlobalFormat(GlobalConvertToDecimal(response.RuleValue)));
            }
            else
                showAlert("error", response.message);

        }
    });
}

function calculateUnitTotal() {
    if ($("#txtArea").val() != "") {
        //var Length = GlobalConvertToDecimal($("#txtLength").val());
        //var Width = GlobalConvertToDecimal($("#txtWidth").val());
        var UnitCost = GlobalConvertToDecimal($("#txtUnitCost").val());
        //var Area = Length * Width;
        var Area = GlobalConvertToDecimal($("#txtArea").val());
        $("#txtArea").val(CurrencyGlobalFormat(GlobalConvertToDecimal($("#txtArea").val())));
        var TotalCost = Area * UnitCost;
        $("#txtTotal").val(GlobalFormat(TotalCost));
        //$("#txtLength").val(CurrencyGlobalFormat(GlobalConvertToDecimal($("#txtLength").val())));
        //$("#txtWidth").val(CurrencyGlobalFormat(GlobalConvertToDecimal($("#txtWidth").val())));
        return true;
    }

    return false;
}

function SaveUnit() {
    $.validator.unobtrusive.parse('#frmFillingUnit');
    if ($("#frmFillingUnit").valid()) {
        $("#ImageIds").val(imageIds);
        //var Length = $("#txtLength").val();
        //var Width = $("#txtWidth").val();
        //if (!isNaN(GlobalConvertToDecimal(Length)) && GlobalConvertToDecimal(Length) <= 0) {
        //    showAlert('error', LengthRequiredMessage);
        //    return false;
        //}
        //if (!isNaN(GlobalConvertToDecimal(Width)) && GlobalConvertToDecimal(Width) <= 0) {
        //    showAlert('error', WidthRequiredMessage);
        //    return false;
        //}
        // Check for Total Value        
        if (calculateUnitTotal()) {
            var total = $("#txtTotal").val();
            if (isNaN(GlobalConvertToDecimal(total)) === true || GlobalConvertToDecimal(total) < 0) {
                showAlert('error', RequiredTotalValueErrMsg);
                return false;
            }
            else {
                showAlert('info', FillingProcessInfoMsg, 0);
                return true;
            }
        }
        else
            return false;
    }
    return false;
}

function SaveUnitEdit() {
    $.validator.unobtrusive.parse('#frmFillingUnit');
    if ($("#frmFillingUnit").valid()) {
        $("#ImageIds").val(imageIds);
        //var Length = $("#txtLength").val();
        //var Width = $("#txtWidth").val();
        //if (!isNaN(GlobalConvertToDecimal(Length)) && GlobalConvertToDecimal(Length) <= 0) {
        //    showAlert('error', LengthRequiredMessage);
        //    return false;
        //}
        //if (!isNaN(GlobalConvertToDecimal(Width)) && GlobalConvertToDecimal(Width) <= 0) {
        //    showAlert('error', WidthRequiredMessage);
        //    return false;
        //}
        // Check for Total Value        
        if (calculateUnitTotal()) {
            var total = $("#txtTotal").val();
            if (isNaN(GlobalConvertToDecimal(total)) === true || GlobalConvertToDecimal(total) < 0) {
                showAlert('error', RequiredTotalValueErrMsg);
                return false;
            }
            else {
                swal({
                    title: UpdateWaringMsg,
                    type: "warning",
                    showCancelButton: true,
                    cancelButtonText: cancelBtnText,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: updateBtnText,
                    closeOnConfirm: true
                }, function (confirmed) {
                    if (confirmed) {
                        showAlert('info', FillingProcessInfoMsg, 0);
                        $("#frmFillingUnit").submit();
                    }
                    else
                        return false;
                });
            }
            return true;
        }
        else
            return false;
    }
    return false;
}

//=========Measured Water POPUP Functions
$(document).on('click', '#viewFillingMeasuredWater', function (e) {
    var collectionDetail = {};
    var element = this;
    collectionDetail.AccountServiceCollectionDetailId = $(this).attr("data-detailid");
    collectionDetail.ServiceName = $(this).attr("data-servicename");
    collectionDetail.ServicePeriod = $(this).attr("data-serviceperiod");
    var SequenceID = $(this).attr("data-sequenceid");
    var ServiceStartPeriod = $(this).attr("data-startperiodid");

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/FillingMeasuredWater",
        data: {
            'collectionDetail': collectionDetail, 'isViewMode': true, 'isSummary': $(this).attr("data-isSummary")
        },
        success: function (data) {
            $("#dvMeasuredWater").html(data);
            $("#btnAddMeasuredWater").hide();
            $('#SequenceID').val(SequenceID);
            $('#ServiceStartPeriod').val(ServiceStartPeriod);
            $("#measuredwatermodal").modal('show');
            $("#frmFillingMeasuredWater input").prop("disabled", true);
        }, error: function () { }
    }).always(function () {
    });
});

$(document).on('click', '.addFillingMeasuredWater', function (e) {
    var collectionDetail = {};
    var element = this;
    collectionDetail.AccountServiceCollectionDetailId = $(this).attr("data-detailid");
    collectionDetail.ServiceName = $(this).attr("data-servicename");
    collectionDetail.ServicePeriod = $(this).attr("data-serviceperiod");
    collectionDetail.RowVersion64 = $(this).attr("data-rowVersion64");
    collectionDetail.GroupID = $(this).attr("data-groupid");
    var SequenceID = $(this).attr("data-sequenceid");
    var ServiceStartPeriod = $(this).attr("data-startperiodid");

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/FillingMeasuredWater",
        data: { 'collectionDetail': collectionDetail, 'isViewMode': false, 'isSummary': $(this).attr("data-isSummary") },
        success: function (data) {
            $("#dvMeasuredWater").html(data);
            $('#txtPreviousMeasure').focus();
            $('#SequenceID').val(SequenceID);
            $('#ServiceStartPeriod').val(ServiceStartPeriod);
            $('.clearMeasuredWater').val(null);
            initializeFillingDate();
            initializeDropzone();
        }, error: function () { }
    }).always(function () {
    });
});

$(document).on('click', '.editFillingMeasuredWater', function (e) {
    var collectionDetail = {};
    var element = this;
    collectionDetail.AccountServiceCollectionDetailId = $("#dvMeasuredWater #AccountServiceCollectionDetailId").val();
    collectionDetail.ServiceName = $("#dvMeasuredWater #ServiceName").val();
    collectionDetail.ServicePeriod = $("#dvMeasuredWater #ServicePeriod").val();
    collectionDetail.RowVersion64 = $("#dvMeasuredWater #RowVersion").val();
    collectionDetail.GroupID = $("#dvMeasuredWater #GroupID").val();
    var SequenceID = $("#dvMeasuredWater #SequenceID").val();
    var ServiceStartPeriod = $("#dvMeasuredWater #ServiceStartPeriod").val();

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/EditFillingMeasuredWater",
        data: { 'collectionDetail': collectionDetail, 'isViewMode': false, 'isSummary': $("#dvLicense #isSummary").val() }, //query $("#dvLicense #isSummary").val() 
        success: function (data) {
            $("#dvMeasuredWater").html(data);
            $('#txtPreviousMeasure').focus();
            $('#SequenceID').val(SequenceID);
            $('#ServiceStartPeriod').val(ServiceStartPeriod);
            initializeDropzone();
        }, error: function () { }
    }).always(function () {
    });
});

$(document).on("click", "#btnDelete", function () {
    var confMessage = "";
    var textmessage = "";
    var AccountServiceID = $(this).attr("data-id");
    $("#txtReason").val('');
    $("#DeleteAccServiceConfirmModal").attr("data-accountserviceid", AccountServiceID);
    $("#DeleteAccServiceConfirmModal").modal("show");

});

$("#DeleteAccServiceConfirmModal").on("click", "#btnModalDelete", function () {
    var confMessage = "";
    var textmessage = "";
    var AccountServiceID = $("#DeleteAccServiceConfirmModal").attr("data-accountserviceid");
    if ($("#frmDeleteService").valid()) {
        swal({
            title: suremsg,
            text: textmessage,
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: confMessage,
            cancelButtonText: cancel,
            closeOnConfirm: true
        }, function () {
            DeleteRecord(AccountServiceID);
        });
    }
});

$("#DeleteConfirmModal").on("click", "#btnModalDelete", function () {
    var confMessage = "";
    var textmessage = "";
    var AccountServiceCollectionDetailId = $("#DeleteConfirmModal").attr("data-accountservicecollectionid");
    var FillingForm = $("#DeleteConfirmModal").attr("data-fillingform");
    if ($("#frmDeleteServiceFiling").valid()) {
        swal({
            title: suremsg,
            text: textmessage,
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: confMessage,
            cancelButtonText: cancel,
            closeOnConfirm: true
        }, function () {
            if (FillingForm == "1" || FillingForm == "6")
                DeleteFillingLicense(AccountServiceCollectionDetailId);
            else if (FillingForm == "3")
                DeleteFillingMeasuredWater(AccountServiceCollectionDetailId);
            else if (FillingForm == "4")
                DeleteFillingUnit(AccountServiceCollectionDetailId);
            else if (FillingForm == "5")
                DeleteFillingPropertyTax(AccountServiceCollectionDetailId);
            else if (FillingForm == "0")
                DeleteFillingAutoFiling(AccountServiceCollectionDetailId);
        });
    }
});

function DeleteRecord(AccountServiceID) {
    $.ajax({
        type: "GET",
        url: ROOTPath + '/Accounts/AccountService/Delete',
        data: { id: AccountServiceID, Reason: $("#DeleteAccServiceConfirmModal #txtReason").val(), isWindowReload: true },
        success: function (data) {
            if (data.status == true) {
                window.location.href = ROOTPath + '/AccountService/List';
            }
            else
                showAlert('error', data.message);
        },
        error: function () { }
    });

}

$(document).on("click", "#NoteDelete", function () {
    var confMessage = "";
    var textmessage = "";
    var AccountServiceID = $(this).attr("data-id");
    $("#txtReason").val('');
    $("#DeleteAccServiceNoteConfirmModal").attr("data-accountserviceid", AccountServiceID);
    $("#DeleteAccServiceNoteConfirmModal").attr("data-isaccountservicenote", $(this).attr("data-isaccountservicenote"));
    $("#DeleteAccServiceNoteConfirmModal").modal("show");

});

$("#DeleteAccServiceNoteConfirmModal").on("click", "#btnModalDelete", function () {
    var confMessage = "";
    var textmessage = "";
    var AccountServiceID = $("#DeleteAccServiceNoteConfirmModal").attr("data-accountserviceid");
    var IsAccountServiceNote = $("#DeleteAccServiceNoteConfirmModal").attr("data-isaccountservicenote");
    if ($("#frmDeleteService").valid()) {
        swal({
            title: suremsg,
            text: textmessage,
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: confMessage,
            cancelButtonText: cancel,
            closeOnConfirm: true
        }, function () {
            DeleteNoteRecord(AccountServiceID, IsAccountServiceNote);
        });
    }
});

function DeleteNoteRecord(AccountServiceID, IsAccountServiceNote) {
    $.ajax({
        type: "GET",
        url: ROOTPath + '/Accounts/AccountService/DeleteNote',
        data: { id: AccountServiceID, Reason: $("#DeleteAccServiceNoteConfirmModal #txtReason").val(), IsAccountServiceNote: IsAccountServiceNote, isWindowReload: true },
        success: function (data) {
            if (data.status == true) {
                window.location.reload();
            }
            else
                showAlert('error', data.message);
        },
        error: function () { }
    });

}

$(document).on("click", ".deleteAutoFilling", function () {

    var AccountServiceCollectionDetailId = $(this).attr("data-id");
    $("#txtReason").val('');
    $("#DeleteConfirmModal").attr("data-accountservicecollectionid", AccountServiceCollectionDetailId);
    $("#DeleteConfirmModal").attr("data-fillingform", "0");
    $("#DeleteConfirmModal").modal("show");

});

function DeleteFillingAutoFiling(AccountServiceCollectionDetailId) {
    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/DeleteFillingAutoFiling",
        data: { 'ID': AccountServiceCollectionDetailId, Reason: $("#txtReason").val() },
        success: function (data) {
            if (data.status == true) {
                location.href = ROOTPath + "/AccountService/View?accountServiceID=" + $("#hdnAccountServiceId").val();
            }
            else
                showAlert('error', data.message);
        }, error: function () { }
    }).always(function () {
    });
}

$(document).on("click", ".deleteFillingMeasuredWater", function () {

    var AccountServiceCollectionDetailId = $("#dvMeasuredWater #AccountServiceCollectionDetailId").val();
    $("#txtReason").val('');
    $("#DeleteConfirmModal").attr("data-accountservicecollectionid", AccountServiceCollectionDetailId);
    $("#DeleteConfirmModal").attr("data-fillingform", "3");
    $("#DeleteConfirmModal").modal("show");

});

function DeleteFillingMeasuredWater(AccountServiceCollectionDetailId) {
    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/DeleteFillingMeasuredWater",
        data: { 'ID': AccountServiceCollectionDetailId, Reason: $("#txtReason").val() }, //query $("#dvLicense #isSummary").val() 
        success: function (data) {
            if (data.status == true) {
                location.href = ROOTPath + "/AccountService/View?accountServiceID=" + $("#hdnAccountServiceId").val();
            }
            else
                showAlert('error', data.message);
        }, error: function () { }
    }).always(function () {
    });
}

$(document).on("click", ".DeleteFillingUnit", function () {

    var AccountServiceCollectionDetailId = $("#dvUnit #AccountServiceCollectionDetailId").val();
    $("#txtReason").val('');
    $("#DeleteConfirmModal").attr("data-accountservicecollectionid", AccountServiceCollectionDetailId);
    $("#DeleteConfirmModal").attr("data-fillingform", "4");
    $("#DeleteConfirmModal").modal("show");

});

function DeleteFillingUnit(AccountServiceCollectionDetailId) {

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/DeleteFillingUnit",
        data: { 'ID': AccountServiceCollectionDetailId, Reason: $("#txtReason").val() }, //query $("#dvLicense #isSummary").val() 
        success: function (data) {
            if (data.status == true) {
                location.href = ROOTPath + "/AccountService/View?accountServiceID=" + $("#hdnAccountServiceId").val();
            }
            else
                showAlert('error', data.message);
        }, error: function () { }
    }).always(function () {
    });



}

$(document).on("click", ".DeleteFillingPropertyTax", function () {

    var AccountServiceCollectionDetailId = $("#dvPropertyTax #AccountServiceCollectionDetailId").val();
    $("#txtReason").val('');
    $("#DeleteConfirmModal").attr("data-accountservicecollectionid", AccountServiceCollectionDetailId);
    $("#DeleteConfirmModal").attr("data-fillingform", "5");
    $("#DeleteConfirmModal").modal("show");

});

function DeleteFillingPropertyTax(AccountServiceCollectionDetailId) {

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/DeleteFillingPropertyTax",
        data: { 'ID': AccountServiceCollectionDetailId, Reason: $("#txtReason").val() }, //query $("#dvLicense #isSummary").val() 
        success: function (data) {
            if (data.status == true) {
                location.href = ROOTPath + "/AccountService/View?accountServiceID=" + $("#hdnAccountServiceId").val();
            }
            else
                showAlert('error', data.message);
        }, error: function () { }
    }).always(function () {
    });



}

$(document).on("click", ".DeleteFillingLicense", function () {

    var AccountServiceCollectionDetailId = $("#dvLicense #AccountServiceCollectionDetailId").val();
    $("#txtReason").val('');
    $("#DeleteConfirmModal").attr("data-accountservicecollectionid", AccountServiceCollectionDetailId);
    //$("#DeleteConfirmModal").attr("data-fillingform", "1");
    $("#DeleteConfirmModal").attr("data-fillingform", FilingFormID);
    $("#DeleteConfirmModal").modal("show");

});

function DeleteFillingLicense(AccountServiceCollectionDetailId) {

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/DeleteFillingLicense",
        data: { 'ID': AccountServiceCollectionDetailId, Reason: $("#txtReason").val() }, //query $("#dvLicense #isSummary").val() 
        success: function (data) {
            if (data.status == true) {
                location.href = ROOTPath + "/AccountService/View?accountServiceID=" + $("#hdnAccountServiceId").val();
            }
            else
                showAlert('error', data.message);
        }, error: function () { }
    }).always(function () {
    });



}

function calculateWaterConsumption() {
    var previousMeasure = GlobalConvertToDecimal($("#txtPreviousMeasure").val() != "" ? $("#txtPreviousMeasure").val() : "0");
    var actualMeasure = GlobalConvertToDecimal($("#txtActualMeasure").val() != "" ? $("#txtActualMeasure").val() : "0");
    var waterConsumption = actualMeasure - previousMeasure;
    $("#txtWaterConsumption").val(CurrencyGlobalFormat(waterConsumption));
    $("#txtPreviousMeasure").val(CurrencyGlobalFormat(GlobalConvertToDecimal($("#txtPreviousMeasure").val() != "" ? $("#txtPreviousMeasure").val() : "0")));
    $("#txtActualMeasure").val(CurrencyGlobalFormat(GlobalConvertToDecimal($("#txtActualMeasure").val() != "" ? $("#txtActualMeasure").val() : "0")));

    if ($("#txtPreviousMeasure").val() != "" && $("#txtActualMeasure").val() != "")
        return true;
    else
        return false;
}

function SaveMeasuredWater() {
    $.validator.unobtrusive.parse('#frmFillingMeasuredWater');
    if ($("#frmFillingMeasuredWater").valid()) {
        $("#ImageIds").val(imageIds);
        var previousMeasure = $("#txtPreviousMeasure").val();
        var actualMeasure = $("#txtActualMeasure").val();
        if ($("#SequenceID").val() != $("#ServiceStartPeriod").val() && !isNaN(GlobalConvertToDecimal(previousMeasure)) && GlobalConvertToDecimal(previousMeasure) <= 0) {
            showAlert('error', PreviousMeasureRequiredMessage);
            return false;
        }
        if (!isNaN(GlobalConvertToDecimal(actualMeasure)) && GlobalConvertToDecimal(actualMeasure) <= 0) {
            showAlert('error', ActualMeasureRequiredMessage);
            return false;
        }
        // Check for Water Consumption Value
        if (calculateWaterConsumption()) {
            var WaterConsumption = $("#txtWaterConsumption").val();
            if (isNaN(GlobalConvertToDecimal(WaterConsumption)) === true || GlobalConvertToDecimal(WaterConsumption) < 0) {
                showAlert('error', WaterConsumptionErrMsg);
                return false;
            }
            else {
                showAlert('info', FillingProcessInfoMsg, 0);
                return true;
            }
        }
        else
            return false;
    }
    return false;
}

function SaveMeasuredWaterEdit() {
    $.validator.unobtrusive.parse('#frmFillingMeasuredWater');
    if ($("#frmFillingMeasuredWater").valid()) {
        $("#ImageIds").val(imageIds);
        var previousMeasure = $("#txtPreviousMeasure").val();
        var actualMeasure = $("#txtActualMeasure").val();
        if ($("#SequenceID").val() != $("#ServiceStartPeriod").val() && !isNaN(GlobalConvertToDecimal(previousMeasure)) && GlobalConvertToDecimal(previousMeasure) <= 0) {
            showAlert('error', PreviousMeasureRequiredMessage);
            return false;
        }
        if (!isNaN(GlobalConvertToDecimal(actualMeasure)) && GlobalConvertToDecimal(actualMeasure) <= 0) {
            showAlert('error', ActualMeasureRequiredMessage);
            return false;
        }
        //Check for Water Consumption Value
        if (calculateWaterConsumption()) {
            var WaterConsumption = $("#txtWaterConsumption").val();
            if (isNaN(GlobalConvertToDecimal(WaterConsumption)) === true || GlobalConvertToDecimal(WaterConsumption) < 0) {
                showAlert('error', WaterConsumptionErrMsg);
                return false;
            }
            else {
                swal({
                    title: UpdateWaringMsg,
                    type: "warning",
                    showCancelButton: true,
                    cancelButtonText: cancelBtnText,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: updateBtnText,
                    closeOnConfirm: true
                }, function (confirmed) {
                    if (confirmed) {
                        showAlert('info', FillingProcessInfoMsg, 0);
                        $("#frmFillingMeasuredWater").submit();
                    }
                    else
                        return false;
                });
            }
            return true;
        }
        else
            return false;
    }
    return false;
}
//

//============== Property Tax =========================================
$(document).on('click', '#viewFillingPropertyTax', function (e) {
    var collectionDetail = {};
    var element = this;
    collectionDetail.AccountServiceCollectionDetailId = $(this).attr("data-detailid");
    collectionDetail.ServiceName = $(this).attr("data-servicename");
    collectionDetail.ServicePeriod = $(this).attr("data-serviceperiod");
    collectionDetail.ExceptionValue = $(this).attr("data-exceptionValue");

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/FillingPropertyTax",
        data: {
            'AccountPropertyID': $("#hdnAccountPropertyId").val(), 'collectionDetail': collectionDetail, 'isViewMode': true, 'isSummary': $(this).attr("data-isSummary")
        },
        success: function (data) {
            $("#dvPropertyTax").html(data);
            $("#btnAddPropertyTax").hide();
            $(".select2dropdown").select2({ width: '100%' });
            $("#propertytaxmodal").modal('show');
            $("#frmFillingPropertyTax input,.select2dropdown,#frmFillingPropertyTax textarea").prop("disabled", true);
        }, error: function () { }
    }).always(function () {
    });
});

$(document).on('click', '.addFillingPropertyTax', function (e) {
    var collectionDetail = {};
    var element = this;
    collectionDetail.AccountServiceCollectionDetailId = $(this).attr("data-detailid");
    collectionDetail.ServiceName = $(this).attr("data-servicename");
    collectionDetail.ServicePeriod = $(this).attr("data-serviceperiod");
    collectionDetail.RowVersion64 = $(this).attr("data-rowVersion64");
    collectionDetail.ExceptionValue = $(this).attr("data-exceptionValue");
    collectionDetail.GroupID = $(this).attr("data-groupid");

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/FillingPropertyTax",
        data: { 'AccountPropertyID': $("#hdnAccountPropertyId").val(), 'collectionDetail': collectionDetail, 'isViewMode': false, 'isSummary': $(this).attr("data-isSummary") },
        success: function (data) {
            $("#dvPropertyTax").html(data);
            $(".select2dropdown").select2({ width: '100%' });
            //$(".ContructionValue:not(:first)").prop('readonly', true);
            //$(".ComplementaryValue:not(:first)").prop('readonly', true);
            //$(".TerrainValue:not(:first)").prop('readonly', true);
            $(".clearPropertyTax").val(null);
            initializeFillingDate();
            initializeDropzone();
        }, error: function () { }
    }).always(function () {
    });
});

$(document).on('click', '.editFillingPropertyTax', function (e) {
    var collectionDetail = {};
    var element = this;
    collectionDetail.AccountServiceCollectionDetailId = $("#dvPropertyTax #AccountServiceCollectionDetailId").val();
    collectionDetail.ServiceName = $("#dvPropertyTax #ServiceName").val();
    collectionDetail.ServicePeriod = $("#dvPropertyTax #ServicePeriod").val();
    collectionDetail.RowVersion64 = $("#dvPropertyTax #RowVersion").val();
    collectionDetail.ExceptionValue = $('#ExceptionValue').val();
    collectionDetail.GroupID = $("#dvPropertyTax #GroupID").val();

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/EditFillingPropertyTax",
        data: { 'AccountPropertyID': $("#hdnAccountPropertyId").val(), 'collectionDetail': collectionDetail, 'isViewMode': false, 'isSummary': $("#dvLicense #isSummary").val() }, //query $("#dvLicense #isSummary").val() 
        success: function (data) {
            $("#dvPropertyTax").html(data);
            //$(".ContructionValue:not(:first)").prop('readonly', true);
            //$(".ComplementaryValue:not(:first)").prop('readonly', true);
            //$(".TerrainValue:not(:first)").prop('readonly', true);
            $(".select2dropdown").select2({ width: '100%' });
            var totalvalue = $("#txtTotal").val();

            //if(GlobalConvertToDecimal($(".TerrainValue:first").val()) > 0)
            //{
            $(".TerrainValue").each(function (e) {
                //if (GlobalConvertToDecimal($(this).val()) <= 0) {
                $(this).val($(".TerrainValue:first").val()).trigger('change').trigger("focusout");
                //}
            });
            //}
            //if(GlobalConvertToDecimal($(".ContructionValue:first").val()) > 0)
            //{
            //$(".ContructionValue").each(function (e) {
            //    //if (GlobalConvertToDecimal($(this).val()) <= 0) {
            //    $(this).val($(".ContructionValue:first").val()).trigger('change').trigger("focusout");
            //    //}
            //});
            ////}
            ////if(GlobalConvertToDecimal($(".ComplementaryValue:first").val()) > 0)
            ////{
            //$(".ComplementaryValue").each(function (e) {
            //    //if (GlobalConvertToDecimal($(this).val()) <= 0) {
            //    $(this).val($(".ComplementaryValue:first").val()).trigger('change').trigger("focusout");
            //    //}
            //});
            //}
            initializeDropzone();

            //if ($(".MovementTypeID").val() == "") {
            if ($(".MovementTypeID").val() == "" || ($(".MovementTypeID").val() != 3 && $(".MovementTypeID").val() != 4 && $(".MovementTypeID").val() != 5 && $(".MovementTypeID").val() != 6 && $(".MovementTypeID").val() != 13) == true) {
                $(".ContructionValue ").attr('readonly', false);
                $(".TerrainValue").attr('readonly', false);
                $(".ComplementaryValue").attr('readonly', false);
                $("#txtTotal").attr('readonly', true);

            }
            else {
                $(".ContructionValue ").attr('readonly', true);
                $(".TerrainValue").attr('readonly', true);
                $(".ComplementaryValue").attr('readonly', true);
                $("#txtTotal").val(totalvalue).attr('readonly', false);
            }

        }, error: function () { }
    }).always(function () {
    });
});

$(document).on("change", ".MovementTypeID", function () {
    var totalvalue = $("#txtTotal").val();
    if ($(this).val() == "" || ($(this).val() != 3 && $(this).val() != 4 && $(this).val() != 5 && $(this).val() != 6 && $(this).val() != 13) == true) {
        $(".ContructionValue ").attr('readonly', false);
        $(".TerrainValue").attr('readonly', false);
        $(".ComplementaryValue").attr('readonly', false);
        $("#txtTotal").val("").attr('readonly', true);
        $("#TotalValue").val("");
        $(".FillingPropertyTax:first").blur();
    }
    else {
        $(".FillingPropertyTax").each(function () {
            if ($(this).val() == "" || $("#btnAddPropertyTax").data('mode') == 'add') {
                $(this).val(CurrencyGlobalFormat(0));
            }
        });
        $(".ContructionValue ").attr('readonly', true);
        $(".TerrainValue").attr('readonly', true);
        $(".ComplementaryValue").attr('readonly', true);
        $("#txtTotal").val(totalvalue).attr('readonly', false);
        $("#TotalValue").val(totalvalue);
    }
});

function calculateTotalValue() {
    return true;
}

$(document).on("focusout", ".FillingPropertyTax", function (e) {
    if ($(this).val() == "") {
        $(this).val(CurrencyGlobalFormat(0));
    }
    if ($(e.relatedTarget).hasClass("closemodal")) {
        return true;
    }
    else {
        //if ($(this).val() != "") {
        var $Area = $(".Area");
        var $TerrainValue = $(".TerrainValue");
        //var $ConstructionArea = $(".ConstructionArea");
        //var $ComplementaryArea = $(".ComplementaryArea");        
        //var $ContructionValue = $(".ContructionValue");
        //var $ComplementaryValue = $(".ComplementaryValue");
        //if ($Area.val() != "" && $ConstructionArea.val() != "" && $ComplementaryArea.val() != "" && $TerrainValue.val() != "" && $ContructionValue.val() != "" && $ComplementaryValue.val() != "") {
        //    var Total = CurrencyGlobalFormat((GlobalConvertToDecimal($Area.val()) * GlobalConvertToDecimal($TerrainValue.val())) + (GlobalConvertToDecimal($ConstructionArea.val()) * GlobalConvertToDecimal($ContructionValue.val())) + (GlobalConvertToDecimal($ComplementaryArea.val()) * GlobalConvertToDecimal($ComplementaryValue.val())));
        //    //$(this).closest("tr").find(".TotalValue").val(Total);
        //    $("#txtTotal").val(Total);
        //    $(".TotalValue").val(Total);
        //}
        //else {
        //    $("#txtTotal").val(CurrencyGlobalFormat(0));
        //    $(".TotalValue").val(CurrencyGlobalFormat(0));
        //    //$(this).closest("tr").find(".TotalValue").val("");
        //}
        $(this).val(CurrencyGlobalFormat(GlobalConvertToDecimal($(this).val())));
        var Total = 0;
        var IsAnyMisConstrucionValue = false;
        $(".ContructionValue").each(function () {

            var $ConstructionArea = $(this).closest("tr").find(".ConstructionArea");
            var $ContructionValue = $(this).closest("tr").find(".ContructionValue");
            if ($Area.val() != "" && $ConstructionArea.val() != "" && $TerrainValue.val() != "" && $ContructionValue.val() != "") {
                Total = Total + (GlobalConvertToDecimal($ConstructionArea.val()) * GlobalConvertToDecimal($ContructionValue.val()));

            }
            else {
                $("#txtTotal").val(CurrencyGlobalFormat(0));
                $("#TotalValue").val(CurrencyGlobalFormat(0));
                IsAnyMisConstrucionValue = true;
            }
        });
        if (IsAnyMisConstrucionValue) {
            $("#txtTotal").val(CurrencyGlobalFormat(0));
            $("#TotalValue").val(CurrencyGlobalFormat(0));
        }
        else {
            Total = Total + (GlobalConvertToDecimal($Area.val()) * GlobalConvertToDecimal($TerrainValue.val()));
            $("#txtTotal").val(CurrencyGlobalFormat(Total));
            $("#TotalValue").val(CurrencyGlobalFormat(Total));
        }

        //if ($(this).hasClass("ContructionValue")) {
        //    //$(".ContructionValue").val($(this).val());
        //    updateTotalValueForPropertyTax();
        //}
        //if ($(this).hasClass("ComplementaryValue")) {
        //    $(".ComplementaryValue").val($(this).val());
        //    updateTotalValueForPropertyTax();
        //}  
        //if ($(this).hasClass("TerrainValue")) {
        //    $(".TerrainValue").val($(this).val());
        //    updateTotalValueForPropertyTax();
        //} 
        //calculatePropertyTaxTotal();
        ////GetExemptionAmountForProperty();
        //calculatePropertyTaxTotal();
    }
    //if ($(this).val() != '') {
    //    $(this).val(new Intl.NumberFormat('en-CS', { minimumFractionDigits: 2 }).format($(this).val()));
    //}

});

function updateTotalValueForPropertyTax() {
    $(".ContructionValue:not(:first)").each(function () {
        var $Area = $(this).closest("tr").find(".Area");
        var $ConstructionArea = $(this).closest("tr").find(".ConstructionArea");
        var $ComplementaryArea = $(this).closest("tr").find(".ComplementaryArea");
        var $TerrainValue = $(this).closest("tr").find(".TerrainValue");
        var $ContructionValue = $(this).closest("tr").find(".ContructionValue");
        var $ComplementaryValue = $(this).closest("tr").find(".ComplementaryValue");
        if ($Area.val() != "" && $ConstructionArea.val() != "" && $ComplementaryArea.val() != "" && $TerrainValue.val() != "" && $ContructionValue.val() != "" && $ComplementaryValue.val() != "") {
            var Total = CurrencyGlobalFormat((GlobalConvertToDecimal($Area.val()) * GlobalConvertToDecimal($TerrainValue.val())) + (GlobalConvertToDecimal($ConstructionArea.val()) * GlobalConvertToDecimal($ContructionValue.val())) + (GlobalConvertToDecimal($ComplementaryArea.val()) * GlobalConvertToDecimal($ComplementaryValue.val())));
            $(this).closest("tr").find(".TotalValue").val(Total);
        }
        else {
            $(this).closest("tr").find(".TotalValue").val("");
        }
    });


}

function SavePropertyTax() {
    $.validator.unobtrusive.parse('#frmFillingPropertyTax');
    if ($("#frmFillingPropertyTax").valid()) {
        $("#ImageIds").val(imageIds);
        //if ($(".MovementTypeID").val() != "") {
        //    $(".TotalValue").val($("#txtTotal").val());
        //}
        $("#TotalValue").val($("#txtTotal").val());
        // Check for Total Value
        if (calculateTotalValue()) {
            showAlert('info', FillingProcessInfoMsg, 0);
            return true;
        }
        else
            return false;
    }
    return false;
}

function SavePropertyTaxEdit() {
    $.validator.unobtrusive.parse('#frmFillingPropertyTax');
    if ($("#frmFillingPropertyTax").valid()) {
        $("#ImageIds").val(imageIds);

        $("#TotalValue").val($("#txtTotal").val());

        //Check for Total Value
        if (calculateTotalValue()) {
            //swal({
            //    title: UpdateWaringMsg,
            //    type: "warning",
            //    showCancelButton: true,
            //    cancelButtonText: cancelBtnText,
            //    confirmButtonColor: "#DD6B55",
            //    confirmButtonText: updateBtnText,
            //    closeOnConfirm: true
            //}, function (confirmed) {
            //    if (confirmed) {
            //        showAlert('info', FillingProcessInfoMsg, 0);
            //        $("#frmFillingPropertyTax").submit();
            //    }
            //    else
            //        return false;
            //});         
            showAlert('info', FillingProcessInfoMsg, 0);
            $("#frmFillingPropertyTax").submit();
            return true;
        }
        else
            return false;
    }
    return false;
}
//

//========= Debt Tab Functions ============= *@
function activateTab(target) {
    $(".nav-tabs li").removeClass("active");
    $(".tab-content div").removeClass("active");
    $("#li" + target).addClass("active");
    $("#tab" + target).addClass("active");

}

$("#liDebt").bind("click", function () {
    loadDebtTab("Debt", $("#liDebt").attr("data-accountserviceid"), 0);
    $("#liDebt").unbind("click");
});

function loadDebtTab(target, accountServiceId, accountServiceCollectionDetailId) {
    loadDebt(accountServiceId, accountServiceCollectionDetailId);
    activateTab(target);
}

function refreshDebtList(accountServiceId) {
    loadDebt(accountServiceId, 0);
    $('#ddlDetail').val(null).trigger('change');
}

function filterDebtList() {
    var selectedVal = $("#ddlDetail").val();
    if (selectedVal === '')
        selectedVal = 0;
    var accountServiceId = $("#ddlDetail").attr('data-accountserviceid');
    loadDebt(accountServiceId, selectedVal);
}

function loadDebt(accountServiceId, accountServiceCollectionDetailId) {
    $.ajax({
        url: ROOTPath + "/AccountService/CollectionDebtGet",
        async: false,
        data: { accountServiceId: accountServiceId, accountServiceCollectionDetailId: accountServiceCollectionDetailId },
        dataType: 'json',
        success: function (data) {
            if (data.status === true) {
                $("#collectiondebtlist").html(data.returnData);
                $('.footable').footable();
                $('#ddlDetail').val(accountServiceCollectionDetailId == 0 ? null : accountServiceCollectionDetailId).trigger('change');
            }
            else {
                showAlert('error', data.msg);
            }
        }
    });
}

//========= Discount Tab Functions ============= *@
$("#liDiscount").bind("click", function () {
    loadDiscountTab("Discount", $("#liDiscount").attr("data-accountserid"), 0);
    $("#liDiscount").unbind("click");
});

function loadDiscountTab(target, accountServiceId, accountServiceCollectionDetailId) {
    loadDiscount(accountServiceId, accountServiceCollectionDetailId);
    activateTab(target);
}

function refreshDiscountList(accountServiceId) {
    loadDiscount(accountServiceId, 0);
    $('#ddlCollectionDetail').val(null).trigger('change');
}

function filterDiscountList() {
    var selectedVal = $("#ddlCollectionDetail").val();
    if (selectedVal === '')
        selectedVal = 0;
    loadDiscount($("#liDiscount").attr("data-accountserid"), selectedVal);
}

function loadDiscount(accountServiceId, accountServiceCollectionDetailId) {
    $.ajax({
        url: ROOTPath + "/AccountService/CollectionDiscountGet",
        async: false,
        data: { accountServiceId: accountServiceId, accountServiceCollectionDetailId: accountServiceCollectionDetailId, filingFormID: FilingFormID },
        dataType: 'json',
        success: function (data) {
            if (data.status === true) {
                $("#collectiondiscountlist").html(data.returnData);
                $('.footable').footable();
                $('#ddlCollectionDetail').val(accountServiceCollectionDetailId == 0 ? null : accountServiceCollectionDetailId).trigger('change');
            }
            else {
                showAlert('error', data.msg);
            }
        }
    });
}

//========= Payment History Tab Functions ============= *@
$("#liPayment").bind("click", function () {
    loadPaymentHistoryTab("Payment", $("#liPayment").attr("data-accountserid"), 0);
    $("#liPayment").unbind("click");
});

function loadPaymentHistoryTab(target, accountServiceId, accountServiceCollectionDetailId) {
    loadPaymentHistory(accountServiceId, accountServiceCollectionDetailId);
    activateTab(target);
}

function refreshPaymentHistoryList(accountServiceId) {
    loadPaymentHistory(accountServiceId, 0);
    $('#ddlPaymentDetail').val(null).trigger('change');
}

function filterPaymentHistoryList() {
    var selectedVal = $("#ddlPaymentDetail").val();
    if (selectedVal === '')
        selectedVal = 0;
    var accountServiceId = $("#ddlPaymentDetail").attr('data-accountserviceid');
    loadPaymentHistory(accountServiceId, selectedVal);
}

function loadPaymentHistory(accountServiceId, accountServiceCollectionDetailId) {
    $.ajax({
        url: ROOTPath + "/AccountService/CollectionPaymentHistoryGet",
        async: false,
        data: { accountServiceId: accountServiceId, accountServiceCollectionDetailId: accountServiceCollectionDetailId },
        dataType: 'json',
        success: function (data) {
            if (data.status === true) {
                $("#collectionpaymenthistorylist").html(data.returnData);
                $('.footable').footable();
                $('#ddlPaymentDetail').val(accountServiceCollectionDetailId == 0 ? null : accountServiceCollectionDetailId).trigger('change');
            }
            else {
                showAlert('error', data.msg);
            }
        }
    });
}

//================================================ Payment ======================================================= *@
function loadPaymentPopup(accountServiceID) {
    $.ajax({
        url: ROOTPath + "/AccountService/IsPaymentPending",
        data: {
            'accountServiceID': accountServiceID
        },
        success: function (data) {
            if (data.status === true) {
                $("#txtPaidAmount").val('');
                $("#txtPaidAmount").removeClass("input-validation-error");
                $("#spndueAmount").text(GlobalFormatWithText(GlobalConvertToDecimal($("#hdnTotal").val())));
                $("#txtPaidAmount").next("span").removeClass("field-validation-error").addClass("field-validation-valid");
                $("#paymentmodal").modal("show");
                $("#txtPaidAmount").focus();
            }
            else {
                if (data.salesDocumentId > 0) {
                    swal({
                        title: data.message,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: OkMsg,
                        closeOnConfirm: true
                    });
                }
                else
                    showAlert('error', data.message);
            }
        }
    });
}

function SaveInvoice(accountServiceID, accountID) {
    // validation for payment
    var paidAmount = $("#txtPaidAmount").val();
    var maxallowedAmount = $("#hdnTotal").val();
    if (GlobalConvertToDecimal(paidAmount) > GlobalConvertToDecimal(maxallowedAmount)) {
        showAlert("error", ValAmountToPay);
        return false;
    }
    else if ($("#frmMakePayment").valid()) {
        showAlert('info', PaymentProcessInfoMsg, 0);
        $.ajax({
            url: ROOTPath + "/AccountService/NewPayment",
            data: {
                'accountID': accountID, 'accountServiceID': accountServiceID, 'amountToPay': $("#txtPaidAmount").val()
            },
            success: function (data) {
                toastr.clear();
                $("#paymentmodal").modal("hide");
                if (data.status === true) {
                    swal({
                        title: invoiceNo + " : " + data.invoiceNo,
                        text: invoiceGeneratedMsg, // + "\n" + goToFinanceForPaymentMsg
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: OkMsg,
                        closeOnConfirm: true
                    });
                }
                else {
                    showAlert('error', data.message);
                }
            }
        });
    }
    else
        return false;
}

/*=============================== Note ====================================================*/
function openNotePopup() {
    $("#txtAccountServiceNote").val('');
    $("#noteModal").modal('show');
}

function NoteSuccessCallback(response) {
    $("#noteModal").modal('hide');
    if (response.status === false) {
        showAlert("error", response.message);
    }
    else {
        showAlert('success', response.message);
        $("#tabNote").html(response.returnData);
        $('.full-height-scroll').slimscroll({
            height: '100%'
        });
        $("#tblNoteList").footable();
    }
}

/*================================ Issue =====================================================*/
function IssueService() {
    // Confirm Issue    
    swal({
        title: swalTitleText,
        type: "warning",
        showCancelButton: true,
        cancelButtonText: cancelBtnText,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: issue,
        closeOnConfirm: true
    }, function (confirmed) {
        if (confirmed) {
            $.ajax({
                type: "POST",
                url: ROOTPath + "/AccountService/Issue",
                data: { 'id': $("#hdnAccountServiceId").val(), 'rowVersion64': $("#hdnRowVersion64").val() },
                success: function (data) {
                    if (data.status == true) {
                        showAlert("success", data.message);
                        $("#lnkIssue").remove();
                        $("#lnkRePrint").removeClass("hide");
                        $("#dvPrintModal").html(data.htmlContent);
                        $("#printModal").modal('show');

                    }
                    else {
                        showAlert("error", data.message);
                    }
                }, error: function () { }
            }).always(function () {
            });
        }
        else
            return false;
    });
}

/*============================== Print ==================================================*/
function PrintSuccessCallback() {
    $("#hdnIsPrinted").val(true);
    $("#dvPrintModal").html("");
    $("#printModal").modal('hide');
    $("#aRePrint").attr('href', $("#aRePrint").attr('href').replace("isPrinted=False", "isPrinted=True"));
}

/*================================== Lock Service =============================================*/
$("#btnLockSave").click(function (event) {
    $("#hiddenForCustomField1").val($("#spnCustomField1").html());
    $("#hiddenForCustomField2").val($("#spnCustomField2").html());
    $("#hiddenForCustomField3").val($("#spnCustomField3").html());
    $("#hiddenForCustomField4").val($("#spnCustomField4").html());
});

function openLockPopup() {
    $("#txtLockComment").val('');
    $("#ddlLockReason").val("").trigger("change");
    $('#lockModal').modal('show');
}

function LockSuccessCallback(response) {
    if (response.status == true) {
        location.href = ROOTPath + "/AccountService/View?accountServiceID=" + $("#hdnAccountServiceId").val();
        return false;
    }
    else {

        showAlert("error", response.message);

        //if (response.isLock == false) {
        //    $('#lockModal').modal('hide');
        //}        
    }
}

//======================Adjustment POPUP Functions==========================================
function openAdjustmentPopup(accountserviceId, serviceId) {

    $.ajax({
        url: ROOTPath + "/AccountService/LoadAdjustmentPopup",
        data: {
            'accountServiceID': accountserviceId, 'serviceID': serviceId
        },
        success: function (response) {
            if (response.status == false) {
                showAlert('error', response.message);
            }
            else {
                $("#dvAdjustmentModal").html(response);
                $(".select2dropdown").select2({ width: '100%' });
                //setCollectionValues();
                //$.validator.unobtrusive.parse('#frmAccountService_Adjustment');
                $("#adjustmentModal").modal('show');
            }
        }
    });
}

function openCorrectionPopup(accountserviceId) {
    var element = this;
    var AccountServiceCollectionDetailId = $("#hdnCollectionDetailID").val();
    //collectionDetail.ServiceName = $(this).attr("data-servicename");
    //collectionDetail.ServicePeriod = $(this).attr("data-serviceperiod");
    //collectionDetail.RowVersion64 = $(this).attr("data-rowVersion64");
    //collectionDetail.ExceptionValue = $(this).attr("data-exceptionValue");
    //collectionDetail.GroupID = $(this).attr("data-groupid");

    $.ajax({
        url: ROOTPath + "/AccountService/LoadCorrectionPopup",
        data: {
            'accountServiceID': accountserviceId, 'AccountPropertyID': $("#hdnAccountPropertyId").val(), 'AccountServiceCollectionDetailId': AccountServiceCollectionDetailId
        },
        success: function (response) {
            if (response.status == false) {
                showAlert('error', response.message);
            }
            else {
                $("#dvCorrectionModal").html(response);
                $(".select2dropdown").select2({ width: '100%' });
                $(".clearPropertyTax").val(null);
                initializeFillingDate();
                //$("#correctiontModal input[type='text']").val('')
                //$.validator.unobtrusive.parse('#frmCorrectionPropertyTax');
                $("#correctiontModal").modal('show');
            }
        }
    });
}

$(document).on("change", ".CorrectionMovementTypeID", function () {
    var totalvalue = $("#txtCorrectionTotal").val();
    if ($(this).val() == "" || ($(this).val() != 3 && $(this).val() != 4 && $(this).val() != 5 && $(this).val() != 6 && $(this).val() != 13) == true) {
        $(".CorrectionContructionValue ").attr('readonly', false);
        $(".CorrectionTerrainValue").attr('readonly', false);
        $(".CorrectionComplementaryValue").attr('readonly', false);
        $("#txtCorrectionTotal").val("").attr('readonly', true);
        $("#CorrectionTotalValue").val("");
        $(".CorrectionPropertyTax:first").blur();
    }
    else {
        $(".CorrectionPropertyTax").each(function () {
            if ($(this).val() == "") {
                $(this).val(CurrencyGlobalFormat(0));
            }
        });
        $(".CorrectionContructionValue ").attr('readonly', true);
        $(".CorrectionTerrainValue").attr('readonly', true);
        $(".CorrectionComplementaryValue").attr('readonly', true);
        $("#txtCorrectionTotal").val(totalvalue).attr('readonly', false);
        $("#CorrectionTotalValue").val(totalvalue);
    }
});

$(document).on("focusout", ".CorrectionPropertyTax", function (e) {
    if ($(this).val() == "") {
        $(this).val(CurrencyGlobalFormat(0));
    }
    if ($(e.relatedTarget).hasClass("closemodal")) {
        return true;
    }
    else {
        //if ($(this).val() != "") {
        var $Area = $(".CorrectionArea");
        var $TerrainValue = $(".CorrectionTerrainValue");
        //var $ConstructionArea = $(".ConstructionArea");
        //var $ComplementaryArea = $(".ComplementaryArea");        
        //var $ContructionValue = $(".ContructionValue");
        //var $ComplementaryValue = $(".ComplementaryValue");
        //if ($Area.val() != "" && $ConstructionArea.val() != "" && $ComplementaryArea.val() != "" && $TerrainValue.val() != "" && $ContructionValue.val() != "" && $ComplementaryValue.val() != "") {
        //    var Total = CurrencyGlobalFormat((GlobalConvertToDecimal($Area.val()) * GlobalConvertToDecimal($TerrainValue.val())) + (GlobalConvertToDecimal($ConstructionArea.val()) * GlobalConvertToDecimal($ContructionValue.val())) + (GlobalConvertToDecimal($ComplementaryArea.val()) * GlobalConvertToDecimal($ComplementaryValue.val())));
        //    //$(this).closest("tr").find(".TotalValue").val(Total);
        //    $("#txtTotal").val(Total);
        //    $(".TotalValue").val(Total);
        //}
        //else {
        //    $("#txtTotal").val(CurrencyGlobalFormat(0));
        //    $(".TotalValue").val(CurrencyGlobalFormat(0));
        //    //$(this).closest("tr").find(".TotalValue").val("");
        //}
        $(this).val(CurrencyGlobalFormat(GlobalConvertToDecimal($(this).val())));
        var Total = 0;
        var IsAnyMisConstrucionValue = false;
        $(".CorrectionContructionValue").each(function () {

            var $ConstructionArea = $(this).closest("tr").find(".CorrectionConstructionArea");
            var $ContructionValue = $(this).closest("tr").find(".CorrectionContructionValue");
            if ($Area.val() != "" && $ConstructionArea.val() != "" && $TerrainValue.val() != "" && $ContructionValue.val() != "") {
                Total = Total + (GlobalConvertToDecimal($ConstructionArea.val()) * GlobalConvertToDecimal($ContructionValue.val()));

            }
            else {
                $("#txtCorrectionTotal").val(CurrencyGlobalFormat(0));
                $("#CorrectionTotalValue").val(CurrencyGlobalFormat(0));
                IsAnyMisConstrucionValue = true;
            }
        });
        if (IsAnyMisConstrucionValue) {
            $("#txtCorrectionTotal").val(CurrencyGlobalFormat(0));
            $("#CorrectionTotalValue").val(CurrencyGlobalFormat(0));
        }
        else {
            Total = Total + (GlobalConvertToDecimal($Area.val()) * GlobalConvertToDecimal($TerrainValue.val()));
            $("#txtCorrectionTotal").val(CurrencyGlobalFormat(Total));
            $("#CorrectionTotalValue").val(CurrencyGlobalFormat(Total));
        }

    }

});


//======================Exception POPUP Functions==========================================
function openExemptionPopup(accountserviceId) {

    $.ajax({
        url: ROOTPath + "/AccountService/LoadExemptionPopup",
        data: {
            'accountServiceID': accountserviceId
        },
        success: function (response) {
            if (response.status == false) {
                showAlert('error', response.message);
            }
            else {
                $("#dvExemptionModal").html(response);
                $(".select2dropdown").select2({ width: '100%' });
                //$.validator.unobtrusive.parse('#frmAccountService_Exemption');
                $("#ExemptionModal").modal('show');
            }
        }
    });
}

$(".btnExemptionEdit").on("click", function () {
    var $this = $(this);
    $.ajax({
        url: ROOTPath + "/AccountService/LoadEditExemptionPopup",
        data: {
            'accountServiceID': $("#hdnAccountServiceId").val(), 'CollectionID': $(this).attr("data-collectionid"), 'ID': $(this).attr("data-id"), 'Debt': GlobalConvertToDecimal($(this).attr("data-debt")), 'Reason': $(this).attr('data-reason')
        },
        success: function (response) {
            if (response.status == false) {
                showAlert('error', response.message);
            }
            else {
                $("#dvExemptionModal").html(response);
                $(".select2dropdown").select2({ width: '100%' });
                $.validator.unobtrusive.parse('#frmAccountService_Exemption');
                $('#CollectionDetail').select2('val', [$this.attr("data-collectionid")]);
                $("#CollectionDetail").attr("disabled", true);
                $("#ExemptionModal").modal('show');
            }
        }
    });
});

$(".btnExemptionDelete").on("click", function () {
    var $this = $(this);

    swal({
        title: swalTitleText,
        text: ExemptionDeleteText,
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: confMessage,
        cancelButtonText: cancelBtnText,
        closeOnConfirm: true
    }, function () {
        $.ajax({
            url: ROOTPath + "/AccountService/ExemptionDelete",
            data: {
                'ID': $this.attr("data-id")
            },
            success: function (data) {
                if (data.status == true) {
                    location.href = ROOTPath + "/AccountService/View?accountServiceID=" + $("#hdnAccountServiceId").val();
                }
                else
                    showAlert('error', data.message);
            }
        });
    });


});

$(".btnExemptionDeleteAll").on("click", function () {
    var $this = $(this);
    swal({
        title: swalTitleText,
        text: ExemptionDeleteAllText,
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: confMessage,
        cancelButtonText: cancelBtnText,
        closeOnConfirm: true
    }, function () {
        $.ajax({
            url: ROOTPath + "/AccountService/ExemptionDeleteAll",
            data: {
                'ID': $("#hdnAccountServiceId").val()
            },
            success: function (data) {
                if (data.status == true) {
                    location.href = ROOTPath + "/AccountService/View?accountServiceID=" + $("#hdnAccountServiceId").val();
                }
                else
                    showAlert('error', data.message);
            }
        });
    });


});

$(".btnExemptionLog").on("click", function () {
    var $this = $(this);
    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/GetExemptionHistory",
        data: { 'ID': $("#hdnAccountServiceId").val() },
        success: function (data) {
            if (data.status == false) {
                showAlert('error', data.message);
                e.stopPropagation();
            }
            else {
                $("#divExemptionHistory").html(data.returnData);
                $("#ExemptionHistory").modal('show');
            }
        }, error: function () {
        }
    }).always(function () {
    });
});

$(document).on("click", "#btnExemptionSave", function (e) {
    var selectCollectionDetailList = [];
    if ($("#CollectionDetail").select2('data').length) {
        $.each($("#CollectionDetail").select2('data'), function (key, item) {
            if (item.id != 0)
                selectCollectionDetailList += "," + item.id;
        });
        var selectedCollectionDetailIDs = "";
        if (selectCollectionDetailList.length > 0)
            selectedCollectionDetailIDs = selectCollectionDetailList.substring(1, selectCollectionDetailList.length);
        $("#AccountServiceCollectionDetailId").val(selectedCollectionDetailIDs);
    }
    if ($("#txtExemptionAmount").val() != "" && GlobalConvertToDecimal($("#txtExemptionAmount").val()) <= 0) {
        showAlert('error', RequiredTotalValueErrMsg);
        return false;
    }
    $("#frmAccountService_Exemption").submit();

});

$(document).on("click", "#btnAdjustmentSave", function (e) {
    var selectCollectionDetailList = [];
    if ($("#ddlCollectionDetail1").select2('data').length) {
        $.each($("#ddlCollectionDetail1").select2('data'), function (key, item) {
            if (item.id != 0)
                selectCollectionDetailList += "," + item.id;
        });
        var selectedCollectionDetailIDs = "";
        if (selectCollectionDetailList.length > 0)
            selectedCollectionDetailIDs = selectCollectionDetailList.substring(1, selectCollectionDetailList.length);
        $("#AccountServiceCollectionDetailId").val(selectedCollectionDetailIDs);
    }
    $("#frmAccountService_Adjustment").submit();

});

$(document).on("click", "#btnCoorectionSave", function (e) {
    $.validator.unobtrusive.parse('#frmCorrectionPropertyTax');
    if ($("#frmCorrectionPropertyTax").valid()) {
        $("#CorrectionTotalValue").val($("#txtCorrectionTotal").val());
        $("#frmCorrectionPropertyTax").submit();
    }
});

$(".btnAdjustmentDelete").on("click", function () {
    var $this = $(this);

    swal({
        title: swalTitleText,
        text: AdjustmentDeleteText,
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: confMessage,
        cancelButtonText: cancelBtnText,
        closeOnConfirm: true
    }, function () {
        $.ajax({
            url: ROOTPath + "/AccountService/AdjustmentDelete",
            data: {
                'ID': $this.attr("data-id")
            },
            success: function (data) {
                if (data.status == true) {
                    location.href = ROOTPath + "/AccountService/View?accountServiceID=" + $("#hdnAccountServiceId").val();
                }
                else
                    showAlert('error', data.message);
            }
        });
    });


});

$(document).on("change", "#CollectionDetail", function (e) {
    var Balance = 0.0;

    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#CollectionDetail option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }

    if ($("#CollectionDetail").select2('data').length) {
        $.each($("#CollectionDetail").select2('data'), function (key, item) {
            Balance += GlobalConvertToDecimal($('#dvExemptionModal #CollectionDetail option[value=' + item.id + ']').attr('data-balance'));
        });
    }
    $("#txtBalance").val(CurrencyGlobalFormat(Balance));
});

$(document).on("focusout", "#dvExemptionModal #txtExemptionAmount", function (e) {
    if ($(this).val() == "") {
        $(this).val(CurrencyGlobalFormat(0));
    }
    else {
        $(this).val(CurrencyGlobalFormat(GlobalConvertToDecimal($(this).val())));
    }
});

$(document).on("change", "#ddlCollectionDetail1", function (e) {
    var Balance = 0.0;
    var Princial = 0, Penalty = 0, Charges = 0, Interest = 0, Discount = 0;

    if ($(this).val() != null && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#ddlCollectionDetail1 option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }

    if ($("#ddlCollectionDetail1").select2('data').length) {
        $.each($("#ddlCollectionDetail1").select2('data'), function (key, item) {
            Princial = Princial + GlobalConvertToDecimal($('#dvAdjustmentModal #ddlCollectionDetail1 option[value=' + item.id + ']').attr('data-principal'));
            Penalty = Penalty + GlobalConvertToDecimal($('#dvAdjustmentModal #ddlCollectionDetail1 option[value=' + item.id + ']').attr('data-penalty'));
            Charges = Charges + GlobalConvertToDecimal($('#dvAdjustmentModal #ddlCollectionDetail1 option[value=' + item.id + ']').attr('data-charges'));
            Interest = Interest + GlobalConvertToDecimal($('#dvAdjustmentModal #ddlCollectionDetail1 option[value=' + item.id + ']').attr('data-interest'));
            Discount = Discount + GlobalConvertToDecimal($('#dvAdjustmentModal #ddlCollectionDetail1 option[value=' + item.id + ']').attr('data-discount'));
        });
    }
    if (Discount > 0)
        $("#txtPrincipal").val(CurrencyGlobalFormat(Princial - Discount));
    else
        $("#txtPrincipal").val(CurrencyGlobalFormat(Princial));
    $("#txtPenalty").val(CurrencyGlobalFormat(Penalty));
    $("#txtCharges").val(CurrencyGlobalFormat(Charges));
    $("#txtInterest").val(CurrencyGlobalFormat(Interest));
});

function setCollectionValues(sender) {
    if ($(sender).find(':selected').attr('data-principal') != undefined) {
        $("#txtPrincipal").val(CurrencyGlobalFormat(GlobalConvertToDecimal($(sender).find(':selected').attr('data-principal'))));
        $("#txtPenalty").val(CurrencyGlobalFormat(GlobalConvertToDecimal($(sender).find(':selected').attr('data-penalty'))));
        $("#txtCharges").val(CurrencyGlobalFormat(GlobalConvertToDecimal($(sender).find(':selected').attr('data-charges'))));
        $("#txtInterest").val(CurrencyGlobalFormat(GlobalConvertToDecimal($(sender).find(':selected').attr('data-interest'))));
    } else {
        var Princial = 0, Penalty = 0, Charges = 0, Interest = 0;
        $("#ddlCollectionDetail option").each(function () {
            if ($(this).attr('data-principal') != undefined) {
                Princial = Princial + GlobalConvertToDecimal($(this).attr('data-principal'));
                Penalty = Penalty + GlobalConvertToDecimal($(this).attr('data-penalty'));
                Charges = Charges + GlobalConvertToDecimal($(this).attr('data-charges'));
                Interest = Interest + GlobalConvertToDecimal($(this).attr('data-interest'));
            }
            $("#txtPrincipal").val(CurrencyGlobalFormat(Princial));
            $("#txtPenalty").val(CurrencyGlobalFormat(Penalty));
            $("#txtCharges").val(CurrencyGlobalFormat(Charges));
            $("#txtInterest").val(CurrencyGlobalFormat(Interest));
        });

        //$("#txtPrincipal").val('');
        //$("#txtPenalty").val('');
        //$("#txtCharges").val('');
        //$("#txtInterest").val('');
    }
}

function AdjustmentSuccessCallback(response) {
    if (response.status == true) {
        location.href = ROOTPath + "/AccountService/View?accountServiceID=" + $("#hdnAccountServiceId").val();
        return false;
    }
    else {
        showAlert("error", response.message);
    }
}

function CorrectionSuccessCallback(response) {
    if (response.status == true) {
        location.href = ROOTPath + "/AccountService/View?accountServiceID=" + $("#hdnAccountServiceId").val();
        return false;
    }
    else {
        showAlert("error", response.message);
    }
}

function ExemptionSuccessCallback(response) {
    if (response.status == true) {
        location.href = ROOTPath + "/AccountService/View?accountServiceID=" + $("#hdnAccountServiceId").val();
        return false;
    }
    else {
        showAlert("error", response.message);
    }
}

//======================Extension POPUP Functions==========================================
function openExtensionPopup(accountserviceId, filingDueDate) {
    $.ajax({
        type: "GET",
        url: ROOTPath + "/AccountService/FilingExtensionGet",
        data: {
            'accountserviceId': accountserviceId, 'filingDueDate': filingDueDate
        },
        success: function (data) {
            if (data.status == false) {
                showAlert("error", data.message);
            }
            else {
                $("#dvLicense").html(data);
                $('#frmFillingExtension #Months').focus();
                $("#licensemodal").modal("show");
                initializeDropzone(1);
            }
        }, error: function () { }
    }).always(function () {
    });
}

function viewExtensionPopup(id, accountserviceid) {
    $.ajax({
        type: "GET",
        url: ROOTPath + "/AccountService/FilingExtensionView",
        data: {
            'ID': id, 'accountserviceID': accountserviceid
        },
        success: function (data) {
            if (data.status == false) {
                showAlert("error", data.message);
            }
            else {
                $("#dvLicense").html(data);
                $("#frmFillingExtension input").prop("disabled", true);
                $("#frmFillingExtension select").prop("disabled", true);
                $("#licensemodal").modal("show");
            }
        }, error: function () { }
    }).always(function () {
    });
}

function changeExtensionEndDate(selectedValue) {
    if (selectedValue >= 1 && selectedValue <= 9) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/AccountService/GetEndDate",
            data: {
                'startDate': $('#StartDate').val(), 'month': selectedValue
            },
            success: function (data) {
                if (data.status == true) {
                    $('#EndDate').val(data.endDate);
                }
                else
                    showAlert("error", data.message);
            }, error: function () { }
        }).always(function () {
        });
    }
}

function SaveExtension() {
    $.validator.unobtrusive.parse('#frmFillingExtension');
    if ($("#frmFillingExtension").valid()) {
        var grossAmount = $("#txtGrossVolume").val();
        if (!isNaN(GlobalConvertToDecimal(grossAmount)) && GlobalConvertToDecimal(grossAmount) <= 0) {
            showAlert('error', GrossAmountRequiredMessage);
            return false;
        }
        // Check for Total Value
        if (calculateTaxTotal()) {
            var total = $("#txtTotal").val();
            if (isNaN(GlobalConvertToDecimal(total)) === true || GlobalConvertToDecimal(total) < 0) {
                showAlert('error', RequiredTotalValueErrMsg);
                return false;
            }
            return true;
        }
        else
            return false;
    }
    return false;
}

//==================== Custom Field Functions =============================================
$(document).on('click', '#btnEditCustomField', function (e) {

    confMsgcustomFieldUpdateForReplace = null;

    var customField = $(this).attr("data-CustomField");
    if (customField == 1) {
        confMsgcustomFieldUpdateForReplace = confMsgcustomFieldUpdate.replace("{0}", $(this).attr("data-SerCustomField1"));
    }
    if (customField == 2) {
        confMsgcustomFieldUpdateForReplace = confMsgcustomFieldUpdate.replace("{0}", $(this).attr("data-SerCustomField2"));
    }
    if (customField == 3) {
        confMsgcustomFieldUpdateForReplace = confMsgcustomFieldUpdate.replace("{0}", $(this).attr("data-SerCustomField3"));
    }
    if (customField == 4) {
        confMsgcustomFieldUpdateForReplace = confMsgcustomFieldUpdate.replace("{0}", $(this).attr("data-SerCustomField4"));
    }
    if (customField == 5) {
        confMsgcustomFieldUpdateForReplace = confMsgcustomFieldUpdate.replace("{0}", $(this).attr("data-SerCustomField5"));
    }
    if (customField == 0) {
        confMsgcustomFieldUpdateForReplace = confMsgcustomFieldUpdate.replace("{0}", $(this).attr("data-SerCustomDateField"));
    }

    var accountServiceModel = {};
    accountServiceModel.SerCustomField1 = $(this).attr("data-SerCustomField1");
    accountServiceModel.SerCustomField2 = $(this).attr("data-SerCustomField2");
    accountServiceModel.SerCustomField3 = $(this).attr("data-SerCustomField3");
    accountServiceModel.SerCustomField4 = $(this).attr("data-SerCustomField4");
    accountServiceModel.SerCustomField5 = $(this).attr("data-SerCustomField5");
    accountServiceModel.SerCustomDateField = $(this).attr("data-SerCustomDateField");
    accountServiceModel.CustomField1 = $(this).attr("data-CustomField1");
    accountServiceModel.CustomField2 = $(this).attr("data-CustomField2");
    accountServiceModel.CustomField3 = $(this).attr("data-CustomField3");
    accountServiceModel.CustomField4 = $(this).attr("data-CustomField4");
    accountServiceModel.CustomField5 = $(this).attr("data-CustomField5");
    accountServiceModel.CustomDateField = $(this).attr("data-CustomDateField");
    accountServiceModel.Year = $(this).attr("data-Year");
    accountServiceModel.ID = $("#hdnAccountServiceId").val();
    accountServiceModel.AccountID = $("#hdnAccountId").val();
    accountServiceModel.ServiceID = $("#hdnServiceID").val();
    accountServiceModel.RowVersion64 = $("#hdnRowVersion64").val();
    accountServiceModel.IsDateFieldCustomField1 = $("#IsDateFieldCustomField1").val();

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/CustomField",
        data: {
            'accountServiceModel': accountServiceModel, 'customField': $(this).attr("data-CustomField")
        },
        success: function (data) {
            $("#dvCustomFieldModal").html(data);
            $.validator.unobtrusive.parse('#frmAccountService_CustomField');
            $(".newCustomField").focus();
            $('.inputcustomdate').datepicker({
                todayHighlight: true,
                keyboardNavigation: false,
                forceParse: false,
                calendarWeeks: true,
                autoclose: true,
                format: __dateFormat,
                language: __culture
            });
            $('.CustomFieldUpdateDate').datepicker({
                todayHighlight: true,
                keyboardNavigation: false,
                forceParse: false,
                calendarWeeks: false,
                autoclose: true,
                format: __dateFormat,
                language: __culture,
                startDate: new Date(accountServiceModel.Year, 0, 1),
                endDate: new Date(accountServiceModel.Year, 11, 31)
            });

            $("#customFieldModal").modal('show');
        }, error: function () { }
    }).always(function () {
    });
});

function CustomFieldSuccessCallback(response) {
    $("#customFieldModal").modal('hide');

    if (response.status === false) {
        showAlert("error", response.message);
    }
    else {
        location.href = ROOTPath + "/AccountService/View?accountServiceID=" + $("#hdnAccountServiceId").val();
    }
}

function SaveCustomField() {
    $.validator.unobtrusive.parse('#frmAccountService_CustomField');
    if ($("#frmAccountService_CustomField").valid()) {
        swal({
            title: confMsgcustomFieldUpdateForReplace,
            type: "warning",
            showCancelButton: true,
            cancelButtonText: no,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: yes,
            closeOnConfirm: true
        }, function (confirmed) {
            if (confirmed) {
                $("#hfNewCustomDateField").val($("#CustomDateFieldNewValue").val());
                $("#frmAccountService_CustomField").submit();
            }
            else {

                return false;

            }
        });
    }
    return false;
}

//==================== Right functions ================================================
$(document).on('click', '#addRight', function (e) {
    var accountServiceModel = {};
    accountServiceModel.ID = $(this).attr("data-AccountServiceID");
    accountServiceModel.AccountID = $(this).attr("data-AccountID");
    accountServiceModel.Year = $(this).attr("data-Year");
    accountServiceModel.ServiceID = $(this).attr("data-ServiceID");
    accountServiceModel.RowVersion64 = $(this).attr("data-rowVersion64");
    accountServiceModel.AccountPropertyID = $(this).attr("data-accountpropertyid");
    accountServiceModel.ServiceTypeID = $(this).attr("data-ServiceTypeID");

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/AddRight",
        data: { 'model': accountServiceModel },
        success: function (data) {
            $("#dvRightModal").html(data);
            $.validator.unobtrusive.parse('#frmAddRight');
            $("#rightModal").modal('show');
            $(".select2dropdown").select2({ width: '100%' });
            $('#frmAddRight').validate();
            $("#ddlRight").focus();

            //Right Required
            if (
                accountServiceModel.ServiceTypeID == 20 ||  //20= PROPERTIES
                accountServiceModel.ServiceTypeID == 22 ||  //22 = PROPERTY TAX 96 / 97
                accountServiceModel.ServiceTypeID == 27 ||  //27 = ParkMaintanance
                accountServiceModel.ServiceTypeID == 24     //24 = Road Cleaning
            ) {
                $("#spRight").removeClass("hide");
            }
            else {
                $("#spRight").addClass("hide");
            }

        }, error: function () { }
    }).always(function () {
    });
});

$(document).on('click', '#btnRight', function (e) {
    var accountServiceModel = {};
    accountServiceModel.ID = $(this).attr("data-AccountServiceID");
    accountServiceModel.AccountID = $(this).attr("data-AccountID");
    accountServiceModel.Year = $(this).attr("data-Year");
    accountServiceModel.ServiceID = $(this).attr("data-ServiceID");
    accountServiceModel.RowVersion64 = $(this).attr("data-rowVersion64");

    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountService/AddRight",
        data: { 'model': accountServiceModel },
        success: function (data) {
            $("#dvRightModal").html(data);
            $(".select2dropdown").select2();
            GetAccountPropertyRightForSelect('ddlRight', dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, false);
            $.validator.unobtrusive.parse('#frmAddRight');
            $("#rightModal").modal('show');
            $('#frmAddRight').validate();
            $("#ddlRight").focus();
        }, error: function () { }
    }).always(function () {
    });
});

function RightSuccessCallback(response) {
    $("#rightModal").modal('hide');

    if (response.status === false) {
        showAlert("error", response.message);
    }
    else {
        location.href = ROOTPath + "/AccountService/View?accountServiceID=" + $("#hdnAccountServiceId").val();
    }
}

$(document).on('click', '#btnAddRight', function (e) {

    var serviceTypeID = $(this).attr("data-ServiceTypeID");

    // Right Required
    if (
        serviceTypeID == 20 ||  //20= PROPERTIES
        serviceTypeID == 22 ||  //22 = PROPERTY TAX 96 / 97
        serviceTypeID == 27 ||  //27 = ParkMaintanance
        serviceTypeID == 24     //24 = Road Cleaning
    ) {
        $("#ddlRight").addClass("required");
    }
    else {
        $("#ddlRight").removeClass("required");
        $("#rightModal").modal('hide');
    }
});

//==================== Edit License Number Functions =============================================
function openEditLicenseNumberPopup(accountserviceId) {

    $.ajax({
        url: ROOTPath + "/AccountService/EditLicenseNumber",
        data: {
            'accountServiceID': accountserviceId
        },
        success: function (response) {
            if (response.status == false) {
                showAlert('error', response.message);
            }
            else {
                $("#dvEditLicenseNumberModal").html(response);
                $("#EditLicenseNumberModal").modal('show');
                $("#txtLicenseNumber").focus();
            }
        }
    });
}

$(document).on("click", "#btnLicenseNumberSave", function (e) {
    $.validator.unobtrusive.parse('#frmAccountServiceLicenseNumber');
    if ($("#frmAccountServiceLicenseNumber").valid()) {
        $("#frmAccountServiceLicenseNumber").submit();
    }
});

function LicenseNumberSuccessCallback(response) {
    if (response.isDuplicateLicenseNumber != true) {
        $("#EditLicenseNumberModal").modal('hide');
    }

    if (response.status === false) {
        showAlert("error", response.message);
    }
    else {
        location.href = ROOTPath + "/AccountService/View?accountServiceID=" + $("#hdnAccountServiceId").val();
    }
}

function CloseLicenseNumberPopUp() {
    $("#EditLicenseNumberModal").modal('hide');
}
