$(document).ready(function () {
    GetAccountForSelect('AccountId', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px');
});

$(document).on('click', '#btnGo', function () {
    if (checkAccountIdInput() && $("#form").valid()) {
        InitializeData();
    }
    return false;
});

var debtCertificationDataTable = null;

function initDataTable() {
    debtCertificationDataTable = $('#debtCertificationDataTable').DataTable({
        "initComplete": function (settings, json) {
            $("#debtCertificationDataTable").wrap("<div style='max-height: 570px;overflow:auto; width:100%;position:relative;'></div>");
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
        }
    });
}

function InitializeData() {
    if ($("#AccountId").val() > 0) {
        $.ajax({
            type: "POST",
            url: ROOTPath + "/Reports/Report/DebtCertification",
            data: {
                'accountId': $("#AccountId").val()
            },
            success: function (response) {
                if (response.status) {
                    hideLoading();
                    $("#divDebtCertificationReport").html("").html(response.data);
                    //initDataTable();
                    $('#debtCertificationDataTable tbody tr td').tooltip({
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
}

function Print(data) {
    if (checkAccountIdInput() && $("#form").valid()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintDebtCertification",
            data:
            {
                'accountId': $("#AccountId").val()
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
                    return false;
                }
            }
        });
    }
    else
        return false;
    return true;
}

function checkAccountIdInput() {
    var isvalid = true;
    if ($("#AccountId").val() == '') {
        $("#AccountId").focus();
    }
    return isvalid;
}