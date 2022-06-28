var from = 0;

$(document).ready(function () {

    AccountServiceGet(SetCustomSearch(0));

    $(".select2dropdown").select2({ width: '100%' });

    $("#AccountID").on("change", function () {
        var accountID = this.value;
        var accountModel = $.grep(accountModels, function (e) {
            return (e.ID == accountID);
        });
        AccountServiceGet(SetCustomSearch(accountID));
    });

    var saveResult = function (data) {
        from = data.from;
    };

    $("#Weight").ionRangeSlider({
        min: 1,
        max: 10,
        from: 1,
        to: 10,
        onLoad: function (data) {
            saveResult(data);
        },
        onChange: saveResult,
        onFinish: saveResult
    });

    $("#btnNew").click(function () {
        SaveNewData();
    });

    $.validator.addMethod('selectRule', function (value, element) {
        return parseInt($(element).val()) > 0; // return bool here if valid or not.
    }, requiredValidationMsg);

    $('#form').validate();

    $("#btnCancel").click(function () {
        window.location = ROOTPath + "/Cases/Case/List";
    });
   
});

function AccountServiceGet(customSearch) {
    $('#tblAccountServices').dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg
        },
        "scrollY": 160,
        scrollCollapse: true,
        "bPaginate": false,
        "serverSide": true,
        "processing": false,
        "bFilter": false,
        "bInfo": false,
        "bLengthChange": false,
        "ordering": false,
        "ajax": {
            "url": ROOTPath + "/Case/AccountServiceGet",
            "type": "POST",
            "data": function (data) {
                data.customSearch = customSearch;
            }
        },
        destroy: true,
        "columns": [
                {
                    className: "text-center", sortable: false, width: "5%", "data": function (data, type) {
                        return "<input type=\"checkbox\" class=\"i-checks\" name=\"chkItems\" data-accountServiceID=\"" + data.ID + "\" />";
                    }
                },
              {
                  name: "ID", title: id, "data": "ID", width: "8%"
              },
               {
                   name: "Service", title: service, "data": "ServiceName", width: "13%"
               },
              {
                  name: "Year", title: year, "data": "Year", width: "10%"
              },
               {
                   name: "LicenseType", title: licenseType, "data": "LicenseType", width: "10%"
               },
                {
                    name: "Balance", title: balance, "data": function (data, type) {
                        return data.AnnualIncomeWithCurrencyCode;
                    }, width: "10%", className: "text-right"
                }
        ],
        "initComplete": function (settings, json) {
            $('.i-checks').iCheck({
                checkboxClass: 'icheckbox_square-blue'
            });
        }
    });
}

function SetCustomSearch(accountID) {
    var data = {
        AccountID: accountID
    };
    return data;
}

function SaveNewData() {
    var isFormValid = $("#form").valid();
    if (isFormValid) {
        var selectedRowsData = GetDataTableSelectedRows();
        if (selectedRowsData.accountServiceID.length == 0) {
            showAlert("warning", requiredGridValidationMsg);
            return false;
        }
        var model = SetModel(selectedRowsData.accountServiceID.join(","));
        $.post(ROOTPath + "/Case/Add", model, function (response) {
            if (!response.status) {
                showAlert("error", response.message);
            }
            else {
                window.location = ROOTPath + "/Cases/Case/List";
            }
        });
    }
    return isFormValid;
}

function GetDataTableSelectedRows() {
    var accountServiceID = new Array();
    var oTable = $('#tblAccountServices').dataTable();
    var rowcollection = oTable.$("input[name='chkItems']", { "page": "all" });
    var rowSelectedcollection = oTable.$("input[name='chkItems']:checked", { "page": "all" });
    rowSelectedcollection.each(function (index, elem) {
        accountServiceID.push($(elem).attr("data-accountServiceID"));
    });
    return {
        accountServiceID: accountServiceID,
        rowcollection: rowcollection.length
    };
}

//Set Case Model for create new case.
function SetModel(accountServices) {
    return {
        AccountID: $("#AccountID").val(),
        AccountServices: accountServices,
        WorkflowID: $("#WorkflowID").val(),
        Name: $("#Name").val(),
        Refrence: $("#Refrence").val(),
        Note: $("#Note").val(),
        PriorityID: $("#PriorityID").val(),
        Weight: from == 0 ? 1 : from,
        OwnerID: $("#OwnerID").val(),
        AssignToID: $("#AssignToID").val(),
        PriorityName: $("#PriorityID option:selected").text(),
        AccountName: $("#AccountID option:selected").text()
    };
}