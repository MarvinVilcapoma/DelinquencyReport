
$(function () {
    $('.select2dropdown').not('#ddlAccount').select2({ width: '100%' });
});

$(document).ready(function () {

    if (ActionTypeID == 1) {
        GetAccountForSelect('ddlAccount', null, '4,5,6', dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg);
    }

    if (ActionTypeID < 3) // Add,Edit
    {
        $('#StartDate').datepicker({
            todayHighlight: true,
            keyboardNavigation: false,
            forceParse: false,
            calendarWeeks: true,
            autoclose: true,
            format: __dateFormat,
            language: __culture
        }).on("changeDate", function () {
            loadAccountServiceCollectionDetail();
        });

        if (ActionTypeID == 1)
            $('#StartDate').datepicker('update', new Date());
    }
    if (ActionTypeID > 1) // Edit=2,View=3
    {
        $("#divAccountPaymentDetail").show();

        if (ActionTypeID == 3) { //  View
            $("#btnCancel").focus();
            $("#frmAccountPaymentPlan select").prop("disabled", true);
            $("#frmAccountPaymentPlan input").prop("disabled", true);
        }
        else //  Edit
        {
            $("#frmAccountPaymentPlan select").not('#ddlServicePaymentPlan').prop("disabled", true);
            /*  $("#selectedItemstotalAmnestyAmount").val($("#AmnestyAmount").val());*/
        }

    }
});

function initializeDate(className, dateFormat) {
    $('.' + className).datepicker({
        defaultDate: new Date(),
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: dateFormat,
        language: __culture
    });
}

function ClearAccountPaymentPlanSummary(isClearDetail) {//11-Sep-2019
    if (isClearDetail == true) {
        $("#divAccountPaymentDetail").html(null);
        $("#divAccountPaymentDetail").hide();
    }
}

function AccountServiceCollectionDetailGet(callback) {
    var data = {
        actionType: ActionTypeID,
        accountId: $("#ddlAccount").val(),
        "selectedItemIds": ActionTypeID == 1 ? selectedItems.join(",") : $("#AccountServiceCollectionDetailIDs").val(),
        "applybyAmnesty": $("#ApplybyAmnesty").prop("checked"),
        forEdit: ActionTypeID == 2 ? true : false
    };
    $.post(ROOTPath + "/AccountPaymentPlan/GetAccountServiceCollectionDetail", data, function (response) {
        callback(response);
    });
}

