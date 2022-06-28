$(document).ready(function () {
    $('.footable').footable();
    $('.footable').footable();
    $.urlParam = function (name) {
        var results = new RegExp('[\?&]' + name + '=([^&#]*)')
            .exec(window.location.href);
        if (results) return results[1] || 0; else return null;
    };

    var tabName;
    var accType = $.urlParam('accountType');
    var groupId = $.urlParam('groupId');

    if (groupId == 1)
        tabName = 'tabRight';
    else if (groupId == 2)
        tabName = 'tabPaymentPlan';
    else if (groupId == 0)
        tabName = 'tabServices';
    else
        tabName = 'tabRight';

    activaTab(tabName);

    InitializeAccountServiceDataTable();
    InitializeAccountPaymentPlanDataTable();
});

function activaTab(tab) {
    $('.nav-tabs a[href="#' + tab + '"]').tab('show');
}
function ConfirmActivedeactive(str, val, isAccount, isService) {
    var textmessage = "";
    var confMessage = "";
    var action = true;

    if (str.trim().toLowerCase() == "active") {
        confMessage = inactiveconfirm;
        action = false;
    }
    else {
        confMessage = activeconfirm;
        action = true;
    }

    if (isAccount && str.trim().toLowerCase() == "active") {
        $.ajax({
            type: "GET",
            url: ROOTPath + '/Accounts/AccountProperty/AccountPropertyHasDebt',
            data: { id: _id, accountID: _ownerID },
            success: function (data) {
                if (data.status == true) {
                    if (data.HasDebt == true) {
                        swal({
                            title: "",
                            text: validRightHasDebt,
                            type: "warning",
                            showCancelButton: false,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: okResource,
                            closeOnConfirm: true
                        });
                    }
                    else {
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
                            InActiveRecord(val, action, isAccount, isService);
                        });
                    }
                }
                else
                    showAlert('error', data.message);
            },
            error: function () { }
        });
    }
    else {
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
            InActiveRecord(val, action, isAccount, isService);
        });
    }
}

function InActiveRecord(Id, isActive, isAccount, isService) {
    var urlStr = null;

    if (isAccount) {
        urlStr = ROOTPath + "/AccountProperty/ActiveDeactive";
    }
    else if (isService) {
        urlStr = ROOTPath + "/AccountService/ActiveDeactive";
    }
    else
        urlStr = ROOTPath + "/AccountPaymentPlan/ActiveDeactive";

    $.ajax({
        type: "GET",
        url: urlStr,
        data: { isActive: isActive, id: Id },
        success: function (data) {
            if (data.status == true) {
                if (isActive == true) {
                    msg = activemsg;
                    $("#deactive_" + Id).css("display", "none");
                    $("#active_" + Id).css("display", "block");

                    if (!isAccount)
                        $("#tr_" + Id).children("td").not(":last-child").removeClass("text-danger");
                }
                else {
                    msg = deactivemsg;
                    $("#active_" + Id).css("display", "none");
                    $("#deactive_" + Id).css("display", "block");

                    if (!isAccount)
                        $("#tr_" + Id).children("td").not(":last-child").addClass("text-danger");
                }
                showAlert('success', msg);

                if (isAccount && isActive == true) {
                    $("#lnkCopy").removeClass("disabled");
                    $("#lnkViewEdit").removeClass("disabled");
                    $("#btnNewService").removeClass("disabled");
                    $("#btnNewService").prop("disabled", "");
                }
                else if (isAccount && isActive != true) {
                    $("#lnkCopy").addClass("disabled");
                    $("#lnkViewEdit").addClass("disabled");
                    $("#btnNewService").addClass("disabled");
                    $("#btnNewService").prop("disabled", "");
                }
            }
            else
                showAlert('error', data.message);
        }, error: function () { }
    }).always(function () {
    });
}

$('#lnkMoreNotes').on('click', function () {
    $('#MoreNotes').toggle();
    if ($(this).text() == showLessText) $(this).text(showMoreText);
    else $(this).text(showLessText);
});

$(document).on("click", "#lnkViewHistory", function () {
    var code = $(this).closest('tr').attr('data-code');
    var $this = $(this);
    var timezone = /\((.*)\)/.exec(new Date().toString())[1];
    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/AccountProperty/GetFillingPropertyTaxHistory",
        data: { PropertyAccountID: $("#ID").val(), PropertyNumber: $("#PropertyNumber").val(), RightNumber: $("#RigthNumber").val(), TimeZone: timezone },
        success: function (data) {
            if (data.status == false) {
                showAlert('error', data.message);
                e.stopPropagation();
            }
            else {

                $("#divFillingPropertyTaxHistory").html(data.returnData);
                $("#TxtRightCode").val($("#RigthNumber").val());
                $("#FillingPropertyTaxHistory").modal('show');
            }
        }, error: function () {
        }
    }).always(function () {
    });
});

