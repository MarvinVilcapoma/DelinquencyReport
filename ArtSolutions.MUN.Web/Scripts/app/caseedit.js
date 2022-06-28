
var from = 0;
$(document).ready(function () {

    $(".select2dropdown").select2({ width: '100%' });

    var saveResult = function (data) {
        from = data.from;
    };
    from = $("#hdnWeight").val();
    $("#Weight").ionRangeSlider({
        min: 1,
        max: 10,
        from: from,
        to: 10,
        onLoad: function (data) {
            saveResult(data);
        },
        onChange: saveResult,
        onFinish: saveResult
    });

    $("#btnEdit").click(function () {
        UpdateCaseData();
    });

    $.validator.addMethod('selectRule', function (value, element) {
        return parseInt($(element).val()) > 0; // return bool here if valid or not.
    }, requiredValidationMsg);

    $('#form1').validate();

    $("#btnCancel").click(function () {
        window.location = $("#hdnPreviousUrl").val();
    });
    $('.i-checks').iCheck({
        checkboxClass: 'icheckbox_square-blue'
    });
    $('.i-checks').iCheck('disable');
});

//Update case data
function UpdateCaseData() {
    var isFormValid = $("#form1").valid();
    if (isFormValid) {
        $.post(ROOTPath + "/Cases/Case/Edit", SetModel(), function (response) {
            if (!response.status) {
                showAlert("error", response.message);
            }
            else {
                window.location = $("#hdnPreviousUrl").val();
            }
        });
    }
    return isFormValid;
}

//Set Case Model for edit new case.
function SetModel(accountServices) {
    return {
        CaseID: $("#hdnCaseID").val(),
        AccountID: $("#hdnAccountID").val(),
        Name: $("#Name").val(),
        Refrence: $("#Refrence").val(),
        Note: $("#Note").val(),
        PriorityID: $("#PriorityID").val(),
        Weight: from == 0 ? 1 : from,
        OwnerID: $("#OwnerID").val(),
        AssignToID: $("#AssignToID").val(),
        PriorityName: $("#PriorityID option:selected").text(),
        AccountName: $("#AccountName").text()
    };
}