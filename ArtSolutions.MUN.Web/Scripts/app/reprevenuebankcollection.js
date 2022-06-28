var pageSize = 100;
var rowCount = pageSize;

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
    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });
});

var hdnAccount;
var hdnService;

function SetResetFilterOption() {
    var PreviousSelectedData = [];
    if (hdnService != undefined && hdnService.length > 0) {
        PreviousSelectedData = hdnService.split(",");
        $('#ServiceIDs').val(PreviousSelectedData).trigger('change');
    }
}

$(document).on('change', '#ServiceIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("-1").trigger('change');
        $("#ServiceIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

$(document).on('change', '#BankIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("-1").trigger('change');
        $("#BankIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

$(document).on('change', '#ContractIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("-1").trigger('change');
        $("#ContractIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

function clearSearch(filterCriteria) {
    if (filterCriteria == 'services') {
        $("#ServiceIDs").val(null).trigger('change');
        $("#spnSelectedService").addClass('hide');
        hdnService = "";
    }
    InitializeData();
}

function validateForm() {
    var isvalid = true;

    if (isvalid) {
        hdnService = getServiceCodes();
        $("#FilterServiceID").val(hdnService);
        $("#FilterBankID").val(getBankCodes);
        $("#FilterContractID").val(getContractCodes);
    }
    console.log(isvalid)
    return isvalid;
}

function validateAdvanceSearch() {
    var isvalid = true;
    InitializeData();
    $("#advanceSearchModal").modal('hide');

    var selectedServiceTexts = "";
    selectedServiceTexts = getServiceText();
    if (selectedServiceTexts != '') {
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
    $('#ServiceIDs').val([]).trigger('change');
    SetResetFilterOption();
    $("#advanceSearchModal").modal('show');
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

function getBankCodes() {
    var selectBankList = [];
    if ($("#BankIDs").select2('data').length) {
        $.each($("#BankIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectBankList += "," + item.id;
        });
    }
    var selectedServiceIDs = "";
    if (selectBankList.length > 0)
        selectedServiceIDs = selectBankList.substring(1, selectBankList.length);
    return selectedServiceIDs;
}

function getContractCodes() {
    var selectContractList = [];
    if ($("#ContractIDs").select2('data').length) {
        $.each($("#ContractIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectContractList += "," + item.id;
        });
    }
    var selectedServiceIDs = "";
    if (selectContractList.length > 0)
        selectedServiceIDs = selectContractList.substring(1, selectContractList.length);
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

$(document).on('click', '#btnGo', function () {
    if (validateForm()) {
        InitializeData();
        return true;
    }
    return false;
});

//$(window).resize(function () {
//    if (validateForm()) {
//        InitializeData();
//        return true;
//    }
//    return false;
//});

function InitializeDataTable() {
    $('#tblRevenueDailyCollection').DataTable({
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
        }
    });
}

function InitializeData() {
    $.ajax({
        type: "POST",
        url: ROOTPath + "/Reports/Report/RevenuefromBankCollection",
        data: {
            'startPeriod': $("#StartPeriodDate").val(),
            'contractIds': getContractCodes(),
            'bankIds': getBankCodes(),
            'serviceIds': getServiceCodes(),
            'pageIndex': 1,
            'pageSize': pageSize,
            'isLoadMore': false
        },
        success: function (response) {
            if (response.status) {
                hideLoading();

                ////Previous Record
                //$("#divRevenueDailyCollection").html("").html(response.data);
                //InitializeDataTable();
                //rowCount = pageSize;
                ////

                //test
                $("#divRevenueBankCollection").html("").html(response.data);

                if ($("#TotalRecord").val() <= 100) {
                    $(".tdTotal").show();
                }
                else {
                    //TotalAmount = parseFloat(response.TotalAmount);
                    $(".tdTotal").hide();
                }
                rowCount = pageSize;
                InitializeDataTable();
                //

            }
            else {
                showAlert('error', response.data);
            }
        }
    });
}

function loadMoreRevenueBankCollection(element) {
    var currentQualitativePage = $(element).data('currentpage') ? $(element).data('currentpage') : 1;
    var newPage = currentQualitativePage + 1;

    $.ajax({
        type: "POST",
        url: ROOTPath + "/Reports/Report/RevenuefromBankCollection",
        data: {
            'startPeriod': $("#StartPeriodDate").val(),
            'contractIds': getContractCodes(),
            'bankIds': getBankCodes(),
            'serviceIds': getServiceCodes(),
            'pageIndex': newPage,
            'pageSize': pageSize,
            'isLoadMore': true
        },
        success: function (response) {

            ////Previous Code
            //$(response.data).insertBefore("#tbodyRevenueBankCollection");
            //$(element).data('currentpage', newPage);
            //currentQualitativePage = newPage;
            //rowCount = rowCount + response.RowCount;
            //if (rowCount < $("#TotalRecord").val()) {
            //    $("#tbodyRevenueBankCollection").show();
            //}
            //else {
            //    $("#tbodyRevenueBankCollection").hide();
            //}
            ////

            $(response.data).insertBefore("#tbodyRevenueBankCollection");
            $(element).data('currentpage', newPage);

            currentQualitativePage = newPage;
            rowCount = rowCount + response.RowCount;
            if (rowCount < $("#TotalRecord").val()) {
                $("#tbodyRevenueBankCollection").show();
            }
            else {
                $("#tbodyRevenueBankCollection").hide();
            }

            //TotalAmount = parseFloat(response.TotalAmount);

            if (rowCount == $("#TotalRecord").val()) {
                //$("#tdTotalAmount").html(GlobalFormat(TotalAmount));
                $(".tdTotal").show();
            }
        }
    });
}
function Print() {

    $.ajax({
        type: "GET",
        url: ROOTPath + "/Reports/Report/PrintRevenueByBankCollection",
        data: {
            'startPeriod': $("#StartPeriodDate").val(), 'contractIds': getContractCodes(), 'bankIds': getBankCodes(), 'serviceIds': getServiceCodes()
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
                return false;
            }
            else {
                showAlert('error', response.data);
                return false;
            }
        }
    });

    return true;
}