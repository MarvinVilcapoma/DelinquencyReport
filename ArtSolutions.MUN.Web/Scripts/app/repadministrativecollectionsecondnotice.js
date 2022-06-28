$(document).ready(function () {
    $(".select2dropdown").select2();
    GetAccountForSelect('AccountId', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, '300px');
    $("#AccountId").focus();
    initializeDatePicker();
    $(".closePopUp").on("click", function () {
        SetResetFilterOption();
    });
});

function initializeDatePicker() {
    $(".notificationDate").datepicker({
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture
    });
}

$('.notificationDate').keypress(function (e) {
    e.preventDefault();
});

var hdnNotificationExpirationDate;
var hdnNotificationDate;
var hdnRepresentativesName;

$(document).on('click', '#btnGo', function () {
    if (checkAccountIdInput() && $("#form").valid() && validateForm()) {
        InitializeData();
    }
    return false;
});

function loadAdvanceSearch() {
    if ($("#AccountId").val() > 0 && $("#form").valid()) {
        $("#txtRepresentativesName").val('');
        $('#txtNotificationExpirationDate').val('');
        $('#txtNotificationDate').val('');

        SetResetFilterOption();
        $("#advanceSearchModal").modal('show');
        $("#txtRepresentativesName").focus();

        // Required Validation Clear
        $("#txtNotificationExpirationDate").removeClass("input-validation-error");
        $("#txtNotificationExpirationDate").next("span").addClass("field-validation-valid");
        $("#txtNotificationExpirationDate").next("span").hide();
        $("#txtNotificationDate").removeClass("input-validation-error");
        $("#txtNotificationDate").next("span").addClass("field-validation-valid");
        $("#txtNotificationDate").next("span").hide();
        $("#txtRepresentativesName").removeClass("input-validation-error");
        $("#txtRepresentativesName").next("span").addClass("field-validation-valid");
        $("#txtRepresentativesName").next("span").hide();
        //
    }
}

function SetResetFilterOption() {
    if (hdnNotificationExpirationDate != undefined) {
        $('#txtNotificationExpirationDate').datepicker('setDate', hdnNotificationExpirationDate);
        $("#NotificationExpirationDate").val(new Date($("#txtNotificationExpirationDate").datepicker('getDate')).toDateString());
    }
    if (hdnNotificationDate != undefined) {
        $('#txtNotificationDate').datepicker('setDate', hdnNotificationDate);
        $("#NotificationDate").val(new Date($("#txtNotificationDate").datepicker('getDate')).toDateString());
    }
    if (hdnRepresentativesName != undefined) {
        $("#txtRepresentativesName").val(hdnRepresentativesName);
    }
}

function checkAccountIdInput() {
    var isvalid = true;
    if ($("#AccountId").val() == '' || $("#AccountId").val() == null || $("#AccountId").val() == undefined) {
        $("#AccountId").focus();
    }
    return isvalid;
}

function validateAdvanceSearch() {
    if ($("#txtNotificationExpirationDate").val() == "" || $("#txtNotificationDate").val() == "" || $("#txtRepresentativesName").val().trim() == "") {

        if ($("#txtNotificationExpirationDate").val() == "") {
            $("#txtNotificationExpirationDate").addClass("input-validation-error");
            $("#txtNotificationExpirationDate").next("span").addClass("field-validation-error");
            $("#txtNotificationExpirationDate").next("span").show();
        }
        else {
            $("#txtNotificationExpirationDate").removeClass("input-validation-error");
            $("#txtNotificationExpirationDate").next("span").addClass("field-validation-valid");
            $("#txtNotificationExpirationDate").next("span").hide();
        }

        if ($("#txtNotificationDate").val() == "") {
            $("#txtNotificationDate").addClass("input-validation-error");
            $("#txtNotificationDate").next("span").addClass("field-validation-error");
            $("#txtNotificationDate").next("span").show();
        }
        else {
            $("#txtNotificationDate").removeClass("input-validation-error");
            $("#txtNotificationDate").next("span").addClass("field-validation-valid");
            $("#txtNotificationDate").next("span").hide();
        }

        if ($("#txtRepresentativesName").val().trim() == "") {
            $("#txtRepresentativesName").addClass("input-validation-error");
            $("#txtRepresentativesName").next("span").addClass("field-validation-error");
            $("#txtRepresentativesName").next("span").show();
        }
        else {
            $("#txtRepresentativesName").removeClass("input-validation-error");
            $("#txtRepresentativesName").next("span").addClass("field-validation-valid");
            $("#txtRepresentativesName").next("span").hide();
        }

        return false;
    }
    else {
        var isvalid = true;
        InitializeData();
        $("#advanceSearchModal").modal('hide');

        if (isvalid) {
            hdnRepresentativesName = $("#txtRepresentativesName").val().trim();
            hdnNotificationExpirationDate = $("#txtNotificationExpirationDate").val();
            hdnNotificationDate = $("#txtNotificationDate").val();
        }
        return isvalid;
    }
}

