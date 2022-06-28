var pageSize = 100;
var rowCount = pageSize;

$(document).ready(function () {
    $(".select2dropdown").select2();
    $("#ddlType").focus();

    GetAccountForSelect('AccountIDs', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px', true);
    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });
});

var hdnAccount;
var hdnService;

function SetResetFilterOption() {
    var PreviousSelectedData = [];
    if (hdnAccount != undefined && hdnAccount.length > 0) {
        PreviousSelectedData = hdnAccount.split(",");
        $('#AccountIDs').val(PreviousSelectedData).trigger('change');
    }
    if (hdnService != undefined && hdnService.length > 0) {
        PreviousSelectedData = hdnService.split(",");
        $('#ServiceIDs').val(PreviousSelectedData).trigger('change');
    }
}

$(document).on('change', '#AccountIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#AccountIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

$(document).on('change', '#ServiceIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#ServiceIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

function clearSearch(filterCriteria) {
    if (filterCriteria == 'accounts') {
        $("#AccountIDs").val(null).trigger('change');
        $("#spnSelectedAccount").addClass('hide');
        hdnAccount = "";
    }
    if (filterCriteria == 'services') {
        $("#ServiceIDs").val(null).trigger('change');
        $("#spnSelectedService").addClass('hide');
        hdnService = "";
    }
    InitializeData();
}

function validateForm() {
    var isvalid = true;

    if (isvalid) {
        hdnAccount = getAccountIDs();
        hdnService = getServiceCodes();
        $("#FilterAccountID").val(hdnAccount);
        $("#FilterServiceCodes").val(hdnService);
    }
    return isvalid;
}

function validateAdvanceSearch() {
    var isvalid = true;
    InitializeData();
    $("#advanceSearchModal").modal('hide');

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

    if (isvalid) {
        hdnAccount = getAccountIDs();
        hdnService = getServiceCodes();
    }

    return isvalid;
}

function loadAdvanceSearch() {
    $("#AccountIDs").val(null).trigger('change');
    $('#ServiceIDs').val(null).trigger('change');
    SetResetFilterOption();
    $("#advanceSearchModal").modal('show');
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

function getServiceCodes() {
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
    $('#tblMigrationValidationCobros').DataTable({
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
        url: ROOTPath + "/Reports/Report/MigrationValidationCobros",
        data: {
            'typeID': $("#ddlType").val(),
            'serviceCodes': getServiceCodes(),
            'accountIDs': getAccountIDs(),
            'pageIndex': 1,
            'pageSize': pageSize,
            'isLoadMore': false
        },
        success: function (response) {
            if (response.status) {
                hideLoading();
                $("#divMigrationValidationCobros").html("").html(response.data);
                InitializeDataTable();
                rowCount = pageSize;
            }
            else {
                showAlert('error', response.data);
            }
        }
    });
}

function loadMoreMigrationValidationCobros(element) {
    var currentQualitativePage = $(element).data('currentpage') ? $(element).data('currentpage') : 1;
    var newPage = currentQualitativePage + 1;

    $.ajax({
        type: "POST",
        url: ROOTPath + "/Reports/Report/MigrationValidationCobros",
        data: {
            'typeID': $("#ddlType").val(),
            'serviceCodes': getServiceCodes(),
            'accountIDs': getAccountIDs(),
            'pageIndex': newPage,
            'pageSize': pageSize,
            'isLoadMore': true
        },
        success: function (response) {
            $(response.data).insertBefore("#tbodyMigrationValidationCobros");
            $(element).data('currentpage', newPage);
            currentQualitativePage = newPage;
            rowCount = rowCount + response.RowCount;
            if (rowCount < $("#TotalRecord").val()) {
                $("#tbodyMigrationValidationCobros").show();
            }
            else {
                $("#tbodyMigrationValidationCobros").hide();
            }
        }
    });
}