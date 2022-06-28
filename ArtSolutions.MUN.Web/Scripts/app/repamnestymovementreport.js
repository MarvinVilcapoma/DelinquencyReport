$(document).ready(function () {
    initializeDatePicker();
});

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
    $('#FromDate').datepicker("setDate", new Date(dt.getFullYear(), dt.getMonth(), dt.getDate() - 1));
    $('#ToDate').datepicker('setDate', new Date(dt.getFullYear(), dt.getMonth(), dt.getDate()));
}

$(document).on('click', '#btnGo', function () {
    if (validateForm()) {
        InitializeData();
        return true;
    }
    return false;
});

$(window).resize(function () {
    $('#amnestyMovementTable').DataTable().columns.adjust().responsive.recalc();
});

function validateForm() {
    var isvalid = false;
    if ($("#FromDate").val() == undefined || $("#FromDate").val() == '')
        showAlert('error', $("#FromDate").attr("data-required-msg"));
    else if ($("#ToDate").val() == undefined || $("#ToDate").val() == '')
        showAlert('error', $("#ToDate").attr("data-required-msg"));
    //else if (new Date($("#ToDate").datepicker('getDate')) <= new Date($("#FromDate").datepicker('getDate')))
    //    showAlert('error', $("#ToDate").attr("data-compare-val-msg"));
    else if (new Date($("#ToDate").datepicker('getDate')) < new Date($("#FromDate").datepicker('getDate')))
        showAlert('error', $("#ToDate").attr("data-compare-val-msg"));
    else
        isvalid = true;
    return isvalid;
}

function initDataTable() {
    if ($("#amnestyMovementTable tbody tr.no-data").length == 1) {
        return false;
    }
    accountStatementDataTable = $('#amnestyMovementTable').DataTable({
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
        url: ROOTPath + "/Reports/Report/AmnestyMovementReport",
        data: {
            'FromDate': new Date($("#FromDate").datepicker('getDate')).toDateString(),
            'ToDate': new Date($("#ToDate").datepicker('getDate')).toDateString()
        },
        success: function (response) {
            if (response.status) {
                hideLoading();
                $("#divAmnestyMovement").html("").html(response.data);
                initDataTable();
                $('#amnestyMovementTable tbody tr td').tooltip({
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
            url: ROOTPath + "/Reports/Report/PrintAmnestyMovementReport",
            data: {
                'FromDate': $("#FromDate").val(),
                'ToDate': $("#ToDate").val()
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