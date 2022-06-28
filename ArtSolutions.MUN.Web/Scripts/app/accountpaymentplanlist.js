$(window).on("load", function () {
    $("#txtSearch").focus();
    InitializeDataTable();
});

$("#btnrefresh").click(function () {
    $("#txtSearch").focus();
    $('#txtSearch').val('');
    InitializeDataTable();
});

$(document).on('click', '#btnSearch', function () {
    $('#tblAccountPaymentPlan').DataTable().search($('#txtSearch').val()).draw();
});

function InitializeDataTable() {
    $('#tblAccountPaymentPlan').dataTable({
        "oLanguage": {
            "sEmptyTable": listNoDataFound,
            oPaginate: {
                sPrevious: bTNPrevious,
                sNext: bTNNext
            }
        },
        "fnDrawCallback": function () {
            $('#tblAccountPaymentPlan tbody tr td').each(function () {
                if ($(this).index() == 2 || $(this).index() == 3)
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
                        str = '<a href="javascript:;" id="active_' + row.ID + '" onclick="ConfirmActivedeactive(\'Active\',' + row.ID + ')"><span class="label label-primary">' + active + '</span></a>' + '<a href="javascript:;" onclick="ConfirmActivedeactive(\'Deactive\',' + row.ID + ')" style="display:none;" id="Edit_' + row.ID + '"><span class="label label-warning">' + inActivebtn + '</span></a>';
                    if (row.IsActive == false)
                        str = '<a href="javascript:;" id="Edit_' + row.ID + '" onclick="ConfirmActivedeactive(\'Deactive\',' + row.ID + ')"><span class="label label-warning">' + inActivebtn + '</span></a>' + '<a href="javascript:;" onclick="ConfirmActivedeactive(\'Active\',' + row.ID + ')" style="display:none;" id="active_' + row.ID + '"><span class="label label-primary">' + active + '</span></a>';
                    return str;
                }
            },
            {
                name: "Number", title: number, "data": "Number", className: "col-lg-1 table-description-field",
            },
            {
                name: "DisplayName", title: displayName, "data": "DisplayName", className: "col-lg-2 table-description-field",
            },
            {
                name: "PaymentPlanName", title: paymentPlanName, "data": "PaymentPlanName", className: "col-lg-2 table-description-field",
            },
            {
                name: "StartDateByCulture", title: startDate, "data": "StartDateByCulture", className: "col-lg-1",
            },
            {
                name: "EndDateByCulture", title: endDate, "data": "EndDateByCulture", className: "col-lg-1",
            },
            {
                name: "Months", title: months, "data": "Months", className: "col-lg-1 text-right",
            },
            {
                name: "TotalPayment", title: totalAmountOfPaymentPlan, "data": "TotalPaymentByCulture", className: "col-lg-1 text-right",
            },
            {
                name: "Actions",
                title: actions,
                className: "col-lg-2 text-right",
                "bSortable": false,
                "data": function (row) {
                    var str = "";
                    str = '<a href=' + ROOTPath + "/Accounts/AccountPaymentPlan/View?accountPaymentPlanID=" + row.ID + '  class="btn btn-white btn-sm"><i class="fa fa-folder"></i> ' + view + ' </a>';
                    str = str + '<a href=' + ROOTPath + "/Accounts/AccountPaymentPlan/Add?accountPaymentPlanID=" + row.ID + '  class="btn btn-white btn-sm m-l-xs"><i class="fa fa-pencil"></i> ' + edit + ' </a>';
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
        confMessage = inActiveConfirm;
        action = false;
    }
    else {
        confMessage = activeConfirm;
        action = true;
    }
    swal({
        title: sureMessage,
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
        url: ROOTPath + '/Accounts/AccountPaymentPlan/ActiveDeactive',
        data: { isActive: isActive, id: Id },
        success: function (data) {
            if (data.status == true) {
                if (isActive == true) {
                    msg = activatedMessage;
                    $("#active_" + Id).css("display", "block");
                    $("#Edit_" + Id).css("display", "none");
                }
                else {
                    msg = deactivatedMessage;
                    $("#active_" + Id).css("display", "none");
                    $("#Edit_" + Id).css("display", "block");
                }
                $('#tblAccountPaymentPlan').DataTable().search($('#txtSearch').val()).draw();
                showAlert('success', msg);
                $('#txtSearch').focus();
            }
            else
                showAlert('error', data.message);
        }, error: function () { }
    }).always(function () {
    });
}
