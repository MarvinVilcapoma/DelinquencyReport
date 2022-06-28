var pageSize = 100;
var rowCount = pageSize;

$(document).ready(function () {
    $(".select2dropdown").select2();
    $("#ddlType").focus();

    GetAccountForSelect('AccountIDs', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px', true);
    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });
    InitializeData();
});

var hdnServiceType;
var hdnService;
var hdnAccount;

function SetResetFilterOption() {
    var PreviousSelectedData = [];

    if (hdnServiceType != undefined && hdnServiceType.length > 0) {
        PreviousSelectedData = hdnServiceType.split(",");
        $('#ServiceTypeIDs').val(PreviousSelectedData).trigger('change');
    }

    if (hdnService != undefined && hdnService.length > 0) {
        PreviousSelectedData = hdnService.split(",");
        $('#ServiceIDs').val(PreviousSelectedData).trigger('change');
    }

    if (hdnAccount != undefined && hdnAccount.length > 0) {
        PreviousSelectedData = hdnAccount.split(",");
        $('#AccountIDs').val(PreviousSelectedData).trigger('change');
    }
}

$(document).on('change', '#ServiceTypeIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#ServiceTypeIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }

    $("#ServiceIDs").empty();
    $.ajax({
        url: ROOTPath + "/Service/GetServiceByServiceTypes?serviceTypeIds=" + getServiceTypeIDs(),
        success: function (data) {
            $.each(data, function (index, optiondata) {
                $("#ServiceIDs").append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
            });
            $("#ServiceIDs").select2();
            $('#ServiceIDs').val([]).trigger('change');
            var PreviousSelectedData = [];

            if (hdnService != undefined && hdnService.length > 0) {
                PreviousSelectedData = hdnService.split(",");
                $('#ServiceIDs').val(PreviousSelectedData).trigger('change');
            }

        }
    });
});

