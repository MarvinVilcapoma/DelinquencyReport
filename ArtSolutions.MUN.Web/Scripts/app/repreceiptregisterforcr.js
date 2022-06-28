$(document).ready(function () {
    $('#dvUpdate').hide();
    $(".select2dropdown").not('#AccountIDs').select2({ width: '300px' });
    GetAccountForSelect('AccountIDs', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px', true);
    initializeDatePicker();
    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });

});

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

var hdnType;
var hdnAccount;
var hdnCollector;
var hdnBalanceFrom;
var hdnBalanceTo;

function SetResetFilterOption() {
    var PreviousSelectedData = [];

    if (hdnType != undefined && hdnType.length > 0) {
        PreviousSelectedData = hdnType.split(",");
        $('#ddlPaymentReceiptType').val(PreviousSelectedData).trigger('change');
    }

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

$(document).on('change', '#ddlPaymentReceiptType', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#ddlPaymentReceiptType option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
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

    if (filterCriteria == 'type') {
        $("#ddlPaymentReceiptType").val(null).trigger('change');
        $("#spnSelectedType").addClass('hide');
        hdnType = "";
    }
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
        hdnType = getTypeIDs();
        hdnAccount = getAccountIDs();
        hdnCollector = getCashierIDs();
        hdnBalanceFrom = $("#txtBalanceFrom").val();
        hdnBalanceTo = $("#txtBalanceTo").val();

        //
        var paymentReceiptTypeID = null;
        var printTemplateID = null;

        if ($("#ddlPaymentReceiptType").val() == "i") {
            paymentReceiptTypeID = 1;
            printTemplateID = 6;
        }
        else if ($("#ddlPaymentReceiptType").val() == "o") {
            paymentReceiptTypeID = 1;
            printTemplateID = 10;
        }
        else {
            paymentReceiptTypeID = $("#ddlPaymentReceiptType").val();
        }

        $("#PaymentReceiptTypeID").val(paymentReceiptTypeID);
        $("#PrintTemplateID").val(printTemplateID);
        //

        $("#FilterAccountID").val(hdnAccount);
        $("#FilterCashierID").val(hdnCollector);
        $("#BalanceFrom").val(hdnBalanceFrom);
        $("#BalanceTo").val(hdnBalanceTo);
    }
    return isvalid;
}

function validateAdvanceSearch() {
    var selectedTypeTexts = "";
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

        selectedTypeTexts = getTypeText();
        if (selectedTypeTexts != '') {
            $("#dvsearchinfo").removeClass('hide');
            $("#spnSelectedType").removeClass('hide');
            $("#TypeIdSearchText").text(SelectedType + " : " + selectedTypeTexts);
        }
        else {
            $("#spnSelectedType").addClass('hide');
            $("#TypeIdSearchText").text('');
        }
    }
    if (isvalid) {
        htnType = getTypeIDs();
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
        $("#ddlPaymentReceiptType").val(null).trigger('change');
        $("#AccountIDs").val(null).trigger('change');
        $("#CashierIDs").val(null).trigger('change');
        SetResetFilterOption();
        $("#advanceSearchModal").modal('show');
        $("#txtBalanceFrom").focus();
    }

}

function getTypeIDs() {
    var selectTypeList = [];
    if ($("#ddlPaymentReceiptType").select2('data').length) {
        $.each($("#ddlPaymentReceiptType").select2('data'), function (key, item) {
            if (item.id != 0)
                selectTypeList += "," + item.id;
        });
    }
    var selectedTypeIDs = "";
    if (selectTypeList.length > 0)
        selectedTypeIDs = selectTypeList.substring(1, selectTypeList.length);
    return selectedTypeIDs;
}

