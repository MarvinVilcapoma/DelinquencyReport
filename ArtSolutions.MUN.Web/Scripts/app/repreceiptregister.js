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
        InitializeDataTable("tblReceiptRegister");
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

    if (filterCriteria == 'balance') {
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
    InitializeDataTable("tblReceiptRegister");
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
    var selectedAccountTexts = "";
    var selectedCollectorTexts = "";
    var isvalid = false;
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
        InitializeDataTable("tblReceiptRegister");
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
    var indexGroup;
    indexGroup = null;
    var groupColumn = 0;
    $('#' + tableName).dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg
        },
        "serverSide": true,
        "processing": true,
        "bFilter": false,
        "bInfo": false,
        "bLengthChange": false,
        "autoWidth": false,
        "ordering": false,
        "paging": false,
        "scrollY": "100vh",
        "scrollCollapse": true,
        "ajax": {
            "url": ROOTPath + "/Reports/Report/ReceiptRegister",
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
        "columnDefs": [
            { "visible": true, "targets": groupColumn }
        ],
        "columns": [
            {
                name: "Number", title: recieptNumber, "data": "Number", className: "col-lg-1"
            },
            {
                name: "Date", title: date, "data": "FormattedDate", className: "col-lg-1 text-center"
            },
            {
                name: "AccountDisplayName", title: name, "data": "AccountDisplayName", className: "col-lg-2 text-description-field"
            },
            {
                name: "PaymentType", title: paymentGroup, "data": "PaymentType", className: "col-lg-1 text-description-field"
            },
            {
                name: "StatusBatchNumber", title: statuBatchNumber, "data": "FormattedStatusBatchNumber", className: "col-lg-1 text-description-field"
            },
            {
                name: "Bank", title: bankAccount, "data": "Bank", className: "col-lg-2 text-description-field"
            },
            {
                name: "Collector", title: collector, "data": "FormattedCashier", className: "col-lg-2 text-description-field"
            },
            {
                name: "TotalAmountAuxiliaryReceipt", title: totalAmountAuxiliaryReceipt, "data": "FormattedTotalAmountAuxiliaryReceipt", className: "col-lg-1 text-right"
            },
            {
                name: "TotalAmountOfficialReceipt", title: totalAmountOfficialTicket, "data": "FormattedTotalAmountOfficialReceipt", className: "col-lg-1 text-right"
            }
        ],
        "lengthMenu": [[15, 25, 50, 100], [15, 25, 50, 100]],
        "order": [[1, "asc"]],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $(nRow).find("td").each(function (index) {
                this.setAttribute('title', $(this).text().trim());
            });
        },
        "drawCallback": function (settings) {
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var last = null;
            var aData = new Array();
            var index = 0;

            api.column(groupColumn, { page: 'current' }).data().each(function (group, i) {
                var vals = api.row(api.row($(rows).eq(i)).index()).data();
                var amount = vals["TotalAmountOfficialReceipt"];

                if (typeof aData[group] == 'undefined') {
                    aData[group] = new Array();
                    aData[group].rows = [];
                    aData[group].TotalAmount = [];
                }

                aData[group].rows.push(i);
                aData[group].TotalAmount.push(amount);
            });

            var idx = 0;
            for (var item in aData) {
                idx = Math.max.apply(Math, aData[item].rows);
                var sum = 0;
                $.each(aData[item].TotalAmount, function (k, v) {
                    sum = sum + v;
                });
                $(rows).eq(idx).after(
                    '<tr class="group"><td class="text-right body-content" colspan="7">' + TotalValue + ':</td>' +
                    '<td class="text-center body-content">' + CurrencyGlobalFormatWithText(sum) + '</td><td></td></tr>'
                );
            }
        },
        "footerCallback": function (row, data, start, end, display) {
            var totalAmountAuxiliary = 0;
            var totalAmountOfficial = 0;

            for (var i = 0; i < display.length; i++) {
                totalAmountAuxiliary = totalAmountAuxiliary + data[i].TotalAmountAuxiliaryReceipt;
                totalAmountOfficial = totalAmountOfficial + data[i].TotalAmountOfficialReceipt;
            }
            $(row).find("th").eq("1").text(NumberToCultureFormat(totalAmountAuxiliary));
            $(row).find("th").eq("2").text(NumberToCultureFormat(totalAmountOfficial));
            $("#tblFooter").removeClass("hide");
        }
    });
}

function Print() {
    if (validateForm()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintReceiptRegister",
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