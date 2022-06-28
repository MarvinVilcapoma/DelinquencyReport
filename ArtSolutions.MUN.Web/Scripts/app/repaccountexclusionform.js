$(document).ready(function () {
    $(".select2dropdown").select2({ width: '300px' });

    GetAccountForSelect('AccountId', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px');
});

$('#AccountId').on('select2:select', function (e) {
    $("#Observations").val(null);
    $('#btnGo').click();
});

$(document).on('click', '#btnGo', function () {
    if (checkAccountIdInput() && $("#form").valid()) {
        InitializeData();
        return true;
    }
    return false;
});

$(window).resize(function () {
    if (checkAccountIdInput() && $("#form").valid()) {
        InitializeData();
        return true;
    }
    return false;
});


function checkAccountIdInput() {
    var isvalid = true;
    if ($("#AccountId").val() == '') {
        $("#AccountId").focus();
    }
    return isvalid;
}

function initDataTable() {
    if ($("#tblPropertyTax tbody tr.no-data").length == 1) {
        return false;
    }

    $('#tblPropertyTax').DataTable({
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
            initWaterMeasureDataTable();
        },
        "scrollY": "100vh",
        "scrollX": true,
        "scrollCollapse": true
    });
}

function initWaterMeasureDataTable() {
    if ($("#tblWaterMeasure tbody tr.no-data").length == 1) {
        return false;
    }

    $('#tblWaterMeasure').DataTable({
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
            initOtherServicesDataTable();
        },
        "scrollY": "100vh",
        "scrollX": true,
        "scrollCollapse": true
    });
}

function initOtherServicesDataTable() {
    if ($("#tblOtherServices tbody tr.no-data").length == 1) {
        return false;
    }

    $('#tblOtherServices').DataTable({
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
        "scrollCollapse": true
    });
}

function InitializeData() {
    if ($("#AccountId").val() > 0) {
        $.ajax({
            type: "POST",
            url: ROOTPath + "/Reports/Report/AccountExclusionForm",
            data: {
                'accountId': $("#AccountId").val(),
                'observations': $("#Observations").val()
            },
            success: function (response) {
                if (response.status) {
                    hideLoading();
                    $("#divAccountExclusionForm").html("").html(response.data);
                    initDataTable();

                    $('#tblPropertyTax tbody tr td').tooltip({
                        container: "body",
                        html: true
                    });
                    $('#tblWaterMeasure tbody tr td').tooltip({
                        container: "body",
                        html: true
                    });
                    $('#tblOtherServices tbody tr td').tooltip({
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

function Print() {
    if (checkAccountIdInput() && $("#form").valid()) {
        accountExclusionFormModel = {};
        accountExclusionFormModel.AccountId = $("#AccountId").val();
        accountExclusionFormModel.Observations = $("#Observations").val();

        $.ajax({
            type: "POST",
            url: ROOTPath + "/Reports/Report/PrintAccountExclusionForm",
            data: {
                'model': accountExclusionFormModel
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