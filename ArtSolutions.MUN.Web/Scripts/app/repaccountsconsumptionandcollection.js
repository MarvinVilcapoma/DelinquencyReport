$(document).ready(function () {
    initializeDatePicker();
    $("#btnGo").focus();
});

$(document).on('click', '#btnGo', function () {
    if (validateForm()) {
        InitializeData();
        return true;
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
    $('#tblAccountsConsumptionAndCollection').DataTable({
        "initComplete": function (settings, json) {
            $("#tblAccountsConsumptionAndCollection").wrap("<div style='max-height: 570px;overflow:auto; width:100%;position:relative;'></div>");
        },
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
        columnDefs: [
            // Center align the header content of column
            { className: "dataTable th" }
        ]
    });
}

function InitializeData() {
    $.ajax({
        type: "POST",
        url: ROOTPath + "/Reports/Report/AccountsConsumptionAndCollection",
        data: {
            'Period': new Date($("#Period").datepicker('getDate')).toDateString()
        },
        success: function (response) {
            if (response.status) {
                hideLoading();
                $("#divAccountsConsumptionAndCollection").html("").html(response.data);
                initDataTable();
                $("#spnPeriod").html(response.Period);
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
            url: ROOTPath + "/Reports/Report/PrintAccountsConsumptionAndCollection",
            data: {
                'Period': new Date($("#Period").datepicker('getDate')).toDateString()
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