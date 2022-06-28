$(document).ready(function () {
    $("#txtSearch").focus();
    $(".select2dropdown").select2();
    if ($("#ddlSearchTypeList").val() == "3")
        $("#txtSearch").val($("#top-search").val());
    InitializeDataTable();
});

$(document).on('click', '#btnSearch', function () {
    $('#tblAccountProperty').DataTable().search($('#txtSearch').val()).draw();
});

$("#btnrefresh").click(function () {
    $("#txtSearch").focus();
    $('#txtSearch').val('');
    InitializeDataTable();
});

function InitializeDataTable(tableName) {
    $('#tblAccountProperty').dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg,
            oPaginate: {
                sPrevious: previous,
                sNext: next
            }
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
            "url": ROOTPath + '/AccountProperty/Get',
            "type": "POST",
            "data": function (data) {
                data.filterText = $("#txtSearch").val();
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
                        str = '<a href="javascript:;" id="active_' + row.ID + '" onclick="ConfirmActivedeactive(\'Active\',' + row.ID + ',' + row.OwnerID + ')"><span class="label label-primary">' + active + '</span></a>' + '<a href="javascript:;" onclick="ConfirmActivedeactive(\'Deactive\',' + row.ID + ',' + row.OwnerID + ')" style="display:none;" id="Edit_' + row.ID + '"><span class="label label-warning">' + inactivebtn + '</span></a>';
                    if (row.IsActive == false)
                        str = '<a href="javascript:;" id="Edit_' + row.ID + '" onclick="ConfirmActivedeactive(\'Deactive\',' + row.ID + ',' + row.OwnerID + ')"><span class="label label-warning">' + inactivebtn + '</span></a>' + '<a href="javascript:;" onclick="ConfirmActivedeactive(\'Active\',' + ',' + row.OwnerID + ')" style="display:none;" id="active_' + row.ID + '"><span class="label label-primary">' + active + '</span></a>';
                    return str;
                }
            },
            {
                name: "FincaID", title: propertyNumber, "data": "FincaID", className: "col-lg-2  table-description-field"
            },
            {
                name: "Owner", title: owner, "data": "Owner", className: "col-lg-2 table-description-field"
            },
            {
                name: "CadastralPlanNumber", title: cadastralPlanNumber, "data": "CadastralPlanNumber", className: "col-lg-1 table-description-field"
            },
            {
                name: "Area", title: totalArea, "data": "FormattedArea", className: "col-lg-2 text-right"
            },
            {
                name: "ToTalValue", title: toTalValue, "data": "FormattedTotalValue", className: "col-lg-1 text-right"
            },
            {
                name: "Actions",
                title: actions,
                className: "col-lg-3 text-right",
                "bSortable": false,
                "data": function (row) {
                    var str = "";
                    str = '<a id="btnView_' + row.ID + '" href=' + ROOTPath + "/AccountProperty/View?ID=" + row.ID + ' class="btn btn-white btn-sm"><i class="fa fa-folder"></i> ' + view + ' </a>';
                    str = str + '<a id="btnEdit_' + row.ID + '" onclick=' + "EditRight(" + row.ID + ")" + ' class="btn btn-white btn-sm m-l-xs ' + ((row.IsActive == true) ? "" : "hide") + '"><i class="fa fa-pencil"></i> ' + edit + ' </a>';
                    return str;
                }
            }
        ],
    });
}

function ConfirmActivedeactive(str, val, ownerID) {
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
    if (str.trim().toLowerCase() == "active") {
        $.ajax({
            type: "GET",
            url: ROOTPath + '/Accounts/AccountProperty/AccountPropertyHasDebt',
            data: { id: val, accountID: ownerID },
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
                            InActiveAccount(val, action);
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
            InActiveAccount(val, action);
        });
    }

}

function InActiveAccount(ID, isActive) {
    $.ajax({
        type: "GET",
        url: ROOTPath + '/AccountProperty/ActiveDeactive',
        data: { isActive: isActive, id: ID },
        success: function (data) {
            if (data.status == true) {
                if (isActive == true) {
                    msg = activemsg;
                    $("#Edit_" + ID).css("display", "none");
                    $("#active_" + ID).css("display", "block");

                    $("#btnEdit_" + ID).removeClass("hide");
                }
                else {
                    msg = deactivemsg;
                    $("#active_" + ID).css("display", "none");
                    $("#Edit_" + ID).css("display", "block");

                    $("#btnEdit_" + ID).addClass("hide");
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

function EditRight(id) {
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
                window.open(ROOTPath + "/AccountProperty/Edit?ID=" + id, '_blank');
            }

        }, error: function () {
        }
    }).always(function () {
    });
}

