$(window).on("load", function () {
    $('#Name').focus();

    if (ActionTypeID == 4) {
        $("#txtSearch").focus();
        InitializeDataTable();
    }

    if (ActionTypeID == 3) {
        $("#btnCancel").focus();
        $("#frmPaymentPlan select").prop("disabled", true);
        $("#frmPaymentPlan input").prop("disabled", true);
    }
});

$(function () {
    if (ActionTypeID != 4)
        $(".select2dropdown").select2({ width: '100%' });
});

$(document).ready(function () {
    if (ActionTypeID != 4) {
        initializeDate('effectiveDate', __dateFormat, false);

        if (ActionTypeID == 1) {
            $('.effectiveDate').datepicker('update', '');
            $('.paymentPlan').val('');
        }
    }
});

function initializeDate(className, dateFormat) {
    $('.' + className).datepicker({
        defaultDate: new Date(),
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: dateFormat,
        language: __culture
    });
}

function UserSuccessCallback(response) {
    if (response.status == false) {
        showAlert("error", response.message);
    }
    else {
        if (response.actionType == 1) // Save
        {
            window.location = ROOTPath + "/PaymentPlan/List";
        }
        else if (response.actionType == 2)// Save & Add New
        {
            window.location = ROOTPath + "/PaymentPlan/Add";
        }
    }
}

//------------------------------------------ List -------------------------------------------
$("#btnrefresh").click(function () {
    $("#txtSearch").focus();
    $('#txtSearch').val('');
    InitializeDataTable();
});

$(document).on('click', '#btnSearch', function () {
    $('#tblPaymentPlan').DataTable().search($('#txtSearch').val()).draw();
});

function InitializeDataTable() {
    $('#tblPaymentPlan').dataTable({
        "oLanguage": {
            "sEmptyTable": listNoDataFound,
            oPaginate: {
                sPrevious: bTNPrevious,
                sNext: bTNNext
            }
        },
        "fnDrawCallback": function () {
            $('#tblPaymentPlan tbody tr td').each(function () {
                if ($(this).index() == 1)
                    this.setAttribute('title', $(this).text());
            });

            /* Apply the tooltips */
            $('#tblPaymentPlan tbody tr td').tooltip({
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
            "url": ROOTPath + '/Services/PaymentPlan/List',
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
                name: "Name", title: name, "data": "Name", className: "col-lg-2 table-description-field"
            },
            //{
            //    name: "ServiceTypeName", title: serviceType, "data": "ServiceTypeName", className: "col-lg-2 table-description-field"
            //},
            {
                name: "InstalmentPercentageValue", title: downPayment + per, "data": "InstalmentPercentageValue", className: "col-lg-2 text-right"
            },
            {
                name: "InterestPercentageValue", title: monthlyInterest + per, "data": "InterestPercentageValue", className: "col-lg-2 text-right"
            },
            {
                name: "LateInterestPercentageValue", title: latePaymentInterest + per, "data": "LateInterestPercentageValue", className: "col-lg-2 text-right"
            },
            {
                name: "Months", title: noOfMonths, "data": "Months", className: "col-lg-1 text-right"
            },
            {
                name: "Actions",
                title: actions,
                className: "col-lg-2 text-right",
                "bSortable": false,
                "data": function (row) {
                    var str = "";
                    str = '<a href=' + ROOTPath + "/Services/PaymentPlan/Edit?paymentPlanID=" + row.ID + '&actionType=' + 3 + '  class="btn btn-white btn-sm"><i class="fa fa-folder"></i> ' + view + ' </a>';
                    str = str + '<a id="btnEdit_' + row.ID + '" href=' + ROOTPath + "/Services/PaymentPlan/Edit?paymentPlanID=" + row.ID + '&actionType=' + 2 + '  class="btn btn-white btn-sm m-l-xs ' + (row.IsActive == true ? "" : "hide") + '"><i class="fa fa-pencil"></i> ' + edit + ' </a>';
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
        url: ROOTPath + '/Services/PaymentPlan/ActiveDeactive',
        data: { isActive: isActive, iD: Id },
        success: function (data) {
            if (data.status == true) {
                if (isActive == true) {
                    msg = activatedMessage;
                    $("#active_" + Id).css("display", "block");
                    $("#Edit_" + Id).css("display", "none");

                    $("#btnEdit_" + Id).removeClass("hide");
                }
                else {
                    msg = deactivatedMessage;
                    $("#active_" + Id).css("display", "none");
                    $("#Edit_" + Id).css("display", "block");

                    $("#btnEdit_" + Id).addClass("hide");
                }
                $('#tblPaymentPlan').DataTable().search($('#txtSearch').val()).draw();
                showAlert('success', msg);
                $('#txtSearch').focus();
            }
            else
                showAlert('error', data.message);
        }, error: function () { }
    }).always(function () {
    });
}