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
    $('#StartDate').datepicker("setDate", new Date(dt.getFullYear(), dt.getMonth(), dt.getDate() - 1));
    $('#EndDate').datepicker('setDate', new Date(dt.getFullYear(), dt.getMonth(), dt.getDate()));
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

function validateForm() {
    var isvalid = false;
    if ($("#StartDate").val() == undefined || $("#StartDate").val() == '')
        showAlert('error', $("#StartDate").attr("data-required-msg"));
    else if ($("#EndDate").val() == undefined || $("#EndDate").val() == '')
        showAlert('error', $("#EndDate").attr("data-required-msg"));
    //else if (new Date($("#EndDate").datepicker('getDate')) <= new Date($("#StartDate").datepicker('getDate')))
    //    showAlert('error', $("#EndDate").attr("data-compare-val-msg"));
    else if (new Date($("#EndDate").datepicker('getDate')) < new Date($("#StartDate").datepicker('getDate')))
        showAlert('error', $("#EndDate").attr("data-compare-val-msg"));
    else
        isvalid = true;
    return isvalid;
}

function initDataTable() {
    if ($("#cashReceiptControlTable tbody tr.no-data").length == 1) {
        return false;
    }
    accountStatementDataTable = $('#cashReceiptControlTable').DataTable({
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
        url: ROOTPath + "/Reports/Report/CashReceiptControl",
        data: {
            'startDate': new Date($("#StartDate").datepicker('getDate')).toDateString(),
            'endDate': new Date($("#EndDate").datepicker('getDate')).toDateString()
        },
        success: function (response) {
            if (response.status) {
                hideLoading();
                $("#divCashReceiptControl").html("").html(response.data);
                initDataTable();
                $('#cashReceiptControlTable tbody tr td').tooltip({
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
            url: ROOTPath + "/Reports/Report/PrintCashReceiptControl",
            data: {
                'startDate': $("#StartDate").val(),
                'endDate': $("#EndDate").val()
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