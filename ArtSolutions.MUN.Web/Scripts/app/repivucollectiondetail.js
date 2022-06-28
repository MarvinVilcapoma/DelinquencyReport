$(document).ready(function () {
    

    initializeDatePicker();
});

function initializeDatePicker() {
    $('.dateinput').datepicker({
        defaultDate: new Date(),
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture
    });
    $('.dateinput').datepicker('update', new Date());
}
$(document).on('click', '#btnGo', function () {
    if (validateFormInput()) {
        loadCollectionDetail();
        return true;
    }
    return false;
});

function validateFormInput() {
    var isvalid = false;
    if ($("#txtStartPeriodDate").val() == '')
        showAlert('error', $("#txtStartPeriodDate").attr("data-required-msg"));
    else if ($("#txtEndPeriodDate").val() == '')
        showAlert('error', $("#txtEndPeriodDate").attr("data-required-msg"));
    else if (new Date($("#txtEndPeriodDate").val()) < new Date($("#txtStartPeriodDate").val()))
        showAlert('error', $("#txtEndPeriodDate").attr("data-compare-val-msg"));
    else
        isvalid = true;

    return isvalid;
}
function loadCollectionDetail() {
    $('#tblIVUCollectionDetail').dataTable({
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
        "ordering": false,
        "paging": false,
        "conditionalPaging": true,
        "scrollY": "100vh",
        "scrollCollapse": true,
        "ajax": {
            "url": ROOTPath + "/Reports/Report/IVUCollectionDetail",
            "type": "POST",
            "data": function (data) {
                data.startPeriodDate = new Date($("#txtStartPeriodDate").datepicker('getDate')).toDateString();
                data.endPeriodDate = new Date($("#txtEndPeriodDate").datepicker('getDate')).toDateString();
            }            
        },
        destroy: true,
        "columns": [
            {
                name: "AccountNumber", title: AccountNumber, "data": "AccountId", className: "text-left", "width": "7%"
            },
            {
                name: "Name", title: Name, "data": "AccountName", className: "text-left", "width": "16%"
            },
            {
                name: "Period", title: Period, "data": "FormattedPeriod", className: "text-center", "width": "7%"
            },
            {
                name: "PaymentReceipt", title: PaymentReceipt, "data": "FormattedReceiptNumber", className: "text-center", "width": "7%"
            },
            {
                name: "PaymentDate", title: PaymentDate, "data": "FormattedPaymentDate", className: "text-center", "width": "7%"
            },
            {
                name: "TotalUse", title: TotalUse, "data": "FormattedTotalUse", className: "text-right", "width": "7%"
            },
            {
                name: "TotalTaxable", title: TotalTaxable, "data": "FormattedTotalTaxable", className: "text-right", "width": "7%"
            },
            {
                name: "TotalSubjectToIVU", title: TotalSubjectToIVU, "data": "FormattedTotalSubjectToIVU", className: "text-right", "width": "7%"
            },
            {
                name: "TotalMunicipalTax", title: TotalMunicipalTax, "data": "FormattedMunicipalTax", className: "text-right", "width": "7%"
            }
            , {
                name: "TotalIVUTaxToPay", title: TotalIVUTaxToPay, "data": "FormattedPayableTax", className: "text-right", "width": "7%"
            }
            , {
                name: "TotalPaid", title: TotalPaid, "data": "FormattedTotalPaid", className: "text-right", "width": "7%"
            }
            , {
                name: "Balance", title: Balance, "data": "FormattedBalance", className: "text-right", "width": "7%"
            }
            , {
                name: "User", title: User, "data": "FormattedUser", "width": "7%"
            }
        ],
        "lengthMenu": [[15, 25, 50, 100], [15, 25, 50, 100]],
        "order": [[1, "asc"]]
        , "footerCallback": function (row, data, start, end, display) {
            var totalBalance = 0;
            for (var i = 0; i < display.length; i++) {
                totalBalance = totalBalance + data[i].Balance;
            }

            $(row).find("th").eq("0").find("#spstartdate").text(new Date($("#txtStartPeriodDate").datepicker('getDate')).toDateString());
            $(row).find("th").eq("1").text(NumberToCultureFormat(totalBalance));
            $("#tblFooter").removeClass("hide");
        },
        rowGroup: {
            startRender: null,
            endRender: function (rows, group) {
                return $('<tr class="sub-header font-bold">')
                    .append('<td colspan="5" class="p-l-sm-td text-right">' + FillingOn + "  " + rows.data().pluck("FormattedFillingDate")[0] + ':</td>')
                    .append('<td class="text-right">' + rows.data().count() + '</td>')
                    .append('<td  colspan="7"></td>')
                    .append('</tr>');
            },
            dataSrc: 'FormattedFillingDate'
        }
    });
}

function Print() {
    if (validateFormInput()) {
        $.ajax({
            type: "GET",
            async: false,
            url: ROOTPath + "/Reports/Report/PrintIVUCollectionDetail",
            data: { 'startPeriodDate': new Date($("#txtStartPeriodDate").datepicker('getDate')).toDateString(), 'endPeriodDate': new Date($("#txtEndPeriodDate").datepicker('getDate')).toDateString() },
            success: function (response) {
                if (response.status) {
                    hideLoading();
                    var printWindow = window.open('', '_blank');
                    printWindow.document.write(response.data);
                    printWindow.document.close();
                    setTimeout(function () { printWindow.print(); }, 20);
                    printWindow.onfocus = function () { setTimeout(function () { printWindow.close(); }, 20); };
                }
                else {
                    showAlert('error', response.data);
                    return false;
                }
            }
        });
    }
    else
        return false;
    return true;
}

