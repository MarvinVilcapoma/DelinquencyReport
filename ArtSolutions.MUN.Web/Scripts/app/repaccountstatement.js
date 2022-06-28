$(document).ready(function () {
    GetAccountForSelect('AccountId', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px');

    $("#ddlRight").select2();

    initializeDatePicker();
    $(".closemodal").on("click", function () {
        SetResetFilterOption();
    });

    if ($("#hdAccountID").val() > 0 && checkAccountIdInput() && $("#form").valid() && validateForm()) {
        InitializeData();
    }

    $(document).on("change", "#ddlRight", function (e) {
        ClearAccountStatement();
    });

    $('#txtPeriod').on('keyup mousedown mouseleave', function () {
        var numbers = ['','1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12'];
        if (jQuery.inArray($(this).val(), numbers) == -1) {
            $('#txtPeriod').val('12');
        }
    });
});

function ClearAccountStatement() {
    $("#divAccountStatementReport").html("");
    $("#AccountServiceCollectionDetailIDs").val("");
    selectedAccountServiceCollectionDetailIDs = [];

    $('#btnAccountPaymentPlan').prop('disabled', false);
    $('#btnAccountPaymentPlan').addClass("hide");
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
var hdnPeriod;

$(document).on('click', '#btnGo', function () {
    if (checkAccountIdInput() && $("#form").valid() && validateForm()) {
        InitializeData();
    }
    return false;
});

$(window).resize(function () {
    $('#accountStatementTable').DataTable().columns.adjust().responsive.recalc();
});

function SetResetFilterOption() {
    if (hdnYear != undefined) {
        $("#txtYear").val(hdnYear);
    }
    if (hdnPeriod != undefined) {
        $("#txtPeriod").val(hdnPeriod);
    }
    if (hdnDate != undefined) {
        $('#txtDate').datepicker('setDate', hdnDate);

        if ($("#txtDate").val() == "" || $("#txtDate").val() == null || $("#txtDate").val() == undefined) {
            $("#Date").val("");
        }
        else {
            $("#Date").val(new Date($("#txtDate").datepicker('getDate')).toDateString());//test1
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
    if (filterCriteria == 'tillPeriod') {
        $("#txtPeriod").val(null);
        $("#spnPeriod").addClass('hide');
        hdnPeriod = "";
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

var accountStatementDataTable = null;

function initDataTable() {
    if ($("#accountStatementTable tbody tr.no-data").length == 1) {
        return false;
    }
    accountStatementDataTable = $('#accountStatementTable').DataTable({
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
    if ($("#AccountId").val() > 0) {
        $.ajax({
            type: "POST",
            url: ROOTPath + "/Reports/Report/AccountStatement",
            data: {
                'accountId': $("#AccountId").val(),
                'accountPropertyID': $("#ddlRight").val(),
                'tillYear': $("#txtYear").val(),
                'tillPeriod': $("#txtPeriod").val(),
                'tillDate': ($("#txtDate").val() == "" ? null : new Date($("#txtDate").datepicker('getDate')).toDateString()),
                'acountServiceCollectionDetailIDs': null,
                'isExport': false
            },
            success: function (response) {
                if (response.status) {

                    //Enable / Disable Account Payment Plan  Button                   
                    $("#btnAccountPaymentPlan").removeClass("hide");
                    if (response.accountPaymentPlanNames == null) {
                        $('#btnAccountPaymentPlan').prop('disabled', true);
                    }
                    else {
                        $('#btnAccountPaymentPlan').prop('disabled', false);
                    }
                    //

                    hideLoading();
                    $("#divAccountStatementReport").html("").html(response.data);
                    initDataTable();
                    $('#accountStatementTable tbody tr td').tooltip({
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
    else {
        $('#btnAccountPaymentPlan').prop('disabled', false);
        $('#btnAccountPaymentPlan').addClass("hide");
    }
}

function  Print(data) {
    if (checkAccountIdInput() && $("#form").valid() && validateForm(true)) {
        $.ajax({
            type: "POST",
            url: ROOTPath + "/Reports/Report/PrintAccountStatement",
            data:
            {
                'accountId': $("#AccountId").val(),
                'accountPropertyID': $("#ddlRight").val(),
                'tillYear': $("#txtYear").val(),
                'tillPeriod': $("#txtPeriod").val(),
                'tillDate': ($("#txtDate").val() == "" ? null : new Date($("#txtDate").datepicker('getDate')).toDateString()),
                'accountServiceCollectionDetailIDs': $("#AccountServiceCollectionDetailIDs").val(),
                'isExport': true
            },
            beforeSend: function () {
                showLoading();
            },
            success:  function (response) {
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

function GetRight() {
    if ($("#AccountId").val() > 0) {
        $.ajax({
            url: ROOTPath + "/AccountService/GetAccountPropertyRightByOwner",
            data: {
                ownerID: $("#AccountId").val(),
                serviceID: 0,
                fiscalYearID: 0,
                ID: null,
                forAccountStatementReport: true
            },
            success: function (data) {
                $("#ddlRight").empty();
                $.each(data, function (index, optiondata) {
                    $("#ddlRight").append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
                });
                $("#ddlRight").select2({ width: '100%' });
                $("#ddlRight option:first").val("");
            }
        });
    }
    else {
        $("#ddlRight").empty();
        $("#ddlRight").append("<option value='" + '' + "'>" + defaultSelectText + "</option>");
        $("#divAccountStatementReport").html(null);
        ClearAccountStatement();
    }
}

function validateForm(isExport) {
    selectedAccountServiceCollectionDetailIDs = [];

    var isvalid = true;
    if (isvalid) {
        hdnYear = $("#txtYear").val();
        $("#Year").val(hdnYear);
        hdnDate = $("#txtDate").val();
        $("#Date").val(hdnDate);
        hdnPeriod = $("#txtPeriod").val();
        $("#Period").val(hdnPeriod);
    }

    if (
        $('#accountStatementTable').find(".no-data").length == 0
        &&
        ($("#AccountId").val() > 0 && isExport == true && $("#accountStatementTable input[type=checkbox]:checked").length == 0)

    ) {
        isvalid = false;
        showAlert('error', selectAtLeastOneItemRequiredMsg);
    }

    return isvalid;
}

function validateAdvanceSearch() {
    var isvalid = true;
    InitializeData();
    $("#advanceSearchModal").modal('hide');

    if ($("#txtYear").val() != '') {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnYear").removeClass('hide');
        $("#YearSearchText").text(selectedYear + " : " + $("#txtYear").val());
    }
    else {
        $("#spnYear").addClass('hide');
        $("#YearSearchText").text('');
    }

    if ($('#txtPeriod').val() != '') {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnPeriod").removeClass('hide');
        $("#PeriodSearchText").text(selectedPeriod + " : " + $("#txtPeriod").val());
    } else {
        $("#spnPeriod").addClass('hide');
        $("#PeriodSearchText").text('');
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

    if (isvalid) {
        hdnYear = $("#txtYear").val();
        hdnPeriod = $("#txtPeriod").val();
        hdnDate = $("#txtDate").val();

        if ($("#txtDate").val() == "" || $("#txtDate").val() == null || $("#txtDate").val() == undefined) {
            $("#spCurrentDate").html(new Date().toLocaleDateString(__culture));
            $("#Date").val("");
        }
        else {
            $("#spCurrentDate").html(new Date($("#txtDate").datepicker('getDate')).toLocaleDateString(__culture));
            $("#Date").val(new Date($("#txtDate").datepicker('getDate')).toDateString());
        }
    }
    return isvalid;
}

function loadAdvanceSearch() {
    $("#txtYear").val('');
    $("#txtPeriod").val('');
    $('#txtDate').datepicker('setDate', '');
    $("#Date").val('');

    SetResetFilterOption();
    $("#advanceSearchModal").modal('show');
    $("#txtYear").focus();
}

$("#btnAccountPaymentPlan").bind("click", function () {
    if ($("#AccountId").val() > 0) {
        window.open(
            ROOTPath + "/Reports/Report/PaymentPlanByTaxpayer?accountId=" + $("#AccountId").val(),
            '_blank'
        );
    }
});

//------------------- Check Box -------------------------------------
var selectedAccountServiceCollectionDetailIDs = [];

function pushSelectedItems() {
    selectedAccountServiceCollectionDetailIDs = [];
    totalPrincipal = 0.0;
    totalCharges = 0.0;
    totalInterest = 0.0;
    totalDiscount = 0.0;
    totalPendingAmount = 0.0;

    $("#AccountServiceCollectionDetailIDs").val("");

    if ($(".collectionItem").length > 0) {
        $(".collectionItem").each(function (index) {

            if ($(this).is(':checked')) {
                selectedAccountServiceCollectionDetailIDs.push($(this).attr("dataAccountServiceCollectionDetailID"));

                if (GlobalConvertToDecimal($(this).attr("dataTotalPrincipal")) > 0)
                    totalPrincipal = totalPrincipal + GlobalConvertToDecimal($(this).attr("dataTotalPrincipal"));

                if (GlobalConvertToDecimal($(this).attr("dataTotalCharges")) > 0)
                    totalCharges = totalCharges + GlobalConvertToDecimal($(this).attr("dataTotalCharges"));

                if (GlobalConvertToDecimal($(this).attr("dataTotalInterest")) > 0)
                    totalInterest = totalInterest + GlobalConvertToDecimal($(this).attr("dataTotalInterest"));

                if (GlobalConvertToDecimal($(this).attr("dataTotalDiscount")) > 0)
                    totalDiscount = totalDiscount + GlobalConvertToDecimal($(this).attr("dataTotalDiscount"));

                if (GlobalConvertToDecimal($(this).attr("dataTotalPendingAmount")) > 0)
                    totalPendingAmount = totalPendingAmount + GlobalConvertToDecimal($(this).attr("dataTotalPendingAmount"));
            }
        });
    }

    $("#AccountServiceCollectionDetailIDs").val(selectedAccountServiceCollectionDetailIDs.join(','));
    $('#accountStatementTable tr:last').find("#TotalPrincipal").html(CurrencyGlobalFormatWithTextAndThousandSeparator(totalPrincipal));
    $('#accountStatementTable tr:last').find("#TotalCharges").html(CurrencyGlobalFormatWithTextAndThousandSeparator(totalCharges));
    $('#accountStatementTable tr:last').find("#TotalInterest").html(CurrencyGlobalFormatWithTextAndThousandSeparator(totalInterest));
    $('#accountStatementTable tr:last').find("#TotalDiscount").html(CurrencyGlobalFormatWithTextAndThousandSeparator(totalDiscount));
    $('#accountStatementTable tr:last').find("#TotalPendingAmount").html(CurrencyGlobalFormatWithTextAndThousandSeparator(totalPendingAmount));
    //
}

$(document).on("click", ".chkServiceAll", function (e) {
    var isChecked = $(this).is(":checked");
    var serviceID = $(this).attr("dataServiceID");

    if (isChecked) {
        $("#accountStatementTable").closest("table").find("input:checkbox." + serviceID).prop("checked", true);

        //For Select all parent and child
        if ($('.chkSelectedItem:checked').length == $('.chkSelectedItem').length) {
            $('#SelectedItemAll').prop('checked', true);
        }
        //

        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPrincipal').val(0);//Clear previous data
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalCharges').val(0);//Clear Previous 
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalInterest').val(0);//Clear Previous 
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalDiscount').val(0);//Clear Previous 
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPendingAmount').val(0);//Clear Previous 

        var previousTotalPrincipal = GlobalConvertToDecimal($("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPrincipal').val());
        var totalPrincipal = previousTotalPrincipal + GlobalConvertToDecimal($(this).attr("dataTotalPrincipal"));
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPrincipal').val(GlobalFormatForFixedDecimal(totalPrincipal, __decimalPoints));

        var previousTotalCharges = GlobalConvertToDecimal($("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalCharges').val());
        var totalCharges = previousTotalCharges + GlobalConvertToDecimal($(this).attr("dataTotalCharges"));
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalCharges').val(GlobalFormatForFixedDecimal(totalCharges, __decimalPoints));

        var previousTotalInterest = GlobalConvertToDecimal($("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalInterest').val());
        var totalInterest = previousTotalInterest + GlobalConvertToDecimal($(this).attr("dataTotalInterest"));
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalInterest').val(GlobalFormatForFixedDecimal(totalInterest, __decimalPoints));

        var previousTotalDiscount = GlobalConvertToDecimal($("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalDiscount').val());
        var totalDiscount = previousTotalDiscount + GlobalConvertToDecimal($(this).attr("dataTotalDiscount"));
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalDiscount').val(GlobalFormatForFixedDecimal(totalDiscount, __decimalPoints));

        var previousTotalPendingAmount = GlobalConvertToDecimal($("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPendingAmount').val());
        var totalPendingAmount = previousTotalPendingAmount + GlobalConvertToDecimal($(this).attr("dataTotalPendingAmount"));
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPendingAmount').val(GlobalFormatForFixedDecimal(totalPendingAmount, __decimalPoints));

    }
    else {
        $("#accountStatementTable").closest("table").find("input:checkbox." + serviceID).prop("checked", false);

        //For Select all parent and child
        if ($("#SelectedItemAll").is(":checked"))
            $("#SelectedItemAll").prop("checked", false);
        //

        var previousTotalPrincipal = GlobalConvertToDecimal($("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPrincipal').val());
        var totalPrincipal = previousTotalPrincipal - GlobalConvertToDecimal($(this).attr("dataTotalPrincipal"));
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPrincipal').val(GlobalFormatForFixedDecimal(totalPrincipal, __decimalPoints));

        var previousTotalCharges = GlobalConvertToDecimal($("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalCharges').val());
        var totalCharges = previousTotalCharges - GlobalConvertToDecimal($(this).attr("dataTotalCharges"));
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalCharges').val(GlobalFormatForFixedDecimal(totalCharges, __decimalPoints));

        var previousTotalInterest = GlobalConvertToDecimal($("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalInterest').val());
        var totalInterest = previousTotalInterest - GlobalConvertToDecimal($(this).attr("dataTotalInterest"));
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalInterest').val(GlobalFormatForFixedDecimal(totalInterest, __decimalPoints));

        var previousTotalDiscount = GlobalConvertToDecimal($("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalDiscount').val());
        var totalDiscount = previousTotalDiscount - GlobalConvertToDecimal($(this).attr("dataTotalDiscount"));
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalDiscount').val(GlobalFormatForFixedDecimal(totalDiscount, __decimalPoints));

        var previousTotalPendingAmount = GlobalConvertToDecimal($("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPendingAmount').val());
        var totalPendingAmount = previousTotalPendingAmount - GlobalConvertToDecimal($(this).attr("dataTotalPendingAmount"));
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPendingAmount').val(GlobalFormatForFixedDecimal(totalPendingAmount, __decimalPoints));

    }

    loadDetail();
    UpdateServiceTotal();
});

$(document).on("click", ".chkAll", function (e) {
    var isChecked = $(this).is(":checked");
    if (isChecked) {
        $("#accountStatementTable").closest("table").find("input[type='checkbox']").prop("checked", true);

        $("#accountStatementTable tr td.accountservice").each(function () {
            var serviceID = $(this).find("input:checkbox").attr("dataServiceID");

            $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPrincipal').val(0);//Clear Previous 
            $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalCharges').val(0);//Clear Previous 
            $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalInterest').val(0);//Clear Previous 
            $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalDiscount').val(0);//Clear Previous 
            $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPendingAmount').val(0);//Clear Previous 

            var totalPrincipal = GlobalConvertToDecimal($(this).find("input:checkbox").attr("dataTotalPrincipal"));
            $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPrincipal').val(GlobalFormatForFixedDecimal(totalPrincipal, __decimalPoints));
            $('#accountStatementTable tr.servicetotal.' + serviceID).find('td.totalPrincipal .spTotalPrincipal').html(CurrencyGlobalFormatWithTextAndThousandSeparator(totalPrincipal));

            var totalCharges = GlobalConvertToDecimal($(this).find("input:checkbox").attr("dataTotalCharges"));
            $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalCharges').val(GlobalFormatForFixedDecimal(totalCharges, __decimalPoints));
            $('#accountStatementTable tr.servicetotal.' + serviceID).find('td.totalCharges .spTotalCharges').html(CurrencyGlobalFormatWithTextAndThousandSeparator(totalCharges));

            var totalInterest = GlobalConvertToDecimal($(this).find("input:checkbox").attr("dataTotalInterest"));
            $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalInterest').val(GlobalFormatForFixedDecimal(totalInterest, __decimalPoints));
            $('#accountStatementTable tr.servicetotal.' + serviceID).find('td.totalInterest .spTotalInterest').html(CurrencyGlobalFormatWithTextAndThousandSeparator(totalInterest));

            var totalDiscount = GlobalConvertToDecimal($(this).find("input:checkbox").attr("dataTotalDiscount"));
            $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalDiscount').val(GlobalFormatForFixedDecimal(totalDiscount, __decimalPoints));
            $('#accountStatementTable tr.servicetotal.' + serviceID).find('td.totalDiscount .spTotalDiscount').html(CurrencyGlobalFormatWithTextAndThousandSeparator(totalDiscount));

            var totalPendingAmount = GlobalConvertToDecimal($(this).find("input:checkbox").attr("dataTotalPendingAmount"));
            $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPendingAmount').val(GlobalFormatForFixedDecimal(totalPendingAmount, __decimalPoints));
            $('#accountStatementTable tr.servicetotal.' + serviceID).find('td.totalPendingAmount .spTotalPendingAmount').html(CurrencyGlobalFormatWithTextAndThousandSeparator(totalPendingAmount));

        });
    }
    else {
        $("#accountStatementTable").closest("table").find("input[type='checkbox']").prop("checked", false);

        $("#accountStatementTable tr td.accountservice").each(function () {
            var serviceID = $(this).find("input:checkbox").attr("dataServiceID");

            $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPrincipal').val(0);
            $('#accountStatementTable tr.servicetotal.' + serviceID).find('td.totalPrincipal .spTotalPrincipal').html(CurrencyGlobalFormatWithTextAndThousandSeparator(0));

            $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalCharges').val(0);
            $('#accountStatementTable tr.servicetotal.' + serviceID).find('td.totalCharges .spTotalCharges').html(CurrencyGlobalFormatWithTextAndThousandSeparator(0));

            $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalInterest').val(0);
            $('#accountStatementTable tr.servicetotal.' + serviceID).find('td.totalInterest .spTotalInterest').html(CurrencyGlobalFormatWithTextAndThousandSeparator(0));

            $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalDiscount').val(0);
            $('#accountStatementTable tr.servicetotal.' + serviceID).find('td.totalDiscount .spTotalDiscount').html(CurrencyGlobalFormatWithTextAndThousandSeparator(0));

            $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPendingAmount').val(0);
            $('#accountStatementTable tr.servicetotal.' + serviceID).find('td.totalPendingAmount .spTotalPendingAmount').html(CurrencyGlobalFormatWithTextAndThousandSeparator(0));
        });
    }
    loadDetail();
});

function loadDetail() {
    pushSelectedItems();
}

function GetSelectedItems(e) {
    var accountServiceModel = {};
    var serviceID = $(e).attr("dataServiceID");

    if ($(e).is(':checked')) {
        //Service Checkbox
        if (serviceID > 0) {

            if (
                $("#accountStatementTable").closest("table").find("input:checkbox.collectionItem." + serviceID + ":checked").length
                ==
                $("#accountStatementTable").closest("table").find("input:checkbox.collectionItem." + serviceID).length
            ) {
                $("#accountStatementTable").closest("table").find("input:checkbox.chkServiceAll." + serviceID).prop("checked", true);
            }
        }
        //

        if ($('.chkSelectedItem:checked').length == $('.chkSelectedItem').length) {
            $('#SelectedItemAll').prop('checked', true);
        }

        var previousTotalPrincipal = GlobalConvertToDecimal($("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPrincipal').val());
        var totalPrincipal = previousTotalPrincipal + GlobalConvertToDecimal($(e).attr("dataTotalPrincipal"));
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPrincipal').val(GlobalFormatForFixedDecimal(totalPrincipal, __decimalPoints));

        var previousTotalCharges = GlobalConvertToDecimal($("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalCharges').val());
        var totalCharges = previousTotalCharges + GlobalConvertToDecimal($(e).attr("dataTotalCharges"));
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalCharges').val(GlobalFormatForFixedDecimal(totalCharges, __decimalPoints));

        var previousTotalInterest = GlobalConvertToDecimal($("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalInterest').val());
        var totalInterest = previousTotalInterest + GlobalConvertToDecimal($(e).attr("dataTotalInterest"));
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalInterest').val(GlobalFormatForFixedDecimal(totalInterest, __decimalPoints));

        var previousTotalDiscount = GlobalConvertToDecimal($("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalDiscount').val());
        var totalDiscount = previousTotalDiscount + GlobalConvertToDecimal($(e).attr("dataTotalDiscount"));
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalDiscount').val(GlobalFormatForFixedDecimal(totalDiscount, __decimalPoints));

        var previousTotalPendingAmount = GlobalConvertToDecimal($("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPendingAmount').val());
        var totalPendingAmount = previousTotalPendingAmount + GlobalConvertToDecimal($(e).attr("dataTotalPendingAmount"));
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPendingAmount').val(GlobalFormatForFixedDecimal(totalPendingAmount, __decimalPoints));

    }
    else {
        //Service Checkbox
        if (serviceID > 0) {
            if (
                $("#accountStatementTable").closest("table").find("input:checkbox.collectionItem." + serviceID + ":checked").length
                !=
                $("#accountStatementTable").closest("table").find("input:checkbox.collectionItem." + serviceID).length
            ) {
                $("#accountStatementTable").closest("table").find("input:checkbox.chkServiceAll." + serviceID).prop("checked", false);
            }
        }
        //
        if ($("#SelectedItemAll").is(":checked"))
            $("#SelectedItemAll").prop("checked", false);

        var previousTotalPrincipal = GlobalConvertToDecimal($("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPrincipal').val());
        var totalPrincipal = previousTotalPrincipal - GlobalConvertToDecimal($(e).attr("dataTotalPrincipal"));
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPrincipal').val(GlobalFormatForFixedDecimal(totalPrincipal, __decimalPoints));

        var previousTotalCharges = GlobalConvertToDecimal($("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalCharges').val());
        var totalCharges = previousTotalCharges - GlobalConvertToDecimal($(e).attr("dataTotalCharges"));
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalCharges').val(GlobalFormatForFixedDecimal(totalCharges, __decimalPoints));

        var previousTotalInterest = GlobalConvertToDecimal($("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalInterest').val());
        var totalInterest = previousTotalInterest - GlobalConvertToDecimal($(e).attr("dataTotalInterest"));
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalInterest').val(GlobalFormatForFixedDecimal(totalInterest, __decimalPoints));

        var previousTotalDiscount = GlobalConvertToDecimal($("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalDiscount').val());
        var totalDiscount = previousTotalDiscount - GlobalConvertToDecimal($(e).attr("dataTotalDiscount"));
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalDiscount').val(GlobalFormatForFixedDecimal(totalDiscount, __decimalPoints));

        var previousTotalPendingAmount = GlobalConvertToDecimal($("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPendingAmount').val());
        var totalPendingAmount = previousTotalPendingAmount - GlobalConvertToDecimal($(e).attr("dataTotalPendingAmount"));
        $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPendingAmount').val(GlobalFormatForFixedDecimal(totalPendingAmount, __decimalPoints));
    }

    UpdateServiceTotal();
    loadDetail();
}

function UpdateServiceTotal() {
    $("#accountStatementTable tr td.accountservice").each(function () {
        var serviceID = $(this).find("input:checkbox").attr("dataServiceID");

        var currencyTotalPrincipal = $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPrincipal').val();
        $('#accountStatementTable tr.servicetotal.' + serviceID).find('td.totalPrincipal .spTotalPrincipal').html(CurrencyGlobalFormatWithTextAndThousandSeparator(GlobalConvertToDecimal(currencyTotalPrincipal)));

        var currencyTotalCharges = $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalCharges').val();
        $('#accountStatementTable tr.servicetotal.' + serviceID).find('td.totalCharges .spTotalCharges').html(CurrencyGlobalFormatWithTextAndThousandSeparator(GlobalConvertToDecimal(currencyTotalCharges)));

        var currencyTotalInterest = $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalInterest').val();
        $('#accountStatementTable tr.servicetotal.' + serviceID).find('td.totalInterest .spTotalInterest').html(CurrencyGlobalFormatWithTextAndThousandSeparator(GlobalConvertToDecimal(currencyTotalInterest)));

        var currencyTotalDiscount = $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalDiscount').val();
        $('#accountStatementTable tr.servicetotal.' + serviceID).find('td.totalDiscount .spTotalDiscount').html(CurrencyGlobalFormatWithTextAndThousandSeparator(GlobalConvertToDecimal(currencyTotalDiscount)));

        var currencyTotalPendingAmount = $("#accountStatementTable").closest("table").find('tr.servicetotal.' + serviceID + ' #hdTotalPendingAmount').val();
        $('#accountStatementTable tr.servicetotal.' + serviceID).find('td.totalPendingAmount .spTotalPendingAmount').html(CurrencyGlobalFormatWithTextAndThousandSeparator(GlobalConvertToDecimal(currencyTotalPendingAmount)));

    });
}
//------------------------------------------------------------------