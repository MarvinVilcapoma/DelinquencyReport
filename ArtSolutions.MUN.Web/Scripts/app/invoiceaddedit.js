$(window).on("load", function () {
    $('#AccountID').next().find('.select2-selection').focus();
});
$(document).ready(function () {
    InitializeDropDown();
    InitializeDate();
    if ($(".DetailAmount").length <= 0)
        addServiceTypeOption();

    $("#AccountID").on("change", function () {
        if ($(this).val() > 0) {
            $.ajax({
                url: ROOTPath + "/Invoice/IsSponsorByAccount",
                dataType: 'json',
                type: 'GET',
                data: { AccountId: $(this).val() },
                success: function (result) {
                    if (result.status) {
                        $("#IsSponsor").prop("checked", result.IsSponspor);
                    }
                    else {
                        showAlert("error", result.message);
                    }
                }
            });

        }
        else {
            $("#invoiceRows").html('');
            $("#IsSponsor").prop("checked", false);
            var amount = "0";
            $("#hdnTotal").val(amount);
            $("#spnTotal").text(CurrencyGlobalFormatWithText(GlobalConvertToDecimal(amount)));
        }
    });

    GetAccountForSelect('AccountID', null, '4,5,6', dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg);
});

function InitializeDropDown() {
    $(".select2dropdown").not('#AccountID').select2({ width: '100%' });
}
function InitializeDate() {
    $('.inputdate').datepicker({
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture
    });
}
function LoadCollectionItem(e) {
    var selectedItem = $(e).val();
    if (selectedItem == 1) {
        $.ajax({
            url: ROOTPath + "/Invoice/GetService",
            dataType: 'json',
            type: 'GET',
            data: { FormValue: $(e).closest('tr').find('input[name="InvoiceDetail.index"]').val() },
            success: function (result) {
                if (result.status) {
                    $(".itemtypelist", $(e).closest('tr')).html(result.service);
                    InitializeDropDown();
                    $('#InvoiceDetail_ServiceID').next().find('.select2-selection').focus();
                }
                else {
                    showAlert("error", result.message);
                }
            }
        });
        //$("#addInvoiceRow").attr("disabled", "disabled");
        if ($(".checkbooklist").length > 0) {
            $(".checkbooklist", $(e).closest("tr")).html('');
        }
        if ($(".checkbooklist:empty").length == $(".checkbooklist").length) {
            $(".CheckbookHead").remove();
            $(".checkbooklist").remove();
            $(".FotterSpan").attr("colspan", "2");
            $("td:lt(3)", $(".table-multi-row tbody tr")).removeClass("col-lg-2").addClass("col-lg-3");
            $("th:lt(3)", $(".itemTypeHead").closest('tr')).removeClass("col-lg-2").addClass("col-lg-3");
        }
        //if ($(".table-multi-row tbody tr").length > 1 && $(".checkbooklist").length <= 0) {
        //    $(".table-multi-row tbody tr:last").remove();
        //}
    }
    else if (selectedItem == 2) {
        $.ajax({
            url: ROOTPath + "/Invoice/GetGrantList",
            dataType: 'json',
            data: { AccountId: ($('#AccountID').val() || 0), date: $("#Date").val(), FormValue: $(e).closest('tr').find('input[name="InvoiceDetail.index"]').val() },
            type: 'GET',
            success: function (result) {
                if (result.status) {
                    $(".itemtypelist", $(e).closest('tr')).html(result.collectiontemlate);
                    InitializeDropDown();
                    $('#InvoiceDetail_CollectionTemplateID').next().find('.select2-selection').focus();
                }
                else {
                    showAlert("error", result.message);
                }
            }
        });
        $("#addInvoiceRow").removeAttr("disabled");

    }
}
function onInvoiceSaveSuccess(response) {
    if (!response.status) {
        showAlert("error", response.message);
    } else {

        if (response.actionType == 1) // Save && redirect to account view page
        {
            window.location.href = ROOTPath + '/Account/View?accountID=' + $("#AccountID").val() + '&accountType=' + response.AccountTypeID + '&groupId=3';
        }
        else if (response.actionType == 2)// Save & Add New
        {
            //window.location.href = ROOTPath + "/Collections/Invoice/Add";
            window.location.href = window.location;
        }
        else if (response.actionType == 3 || response.actionType == 4) {  // canecl && redirect to list page
            window.location.href = ROOTPath + "/Collections/Invoice";
        }

    }
}
function onInvoiceBegin() {
    var amount = $("#txtDetailAmount").val() == "" || $("#txtDetailAmount").val() == null ? 0 : $("#txtDetailAmount").val();
    if (amount == 0) {
        showAlert("warning", ValAmount);
        return false;
    }
}
function confirmInvoice(actionType) {
    var btnText = "";
    if (actionType == 3) {
        btnText = updateInvoice;
    }
    else {
        btnText = postInvoice;
    }
    if ($("#frmInvoice").valid()) {
        swal({
            title: swalTitleText,
            type: "warning",
            showCancelButton: true,
            cancelButtonText: cancelBtnText,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: btnText,
            closeOnConfirm: true
        }, function (confirmed) {
            if (confirmed)
                if (actionType == 1 || actionType == 4 || actionType == 3)
                    $("#btnSave").prop("type", "submit").click().prop("type", "button");
                else if (actionType == 2)
                    $("#btnSaveNew").prop("type", "submit").click().prop("type", "button");
                else
                    return false;
        });
    }
    else
        return false;
}
function UpdateTotalAmount() {
    var amount = 0;
    $(".DetailAmount").each(function (item, index) {
        if ($(this).val() != "") {
            amount = amount + GlobalConvertToDecimal($(this).val());
            $(this).val(CurrencyGlobalFormat(GlobalConvertToDecimal($(this).val())));

        }
    });
    $("#hdnTotal").val(CurrencyGlobalFormat(amount));
    $("#spnTotal").text(CurrencyGlobalFormatWithText(amount));
}
function confirmVoiding(invoiceId) {
    var objPrint = {};
    objPrint.Invoice = {};
    objPrint.Invoice.ID = invoiceId;
    objPrint.Invoice.RowVersion64 = $("#RowVersion64").val();
    swal({
        title: swalTitleText,
        type: "warning",
        showCancelButton: true,
        cancelButtonText: cancelBtnText,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: voidinvoice,
        closeOnConfirm: true
    }, function (confirmed) {
        if (confirmed) {
            $.ajax({
                url: ROOTPath + "/Invoice/Void",
                data: { 'model': objPrint },
                dataType: 'json',
                type: 'POST',
                success: function (result) {
                    if (result.status) {
                        window.location.href = ROOTPath + "/Collections/Invoice/View?ID=" + invoiceId;
                    }
                    else {
                        showAlert("error", result.message);
                    }
                }
            });
        }
        else
            return false;
    });
}

