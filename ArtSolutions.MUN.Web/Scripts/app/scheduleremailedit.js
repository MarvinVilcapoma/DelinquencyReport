
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
        window.location = ROOTPath + "/SchedulerEmail/List";
    }
}
