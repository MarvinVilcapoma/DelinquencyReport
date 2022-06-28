$(document).ready(function () {
    $("#txtSearch").focus();
    $("#ServiceTypeIDs").select2({
        placeholder: serviceType
    });
    if ($("#ddlSearchTypeList").val() == "2")
        $("#txtSearch").val($("#top-search").val());
    InitializeDataTable();
    $(document).on("change", "#ServiceTypeIDs", function (e) {
        if ($(this).val() != null && $(this).val()[0] == 0) {
            $(this).val("").click();
            $("#ServiceTypeIDs option:gt(0)").prop('selected', true);
            $(this).trigger('change');
        }
    });

    $("#tblAccountService").on("click", ".btnDelete", function () {
        var confMessage = "";
        var textmessage = "";
        var AccountServiceID = $(this).attr("data-id");
        $("#DeleteConfirmModal #txtReason").val('');
        $("#DeleteConfirmModal").attr("data-accountserviceid", AccountServiceID);
        $("#DeleteConfirmModal").modal("show");

    });
    $("#DeleteConfirmModal").on("click", "#btnModalDelete", function () {
        var confMessage = "";
        var textmessage = "";
        var AccountServiceID = $("#DeleteConfirmModal").attr("data-accountserviceid");
        if ($("#frmDeleteService").valid()) {
            swal({
                title: suremsg,
                text: textmessage,
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: confMessage,
                cancelButtonText: cancel,
                closeOnConfirm: true
            }, function () {
                DeleteRecord(AccountServiceID);
            });
        }
    });

    $(".ExportMeasureWater").on("click", function () {
        showLoading();        
        $.fileDownload(ROOTPath + "/AccountService/ExportMeasureWater", {
            successCallback: function (url) {
                hideLoading();
            },
            failCallback: function (responseHtml, url) {
                showAlert("error", errorMessage);
                hideLoading();
            }
        });
    });



});

$(document).on('click', '#btnSearch', function () {
    $('#tblAccountService').DataTable().search($('#txtSearch').val()).draw();
});

$("#btnrefresh").click(function () {
    $("#txtSearch").focus();
    $('#txtSearch').val('');
    InitializeDataTable();
});

function DeleteRecord(AccountServiceID) {
    $.ajax({
        type: "GET",
        url: ROOTPath + '/Accounts/AccountService/Delete',
        data: { id: AccountServiceID, Reason: $("#DeleteConfirmModal #txtReason").val() },
        success: function (data) {
            if (data.status == true) {
                $("#DeleteConfirmModal").modal("hide");
                $('#tblAccountService').DataTable().search($('#txtSearch').val()).draw();
                showAlert('success', data.message);
            }
            else
                showAlert('error', data.message);
        },
        error: function () { }
    });

}

