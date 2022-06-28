$(document).ready(function () {
    if ($("#hdAccountID").val() > 0) {
        GetAccountForSelect('AccountId', $("#hdAccountID").val(), null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px');
    }
    else {
        GetAccountForSelect('AccountId', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px');
    }

    $("#ddlRight").select2();

    if ($("#hdAccountID").val() > 0 && checkAccountIdInput() && $("#form").valid()) {
        InitializeData();
    }
});

$(document).on('click', '#btnGo', function () {
    if (checkAccountIdInput() && $("#form").valid()) {
        InitializeData();
    }
    return false;
});

var PropertyDataTable = null;

function initDataTable() {
    if ($("#PropertyTable tbody tr.no-data").length == 1) {
        return false;
    }
    PropertyDataTable = $('#PropertyTable').DataTable({        
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
    if ($("#AccountId").val() > 0) {
        $.ajax({
            type: "POST",
            url: ROOTPath + "/Reports/Report/Property",
            data: {
                'accountId': $("#AccountId").val(),
                'accountPropertyIDs': $("#ddlRight").val(),
                'balanceFrom': null,
                'balanceTo': null
            },
            success: function (response) {
                if (response.status) {
                    hideLoading();
                    $("#divPropertyReport").html("").html(response.data);
                    initDataTable();
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
            url: ROOTPath + "/Reports/Report/PrintProperty",
            data:
            {
                'accountId': $("#AccountId").val(),
                'accountPropertyID': $("#ddlRight").val(),
                'balanceFrom': null,
                'balanceTo': null
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

function GetRight() {
    if ($("#AccountId").val() > 0) {
        $.ajax({
            url: ROOTPath + "/AccountService/GetAccountPropertyRightByOwner",
            data: {
                ownerID: $("#AccountId").val(),
                serviceID: 0,
                fiscalYearID: 0,
                ID: null,
                forAccountStatementReport: false
            },
            success: function (data) {
                $("#ddlRight").empty();
                console.log(data.length)
                if (data.length > 0) {
                    $.each(data, function (index, optiondata) {
                        $("#ddlRight").append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
                    });
                    $("#ddlRight").select2({ width: '100%' });
                    $("#ddlRight option:first").val("");                    
                }
                else {
                    var str = AccounthaveNoRightsText;
                    str = str.replace("{1}", $("#AccountId").text());
                    str = str.replace("{1}", "");
                    showAlert("error", str);
                    $("#divPropertyReport").html(null);
                }
              
            }
        });
    }
    else {
        $("#ddlRight").empty();
        $("#ddlRight").append("<option value='" + '' + "'>" + defaultSelectText + "</option>");
        $("#divPropertyReport").html(null);
    }
}