function loadAccountServiceCollectionDetail() {
    if (ActionTypeID <= 2) {
        var accountID = 0;
        var serviceTypeID = 0;

        if ($("#ddlAccount").val() > 0)
            accountID = $("#ddlAccount").val();
        pushSelectedItems();
        AccountServiceCollectionDetailGet(function (responce) {
            if (responce.status == true) {
                loadPaymentPlan();

                if (ActionTypeID == 2) // 20
                    $("#collectiondetailRows").html(null);

                $("#collectiondetailRows").html(responce.data);

                if (responce.accountServiceIDs == "")
                    $("#btnCalculatePaymentPlan").prop("disabled", true);
                else
                    $("#btnCalculatePaymentPlan").prop("disabled", false);

                ClearAccountPaymentPlanSummary(true);//11-Sep-2019

                //13-May-2020 - CO-1556
                $(".divAccountPaymentPlan table").each(function () {
                    tableID = null;
                    sumTotal = 0.0;
                    sumPrincipal = 0.0;
                    sumPenalties = 0.0;
                    sumDebt = 0.0;
                    sumDiscount = 0.0;
                    sumPaidAmount = 0.0;

                    var tableID = $(this).attr("id");

                    if ($("." + tableID + " .collectionItem").length > 0) {
                        $("." + tableID + " .collectionItem").each(function (index) {
                            if ($(this).is(':checked')) {
                                if (GlobalConvertToDecimal($(this).attr("dataTotal")) > 0)
                                    sumTotal = sumTotal + GlobalConvertToDecimal($(this).attr("dataTotal"));
                                if (GlobalConvertToDecimal($(this).attr("dataTotalPrincipal")) > 0)
                                    sumPrincipal = sumPrincipal + GlobalConvertToDecimal($(this).attr("dataTotalPrincipal"));
                                if (GlobalConvertToDecimal($(this).attr("dataTotalPenalties")) > 0)
                                    sumPenalties = sumPenalties + GlobalConvertToDecimal($(this).attr("dataTotalPenalties"));
                                if (GlobalConvertToDecimal($(this).attr("dataDebtTotal")) > 0)
                                    sumDebt = sumDebt + GlobalConvertToDecimal($(this).attr("dataDebtTotal"));
                                if (GlobalConvertToDecimal($(this).attr("dataDiscountTotal")) > 0)
                                    sumDiscount = sumDiscount + GlobalConvertToDecimal($(this).attr("dataDiscountTotal"));
                                if (GlobalConvertToDecimal($(this).attr("dataTotalPaidAmount")) > 0)
                                    sumPaidAmount = sumPaidAmount + GlobalConvertToDecimal($(this).attr("dataTotalPaidAmount"));
                            }
                        });
                    }
                    $("." + tableID + " .totalRow").find(".sumTotal").html(CurrencyGlobalFormatWithText(sumTotal));
                    $("." + tableID + " .totalRow").find(".sumPrincipal").html(CurrencyGlobalFormatWithText(sumPrincipal));
                    $("." + tableID + " .totalRow").find(".sumPenalties").html(CurrencyGlobalFormatWithText(sumPenalties));
                    $("." + tableID + " .totalRow").find(".sumDebt").html(CurrencyGlobalFormatWithText(sumDebt));
                    $("." + tableID + " .totalRow").find(".sumDiscount").html(CurrencyGlobalFormatWithText(sumDiscount));
                    $("." + tableID + " .totalRow").find(".sumPaidAmount").html(CurrencyGlobalFormatWithText(sumPaidAmount));
                });
                //
            }
            else
                showAlert("error", responce.message);
        });
    }
    else if (ActionTypeID == 2) {
        ClearAccountPaymentPlanSummary(true);
    }
}

function loadPaymentPlan() {
    if ($("#ddlServicePaymentPlan").val() == "") {
        $.ajax({
            url: ROOTPath + "/AccountPaymentPlan/GetPaymentPlan",
            data: { "startDate": $("#StartDate").val() },
            success: function (data) {
                $("#ddlServicePaymentPlan").empty();
                $.each(data, function (index, optiondata) {
                    $("#ddlServicePaymentPlan").append("<option value='" + optiondata.Value + "'>" + optiondata.Text + "</option>");
                });
                $('#ddlServicePaymentPlan').val($("#ddlServicePaymentPlan option:first").val()).trigger('change');
                $.validator.unobtrusive.parse('#frmAccountPaymentPlan');
                $("#ddlServicePaymentPlan option:first").val("");
            }
        });
    }
    //}
}

function GetTotalDebtForSelectedItems(e) {
    var accServiceCollectionDetailIDs = "";
    var accServiceCollectionDetailID = "";
    var accServiceIDs = "";

    $("#divAccountPaymentDetail").hide();

    accServiceCollectionDetailIDs = $(e).attr("dataAccountServiceCollectionDetailIDs");
    accServiceCollectionDetailID = $(e).attr("dataserviceCollectionDetailID");
    accServiceIDs = $(e).attr("dataAccountServiceID");

    if ($(e).is(':checked')) {
        if ($('.chkSelectedItem:checked').length == $('.chkSelectedItem').length) {
            $('#SelectedItemAll').prop('checked', true);
        }
    }
    else {
        if ($("#SelectedItemAll").is(":checked"))
            $("#SelectedItemAll").prop("checked", false);
    }
    loadAccountServiceCollectionDetail();
}

