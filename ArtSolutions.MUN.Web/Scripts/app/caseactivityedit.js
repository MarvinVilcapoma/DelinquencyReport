$(document).ready(function () {
    if (GetLocalStorage("ActivityMsg")) {
        showAlert("success", GetLocalStorage("ActivityMsg"));
        RemoveLocalStorage("ActivityMsg");
    }
    $('#dvEditHistoryModel').on('shown.bs.modal', function () {
        $("#btnEditHistory").click(function () {
            var isFormValid = $("#editActivityForm").valid();
            if (isFormValid) {
                $.post(ROOTPath + "/Cases/Case/EditHistory", CreatEditActivityModel(), function (response) {
                    if (response.Status) {
                        if (response.Message) {
                            SetLocalStorage("ActivityMsg", response.Message);
                        }
                        $('#dvEditHistoryModel').modal('hide');
                        setTimeout(function () {
                            window.location.reload(true);
                        }, 500);
                    }
                    else {
                        if (response.Message) {
                            showAlert("error", response.Message);
                        }
                    }
                });
            }
            return isFormValid;
        });
        $("#btnEditCancelHistory").click(function () {
            $('#dvEditHistoryModel').modal('hide');
        });
       
    });
    $('#ddlEditDate').datepicker({
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture,
    });
});
function CreatEditActivityModel() {
    return {
        ID: $("#hdnHistoryID").val(),
        CaseID: caseID,
        MeetingTypeID: $("#ddlEditMeetingTypes").val(),
        MeetingType: $("#ddlEditMeetingTypes option:selected").text(),
        Date: $('#ddlEditDate').val(),
        Notes: $("#ddlEditNote").val()
    };
}
function EditHistory(id,caseid) {
    var data = {
        id: id,
        caseID: caseid
    };
    $.get(ROOTPath + "/Cases/Case/EditHistory", data, function (response) {
        $("#ddlEditMeetingTypes").val(response.MeetingTypeID);
        $("#ddlEditMeetingTypes").trigger("change");
        $("#hdnHistoryID").val(response.ID);
        $("#ddlEditDate").datepicker('setDate', parseJsonDate(response.MeetingDate));
        $("#ddlEditNote").val(response.Notes);
        $('#dvEditHistoryModel').modal('show');
    });
}
function VieWHistory(id, caseid) {
    var data = {
        id: id,
        caseID: caseid
    };
    $.get(ROOTPath + "/Cases/Case/EditHistory", data, function (response) {
        $("#txtViewMeetingType").val(response.MeetingType);
        $("#txtViewDate").datepicker('setDate', parseJsonDate(response.MeetingDate));
        $("#txtViewNote").val(response.Notes);
        $('#dvViewHistoryModel').modal('show');
    });
}