var isRemoveFromDir = true;
var attachmentDropzone;
var htmlString = "";

$(document).ready(function () {

    $('.periodDate').datepicker({
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: "MM yyyy",
        language: __culture
    });

    $('.periodDate').datepicker('update', new Date());

    ClearFile();
    initializeDropzone();
    $("#frmReValidateMeasuredWaterFiling").hide();
    $("#frmReValidateMeasuredWaterFiling #btnDownload").hide();
    $("#frmReValidateMeasuredWaterFiling #btnImport").hide();
});

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

//========= DropZone Control Functions============= *@
function initializeDropzone() {
    $('#attachmentDropzone').dropzone({
        url: ROOTPath + "/AccountService/UploadFileForValidateMeasuredWaterFiling",
        autoProcessQueue: true,
        addRemoveLinks: true,
        maxFiles: 5,
        maxFilesize: maxFileLength,
        acceptedFiles: allowedFileTypes, // issue
        parallelUploads: 1,
        uploadMultiple: false,
        dictRemoveFile: removefile,
        dictDefaultMessage: dropzonemessage,
        init: function () {
            attachmentDropzone = this;

            this.on("addedfile", function (file) {

                if (this.files.length > this.options.maxFiles) {
                    isRemoveFromDir = false;
                    this.removeFile(file);
                    showAlert("warning", Uploadedfilemaxlimitmessage);
                    return false;
                }

                if (file.size == 0) {
                    isRemoveFromDir = false;
                    this.removeFile(file);
                    showAlert("warning", EmptyFileMsg);
                    return false;
                }

                if (file.size > this.options.maxFilesize) {
                    isRemoveFromDir = false;
                    this.removeFile(file);
                    showAlert("warning", Uploadedfilemaxsizemessage + file.name);
                    return false;
                }

                if (allowedFileTypes.indexOf(file.type) < 0) {
                    showAlert("warning", uploadfiletypemsg);
                    isRemoveFromDir = false;
                    this.removeFile(file);
                    return false;
                }

                if (this.files.length) { //Prevent Duplicate File Upload //tested
                    var _i, _len;
                    for (_i = 0, _len = this.files.length; _i < _len - 1; _i++) // -1 to exclude current file
                    {
                        if (this.files[_i].name === file.name && this.files[_i].size === file.size && this.files[_i].lastModifiedDate.toString() === file.lastModifiedDate.toString()) {
                            isRemoveFromDir = false;
                            this.removeFile(file);
                        }
                    }
                }

            });
            this.on("removedfile", function (file) {
                if (!isRemoveFromDir) {
                    isRemoveFromDir = true;
                }
                else {
                    $.ajax({
                        url: ROOTPath + "/AccountService/RemoveUploadImportFile",
                        data: { 'fileName': file.NewFileName },
                        success: function (data) {
                            if (data.status == false) {
                                showAlert("error", data.message);
                            }
                        }, error: function () {
                        }
                    }).always(function () {
                    });
                }

            });
        },
        success: function (file, response) {
            file.NewFileName = response.newFileName;
        }
    });
}

function ClearFile() {
    if (attachmentDropzone != undefined) {
        attachmentDropzone.removeAllFiles();
    }
}

