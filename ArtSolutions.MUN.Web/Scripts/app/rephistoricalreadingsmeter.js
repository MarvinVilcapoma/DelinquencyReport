$(document).ready(function () {
    $('#dvUpdate').hide();
    GetAccountForSelect('AccountId', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px');

    if ($("#hdAccountID").val() > 0 && validateForm()) {
        InitializeData();
    }
});

$(document).on('click', '#btnGo', function () {
    if (validateForm()) {
        InitializeData();
    }
    return false;
});

function initDataTable() {
    $('#historicalReadingsMeterTable').DataTable({
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
            $('#historicalReadingsMeterTable tbody tr td').each(function () {
                if ($(this).find('#hdMeter').val() != undefined && $(this).index() == 0) {
                    this.setAttribute('title', $(this).find('#hdMeter').val());
                }
                if ($(this).find('#hdAccountName').val() != undefined && $(this).index() == 1) {
                    this.setAttribute('title', $(this).find('#hdAccountName').val());
                }
            });

            /* Apply the tooltips */
            $('#historicalReadingsMeterTable tbody tr td').tooltip({
                container: "body",
                html: true
            });
        },
        "scrollY": "100vh",
        "scrollX": true,
        scrollCollapse: true
    });
}

function InitializeData() {
    $.ajax({
        type: "POST",
        url: ROOTPath + "/Reports/Report/HistoricalReadingsByMeter",
        data: {
            'meter': $("#txtMeter").val(),
            'accountID': $("#AccountId").val()
        },
        success: function (response) {
            if (response.status) {
                hideLoading();
                $("#divHistoricalReadingsMeter").html("").html(response.data);
                initDataTable();
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
            url: ROOTPath + "/Reports/Report/PrintHistoricalReadingsByMeter",
            data: {
                'meter': $("#txtMeter").val(),
                'accountID': $("#AccountId").val()
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

function validateForm() {
    var isvalid = true;
    return isvalid;
}