function VoidSuccessCallback(response) {
    if (response.status) {
        window.location.href = ROOTPath + "/Collections/Invoice/View?ID=" + response.ID;

    }
    else {
        showAlert("error", response.message);
    }
}

function addServiceTypeOption() {
    if ($("#addInvoiceRow").attr("disabled") == undefined) {
        $.ajax({
            url: ROOTPath + "/Invoice/GetServiceTypeOption",
            success: function (response) {
                if (response.status == undefined) {
                    $("#invoiceRows").append(response);
                    if ($(".checkbooklist").length > 0) {
                        var CheckbookHtml = '<td class="col-lg-2 checkbooklist"></td>';
                        $("td:eq(1)", $(".table-multi-row tbody tr:last")).after(CheckbookHtml);

                        $("td:lt(3)", $(".table-multi-row tbody tr")).removeClass("col-lg-3").addClass("col-lg-2");
                        $("th:lt(3)", $(".itemTypeHead").closest('tr')).removeClass("col-lg-3").addClass("col-lg-2");
                    }
                    InitializeDropDown();
                }
                else
                    showAlert("error", response.message);

            }
        });

    }
}

function removeRule(e) {
    $(e).closest('tr').remove();
    UpdateTotalAmount();
    $("#addInvoiceRow").removeAttr("disabled");
    //if ($(".checkbooklist").length == 0) {
    //    $(".CheckbookHead").remove();
    //    $(".checkbooklist").remove();
    //    $(".FotterSpan").attr("colspan", "2");
    //    $("td:lt(3)", $(".table-multi-row tbody tr")).removeClass("col-lg-3").addClass("col-lg-2");
    //    $("th:lt(3)", $(".itemTypeHead").closest('tr')).removeClass("col-lg-3").addClass("col-lg-2");
    //}


}