$(document).on('change', '#ServiceIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#ServiceIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

$(document).on('change', '#AccountIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#AccountIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

function clearSearch(filterCriteria) {
    if (filterCriteria == 'servicetype') {
        $("#ServiceTypeIDs").val(null).trigger('change');
        $("#spnSelectedServiceType").addClass('hide');
        hdnServiceType = "";
        $("#spnSelectedService").addClass('hide');
        hdnService = "";
    }
    if (filterCriteria == 'services') {
        $("#ServiceIDs").val(null).trigger('change');
        $("#spnSelectedService").addClass('hide');
        hdnService = "";
    }
    if (filterCriteria == 'accounts') {
        $("#AccountIDs").val(null).trigger('change');
        $("#spnSelectedAccount").addClass('hide');
        hdnAccount = "";
    }
    InitializeData();
}

function validateExportForm(exportType) {
    if (validateForm()) {
        var myData = { TypeID: $("#ddlType").val(), FilterServiceTypeID: $("#FilterServiceTypeID").val(), FilterServiceID: $("#FilterServiceID").val(), FilterAccountID: $("#FilterAccountID").val(), "exportType": exportType }
        showLoading();
        $.fileDownload(ROOTPath + "/Report/ExportServicesLinkedToRight", {
            data: myData,           
            successCallback: function (url) {
                hideLoading();
            },
            failCallback: function (responseHtml, url) {   
                showAlert("error", errorMessage);
                hideLoading();
            }
        });
    }
}
function validateForm() {
    var isvalid = true;
    if (isvalid) {
        hdnServiceType = getServiceTypeIDs();
        hdnService = getServiceIDs();
        hdnAccount = getAccountIDs();
        $("#FilterServiceTypeID").val(hdnServiceType);
        $("#FilterServiceID").val(hdnService);
        $("#FilterAccountID").val(hdnAccount);
    }
    return isvalid;
}

function validateAdvanceSearch() {
    var isvalid = true;
    InitializeData();
    $("#advanceSearchModal").modal('hide');

    var selectedServiceTypeTexts = "";
    selectedServiceTypeTexts = getServiceTypeText();
    if (selectedServiceTypeTexts != '') {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnSelectedServiceType").removeClass('hide');
        $("#ServiceTypeIdSearchText").text(SelectedServiceType + " : " + selectedServiceTypeTexts);
    }
    else {
        $("#spnSelectedServiceType").addClass('hide');
        $("#ServiceTypeIdSearchText").text('');
    }

    var selectedServiceTexts = "";
    selectedServiceTexts = getServiceText();
    if (selectedServiceTexts != '') {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnSelectedService").removeClass('hide');
        $("#ServiceIdSearchText").text(SelectedService + " : " + selectedServiceTexts);
    }
    else {
        $("#spnSelectedService").addClass('hide');
        $("#ServiceIdSearchText").text('');
    }

    var selectedAccountTexts = "";
    selectedAccountTexts = getAccountText();
    if (selectedAccountTexts != '') {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnSelectedAccount").removeClass('hide');
        $("#AccountIdSearchText").text(SelectedAccount + " : " + selectedAccountTexts);
    }
    else {
        $("#spnSelectedAccount").addClass('hide');
        $("#AccountIdSearchText").text('');
    }

    if (isvalid) {
        hdnServiceType = getServiceTypeIDs();
        hdnService = getServiceIDs();
        hdnAccount = getAccountIDs();
    }

    return isvalid;
}

function loadAdvanceSearch() {
    if (hdnServiceType == "") {
        $('#ServiceTypeIDs').val([]).trigger('change');
        $('#ServiceIDs').val([]).trigger('change');
    }
    $("#AccountIDs").val([]).trigger('change');
    SetResetFilterOption();
    $("#advanceSearchModal").modal('show');
}

function getServiceTypeIDs() {
    var selectServiceTypeList = [];
    if ($("#ServiceTypeIDs").select2('data').length) {
        $.each($("#ServiceTypeIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectServiceTypeList += "," + item.id;
        });
    }
    var selectedServiceTypeIDs = "";
    if (selectServiceTypeList.length > 0)
        selectedServiceTypeIDs = selectServiceTypeList.substring(1, selectServiceTypeList.length);
    return selectedServiceTypeIDs;
}

function getServiceIDs() {
    var selectServiceList = [];
    if ($("#ServiceIDs").select2('data').length) {
        $.each($("#ServiceIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectServiceList += "," + item.id;
        });
    }
    var selectedServiceIDs = "";
    if (selectServiceList.length > 0)
        selectedServiceIDs = selectServiceList.substring(1, selectServiceList.length);
    return selectedServiceIDs;
}

function getAccountIDs() {
    var selectAccountList = [];
    if ($("#AccountIDs").select2('data').length) {
        $.each($("#AccountIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectAccountList += "," + item.id;
        });
    }
    var selectedAccountIDs = "";
    if (selectAccountList.length > 0)
        selectedAccountIDs = selectAccountList.substring(1, selectAccountList.length);
    return selectedAccountIDs;
}

function getServiceTypeText() {
    var selectServiceTypeList = [];
    if ($("#ServiceTypeIDs").select2('data').length) {
        $.each($("#ServiceTypeIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectServiceTypeList += "," + item.text;
        });
    }
    var selectedServiceTypeTexts = "";
    if (selectServiceTypeList.length > 0)
        selectedServiceTypeTexts = selectServiceTypeList.substring(1, selectServiceTypeList.length);
    return selectedServiceTypeTexts;
}

function getServiceText() {
    var selectServiceList = [];
    if ($("#ServiceIDs").select2('data').length) {
        $.each($("#ServiceIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectServiceList += "," + item.text;
        });
    }
    var selectedServiceTexts = "";
    if (selectServiceList.length > 0)
        selectedServiceTexts = selectServiceList.substring(1, selectServiceList.length);
    return selectedServiceTexts;
}

function getAccountText() {
    var selectAccountList = [];
    if ($("#AccountIDs").select2('data').length) {
        $.each($("#AccountIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectAccountList += "," + item.text;
        });
    }
    var selectedAccountTexts = "";
    if (selectAccountList.length > 0)
        selectedAccountTexts = selectAccountList.substring(1, selectAccountList.length);
    return selectedAccountTexts;
}

$(document).on('click', '#btnGo', function () {
    if (validateForm()) {
        InitializeData();
        return true;
    }
    return false;
});

$(window).resize(function () {
    if (validateForm()) {
        InitializeData();
        return true;
    }
    return false;
});

function InitializeDataTable() {
    $('#tblServicesLinkedToRight').DataTable({
        "scrollY": "100vh",
        "scrollX": true,
        scrollCollapse: true,
        "paging": false,
        "ordering": false,
        "info": false,
        "searching": false,
        "lengthChange": false,
        responsive: false,
        autoWidth: false,
        language: {
            "emptyTable": nodatamsg
        }
    });
}

function InitializeData() {
    $.ajax({
        type: "POST",
        url: ROOTPath + "/Reports/Report/ServicesLinkedToRight",
        data: {
            'typeID': $("#ddlType").val(),
            'commaSeperatedServiceTypeIDs': getServiceTypeIDs(),
            'commaSeperatedServiceIDs': getServiceIDs(),
            'commaSeperatedAccountIDs': getAccountIDs(),
            'pageIndex': 1,
            'pageSize': pageSize,
            'isLoadMore': false
        },
        success: function (response) {
            if (response.status) {
                hideLoading();
                $("#divServicesLinkedToRight").html("").html(response.data);
                InitializeDataTable();
                rowCount = pageSize;
            }
            else {
                showAlert('error', response.data);
            }
        }
    });
}

function loadMoreServicesLinkedToRight(element) {
    var currentQualitativePage = $(element).data('currentpage') ? $(element).data('currentpage') : 1;
    var newPage = currentQualitativePage + 1;

    $.ajax({
        type: "POST",
        url: ROOTPath + "/Reports/Report/ServicesLinkedToRight",
        data: {
            'typeID': $("#ddlType").val(),
            'commaSeperatedServiceTypeIDs': getServiceTypeIDs(),
            'commaSeperatedServiceIDs': getServiceIDs(),
            'commaSeperatedAccountIDs': getAccountIDs(),
            'pageIndex': newPage,
            'pageSize': pageSize,
            'isLoadMore': true
        },
        success: function (response) {
            $(response.data).insertBefore("#tbodyServicesLinkedToRight");
            $(element).data('currentpage', newPage);
            currentQualitativePage = newPage;
            rowCount = rowCount + response.RowCount;
            if (rowCount < $("#TotalRecord").val()) {
                $("#tbodyServicesLinkedToRight").show();
            }
            else {
                $("#tbodyServicesLinkedToRight").hide();
            }
        }
    });
}

function Print() {
    if (validateForm()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintServicesLinkedToRight",
            data: {
                'typeID': $("#ddlType").val(),
                'commaSeperatedServiceTypeIDs': getServiceTypeIDs(),
                'commaSeperatedServiceIDs': getServiceIDs(),
                'commaSeperatedAccountIDs': getAccountIDs()
            },
            beforeSend: function () {
                showLoading();
            },
            success: function (response) {
                if (response.status) {
                    hideLoading();
                    var printWindow = window.open('', '_blank');
                    printWindow.document.write(response.data);
                    var fun = function () {
                        printWindow.document.close();
                        setTimeout(function () { printWindow.print(); }, 20);
                        printWindow.onfocus = function () { setTimeout(function () { printWindow.close(); }, 20); };
                    };
                    var img = printWindow.document.getElementById('img-responsive');
                    if (img.complete) {
                        fun.call(img);
                    } else {
                        img.onload = fun
                    }
                }
                else {
                    showAlert('error', response.data);
                }
            }
        });
    }
    else
        return false;
    return true;
}