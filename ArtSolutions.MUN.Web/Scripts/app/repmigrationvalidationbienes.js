var pageSize = 100;
var rowCount = pageSize;

$(document).ready(function () {
    $(".select2dropdown").select2();
    $("#ddlType").focus();

    GetAccountForSelect('AccountIDs', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px', true);
    GetAccountPropertyRightForSelect('PropertyIDs', dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, true);
    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });
});

var hdnAccount;
var hdnProperty;

function SetResetFilterOption() {
    var PreviousSelectedData = [];
    if (hdnAccount != undefined && hdnAccount.length > 0) {
        PreviousSelectedData = hdnAccount.split(",");
        $('#AccountIDs').val(PreviousSelectedData).trigger('change');
    }
    if (hdnProperty != undefined && hdnProperty.length > 0) {
        PreviousSelectedData = hdnProperty.split(",");
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
    if (filterCriteria == 'property') {
        $("#PropertyIDs").val(null).trigger('change');
        $("#spnSelectedProperty").addClass('hide');
        hdnProperty = "";
    }
    InitializeData();
}

function validateForm() {
    var isvalid = true;

    if (isvalid) {
        hdnAccount = getAccountIDs();
        hdnProperty = getFincaIDs();
        $("#FilterAccountID").val(hdnAccount);
        $("#FilterFincaIDs").val(hdnProperty);
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

    var selectedFincaIDs = "";
    selectedFincaIDs = getFincaIDs();
    if (selectedFincaIDs != '') {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnSelectedProperty").removeClass('hide');
        $("#FincaIdSearchText").text(SelectedProperty + " : " + selectedFincaIDs);
    }
    else {
        $("#spnSelectedProperty").addClass('hide');
        $("#FincaIdSearchText").text('');
    }

    if (isvalid) {
        hdnAccount = getAccountIDs();
        hdnProperty = getFincaIDs();
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

function getFincaIDs() {
    var selectPropertyList = [];
    if ($("#PropertyIDs").select2('data').length) {
        $.each($("#PropertyIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectPropertyList += "," + item.text;
        });
    }
    var selectedPropertyIDs = "";
    if (selectPropertyList.length > 0)
        selectedPropertyIDs = selectPropertyList.substring(1, selectPropertyList.length);
    return selectedPropertyIDs;
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
    $('#tblMigrationValidationBienes').DataTable({
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
        url: ROOTPath + "/Reports/Report/MigrationValidationBienes",
        data: {
            'typeID': $("#ddlType").val(),
            'FincaIDs': getFincaIDs(),
            'accountIDs': getAccountIDs(),
            'pageIndex': 1,
            'pageSize': pageSize,
            'isLoadMore': false
        },
        success: function (response) {
            if (response.status) {
                hideLoading();
                $("#divMigrationValidationBienes").html("").html(response.data);
                InitializeDataTable();
                rowCount = pageSize;
            }
            else {
                showAlert('error', response.data);
            }
        }
    });
}

function loadMoreMigrationValidationBienes(element) {
    var currentQualitativePage = $(element).data('currentpage') ? $(element).data('currentpage') : 1;
    var newPage = currentQualitativePage + 1;

    $.ajax({
        type: "POST",
        url: ROOTPath + "/Reports/Report/MigrationValidationBienes",
        data: {
            'typeID': $("#ddlType").val(),
            'FincaIDs': getFincaIDs(),
            'accountIDs': getAccountIDs(),
            'pageIndex': newPage,
            'pageSize': pageSize,
            'isLoadMore': true
        },
        success: function (response) {
            $(response.data).insertBefore("#tbodyMigrationValidationBienes");
            $(element).data('currentpage', newPage);
            currentQualitativePage = newPage;
            rowCount = rowCount + response.RowCount;
            if (rowCount < $("#TotalRecord").val()) {
                $("#tbodyMigrationValidationBienes").show();
            }
            else {
                $("#tbodyMigrationValidationBienes").hide();
            }
        }
    });
}