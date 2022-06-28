$(document).ready(function () {
    GetAccountForSelect('AccountId', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px');
});

$('#AccountId').on('select2:select', function (e) {
    $("#Note").val(noDebtCertificateNotePreLoadText);
    $('#btnGo').click();
});

$(document).on('click', '#btnGo', function () {
    if (checkAccountIdInput() && $("#form").valid()) {
        InitializeData();
    }
    return false;
});

$(window).resize(function () {
    $('#noDebtCertificationDataTable').DataTable().columns.adjust().responsive.recalc();
});

function initDataTable() {
    if ($("#noDebtCertificationDataTable tbody tr.no-data").length == 1) {
        return false;
    }
    noDebtCertificationDataTable = $('#noDebtCertificationDataTable').DataTable({
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
        "fnDrawCallback": function () {
            $("#divBottomContent").removeClass("hide");
        },
        "scrollY": "100vh",
        "scrollX": true,
        scrollCollapse: true
    });
}

function InitializeData() {
    $.ajax({
        type: "POST",
        url: ROOTPath + "/Reports/Report/NoDebtCertification",
        data: {
            'accountId': $("#AccountId").val(),
            'note': $("#Note").val()
        },
        success: function (response) {
            if (response.status) {
                hideLoading();
                $("#divNoDebtCertificationReport").html("").html(response.data);
                initDataTable();
                $('#noDebtCertificationDataTable tbody tr td').tooltip({
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
    if (checkAccountIdInput() && $("#form").valid()) {
        noDebtCertificationModel = {};
        noDebtCertificationModel.AccountId = $("#AccountId").val();
        noDebtCertificationModel.Note = $("#Note").val();
        noDebtCertificationModel.Observations = $("#Observations").val();

        $.ajax({
            type: "POST",
            url: ROOTPath + "/Reports/Report/PrintNoDebtCertification",
            data: {
                'model': noDebtCertificationModel
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

function checkAccountIdInput() {
    var isvalid = true;
    if ($("#AccountId").val() == '') {
        $("#AccountId").focus();
    }
    return isvalid;
}