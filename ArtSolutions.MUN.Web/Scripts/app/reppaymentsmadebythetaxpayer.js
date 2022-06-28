$(document).ready(function () {  
    $('#dvUpdate').hide();
    GetAccountForSelect('AccountIDs', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px', true);
    initializeDatePicker();
    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });
});
var hdnAccount;
function SetResetFilterOption() {
    var PreviousSelectedData = [];
    if (hdnAccount != undefined && hdnAccount.length > 0) {
        PreviousSelectedData = hdnAccount.split(",");
        $('#AccountIDs').val(PreviousSelectedData).trigger('change');
    }
}

function initializeDatePicker() {
    $('.periodDate').datepicker({
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture
    });
    var dt = new Date();
    $('#StartDate').datepicker("setDate", new Date(dt.getFullYear(), dt.getMonth(), dt.getDate() - 1));
    $('#EndDate').datepicker('setDate', new Date(dt.getFullYear(), dt.getMonth(), dt.getDate()));
}

$(document).on('click', '#btnGo', function () {
    if (validateForm()) {
        InitializeData();
    }
    return false;
});

$(window).resize(function () {
    if (validateForm()) {
        //InitializeData();
        $('#paymentsMadeByTheTaxpayerTable').DataTable().columns.adjust().responsive.recalc();
    }
    return false;
});

$(document).on('change', '#AccountIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#AccountIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

function clearSearch(filterCriteria) {
    if (filterCriteria == 'accounts') {
        $("#AccountIDs").val(null).trigger('change');
        $("#spnSelectedAccount").addClass('hide');
        hdnAccount = "";
    }

    InitializeData();
}

function validateForm() {
    var isvalid = false;
    if ($("#StartDate").val() == undefined || $("#StartDate").val() == '')
        showAlert('error', $("#StartDate").attr("data-required-msg"));
    else if ($("#EndDate").val() == undefined || $("#EndDate").val() == '')
        showAlert('error', $("#EndDate").attr("data-required-msg"));
    else if (new Date($("#EndDate").datepicker('getDate')) < new Date($("#StartDate").datepicker('getDate')))
        showAlert('error', $("#EndDate").attr("data-compare-val-msg"));
    else
        isvalid = true;
    if (isvalid) {
        hdnAccount = getAccountIDs();
        $("#FilterAccountID").val(hdnAccount);
    }
    return isvalid;
}

function validateAdvanceSearch() {
    var isvalid = false;
    var selectedAccountTexts = "";
    isvalid = true;

    InitializeData();

    $("#advanceSearchModal").modal('hide');

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
        hdnAccount = getAccountIDs();
    }
    return isvalid;
}

function loadAdvanceSearch() {
    $("#AccountIDs").val(null).trigger('change');
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

var paymentsMadeByTheTaxpayerDataTable = null;

function initDataTable() {
    if ($("#paymentsMadeByTheTaxpayerTable tbody tr.no-data").length == 1) {
        return false;
    }
    accountStatementDataTable = $('#paymentsMadeByTheTaxpayerTable').DataTable({
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
    $.ajax({
        type: "POST",
        url: ROOTPath + "/Reports/Report/PaymentsMadeByTheTaxpayer",
        data: {
            'startDate': new Date($("#StartDate").datepicker('getDate')).toDateString()
            , 'endDate': new Date($("#EndDate").datepicker('getDate')).toDateString()
            , 'accountIDs': getAccountIDs()
        },
        success: function (response) {
            if (response.status) {
                hideLoading();
                $("#divPaymentsMadeByTheTaxpayer").html("").html(response.data);
                initDataTable();
                $('#paymentsMadeByTheTaxpayerTable tbody tr td').tooltip({
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

function Print() {
    if (validateForm()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintPaymentsMadeByTheTaxpayer",
            data: {
                'startDate': new Date($("#StartDate").datepicker('getDate')).toDateString()
                , 'endDate': new Date($("#EndDate").datepicker('getDate')).toDateString()
                , 'accountIDs': getAccountIDs()
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