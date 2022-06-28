$(document).ready(function () {
    GetAccountForSelect('AccountId', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px');

    $("#ddlAccountPaymentPlan").select2();

    initializeDatePicker();
    $(".closemodal").on("click", function () {
        SetResetFilterOption();
    });

    if ($("#hdAccountID").val() > 0 && checkAccountIdInput() && $("#form").valid() && validateForm()) {
        InitializeData();
    }

    $(document).on("change", "#ddlAccountPaymentPlan", function (e) {
        ClearPaymentPlan();
    });
});

function ClearPaymentPlan() {
    $("#divPaymentPlanByTaxpayer").html("");
    $("#divPaymentPlanDetailByTaxpayer").html("");
    $("#RowNos").val("");
    selectedRowNos = [];
}

function initializeDatePicker() {
    $(".periodDate").datepicker({
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture,
        startDate: new Date()
    });
}

var hdnYear;
var hdnDate;
var hdnQuotasToCalculate;

$(document).on('click', '#btnGo', function () {
    if (checkAccountIdInput() && $("#form").valid() && validateForm()) {
        InitializeData();
    }
    return false;
});

$(window).resize(function () {
    $('.paymentPlanByTaxpayerTable').DataTable()
        .columns.adjust()
        .responsive.recalc();

    $('.paymentPlanDetailByTaxpayerTable').DataTable()
        .columns.adjust()
        .responsive.recalc();
});


function SetResetFilterOption() {
    if (hdnYear != undefined) {
        $("#txtYear").val(hdnYear);
    }
    if (hdnQuotasToCalculate != undefined) {
        $("#txtQuotasToCalculate").val(hdnQuotasToCalculate);
    }
    if (hdnDate != undefined) {
        $('#txtDate').datepicker('setDate', hdnDate);

        if ($("#txtDate").val() == "" || $("#txtDate").val() == null || $("#txtDate").val() == undefined) {
            $("#Date").val("");
        }
        else {
            $("#Date").val(new Date($("#txtDate").datepicker('getDate')).toDateString());
        }
    }
    if (hdnDate == undefined) {
        $('#txtDate').datepicker('setDate', "");
    }
}

function clearSearch(filterCriteria) {
    if (filterCriteria == 'tillYear') {
        $("#txtYear").val(null);
        $("#spnYear").addClass('hide');
        hdnYear = "";
    }
    if (filterCriteria == 'quotasToCalculate') {
        $("#txtQuotasToCalculate").val(null);
        $("#spnQuotasToCalculate").addClass('hide');
        hdnQuotasToCalculate = "";
    }
    if (filterCriteria == 'tillDate') {

        $('#txtDate').datepicker('setDate', '');
        if ($("#txtDate").val() == "" || $("#txtDate").val() == null || $("#txtDate").val() == undefined) {
            $("#Date").val("");
        }
        else {
            $("#Date").val(new Date($("#txtDate").datepicker('getDate')).toDateString());//test1
        }

        $("#spnDate").addClass('hide');
        hdnDate = $("#txtDate").val();

        if ($("#txtDate").val() == "" || $("#txtDate").val() == null || $("#txtDate").val() == undefined) {
            $("#spCurrentDate").html(new Date().toLocaleDateString(__culture));
        }
        else {
            $("#spCurrentDate").html(new Date($("#txtDate").datepicker('getDate')).toLocaleDateString(__culture));
        }
    }
    InitializeData();
}

var paymentPlanByTaxpayerDataTable = null;

function initDataTable(id) {
    if ($("." + id + "tbody tr.no-data").length == 1) {
        return false;
    }
    paymentPlanDataTable = $("." + id).DataTable({
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

function InitializeData(isAdvanceSearch) {
    if ($("#AccountId").val() > 0) {
        $.ajax({
            type: "POST",
            url: ROOTPath + "/Reports/Report/PaymentPlanByTaxpayer",
            data: {
                'accountId': $("#AccountId").val(),
                'accountPaymentPlanIDs': $("#ddlAccountPaymentPlan").val() == null ? null : $("#ddlAccountPaymentPlan").val().join(','),
                'tillYear': $("#txtYear").val(),
                'tillDate': ($("#txtDate").val() == "" ? null : new Date($("#txtDate").datepicker('getDate')).toDateString()),
                'quotasToCalculate': $("#txtQuotasToCalculate").val(),
                'rowNos': null,
                'isTermDetail': false
            },
            success: function (response) {
                //if (isAdvanceSearch && $("#txtQuotasToCalculate").val() > response.minAPager) {
                //    showAlert('error', quotasToCalculateValidationMsg);
                //}
                //else {
                    if (response.status) {
                        hideLoading();
                        ClearPaymentPlan();
                        $("#divPaymentPlanByTaxpayer").html("").html(response.data);
                        initDataTable("paymentPlanByTaxpayerTable");
                        $('.paymentPlanByTaxpayerTable tbody tr td').tooltip({
                            container: "body",
                            html: true
                        });

                        //Advance Search Code
                        $("#advanceSearchModal").modal('hide');

                        //Test Code
                        if ($("#txtYear").val() != '') {
                            $("#dvsearchinfo").removeClass('hide');
                            $("#spnYear").removeClass('hide');
                            $("#YearSearchText").text(selectedYear + " : " + $("#txtYear").val());
                        }
                        else {
                            $("#spnYear").addClass('hide');
                            $("#YearSearchText").text('');
                        }

                        if ($("#txtQuotasToCalculate").val() != '') {
                            $("#dvsearchinfo").removeClass('hide');
                            $("#spnQuotasToCalculate").removeClass('hide');
                            $("#QuotasToCalculateSearchText").text(selectedQuotasToCalculate + " : " + $("#txtQuotasToCalculate").val());
                        }
                        else {
                            $("#spnQuotasToCalculate").addClass('hide');
                            $("#QuotasToCalculateSearchText").text('');
                        }

                        if ($("#txtDate").val() != '') {
                            $("#dvsearchinfo").removeClass('hide');
                            $("#spnDate").removeClass('hide');
                            $("#DateSearchText").text(selectedInterestCalculationDate + " : " + $("#txtDate").val());
                        }
                        else {
                            $("#spnDate").addClass('hide');
                            $("#DateSearchText").text('');
                        }
                        //

                        hdnYear = $("#txtYear").val();
                        hdnDate = $("#txtDate").val();
                        hdnQuotasToCalculate = $("#txtQuotasToCalculate").val();

                        if ($("#txtDate").val() == "" || $("#txtDate").val() == null || $("#txtDate").val() == undefined) {
                            $("#spCurrentDate").html(new Date().toLocaleDateString(__culture));
                            $("#Date").val("");
                        }
                        else {
                            $("#spCurrentDate").html(new Date($("#txtDate").datepicker('getDate')).toLocaleDateString(__culture));
                            $("#Date").val(new Date($("#txtDate").datepicker('getDate')).toDateString());
                        }
                    }
                    else {
                        showAlert('error', response.data);
                    }
                //}
            }
        });
    }
}

function Print(data) {
    if (checkAccountIdInput() && $("#form").valid() && validateForm(true)) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintPaymentPlanByTaxpayer",
            data:
            {
                'accountId': $("#AccountId").val(),
                'accountPaymentPlanIDs': $("#ddlAccountPaymentPlan").val() == null ? null : $("#ddlAccountPaymentPlan").val().join(','),
                'tillYear': $("#txtYear").val(),
                'tillDate': ($("#txtDate").val() == "" ? null : new Date($("#txtDate").datepicker('getDate')).toDateString()),
                'quotasToCalculate': $("#txtQuotasToCalculate").val(),
                'rowNos': $("#RowNos").val(),
                'isTermDetail': null
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

function checkAccountIdInput() {
    var isvalid = true;
    if ($("#AccountId").val() == '') {
        $("#AccountId").focus();
    }
    return isvalid;
}

function validateForm(isExport) {
    selectedRowNos = [];

    var isvalid = true;
    if (isvalid) {
        hdnYear = $("#txtYear").val();
        $("#Year").val(hdnYear);
        hdnQuotasToCalculate = $("#txtQuotasToCalculate").val();
        $("#QuotasToCalculate").val(hdnQuotasToCalculate);
        hdnDate = $("#txtDate").val();
        $("#Date").val(hdnDate);
    }

    if (
        isExport == true
        &&
        $(".paymentPlanByTaxpayerTable .chkSelectedItem:checked").length == 0
    ) {
        isvalid = false;
        showAlert('error', selectAtLeastOneItemRequiredMsg);
    }

    return isvalid;
}

function validateAdvanceSearch() {
    var isvalid = true;
    if ($("#AccountId").val() > 0 && $("#txtQuotasToCalculate").val() == "0") {
        isvalid = false;
        showAlert('error', quotasToCalculateMustBeGreaterThanZeroMsg);
    }
    if (isvalid) {
        InitializeData(true);
    }
    if ($("#AccountId").val() == "") {
        $("#advanceSearchModal").modal('hide');
    }
    return isvalid;
}

function loadAdvanceSearch() {
    $("#txtYear").val('');
    $("#txtQuotasToCalculate").val('');
    $('#txtDate').datepicker('setDate', '');
    $("#Date").val('');

    SetResetFilterOption();
    $("#advanceSearchModal").modal('show');
    $("#txtYear").focus();
}

var selectedRowNos = [];

function pushSelectedItems() {
    selectedRowNos = [];

    if ($(".collectionItem").length > 0) {
        $(".collectionItem").each(function (index) {
            if ($(this).is(':checked')) {
                selectedRowNos.push($(this).attr("dataRowNo"));
            }
        });
    }

    $("#divPaymentPlanDetailByTaxpayer").html("");
    $("#RowNos").val("");

    var commaSeperatedRowNosList = '';
    var j;
    for (j = 0; j < selectedRowNos.length; ++j) {
        if (selectedRowNos[j] != undefined) {
            commaSeperatedRowNosList += ", " + selectedRowNos[j];
        }
    }

    $("#RowNos").val(commaSeperatedRowNosList);
}

$(document).on("click", ".chkAll", function (e) {
    var isChecked = $(this).is(":checked");
    if (isChecked) {
        $(".paymentPlanByTaxpayerTable").closest("table").find("input[type='checkbox']").prop("checked", true);
    }
    else {
        $(".paymentPlanByTaxpayerTable").closest("table").find("input[type='checkbox']").prop("checked", false);
    }
    loadDetail();
});

function loadDetail() {
    if ($("#AccountId").val() > 0)
        accountID = $("#ddlAccount").val();

    pushSelectedItems();
}

function GetSelectedItems(e) {
    if ($(e).is(':checked')) {
        if ($('.chkSelectedItem:checked').length == $('.chkSelectedItem').length) {
            $('#SelectedItemAll').prop('checked', true);
        }
    }
    else {
        if ($("#SelectedItemAll").is(":checked"))
            $("#SelectedItemAll").prop("checked", false);
    }

    loadDetail();
}

function GetPaymentPlanDetail() {

    if ($(".paymentPlanByTaxpayerTable .chkSelectedItem:checked").length == 0) {
        $("#divPaymentPlanDetailByTaxpayer").html("");
        showAlert('error', selectAtLeastOneItemRequiredMsg);
    }
    else {

        if ($("#AccountId").val() > 0) {
            $.ajax({
                type: "POST",
                url: ROOTPath + "/Reports/Report/PaymentPlanByTaxpayerTermDetail",
                data: {
                    'accountId': $("#AccountId").val(),
                    'accountPaymentPlanIDs': $("#ddlAccountPaymentPlan").val() == null ? null : $("#ddlAccountPaymentPlan").val().join(','),
                    'tillYear': $("#txtYear").val(),
                    'tillDate': ($("#txtDate").val() == "" ? null : new Date($("#txtDate").datepicker('getDate')).toDateString()),
                    'quotasToCalculate': $("#txtQuotasToCalculate").val(),
                    'rowNos': $("#RowNos").val(),
                    'isTermDetail': true
                },
                success: function (response) {
                    if (response.status) {
                        hideLoading();
                        $("#divPaymentPlanDetailByTaxpayer").html("").html(response.data);
                        initDataTable("paymentPlanDetailByTaxpayerTable");
                        $('.paymentPlanDetailByTaxpayerTable tbody tr td').tooltip({
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
    }
}

function GetPaymentPlan() {
    if ($("#AccountId").val() > 0) {
        $.ajax({
            url: ROOTPath + "/AccountPaymentPlan/GetAccountPaymentPlanByAccountID",
            data: {
                accountID: $("#AccountId").val(),
            },
            success: function (data) {
                $("#ddlAccountPaymentPlan").empty();
                $.each(data, function (index, optiondata) {
                    $("#ddlAccountPaymentPlan").append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
                });
                $("#ddlAccountPaymentPlan").select2({ width: '100%' });
                $("#divPaymentPlanByTaxpayer").html("");
                ClearPaymentPlan();
            }
        });
    }
    else {
        $("#ddlAccountPaymentPlan").empty();
        $("#divPaymentPlanByTaxpayer").html(null);
        ClearPaymentPlan();
    }
}