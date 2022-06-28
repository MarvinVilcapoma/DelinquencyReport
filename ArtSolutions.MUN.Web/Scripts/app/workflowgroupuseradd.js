$(document).ready(function () {
    $('#GroupList').bootstrapDualListbox({
        selectorMinimalHeight: 230,
        showFilterInputs: false,
        infoText: false,
        moveAllLabel: moveAll,
        removeAllLabel: removeAll
    });
    $("#btnSave").click(function () {
        var tempArray = new Array();
        $("#GroupList").find("option:selected").each(function (i, elmnt) {
            tempArray.push({
                ID: parseInt($("#hdnGroupID").val()),
                UserID: $(elmnt).val(),
                DisplayName: $(elmnt).text()
            });
        });
        var model = {
            SecurityUserList: tempArray
        };
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            type: 'POST',
            url: (ROOTPath + "/Workflows/WorkflowGroup/UserAdd"),
            data: JSON.stringify(model),
            success: function (result) {
                if (result.Status) {
                    showAlert("success", result.Message);
                    setTimeout(function () {
                        window.location.href = ROOTPath + '/Workflows/Workflow/Editor/' + $("#hdnWorkflowID").val() + "?documentTypeID=" + $("#hdnDocumentTypeID").val();
                    }, 2000);
                }
                else {
                    showAlert("error", result.Message);
                }
            }
        });
    });
});