function validateForm() {

    var isvalid = true;

    if ($("#AccountId").val() == '' || $("#AccountId").val() == null || $("#AccountId").val() == undefined) {
        isvalid = false;
        $("#AccountId").focus();
        showAlert('error', $("#AccountId").attr("data-required-msg"));
    }
    else if ($("#txtNotificationExpirationDate").val() == undefined || $("#txtNotificationExpirationDate").val() == '') {
        isvalid = false;
        showAlert('error', $("#txtNotificationExpirationDate").attr("data-required-msg"));
    }
    else if ($("#txtNotificationDate").val() == undefined || $("#txtNotificationDate").val() == '') {
        isvalid = false;
        showAlert('error', $("#txtNotificationDate").attr("data-required-msg"));
    }
    else if ($("#txtRepresentativesName").val().trim() == undefined || $("#txtRepresentativesName").val().trim() == '') {
        isvalid = false;
        showAlert('error', $("#txtRepresentativesName").attr("data-required-msg"));
    }

    if (isvalid) {

        hdnRepresentativesName = $("#txtRepresentativesName").val().trim();
        $("#RepresentativesName").val(hdnRepresentativesName);

        hdnNotificationExpirationDate = $("#txtNotificationExpirationDate").val();
        $('#txtNotificationExpirationDate').datepicker('setDate', hdnNotificationExpirationDate);
        $("#NotificationExpirationDate").val(new Date($("#txtNotificationExpirationDate").datepicker('getDate')).toDateString());

        hdnNotificationDate = $("#txtNotificationDate").val();
        $('#txtNotificationDate').datepicker('setDate', hdnNotificationDate);
        $("#NotificationDate").val(new Date($("#txtNotificationDate").datepicker('getDate')).toDateString());
    }
    return isvalid;
}

var administrativeCollectionNoticeTable = null;

function initDataTable() {
    //if ($("#administrativeCollectionNoticeTable tbody tr.no-data").length == 1) {
    //    return false;
    //}
    //$('#accountPropertyListTable').DataTable({
    //    "paging": false,
    //    "ordering": false,
    //    "info": false,
    //    "searching": false,
    //    "lengthChange": false,
    //    responsive: false,
    //    autoWidth: false,
    //    fixedHeader: true,
    //    language: {
    //        "emptyTable": nodatamsg
    //    },
    //    "scrollY": "100vh",
    //    "scrollX": true,
    //    scrollCollapse: true
    //});
  
    if ($("#administrativeCollectionNoticeTable tbody tr.no-data").length == 1) {
        return false;
    }
    administrativeCollectionNoticeTable = $('#administrativeCollectionNoticeTable').DataTable({
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
        showLoading();
        $.ajax({
            type: "POST",
            url: ROOTPath + "/Reports/Report/AdministrativeCollectionSecondNotice",
            data: {
                'accountId': $("#AccountId").val(),
                'notificationExpirationDate': new Date($("#txtNotificationExpirationDate").datepicker('getDate')).toDateString(),
                'notificationDate': new Date($("#txtNotificationDate").datepicker('getDate')).toDateString(),
                'representativesName': $("#txtRepresentativesName").val().trim()
            },
            success: function (response) {
                if (response.status) {
                    hideLoading();
                    $("#divAdministrativeCollectionNoticeReport").html("").html(response.data);
                    initDataTable();
                    $('#administrativeCollectionNoticeTable tbody tr td').tooltip({
                        container: "body",
                        html: true
                    });
                }
                else {
                    showAlert('error', response.data);
                }
                hideLoading();
            }
        });
    }
}

function Print(data) {
    if (checkAccountIdInput() && $("#form").valid() && validateForm()) {
        $.ajax({
            type: "GET",
            url: ROOTPath + "/Reports/Report/PrintAdministrativeCollectionSecondNotice",
            data:
            {
                'accountId': $("#AccountId").val(),
                'notificationExpirationDate': new Date($("#txtNotificationExpirationDate").datepicker('getDate')).toDateString(),
                'notificationDate': new Date($("#txtNotificationDate").datepicker('getDate')).toDateString(),
                'representativesName': $("#txtRepresentativesName").val().trim()
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
