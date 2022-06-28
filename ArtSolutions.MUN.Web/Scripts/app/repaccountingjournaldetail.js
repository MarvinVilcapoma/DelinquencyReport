$(document).ready(function () {
    
    $('#btnGo').focus();
          
    $(".select2dropdown").not('#AccountIDs').select2();
    GetAccountForSelect('AccountIDs', null, null, dropDownSelectMessage, minimumInputSearchCharacterMessage, nodatamsg, null, true);
    
    initializeDatePicker();
    initDataTable();
    $(document).on('click', '#btnGo', function () {
        if (validateForm()) {
            InitializeData();
        }
    });

    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });
});

var hdnAccount;
var hdnDocumentType;
var hdnGrant;
var hdnCheckbook;
var hdnBankAccount;
var hdnAccountServiceType;
var hdnInvoiceItem;
var hdnPaymentPlanType;
var hdnBalanceFrom;
var hdnBalanceTo;

function SetResetFilterOption() {
    var PreviousSelectedData = [];
    if (hdnAccount != undefined && hdnAccount.length > 0) {
        PreviousSelectedData = hdnAccount.split(",");       
        $('#AccountIDs').val(PreviousSelectedData).trigger('change');
    }
    if (hdnDocumentType != undefined && hdnDocumentType.length > 0) {
        PreviousSelectedData = hdnDocumentType.split(",");      
        $('#DocumentTypeIDs').val(PreviousSelectedData).trigger('change');
    }
    if (hdnGrant != undefined && hdnGrant.length > 0) {
        PreviousSelectedData = hdnGrant.split(",");        
        $('#GrantIDs').val(PreviousSelectedData).trigger('change');
    }
    if (hdnCheckbook != undefined && hdnCheckbook.length > 0) {
        PreviousSelectedData = hdnCheckbook.split(",");       
        $('#CheckbookIDs').val(PreviousSelectedData).trigger('change');
    }
    if (hdnBankAccount != undefined && hdnBankAccount.length > 0) {
        PreviousSelectedData = hdnBankAccount.split(",");        
        $('#BankAccountIDs').val(PreviousSelectedData).trigger('change');
    }
    if (hdnAccountServiceType != undefined && hdnAccountServiceType.length > 0) {
        PreviousSelectedData = hdnAccountServiceType.split(",");        
        $('#AccountServiceTypeIDs').val(PreviousSelectedData).trigger('change');
    }
    if (hdnInvoiceItem != undefined && hdnInvoiceItem.length > 0) {
        PreviousSelectedData = hdnInvoiceItem.split(",");      
        $('#InvoiceTypeIDs').val(PreviousSelectedData).trigger('change');
    }
    if (hdnPaymentPlanType != undefined && hdnPaymentPlanType.length > 0) {
        PreviousSelectedData = hdnPaymentPlanType.split(",");       
        $('#PaymentPlanTypeIDs').val(PreviousSelectedData).trigger('change');
    }
    if (hdnBalanceFrom != undefined && hdnBalanceFrom.length > 0) {
        $("#txtBalanceFrom").val(hdnBalanceFrom);
        $("#txtBalanceTo").val(hdnBalanceTo);
    }
}

