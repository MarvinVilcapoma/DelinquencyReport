$(document).ready(function () {
    
    initializeDatePicker();
    GetAccountForSelect('AccountId', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px');
});

$(document).on('click', '#btnGo', function () {
    if (checkAccountIdInput() && $("#form").valid()) {
        getAccountInfo();
    }
    return false;
});
function initializeDatePicker() {
    $('.periodDate').datepicker({
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture,
    });
    var dt = new Date();
    $('#FutureDate').datepicker("setDate", new Date(dt.getFullYear(), dt.getMonth(), dt.getDate()));
}
function getAccountInfo() {
    var retval = true;
    $.ajax({
        type: "POST",
        async: false,
        url: ROOTPath + "/Reports/Report/SalesTaxFillingDetailGet",
        data: { 'accountId': $("#AccountId").val(), 'futureDate': new Date($("#FutureDate").datepicker('getDate')).toDateString()},
        success: function (response) {
            if (response.status) {
                $("#dvUpdate").html(response.data);
                $("#advanceSearchModal").modal('hide');
                showLoading();
                InitializeDataTable("tblIVUFilledCertificate");
            }
            else {
                retval = false;
                $("#dvUpdate").html('');
                showAlert('error', response.data);
            }
        },
        error: function (error) {
            retval = false;
        }
    });
    if (retval) {
        showLoading();
    }
}
function InitializeDataTable(tableName) {    
    $('#' + tableName).dataTable({
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
        "scrollY": "100vh",
        "scrollCollapse": true,
        "ajax": {
            "url": ROOTPath + "/Reports/Report/SalesTaxFillingCertificate",
            "type": "POST",
            "data": function (data) {
                data.futureDate = new Date($("#FutureDate").datepicker('getDate')).toDateString();
                data.accountId = $("#AccountId").val();
            }
        },
        "initComplete": function (json) {
            var companyModelData = json.json.companyModel;
            $('#SalesTaxFilingLine1Desc').each(function () {
                var text = $(this).html();
                $(this).html(text.replace('#CompanyName#', companyModelData.Name));
            });
            var stext = $('#SalesTaxFilingLine2Desc').text();
            stext = stext.replace('#CompanyState#', companyModelData.CountyName == null ? "" : companyModelData.CountyName);
            stext = stext.replace('#CompanyCity#', companyModelData.Name == null ? "" : companyModelData.Name);
            $('#SalesTaxFilingLine2Desc').text(stext);
        },
        destroy: true,
        "columns": [
            {
                name: "Period", title: Period, "data": "FormattedIVUPeriod", className: "col-lg-1 text-center"
            },
            {
                name: "IVU", title: IVU, "data": "FormattedPrincipal", className: "col-lg-1 text-right"
            }
            ,
            {
                name: "Penalties", title: Penalty, "data": "FormattedPenalties", className: "col-lg-2 text-right"
            }
            ,
            {
                name: "Charges", title: Surcharges, "data": "FormattedCharges", className: "col-lg-2 text-right"
            }
            ,
            {
                name: "Interest", title: Interests, "data": "FormattedInterest", className: "col-lg-2 text-right"
            }
            ,
            {
                name: "Payment", title: Payment, "data": "FormattedPaidAmount", className: "col-lg-2 text-right"
            }
            ,
            {
                name: "Balance", title: Balance, "data": "FormattedBalance", className: "col-lg-2 text-right"
            }
        ],
        "lengthMenu": [[15, 25, 50, 100], [15, 25, 50, 100]],
        "order": [[1, "asc"]]
        , "footerCallback": function (row, data, start, end, display) {
            var totalBalance = 0;
            for (var i = 0; i < display.length; i++) {
                totalBalance = totalBalance + data[i].Balance;
            }

            $(row).find("th").eq("1").text(NumberToCultureFormat(parseFloat(totalBalance)));
            $("#tblFooter").removeClass("hide");
            hideLoading();
        }
    });
}
function Print(data) {
    if ($("#form").valid()) {
        $.ajax({
            type: "GET",
            async: false,
            url: ROOTPath + "/Reports/Report/PrintSalesTaxFilingCertification",
            data: { 'accountId': $("#AccountId").val(), 'futureDate': new Date($("#FutureDate").datepicker('getDate')).toDateString() },
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

function checkAccountIdInput() {
    var isvalid = true;
    $("#FutureDate").removeClass("error");
    if ($("#FutureDate").val() == undefined || $("#FutureDate").val() == '') {        
        $("#FutureDate").addClass("error");
        isvalid = false;
    }
    if ($("#AccountId").val() == '') {        
        $("#AccountId").focus();
        //isvalid
    }
    return isvalid;
}
