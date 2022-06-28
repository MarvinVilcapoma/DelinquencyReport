$(document).ready(function () {
    enableViewMode("dvCollectionTemplate");
});

function enableViewMode(parentdivId) {
    if (isViewMode == "True") {
        $("#" + parentdivId + " :submit").addClass("hide");
        $("#" + parentdivId + " input").prop("disabled", true);
        $("#" + parentdivId + " select").prop("disabled", true);
        $("#" + parentdivId + " textarea").prop("disabled", true);        
    }
}