$(document).on('change', '#AccountIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#AccountIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});
$(document).on('change', '#DocumentTypeIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#DocumentTypeIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});
$(document).on('change', '#GrantIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#GrantIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});
$(document).on('change', '#CheckbookIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#CheckbookIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});
$(document).on('change', '#BankAccountIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#BankAccountIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});
$(document).on('change', '#AccountServiceTypeIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#AccountServiceTypeIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});
$(document).on('change', '#InvoiceTypeIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#InvoiceTypeIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});
$(document).on('change', '#PaymentPlanTypeIDs', function () {
    if ($(this).val() && $(this).val()[0] == 0) {
        $(this).val("").click();
        $("#PaymentPlanTypeIDs option:gt(0)").prop('selected', true);
        $(this).trigger('change');
    }
});

function clearSearch(filterCriteria) {

    if (filterCriteria == 'balance') {
        $("#txtBalanceFrom").val('');
        $("#txtBalanceTo").val('');
        $("#spnBalanceRange").addClass('hide');
        hdnBalanceFrom = "";
        hdnBalanceTo = "";
    }

    if (filterCriteria == 'documents') {      
        $('#DocumentTypeIDs').val(null).trigger('change');
        $("#spnSelectedDocument").addClass('hide');
        hdnDocumentType = "";
    }
    if (filterCriteria == 'accounts') {        
        $('#AccountIDs').val(null).trigger('change');
        $("#spnSelectedAccount").addClass('hide');
        hdnAccount = "";
    }    
    if (filterCriteria == 'grants') {       
        $('#GrantIDs').val(null).trigger('change');
        $("#spnSelectedGrant").addClass('hide');
        hdnGrant = "";
    }
    if (filterCriteria == 'checkbooks') {        
        $('#CheckbookIDs').val(null).trigger('change');
        $("#spnSelectedCheckbook").addClass('hide');
        hdnCheckbook = "";
    }
    if (filterCriteria == 'bankaccounts') {        
        $('#BankAccountIDs').val(null).trigger('change');
        $("#spnSelectedBankAccount").addClass('hide');
        hdnBankAccount = "";
    }
    if (filterCriteria == 'accountservicetypes') {        
        $('#AccountServiceTypeIDs').val(null).trigger('change');
        $("#spnSelectedAccountServiceType").addClass('hide');
        hdnAccountServiceType = "";
    }
    if (filterCriteria == 'invoicetypes') {        
        $('#InvoiceTypeIDs').val(null).trigger('change');
        $("#spnSelectedInvoiceType").addClass('hide');
        hdnInvoiceItem = "";
    }
    if (filterCriteria == 'paymentplantypes') {        
        $('#PaymentPlanTypeIDs').val(null).trigger('change');
        $("#spnSelectedPaymentPlanType").addClass('hide');
        hdnPaymentPlanType = "";
    }

    InitializeData();
}

function initDataTable() {
    $('#journalDetailTable').DataTable({
        "scrollY": "100vh",
        "scrollX": true,
        "scrollCollapse": true,        
        "paging": false,
        "ordering": false,
        "info": false,
        "searching": false,
        "lengthChange": false,
        responsive: false,
        autoWidth: false,
        fixedHeader: true,
        language: {
            "emptyTable": NoDataMsg
        }
    });
}

function InitializeData() {
    $.ajax({
        type: "POST",
        url: ROOTPath + "/Reports/Report/JournalDetail",
        data: {
            'startDate': new Date($("#StartDate").datepicker('getDate')).toDateString()
            , 'endDate': new Date($("#EndDate").datepicker('getDate')).toDateString()
            , 'DocumentTypeIDs': getDocumentTypeIDs()
            , 'accountIDs': getAccountIDs()
            , 'accountServiceTypeIDs': getAccountServiceTypeIDs()  
            , 'invoiceTypeIDs': getInvoiceItemText()
            , 'paymentPlanTypeIDs': getPaymentPlanTypeText()
            , 'bankAccountIDs': getBankAccountIDs()
            , 'grantIDs': getGrantIDs()
            , 'checkbookIDs': getCheckbookIDs()
            , 'balanceFrom': $("#txtBalanceFrom").val() ? parseFloat($("#txtBalanceFrom").val()) : null
            , 'balanceTo': $("#txtBalanceTo").val() ? parseFloat($("#txtBalanceTo").val()) : null
        },
        success: function (response) {
            if (response.status) {
                hideLoading();
                $("#divJournalReport").html("").html(response.data);
                initDataTable();
            }
            else {
                showAlert('error', response.data);
            }
        }
    });
}

function initializeDatePicker() {
    $('.periodDate').datepicker({
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture,
    });
    var dt = new Date();
    $('#StartDate').datepicker("setDate", new Date(dt.getFullYear(), dt.getMonth(), dt.getDate() - 1));
    $('#EndDate').datepicker('setDate', new Date(dt.getFullYear(), dt.getMonth(), dt.getDate()));
}

function validateForm() {
    var isvalid = false;
    if ($("#StartDate").val() == undefined || $("#StartDate").val() == '')
        showAlert('error', $("#StartDate").attr("data-required-msg"));
    else if ($("#EndDate").val() == undefined || $("#EndDate").val() == '')
        showAlert('error', $("#EndDate").attr("data-required-msg"));
    else if (new Date($("#EndDate").val()) < new Date($("#StartDate").val()))
        showAlert('error', $("#EndDate").attr("data-compare-val-msg"));
    else if ($("#txtBalanceTo").val() != "" && $("#txtBalanceFrom").val() == "")
        showAlert('error', balanceFromRequiredMsg);
    else if ($("#txtBalanceFrom").val() != "" && $("#txtBalanceTo").val() == "")
        showAlert('error', balanceToRequiredMsg);
    else if (($("#txtBalanceTo").val() != "" && $("#txtBalanceFrom").val() != "") && (parseInt($("#txtBalanceFrom").val()) > parseInt($("#txtBalanceTo").val())))
        showAlert('error', compareBalanceValidationMsg);
    else
        isvalid = true;
    if (isvalid) {
        hdnBalanceFrom = $("#txtBalanceFrom").val();
        hdnBalanceTo = $("#txtBalanceTo").val();       
        hdnAccount = getAccountIDs();
        hdnDocumentType = getDocumentTypeIDs();
        hdnGrant = getGrantIDs();
        hdnCheckbook = getCheckbookIDs();
        hdnBankAccount = getBankAccountIDs();
        hdnAccountServiceType = getAccountServiceTypeIDs();
        hdnInvoiceItem = getInvoiceItemIDs();
        hdnPaymentPlanType = getPaymentPlanTypeIDs();
        $("#BalanceFrom").val(hdnBalanceFrom);
        $("#BalanceTo").val(hdnBalanceTo);   
        $("#FilterAccountID").val(hdnAccount);
        $("#FilterDocumentTypeIDs").val(hdnDocumentType);
        $("#FilterGrantID").val(hdnGrant);
        $("#FilterCheckbookID").val(hdnCheckbook);
        $("#FilterBankAccountID").val(hdnBankAccount);
        $("#FilterAccountServiceTypeID").val(hdnAccountServiceType);        
        $("#FilterInvoiceTypeID").val(getInvoiceItemText);
        $("#FilterPaymentPlanTypeID").val(getPaymentPlanTypeText);
    }
    return isvalid;
}

function getAccountIDs() {
    var selectAccountList = [];
    if ($("#AccountIDs").select2('data').length) {
        $.each($("#AccountIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectAccountList += "," + item.id;
        });
    }
    var selectedAccountIDs = "";
    if (selectAccountList.length > 0)
        selectedAccountIDs = selectAccountList.substring(1, selectAccountList.length);
    return selectedAccountIDs;
}
function getAccountText() {
    var selectAccountList = [];
    if ($("#AccountIDs").select2('data').length) {
        $.each($("#AccountIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectAccountList += "," + item.text;
        });
    }
    var selectedAccountTexts = "";
    if (selectAccountList.length > 0)
        selectedAccountTexts = selectAccountList.substring(1, selectAccountList.length);
    return selectedAccountTexts;
}

function getDocumentTypeIDs() {
    var selectDocumentTypeList = [];
    if ($("#DocumentTypeIDs").select2('data').length) {
        $.each($("#DocumentTypeIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectDocumentTypeList += "," + item.id;
        });
    }
    var selectedDocumentTypeIDs = "";
    if (selectDocumentTypeList.length > 0)
        selectedDocumentTypeIDs = selectDocumentTypeList.substring(1, selectDocumentTypeList.length);
    return selectedDocumentTypeIDs;
}
function getDocumentTypeText() {
    var selectDocumentTypeList = [];
    if ($("#DocumentTypeIDs").select2('data').length) {
        $.each($("#DocumentTypeIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectDocumentTypeList += "," + item.text;
        });
    }
    var selectedDocumentTexts = "";
    if (selectDocumentTypeList.length > 0)
        selectedDocumentTexts = selectDocumentTypeList.substring(1, selectDocumentTypeList.length);
    return selectedDocumentTexts;
}

function getGrantIDs() {
    var selectGrantList = [];
    if ($("#GrantIDs").select2('data').length) {
        $.each($("#GrantIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectGrantList += "," + item.id;
        });
    }
    var selectedGrantIDs = "";
    if (selectGrantList.length > 0)
        selectedGrantIDs = selectGrantList.substring(1, selectGrantList.length);
    return selectedGrantIDs;
}
function getGrantText() {
    var selectGrantList = [];
    if ($("#GrantIDs").select2('data').length) {
        $.each($("#GrantIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectGrantList += "," + item.text;
        });
    }
    var selectedGrantTexts = "";
    if (selectGrantList.length > 0)
        selectedGrantTexts = selectGrantList.substring(1, selectGrantList.length);
    return selectedGrantTexts;
}

function getCheckbookIDs() {
    var selectCheckbookList = [];
    if ($("#CheckbookIDs").select2('data').length) {
        $.each($("#CheckbookIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectCheckbookList += "," + item.id;
        });
    }
    var selectedCheckbookIDs = "";
    if (selectCheckbookList.length > 0)
        selectedCheckbookIDs = selectCheckbookList.substring(1, selectCheckbookList.length);
    return selectedCheckbookIDs;
}
function getCheckbookText() {
    var selectCheckbookList = [];
    if ($("#CheckbookIDs").select2('data').length) {
        $.each($("#CheckbookIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectCheckbookList += "," + item.text;
        });
    }
    var selectedCheckbookTexts = "";
    if (selectCheckbookList.length > 0)
        selectedCheckbookTexts = selectCheckbookList.substring(1, selectCheckbookList.length);
    return selectedCheckbookTexts;
}

function getBankAccountIDs() {
    var selectBankAccountList = [];
    if ($("#BankAccountIDs").select2('data').length) {
        $.each($("#BankAccountIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectBankAccountList += "," + item.id;
        });
    }
    var selectedBankAccountIDs = "";
    if (selectBankAccountList.length > 0)
        selectedBankAccountIDs = selectBankAccountList.substring(1, selectBankAccountList.length);
    return selectedBankAccountIDs;
}
function getBankAccountText() {
    var selectBankAccountList = [];
    if ($("#BankAccountIDs").select2('data').length) {
        $.each($("#BankAccountIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectBankAccountList += "," + item.text;
        });
    }
    var selectedAccountTexts = "";
    if (selectBankAccountList.length > 0)
        selectedAccountTexts = selectBankAccountList.substring(1, selectBankAccountList.length);
    return selectedAccountTexts;
}

function getAccountServiceTypeIDs() {
    var selectAccountServiceTypeList = [];
    if ($("#AccountServiceTypeIDs").select2('data').length) {
        $.each($("#AccountServiceTypeIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectAccountServiceTypeList += "," + item.id;
        });
    }
    var selectedAccountServiceTypeIDs = "";
    if (selectAccountServiceTypeList.length > 0)
        selectedAccountServiceTypeIDs = selectAccountServiceTypeList.substring(1, selectAccountServiceTypeList.length);
    return selectedAccountServiceTypeIDs;
}
function getAccountServiceTypeText() {
    var selectAccountServiceTypeList = [];
    if ($("#AccountServiceTypeIDs").select2('data').length) {
        $.each($("#AccountServiceTypeIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectAccountServiceTypeList += "," + item.text;
        });
    }
    var selectedAccountServiceTypeTexts = "";
    if (selectAccountServiceTypeList.length > 0)
        selectedAccountServiceTypeTexts = selectAccountServiceTypeList.substring(1, selectAccountServiceTypeList.length);
    return selectedAccountServiceTypeTexts;
}

function getInvoiceItemIDs() {
    var selectInvoiceItemList = [];
    if ($("#InvoiceTypeIDs").select2('data').length) {
        $.each($("#InvoiceTypeIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectInvoiceItemList += "," + item.id;
        });
    }
    var selectedInvoiceItemIDs = "";
    if (selectInvoiceItemList.length > 0)
        selectedInvoiceItemIDs = selectInvoiceItemList.substring(1, selectInvoiceItemList.length);
    return selectedInvoiceItemIDs;
}
function getInvoiceItemText() {
    var selectInvoiceItemList = [];
    if ($("#InvoiceTypeIDs").select2('data').length) {
        $.each($("#InvoiceTypeIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectInvoiceItemList += "," + item.text;
        });
    }
    var selectedInvoiceItemTexts = "";
    if (selectInvoiceItemList.length > 0)
        selectedInvoiceItemTexts = selectInvoiceItemList.substring(1, selectInvoiceItemList.length);
    return selectedInvoiceItemTexts;
}

function getPaymentPlanTypeIDs() {
    var selectPaymentPlanTypeList = [];
    if ($("#PaymentPlanTypeIDs").select2('data').length) {
        $.each($("#PaymentPlanTypeIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectPaymentPlanTypeList += "," + item.id;
        });
    }
    var selectedPaymentPlanTypeIDs = "";
    if (selectPaymentPlanTypeList.length > 0)
        selectedPaymentPlanTypeIDs = selectPaymentPlanTypeList.substring(1, selectPaymentPlanTypeList.length);
    return selectedPaymentPlanTypeIDs;
}
function getPaymentPlanTypeText() {
    var selectPaymentPlanTypeList = [];
    if ($("#PaymentPlanTypeIDs").select2('data').length) {
        $.each($("#PaymentPlanTypeIDs").select2('data'), function (key, item) {
            if (item.id != 0)
                selectPaymentPlanTypeList += "," + item.text;
        });
    }
    var selectedPaymentPlanTypeTexts = "";
    if (selectPaymentPlanTypeList.length > 0)
        selectedPaymentPlanTypeTexts = selectPaymentPlanTypeList.substring(1, selectPaymentPlanTypeList.length);
    return selectedPaymentPlanTypeTexts;
}

function loadAdvanceSearch() {
    $("#txtBalanceFrom").val('');
    $("#txtBalanceTo").val('');  
    $('#AccountIDs').val(null).trigger('change');    
    $('#DocumentTypeIDs').val(null).trigger('change');    
    $('#GrantIDs').val(null).trigger('change');
    $('#CheckbookIDs').val(null).trigger('change');
    $('#BankAccountIDs').val(null).trigger('change');
    $('#AccountServiceTypeIDs').val(null).trigger('change');
    $('#InvoiceTypeIDs').val(null).trigger('change');
    $('#PaymentPlanTypeIDs').val(null).trigger('change');    

    SetResetFilterOption();
    if (!validateForm()) {
        return false;
    }
    else {
        $("#advanceSearchModal").modal('show');        
    }
}

function validateAdvanceSearch() {
    if (validateForm()) {

        var selectedAccountTexts = getDocumentTypeText();
        if (selectedAccountTexts != '') {
            $("#dvsearchinfo").removeClass('hide');
            $("#spnSelectedDocument").removeClass('hide');
            $("#DocumentSearchText").text(SelectedDocumentType + " : " + selectedAccountTexts);
        }
        else {
            $("#spnSelectedDocument").addClass('hide');
            $("#DocumentSearchText").text('');
        }

        selectedAccountTexts = getAccountText();
        if (selectedAccountTexts != '') {
            $("#dvsearchinfo").removeClass('hide');
            $("#spnSelectedAccount").removeClass('hide');
            $("#AccountIdSearchText").text(SelectedAccount + " : " + selectedAccountTexts);
        }
        else {
            $("#spnSelectedAccount").addClass('hide');
            $("#AccountIdSearchText").text('');
        }

        selectedAccountTexts = getBankAccountText();
        if (selectedAccountTexts != '') {
            $("#dvsearchinfo").removeClass('hide');
            $("#spnSelectedBankAccount").removeClass('hide');
            $("#BankAccountIdSearchText").text(SelectedBankAccount + " : " + selectedAccountTexts);
        }
        else {
            $("#spnSelectedBankAccount").addClass('hide');
            $("#BankAccountIdSearchText").text('');
        }

        selectedAccountTexts = getGrantText();
        if (selectedAccountTexts != '') {
            $("#dvsearchinfo").removeClass('hide');
            $("#spnSelectedGrant").removeClass('hide');
            $("#GrantIdSearchText").text(SelectedGrant + " : " + selectedAccountTexts);
        }
        else {
            $("#spnSelectedGrant").addClass('hide');
            $("#GrantIdSearchText").text('');
        }

        selectedAccountTexts = getCheckbookText();
        if (selectedAccountTexts != '') {
            $("#dvsearchinfo").removeClass('hide');
            $("#spnSelectedCheckbook").removeClass('hide');
            $("#CheckbookIdSearchText").text(SelectedCheckbook + " : " + selectedAccountTexts);
        }
        else {
            $("#spnSelectedCheckbook").addClass('hide');
            $("#CheckbookIdSearchText").text('');
        }

        selectedAccountTexts = getAccountServiceTypeText();
        if (selectedAccountTexts != '') {
            $("#dvsearchinfo").removeClass('hide');
            $("#spnSelectedAccountServiceType").removeClass('hide');
            $("#AccountServiceTypeIdSearchText").text(SelectedServiceType + " : " + selectedAccountTexts);
        }
        else {
            $("#spnSelectedAccountServiceType").addClass('hide');
            $("#AccountServiceTypeIdSearchText").text('');
        }

        selectedAccountTexts = getInvoiceItemText();
        if (selectedAccountTexts != '') {
            $("#dvsearchinfo").removeClass('hide');
            $("#spnSelectedInvoiceType").removeClass('hide');
            $("#InvoiceTypeIdSearchText").text(SelectedInvoiceItem + " : " + selectedAccountTexts);
        }
        else {
            $("#spnSelectedInvoiceType").addClass('hide');
            $("#InvoiceTypeIdSearchText").text('');
        }

        selectedAccountTexts = getPaymentPlanTypeText();
        if (selectedAccountTexts != '') {
            $("#dvsearchinfo").removeClass('hide');
            $("#spnSelectedPaymentPlanType").removeClass('hide');
            $("#PaymentPlanTypeIdSearchText").text(SelectedPaymentPlan + " : " + selectedAccountTexts);
        }
        else {
            $("#spnSelectedPaymentPlanType").addClass('hide');
            $("#PaymentPlanTypeIdSearchText").text('');
        }

        if ($("#txtBalanceFrom").val() != '' && $("#txtBalanceTo").val() != '') {
            $("#dvsearchinfo").removeClass('hide');
            $("#spnBalanceRange").removeClass('hide');
            $("#SearchText").text(BalanceRange + " : " + $("#txtBalanceFrom").val() + " - " + $("#txtBalanceTo").val());
        }
        else {
            $("#spnBalanceRange").addClass('hide');
            $("#SearchText").text('');
        }

        InitializeData();
        $("#advanceSearchModal").modal('hide');
    }
}