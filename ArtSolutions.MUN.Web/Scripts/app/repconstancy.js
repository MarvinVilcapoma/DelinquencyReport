$(document).ready(function () {
   
});

$(document).on('click', '#btnGo', function () {
    if ($("#form").valid()) {
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
        url: ROOTPath + "/Reports/Report/Constancy",
        data: {
            'taxNumber': $("#txtTaxNumber").val(),
            'note': $("#Note").val()
        },
        success: function (response) {
            if (response.status) {
                hideLoading();
                $("#divConstancyReport").html("").html(response.data);

                if (response.AccountID > 0) {
                    initDataTable();
                    $('#noDebtCertificationDataTable tbody tr td').tooltip({
                        container: "body",
                        html: true
                    });
                } else {
                    $('#Description1Detail').text(txtdescription1);
                    $('#Description2Detail').text(txtdescription2);
                }

                $(".constancy").removeClass("hidden");
            }
            else {
                showAlert('error', response.data);
            }
        }
    });
}

function Print() {
    if ($("#form").valid()) {
        noDebtCertificationModel = {};
        noDebtCertificationModel.UserName = $("#UserName").val();
        noDebtCertificationModel.UserID = $("#UserID").val();
        noDebtCertificationModel.Description1Detail = $("#Description1Detail").val();
        noDebtCertificationModel.Description2Detail = $("#Description2Detail").val();
        noDebtCertificationModel.TaxNumber = $("#txtTaxNumber").val();

        $.ajax({
            type: "POST",
            url: ROOTPath + "/Reports/Report/PrintConstancy",
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
    return false;
}