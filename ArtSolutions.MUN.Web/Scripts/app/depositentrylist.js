$(window).on("load", function () {
    $('#txtSearch').focus();
});
$(document).ready(function () {
    InitializeDataTable();
});

$(document).on('click', '#btnSearch', function () {
    $('#tbldepositEntryList').DataTable().search($('#txtSearch').val()).draw();
    $("#txtSearch").focus();
});

$("#btnrefresh").click(function () {
    $("#txtSearch").focus();
    $('#txtSearch').val('');
    InitializeDataTable();
});

function InitializeDataTable(tableName) {
    $('#tbldepositEntryList').dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg,
            oPaginate: {
                sPrevious: previous,
                sNext: next
            }
        },
        "fnDrawCallback": function () {
            $('#tbldepositEntryList tbody tr td').each(function () {
                if ($(this).index() == 2 || $(this).index() == 3)
                    this.setAttribute('title', $(this).text());
            });

            /* Apply the tooltips */
            $('#tbldepositEntryList tbody tr td').tooltip({
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
            "url": ROOTPath + '/DepositEntry/Get',
            "type": "POST",
            "data": function (data) {
                data.filterText = $("#txtSearch").val();
            }
        },
        destroy: true,
        "columns": [
            {
                name: "Date", title: date, "data": "FormattedDate", className: "col-lg-2"
            },
            {
                name: "Number", title: number, "data": "Number", className: "col-lg-1"
            },
            {
                name: "BankName", title: bank, "data": "BankName", className: "col-lg-2 table-description-field"
            },
            {
                name: "Description", title: description, "data": "Description", className: "col-lg-2 table-description-field"
            },
            {
                name: "ClosingCount", title: closing, "data": "ClosingCount", className: "col-lg-1 text-right"
            },
            {
                name: "NetDeposit", title: amount, "data": "FormattedNetDeposit", className: "col-lg-2 text-right"
            },
            {
                name: "Actions",
                title: actions,
                className: "col-lg-2 text-right",
                "bSortable": false,
                "data": function (row) {                   
                    var str = "";
                    if (!row.IsVoid) {
                        str = str + '<a href="#voidConfirmModal" data-target="#voidConfirmModal" data-toggle="modal" class="btn btn-white btn-sm m-l-xs" onclick="SetDepositID(' + row.ID + ')"><i class="fa fa-pencil"></i> ' + voidresource + ' </a>';
                    }
                    str += '<a href=' + ROOTPath + "/Collections/DepositEntry/View?ID=" + row.ID + ' class="btn btn-white btn-sm text-right"><i class="fa fa-folder"></i> ' + view + ' </a>';
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

function SetDepositID(id) {
    $("#ID").val(id)
}

function VoidDepositEntry() {
    $.ajax({
        url: ROOTPath + "/Collections/DepositEntry/Void",
        data: { ID: $("#ID").val(), VoidReason: $("#VoidReason").val() },
        success: function (data) {
            if (data.status) {
                window.location.href = depositlisturl;
            }
            else
                showAlert("error", data.message);
        }
    });
}

$("#voidConfirmModal").on("click", "#btnVoid", function () {
    if ($("#frmVoidDepositEntry").valid()) {
        VoidDepositEntry(); 
    }
});