//Account Payment Plan List
function InitializeAccountPaymentPlanDataTable() {
    $('#tblAccountPaymentPlan').dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg,
            oPaginate: {
                sPrevious: previous,
                sNext: next
            }
        },
        "fnDrawCallback": function () {
            $('#tblAccountPaymentPlan tbody tr td').each(function () {
                if ($(this).index() == 2)
                    this.setAttribute('title', $(this).text());
            });

            /* Apply the tooltips */
            $('#tblAccountPaymentPlan tbody tr td').tooltip({
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
            "url": ROOTPath + '/Accounts/AccountPaymentPlan/List',
            "type": "POST",
            "data": function (data) {
                data.accountID = _ownerID;
                data.accountPropertyID = _id;
                data.filterText = $("#txtPaymentPlanSearch").val();
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
                        str = '<a href="javascript:;" id="active_' + row.ID + '" onclick="ConfirmActivedeactive(\'Active\',' + row.ID + ',false,false)"><span class="label label-primary">' + active + '</span></a>' + '<a href="javascript:;" onclick="ConfirmActivedeactive(\'Deactive\',' + row.ID + ',false,false)" style="display:none;" id="deactive_' + row.ID + '"><span class="label label-warning">' + inActivebtn + '</span></a>';

                    if (row.IsActive == false)
                        str = '<a href="javascript:;" id="deactive_' + row.ID + '" onclick="ConfirmActivedeactive(\'Deactive\',' + row.ID + ',false,false)"><span class="label label-warning">' + inActivebtn + '</span></a>' + '<a href="javascript:;" onclick="ConfirmActivedeactive(\'Active\',' + row.ID + ',false,false)" style="display:none;" id="active_' + row.ID + '"><span class="label label-primary">' + active + '</span></a>';
                    return str;
                }
            },
            {
                name: "Number", title: planID, "data": "Number", className: "col-lg-1 table-description-field",
            },
            {
                name: "PaymentPlanName", title: paymentPlan, "data": "PaymentPlanName", className: "col-lg-2 table-description-field",
            },
            {
                name: "StartDateByCulture", title: startDate, "data": "StartDateByCulture", className: "col-lg-2",
            },
            {
                name: "EndDateByCulture", title: endDate, "data": "EndDateByCulture", className: "col-lg-2",
            },
            {
                name: "Months", title: months, "data": "Months", className: "col-lg-1 text-right",
            },
            {
                name: "TotalPayment", title: totalAmountOfPaymentPlan, "data": "TotalPaymentByCulture", className: "col-lg-2 text-right",
            },
            {
                name: "Actions",
                className: "col-lg-1 text-right",
                "bSortable": false,
                "data": function (row) {
                    var str = "";
                    str = '<a href=' + ROOTPath + "/Accounts/AccountPaymentPlan/View?accountPaymentPlanID=" + row.ID + '  class="btn btn-white btn-sm"><i class="fa fa-folder"></i> ' + view + ' </a>';
                    return str;
                }
            }
        ]
    });
}
//