function getTypeText() {
    var selectTypeList = [];
    if ($("#ddlPaymentReceiptType").select2('data').length) {
        $.each($("#ddlPaymentReceiptType").select2('data'), function (key, item) {
            if (item.id != 0)
                selectTypeList += "," + item.text;
        });
    }
    var selectedTypeTexts = "";
    if (selectTypeList.length > 0)
        selectedTypeTexts = selectTypeList.substring(1, selectTypeList.length);
    return selectedTypeTexts;
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

$(document).on('click', '#btnGo', function () {
    if (validateForm()) {
        InitializeDataTable("tblReceiptRegister");
        return true;
    }
    return false;
});

function InitializeDataTable(tableName) {
    //
    var paymentReceiptTypeID = null;
    var printTemplateID = null;
    var groupColumn = 0;
    if ($('#ddlPaymentReceiptType').val() == "i") {
        paymentReceiptTypeID = 1;
        printTemplateID = 6;
    }
    else if ($('#ddlPaymentReceiptType').val() == "o") {
        paymentReceiptTypeID = 1;
        printTemplateID = 10;
    }
    else {
        paymentReceiptTypeID = $('#ddlPaymentReceiptType').val();
    }
    //

    $('#dvUpdate').show();

    $('#' + tableName).dataTable({
        "language": {
            "emptyTable": nodatamsg,
            "zeroRecords": nodatamsg
        },
        "zeroRecords": nodatamsg,
        "serverSide": true,
        "processing": true,
        "bFilter": false,
        "bInfo": false,
        "bLengthChange": false,
        "autoWidth": false,
        "ordering": false,
        "paging": false,
        "scrollY": "100vh",
        "scrollX": true,
        scrollCollapse: true,
        "ajax": {
            "url": ROOTPath + "/Reports/Report/ReceiptRegisterForCR",
            "type": "POST",
            "data": function (data) {
                data.startDate = new Date($("#StartDate").datepicker('getDate')).toDateString();
                data.endDate = new Date($("#EndDate").datepicker('getDate')).toDateString();
                data.paymentReceiptTypeID = paymentReceiptTypeID;
                data.printTemplateID = printTemplateID;
                data.balanceFrom = $("#txtBalanceFrom").val() ? parseFloat($("#txtBalanceFrom").val()) : null;
                data.balanceTo = $("#txtBalanceTo").val() ? parseFloat($("#txtBalanceTo").val()) : null;
                data.accountIDs = getAccountIDs();
                data.cashierIDs = getCashierIDs();
            }
        },
        destroy: true,
        "columnDefs": [
            { "visible": true, "targets": 0 }
        ],
        "columns": [
            {
                name: "Number", title: recieptNumber, "data": "Number", className: "text-center text-description-field"
            },
            {
                name: "Date", title: date, "data": "FormattedDate", className: "text-center text-description-field"
            },
            {
                name: "TaxNumber", title: taxNumber, "data": "TaxNumber", className: "text-center text-description-field"
            },
            {
                name: "AccountDisplayName", title: name, "data": "AccountDisplayName", className: "text-center text-description-field"
            },
            {
                name: "CustomField", title: customField, "data": "CustomField", className: "text-center text-description-field"
            },
            {
                name: "FormattedPaymentType", title: paymentGroup, "data": "FormattedPaymentType", className: "text-center text-description-field"
            },
            {
                name: "StatusBatchNumber", title: statuBatchNumber, "data": "FormattedStatusBatchNumber", className: "text-center text-description-field"
            },
            {
                name: "Collector", title: collector, "data": "FormattedCashier", className: "text-center text-description-field"
            },
            {
                name: "Interest", title: interest, "data": "FormattedInterest", className: "text-center text-description-field"
            },
            {
                name: "Penalties", title: penalties, "data": "FormattedPenalties", className: "text-center text-description-field"
            },
            {
                name: "Charges", title: charges, "data": "FormattedCharges", className: "text-center text-description-field"
            },
            {
                name: "Discount", title: exemption + '/' + discount, "data": "FormattedDiscount", className: "text-center text-description-field"
            },
            {
                name: "TotalAmountOfficialReceipt", title: totalAmountOfficialTicket, "data": "FormattedTotalAmountOfficialReceipt", className: "text-center text-description-field"
            },
            {
                name: "PaymentPeriod", title: paymentPeriod, "data": "PaymentPeriod", className: "text-center text-description-field"
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
                    '<tr class="group"><td class="text-right body-content font-bold" colspan="12">' + TotalValue + ':</td>' +
                    '<td class="text-center body-content font-bold">' + CurrencyGlobalFormatWithText(sum) + '</td><td></td></tr>'
                );

            }
        },
        "footerCallback": function (row, data, start, end, display) {
            var totalAmountOfficial = 0;

            for (var i = 0; i < display.length; i++) {
                totalAmountOfficial = totalAmountOfficial + data[i].TotalAmountOfficialReceipt;
            }
            $(row).find("th").eq("1").text(NumberToCultureFormat(totalAmountOfficial));
            $("#tblFooter").removeClass("hide");
        }
    });
}

function Print() {
    if (validateForm()) {

        //
        var paymentReceiptTypeID = null;
        var printTemplateID = null;

        if ($('#ddlPaymentReceiptType').val() == "i") {
            paymentReceiptTypeID = 1;
            printTemplateID = 6;
        }
        else if ($('#ddlPaymentReceiptType').val() == "o") {
            paymentReceiptTypeID = 1;
            printTemplateID = 10;
        }
        else {
            paymentReceiptTypeID = $('#ddlPaymentReceiptType').val();
        }
        //

        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintReceiptRegisterForCR",
            data: {
                'startDate': new Date($("#StartDate").datepicker('getDate')).toDateString()
                , 'endDate': new Date($("#EndDate").datepicker('getDate')).toDateString()
                , 'paymentReceiptTypeID': paymentReceiptTypeID
                , 'printTemplateID': printTemplateID
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