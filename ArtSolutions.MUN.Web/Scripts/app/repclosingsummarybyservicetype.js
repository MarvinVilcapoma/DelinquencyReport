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

var hdnServiceType;
var hdnBalanceFrom;
var hdnBalanceTo;

function SetResetFilterOption() {
    var PreviousSelectedData = [];
    if (hdnServiceType !== undefined && hdnServiceType.length > 0) {
        PreviousSelectedData = hdnServiceType.split(",");        
        $('#ServiceTypeIDs').val(PreviousSelectedData).trigger('change');
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

$(document).on('change', '#ServiceTypeIDs', function () {
    if ($(this).val() && $(this).val()[0] === 0) {
        $(this).val("").click();
        $("#ServiceTypeIDs option:gt(0)").prop('selected', true);
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
        hdnServiceType = getServiceTypeIDs();
        hdnBalanceFrom = $("#txtNetClosingFrom").val();
        hdnBalanceTo = $("#txtNetClosingTo").val();
        $("#FilterServiceTypeID").val(hdnServiceType);
    }
    return isvalid;
}

function getServiceTypeIDs() {
    var selectServiceTypeList = [];
    if ($("#ServiceTypeIDs").select2('data').length) {
        $.each($("#ServiceTypeIDs").select2('data'), function (key,item) {
            if (item.id !== 0)
                selectServiceTypeList += "," + item.id;
        });
    }
    var selectServiceTypeId = "";
    if (selectServiceTypeList.length > 0)
        selectServiceTypeId = selectServiceTypeList.substring(1, selectServiceTypeList.length);
    return selectServiceTypeId;
}

function getServiceTypeText() {
    var selectServiceTypeList = [];
    if ($("#ServiceTypeIDs").select2('data').length) {
        $.each($("#ServiceTypeIDs").select2('data'), function (key,item) {
            if (item.id !== 0)
                selectServiceTypeList += "," + item.text;
        });
    }
    var selectedServiceTypeTexts = "";
    if (selectServiceTypeList.length > 0)
        selectedServiceTypeTexts = selectServiceTypeList.substring(1, selectServiceTypeList.length);
    return selectedServiceTypeTexts;
}

var closingTotal = 0;
function InitializeDataTable() {
    $('#tblClosingSummeryServiceTypeList').dataTable({
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
            "url": ROOTPath + "/Reports/Report/ClosingSummaryByServiceType",
            "type": "POST",
            "data": function (data) {
                data.startPeriod = new Date($("#StartPeriodDate").datepicker('getDate')).toDateString();
                data.endPeriod = new Date($("#EndPeriodDate").datepicker('getDate')).toDateString();
                data.serviceTypeIds = getServiceTypeIDs();
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
            var row = $("#tblClosingSummeryServiceTypeList").DataTable().row(iDisplayIndex);
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
    $.post(ROOTPath + "/Reports/Report/GetPaymentReceiptByServiceTypeHtml", { "closingListJSON": JSON.stringify(d) }, function (response) {
        if (response.status === false) {
            showAlert("error", response.message);
        }
        else {
            $("#tblClosingSummeryServiceTypeList").DataTable().row(idx).child(response).show();
        }
    });
}

function loadAdvanceSearch() {
    $("#txtNetClosingFrom").val("");
    $("#txtNetClosingTo").val("");    
    $('#ServiceTypeIDs').val([]).trigger('change');
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
        hdnServiceType = getServiceTypeIDs();
        hdnBalanceFrom = $("#txtNetClosingFrom").val();
        hdnBalanceTo = $("#txtNetClosingTo").val();
        refreshData();
        $("#FilterServiceTypeID").val(getServiceTypeIDs());
        return true;
    }
    return false;
}

function refreshData() {
    var selectedServiceTypeTexts = "";
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
    selectedServiceTypeTexts = getServiceTypeText();
    if (selectedServiceTypeTexts !== '') {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnSelectedServiceType").removeClass('hide');
        $("#ServiceTypeIdSearchText").text(selectedServiceType + " : " + selectedServiceTypeTexts);
    }
    else {
        $("#spnSelectedServiceType").addClass('hide');
        $("#ServiceTypeIdSearchText").text('');
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
        if (filterCriteria === 'servicetype') {            
            $('#ServiceTypeIDs').val([]).trigger('change');
            $("#spnSelectedServiceType").addClass('hide');
            hdnServiceType = "";
        }
        InitializeDataTable();
    }
}

function Print() {
    if (validateDates()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintClosingSummaryServiceType",
            data: {
                'startPeriod': $("#StartPeriodDate").val(),
                'endPeriod': $("#EndPeriodDate").val(),
                'serviceTypeIds': getServiceTypeIDs(),
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