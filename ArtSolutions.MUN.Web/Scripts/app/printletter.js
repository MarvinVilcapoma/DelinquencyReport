$(document).ready(function () {

    $(".select2dropdown").select2();

    $("#tblCases").dataTable({
        searching: false,
        paging: false,
        "ordering": false,
        "bPaginate": false,
        "bLengthChange": false,
        "bFilter": true,
        "bInfo": false,
        "bAutoWidth": false,
        destroy: true
    });

    $('.initialDate').datepicker({
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture,
    });

    $('.initialDate').datepicker('update', new Date());

    $.validator.addMethod('selectRule', function (value, element) {
        return parseInt($(element).val()) > 0; // return bool here if valid or not.
    }, requiredValidationMsg);


    $('#form').validate();

    $("#btnGenerate").click(function () {
        var isFormValid = $('#form').valid();
        if (isFormValid) {
            SaveLatter();
        }
        return isFormValid;
    });

    $("#btnCancel").click(function () {
        window.location = ROOTPath + "/Cases/case/List/" + getParameterByName("workflowid");
    });
});

function PriviewLatter(elemnt) {
    var id = $(elemnt).closest("tr").find("td").eq("0").text().trim();
    var isFormValid = $('#form').valid();
    if (isFormValid) {
        $.post(ROOTPath + "/Cases/Case/PreviewLatter", CreatePriviewModel(id), function (responce) {
            if (responce.Status) {
                window.location = ROOTPath + '/Cases/case/DownloadPreviewLatter/' + parseInt(responce.Message);
            }
        });
    }
    return isFormValid;
}

function SaveLatter() {

    $.post(ROOTPath + "/Cases/Case/PrintLetter", CreateModel(), function (responce) {
        if (!responce.Status) {
            showAlert("error", responce.Message);
        }
        else {
            window.location = ROOTPath + "/Cases/case/PrintLetterList";
        }
    });
}
function CreateModel() {
    var caseID = new Array();
    $("#tblCases").dataTable().api().rows().every(function (rowIdx, tableLoop, rowLoop) {
        var data = this.data();
        caseID.push(data[0]);
    });
    return {
        PrintTemplateID: $("#PrintTemplateID").val(),
        OutputFormat: $("#OutputFormat").val(),
        Comments: $("#Comments").val(),
        Date: $("#Date").val(),
        "Destinatary.Name": $("#Destinatary_Name").val(),
        "Destinatary.Position": $("#Destinatary_Position").val(),
        "Destinatary.Department": $("#Destinatary_Department").val(),
        DataSourceID: $("#DataSourceID").val(),
        CaseIDs: caseID.join(),
        CaseCount: caseID.length,
        Balance: parseFloat($("th[itag='thTotalAmount']").attr("data-totalAmount")).toFixed(2),
        StatuIdSource: getParameterByName("statuidsource"),
        StatuIdTarget: getParameterByName("statuidtarget"),
        WorkflowID: getParameterByName("workflowid")
    };
}
function CreatePriviewModel(id) {
    var caseID = new Array();
    caseID.push(id);
    return {
        PrintTemplateID: $("#PrintTemplateID").val(),
        OutputFormat: $("#OutputFormat").val(),
        Comments: $("#Comments").val(),
        Date: $("#Date").val(),
        "Destinatary.Name": $("#Destinatary_Name").val(),
        "Destinatary.Position": $("#Destinatary_Position").val(),
        "Destinatary.Department": $("#Destinatary_Department").val(),
        DataSourceID: $("#DataSourceID").val(),
        CaseIDs: caseID.join(),
        CaseCount: caseID.length,
        Balance: parseFloat($("th[itag='thTotalAmount']").attr("data-totalAmount")).toFixed(2),
        StatuIdSource: getParameterByName("statuidsource"),
        StatuIdTarget: getParameterByName("statuidtarget"),
        WorkflowID: getParameterByName("workflowid")
    };
}