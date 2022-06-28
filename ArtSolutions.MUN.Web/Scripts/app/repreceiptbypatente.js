$(document).ready(function () {
    
    $('#dvUpdate').hide();
    $(".select2dropdown").not('#AccountIDs').select2({ width: '300px' });
    GetAccountForSelect('AccountIDs', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px', true);
    initializeDatePicker();
    $("#btnCancel").on("click", function () {
        SetResetFilterOption();

    });
});
var hdnAccount;
var hdnCollector;
var hdnBalanceFrom;
var hdnBalanceTo;
function SetResetFilterOption() {
    var PreviousSelectedData = [];
    if (hdnAccount != undefined && hdnAccount.length > 0) {
        PreviousSelectedData = hdnAccount.split(",");        
        $('#AccountIDs').val(PreviousSelectedData).trigger('change');
    }
    if (hdnCollector != undefined && hdnCollector.length > 0) {
        PreviousSelectedData = hdnCollector.split(",");        
        $('#CashierIDs').val(PreviousSelectedData).trigger('change');
    }
    if (hdnBalanceFrom != undefined && hdnBalanceFrom.length > 0) {
        $("#txtBalanceFrom").val(hdnBalanceFrom);
        $("#txtBalanceTo").val(hdnBalanceTo);
    }
}
function initializeDatePicker() {
    $('.periodDate').datepicker({
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture
    });
    var dt = new Date();
    $('#StartDate').datepicker("setDate", new Date(dt.getFullYear(), dt.getMonth(), dt.getDate() - 1));
    $('#EndDate').datepicker('setDate', new Date(dt.getFullYear(), dt.getMonth(), dt.getDate()));
}

$(document).on('click', '#btnGo', function () {
    if (validateForm()) {
        InitializeDataTable("tblBusinessLicenseReceipt");
        return true;
    }
    return false;
});

$(document).on('change', '#AccountIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#AccountIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

$(document).on('change', '#CashierIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#CashierIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

function clearSearch(filterCriteria) {
    if (filterCriteria =='balance') {
        $("#txtBalanceFrom").val('');
        $("#txtBalanceTo").val('');
        $("#spnBalanceRange").addClass('hide');
        hdnBalanceFrom = "";
        hdnBalanceTo = "";
    }
    if (filterCriteria == 'accounts') {
        $("#AccountIDs").val(null).trigger('change');
        $("#spnSelectedAccount").addClass('hide');
        hdnAccount = "";
    }
    if (filterCriteria == 'collector') {
        $("#CashierIDs").val(null).trigger('change');
        $("#spnSelectedCollector").addClass('hide');
        hdnCollector = "";
    }
    InitializeDataTable("tblBusinessLicenseReceipt");   
}

function validateForm() {
    var isvalid = true;
    $("#StartDate,#EndDate").removeClass("error");
    if ($("#StartDate").val() == undefined || $("#StartDate").val() == '') {
        showAlert('error', $("#StartDate").attr("data-required-msg"));
        $("#StartDate").addClass("error");
        isvalid = false;
    }
    if ($("#EndDate").val() == undefined || $("#EndDate").val() == '') {
        showAlert('error', $("#EndDate").attr("data-required-msg"));
        $("#EndDate").addClass("error");
        isvalid = false;
    }
    if (new Date($("#EndDate").datepicker('getDate')) < new Date($("#StartDate").datepicker('getDate'))) {
        showAlert('error', $("#EndDate").attr("data-compare-val-msg"));
        isvalid = false;
    }
    if (isvalid) {
        hdnAccount = getAccountIDs();
        hdnCollector = getCashierIDs();
        hdnBalanceFrom = $("#txtBalanceFrom").val();
        hdnBalanceTo = $("#txtBalanceTo").val();
        $("#FilterAccountID").val(hdnAccount);
        $("#FilterCashierID").val(hdnCollector);
        $("#BalanceFrom").val(hdnBalanceFrom);
        $("#BalanceTo").val(hdnBalanceTo);
    }
    return isvalid;
}

function validateAdvanceSearch() {
    var isvalid = false;
    var selectedAccountTexts = "";
    var selectedCollectorTexts = "";
    if ($("#txtBalanceTo").val() != "" && $("#txtBalanceFrom").val() == "")
        showAlert('error', balanceFromRequiredMsg);
    else if ($("#txtBalanceFrom").val() != "" && $("#txtBalanceTo").val() == "")
        showAlert('error', balanceToRequiredMsg);
    else if (
        ($("#txtBalanceTo").val() != "" && $("#txtBalanceFrom").val() != "")
        &&
        (parseInt($("#txtBalanceFrom").val()) > parseInt($("#txtBalanceTo").val()))
    )
        showAlert('error', compareBalanceValidationMsg);
    else {
        isvalid = true;
        InitializeDataTable("tblBusinessLicenseReceipt");
        $("#advanceSearchModal").modal('hide');

        if ($("#txtBalanceFrom").val() != '' && $("#txtBalanceTo").val() != '') {
            $("#dvsearchinfo").removeClass('hide');
            $("#spnBalanceRange").removeClass('hide');
            $("#SearchText").text(BalanceRange + " : " + $("#txtBalanceFrom").val() + " - " + $("#txtBalanceTo").val());
        }
        else {
            $("#spnBalanceRange").addClass('hide');
            $("#SearchText").text('');
        }
        selectedAccountTexts = getAccountText();
        if (selectedAccountTexts != '') {
            $("#dvsearchinfo").removeClass('hide');
            $("#spnSelectedAccount").removeClass('hide');
            $("#AccountIdSearchText").text(SelectedAccount + " : " + selectedAccountTexts);
        }
        else {
            $("#spnSelectedAccount").addClass('hide');
            $("#AccountIdSearchText").text('');
        }
        selectedCollectorTexts = getCashierText();
        if (selectedCollectorTexts != '') {
            $("#dvsearchinfo").removeClass('hide');
            $("#spnSelectedCollector").removeClass('hide');
            $("#CollectorIdSearchText").text(SelectedCollector + " : " + selectedCollectorTexts);
        }
        else {
            $("#spnSelectedCollector").addClass('hide');
            $("#CollectorIdSearchText").text('');
        }
    }
    if (isvalid) {
        hdnAccount = getAccountIDs();
        hdnCollector = getCashierIDs();
        hdnBalanceFrom = $("#txtBalanceFrom").val();
        hdnBalanceTo = $("#txtBalanceTo").val();
    }
    return isvalid;
}

function loadAdvanceSearch() {
    if (validateForm()) {
        $("#txtBalanceFrom").val('');
        $("#txtBalanceTo").val('');
        $("#AccountIDs").val(null).trigger('change');
        $("#CashierIDs").val(null).trigger('change');
        SetResetFilterOption();
        $("#advanceSearchModal").modal('show');
        $("#txtBalanceFrom").focus();
    }
}

function getAccountIDs() {
    var selectAccountList = [];
    if ($("#AccountIDs").select2('data').length) {
        $.each($("#AccountIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectAccountList += "," + item.id;
        });
    }
    var selectedAccountIDs = "";
    if (selectAccountList.length > 0)
        selectedAccountIDs = selectAccountList.substring(1, selectAccountList.length);
    return selectedAccountIDs;
}

function getAccountText() {
    var selectAccountList = [];
    if ($("#AccountIDs").select2('data').length) {
        $.each($("#AccountIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectAccountList += "," + item.text;
        });
    }
    var selectedAccountTexts = "";
    if (selectAccountList.length > 0)
        selectedAccountTexts = selectAccountList.substring(1, selectAccountList.length);
    return selectedAccountTexts;
}

function getCashierIDs() {
    var selectCashierList = [];
    if ($("#CashierIDs").select2('data').length) {
        $.each($("#CashierIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectCashierList += "," + item.id;
        });
    }
    var selectedCashierIDs = "";
    if (selectCashierList.length > 0)
        selectedCashierIDs = selectCashierList.substring(1, selectCashierList.length);
    return selectedCashierIDs;
}
function getCashierText() {
    var selectCashierList = [];
    if ($("#CashierIDs").select2('data').length) {
        $.each($("#CashierIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectCashierList += "," + item.text;
        });
    }
    var selectedCashierTexts = "";
    if (selectCashierList.length > 0)
        selectedCashierTexts = selectCashierList.substring(1, selectCashierList.length);
    return selectedCashierTexts;
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
            "url": ROOTPath + "/Reports/Report/ReceiptByBusinessLicense",
            "type": "POST",
            "data": function (data) {
                data.startDate = new Date($("#StartDate").datepicker('getDate')).toDateString();
                data.endDate = new Date($("#EndDate").datepicker('getDate')).toDateString();
                data.balanceFrom = $("#txtBalanceFrom").val() ? parseFloat($("#txtBalanceFrom").val()) : null;
                data.balanceTo = $("#txtBalanceTo").val() ? parseFloat($("#txtBalanceTo").val()) : null;
                data.accountIDs = getAccountIDs();
                data.cashierIDs = getCashierIDs();
            }
        },
        destroy: true,
        "columns": [
            {
                name: "Date", title: date, "data": "FormattedDate", className: "col-lg-1"
            },
            {
                name: "AccountDisplayName", title: name, "data": "AccountDisplayName", className: "col-lg-2 text-description-field"
            },
            {
                name: "AccountRegisterNumber", title: identitymunicipal, "data": "AccountRegisterNumber", className: "col-lg-1"
            },
            {
                name: "Number", title: recieptNumber, "data": "Number", className: "col-lg-1"
            },
            {
                name: "FiscalYear", title: fiscalyear, "data": "FormattedFiscalYearID", className: "col-lg-1"
            },
            {
                name: "Principal1", title: firstsemester, "data": "FormattedPrincipal1", className: "col-lg-1 text-right"
            },
            {
                name: "Principal2", title: secondsemester, "data": "FormattedPrincipal2", className: "col-lg-1 text-right"
            },
            {
                name: "Penalties", title: penalty, "data": "FormattedPenalties", className: "col-lg-1 text-right"
            },
            {
                name: "Charges", title: surcharges, "data": "FormattedCharges", className: "col-lg-1 text-right"
            },
            {
                name: "Interest", title: interests, "data": "FormattedInterest", className: "col-lg-1 text-right"
            },
            {
                name: "Discount", title: discount, "data": "FormattedDiscount", className: "col-lg-1 text-right"
            },
            {
                name: "PaymentAmount", title: paymentamount, "data": "FormattedPaymentamount", className: "col-lg-2 text-right"
            }

        ],
        "lengthMenu": [[15, 25, 50, 100], [15, 25, 50, 100]],
        "order": [[1, "asc"]],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $(nRow).find("td").each(function (index) {
                this.setAttribute('title', $(this).text().trim());
            });
        },
        "footerCallback": function (row, data, start, end, display) {
            var totalPrincipal1 = 0;
            var totalPrincipal2 = 0;
            var totalPenalty = 0;
            var totalCharge = 0;
            var totalInterest = 0;
            var totalDiscount = 0;
            var totalPayment = 0;

            for (var i = 0; i < display.length; i++) {
                totalPrincipal1 = totalPrincipal1 + data[i].Principal1;
                totalPrincipal2 = totalPrincipal2 + data[i].Principal2;
                totalPenalty = totalPenalty + data[i].Penalties;
                totalCharge = totalCharge + data[i].Charges;
                totalInterest = totalInterest + data[i].Interest;
                totalDiscount = totalDiscount + data[i].Discount;
                totalPayment = totalPayment + data[i].TotalPayment;
            }
            $(row).find("th").eq("1").text(NumberToCultureFormat(totalPrincipal1));
            $(row).find("th").eq("2").text(NumberToCultureFormat(totalPrincipal2));
            $(row).find("th").eq("3").text(NumberToCultureFormat(totalPenalty));
            $(row).find("th").eq("4").text(NumberToCultureFormat(totalCharge));
            $(row).find("th").eq("5").text(NumberToCultureFormat(totalInterest));
            $(row).find("th").eq("6").text(NumberToCultureFormat(totalDiscount));
            $(row).find("th").eq("7").text(NumberToCultureFormat(totalPayment));
            $("#tblFooter").removeClass("hide");
        },
        rowGroup: {
            startRender: null,
            endRender: function (rows, group) {
                var subtotalPrincipal1 = rows.data().pluck("Principal1")
                    .reduce(function (a, b) { return a + b; }, 0);
                var subtotalPrincipal2 = rows.data().pluck("Principal2")
                    .reduce(function (a, b) { return a + b; }, 0);
                var subtotalPenalties = rows.data().pluck("Penalties")
                    .reduce(function (a, b) { return a + b; }, 0);
                var subtotalCharges = rows.data().pluck("Charges")
                    .reduce(function (a, b) { return a + b; }, 0);
                var subtotalInterest = rows.data().pluck("Interest")
                    .reduce(function (a, b) { return a + b; }, 0);
                var subtotalDiscount = rows.data().pluck("Discount")
                    .reduce(function (a, b) { return a + b; }, 0);
                var subtotalPaymentAmount = rows.data().pluck("TotalPayment")
                    .reduce(function (a, b) { return a + b; }, 0);

                return $('<tr class="sub-header font-bold">')
                    .append('<td colspan="5" class="p-l-sm-td text-right">' + totaltxt + " - " + rows.data().pluck("FormattedDate")[0] + '</td>')
                    .append('<td class="text-right">' + NumberToCultureFormat(subtotalPrincipal1) + '</td>')
                    .append('<td class="text-right">' + NumberToCultureFormat(subtotalPrincipal2) + '</td>')
                    .append('<td class="text-right">' + NumberToCultureFormat(subtotalPenalties) + '</td>')
                    .append('<td class="text-right">' + NumberToCultureFormat(subtotalCharges) + '</td>')
                    .append('<td class="text-right">' + NumberToCultureFormat(subtotalInterest) + '</td>')
                    .append('<td class="text-right">' + NumberToCultureFormat(subtotalDiscount) + '</td>')
                    .append('<td class="text-right">' + NumberToCultureFormat(subtotalPaymentAmount) + '</td>')
                    .append('</tr>');                
            },
            dataSrc: 'FormattedDate'
        }
    });
}

function Print() {
    if (validateForm()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintReceiptByBusinessLicense",
            data: {
                'startDate': new Date($("#StartDate").datepicker('getDate')).toDateString()
                , 'endDate': new Date($("#EndDate").datepicker('getDate')).toDateString()
                , 'balanceFrom': $("#txtBalanceFrom").val() ? parseFloat($("#txtBalanceFrom").val()) : null
                , 'balanceTo': $("#txtBalanceTo").val() ? parseFloat($("#txtBalanceTo").val()) : null
                , 'accountIDs': getAccountIDs()
                , 'cashierIDs': getCashierIDs()
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