function InitializeDataTable() {
    $('#tblAccountService').dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg,
            oPaginate: {
                sPrevious: previous,
                sNext: next
            }
        },
        "fnDrawCallback": function () {
            $('#tblAccountService tbody tr td').each(function () {
                if ($(this).index() <= 6 && ($(this).index() != 0 && $(this).index() != 4))
                    this.setAttribute('title', $(this).text());
            });

            /* Apply the tooltips */
            $('#tblAccountService tbody tr td').tooltip({
                container: "body",
                html: true
            });
        },
        "serverSide": true,
        "processing": true,
        "bFilter": false,
        "bInfo": false,
        "bLengthChange": false,
        "autoWidth": false,
        "pageLength": pageSize,
        "ordering": true,
        "order": [[1, "asc"]],
        "conditionalPaging": true,
        "ajax": {
            "url": ROOTPath + '/AccountService/Get',
            "type": "POST",
            "data": function (data) {
                data.filterText = $("#txtSearch").val();
                data.ServiceTypeIDs = getServiceTypeIDs();
            }
        },
        destroy: true,
        "columns": [
            {
                name: "Active",
                "bSortable": false,
                className: "col-lg-1",
                "data": function (row) {
                    var str = "";
                    if (row.IsActive == true)
                        str = '<a href="javascript:;" id="active_' + row.ID + '" onclick="ConfirmActivedeactive(\'Active\',' + row.ID + ')"><span class="label label-primary">' + active + '</span></a>' + '<a href="javascript:;" onclick="ConfirmActivedeactive(\'Deactive\',' + row.ID + ')" style="display:none;" id="Edit_' + row.ID + '"><span class="label label-warning">' + inactivebtn + '</span></a>';
                    if (row.IsActive == false)
                        str = '<a href="javascript:;" id="Edit_' + row.ID + '" onclick="ConfirmActivedeactive(\'Deactive\',' + row.ID + ')"><span class="label label-warning">' + inactivebtn + '</span></a>' + '<a href="javascript:;" onclick="ConfirmActivedeactive(\'Active\',' + row.ID + ')" style="display:none;" id="active_' + row.ID + '"><span class="label label-primary">' + active + '</span></a>';
                    return str;
                }
            },
            {
                name: "SSEIN", title: identification, "data": "SSEIN", className: "col-lg-1 table-description-field"
            },
            {
                name: "AccountName", title: accountName, "data": "AccountName", className: "col-lg-2 table-description-field"
            },
            {
                name: "LicenseNumber", title: serviceNumber, "data": "LicenseNumber", className: "col-lg-1 table-description-field"
            },
            {
                name: "Year", title: year, "data": "Year", className: "col-lg-1"
            },
            //{
            //    name: "LicenseType", title: serviceType, "data": "LicenseType", className: "col-lg-1 table-description-field"
            //},
            {
                name: "ServiceName", title: serviceName, "data": "ServiceName", className: "col-lg-2 table-description-field"
            },
            {
                name: "CustomField1", title: customField1, "data": "CustomField1", className: "col-lg-1 table-description-field"
            },
            {
                name: "Payment Plan",
                title: paymentPlan,
                "bSortable": false,
                className: "col-lg-1",
                "data": function (row) {
                    var str = "";
                    if (row.AccountPaymentPlanID > 0)
                        str = '<a target="_blank" href = ' + ROOTPath + "/AccountPaymentPlan/View?accountPaymentPlanID=" + row.AccountPaymentPlanID + '><span class="label label-primary">' + viewPlan + '</span></a>';
                    return str;
                }
            },
            {
                name: "Actions",
                title: actions,
                className: "col-lg-2 text-right",
                "bSortable": false,
                "data": function (row) {

                    var str = "";
                    str = '<a href=' + ROOTPath + "/AccountService/View?accountServiceID=" + row.ID + ' class="btn btn-white btn-sm"><i class="fa fa-folder"></i> ' + view + ' </a>';

                    var accountPaymentPlanID = row.AccountPaymentPlanID;
                    var annualIncome = row.AnnualIncome;

                    var _accountID = null;
                    if (row.AccountTypeID = 6 && row.OwnerID != null)
                        _accountID = row.OwnerID;
                    else
                        _accountID = row.AccountID;

                    if (accountPaymentPlanID > 0 && annualIncome > 0) {
                        //Reciept By Payment Plan
                        str = str + '<a id="btnPay_' + row.ID + '" href=' + ROOTPath + "/Collections/Payment/Add/?ActionType=paymentplan&accountID=" + _accountID + '&accountPaymentPlanID=' + accountPaymentPlanID + '  class="btn btn-white btn-sm m-l-xs ' + ((row.IsActive == true) ? "" : "hide") + '"><i class="fa fa-money"></i> ' + pay + ' </a>';
                    }
                    else if (accountPaymentPlanID == null && annualIncome > 0) {
                        //Reciept By Service
                        str = str + '<a id="btnPay_' + row.ID + '" href=' + ROOTPath + "/Collections/Payment/Add/?ActionType=service&accountID=" + _accountID + '&serviceTypeID=' + row.ServiceTypeID + '  class="btn btn-white btn-sm m-l-xs ' + ((row.IsActive == true) ? "" : "hide") + '"><i class="fa fa-money"></i> ' + pay + ' </a>';
                    }

                    //if (row.PaidAmount == null || row.PaidAmount == 0) {                    
                    if (row.AccountPaymentPlanID == null || row.AccountPaymentPlanID == 0) {
                        str = str + '<a href="javascript:void(0)" data-id="' + row.ID + '" class="btn btn-white btn-sm btnDelete m-l-xs"><i class="fa fa-trash"></i> ' + Delete + ' </a>';
                    }
                    //}

                    return str;
                }
            }
        ],
    });
}

