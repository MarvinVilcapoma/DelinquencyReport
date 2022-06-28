$(document).ready(function () {
    $("#txtSearch").focus();
    $(".select2dropdown").select2();
    if ($("#ddlSearchTypeList").val() == "1")
        $("#txtSearch").val($("#top-search").val());
    InitializeDataTable();
});

$(document).on('click', '#btnSearch', function () {
    $('#tblAccount').DataTable().columns(1).search($('#ddlAccountType').val().trim());
    $('#tblAccount').DataTable().search($('#txtSearch').val()).draw();
});

$("#btnrefresh").click(function () {
    $('#ddlAccountType').val(0).trigger('change');
    $('#ddlStatus').val(1).trigger('change');
    $("#txtSearch").focus();
    $('#txtSearch').val('');
    InitializeDataTable();
});

$(document).on('change', '#ddlAccountType', function () {
    InitializeDataTable();
});

$(document).on('change', '#ddlStatus', function () {
    InitializeDataTable();
});

function InitializeDataTable() {
    $('#tblAccount').dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg,
            oPaginate: {
                sPrevious: previous,
                sNext: next
            }
        },
        "fnDrawCallback": function (oSettings) {
            $('#tblAccount tbody tr td').each(function () {
                //CO-1224 
                if ($(this).index() >= 3 && $(this).index() <= 5)
                    this.setAttribute('title', $(this).text());

            });

            /* Apply the tooltips */
            $('#tblAccount tbody tr td').tooltip({
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
            "url": ROOTPath + '/Account/Get',
            "type": "POST",
            "data": function (data) {
                data.filterText = $("#txtSearch").val();
                data.accountTypeID = $("#ddlAccountType").val();
                data.status = $("#ddlStatus").val() == "0" ? null : ($("#ddlStatus").val() == 1 ? true : false);
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
                name: "AccountTypeName", title: accountType, "data": "AccountTypeName", className: "col-lg-1 table-description-field",
            },
            {
                name: "RegisterNumber", title: registerNumber, "data": "RegisterNumber", className: "col-lg-1 table-description-field",
            },
            {
                name: "DisplayName", title: displayName, "data": "DisplayName", className: "col-lg-3 table-description-field",
            },
            {
                name: "Address", title: address, "data": "Address", className: "col-lg-2 table-description-field",
            },
            {
                name: "PhoneNumber", title: phoneNumber, "data": "PhoneNumber", className: "col-lg-1 table-description-field",
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
                    str = '<a id="btnView_' + row.ID + '" href=' + ROOTPath + "/Account/View?accountID=" + row.ID + "&accountType=" + row.AccountTypeID + ' class="btn btn-white btn-sm"><i class="fa fa-folder"></i> ' + view + ' </a>';
                    str = str + '<a id="btnEdit_' + row.ID + '" href=' + ROOTPath + "/Account/Edit?accountID=" + row.ID + "&accountType=" + row.AccountTypeID + ' class="btn btn-white btn-sm m-l-xs ' + ((row.IsActive == true) ? "" : "hide") + '"><i class="fa fa-pencil"></i> ' + edit + ' </a>';
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
        InActiveAccount(val, action);
    });
}

function InActiveAccount(Id, isActive) {
    $.ajax({
        type: "GET",
        url: ROOTPath + '/Account/ActiveDeactive',
        data: { isActive: isActive, iD: Id },
        success: function (data) {
            if (data.status == true) {
                if (isActive == true) {
                    msg = activemsg;
                    $("#Edit_" + Id).css("display", "none");
                    $("#active_" + Id).css("display", "block");

                    $("#btnEdit_" + Id).removeClass("hide");
                }
                else {
                    msg = deactivemsg;
                    $("#active_" + Id).css("display", "none");
                    $("#Edit_" + Id).css("display", "block");

                    $("#btnEdit_" + Id).addClass("hide");
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