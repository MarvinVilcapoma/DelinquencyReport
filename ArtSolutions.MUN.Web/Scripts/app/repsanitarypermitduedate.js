$(document).ready(function () {
    $('#dvUpdate').hide();
    GetAccountForSelect('AccountIDs', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px', true);
    initializeDatePicker();
    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });
});
var hdnAccount;
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
    $('#FromDate').datepicker("setDate", new Date(dt.getFullYear(), dt.getMonth(), dt.getDate() - 1));
    $('#ToDate').datepicker('setDate', new Date(dt.getFullYear(), dt.getMonth(), dt.getDate()));
}

$(document).on('click', '#btnGo', function () {
    if (validateForm()) {
        InitializeDataTable("tblBusinessLicenceSanitaryPermitDueDate");
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
    if (filterCriteria == 'accounts') {
        $("#AccountIDs").val(null).trigger('change');
        $("#spnSelectedAccount").addClass('hide');
        hdnAccount = "";
    }
    InitializeDataTable("tblBusinessLicenceSanitaryPermitDueDate");
}

function validateForm() {
    var isvalid = false;
    if ($("#FromDate").val() == undefined || $("#FromDate").val() == '')
        showAlert('error', $("#FromDate").attr("data-required-msg"));
    else if ($("#ToDate").val() == undefined || $("#ToDate").val() == '')
        showAlert('error', $("#ToDate").attr("data-required-msg"));
    else if (new Date($("#ToDate").datepicker('getDate')) < new Date($("#FromDate").datepicker('getDate')))
        showAlert('error', $("#ToDate").attr("data-compare-val-msg"));
    else
        isvalid = true;
    if (isvalid) {
        hdnAccount = getAccountIDs();
        $("#FilterAccountID").val(hdnAccount);
    }
    return isvalid;
}

function validateAdvanceSearch() {
    var isvalid = false;
    var selectedAccountTexts = "";
    isvalid = true;
    InitializeDataTable("tblBusinessLicenceSanitaryPermitDueDate");
    $("#advanceSearchModal").modal('hide');

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
    if (isvalid) {
        hdnAccount = getAccountIDs();
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
        columnDefs: [
            // Center align the header content of column
            { className: "dataTable th" }
        ],
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
            "url": ROOTPath + "/Reports/Report/BusinessLicenceSanitaryPermitDueDate",
            "type": "POST",
            "data": function (data) {
                data.fromDate = new Date($("#FromDate").datepicker('getDate')).toDateString();
                data.toDate = new Date($("#ToDate").datepicker('getDate')).toDateString();
                data.accountIDs = getAccountIDs();
            }
        },
        destroy: true,
        "columns": [
            {
                name: "ServiceName", title: license, "data": "ServiceName", className: "col-lg-3 text-description-field"
            },
            {
                name: "AccountName", title: accountName, "data": "AccountName", className: "col-lg-3 text-description-field"
            },
            {
                name: "PatentNumber", title: patentNumber, "data": "PatentNumber", className: "col-lg-3 text-description-field"
            },
            {
                name: "PhoneNumber", title: phoneNumber, "data": "PhoneNumber", className: "text-center col-lg-2 text-description-field"
            },
            {
                name: "PermisoSanitario", title: permisoSanitario, "data": "PermisoSanitario", className: "text-center col-lg-2 text-description-field"
            },
            {
                name: "DueDate", title: dueDate, "data": "FormattedDueDate", className: "text-center col-lg-2"
            },
            {
                name: "Days", title: elapsedDays, "data": "Days", className: "text-center col-lg-2"
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
            url: ROOTPath + "/Reports/Report/PrintBusinessLicenceSanitaryPermitDueDate",
            data: {
                'fromDate': new Date($("#FromDate").datepicker('getDate')).toDateString()
                , 'toDate': new Date($("#ToDate").datepicker('getDate')).toDateString()
                , 'accountIDs': getAccountIDs()
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