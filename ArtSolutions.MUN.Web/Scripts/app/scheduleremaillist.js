$(document).ready(function () {
    $("#txtSearch").focus();    
    InitializeDataTable();
});

$(document).on('click', '#btnSearch', function () {
    $('#tblAccount').DataTable().search($('#txtSearch').val()).draw();
});

$("#btnrefresh").click(function () {    
    $("#txtSearch").focus();
    $('#txtSearch').val('');
    InitializeDataTable();
});

function InitializeDataTable() {
    $('#tblSchedulerEmail').dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg,
            oPaginate: {
                sPrevious: previous,
                sNext: next
            }
        },
        "fnDrawCallback": function (oSettings) {
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
            "url": ROOTPath + '/SchedulerEmail/Get',
            "type": "POST",
            "data": function (data) {
                data.filterText = $("#txtSearch").val();                
            }
        },
        destroy: true,
        "columns": [
            {
                name: "IsActive",
                "bSortable": false,
                className: "col-lg-2",
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
                name: "Email", title: Email, "data": "Email", className: "col-lg-8 table-description-field",
            },
            {
                name: "Actions",
                title: actions,
                className: "col-lg-2 text-right",
                "bSortable": false,
                "data": function (row) {
                    var str = "";
                    str = str + '<a id="btnEdit_' + row.ID + '" href=' + ROOTPath + "/SchedulerEmail/Edit?schedulerEmailID=" + row.ID + ' class="btn btn-white btn-sm m-l-xs ' + ((row.IsActive == true) ? "" : "hide") + '"><i class="fa fa-pencil"></i> ' + edit + ' </a>';
                    str = str + '<a href="javascript:void(0)" data-id="' + row.ID + '" class="btn btn-white btn-sm btnDelete m-l-xs"><i class="fa fa-trash"></i> ' + Delete + ' </a>';
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
        url: ROOTPath + '/SchedulerEmail/ActiveDeactive',
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
$("#tblSchedulerEmail").on("click", ".btnDelete", function () {
    var confMessage = "";
    var textmessage = "";
    var SchedulerEmailID = $(this).attr("data-id");
    $("#txtReason").val('');
    $("#DeleteConfirmModal").attr("data-scheduleremailid", SchedulerEmailID);
    $("#DeleteConfirmModal").modal("show");

});
$("#DeleteConfirmModal").on("click", "#btnModalDelete", function () {
    var confMessage = "";
    var textmessage = "";
    var SchedulerEmailID = $("#DeleteConfirmModal").attr("data-scheduleremailid");
    if ($("#frmDeleteSchedulerEmail").valid()) {
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
            DeleteRecord(SchedulerEmailID);
        });
    }
});

function DeleteRecord(SchedulerEmailID) {
    $.ajax({
        type: "GET",
        url: ROOTPath + '/SchedulerEmail/Delete',
        data: { id: SchedulerEmailID, Reason: $("#DeleteConfirmModal #txtReason").val(), isWindowReload: true },
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