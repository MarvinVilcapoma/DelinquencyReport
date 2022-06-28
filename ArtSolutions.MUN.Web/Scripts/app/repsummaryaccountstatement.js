$(document).ready(function () {
    $(".select2dropdown").select2();
    GetAccountForSelect('AccountId', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px');
    $("#ddlRight").select2();
});

var hdnYear;

function SetResetFilterOption() {
    var PreviousSelectedData = [];
    if (hdnYear != undefined && hdnYear.length > 0) {
        PreviousSelectedData = hdnYear.split(",");
        $('#CommaSeperatedYearIDs').val(PreviousSelectedData).trigger('change');
    }
}

$(document).on('click', '#btnGo', function () {
    if (checkAccountIdInput() && $("#form").valid() && validateForm()) {
        InitializeData();
    }
    return false;
});

$(window).resize(function () {
    if (checkAccountIdInput() && $("#form").valid() && validateForm()) {
        InitializeData();
    }
});

$(document).on('change', '#CommaSeperatedYearIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#CommaSeperatedYearIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

function clearSearch(filterCriteria) {
    if (filterCriteria == 'year') {
        $("#CommaSeperatedYearIDs").val(null).trigger('change');
        $("#spnYear").addClass('hide');
        hdnYear = "";
    }
    InitializeData();
}

function validateForm() {
    var isvalid = true;
    if (isvalid) {
        hdnYear = getYearIDs();
        $("#FilterYearID").val(hdnYear);
    }
    return isvalid;
}

function validateAdvanceSearch() {
    var selectedYearTexts = "";
    var isvalid = true;
    InitializeData();
    $("#advanceSearchModal").modal('hide');

    selectedYearTexts = getYearText();
    if (selectedYearTexts != '') {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnYear").removeClass('hide');
        $("#YearSearchText").text(selectedYear + " : " + selectedYearTexts);
    }
    else {
        $("#spnYear").addClass('hide');
        $("#YearSearchText").text('');
    }

    if (isvalid) {
        hdnYear = getYearIDs();
    }
    return isvalid;
}

function loadAdvanceSearch() {
    if (validateForm()) {
        $("#CommaSeperatedYearIDs").val(null).trigger('change');
        SetResetFilterOption();
        $("#advanceSearchModal").modal('show');
        $("#CommaSeperatedYearIDs").focus();
    }
}

function getYearIDs() {
    var selectYearList = [];
    if ($("#CommaSeperatedYearIDs").select2('data').length) {
        $.each($("#CommaSeperatedYearIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectYearList += "," + item.id;
        });
    }
    var selectedYearIDs = "";
    if (selectYearList.length > 0)
        selectedYearIDs = selectYearList.substring(1, selectYearList.length);
    return selectedYearIDs;
}

function getYearText() {
    var selectYearList = [];
    if ($("#CommaSeperatedYearIDs").select2('data').length) {
        $.each($("#CommaSeperatedYearIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectYearList += "," + item.text;
        });
    }
    var selectedYearTexts = "";
    if (selectYearList.length > 0)
        selectedYearTexts = selectYearList.substring(1, selectYearList.length);
    return selectedYearTexts;
}

function checkAccountIdInput() {
    var isvalid = true;
    if ($("#AccountId").val() == '') {
        $("#AccountId").focus();
    }
    return isvalid;
}

var summaryAccountStatementTable = null;

function initDataTable() {
    if ($("#summaryAccountStatementTable tbody tr.no-data").length == 1) {
        return false;
    }
    accountStatementDataTable = $('#summaryAccountStatementTable').DataTable({
        "paging": false,
        "ordering": false,
        "info": false,
        "searching": false,
        "lengthChange": false,
        responsive: false,
        autoWidth: false,
        fixedHeader: true,
        language: {
            "emptyTable": nodatamsg
        },
        "scrollY": "100vh",
        "scrollX": true,
        scrollCollapse: true
    });
}

function InitializeData() {
    if ($("#AccountId").val() > 0) {
        $.ajax({
            type: "POST",
            url: ROOTPath + "/Reports/Report/SummaryAccountStatement",
            data: {
                'accountId': $("#AccountId").val(),
                'accountPropertyID': $("#ddlRight").val(),
                'commaSeperatedYearIDs': getYearIDs()
            },
            success: function (response) {
                if (response.status) {
                    hideLoading();
                    $("#divSummaryAccountStatementReport").html("").html(response.data);
                    initDataTable();
                    $('#summaryAccountStatementTable tbody tr td').tooltip({
                        container: "body",
                        html: true
                    });
                }
                else {
                    showAlert('error', response.data);
                }
            }
        });
    }
}

function Print(data) {
    if (checkAccountIdInput() && $("#form").valid() && validateForm()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintSummaryAccountStatement",
            data:
            {
                'accountId': $("#AccountId").val(),
                'accountPropertyID': $("#ddlRight").val(),
                'commaSeperatedYearIDs': getYearIDs()
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
                    return false;
                }
            }
        });
    }
    else
        return false;
    return true;
}

function GetRight() {
    if ($("#AccountId").val() > 0) {
        $.ajax({
            url: ROOTPath + "/AccountService/GetAccountPropertyRightByOwner",
            data: {
                ownerID: $("#AccountId").val(),
                serviceID: 0,
                fiscalYearID: 0,
                ID: null,
                forAccountStatementReport: true
            },
            success: function (data) {
                $("#ddlRight").empty();
                $.each(data, function (index, optiondata) {
                    $("#ddlRight").append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
                });
                $("#ddlRight").select2({ width: '100%' });
                $("#ddlRight option:first").val("");
            }
        });
    }
    else {
        $("#ddlRight").empty();
        $("#ddlRight").append("<option value='" + '' + "'>" + defaultSelectText + "</option>");
        $("#divSummaryAccountStatementReport").html(null);
    }
}