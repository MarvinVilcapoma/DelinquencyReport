var pageSize = 100;
var rowCount = pageSize;

$(document).ready(function () {
    initializeDatePicker();
    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });
    $(".select2dropdown").select2({ width: '100%' });
});

var hdnFrom;
var hdnTo;
var hdnTaxNumber;
var hdnMeter;

$(document).on('click', '#btnGo', function () {
    if (validateForm()) {
        InitializeData();
    }
    return false;
});

function SetResetFilterOption() {
    if (hdnFrom != undefined && hdnFrom.length > 0) {
        $("#txtFrom").val(hdnFrom);
        $("#txtTo").val(hdnTo);
    }
    if (hdnTaxNumber != undefined) {
        $("#txtTaxNumber").val(hdnTaxNumber);
    }
    if (hdnMeter != undefined) {
        $("#txtMeter").val(hdnMeter);
    }
}

function clearSearch(filterCriteria) {
    if (filterCriteria == 'range') {
        $("#txtFrom").val('');
        $("#txtTo").val('');
        $("#spnRange").addClass('hide');
        hdnFrom = "";
        hdnTo = "";
    }

    if (filterCriteria == 'taxnumber') {
        $("#txtTaxNumber").val(null);
        $("#spnTaxNumber").addClass('hide');
        hdnTaxNumber = "";
    }

    if (filterCriteria == 'meter') {
        $("#txtMeter").val(null);
        $("#spnMeter").addClass('hide');
        hdnMeter = "";
    }

    InitializeData();
}

function initializeDatePicker() {
    $('.periodDate').datepicker({
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: "MM yyyy",
        language: __culture
    });
    var dt = new Date();
    $('#FromDate').datepicker("setDate", new Date(dt.getFullYear(), dt.getMonth(), dt.getDate() - 1));
    $('#ToDate').datepicker('setDate', new Date(dt.getFullYear(), dt.getMonth(), dt.getDate()));
}

function initDataTable() {
    $('#tblConsumptionRange').DataTable({
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
            $('#tblConsumptionRange tbody tr td').each(function () {
                if ($(this).find('#hdMeter').val() != undefined && $(this).index() == 1) {
                    this.setAttribute('title', $(this).find('#hdMeter').val());
                }
                if ($(this).find('#hdAccountName').val() != undefined && $(this).index() == 4) {
                    this.setAttribute('title', $(this).find('#hdAccountName').val());
                }
            });

            /* Apply the tooltips */
            $('#tblConsumptionRange tbody tr td').tooltip({
                container: "body",
                html: true
            });
        }
    });
}

function InitializeData() {
    $.ajax({
        type: "POST",
        url: ROOTPath + "/Reports/Report/ConsumptionByRange",
        data: {
            'fromDate': new Date($("#FromDate").datepicker('getDate')).toDateString(),
            'toDate': new Date($("#ToDate").datepicker('getDate')).toDateString(),
            'from': $("#txtFrom").val() ? parseFloat($("#txtFrom").val()) : null,
            'to': $("#txtTo").val() ? parseFloat($("#txtTo").val()) : null,
            'taxNumber': $("#txtTaxNumber").val(),
            'meter': $("#txtMeter").val(),
            'conditionFrom': $("#ddlConditionFrom").val(),
            'conditionTo': $("#ddlConditionTo").val(),
            'conditionType': $("input[name='ConditionType']:checked").val(),
            'pageIndex': 1,
            'pageSize': pageSize,
            'isLoadMore': false
        },
        success: function (response) {
            if (response.status) {
                hideLoading();
                $("#divConsumptionByRange").html("").html(response.data);
                initDataTable();
                rowCount = pageSize;
            }
            else {
                showAlert('error', response.data);
            }
        }
    });
}

