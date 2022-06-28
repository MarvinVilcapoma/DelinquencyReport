
/*******************[START] - On DOM Ready Functions  *******************************/
$(window).on("load", function () {
    $('#Email').focus();
});


$("#btnSave").click(function (event) {
    if (Save() == false)
        return false;
});
$("#btnSaveNew").click(function (event) {
    if (Save() == false)
        return false;
});
function Save() {
    if ($("#form").valid()) {
        return true;
    }
    else
        return false;
}





function SchedulerEmailSuccessCallback(response) {
    if (response.status == false) {        
        showAlert("error", response.message);
    }
    else {
        resolveRedirectURL(response.actionType);
    }
}

function resolveRedirectURL(actionType) {
    if (actionType == 1 || actionType == 3) // Save OR Cancel
    {
        window.location = ROOTPath + "/SchedulerEmail/List";
    }
    else if (actionType == 2)// Save & Add New
    {
        window.location = ROOTPath + "/SchedulerEmail/Add";
    }
}
