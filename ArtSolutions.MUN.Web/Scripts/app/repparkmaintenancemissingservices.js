$(document).ready(function () {
    $('#dvUpdate').hide();
    $("#Year").focus();
});

$(document).on('click', '#btnGo', function () {
    if (validateForm()) {
        InitializeDataTable("tblParkMaintenanceMissingServices");
        return true;
    }
    return false;
});

$(window).resize(function () {
    if (validateForm()) {
        InitializeDataTable("tblParkMaintenanceMissingServices");
        return true;
    }
    return false;
});

function validateForm() {
    var isvalid = true;
    $("#Year").removeClass("error");
    if ($("#Year").val() == undefined || $("#Year").val() == '') {
        $("#Year").addClass("error");
        isvalid = false;
    }
    return isvalid;
}

function InitializeDataTable(tableName) {
    $('#dvUpdate').show();
    $('#' + tableName).dataTable({
        "language": {
            "emptyTable": nodatamsg,
            "zeroRecords": nodatamsg,
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
        "ordering": false,
        "paging": false,
        "conditionalPaging": true,
        "scrollY": "100vh",
        "scrollX": true,
        "scrollCollapse": true,
        "ajax": {
            "url": ROOTPath + "/Reports/Report/ParkMaintenanceMissingServices",
            "type": "POST",
            "data": function (data) {
                data.year = $("#Year").val();
            }
        },
        destroy: true,
        "columns": [
            {
                name: "TaxNumber", title: taxNumber, "data": "TaxNumber", className: "col-lg-2 text-center"
            },
            {
                name: "DisplayName", title: name, "data": "DisplayName", className: "col-lg-2 text-center"
            },
            {
                name: "RightNumber", title: rightNumber, "data": "RightNumber", className: "col-lg-2 text-center"
            },
            {
                name: "FiscalYearID", title: year, "data": "FiscalYearID", className: "col-lg-1 text-center"
            },
            {
                name: "Code", title: code, "data": "Code", className: "col-lg-1 text-center"
            },
            {
                name: "ServiceName", title: serviceName, "data": "ServiceName", className: "col-lg-2 text-center"
            },
            {
                name: "FormattedPropertyTaxPrincipalAmount", title: propertyTaxPrincipalAmount, "data": "FormattedPropertyTaxPrincipalAmount", className: "col-lg-2 text-center"
            }
        ],
        "lengthMenu": [[15, 25, 50, 100], [15, 25, 50, 100]],
        "order": [[1, "asc"]]
    });
}

function Print() {
    if (validateForm()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintParkMaintenanceMissingServices",
            data: {
                'year': $("#Year").val()
            },
            beforeSend: function () {
                showLoading();
            },
            success: function (response) {
                if (response.status) {
                    hideLoading();
                    var printWindow = window.open('', '_blank');
                    printWindow.document.write(response.data);
                    var fun = function () {
                        printWindow.document.close();
                        setTimeout(function () { printWindow.print(); }, 20);
                        printWindow.onfocus = function () { setTimeout(function () { printWindow.close(); }, 20); };
                    };
                    var img = printWindow.document.getElementById('img-responsive');
                    if (img.complete) {
                        fun.call(img);
                    } else {
                        img.onload = fun
                    }
                }
                else {
                    showAlert('error', response.data);
                }
            }
        });
    }
    else
        return false;
    return true;
}