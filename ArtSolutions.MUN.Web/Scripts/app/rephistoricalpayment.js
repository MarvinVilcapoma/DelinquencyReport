var pageSize = 100;
var rowCount = pageSize;

$(window).resize(function () {
    $('#tblHistoricalPayment').DataTable().columns.adjust().responsive.recalc();
});

$(document).ready(function () {
    initializeDatePicker();
});

$(document).on('click', '#btnGo', function () {
    if (validateForm()) {
        InitializeData();
    }
    return false;
});

function initializeDatePicker() {
    $('.periodDate').datepicker({
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: "MM yyyy",
        language: __culture
    });
    var dt = new Date();
    $('#FromDate').datepicker("setDate", new Date(dt.getFullYear(), dt.getMonth(), dt.getDate() - 1));
    $('#ToDate').datepicker('setDate', new Date(dt.getFullYear(), dt.getMonth(), dt.getDate()));
}

function initDataTable() {
    $('#tblHistoricalPayment').DataTable({
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
        },
        "fnDrawCallback": function () {
            //$('#tblConsumptionRange tbody tr td').each(function () {
            //    if ($(this).find('#hdMeter').val() != undefined && $(this).index() == 1) {
            //        this.setAttribute('title', $(this).find('#hdMeter').val());
            //    }
            //    if ($(this).find('#hdAccountName').val() != undefined && $(this).index() == 4) {
            //        this.setAttribute('title', $(this).find('#hdAccountName').val());
            //    }
            //});

            /* Apply the tooltips */
            $('#tblHistoricalPayment tbody tr td').tooltip({
                container: "body",
                html: true
            });
        }
    });
}

function InitializeData() {
    var fromYear = $("#FromDate").datepicker('getDate').getFullYear();
    var fromMonth = $("#FromDate").datepicker('getDate').getMonth();
    var fromDate = new Date(fromYear, fromMonth, 1);

    var toYear = $("#ToDate").datepicker('getDate').getFullYear();
    var toMonth = $("#ToDate").datepicker('getDate').getMonth();
    var toDate = new Date(toYear, toMonth + 1, 0);


    $.ajax({
        type: "POST",
        url: ROOTPath + "/Reports/Report/HistoricalPaymentReport",
        data: {
            'fromDate': fromDate.toDateString(),
            'toDate': toDate.toDateString(),
            'pageIndex': 1,
            'pageSize': pageSize,
            'isLoadMore': false
        },
        success: function (response) {
            if (response.status) {
                hideLoading();
                $("#divHistoricalPaymentReport").html("").html(response.data);
                initDataTable();
                rowCount = pageSize;
            }
            else {
                showAlert('error', response.data);
            }
        }
    });
}

function validateForm() {

    var isvalid = true;
    $("#FromDate,#ToDate").removeClass("error");
    if ($("#FromDate").val() == undefined || $("#FromDate").val() == '') {
        $("#FromDate").addClass("error");
        isvalid = false;
    }
    if ($("#ToDate").val() == undefined || $("#ToDate").val() == '') {
        $("#ToDate").addClass("error");
        isvalid = false;
    }

    var fromYear = $("#FromDate").datepicker('getDate').getFullYear();
    var fromMonth = $("#FromDate").datepicker('getDate').getMonth();
    var fromDate = new Date(fromYear, fromMonth, 1);

    var toYear = $("#ToDate").datepicker('getDate').getFullYear();
    var toMonth = $("#ToDate").datepicker('getDate').getMonth();
    var toDate = new Date(toYear, toMonth, 1);

    if (toDate < fromDate) {
        showAlert('error', $("#ToDate").attr("data-compare-val-msg"));
        isvalid = false;
    }

    if (isvalid) {
        $($("#FormattedFromDate")).val(fromDate.toDateString());
        $($("#FormattedToDate")).val(new Date(toYear, toMonth + 1, 0).toDateString());
    }

    return isvalid;
}

function loadMoreHistoricalPayment(element) {
    var fromYear = $("#FromDate").datepicker('getDate').getFullYear();
    var fromMonth = $("#FromDate").datepicker('getDate').getMonth();
    var fromDate = new Date(fromYear, fromMonth, 1);

    var toYear = $("#ToDate").datepicker('getDate').getFullYear();
    var toMonth = $("#ToDate").datepicker('getDate').getMonth();
    var toDate = new Date(toYear, toMonth + 1, 0);

    var currentQualitativePage = $(element).data('currentpage') ? $(element).data('currentpage') : 1;
    var newPage = currentQualitativePage + 1;
    $.ajax({
        type: "POST",
        url: ROOTPath + "/Report/HistoricalPaymentReport",
        data: {
            'fromDate': fromDate.toDateString(),
            'toDate': toDate.toDateString(),
            'pageIndex': newPage,
            'pageSize': pageSize,
            'isLoadMore': true
        },
        success: function (response) {
            $(response.data).insertBefore("#tbodyHistoricalPayment");
            $(element).data('currentpage', newPage);
            currentQualitativePage = newPage;
            rowCount = rowCount + response.RowCount;
            if (rowCount < $("#TotalRecord").val()) {
                $("#tbodyHistoricalPayment").show();
            }
            else {
                $("#tbodyHistoricalPayment").hide();
            }
        }
    });
}