function GetAccountPaymentPlanDetail() {
    if ($("#frmAccountPaymentPlan").valid()) {
        $.ajax({
            url: ROOTPath + "/AccountPaymentPlan/GetAccountPaymentPlanDetail",
            type: "POST",
            data: {
                paymentPlanId: $("#ddlServicePaymentPlan").val(),
                /*totalDebt: ActionTypeID == 1 ? $("#selectedItemstotalDebt").val() : $("#TotalDebt").val(),*/
                totalDebt: $("#selectedItemstotalDebt").val(),
                startDate: $("#StartDate").val(),
                InstalmentPercentage: GlobalFormatForFixedDecimal(GlobalConvertToDecimal($("#txtInstalmentPercentage").val()), __decimalPoints),
                InterestPercentage: GlobalFormatForFixedDecimal(GlobalConvertToDecimal($("#txtInterestPercentage").val()), 5),
                LateInterestPercentage: GlobalFormatForFixedDecimal(GlobalConvertToDecimal($("#txtLateInterestPercentage").val()), 5),
                Months: $("#Months").val(),
                accountId: $("#ddlAccount").val(),
                accountServiceIds: ActionTypeID == 1 ? $("#AccountServiceIDs").val() : $("#AccountPaymentPlanServiceIDs").val(),
                ServiceCollectionDetailIDs: ActionTypeID == 1 ? $("#AccountServiceCollectionDetailIDs").val() : $("#AccountPaymentPlanServiceCollectionDetailIDs").val(),
                forEdit: ActionTypeID == 2 ? true : false,
                applybyAmnesty: $("#ApplybyAmnesty").prop("checked")
            },
            success: function (responce) {
                if (responce.status == true) {
                    $("#divAccountPaymentDetail").html(responce.data);
                    $("#divAccountPaymentDetail").show();
                }
                else {
                    $("#divAccountPaymentDetail").hide();
                    showAlert("error", responce.message);
                }
            }
        });
    }
    else {
        $("#divAccountPaymentDetail").hide();
        return false;
    }
}

//Return String Value
function fixedDecimal(values) {
    values = Number(values.toString());
    return values.toFixed(__decimalPoints);
}

$("#ddlServicePaymentPlan").on("change", function () {

    ClearAccountPaymentPlanSummary(true);//11-Sep-2019

    if ($("#ddlServicePaymentPlan").val() == "" || $("#ddlServicePaymentPlan").val() == 0)
        $("#divAccountPaymentDetail").hide();
    else {
        $.ajax({
            url: ROOTPath + "/AccountPaymentPlan/GetAccountPaymentPlan",
            data: {
                paymentPlanId: $("#ddlServicePaymentPlan").val()
            },
            success: function (response) {
                if (response.status == true) {
                    $("#txtInstalmentPercentage").val(GlobalFormatForFixedDecimal(response.data.InstalmentPercentage, __decimalPoints));
                    $("#txtInterestPercentage").val(GlobalFormatForFixedDecimal(response.data.InterestPercentage, 5));
                    $("#txtLateInterestPercentage").val(GlobalFormatForFixedDecimal(response.data.LateInterestPercentage, 5));
                    $("#Months").val(response.data.Months);
                }
                else {
                    showAlert("error", response.message);
                }
            }
        });
    }
});

$("#btnCalculatePaymentPlan").click(function (event) {
    if ($("#frmAccountPaymentPlan").valid()) {

        if ($("#Months").val() < 1) {
            showAlert("error", MustBeEqualOrGreaterThan3Msg);
            return false;
        }
        var $selectedRow = $('table').find('tbody#tbAccountServiceCollectionDetail')
            .find('tr')
            .has('input[type=checkbox]:checked');

        if (ActionTypeID == 1 && $selectedRow.length == 0) {
            showAlert("error", selectAtLeastOneItemRequiredMsg);
            return false;
        }
        else {
            $("#divAccountPaymentDetail").show();
            GetAccountPaymentPlanDetail();
        }
    }
    else
        return false;
});