function FillFINGranName(e) {
    $(e).closest('td').find("input.FinGrantName").val($("option:selected", $(e)).text().trim());
    $(e).closest('td').find("input.FinGrantCode").val($("option:selected", $(e)).data('code'));
    $(e).closest('td').find("input.ReceivableAccountID").val($("option:selected", $(e)).data('receivableaccountid'));
    $(e).closest('td').find("input.ReceivableCode").val($("option:selected", $(e)).data('receivableaccountcode'));
    $(e).closest('td').find("input.ReceivableName").val($("option:selected", $(e)).data('receivableaccountname'));
    $(e).closest('td').find("input.RevenueAccountID").val($("option:selected", $(e)).data('revenueaccountid'));
    $(e).closest('td').find("input.RevenueCode").val($("option:selected", $(e)).data('revenueaccountcode'));
    $(e).closest('td').find("input.RevenueName").val($("option:selected", $(e)).data('revenueaccountname'));
    $(e).closest('td').find("input.CreditAccountID").val($("option:selected", $(e)).data('creditaccountid'));
    $(e).closest('td').find("input.CreditAccountCode").val($("option:selected", $(e)).data('creditaccountcode'));
    $(e).closest('td').find("input.CreditAccountName").val($("option:selected", $(e)).data('creditaccountname'));
    if ($(e).val() > 0) {
        $.ajax({
            url: ROOTPath + "/Invoice/GetCheckbookList",
            dataType: 'json',
            data: { grantId: $(e).val(), FormValue: $(e).closest('tr').find('input[name="InvoiceDetail.index"]').val() },
            type: 'GET',
            success: function (result) {
                if (result.status) {
                    if ($(".CheckbookHead").length == 0) {
                        var CheckbookHeadHtml = '<th class="col-lg-2 CheckbookHead">\
                   '+ CheckBook + '</th >';

                        $(".itemTypeHead").after(CheckbookHeadHtml);
                        $(".FotterSpan").attr("colspan", "3");
                    }
                    if ($(".checkbooklist", $(e).closest('tr')).length > 0)
                        $(".checkbooklist", $(e).closest('tr')).remove();
                    $("td:lt(2)", $(e).closest('tr')).removeClass("col-lg-3").addClass("col-lg-2");
                    $("th:lt(3)", $(".itemTypeHead").closest('tr')).removeClass("col-lg-3").addClass("col-lg-2");
                    $(".itemtypelist", $(e).closest('tr')).after(result.collectiontemlate);
                    InitializeDropDown();
                    $('#InvoiceDetail_CollectionTemplateID').next().find('.select2-selection').focus();
                }
                else {
                    showAlert("error", result.message);
                }
            }
        });
    }
    else {
        if ($(".checkbooklist").length > 1) {
            $(".checkbooklist", $(e).closest('tr')).html('');
        }
        else {
            $(".CheckbookHead").remove();
            $(".checkbooklist").remove();
            $(".FotterSpan").attr("colspan", "2");
        }

    }
}

function FillCheckbookName(e) {
    $(e).closest('td').find("input.CheckbookCode").val($("option:selected", $(e)).data('checkbookcode'));
    $(e).closest('td').find("input.CheckbookName").val($("option:selected", $(e)).data('checkbookname'));
    $(e).closest('td').find("input.CashAccountID").val($("option:selected", $(e)).data('cashaccountid'));
    $(e).closest('td').find("input.CashAccountCode").val($("option:selected", $(e)).data('cashaccountcode'));
    $(e).closest('td').find("input.CashAccountName").val($("option:selected", $(e)).data('cashaccountname'));
}