function Print() {
    if (validateForm()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintConsumptionByRange",
            data: {
                'fromDate': new Date($("#FromDate").datepicker('getDate')).toDateString(),
                'toDate': new Date($("#ToDate").datepicker('getDate')).toDateString(),
                'from': $("#txtFrom").val() ? parseFloat($("#txtFrom").val()) : null,
                'to': $("#txtTo").val() ? parseFloat($("#txtTo").val()) : null,
                'taxNumber': $("#txtTaxNumber").val(),
                'meter': $("#txtMeter").val(),
                'conditionFrom': $("#ddlConditionFrom").val(),
                'conditionTo': $("#ddlConditionTo").val(),
                'conditionType': $("input[name='ConditionType']:checked").val(),
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

function validateForm() {

    var isvalid = true;
    $("#FromDate,#ToDate").removeClass("error");
    if ($("#FromDate").val() == undefined || $("#FromDate").val() == '') {
        $("#FromDate").addClass("error");
        isvalid = false;
    }
    if ($("#ToDate").val() == undefined || $("#ToDate").val() == '') {
        $("#ToDate").addClass("error");
        isvalid = false;
    }
    if (new Date($("#ToDate").datepicker('getDate')) < new Date($("#FromDate").datepicker('getDate'))) {
        showAlert('error', $("#ToDate").attr("data-compare-val-msg"));
        isvalid = false;
    }

    $("#ddlFrom").val($("#ddlConditionFrom").val());
    $("#ddlTo").val($("#ddlConditionTo").val());
    $("#ConditionTypeID").val($("input[name='ConditionType']:checked").val());

    if (isvalid) {
        hdnFrom = $("#txtFrom").val();
        hdnTo = $("#txtTo").val();
        hdnTaxNumber = $("#txtTaxNumber").val();
        hdnMeter = $("#txtMeter").val();
        $("#From").val(hdnFrom);
        $("#To").val(hdnTo);
        $("#TaxNumber").val(hdnTaxNumber);
        $("#Meter").val(hdnMeter);

        //
        $($("#FormattedFromDate")).val(new Date($("#FromDate").datepicker('getDate')).toDateString());
        $($("#FormattedToDate")).val(new Date($("#ToDate").datepicker('getDate')).toDateString());
        //
    }
    return isvalid;
}

function validateAdvanceSearch() {
    var isvalid = false;
    if ($("#txtTo").val() != "" && $("#txtFrom").val() == "")
        showAlert('error', rangeFromRequiredMsg);
    else if ($("#txtFrom").val() != "" && $("#txtTo").val() == "")
        showAlert('error', rangeToRequiredMsg);
    else if (
        ($("#txtTo").val() != "" && $("#txtFrom").val() != "")
        &&
        (parseInt($("#txtFrom").val()) > parseInt($("#txtTo").val()))
    )
        showAlert('error', compareRangeValidationMsg);
    else {
        isvalid = true;
        InitializeData();
        $("#advanceSearchModal").modal('hide');

        if ($("#txtFrom").val() != '' && $("#txtTo").val() != '') {
            $("#dvsearchinfo").removeClass('hide');
            $("#spnRange").removeClass('hide');
            $("#SearchText").text(consumptionRange + " : " + $("#txtFrom").val() + " - " + $("#txtTo").val());
        }
        else {
            $("#spnRange").addClass('hide');
            $("#SearchText").text('');
        }

        if ($("#txtTaxNumber").val() != '') {
            $("#dvsearchinfo").removeClass('hide');
            $("#spnTaxNumber").removeClass('hide');
            $("#TaxNumberSearchText").text(accountID + " : " + $("#txtTaxNumber").val());
        }
        else {
            $("#spnTaxNumber").addClass('hide');
            $("#TaxNumberSearchText").text('');
        }

        if ($("#txtMeter").val() != '') {
            $("#dvsearchinfo").removeClass('hide');
            $("#spnMeter").removeClass('hide');
            $("#MeterSearchText").text(meterNo + " : " + $("#txtMeter").val());
        }
        else {
            $("#spnMeter").addClass('hide');
            $("#MeterSearchText").text('');
        }
    }
    if (isvalid) {
        hdnFrom = $("#txtFrom").val();
        hdnTo = $("#txtTo").val();
        hdnTaxNumber = $("#txtTaxNumber").val();
        hdnMeter = $("#txtMeter").val();
    }
    return isvalid;
}

function loadAdvanceSearch() {
    $("#txtFrom").val('');
    $("#txtTo").val('');
    $("#txtTaxNumber").val('');
    $("#txtMeter").val('');
    SetResetFilterOption();
    $("#advanceSearchModal").modal('show');
    $("#txtFrom").focus();
}

function loadMoreConsumptionRange(element) {
    var currentQualitativePage = $(element).data('currentpage') ? $(element).data('currentpage') : 1;
    var newPage = currentQualitativePage + 1;    
    $.ajax({
        type: "POST",
        url: ROOTPath + "/Report/ConsumptionByRange",
        data: {
            'fromDate': new Date($("#FromDate").datepicker('getDate')).toDateString(),
            'toDate': new Date($("#ToDate").datepicker('getDate')).toDateString(),
            'from': $("#txtFrom").val() ? parseFloat($("#txtFrom").val()) : null,
            'to': $("#txtTo").val() ? parseFloat($("#txtTo").val()) : null,
            'taxNumber': $("#txtTaxNumber").val(),
            'meter': $("#txtMeter").val(),
            'conditionFrom': $("#ddlConditionFrom").val(),
            'conditionTo': $("#ddlConditionTo").val(),
            'conditionType': $("input[name='ConditionType']:checked").val(),
            'pageIndex': newPage,
            'pageSize': pageSize,
            'isLoadMore': true
        },
        success: function (response) {
            $(response.data).insertBefore("#tbodyConsumptionRange");
            $(element).data('currentpage', newPage);
            currentQualitativePage = newPage;
            rowCount = rowCount + response.RowCount;
            if (rowCount < $("#TotalRecord").val()) {
                $("#tbodyConsumptionRange").show();
            }
            else {
                $("#tbodyConsumptionRange").hide();
            }
        }
    });
}