//Service List
function InitializeAccountServiceDataTable() {
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
                if ($(this).index() >= 1 && $(this).index() <= 5)
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
                data.accountID = _accountID;
                data.filterText = $("#txtAccountServiceSearch").val();
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
                        str = '<a href="javascript:;" id="active_' + row.ID + '" onclick="ConfirmActivedeactive(\'Active\',' + row.ID + ',false,true)"><span class="label label-primary">' + active + '</span></a>' + '<a href="javascript:;" onclick="ConfirmActivedeactive(\'Deactive\',' + row.ID + ',false,true)" style="display:none;" id="deactive_' + row.ID + '"><span class="label label-warning">' + inActivebtn + '</span></a>';

                    if (row.IsActive == false)
                        str = '<a href="javascript:;" id="deactive_' + row.ID + '" onclick="ConfirmActivedeactive(\'Deactive\',' + row.ID + ',false,true)"><span class="label label-warning">' + inActivebtn + '</span></a>' + '<a href="javascript:;" onclick="ConfirmActivedeactive(\'Active\',' + row.ID + ',false,true)" style="display:none;" id="active_' + row.ID + '"><span class="label label-primary">' + active + '</span></a>';

                    return str;
                }
            },
            {
                name: "ServiceName", title: service, "data": "ServiceName", className: "col-lg-3 table-description-field"
            },
            {
                name: "LicenseNumber", title: serviceID, "data": "LicenseNumber", className: "col-lg-3 table-description-field"
            },
            {
                name: "Year", title: year, "data": "Year", className: "col-lg-1"
            },
            {
                name: "FormattedAnnualIncome", title: balance, "data": "FormattedAnnualIncome", className: "col-lg-2 text-right"
            },
            {
                name: "Audit",
                title: audit,
                className: "col-lg-1 text-center",
                "bSortable": false,
                "data": function (row) {
                    var str = createdDate + " : " + row.FormattedCreatedDate + "<br/>" +
                        modifiedDate + " : " + row.FormattedModifiedDate + "<br/>" +
                        createdUser + " : " + row.FormattedCreatedUser;
                    return '<a href="#" data-toggle="tooltip" data-placement="top" data-html="true" title="' + str + '"><i class="fa fa-ellipsis-h"></i></a>';
                }
            },
            {
                name: "Actions",
                className: "col-lg-1 text-right",
                "bSortable": false,
                "data": function (row) {
                    var str = "";
                    str = '<a href=' + ROOTPath + "/AccountService/View?accountServiceID=" + row.ID + ' class="btn btn-white btn-sm"><i class="fa fa-folder"></i> ' + view + ' </a>';
                    //if (row.PaidAmount == null || row.PaidAmount == 0) {
                    //console.log(row.AccountPaymentPlanID)
                    
                    if (row.AccountPaymentPlanID == null || row.AccountPaymentPlanID == 0) {
                        str = str + '<a href="javascript:void(0)" data-id="' + row.ID + '" class="btn btn-white btn-sm btnDelete m-t-sm"><i class="fa fa-trash"></i> ' + Delete + ' </a>';
                    }
                    //}
                    return str;
                }
            }
        ],
        "headerCallback": function (thead, data, start, end, display) {
            var str = "";
            //str = '<a id="btnNewService" href=' + ROOTPath + "/AccountService/Add?accountId=" + _accountID + '&ownerID=' + _ownerID + '&accountPropertyID=' + _id + '&accountTypeId=' + _accountType + '&serviceType=' + 0 + '  class="btn btn-primary btn-xs"><i class="fa fa-plus"></i> ' + newService + ' </a>';

            str = '<a id="btnNewService" href=' + ROOTPath + "/AccountService/Add?accountId=" + _accountID + '&ownerID=' + _ownerID + '&accountPropertyID=' + _id + '&accountTypeId=' + _accountType + '&serviceType=' + 0 + '&isFromRight=' + 1 + '  class="btn btn-primary btn-xs"><i class="fa fa-plus"></i> ' + newService + ' </a>';


            $(thead).find('th').eq(6).html(str);
        }
    });
}

$("#tblAccountService").on("click", ".btnDelete", function () {
    var confMessage = "";
    var textmessage = "";
    var AccountServiceID = $(this).attr("data-id");
    $("#txtReason").val('');
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

function DeleteRecord(AccountServiceID) {
    $.ajax({
        type: "GET",
        url: ROOTPath + '/Accounts/AccountService/Delete',
        data: { id: AccountServiceID, Reason: $("#DeleteConfirmModal #txtReason").val(), isWindowReload: true },
        success: function (data) {
            if (data.status == true) {
                window.location.reload();
            }
            else
                showAlert('error', data.message);
        },
        error: function () { }
    });

}

//Search List
function SearchItems(type) {
    if (type == 'accountservice') {
        $('#tblAccountService').DataTable().search($('#txtAccountServiceSearch').val()).draw();
    }
    if (type == 'paymentplan') {
        $('#tblAccountPaymentPlan').DataTable().search($('#txtPaymentPlanSearch').val()).draw();
    }
}

//Search When Key Press
function SearchItemsOnKeyPress(type, event) {
    if (event.keyCode == 13)  // the enter key code
    {
        if (type == 'accountservice')
            $('#btnAccountServiceSearch').click();
        if (type == 'paymentplan')
            $('#btnPaymentPlanSearch').click();
        return false;
    }
}
//

//Refresh List
function RefreshItems(type, searchTextID) {
    $("#" + searchTextID).focus();
    $("#" + searchTextID).val('');
    if (type == 'accountservice') {
        InitializeAccountServiceDataTable();
    }
    if (type == 'paymentplan') {
        InitializeAccountPaymentPlanDataTable();
    }
}
//

//Edit Right Link 
function EditRight(id, isCopy) {
    $.ajax({
        url: ROOTPath + "/AccountProperty/GetAccountProperty",
        data: {
            "id": id
        },
        success: function (data) {
            if (data.IsActive == false) {
                showAlert("error", editInactiveRightValidationMsg);
            }
            else {

                if (isCopy) {
                    window.open(ROOTPath + "/AccountProperty/Copy?ID=" + id, '_blank');
                }
                else {
                    window.open(ROOTPath + "/AccountProperty/Edit?ID=" + id, '_blank');
                }
            }

        }, error: function () {
        }
    }).always(function () {
    });
}
//
function ViewHistory(id) {    
    $.ajax({
        type: "GET",
        async: false,
        url: ROOTPath + "/AccountProperty/GetAccountPropertyRightHistory",
        data: { id: id },
        success: function (data) {
            if (data.status == false) {
                showAlert('error', data.message);
                e.stopPropagation();
            }
            else {
                $("#divAccountPropertyRightHistory").html(data.returnData);                
                $("#AccountPropertyRightHistory").modal('show');
            }
        }, error: function () {
        }
    }).always(function () {
    });
}