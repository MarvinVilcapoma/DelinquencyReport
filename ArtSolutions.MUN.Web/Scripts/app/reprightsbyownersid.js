$(document).ready(function () {
    GetAccountForSelect('OwnerID', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px');
});

//$('#OwnerID').on('select2:select', function (e) {   
//    $('#btnGo').click();
//});

$(document).on('click', '#btnGo', function () {
    if (CheckOwnerIDInput() && $("#frmRightsByOwnersID").valid()) {
        InitializeData();
    }
    return false;
});

$(window).resize(function () {
    if (CheckOwnerIDInput() && $("#frmRightsByOwnersID").valid()) {
        InitializeData();
    }
    return false;
});

function initDataTable() {

    $('#rightsByOwnersIDDataTable').DataTable({
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

    //if ($("#rightsByOwnersIDDataTable tbody tr.no-data").length == 1) {
    //    return false;
    //}
    //rightsByOwnersIDDataTable = $('#rightsByOwnersIDDataTable').DataTable({
    //    "paging": false,
    //    "ordering": false,
    //    "info": false,
    //    "searching": false,
    //    "lengthChange": false,
    //    responsive: false,
    //    autoWidth: false,
    //    fixedHeader: true,
    //    language: {
    //        "emptyTable": nodatamsg
    //    },
    //    //"fnDrawCallback": function () {
    //    //    $("#divBottomContent").removeClass("hide");
    //    //},
    //    "scrollY": "100vh",
    //    "scrollX": true,
    //    scrollCollapse: true
    //});
}

function InitializeData() {
    $.ajax({
        type: "POST",
        url: ROOTPath + "/Reports/Report/RightsByOwnersID",
        data: {
            'ownerID': $("#OwnerID").val()
        },
        success: function (response) {
            if (response.status) {
                hideLoading();
                $("#divRightsByOwnersID").html("").html(response.data);
                initDataTable();
                $('#rightsByOwnersIDDataTable tbody tr td').tooltip({
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
    if (CheckOwnerIDInput() && $("#frmRightsByOwnersID").valid()) {
        //noDebtCertificationModel = {};
        //noDebtCertificationModel.OwnerID = $("#OwnerID").val();

        $.ajax({
            type: "POST",
            url: ROOTPath + "/Reports/Report/PrintRightsByOwnersID",
            data: {
                //'model': noDebtCertificationModel
                'ownerID': $("#OwnerID").val()
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

function CheckOwnerIDInput() {
    var isvalid = true;
    if ($("#OwnerID").val() == '') {
        $("#OwnerID").focus();
    }
    return isvalid;
}