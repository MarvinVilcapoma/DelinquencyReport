$(document).ready(function () {
    $(".select2dropdown").select2({ width: '300px' });
    
});

$(document).on('click', '#btnGo', function () {
    if ($("#form").valid()) {
        InitializeData();
        return true;
    }
    return false;
});
function initDataTable() {
    if ($("#tblTaxPayer tbody tr.no-data").length == 1) {
        return false;
    }

    $('#tblTaxPayer').DataTable({
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
        "scrollCollapse": true
    });
}
function InitializeData() {
    if ($("#ServiceTypeID").val() > 0) {
        $.ajax({
            type: "POST",
            url: ROOTPath + "/Reports/Report/AccountByServiceType",
            data: {
                'serviceTypeID': $("#ServiceTypeID").val(),
                'isNotAssignServices': $('#chkServiceAssign').prop('checked')
            },
            success: function (response) {
                if (response.status) {
                    hideLoading();
                    $("#divAccountByServiceTypeReport").html("").html(response.data);
                    //initDataTable();

                    $('#tblTaxPayer tbody tr td').tooltip({
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
function checkServiceTypeIdInput() {
    var isvalid = true;
    if ($("#ServiceTypeID").val() == '') {
        $("#ServiceTypeID").focus();
    }
    return isvalid;
}

function validateForm() {
    var isvalid = true;

    if ($("#ServiceTypeID").val() == '' || $("#ServiceTypeID").val() == null || $("#ServiceTypeID").val() == undefined) {
        isvalid = false;
        $("#ServiceTypeID").focus();
        showAlert('error', $("#ServiceTypeID").attr("data-required-msg"));
    }
    return isvalid;
}
function Print() {
    if (checkServiceTypeIdInput() && $("#form").valid()) {
        accountByServiceTypeModel = {};
        accountByServiceTypeModel.ServiceTypeID = $("#ServiceTypeID").val();
        accountByServiceTypeModel.isNotAssignServices = $('#chkServiceAssign').prop('checked');
        $.ajax({
            type: "POST",
            url: ROOTPath + "/Reports/Report/PrintAccountByServiceType",
            data: {
                'model': accountByServiceTypeModel
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