$("#btnSave").click(function (event) {
    if ($("#frmAccountPaymentPlan").valid()) {

        var $selectedRow = $('table').find('tbody#tbAccountServiceCollectionDetail')
            .find('tr')
            .has('input[type=checkbox]:checked');

        if (ActionTypeID == 1 && $selectedRow.length == 0) {
            showAlert("error", selectAtLeastOneItemRequiredMsg);
            return false;
        }

        if (!$("#btnCalculatePaymentPlan").is(":disabled") && $("#divAccountPaymentDetail").is(':hidden')) {
            showAlert("error", pleaseCalculatePaymentPlanBeforeSaveMsg);
            return false;
        }

        $("#AccountServiceCollectionDetailIDs").val($("#AccountServiceCollectionDetailIDs").val());
        /* $("#AmnestyAmount").val($("#selectedItemstotalAmnestyAmount").val());*/
    }
});

function UserSuccessCallback(response) {
    if (response.status == false)
        showAlert("error", response.message);
    else
        window.location = ROOTPath + "/AccountPaymentPlan/List";
}

function onItemSelectAll(obj) {
    accServiceCollectionDetailIDs = $(obj).closest('th').data('accountservicecollectionids');
    accServiceIDs = $(obj).closest('th').data('accountserviceid');

    if ($(obj).is(":checked")) {
        $(".chkSelectedItem").prop("checked", true);
    } else {
        $(".chkSelectedItem").prop("checked", false);
    }
}

var selectedItems = [];
function pushSelectedItems() {
    selectedItems = [];

    if ($(".collectionItem").length > 0) {
        $(".collectionItem").each(function (index) {
            if ($(this).is(':checked')) {
                selectedItems.push($(this).attr("dataserviceCollectionDetailID"));
            }
        });
    }
}

$(document).on("click", ".chkAll", function (e) {
    var isChecked = $(this).is(":checked");
    if (isChecked) {
        $(this).closest("table").find("input[type='checkbox']").prop("checked", true);
    }
    else {
        $(this).closest("table").find("input[type='checkbox']").prop("checked", false);
    }
    $("#divAccountPaymentDetail").hide();
    loadAccountServiceCollectionDetail();
});
$(document).on("click", "#btnDelete", function () {
    var confMessage = "";
    var textmessage = "";
    var AccountPaymentPlanID = $(this).attr("data-id");
    $("#txtReason").val('');
    $("#DeleteAccPaymentPlanNoteConfirmModal").attr("data-accountpaymentplanid", AccountPaymentPlanID);
    $("#DeleteAccPaymentPlanNoteConfirmModal").modal("show");

});

$("#DeleteAccPaymentPlanNoteConfirmModal").on("click", "#btnModalDelete", function () {
    var confMessage = "";
    var textmessage = "";
    var AccountPaymentPlanID = $("#DeleteAccPaymentPlanNoteConfirmModal").attr("data-accountpaymentplanid");
    if ($("#frmDeleteAccountPaymnetPlan").valid()) {
        swal({
            title: suremsg,
            text: textmessage,
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: confMessage,
            cancelButtonText: cancel,
            closeOnConfirm: true
        }, function () {
            DeleteRecord(AccountPaymentPlanID);
        });
    }
});

function DeleteRecord(AccountPaymentPlanID) {
    $.ajax({
        type: "GET",
        url: ROOTPath + '/Accounts/AccountPaymentPlan/Delete',
        data: { id: AccountPaymentPlanID, Reason: $("#DeleteAccPaymentPlanNoteConfirmModal #txtReason").val(), isWindowReload: true },
        success: function (data) {
            if (data.status == true) {
                window.location.href = ROOTPath + '/AccountPaymentPlan/List';
            }
            else
                showAlert('error', data.message);
        },
        error: function () { }
    });

}

$('#Months').on('change', function () {
    /* $("#btnCalculatePaymentPlan").click();*/
    ClearAccountPaymentPlanSummary(true);
});

$('#txtInterestPercentage').on('change', function () {
    /* $("#btnCalculatePaymentPlan").click();*/
    ClearAccountPaymentPlanSummary(true);
});

$('#txtLateInterestPercentage').on('change', function () {
    /* $("#btnCalculatePaymentPlan").click();*/
    ClearAccountPaymentPlanSummary(true);
});

$('#txtInstalmentPercentage').on('change', function () {
    /* $("#btnCalculatePaymentPlan").click();*/
    ClearAccountPaymentPlanSummary(true);
});