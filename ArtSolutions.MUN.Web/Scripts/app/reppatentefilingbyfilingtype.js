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
        InitializeDataTable("tblDatatable");
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

    InitializeDataTable("tblDatatable");    
}

function validateForm() {
    var isvalid = false;
    if ($("#StartDate").val() == undefined || $("#StartDate").val() == '')
        showAlert('error', $("#StartDate").attr("data-required-msg"));
    else if ($("#EndDate").val() == undefined || $("#EndDate").val() == '')
        showAlert('error', $("#EndDate").attr("data-required-msg"));
    else if (new Date($("#EndDate").val()) < new Date($("#StartDate").val()))
        showAlert('error', $("#EndDate").attr("data-compare-val-msg"));
    else
        isvalid = true;
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
    var isvalid = false;
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
        InitializeDataTable("tblDatatable");
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
    }
    if (isvalid) {
        hdnAccount = getAccountIDs();
        hdnBalanceFrom = $("#txtBalanceFrom").val();
        hdnBalanceTo = $("#txtBalanceTo").val();
    }
    return isvalid;
}

function loadAdvanceSearch() {
    $("#txtBalanceFrom").val('');
    $("#txtBalanceTo").val('');
    $("#AccountIDs").val(null).trigger('change');
    SetResetFilterOption();
    $("#advanceSearchModal").modal('show');
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
        "scrollX": true,
        scrollCollapse: true,
        "ajax": {
            "url": ROOTPath + "/Reports/Report/BusinessLicenseFilingByFilingType",
            "type": "POST",
            "data": function (data) {
                data.startDate = new Date($("#StartDate").datepicker('getDate')).toDateString();
                data.endDate = new Date($("#EndDate").datepicker('getDate')).toDateString();
                data.balanceFrom = $("#txtBalanceFrom").val() ? parseFloat($("#txtBalanceFrom").val()) : null;
                data.balanceTo = $("#txtBalanceTo").val() ? parseFloat($("#txtBalanceTo").val()) : null;
                data.accountIDs = getAccountIDs();
            }
        },
        destroy: true,
        "columns": [
            {
                name: "CustomerNumber", title: customernumber, "data": "AccountId", width: "7%"
            },
            {
                name: "CustomerName", title: name, "data": "AccountName", width: "18%"
            },
            {
                name: "SocialSecurity", title: socialsecurity, "data": "SocialSecurity", width: "9%"
            },
            {
                name: "FiscalYear", title: fiscalyear, "data": "FormattedFiscalYearID", width: "9%"
            },
            {
                name: "YearContrib", title: year, "data": "FiscalYearID", width: "5%"
            },
            {
                name: "FilingDate", title: filingdate, "data": "FormattedFillingDate", width: "9%"
            },
            {
                name: "AccountRegisterNumber", title: identitymunicipal, "data": "AccountIdByFilling", className: "text-left", width: "7%"
            },
            {
                name: "SalesVolume", title: salesvolume, "data": "FormattedSalesVolume", className: "text-right", width: "9%"
            },
            {
                name: "FillingBy", title: fillingby, "data": "FormattedUser", className: "text-left", width: "18%"
            },
            {
                name: "RegistrationDate", title: registrationdate, "data": "FormattedDate", className: "text-left", width: "9%"
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
            var totalSalesVolume = 0;
            for (var i = 0; i < display.length; i++) {
                totalSalesVolume = totalSalesVolume + data[i].SalesVolume;
            }

            $(row).find("th").eq("1").text(NumberToCultureFormat(totalSalesVolume));
            $("#tblFooter").removeClass("hide");
        }
    });
}

function Print() {
    if (validateForm()) {
        $.ajax({
            type: "GET",
            async: false,
            url: ROOTPath + "/Reports/Report/PrintBusinessLicenseFilingByFilingType",
            data: {
                'startDate': new Date($("#StartDate").datepicker('getDate')).toDateString()
                , 'endDate': new Date($("#EndDate").datepicker('getDate')).toDateString()
                , 'balanceFrom': $("#txtBalanceFrom").val() ? parseFloat($("#txtBalanceFrom").val()) : null
                , 'balanceTo': $("#txtBalanceTo").val() ? parseFloat($("#txtBalanceTo").val()) : null
                , 'accountIDs': getAccountIDs()
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