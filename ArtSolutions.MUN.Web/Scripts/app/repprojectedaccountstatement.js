$(document).ready(function () {
    GetAccountForSelect('AccountId', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px');

    initializeDatePicker();
    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });

    if ($("#hdAccountID").val() > 0 && checkAccountIdInput() && $("#form").valid() && validateForm()) {
        InitializeData();
    }
});

function initializeDatePicker() {
    $(".periodDate").datepicker({
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture,
        startDate: new Date()
    });

    var dt = new Date();
    $('#txtDate').datepicker('setDate', dt);
    $("#Date").val(new Date($("#txtDate").datepicker('getDate')).toDateString());
}

$('.periodDate').keypress(function (e) {
    e.preventDefault();
});

var hdnDate;

$(document).on('click', '#btnGo', function () {
    if (checkAccountIdInput() && $("#form").valid() && validateForm()) {
        InitializeData();
    }
    return false;
});

$(window).resize(function () {
    if (checkAccountIdInput() && $("#form").valid() && validateForm()) {
        InitializeData();
    }
    return false;
});

function SetResetFilterOption() {    
    if (hdnDate != undefined) {
        $('#txtDate').datepicker('setDate', hdnDate);
        $("#Date").val(new Date($("#txtDate").datepicker('getDate')).toDateString());
    }
}

function clearSearch(filterCriteria) {    
    if (filterCriteria == 'tillDate') {
        $('#txtDate').datepicker('setDate', new Date());
        $("#Date").val(new Date($("#txtDate").datepicker('getDate')).toDateString());
        $("#spnDate").addClass('hide');
        hdnDate = $("#txtDate").val();
        $("#spCurrentDate").html(new Date($("#txtDate").datepicker('getDate')).toLocaleDateString(__culture));
    }
    InitializeData();
}

var projectedAccountStatementDataTable = null;

function initDataTable() {
    if ($(".projectedAccountStatementTable tbody tr.no-data").length == 1) {
        return false;
    }
    accountStatementDataTable = $('.projectedAccountStatementTable').DataTable({
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
            url: ROOTPath + "/Reports/Report/ProjectedAccountStatement",
            data: {
                'accountId': $("#AccountId").val(),
                'tillDate': ($("#txtDate").val() == "" ? null : new Date($("#txtDate").datepicker('getDate')).toDateString())
            },
            success: function (response) {
                if (response.status) {
                    hideLoading();
                    $("#divProjectedAccountStatement").html("").html(response.data);
                    initDataTable();
                    $('.projectedAccountStatementTable tbody tr td').tooltip({
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

function Print(data) {
    if (checkAccountIdInput() && $("#form").valid() && validateForm()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintProjectedAccountStatement",
            data:
            {
                'accountId': $("#AccountId").val(),
                'tillDate': ($("#txtDate").val() == "" ? null : new Date($("#txtDate").datepicker('getDate')).toDateString())
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

function validateForm() {
    var isvalid = true;
    if (isvalid) {     
        hdnDate = $("#txtDate").val();
    }
    return isvalid;
}

function validateAdvanceSearch() {
    var isvalid = true;
    InitializeData();
    $("#advanceSearchModal").modal('hide');

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
        hdnDate = $("#txtDate").val();
        $("#spCurrentDate").html(new Date($("#txtDate").datepicker('getDate')).toLocaleDateString(__culture));
        $("#Date").val(new Date($("#txtDate").datepicker('getDate')).toDateString());
    }
    return isvalid;
}

function loadAdvanceSearch() {  
    $('#txtDate').datepicker('setDate', new Date());
    $("#Date").val(new Date($("#txtDate").datepicker('getDate')).toDateString());
    SetResetFilterOption();
    $("#advanceSearchModal").modal('show');
}