$(document).ready(function () {
    $(".select2dropdown").select2();
    initializeDatePicker();
});

$(document).on('click', '#btnGo', function () {
    if ($("#form").valid()) {
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
        format: __dateFormat,
        language: __culture
    });
    var dt = new Date();
    $('#Date').datepicker("setDate", new Date(dt.getFullYear(), dt.getMonth(), dt.getDate() - 1));
}

function initDataTable() {
    $('#StatisticsofReceiptsCollectedTable').DataTable({
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
    });
}

function InitializeData() {
    $.ajax({
        type: "POST",
        url: ROOTPath + "/Reports/Report/StatisticsofReceiptsCollected",
        data: {
            'date': new Date($("#Date").datepicker('getDate')).toDateString()
            , 'bankId': $("#BankID").val()
            , 'contract': $("#Contract").val()
        },
        success: function (response) {

            $("#spTitle").html(title + " " + In + " " + the);
            $("#spSubTitle").html($("#BankID option:selected").text() + " " + the + " " + $("#Date").val() + "  " + "  " + agreement + "  " + $("#Contract").val() + " = " + $("#Contract option:selected").text());

            if (response.status) {
                hideLoading();
                $("#divStatisticsofReceiptsCollected").html("").html(response.data);
                initDataTable();
                $('#StatisticsofReceiptsCollectedTable .table-description-field').tooltip({
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

function GetSubTitle() {
    if ($("#form").valid()) {
        $("#spTitle").html(title + " " + In + " " + the);
        $("#spSubTitle").html($("#BankID option:selected").text() + " " + the + " " + $("#Date").val() + "  " + "  " + agreement + "  " + $("#Contract").val() + " = " + $("#Contract option:selected").text());
        return true;
    }
    return false;
}

function Print() {
    if ($("#form").valid()) {
        $("#spTitle").html(title + " " + In + " " + the);
        $("#spSubTitle").html($("#BankID option:selected").text() + " " + the + " " + $("#Date").val() + "  " + "  " + agreement + "  " + $("#Contract").val() + " = " + $("#Contract option:selected").text());

        $.ajax({
            type: "GET",
            //async: false,
            url: ROOTPath + "/Reports/Report/PrintStatisticsofReceiptsCollected",
            data: {
                'date': new Date($("#Date").datepicker('getDate')).toDateString()
                , 'bankId': $("#BankID").val()
                , 'contract': $("#Contract").val()
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