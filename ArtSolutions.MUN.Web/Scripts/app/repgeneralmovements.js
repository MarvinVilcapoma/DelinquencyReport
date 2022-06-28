var pageSize = 50;
var rowCount = pageSize;

$(document).ready(function () {
    $(".select2dropdown").select2();
    GetAutoCompAccountForSelect('AccountId', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px');
    initializeDatePicker();
});
function GetAutoCompAccountForSelect(id, accountID, accountTypeIDs, placeholder, inputTooShort, noResults, width, multiple) {
    $('#' + id).select2({
        placeholder: placeholder,
        minimumInputLength: 3,
        language: {
            inputTooShort: function () { return inputTooShort; }
            , noResults: function () { return noResults; }
        },
        multiple: multiple == null ? false : multiple,
        allowClear: true,
        width: width == null ? '100%' : width,
        ajax: {
            url: ROOTPath + "/Account/GetAccountForSearch",
            data: function (params) {
                var query = {
                    accountTypeIDs: accountTypeIDs,
                    accountID: accountID,
                    searchText: params.term,
                    isActive: null,
                    pageIndex: params.page || 1,
                    pageSize: pageSize
                };
                return query;
            },
            processResults: function (data, params) {
                if (!multiple) {
                    $('#' + id).text('');
                }

                params.page = params.page || 1;
                return {
                    results: data.AccountList,
                    pagination: {
                        more: (params.page * pageSize) < data.TotalRecord
                    }
                };
            }
        }
    });
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

function validateForm() {
    var isvalid = true;
    $("#ToDate").removeClass("error");
    if (new Date($("#ToDate").datepicker('getDate')) < new Date($("#FromDate").datepicker('getDate'))) {
        showAlert('error', $("#ToDate").attr("data-compare-val-msg"));
        isvalid = false;
    }
    return isvalid;
}

$('#UserID').on('select2:unselect', function (e) {
    if (validateForm()) {
        InitializeData();
    }
});

$(window).resize(function () {
    $('#tblGeneralMovements').DataTable().columns.adjust().responsive.recalc();
});

$('#AccountId').on('select2:unselect', function (e) {
    if (validateForm()) {
        InitializeData();
    }
});

$(document).on('click', '#btnGo', function () {
    if (validateForm()) {
        InitializeData();
    }
    return false;
});

function initDataTable() {
    $('#tblGeneralMovements').DataTable({
        "paging": false,
        "ordering": false,
        "info": false,
        "searching": false,
        "lengthChange": false,
        responsive: false,
        autoWidth: false,
        fixedHeader: true,
        language: {
            "emptyTable": nodatamsg
        },
        "scrollY": "100vh",
        "scrollX": true,
        scrollCollapse: true
    });
}

function InitializeData() {
    $.ajax({
        type: "POST",
        url: ROOTPath + "/Reports/Report/GeneralMovements",
        data: {
            'userID': $("#UserID").val()
            , 'accountID': $("#AccountId").val()
            , 'fromDate': new Date($("#FromDate").datepicker('getDate')).toDateString()
            , 'toDate': new Date($("#ToDate").datepicker('getDate')).toDateString()
            , 'pageIndex': 1
            , 'pageSize': pageSize
            , 'isLoadMore': false
        },
        success: function (response) {
            if (response.status) {
                hideLoading();
                $("#divGeneralMovements").html("").html(response.data);
                initDataTable();
                $('#tblGeneralMovements .table-description-field').tooltip({
                    container: "body",
                    html: true
                });
                rowCount = pageSize;
            }
            else {
                showAlert('error', response.data);
            }
        }
    });
}

function loadMoreGeneralMovements(element) {
    var currentQualitativePage = $(element).data('currentpage') ? $(element).data('currentpage') : 1;
    var newPage = currentQualitativePage + 1;

    $.ajax({
        type: "POST",
        url: ROOTPath + "/Reports/Report/GeneralMovements",
        data: {
            'userID': $("#UserID").val()
            , 'accountID': $("#AccountId").val()
            , 'fromDate': new Date($("#FromDate").datepicker('getDate')).toDateString()
            , 'toDate': new Date($("#ToDate").datepicker('getDate')).toDateString()
            , 'pageIndex': newPage
            , 'pageSize': pageSize
            , 'isLoadMore': true
        },
        success: function (response) {
            $(response.data).insertBefore("#tbodyGeneralMovements");
            $(element).data('currentpage', newPage);
            currentQualitativePage = newPage;
            rowCount = rowCount + response.RowCount;
            if (rowCount < $("#TotalRecord").val()) {
                $("#tbodyGeneralMovements").show();
            }
            else {
                $("#tbodyGeneralMovements").hide();
            }
        }
    });
}

function Print() {
    if (validateForm()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintGeneralMovements",
            data: {
                'userID': $("#UserID").val()
                , 'accountID': $("#AccountId").val()
                , 'fromDate': new Date($("#FromDate").datepicker('getDate')).toDateString()
                , 'toDate': new Date($("#ToDate").datepicker('getDate')).toDateString()
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