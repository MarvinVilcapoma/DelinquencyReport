$(document).ready(function () {
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

    $(".select2dropdown").select2();

    $(".closemodal").on("click", function () {
        SetResetFilterOption();
    });
});

var hdnService;

$(document).on('click', '#btnGo', function () {
    if ($("#form").valid() && validateForm()) {
        InitializeData();
    }
    return false;
});

$(window).resize(function () {
    $('#tblInconsistenciesintheApplicationofBankReceipts').DataTable().columns.adjust().responsive.recalc();
});


function SetResetFilterOption() {
    var PreviousSelectedData = [];
    if (hdnService != undefined && hdnService != "" && hdnService != null && hdnService.length > 0) {
        PreviousSelectedData = hdnService.split(",");
    }
    $('#ServiceIDs').val(PreviousSelectedData).trigger('change');
}

function clearSearch(filterCriteria) {
    if (filterCriteria == 'services') {
        $("#ServiceIDs").val([]).trigger('change');
        $("#spnSelectedService").addClass('hide');
        hdnService = "";
    }
    InitializeData();
}

$(document).on('change', '#ServiceIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#ServiceIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

function validateForm() {
    var isvalid = true;
    if (isvalid) {
        hdnService = getServiceCodes();
        $("#FilterServiceID").val(hdnService);
        $("#FilterBankID").val($("#BankIDs").val() != null ? $("#BankIDs").val().join(",") : null);
        $("#FilterContractID").val($("#ContractIDs").val() != null ? $("#ContractIDs").val().join(",") : null);
    }
    return isvalid;
}

function validateAdvanceSearch() {
    var selectedServiceTexts = "";
    var isvalid = true;
    InitializeData();
    $("#advanceSearchModal").modal('hide');

    selectedServiceTexts = getServiceText();
    if (getServiceText != '') {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnSelectedService").removeClass('hide');
        $("#ServiceIdSearchText").text(SelectedService + " : " + selectedServiceTexts);
    }
    else {
        $("#spnSelectedService").addClass('hide');
        $("#ServiceIdSearchText").text('');
    }

    if (isvalid) {
        hdnService = getServiceCodes();
    }
    return isvalid;
}

function loadAdvanceSearch() {
    if (validateForm()) {
        $("#ServiceIDs").val([]).trigger('change');
        SetResetFilterOption();
        $("#advanceSearchModal").modal('show');
        $("#ServiceIDs").focus();
    }
}

function getServiceCodes() {
    var selectServiceList = [];
    if ($("#ServiceIDs").select2('data').length) {
        $.each($("#ServiceIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectServiceList += "," + item.id;
        });
    }
    var selectedServiceIDs = "";
    if (selectServiceList.length > 0)
        selectedServiceIDs = selectServiceList.substring(1, selectServiceList.length);
    return selectedServiceIDs;
}

function getServiceText() {
    var selectServiceList = [];
    if ($("#ServiceIDs").select2('data').length) {
        $.each($("#ServiceIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectServiceList += "," + item.text;
        });
    }
    var selectedServiceTexts = "";
    if (selectServiceList.length > 0)
        selectedServiceTexts = selectServiceList.substring(1, selectServiceList.length);
    return selectedServiceTexts;
}

function initDataTable() {
    if ($("#tblInconsistenciesintheApplicationofBankReceipts tbody tr.no-data").length == 1) {
        return false;
    }
    accountStatementDataTable = $('#tblInconsistenciesintheApplicationofBankReceipts').DataTable({
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
        url: ROOTPath + "/Reports/Report/InconsistenciesInTheBankReceipts",
        data: {
            'startPeriod': new Date($("#StartPeriodDate").datepicker('getDate')).toDateString(),
            'contractIds': ($("#ContractIDs").val() != null ? $("#ContractIDs").val().join(",") : null),
            'bankIds': ($("#BankIDs").val() != null ? $("#BankIDs").val().join(",") : null),
            'serviceIds': getServiceCodes(),
        },
        success: function (response) {
            if (response.status) {
                hideLoading();
                $("#divInconsistenciesintheApplicationofBankReceipts").html("").html(response.data);
                initDataTable();
                $('#tblInconsistenciesintheApplicationofBankReceipts tbody tr td').tooltip({
                    container: "body",
                    html: true
                });
            }
            else {
                showAlert('error', response.data);
            }
        }
    });
}

function Print(data) {
    if ($("#form").valid() && validateForm()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintInconsistenciesInTheBankReceipts",
            data:
            {
                'startPeriod': new Date($("#StartPeriodDate").datepicker('getDate')).toDateString(),
                'contractIds': ($("#ContractIDs").val() != null ? $("#ContractIDs").val().join(",") : null),
                'bankIds': ($("#BankIDs").val() != null ? $("#BankIDs").val().join(",") : null),
                'serviceIds': getServiceCodes(),
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
                    return false;
                }
            }
        });
    }
    else
        return false;
    return true;
}