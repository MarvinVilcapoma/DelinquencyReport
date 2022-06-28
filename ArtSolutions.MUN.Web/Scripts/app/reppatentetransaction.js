$(document).ready(function () {

    $('#dvUpdate').hide();
    GetAccountForSelect('AccountIDs', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px', true);
    initializeDatePicker();
    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });
    $(".ibox-content .select2dropdown").select2();
    $('#ddlStatus').on('change', function () {
        $('#searchStatusID').val($(this).val());
    });
});
var hdnAccount;
var hdnStatus;
function SetResetFilterOption() {
    var PreviousSelectedData = [];
    if (hdnAccount != undefined && hdnAccount.length > 0) {
        PreviousSelectedData = hdnAccount.split(",");
        $('#AccountIDs').val(PreviousSelectedData).trigger('change');
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
        InitializeDataTable("tblBusinessLicenseTransaction");
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
    if (filterCriteria == 'status') {
        $("#ddlStatus").val('0').trigger('change');
        $("#spnSelectedStatus").addClass('hide');
        hdnStatus = "";
    }
    InitializeDataTable("tblBusinessLicenseTransaction");
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
    //if (isvalid) {
    //    hdnAccount = getAccountIDs();
    //    $("#FilterAccountID").val(hdnAccount);
    //}
    return isvalid;
}

function validateAdvanceSearch() {
    var isvalid = false;
    var selectedStatusTexts = "";
    var selectedStatusValue = "";
    isvalid = true;
    InitializeDataTable("tblBusinessLicenseTransaction");
    $("#advanceSearchModal").modal('hide');
    
    selectedStatusTexts = $('#ddlStatus :selected').text();
    selectedStatusValue = $('#ddlStatus :selected').val();
    
    if (selectedStatusValue != '0') {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnSelectedStatus").removeClass('hide');
        $("#StatusSearchText").text(SelectedStatus + " : " + selectedStatusTexts);
    }
    else {
        $("#spnSelectedStatus").addClass('hide');
        $("#StatusSearchText").text('');
    }
    if (isvalid) {
        hdnStatus = $('#ddlStatus').val();
    }
    return isvalid;
}

function loadAdvanceSearch() {
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
            "url": ROOTPath + "/Reports/Report/TransactionByBusinessLicense",
            "type": "POST",
            "data": function (data) {
                data.startDate = new Date($("#StartDate").datepicker('getDate')).toDateString();
                data.endDate = new Date($("#EndDate").datepicker('getDate')).toDateString();
                //data.accountIDs = getAccountIDs();
                data.searchStatus = $('#ddlStatus :selected').val();
            }
        },
        destroy: true,
        "columns": [
            {
                name: "TransactionDate", title: date, "data": "FormattedDate", className: "col-lg-2 text-center text-description-field padding-sm"
            },
            {
                name: "AccountDisplayName", title: name, "data": "AccountDisplayName", className: "col-lg-2 text-center text-description-field padding-sm"
            },
            {
                name: "PatentNumber", title: patentnumber, "data": "PatentNumber", className: "col-lg-2 text-center text-description-field padding-sm"
            },
            {
                name: "FiscalYear", title: fiscalyear, "data": "FormattedFiscalYearID", className: "col-lg-2 text-center text-description-field padding-sm"
            },
            {
                name: "TransactionType", title: type, "data": "TransactionType", className: "col-lg-2 text-center text-description-field padding-sm"
            },
            {
                name: "TransactionReason", title: transreason, "data": "Note", className: "col-lg-2 text-left text-description-field"
            }
        ],
        "lengthMenu": [[15, 25, 50, 100], [15, 25, 50, 100]],
        "order": [[1, "asc"]],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $(nRow).find("td").each(function (index) {
                this.setAttribute('title', $(this).text().trim());
            });
        }
    });
}

function Print() {
    if (validateForm()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintTransactionByBusinessLicense",
            data: {
                'startDate': new Date($("#StartDate").datepicker('getDate')).toDateString()
                , 'endDate': new Date($("#EndDate").datepicker('getDate')).toDateString()
                //, 'accountIDs': getAccountIDs()
                ,'searchStatus':$('#ddlStatus :selected').val()
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