var pageSize = 10;
var rowCount = pageSize;

$(document).ready(function () {
    $(".select2dropdown").select2();
    initializeDatePicker();
});

$(document).on('change', '#CommaSeperatedBanacioIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#CommaSeperatedBanacioIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

$(document).on('click', '#btnGo', function () {
    if (validateForm()) {
        InitializeData();
    }
    return false;
});

function initializeDatePicker() {
    $('#DueDate').datepicker({
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture
    });
    var dt = new Date();
    $('#DueDate').datepicker('setDate', new Date(dt.getFullYear(), dt.getMonth(), dt.getDate()));
}

function initDataTable() {
    $('#tblExportBankPayments').DataTable({
        "scrollY": "100vh",
        "scrollX": true,
        scrollCollapse: true,
        "paging": false,
        "ordering": false,
        "info": false,
        "searching": false,
        "lengthChange": false,
        responsive: false,
        autoWidth: false,
        language: {
            "emptyTable": nodatamsg
        },
        "fnDrawCallback": function () {
            /* Apply the tooltips */
            $('#tblExportBankPayments tbody tr td').tooltip({
                container: "body",
                html: true
            });
        }
    });
}

function InitializeData() {
    $.ajax({
        type: "POST",
        url: ROOTPath + "/Reports/Report/ExportBankPayments",
        data: {
            'dueDate': new Date($("#DueDate").datepicker('getDate')).toDateString(),
            'commaSeperatedContractIDs': ($("#CommaSeperatedBanacioIDs").val() == null ? null : $("#CommaSeperatedBanacioIDs").val().join(',')),
            'pageIndex': 1,
            'pageSize': pageSize,
            'isLoadMore': false
        },
        success: function (response) {
            if (response.status) {
                hideLoading();
                $("#divExportBankPayments").html("").html(response.data);
                initDataTable();
                rowCount = pageSize;
            }
            else {
                showAlert('error', response.data);
            }
        }
    });
}

function validateForm() {
    var isvalid = true;

    $("#DueDate").removeClass("error");
    if ($("#DueDate").val() == undefined || $("#DueDate").val() == '') {
        showAlert('error', $("#DueDate").attr("data-required-msg"));
        $("#DueDate").addClass("error");
        isvalid = false;
    }

    return isvalid;
}

function loadMoreExportBankPayments(element) {
    var currentQualitativePage = $(element).data('currentpage') ? $(element).data('currentpage') : 1;
    var newPage = currentQualitativePage + 1;

    $.ajax({
        type: "POST",
        url: ROOTPath + "/Report/ExportBankPayments",
        data: {
            'dueDate': new Date($("#DueDate").datepicker('getDate')).toDateString(),
            'commaSeperatedContractIDs': ($("#CommaSeperatedBanacioIDs").val() == null ? null : $("#CommaSeperatedBanacioIDs").val().join(',')),
            'pageIndex': newPage,
            'pageSize': pageSize,
            'isLoadMore': true
        },
        success: function (response) {
            $(response.data).insertBefore("#tbodyExportBankPayments");
            $(element).data('currentpage', newPage);
            currentQualitativePage = newPage;
            rowCount = rowCount + response.RowCount;
            if (rowCount < $("#TotalRecord").val()) {
                $("#tbodyExportBankPayments").show();
            }
            else {
                $("#tbodyExportBankPayments").hide();
            }
        }
    });
}

$("button[itag='btnExport']").click(function () {
    if (validateForm()) {
        var data = {
            'dueDate': new Date($("#DueDate").datepicker('getDate')).toDateString(),
            'commaSeperatedContractIDs': $("#CommaSeperatedBanacioIDs").val() == null ? null : $("#CommaSeperatedBanacioIDs").val().join(',')
        };
        $.get(ROOTPath + "/Report/ExportBankPaymentsToDBF", data, function (response) {
            if (response) {
                if (response.status)
                    window.location = downloadURL;
                else
                    if (response.message)
                        showAlert('error', response.message);
            }
        });
    }
    return false;
});