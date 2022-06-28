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
        language: __culture,
    });

    $('.periodDate').datepicker('update', new Date());
    $(".select2dropdown").select2({ width: '100%' });
    $("#CashierIDs").on('change', function () {
        setDropdownAll(this.id);
    });
    InitializeDataTable();
    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });
});
var hdnCollector;
var hdnBalanceFrom;
var hdnBalanceTo;
function SetResetFilterOption() {
    var PreviousSelectedData = [];
    if (hdnCollector != undefined && hdnCollector.length > 0) {
        PreviousSelectedData = hdnCollector.split(",");       
        $('#CashierIDs').val(PreviousSelectedData).trigger('change');
    }
    if (hdnBalanceFrom != undefined && hdnBalanceTo != "0" && hdnBalanceFrom.length > 0) {
        $("#txtNetClosingFrom").val(hdnBalanceFrom)
        $("#txtNetClosingTo").val(hdnBalanceTo)
    }
}
$(document).on('click', '#btnGo', function () {
    if (validateDates()) {
        InitializeDataTable();
        return true;
    }
    return false;
});
function getCashierIDs() {
    var selectCashierList = [];
    if ($("#CashierIDs").select2('data').length) {
        $.each($("#CashierIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectCashierList += "," + item.id;
        });
    }
    var selectedCashierIDs = "";
    if (selectCashierList.length > 0)
        selectedCashierIDs = selectCashierList.substring(1, selectCashierList.length);
    return selectedCashierIDs;
}
function getCashierText() {
    var selectCashierList = [];
    if ($("#CashierIDs").select2('data').length) {
        $.each($("#CashierIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectCashierList += "," + item.text;
        });
    }
    var selectedCashierTexts = "";
    if (selectCashierList.length > 0)
        selectedCashierTexts = selectCashierList.substring(1, selectCashierList.length);
    return selectedCashierTexts;
}
function setDropdownAll(elementID) {
    if ($("#" + elementID).val() && $("#" + elementID).val()[0] == "") {
        //$("#" + elementID).select2("val", "-1")
        $("#" + elementID).val("-1").trigger('change');
        $("#" + elementID + " option:gt(0)").prop('selected', true);
        $("#" + elementID).trigger('change');
    }

}
function validateDates() {
    var isvalid = false;
    if ($("#StartPeriodDate").val() == '')
        showAlert('error', $("#StartPeriodDate").attr("data-required-msg"));
    else if ($("#EndPeriodDate").val() == '')
        showAlert('error', $("#EndPeriodDate").attr("data-required-msg"));
    else if (new Date($("#EndPeriodDate").val()) < new Date($("#StartPeriodDate").val()))
        showAlert('error', $("#EndPeriodDate").attr("data-compare-val-msg"));
    else
        isvalid = true;
    if (isvalid) {
        hdnCollector = getCashierIDs();
        hdnBalanceFrom = $("#txtNetClosingFrom").val();
        hdnBalanceTo = $("#txtNetClosingTo").val();
        $("#FilterCashierID").val(hdnCollector);
        $("#NetClosingFrom").val(hdnBalanceFrom);
        $("#NetClosingTo").val(hdnBalanceTo);
    }
    return isvalid;
}
var closingTotal = 0;
function InitializeDataTable() {
    var table = $('#tblClosingSummeryList').dataTable({
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
            "url": ROOTPath + "/Reports/Report/CollectionClosingSummary",
            "type": "POST",
            "data": function (data) {
                data.startPeriod = new Date($("#StartPeriodDate").datepicker('getDate')).toDateString();
                data.endPeriod = new Date($("#EndPeriodDate").datepicker('getDate')).toDateString();
                data.cashierIds = $("#CashierIDs").val();
                data.NetClosingFrom = $("#txtNetClosingFrom").val();
                data.NetClosingTo = $("#txtNetClosingTo").val();
            }
            , "dataSrc": function (json) {
                closingTotal = json.closingTotal;
                return json.data;
            }
        },
        destroy: true,
        "columns": [
            {
                name: "Number", title: number, "data": "Number", className: "col-lg-2"
            },
            {
                name: "Date", title: date, "data": "FormattedDate", className: "col-lg-2 text-left"
            },
            {
                name: "ClosingTypeName", title: closingType, "data": "ClosingTypeName", className: "col-lg-1 text-left"
            },
            {
                name: "CashierName", title: cashier, "data": "CashierName", className: "col-lg-1 text-left"
            },
            {
                name: "Description", title: description, "data": "Description", className: "col-lg-2 text-left table-description-field"
            },
            {
                name: "PaymentReceiptCount", title: receiptcount, "data": "PaymentReceiptCount", className: "col-lg-2 text-right"
            },
            {
                name: "NetClosing", title: netclosing, "data": "FormattedNetClosing", className: "col-lg-2 text-right"
            }
        ],
        "lengthMenu": [[15, 25, 50, 100], [15, 25, 50, 100]],
        "order": [[1, "asc"]],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            var row = $("#tblClosingSummeryList").DataTable().row(iDisplayIndex);
            var data = row.data();
            createChild(iDisplayIndex, data.PostedPaymentReceiptList);
        },
        "footerCallback": function (row, data, start, end, display) {
            $(row).find("th").eq("1").text(closingTotal);
            $("#tblFooter").removeClass("hide");
        }
    });

}
function createChild(idx, d) {
    $.post(ROOTPath + "/Reports/Report/GetPaymentReceiptHtml", { "paymentReceiptListJSON": JSON.stringify(d) }, function (response) {
        if (response.status == false) {
            showAlert("error", response.message);
        }
        else {
            $("#tblClosingSummeryList").DataTable().row(idx).child(response).show();
        }
    });
}
function loadAdvanceSearch() {
    $("#txtNetClosingFrom").val("");
    $("#txtNetClosingTo").val("");   
    $('#CashierIDs').val([]).trigger('change');

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
    var isvalid = false;
    if ($("#txtNetClosingTo").val() != "" && $("#txtNetClosingFrom").val() == "")
        showAlert('error', balanceFromRequiredMsg);
    else if ($("#txtNetClosingFrom").val() != "" && $("#txtNetClosingTo").val() == "")
        showAlert('error', balanceToRequiredMsg);
    else if (
        ($("#txtNetClosingTo").val() != "" && $("#txtNetClosingFrom").val() != "")
        &&
        (parseInt($("#txtNetClosingFrom").val()) > parseInt($("#txtNetClosingTo").val()))
    )
        showAlert('error', compareBalanceValidationMsg);
    else {
        $("#NetClosingFrom").val($("#txtNetClosingFrom").val());
        $("#NetClosingTo").val($("#txtNetClosingTo").val());
        hdnCollector = getCashierIDs();
        hdnBalanceFrom = $("#txtNetClosingFrom").val();
        hdnBalanceTo = $("#txtNetClosingTo").val();
        refreshData();
        return true;
    }
    return false;
}
function refreshData() {
    var selectedCollectorTexts = "";
    InitializeDataTable();
    $("#advanceSearchModal").modal('hide');
    if ($("#txtNetClosingFrom").val() != '' && $("#txtNetClosingTo").val() != '' && parseFloat($("#txtNetClosingFrom").val()) >= 0 && parseFloat($("#txtNetClosingTo").val()) > 0) {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnBalanceRange").removeClass('hide');
        $("#SearchText").text(BalanceRange + " : " + $("#txtNetClosingFrom").val() + " - " + $("#txtNetClosingTo").val());
    }
    else {
        $("#spnBalanceRange").addClass('hide');
        $("#SearchText").text('');
    }
    selectedCollectorTexts = getCashierText();
    if (selectedCollectorTexts != '') {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnSelectedCashier").removeClass('hide');
        $("#CashierIdSearchText").text(selectedCashier + " : " + selectedCollectorTexts);
    }
    else {
        $("#spnSelectedCashier").addClass('hide');
        $("#CashierIdSearchText").text('');
    }
}
function clearSearch(filterCriteria) {
    if (validateDates()) {
        if (filterCriteria == 'balance') {
            $("#txtNetClosingFrom").val('');
            $("#txtNetClosingTo").val('');
            $("#NetClosingFrom").val('');
            $("#NetClosingTo").val('');
            $("#spnBalanceRange").addClass('hide');
            hdnBalanceFrom = "";
            hdnBalanceTo = "";
        }
        if (filterCriteria == 'cashier') {            
            $('#CashierIDs').val([]).trigger('change');
            $("#spnSelectedCashier").addClass('hide');
            hdnCollector = "";
        }
        InitializeDataTable();
    }
}
function Print() {
    if (validateDates()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintClosingSummary",
            data: { 'startPeriod': $("#StartPeriodDate").val(), 'endPeriod': $("#EndPeriodDate").val(), 'cashierIds': $("#CashierIDs").val() == '' ? null : $("#CashierIDs").val(), 'NetClosingFrom': $("#txtNetClosingFrom").val() == '' ? null : $("#txtNetClosingFrom").val(), 'NetClosingTo': $("#txtNetClosingTo").val() == '' ? null : $("#txtNetClosingTo").val() },
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