$("#frmReValidateMeasuredWaterFiling #btnReValidate").on("click", function () {
    if ($(this).valid()) {
        ImportMeasuredWaterFilingFieldModel = {};
        errorList = [];

        $('#tblInValidDetailRow tr').each(function () {
            $row = $(this);
            ErrorViewModel = {};
            ErrorViewModel.CATEGORIA = ($row.find('#CATEGORIA').val() != null || $row.find('#CATEGORIA').val() != '') ? $row.find('#CATEGORIA').val().trim() : null;
            ErrorViewModel.TaxNumber = ($row.find('#TaxNumber').val() != null || $row.find('#TaxNumber').val() != '') ? $row.find('#TaxNumber').val().trim() : null;
            ErrorViewModel.UBICACION = ($row.find('#UBICACION').val() != null || $row.find('#UBICACION').val() != '') ? $row.find('#UBICACION').val().trim() : null;
            ErrorViewModel.CODIGOM = ($row.find('#CODIGOM').val() != null || $row.find('#CODIGOM').val() != '') ? $row.find('#CODIGOM').val().trim() : null;
            ErrorViewModel.LECTURAACT = $row.find('#LECTURAACT').val();
            ErrorViewModel.LastReading = $row.find('#LastReading').val();
            ErrorViewModel.Difference = $row.find('#lblDifference').text();
            ErrorViewModel.FECHA = ($row.find('#FECHA').val() != null || $row.find('#FECHA').val() != '') ? $row.find('#FECHA').val().trim() : null;
            ErrorViewModel.Note = ""; // $row.find('#Note').val();
            ErrorViewModel.IsValid = $row.find('#ddlIsValid').val();// $row.find('#IsValid').val();
            ErrorViewModel.RowNo = $row.find('#hdfRowNo').val();
            errorList.push(ErrorViewModel);

        });

        ImportMeasuredWaterFilingFieldModel.UploadedFileNames = $("#UploadedFileNames").val();

        if ($("input[name='ProcessType']:checked").val() == 1)//Already Saved Data
        {
            ImportMeasuredWaterFilingFieldModel.PeriodDate = new Date($("#hdProcessedYear").val(), parseInt($("#hdProcessedMonth").val()) - 1, 01, 0, 0, 0, 0).toDateString();
        }
        else {
            ImportMeasuredWaterFilingFieldModel.PeriodDate = new Date($("#PeriodDate").datepicker('getDate')).toDateString();
        }
        ImportMeasuredWaterFilingFieldModel.MeasuredWaterFilingList = errorList;

        $.ajax({
            url: ROOTPath + '/AccountService/ReValidateMeasuredWaterFiling',
            data: { 'model': ImportMeasuredWaterFilingFieldModel },
            type: 'POST',
            success: function (response) {
                if (!response.status) {
                    showAlert("error", response.message);
                }
                else {
                    $("#frmReValidateMeasuredWaterFiling #divValidation").html(response.data);
                    InitFooTable($('#tblInValidRecordsResult'));

                    if (response.valid) {
                        $("#frmReValidateMeasuredWaterFiling #btnReValidate").hide();
                        $("#frmReValidateMeasuredWaterFiling #btnDownload").show();
                        $("#frmReValidateMeasuredWaterFiling #btnImport").show();
                        $("#frmReValidateMeasuredWaterFiling #btnInconsistencies").hide();
                        $("#frmReValidateMeasuredWaterFiling #btnHighConsumption").hide();
                    }
                    else {
                        $("#frmReValidateMeasuredWaterFiling #btnReValidate").show();
                        $("#frmReValidateMeasuredWaterFiling #btnDownload").hide();
                        $("#frmReValidateMeasuredWaterFiling #btnImport").hide();
                    }

                    showAlert('success', response.message);
                }
            }
        });
    }
});

$("#frmValidateMeasuredWaterFiling #btnValidate").on("click", function () {

    if (!$("#frmValidateMeasuredWaterFiling").valid())
        return false;
    else if (attachmentDropzone.getAcceptedFiles().length == 0) {
        showAlert("error", AtleastOneFileRequiredForValidateMeasuredWaterMsg);
        return false;
    }
    else {
        var arrFileName = [];
        $(attachmentDropzone.getAcceptedFiles()).each(function (index, element) {
            arrFileName.push(element.NewFileName);
        });

        $("#UploadedFileNames").val(arrFileName);
        $("#hdPeriodDate").val(new Date($("#PeriodDate").datepicker('getDate')).toDateString());

        formdata = new FormData();
        var other_data = $('#frmValidateMeasuredWaterFiling').serializeArray();
        $.each(other_data, function (key, input) {
            formdata.append(input.name, input.value);
        });

        $.ajax({
            type: 'POST',
            //async: false,
            url: ROOTPath + '/AccountService/ValidateMeasuredWaterFiling',
            data: formdata,
            contentType: false,
            processData: false,
            success: function (response) {
                if (!response.status) {
                    showAlert("error", response.message);
                }
                else {
                    $("#frmValidateMeasuredWaterFiling").hide();
                    $("#frmReValidateMeasuredWaterFiling #divValidation").html(response.data);
                    InitFooTable($('#tblInValidRecordsResult'));
                    $("#frmReValidateMeasuredWaterFiling").show();

                    if (response.valid) {
                        $("#frmReValidateMeasuredWaterFiling #btnReValidate").hide();
                        $("#frmReValidateMeasuredWaterFiling #btnDownload").show();
                        $("#frmReValidateMeasuredWaterFiling #btnImport").show();
                        $("#frmReValidateMeasuredWaterFiling #btnInconsistencies").hide();
                        $("#frmReValidateMeasuredWaterFiling #btnHighConsumption").hide();
                    }
                    else {
                        $("#frmReValidateMeasuredWaterFiling #btnReValidate").show();
                        $("#frmReValidateMeasuredWaterFiling #btnDownload").hide();
                        $("#frmReValidateMeasuredWaterFiling #btnImport").hide();
                    }

                    showAlert('success', response.message);
                }
            }
        });

    }
});

