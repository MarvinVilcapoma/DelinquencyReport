$(document).ready(function () {
    
    $('#dvUpdate').hide();   
    GetAccountForSelect('AccountIDs', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px', true);
    initializeDatePicker();
    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });
});
var hdnAccount;
var hdnBalanceFrom;
var hdnBalanceTo;

function SetResetFilterOption() {
    var PreviousSelectedData = [];
    if (hdnAccount != undefined && hdnAccount.length > 0) {
        PreviousSelectedData = hdnAccount.split(",");        
        $('#AccountIDs').val(PreviousSelectedData).trigger('change');
    }
    if (hdnBalanceFrom != undefined && hdnBalanceFrom.length > 0) {
        $("#txtBalanceFrom").val(hdnBalanceFrom)
        $("#txtBalanceTo").val(hdnBalanceTo)
    }
}
function initializeDatePicker() {
    $('.periodDate').datepicker({
        defaultDate: new Date(),
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture,
    });
    $('.periodDate').datepicker('update', new Date());
}

$(document).on('click', '#btnGo', function () {
    if (validateForm()) {
        InitializeDataTable("tblIVUBalanceSheet");
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

function validateForm() {
    var isvalid = true;
    $("#StartPeriod,#EndPeriod").removeClass("error");
    if ($("#StartPeriod").val() == '') {
        showAlert('error', $("#StartPeriod").attr("data-required-msg"));
        $("#StartPeriod").addClass("error");
        isvalid = false;
    }
    if ($("#EndPeriod").val() == '') {
        showAlert('error', $("#EndPeriod").attr("data-required-msg"));
        $("#EndPeriod").addClass("error");
        isvalid = false;
    }
    if (new Date($("#EndPeriod").datepicker('getDate')) < new Date($("#StartPeriod").datepicker('getDate'))) {
        showAlert('error', $("#EndPeriod").attr("data-compare-val-msg"));
        isvalid = false;
    }

    if (isvalid) {
        hdnAccount = getAccountIDs();
        hdnBalanceFrom = $("#txtBalanceFrom").val();
        hdnBalanceTo = $("#txtBalanceTo").val();
        $("#FilterAccountID").val(hdnAccount);
        $("#BalanceFrom").val(hdnBalanceFrom);
        $("#BalanceTo").val(hdnBalanceTo);
    }
    return isvalid;
}

function validateAdvanceSearch() {
    var selectedAccountTexts = "";
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
        InitializeDataTable("tblIVUBalanceSheet");
        $("#advanceSearchModal").modal('hide');

        if ($("#txtBalanceFrom").val() != '' && $("#txtBalanceTo").val() != '') {
            $("#dvsearchinfo").removeClass('hide');
            $("#spnBalanceRange").removeClass('hide');
            $("#SearchText").text(BalanceRange + " : " + $("#txtBalanceFrom").val() + " - " + $("#txtBalanceTo").val());
        }
        else {
            $("#spnBalanceRange").addClass('hide');
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
    }
    if (isvalid) {
        hdnAccount = getAccountIDs();
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
        SetResetFilterOption();        
        $("#advanceSearchModal").modal('show');
        $("#txtBalanceFrom").focus();
    }
}

function clearSearch(filterCriteria) {
    if (filterCriteria == 'accounts') {
        $("#AccountIDs").val(null).trigger('change');
        $("#spnSelectedAccount").addClass('hide');
        hdnAccount = "";
    }
    if (filterCriteria == 'balance') {
        $("#spnBalanceRange").addClass('hide');
        hdnBalanceFrom = "";
        hdnBalanceTo = "";
        $("#txtBalanceFrom").val('');
        $("#txtBalanceTo").val('');
    }
    InitializeDataTable("tblIVUBalanceSheet");
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

function InitializeDataTable(tableName) {
    $('#dvUpdate').show();

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
        "conditionalPaging": true,
        "scrollY": "100vh",
        "scrollCollapse": true,
        "ajax": {
            "url": ROOTPath + "/Reports/Report/IVUBalanceSheet",
            "type": "POST",
            "data": function (data) {
                data.startPeriod = new Date($("#StartPeriod").datepicker('getDate')).toDateString();
                data.endPeriod = new Date($("#EndPeriod").datepicker('getDate')).toDateString();
                data.accountIDs = getAccountIDs();
                data.balancefrom = $("#txtBalanceFrom").val() ? parseFloat($("#txtBalanceFrom").val()) : null;
                data.balanceto = $("#txtBalanceTo").val() ? parseFloat($("#txtBalanceTo").val()) : null;
            }
        },
        destroy: true,
        "columns": [
            {
                name: "CustomerNumber", title: accountNumber, "data": "CustomerNumber", className: "col-lg-1 text-center"
            },
            {
                name: "CustomerName", title: name, "data": "CustomerName", className: "col-lg-2 text-description-field"
            },
            {
                name: "Period", title: period, "data": "FormattedPeriod", className: "col-lg-1 text-center"
            },
            {
                name: "TotalTaxableSales", title: totaltaxablesales, "data": "FormattedTotalTaxableSales", className: "col-lg-2 text-right"
            },
            {
                name: "Principal", title: ivu, "data": "FormattedPrincipal", className: "col-lg-1 text-right"
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
                name: "Balance", title: balance, "data": "FormattedBalance", className: "col-lg-2 text-right"
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
            var totalBalance = 0;
            for (var i = 0; i < display.length; i++) {
                totalBalance = totalBalance + data[i].Balance;
            }

            $(row).find("th").eq("1").text(NumberToCultureFormat(totalBalance));
            $("#tblFooter").removeClass("hide");
        }
    });
}

function Print() {
    if (validateForm()) {
        $.ajax({
            type: "GET",
            async: false,
            url: ROOTPath + "/Reports/Report/PrintIVUBalanceSheet",
            data: {
                'startPeriod': new Date($("#StartPeriod").datepicker('getDate')).toDateString()
                , 'endPeriod': new Date($("#EndPeriod").datepicker('getDate')).toDateString()
                , 'accountIDs': getAccountIDs()
                , 'balancefrom': $("#txtBalanceFrom").val() ? parseFloat($("#txtBalanceFrom").val()) : null
                , 'balanceto': $("#txtBalanceTo").val() ? parseFloat($("#txtBalanceTo").val()) : null
            },
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
                }
            }
        });
    }
    else
        return false;
    return true;
}