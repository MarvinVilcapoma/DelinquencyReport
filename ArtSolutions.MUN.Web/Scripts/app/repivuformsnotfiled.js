$(document).ready(function () {
    
    $('#dvUpdate').hide();  
    GetAccountForSelect('AccountIDs', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px', true);
    initializeDatePicker();
});

function initializeDatePicker() {
    $('.periodDate').datepicker({
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture
    });
    $('.periodDate').datepicker('clearDates');
    $('.periodDate').datepicker('update', new Date());
}

$(document).on('click', '#btnGo', function () {
    if (validateForm()) {
        InitializeDataTable("tblIVUFormsNotFiled");
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
    var isvalid = false;
    if (new Date($("#Since").val()) > new Date($("#Till").val()))
        showAlert('error', $("#Till").attr("data-compare-val-msg"));
    else if ($("#Since").val() != "" && $("#Till").val() == "")
        showAlert('error', tillDateRequiredMsg);
    else if ($("#Since").val() == "" && $("#Till").val() != "")
        showAlert('error', sinceDateRequiredMsg);
    else if ($("#Since").val() == "" && $("#Till").val() == "")
        isvalid = true;
    else
        isvalid = true;
    return isvalid;
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
        "conditionalPaging": true,
        "paging": false,
        "scrollY": "100vh",
        "scrollCollapse": true,
        "ajax": {
            "url": ROOTPath + "/Reports/Report/IVUFormsNotFiled",
            "type": "POST",
            "data": function (data) {
                data.accountIDs = getAccountIDs();
                data.since = ($("#Since").val() == "") ? null : new Date($("#Since").datepicker('getDate')).toDateString();
                data.till = ($("#Till").val() == "") ? null : new Date($("#Till").datepicker('getDate')).toDateString();
            }
        },
        destroy: true,
        "columns": [
            {
                name: "AccountNumber", title: accountNumber, "data": "AccountNumber", className: "col-lg-2 text-center"
            },
            {
                name: "AccountName", title: name, "data": "AccountName", className: "col-lg-3 text-description-field text-center"
            },
            {
                name: "RegisterNumber", title: registrationNo, "data": "RegisterNumber", className: "col-lg-2 text-center"
            },
            {
                name: "StartPeriodWithoutFiling", title: startPeriodWithoutFiling, "data": "FormattedStartPeriod", className: "col-lg-3 text-center"
            },
            {
                name: "MonthsWithoutFiling", title: monthsWithoutFiling, "data": "FormattedMonthsWithoutFiling", className: "col-lg-2 text-center"
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
            async: false,
            url: ROOTPath + "/Reports/Report/PrintIVUFormsNotFiled",
            data: {
                'accountIDs': getAccountIDs()
                , 'since': ($("#Since").val() == "") ? null : new Date($("#Since").datepicker('getDate')).toDateString()
                , 'till': ($("#Till").val() == "") ? null : new Date($("#Till").datepicker('getDate')).toDateString()
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