function Edit(ele) {
    htmlString = "";
    htmlString = ele;
    ImportMeasuredWaterFilingFieldModel = {};

    MeasuredWaterFilingList = [];
    MeasuredWaterFilingModel = {};
    MeasuredWaterFilingModel.CATEGORIA = $(ele).parents("tr").find("td:eq(0) #lblCATEGORIA").html();
    MeasuredWaterFilingModel.TaxNumber = $(ele).parents("tr").find("td:eq(1) #lblTaxNumber").html();
    MeasuredWaterFilingModel.UBICACION = $(ele).parents("tr").find("td:eq(2) #lblUBICACION").html();
    MeasuredWaterFilingModel.CODIGOM = $(ele).parents("tr").find("td:eq(3) #lblCODIGOM").html();
    MeasuredWaterFilingModel.LECTURAACT = $(ele).parents("tr").find("td:eq(4) #lblLECTURAACT").html();
    MeasuredWaterFilingModel.LastReading = $(ele).parents("tr").find("td:eq(5) #lblLastReading").html();
    MeasuredWaterFilingModel.Difference = $(ele).parents("tr").find("td:eq(6) #lblDifference").html();
    MeasuredWaterFilingModel.FECHA = $(ele).parents("tr").find("td:eq(7) #lblFECHA").html();
    MeasuredWaterFilingModel.Note = "";
    MeasuredWaterFilingModel.IsValid = true;
    MeasuredWaterFilingModel.RowNo = $(ele).parents("tr").find("#hdfRowNo").val();
    MeasuredWaterFilingList.push(MeasuredWaterFilingModel);

    ImportMeasuredWaterFilingFieldModel.MeasuredWaterFilingList = MeasuredWaterFilingList;

    if ($("input[name='ProcessType']:checked").val() == 1)//Already Saved Data
    {
        ImportMeasuredWaterFilingFieldModel.PeriodDate = new Date($("#hdProcessedYear").val(), parseInt($("#hdProcessedMonth").val()) - 1, 01, 0, 0, 0, 0).toDateString();
    }
    else {
        ImportMeasuredWaterFilingFieldModel.PeriodDate = new Date($("#PeriodDate").datepicker('getDate')).toDateString();
    }

    $.ajax({
        type: "POST",
        //async: false,
        url: ROOTPath + "/AccountService/EditValidateMeasuredWaterFiling",
        data: { 'model': ImportMeasuredWaterFilingFieldModel },
        success: function (data) {
            $("#dvmeasuredwaterfiling").html(data);
            $.validator.unobtrusive.parse('#frmSaveValidateMeasuredWaterFiling');
            $("#measuredwaterfilingmodal").modal("show");
            $("#LECTURAACT").focus();
        }, error: function () {
        }
    }).always(function () {
    });
}

function CalculateDifference() {
    var currentReading = $("#frmSaveValidateMeasuredWaterFiling #txtLECTURAACT").val() == '' ? 0 : parseInt($("#frmSaveValidateMeasuredWaterFiling #txtLECTURAACT").val());
    var lastReading = $("#frmSaveValidateMeasuredWaterFiling #txtLastReading").val() == '' ? 0 : parseInt($("#frmSaveValidateMeasuredWaterFiling #txtLastReading").val());
    var difference = currentReading - lastReading;
    $("#frmSaveValidateMeasuredWaterFiling #hdDifference").val(difference);
    $("#frmSaveValidateMeasuredWaterFiling #lblDifference").html(difference);
}

