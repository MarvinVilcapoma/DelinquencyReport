$(document).ready(function () {
    
    $('#btnGo').focus();
    if ($(".select2dropdown").length)
        $(".select2dropdown").select2();

    initializeDatePicker();

    $(document).on('click', '#btnGo', function () {
        InitializeData();
    });
    $("#btnCancel").on("click", function () {
        SetResetFilterOption();
    });
});

var hdnDocumentType;
function SetResetFilterOption() {
    var PreviousSelectedData = [];
    if (hdnDocumentType != undefined && hdnDocumentType.length > 0) {
        PreviousSelectedData = hdnDocumentType.split(",");        
        $('#DocumentTypeIDs').val(PreviousSelectedData).trigger('change');
    }
}
function clearSearch(filterCriteria) {
    
    if (filterCriteria == 'documents') {       
        $('#DocumentTypeIDs').val(null).trigger('change');
        $("#spnSelectedDocument").addClass('hide');
        hdnDocumentType = "";
    }    
    InitializeData();    
}

function InitializeData()
{
    if (validateForm()) {
        $.ajax({
            type: "POST",
            url: ROOTPath + "/Reports/Report/Journal",
            data: {
                'startDate': new Date($("#StartDate").datepicker('getDate')).toDateString()
                , 'endDate': new Date($("#EndDate").datepicker('getDate')).toDateString()
                , 'DocumentTypeIDs': getDocumentTypeIDs()
            },
            success: function (response) {
                if (response.status) {
                    hideLoading();
                    $("#divJournalReport").html("").html(response.data);   
                    InitializeDataTable("tblJournal");
                }
                else {
                    showAlert('error', response.data);
                }
            }
        });
    }
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
    else
        isvalid = true;    
    if (isvalid) {
        hdnDocumentType = getDocumentTypeIDs();
        $("#FilterDocumentTypeIDs").val(hdnDocumentType);
    }
    return isvalid;
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
function loadAdvanceSearch() {    
    $('#DocumentTypeIDs').val(null).trigger('change');
    SetResetFilterOption();
    $("#advanceSearchModal").modal('show');
}

function validateAdvanceSearch() {
    var selectedAccountTexts=getDocumentTypeText();
    if (selectedAccountTexts != '') {
        $("#dvsearchinfo").removeClass('hide');
        $("#spnSelectedDocument").removeClass('hide');
        $("#DocumentSearchText").text(SelectedDocumentType + " : " + selectedAccountTexts);
    }
    else {
        $("#spnSelectedDocument").addClass('hide');
        $("#DocumentSearchText").text('');
    }
    InitializeData();
    $("#advanceSearchModal").modal('hide');
}

function InitializeDataTable(tableName) {
    $('#' + tableName).dataTable({       
        "processing": false,
        "bFilter": false,
        "bInfo": false,
        "bLengthChange": false,
        "autoWidth": false,        
        "ordering": false,
        "paging": false,
        "conditionalPaging": true,
        "scrollY": "100vh",
        "scrollX": true,
        "scrollCollapse": true,      
        "order": [[1, "asc"]],
        "columns": [
            { "width": "25%" },
            { "width": "10%" },
            { "width": "20%" },
            { "width": "25%" },
            { "width": "10%" },
            { "width": "10%" }
        ]
    });

}

function Print() {
    if (validateForm()) {
        $.ajax({
            type: "GET",
            async: false,
            url: ROOTPath + "/Reports/Report/PrintJournal",
            data: {
                'startDate': new Date($("#StartDate").datepicker('getDate')).toDateString()
                , 'endDate': new Date($("#EndDate").datepicker('getDate')).toDateString()
                , 'DocumentTypeIDs': getDocumentTypeIDs()
            },
            success: function (response) {
                if (response.status) {
                    hideLoading();
                    var printWindow = window.open('', '_blank');
                    printWindow.document.write(response.data);
                    printWindow.document.close();
                    setTimeout(function () { printWindow.print(); }, 20);
                    printWindow.onfocus = function () { setTimeout(function () { printWindow.close(); }, 20); };
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