function ConfirmActivedeactive(str, val) {
    var textmessage = "";
    var confMessage = "";
    var action = true;
    $('#txtSearch').focus();
    if (str.trim().toLowerCase() == "active") {
        confMessage = inactiveconfirm;
        action = false;
    }
    else {
        confMessage = activeconfirm;
        action = true;
    }

    var AccountServiceID = val;
    $("#txtReason").val('');
    //$(".lblStatus").html(str.trim())
    $("#btnModalStatus").html(confMessage)
    $("#StatusConfirmModal #txtReason").val('')
    $("#StatusConfirmModal").attr("data-accountserviceid", AccountServiceID)
    $("#StatusConfirmModal").attr("data-action", action)
    $("#StatusConfirmModal").modal("show")

    //swal({
    //    title: suremsg,
    //    text: textmessage,
    //    type: "warning",
    //    showCancelButton: true,
    //    confirmButtonColor: "#DD6B55",
    //    confirmButtonText: confMessage,
    //    cancelButtonText: cancel,
    //    closeOnConfirm: true
    //}, function () {
    //    InActiveAccount(val, action);
    //});
}

function InActiveAccount(Id, isActive) {
    $.ajax({
        type: "GET",
        url: ROOTPath + '/AccountService/ActiveDeactive',
        data: { isActive: isActive, iD: Id, "Reason": $("#StatusConfirmModal #txtReason").val() },
        success: function (data) {
            if (data.status == true) {
                $("#StatusConfirmModal").modal("hide");
                if (isActive == "true") {
                    msg = activemsg;
                    $("#Edit_" + Id).css("display", "none");
                    $("#active_" + Id).css("display", "block");

                    $("#btnPay_" + Id).removeClass("hide");
                }
                else {
                    msg = deactivemsg;
                    $("#active_" + Id).css("display", "none");
                    $("#Edit_" + Id).css("display", "block");

                    $("#btnPay_" + Id).addClass("hide");
                }
                showAlert('success', msg);
                $('#txtSearch').focus();
            }
            else
                showAlert('error', data.message);
        }, error: function () { }
    }).always(function () {
    });
}

function getServiceTypeIDs() {
    var selectServiceTypeList = [];
    if ($("#ServiceTypeIDs").select2('data').length) {
        $.each($("#ServiceTypeIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectServiceTypeList += "," + item.id;
        });
    }
    var selectedServiceTypeIDs = "";
    if (selectServiceTypeList.length > 0)
        selectedServiceTypeIDs = selectServiceTypeList.substring(1, selectServiceTypeList.length);
    return selectedServiceTypeIDs;
}

$("#StatusConfirmModal").on("click", "#btnModalStatus", function () {
    var confMessage = $("#btnModalStatus").html();
    var textmessage = "";
    var AccountServiceID = $("#StatusConfirmModal").attr("data-accountserviceid");
    var action = $("#StatusConfirmModal").attr("data-action");
    if ($("#frmStatusService").valid()) {
        swal({
            title: suremsg,
            text: textmessage,
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: confMessage,
            cancelButtonText: cancel,
            closeOnConfirm: true
        }, function () {
            InActiveAccount(AccountServiceID, action);
        });
    }
});