$(document).on('click', '#btnUpdate', function () {
    if ($('#frmSaveValidateMeasuredWaterFiling').valid()) {
        ImportMeasuredWaterFilingFieldModel = {};
        MeasuredWaterFilingModel = {};

        MeasuredWaterFilingModel.CATEGORIA = $("#frmSaveValidateMeasuredWaterFiling #hdCATEGORIA").val();
        MeasuredWaterFilingModel.TaxNumber = $("#frmSaveValidateMeasuredWaterFiling #hdTaxNumber").val();
        MeasuredWaterFilingModel.UBICACION = $("#frmSaveValidateMeasuredWaterFiling #hdUbicacion").val();
        MeasuredWaterFilingModel.CODIGOM = $("#frmSaveValidateMeasuredWaterFiling #hdCODIGOM").val();
        MeasuredWaterFilingModel.LECTURAACT = $("#frmSaveValidateMeasuredWaterFiling #txtLECTURAACT").val();
        MeasuredWaterFilingModel.LastReading = $("#frmSaveValidateMeasuredWaterFiling #txtLastReading").val();
        MeasuredWaterFilingModel.Difference = $("#frmSaveValidateMeasuredWaterFiling #hdDifference").val();
        MeasuredWaterFilingModel.FECHA = $("#frmSaveValidateMeasuredWaterFiling #hdFECHA").val();
        MeasuredWaterFilingModel.Note = "";
        MeasuredWaterFilingModel.IsValid = true;
        MeasuredWaterFilingModel.RowNo = $("#frmSaveValidateMeasuredWaterFiling #hfEditRowNo").val();

        ImportMeasuredWaterFilingFieldModel.MeasuredWaterFilingModel = MeasuredWaterFilingModel;

        if ($("input[name='ProcessType']:checked").val() == 1)//Already Saved Data
        {
            ImportMeasuredWaterFilingFieldModel.PeriodDate = new Date($("#hdProcessedYear").val(), parseInt($("#hdProcessedMonth").val()) - 1, 01, 0, 0, 0, 0).toDateString();
        }
        else {
            ImportMeasuredWaterFilingFieldModel.PeriodDate = new Date($("#PeriodDate").datepicker('getDate')).toDateString();
        }

        $.ajax({
            url: ROOTPath + '/AccountService/SaveValidateMeasuredWaterFiling',
            data: { 'model': ImportMeasuredWaterFilingFieldModel },
            type: 'POST',
            success: function (response) {
                if (!response.status) {
                    showAlert("error", response.message);
                }
                else {
                    var ele = htmlString;
                    $(ele).parents("tr").find("td:eq(4) #lblLECTURAACT").html(response.data.LECTURAACT);
                    $(ele).parents("tr").find("td:eq(5) #lblLastReading").html(response.data.LastReading);
                    $(ele).parents("tr").find("td:eq(6) #lblDifference").html(response.data.Difference);
                    $("#measuredwaterfilingmodal").modal("hide");
                    showAlert('success', response.message);
                }
            }
        });
    }
    else
        return false;
});

$("#btnDownload").on("click", function () {
    showLoading();

    var processedPeriodDate = null;
    if ($("input[name='ProcessType']:checked").val() == 1)//Already Saved Data
    {
        processedPeriodDate = new Date($("#hdProcessedYear").val(), parseInt($("#hdProcessedMonth").val()) - 1, 01, 0, 0, 0, 0).toDateString();
    }
    else {
        processedPeriodDate = new Date($("#PeriodDate").datepicker('getDate')).toDateString();
    }
    var qStr = ROOTPath + "/AccountService/DownloadValidateMeasuredWaterFiling?periodDate=" + processedPeriodDate;
    $.fileDownload(qStr, {
        successCallback: function (url) {
            hideLoading();
        },
        failCallback: function (responseHtml, url) {
            showAlert("error", errorMessage);
            hideLoading();
        }
    });
});

$("#btnImport").on("click", function () {
    //showLoading();
    showAlert('info', importMeasuredWaterFillingProcessInfoMsg, 0);

    var processedPeriodDate = null;
    if ($("input[name='ProcessType']:checked").val() == 1)//Already Saved Data
    {
        processedPeriodDate = new Date($("#hdProcessedYear").val(), parseInt($("#hdProcessedMonth").val()) - 1, 01, 0, 0, 0, 0).toDateString();
    }
    else {
        processedPeriodDate = new Date($("#PeriodDate").datepicker('getDate')).toDateString();
    }

    $.ajax({
        url: ROOTPath + '/AccountService/ImportMeasuredWaterFilingSave',
        data: { 'periodDate': processedPeriodDate },
        type: 'POST',
        success: function (response) {
            if (!response.status) {
                showAlert("error", response.message);
                hideLoading();
            }
            else {
                $(".toast-info").hide();
                window.location.href = ROOTPath + "/Accounts/AccountService/List";

                //$('#divValidation').hide();
                //$('#btnImport').hide();
                showAlert("success", response.message);
            }
        }
    });
});
//*********************Process Existing Data or Upload New File******************************************
$(document).on('change', '.processtype', function () {
    //Process existing data
    if ($(this).val() == 1) {
        ProcessAlreadySavedData(null);
    }
    //upload new file
    if ($(this).val() == 2) {
        $("#divProcessType").addClass("hidden");
        $("#divUploadFile").removeClass("hidden");
        $("#frmValidateMeasuredWaterFiling #btnValidate").removeClass("hidden");
    }
});

