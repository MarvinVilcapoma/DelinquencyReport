var pageSize = 100;
var rowCount = pageSize;
var lastReadingTotal = 0;
var currentReadingTotal = 0;
var TotalConsumption = 0;
var TotalAmount = 0;

$(document).ready(function () {
    initializeDatePicker();
    $("#btnGo").focus();
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
    $('#Period').datepicker("setDate", new Date(dt.getFullYear(), dt.getMonth(), dt.getDate() - 1));
}


function InitializeData() {
    $.ajax({
        type: "POST",
        url: ROOTPath + "/Reports/Report/InconsistenceReading",
        data: {
            'period': new Date($("#Period").datepicker('getDate')).toDateString(),
            'pageIndex': 1,
            'pageSize': pageSize,
            'isLoadMore': false
        },
        success: function (response) {
            if (response.status) {
                hideLoading();
                $("#divInconsistenceReading").html("").html(response.data);

                if ($("#TotalRecord").val() <= 100) {////19-June-2019 Test
                    $(".tdTotal").show();
                }
                else {
                    lastReadingTotal = parseFloat(lastReadingTotal) + parseFloat(response.lastReadingTotal);
                    currentReadingTotal = parseFloat(currentReadingTotal) + parseFloat(response.currentReadingTotal);
                    //TotalConsumption = parseFloat(TotalConsumption) + parseFloat(response.TotalConsumption);
                    //TotalAmount = parseFloat(TotalAmount) + parseFloat(response.TotalAmount);
                    TotalConsumption = parseFloat(response.TotalConsumption);
                    TotalAmount = parseFloat(response.TotalAmount);
                    $(".tdTotal").hide();
                }

                rowCount = pageSize;
                initTooltip();
                initDataTable();
            }
            else {
                showAlert('error', response.data);
            }
        }
    });
}

function loadMoreInconsistenceReading(element) {
    var currentQualitativePage = $(element).data('currentpage') ? $(element).data('currentpage') : 1;
    var newPage = currentQualitativePage + 1;

    $.ajax({
        type: "POST",
        url: ROOTPath + "/Report/InconsistenceReading",
        data: {
            'period': new Date($("#Period").datepicker('getDate')).toDateString(),
            'pageIndex': newPage,
            'pageSize': pageSize,
            'isLoadMore': true
        },
        success: function (response) {
            $(response.data).insertBefore("#tbodyInconsistenceReading");
            $(element).data('currentpage', newPage);

            currentQualitativePage = newPage;
            rowCount = rowCount + response.RowCount;
            if (rowCount < $("#TotalRecord").val()) {
                $("#tbodyInconsistenceReading").show();
            }
            else {
                $("#tbodyInconsistenceReading").hide();
            }

            lastReadingTotal = parseFloat(lastReadingTotal) + parseFloat(response.lastReadingTotal);
            currentReadingTotal = parseFloat(currentReadingTotal) + parseFloat(response.currentReadingTotal);
            //TotalConsumption = parseFloat(TotalConsumption) + parseFloat(response.TotalConsumption);
            //TotalAmount = parseFloat(TotalAmount) + parseFloat(response.TotalAmount);
            TotalConsumption = parseFloat(response.TotalConsumption);
            TotalAmount = parseFloat(response.TotalAmount);

            if (rowCount == $("#TotalRecord").val()) {//19-June-2019 Test
                $("#tdLastReadingTotal").html(GlobalFormat(lastReadingTotal));
                $("#tdCurrentReadingTotal").html(GlobalFormat(currentReadingTotal));
                $("#tdWaterConsumptionTotal").html(GlobalFormat(TotalConsumption));
                $("#tdTotalAmount").html(GlobalFormat(TotalAmount));
                $(".tdTotal").show();
            }

            initTooltip();
        }
    });
}

function Print() {
    if (validateForm()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintInconsistenceReading",
            data: {
                'period': new Date($("#Period").datepicker('getDate')).toDateString()
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

function initTooltip() {
    $('#tblInconsistenceReading tbody tr td').tooltip({
        container: "body",
        html: true
    });
}

function validateForm() {
    var isvalid = true;
    $("#Period").removeClass("error");
    if ($("#Period").val() == undefined || $("#Period").val() == '') {
        $("#Period").addClass("error");
        isvalid = false;
    }

    if (isvalid) {
        $($("#FormattedPeriod")).val(new Date($("#Period").datepicker('getDate')).toDateString());
    }
    return isvalid;
}

function initDataTable() {
    if ($("#tblInconsistenceReading tbody tr.no-data").length == 1) {
        return false;
    }
    $('#tblInconsistenceReading').DataTable({
        "scrollY": "100vh",
        "scrollX": true,
        scrollCollapse: true,
        "paging": false,
        "ordering": false,
        "info": false,
        "searching": false,
        "lengthChange": false,
        responsive: false,
        autoWidth: false
    });
}