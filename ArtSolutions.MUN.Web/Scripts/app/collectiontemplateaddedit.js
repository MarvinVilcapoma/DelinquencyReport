$(window).on("load", function () {
    $('#txtTemplateName').focus();
});
$(document).ready(function () {
    InitializeDropDown();

    $("#dvPanelList").on("change", ".ddlaccountReceivable", function () {
        if ($(this).val() != "") {
            var Code = $(this).find("option:selected").data('code');
            var Name = $(this).find("option:selected").data('name');
            $(".ReceivableCode", $(this).closest('td')).val(Code);
            $(".ReceivableName", $(this).closest('td')).val(Name);
        }

    });

    $("#dvPanelList").on("change", ".ddlaccountRevenue", function () {
        if ($(this).val() != "") {
            var Code = $(this).find("option:selected").data('code');
            var Name = $(this).find("option:selected").data('name');
            $(".RevenueCode", $(this).closest('td')).val(Code);
            $(".RevenueName", $(this).closest('td')).val(Name);
        }

    });
    $("#dvPanelList").on("change", ".ddlaccountCredit", function () {
        if ($(this).val() != "") {
            var Code = $(this).find("option:selected").data('code');
            var Name = $(this).find("option:selected").data('name');
            $(".CreditAccountCode", $(this).closest('td')).val(Code);
            $(".CreditAccountName", $(this).closest('td')).val(Name);
        }

    });

    $("#dvPanelList").on("change", ".ddlCheckbook", function () {
        if ($(this).val() != "") {
            var Code = $(this).find("option:selected").data('code');
            var Name = $(this).find("option:selected").data('name');
            var FinancePaymentAccountID = $(this).find("option:selected").data('financepaymentaccountid');
            var FinanceAccountName = $(this).find("option:selected").data('financeaccountname');
            var CodeFriendly = $(this).find("option:selected").data('codefriendly');

            $(".CheckbookCode", $(this).closest('td')).val(Code);
            $(".CheckbookName", $(this).closest('td')).val(Name);
            $(".CashAccountID", $(this).closest('td')).val(FinancePaymentAccountID);
            $(".CashAccountCode", $(this).closest('td')).val(CodeFriendly);
            $(".CashAccountName", $(this).closest('td')).val(FinanceAccountName);
        }

    });

});
function InitializeDropDown() {
    $(".select2dropdown").select2({ width: '100%' });
}
function addDetailRow(yearId, targetDivId) {
    $.ajax({
        url: ROOTPath + "/CollectionTemplate/AddDetailRow",
        dataType: 'json',
        type: 'POST',
        data: $("#frmCollectionTemplate").serialize() + "&yearId=" + yearId,
        success: function (result) {
            if (result.status) {
                renderDetailList("dvPanelList", result);
            }
            else {
                showAlert("error", result.message);
            }
        }
    });
}
function removeDetailRow(sender) {
    $(sender).parents("tr").find(".removedRow").val(true);
    $(sender).parents("tr").hide();
}
function renderDetailList(target, result) {
    $("#" + target).html('');
    $("#" + target).html(result.data);
    InitializeDropDown();
}

function SaveTemplateSuccessCallback(response) {
    if (response.status == false) {
        showAlert("error", response.message);
    }
    else {
        resolveRedirectURL(response.actionType);
    }
}
function resolveRedirectURL(actionType) {
    if (actionType == 1) // Save OR Cancel
    {
        window.location = ROOTPath + "/Services/CollectionTemplate/List";
    }
    else if (actionType == 2)// Save & Add New
    {
        window.location = ROOTPath + "/Services/CollectionTemplate/Add";
    }
}
function GetGrantAccounts(sender, idx) {

    var grantId = $(sender).val();
    if (grantId > 0) {
        $.ajax({
            url: ROOTPath + "/CollectionTemplate/GetGrantAccounts",
            dataType: 'json',
            type: 'GET',
            data: { grantId: grantId, idx },
            success: function (result) {
                if (result.status) {
                    $(sender).parents("tr").find(".accountReceivable").html(result.accountreceivable);
                    $(sender).parents("tr").find(".accountRevenue").html(result.accountrevenue);
                    $(sender).parents("tr").find(".accountCredit").html(result.accountcredit);
                    $(sender).parents("tr").find(".Checkbook").html(result.checkbook);
                    InitializeDropDown();
                }
                else {
                    showAlert("error", result.message);
                }
            }
        });

        var SelectedText = $(sender).find("option:selected").text();
        if (SelectedText.length > 1) {
            var LstGrant = SelectedText.split('-')
            $(".GrantCode", $(sender).closest('td')).val(LstGrant[0].trim());
            $(".GrantName", $(sender).closest('td')).val(LstGrant[1].trim());
        }
    }
    else {
        // clear ddl        
        var ddlReveivable = $(sender).parents("tr").find(".accountReceivable select");
        var ddlRevenue = $(sender).parents("tr").find(".accountRevenue select");
        var ddlAccountCredit = $(sender).parents("tr").find(".accountCredit select");
        var ddlCheckbook = $(sender).parents("tr").find(".Checkbook select");

        $(ddlReveivable).find('option').not(':first').remove();
        $(ddlRevenue).find('option').not(':first').remove();
        $(ddlAccountCredit).find('option').not(':first').remove();
        $(ddlCheckbook).find('option').not(':first').remove();       
        $(ddlReveivable).val(null).trigger('change');        
        $(ddlRevenue).val(null).trigger('change');        
        $(ddlAccountCredit).val(null).trigger('change');        
        $(ddlCheckbook).val(null).trigger('change');
        $(".GrantCode", $(sender).closest('td')).val('');
        $(".GrantName", $(sender).closest('td')).val('');
    }
}