function ProcessAlreadySavedData(filterText) {
    $.ajax({
        type: 'POST',
        url: ROOTPath + '/AccountService/ProcessAlreadySavedData',
        data:
        {
            'periodYear': $("#hdProcessedYear").val(),
            'periodMonth': $("#hdProcessedMonth").val(),
            'filterText': filterText
        },
        type: 'POST',
        success: function (response) {
            if (!response.status) {
                showAlert("error", response.message);
            }
            else {
                $("#frmValidateMeasuredWaterFiling").hide();
                $("#frmReValidateMeasuredWaterFiling #divValidation").html(response.data);
                $("#txtSearch").val(response.searchText);
                InitFooTable($('#tblInValidRecordsResult'));
                if (response.valid) {
                    $("#frmReValidateMeasuredWaterFiling #btnReValidate").hide();
                    $("#frmReValidateMeasuredWaterFiling #btnDownload").show();
                    $("#frmReValidateMeasuredWaterFiling #btnImport").show();
                    $("#frmReValidateMeasuredWaterFiling #btnInconsistencies").hide();
                    $("#frmReValidateMeasuredWaterFiling #btnHighConsumption").hide();
                }
                else {
                    $("#frmReValidateMeasuredWaterFiling #btnReValidate").show();
                    $("#frmReValidateMeasuredWaterFiling #btnDownload").hide();
                    $("#frmReValidateMeasuredWaterFiling #btnImport").hide();
                }
                $("#frmReValidateMeasuredWaterFiling").show();
            }
        }
    });
}

//*********************************** Search ***************************************************
$(document).on('click', '#btnSearch', function () {
    ProcessAlreadySavedData($("#txtSearch").val());
});

$(document).on('click', '#btnrefresh', function () {
    ProcessAlreadySavedData(null);
});

$(document).on('change', '#txtSearch', function () {
    ProcessAlreadySavedData($("#txtSearch").val());
});

//********************************** Enable / Disable Row *************************************
function EnableDisable(e) {
    var isValid = $(e).val() == 'true' ? true : false;
    var row = $(e).closest('tr');

    if (isValid)
        $(row).find("input").attr("disabled", true);
    else
        $(row).find("input").attr("disabled", false);
}

////********************************** Calculate Difference *************************************
//function CalculateDifference(e) {   
//    var row = $(e).closest('tr');
//    var currentReading = int.parse($(row).find("LECTURAACT").val());
//    var lastReading = int.parse($(row).find("LastReading").val());
//    var difference = currentReading - lastReading;
//    $(row).find("lblDifference").text();
//}

//*********************************** Download Report **********************************************
$("#btnInconsistencies").on("click", function () {
    showLoading();

    var processedPeriodDate = null;
    if ($("input[name='ProcessType']:checked").val() == 1)//Already Saved Data
    {
        processedPeriodDate = new Date($("#hdProcessedYear").val(), parseInt($("#hdProcessedMonth").val()) - 1, 01, 0, 0, 0, 0).toDateString();
    }
    else {
        processedPeriodDate = new Date($("#PeriodDate").datepicker('getDate')).toDateString();
    }
    var qStr = ROOTPath + "/AccountService/DownloadValidateMeasuredWaterFilingInconsistencies?periodDate=" + processedPeriodDate;
    $.fileDownload(qStr, {
        successCallback: function (url) {
            hideLoading();
        },
        failCallback: function (responseHtml, url) {
            showAlert("error", errorMessage);
            hideLoading();
        }
    });
});

$("#btnHighConsumption").on("click", function () {
    showLoading();

    var processedPeriodDate = null;
    if ($("input[name='ProcessType']:checked").val() == 1)//Already Saved Data
    {
        processedPeriodDate = new Date($("#hdProcessedYear").val(), parseInt($("#hdProcessedMonth").val()) - 1, 01, 0, 0, 0, 0).toDateString();
    }
    else {
        processedPeriodDate = new Date($("#PeriodDate").datepicker('getDate')).toDateString();
    }
    var qStr = ROOTPath + "/AccountService/DownloadValidateMeasuredWaterFilingHighConsumption?periodDate=" + processedPeriodDate;
    $.fileDownload(qStr, {
        successCallback: function (url) {
            hideLoading();
        },
        failCallback: function (responseHtml, url) {
            showAlert("error", errorMessage);
            hideLoading();
        }
    });
});