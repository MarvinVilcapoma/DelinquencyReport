$(document).ready(function () {
    
    initializeDatePicker();
    InitializeDataTable();
    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });
});

var hdnBalanceFrom;
var hdnBalanceTo;
var hdnOtherService;
var hdnGrant;

function SetResetFilterOption() {
    if (hdnBalanceFrom !== undefined && hdnBalanceFrom.length > 0) {
        $("#txtNetClosingFrom").val(hdnBalanceFrom);
        $("#txtNetClosingTo").val(hdnBalanceTo);
    }
    var PreviousSelectedData = [];
    if (hdnOtherService != undefined && hdnOtherService.length > 0) {
        PreviousSelectedData = hdnOtherService.split(",");
        $('#OtherServiceIDs').val(PreviousSelectedData).trigger('change');
    }
    var PreviousSelectedData = [];
    if (hdnGrant != undefined && hdnGrant.length > 0) {
        PreviousSelectedData = hdnGrant.split(",");
        $('#GrantIDs').val(PreviousSelectedData).trigger('change');
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
    $('.periodDate').datepicker('update', new Date());
    $(".select2dropdown").select2({ width: '100%' });
}

$(window).on("load", function () {
    $('#btnGo').focus();
});

$(document).on('click', '#btnGo', function () {
    if (validateDates()) {
        InitializeDataTable();
        return true;
    }
    return false;
});

$(document).on('change', '#OtherServiceIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#OtherServiceIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});
$(document).on('change', '#GrantIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#GrantIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

function clearSearch(filterCriteria) {
    if (validateDates()) {
        if (filterCriteria === 'balance') {
            $("#txtNetClosingFrom").val('');
            $("#txtNetClosingTo").val('');
            $("#NetClosingFrom").val('');
            $("#NetClosingTo").val('');
            $("#spnBalanceRange").addClass('hide');
            hdnBalanceFrom = "";
            hdnBalanceTo = "";
        }
        if (filterCriteria == 'otherservice') {            
            $('#OtherServiceIDs').val([]).trigger('change');
            $("#spnSelectedOtherService").addClass('hide');
            hdnOtherService = "";
        }
        if (filterCriteria == 'grant') {            
            $('#GrantIDs').val([]).trigger('change');
            $("#spnSelectedGrant").addClass('hide');
            hdnGrant = "";
        }
        InitializeDataTable();
    }
}

function validateDates() {
    var isvalid = true;
    $("#StartPeriodDate,#EndPeriodDate").removeClass("error");
    if ($("#StartPeriodDate").val() === '') {
        showAlert('error', $("#StartPeriodDate").attr("data-required-msg"));
        $("#StartPeriodDate").addClass("error");
        isvalid = false;
    }
    if ($("#EndPeriodDate").val() === '') {
        showAlert('error', $("#EndPeriodDate").attr("data-required-msg"));
        $("#EndPeriodDate").addClass("error");
        isvalid = false;
    }
    if (new Date($("#EndPeriodDate").datepicker('getDate')) < new Date($("#StartPeriodDate").datepicker('getDate'))) {
        showAlert('error', $("#EndPeriodDate").attr("data-compare-val-msg"));
        isvalid = false;
    }

    if (isvalid) {
        hdnBalanceFrom = $("#txtNetClosingFrom").val();
        hdnBalanceTo = $("#txtNetClosingTo").val();
        hdnOtherService = getOtherServiceIDs();
        hdnGrant = getGrantIDs();
        $("#FilterOtherServiceID").val(hdnOtherService);
        $("#FilterGrantID").val(hdnGrant);
        $("#txtNetClosingFrom").val(hdnBalanceFrom);
        $("#txtNetClosingTo").val(hdnBalanceTo);
    }
    return isvalid;
}

function validateForm() {
    if ($("#txtNetClosingTo").val() !== "" && $("#txtNetClosingFrom").val() === "")
        showAlert('error', balanceFromRequiredMsg);
    else if ($("#txtNetClosingFrom").val() !== "" && $("#txtNetClosingTo").val() === "")
        showAlert('error', balanceToRequiredMsg);
    else if (
        $("#txtNetClosingTo").val() !== "" && $("#txtNetClosingFrom").val() !== ""
        &&
        parseInt($("#txtNetClosingFrom").val()) > parseInt($("#txtNetClosingTo").val())
    )
        showAlert('error', compareDepositValidationMsg);
    else {
        $("#NetClosingFrom").val($("#txtNetClosingFrom").val());
        $("#NetClosingTo").val($("#txtNetClosingTo").val());
        hdnBalanceFrom = $("#txtNetClosingFrom").val();
        hdnBalanceTo = $("#txtNetClosingTo").val();
        hdnOtherService = getOtherServiceIDs();
        hdnGrant = getGrantIDs();
        refreshData();
        $("#FilterOtherServiceID").val(hdnOtherService);
        $("#FilterGrantID").val(hdnGrant);
        return true;
    }
    return false;
}

function refreshData() {
    InitializeDataTable();
    var selectedTexts = "";
    $("#advanceSearchModal").modal('hide');
    if ($("#txtNetClosingFrom").val() !== '' && $("#txtNetClosingTo").val() !== '' && parseFloat($("#txtNetClosingFrom").val()) >= 0 && parseFloat($("#txtNetClosingTo").val()) > 0) {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnBalanceRange").removeClass('hide');
        $("#SearchText").text(BalanceRange + " : " + $("#txtNetClosingFrom").val() + " - " + $("#txtNetClosingTo").val());
    }
    else {
        $("#spnBalanceRange").addClass('hide');
        $("#SearchText").text('');
    }
    selectedTexts = getOtherServiceText();
    if (selectedTexts != '') {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnSelectedOtherService").removeClass('hide');
        $("#OtherServiceIdSearchText").text(selectedPaymentType + " : " + selectedTexts);
    }
    else {
        $("#spnSelectedOtherService").addClass('hide');
        $("#OtherServiceIdSearchText").text('');
    }
    selectedTexts = getGrantText();
    if (selectedTexts != '') {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnSelectedGrant").removeClass('hide');
        $("#GrantIdSearchText").text(selectedPaymentType + " : " + selectedTexts);
    }
    else {
        $("#spnSelectedGrant").addClass('hide');
        $("#GrantIdSearchText").text('');
    }
}
function getOtherServiceIDs() {
    var selectList = [];
    if ($("#OtherServiceIDs").select2('data').length) {
        $.each($("#OtherServiceIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectList += "," + item.id;
        });
    }
    var selectId = "";
    if (selectList.length > 0)
        selectId = selectList.substring(1, selectList.length);
    return selectId;
}

function getOtherServiceText() {
    var selectList = [];
    if ($("#OtherServiceIDs").select2('data').length) {
        $.each($("#OtherServiceIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectList += "," + item.text;
        });
    }
    var selectedTexts = "";
    if (selectList.length > 0)
        selectedTexts = selectList.substring(1, selectList.length);
    return selectedTexts;
}

function getGrantIDs() {
    var selectList = [];
    if ($("#GrantIDs").select2('data').length) {
        $.each($("#GrantIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectList += "," + item.id;
        });
    }
    var selectId = "";
    if (selectList.length > 0)
        selectId = selectList.substring(1, selectList.length);
    return selectId;
}

function getGrantText() {
    var selectList = [];
    if ($("#GrantIDs").select2('data').length) {
        $.each($("#GrantIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectList += "," + item.text;
        });
    }
    var selectedTexts = "";
    if (selectList.length > 0)
        selectedTexts = selectList.substring(1, selectList.length);
    return selectedTexts;
}

function loadAdvanceSearch() {
    $("#txtNetClosingFrom").val("");
    $("#txtNetClosingTo").val("");        
    $('#OtherServiceIDs').val([]).trigger('change');
    $('#GrantIDs').val([]).trigger('change');
    SetResetFilterOption();
    if (!validateDates()) {
        return false;
    }
    else {
        $("#advanceSearchModal").modal('show');
        $("#txtNetClosingFrom").focus();
    }
}

var closingTotal = 0;
function InitializeDataTable() {
    $('#tblClosingSummeryByInvoiceList').dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg,
            oPaginate: {
                sPrevious: previous,
                sNext: next
            }
        },
        "responsive": true,
        "serverSide": true,
        "processing": true,
        "bFilter": false,
        "bInfo": false,
        "bLengthChange": false,
        "autoWidth": false,
        "pageLength": pageSize,
        "ordering": false,
        "paging": false,
        "scrollY": "100vh",
        "scrollX": true,
        "scrollCollapse": true,
        "ajax": {
            "url": ROOTPath + "/Reports/Report/ClosingSummaryByInvoice",
            "type": "POST",
            "data": function (data) {
                data.startPeriod = new Date($("#StartPeriodDate").datepicker('getDate')).toDateString();
                data.endPeriod = new Date($("#EndPeriodDate").datepicker('getDate')).toDateString();
                data.netClosingFrom = $("#txtNetClosingFrom").val();
                data.netClosingTo = $("#txtNetClosingTo").val();
                data.OtherServiceIDs = getOtherServiceIDs();
                data.GrantIDs = getGrantIDs();
            }
            , "dataSrc": function (json) {
                closingTotal = json.closingTotal;
                return json.data;
            }
        },
        destroy: true,
        "columns": [
            {
                name: "Number", title: number, "data": "Number", className: "col-lg-1"
            },
            {
                name: "Date", title: date, "data": "FormattedDate", className: "col-lg-1 text-left"
            },
            {
                name: "ClosingTypeName", title: closingType, "data": "ClosingTypeName", className: "col-lg-2 text-left"
            },
            {
                name: "CashierName", title: cashier, "data": "CashierName", className: "col-lg-2 text-left"
            },
            {
                name: "Description", title: description, "data": "Description", className: "col-lg-3 text-left"
            },
            {
                name: "PaymentReceiptCount", title: receiptCount, "data": "PaymentReceiptCount", className: "col-lg-1 text-right"
            },
            {
                name: "NetClosing", title: netClosing, "data": "FormattedNetClosing", className: "col-lg-2 text-right"
            }
        ],
        "lengthMenu": [[15, 25, 50, 100], [15, 25, 50, 100]],
        "order": [[1, "asc"]],
        "fnRowCallback": function (iDisplayIndex) {
            var row = $("#tblClosingSummeryByInvoiceList").DataTable().row(iDisplayIndex);
            var data = row.data();
            createChild(iDisplayIndex, data.PostedPaymentReceiptList);
        },
        "footerCallback": function (row) {
            $(row).find("td").eq("1").text(closingTotal);
            $("#tblFooter").removeClass("hide");
        }
    });

}
function createChild(idx, d) {
    $.post(ROOTPath + "/Reports/Report/GetPaymentReceiptByInvoiceHtml", { "closingListJSON": JSON.stringify(d) }, function (response) {
        if (response.status === false) {
            showAlert("error", response.message);
        }
        else {
            $("#tblClosingSummeryByInvoiceList").DataTable().row(idx).child(response).show();
        }
    });
}

function Print() {
    if (validateDates()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintClosingSummaryByInvoice",
            data: {
                'startPeriod': $("#StartPeriodDate").val(),
                'endPeriod': $("#EndPeriodDate").val(),
                'netClosingFrom': $("#txtNetClosingFrom").val() === '' ? null : $("#txtNetClosingFrom").val(),
                'netClosingTo': $("#txtNetClosingTo").val() === '' ? null : $("#txtNetClosingTo").val(),
                'OtherServiceIDs': getOtherServiceIDs(),
                'GrantIDs': getGrantIDs()
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
                    return false;
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