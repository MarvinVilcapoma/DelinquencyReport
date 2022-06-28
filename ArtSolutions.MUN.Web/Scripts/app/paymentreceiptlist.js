$(window).on("load", function () {
    $('#txtSearch').focus();
});
$(document).ready(function () {
    $(".select2dropdown").select2();
    $('#ddlYear').val((new Date).getFullYear()).trigger('change');
    InitializeDataTable();
});

$(document).on('click', '#btnSearch', function () {
    if ($('#ddlYear').val() == "" && $('#txtSearch').val() == "") {
        showAlert('error', selectAllSearchMsg);
    }
    else {
        $('#tblPaymentList').DataTable().columns(0).search($('#ddlYear').val().trim());
        $('#tblPaymentList').DataTable().search($('#txtSearch').val()).draw();
        $("#txtSearch").focus();
    }
});

$("#btnrefresh").click(function () {
    $('#ddlYear').val((new Date).getFullYear()).trigger('change');
    $("#txtSearch").focus();
    $('#txtSearch').val('');
    InitializeDataTable();
});

//$(document).on('change', '#ddlYear', function () {
//    if ($('#ddlYear').val() == "") {
//        showAlert('error', "Search text is required.");
//    }
//    else {
//        InitializeDataTable();
//    }
//});

function InitializeDataTable(tableName) {
    $('#tblPaymentList').dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg,
            oPaginate: {
                sPrevious: previous,
                sNext: next
            }
        },
        "fnDrawCallback": function () {
            $('#tblPaymentList tbody tr td').each(function () {
                if ($(this).index() >= 1 && $(this).index() <= 3)
                    this.setAttribute('title', $(this).text());
            });

            /* Apply the tooltips */
            $('#tblPaymentList tbody tr td').tooltip({
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
        "order": [[0, "desc"]],
        "conditionalPaging": true,
        "ajax": {
            "url": ROOTPath + '/Payment/Get',
            "type": "POST",
            "data": function (data) {
                data.year = $("#ddlYear").val();
                data.filterText = $("#txtSearch").val();
            }
        },
        destroy: true,
        "columns": [
            {
                name: "Date", title: date, "data": "FormattedDate", className: "col-lg-1 col-md-1 col-sm-1"
            },
            {
                name: "Number", title: number, "data": "Number", className: "col-lg-1 col-md-1 col-sm-1"
            },
            {
                name: "AccountDisplayName", title: accountName, "data": "AccountDisplayName", className: "col-lg-4 col-md-3 col-sm-3 table-description-field"
            },
            {
                name: "ItemName", title: itemname, "data": "ItemName", className: "col-lg-3 col-md-3 col-sm-3 table-description-field"
            },
            {
                name: "IsVoid", title: status, "data": "Status", className: "col-lg-1 col-md-1 col-sm-1"
            },
            {
                name: "NetPayment", title: amount, "data": "FormattedNetPayment", className: "col-lg-1 col-md-2 col-sm-2 text-right"
            },
            {
                name: "Actions",
                title: actions,
                className: "col-lg-1 col-md-1 col-sm-1 text-right",
                "bSortable": false,
                "data": function (row) {
                    var str = "";
                    if (row.PaymentPlanName != null)
                        str = '<a href=' + ROOTPath + "/Collections/Payment/View?ID=" + row.ID + '&Type=PaymentPlan class="btn btn-white btn-sm text-right"><i class="fa fa-folder"></i> ' + view + ' </a>';
                    else
                        str = '<a href=' + ROOTPath + "/Collections/Payment/View?ID=" + row.ID + '&Type=Service&TypeID=' + row.ServiceTypeID + ' class="btn btn-white btn-sm text-right"><i class="fa fa-folder"></i> ' + view + ' </a>';
                    return str;
                }
            }
        ],
        "createdRow": function (row, data, dataIndex) {
            if (data.IsVoid === true) {
                $(row).children("td").not(":last-child").addClass("text-danger");
            }
        }
    });
}