$(window).on("load", function () {
    $('#btnGo').focus();
});

$(document).ready(function () {
    

    $('.periodDate').datepicker({
        defaultDate: new Date(),
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture
    });

    $('.periodDate').datepicker('update', new Date());
    $(".select2dropdown").select2({ width: '100%' });

    InitializeDataTable();
    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });
});

var hdnPaymentPlan;
var hdnBalanceFrom;
var hdnBalanceTo;

function SetResetFilterOption() {
    var PreviousSelectedData = [];
    if (hdnPaymentPlan !== undefined && hdnPaymentPlan.length > 0) {
        PreviousSelectedData = hdnPaymentPlan.split(",");        
        $('#PaymentPlanIDs').val(PreviousSelectedData).trigger('change');
    }
    if (hdnBalanceFrom !== undefined && hdnBalanceTo !== "0" && hdnBalanceFrom.length > 0) {
        $("#txtNetClosingFrom").val(hdnBalanceFrom);
        $("#txtNetClosingTo").val(hdnBalanceTo);
    }
}

$(document).on('click', '#btnGo', function () {
    if (validateDates()) {
        InitializeDataTable();
        return true;
    }
    return false;
});

$(document).on('change', '#PaymentPlanIDs', function () {
    if ($(this).val() && $(this).val()[0] === 0) {
        $(this).val("").click();
        $("#PaymentPlanIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

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
        hdnPaymentPlan = getPaymentPlanIDs();
        hdnBalanceFrom = $("#txtNetClosingFrom").val();
        hdnBalanceTo = $("#txtNetClosingTo").val();
        $("#FilterPaymentPlanID").val(hdnPaymentPlan);
    }
    return isvalid;
}

function getPaymentPlanIDs() {
    var selectPaymentPlanList = [];
    if ($("#PaymentPlanIDs").select2('data').length) {
        $.each($("#PaymentPlanIDs").select2('data'), function (key,item) {
            if (item.id !== 0)
                selectPaymentPlanList += "," + item.id;
        });
    }
    var selectPaymentPlanId = "";
    if (selectPaymentPlanList.length > 0)
        selectPaymentPlanId = selectPaymentPlanList.substring(1, selectPaymentPlanList.length);
    return selectPaymentPlanId;
}

function getPaymentPlanText() {
    var selectPaymentPlanList = [];
    if ($("#PaymentPlanIDs").select2('data').length) {
        $.each($("#PaymentPlanIDs").select2('data'), function (key,item) {
            if (item.id !== 0)
                selectPaymentPlanList += "," + item.text;
        });
    }
    var selectedPaymentPlanTexts = "";
    if (selectPaymentPlanList.length > 0)
        selectedPaymentPlanTexts = selectPaymentPlanList.substring(1, selectPaymentPlanList.length);
    return selectedPaymentPlanTexts;
}

var closingTotal = 0;
function InitializeDataTable() {
    $('#tblClosingSummeryPaymentPlanList').dataTable({
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
            "url": ROOTPath + "/Reports/Report/ClosingSummaryByPaymentPlan",
            "type": "POST",
            "data": function (data) {
                data.startPeriod = new Date($("#StartPeriodDate").datepicker('getDate')).toDateString();
                data.endPeriod = new Date($("#EndPeriodDate").datepicker('getDate')).toDateString();
                data.commaSeperatedPaymentPlanIDs = getPaymentPlanIDs();
                data.netClosingFrom = $("#txtNetClosingFrom").val();
                data.netClosingTo = $("#txtNetClosingTo").val();
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
            var row = $("#tblClosingSummeryPaymentPlanList").DataTable().row(iDisplayIndex);
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
    $.post(ROOTPath + "/Reports/Report/GetPaymentReceiptByPaymentPlanHtml", { "closingListJSON": JSON.stringify(d) }, function (response) {
        if (response.status === false) {
            showAlert("error", response.message);
        }
        else {
            $("#tblClosingSummeryPaymentPlanList").DataTable().row(idx).child(response).show();
        }
    });
}

function loadAdvanceSearch() {
    $("#txtNetClosingFrom").val("");
    $("#txtNetClosingTo").val("");    
    $('#PaymentPlanIDs').val([]).trigger('change');

    SetResetFilterOption();
    if (!validateDates()) {
        return false;
    }
    else {
        $("#advanceSearchModal").modal('show');
        $("#txtNetClosingFrom").focus();
    }
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
        showAlert('error', compareBalanceValidationMsg);
    else {
        $("#NetClosingFrom").val($("#txtNetClosingFrom").val());
        $("#NetClosingTo").val($("#txtNetClosingTo").val());
        hdnPaymentPlan = getPaymentPlanIDs();
        hdnBalanceFrom = $("#txtNetClosingFrom").val();
        hdnBalanceTo = $("#txtNetClosingTo").val();
        refreshData();
        $("#FilterPaymentPlanID").val(getPaymentPlanIDs());
        return true;
    }
    return false;
}

function refreshData() {
    var selectedPaymentPlanTexts = "";
    InitializeDataTable();
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
    selectedPaymentPlanTexts = getPaymentPlanText();
    if (selectedPaymentPlanTexts !== '') {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnSelectedPaymentPlan").removeClass('hide');
        $("#PaymentPlanIdSearchText").text(selectedPaymentPlan + " : " + selectedPaymentPlanTexts);
    }
    else {
        $("#spnSelectedPaymentPlan").addClass('hide');
        $("#PaymentPlanIdSearchText").text('');
    }
}

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
        if (filterCriteria === 'paymentPlan') {            
            $('#PaymentPlanIDs').val([]).trigger('change');
            $("#spnSelectedPaymentPlan").addClass('hide');
            hdnPaymentPlan = "";
        }
        InitializeDataTable();
    }
}

function Print() {
    if (validateDates()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintClosingSummaryByPaymentPlan",
            data: {
                'startPeriod': $("#StartPeriodDate").val(),
                'endPeriod': $("#EndPeriodDate").val(),
                'commaSeperatedPaymentPlanIDs': getPaymentPlanIDs(),
                'netClosingFrom': $("#txtNetClosingFrom").val() || null,
                'netClosingTo': $("#txtNetClosingTo").val() || null
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