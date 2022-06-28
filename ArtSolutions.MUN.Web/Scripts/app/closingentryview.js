$(document).ready(function () {
    enableViewMode("dvclosingentry");
});

function enableViewMode(parentdivId) {
    if (isViewMode == "True") {
        $("#" + parentdivId + " :submit").addClass("hide");
        $("#" + parentdivId + " input").prop("disabled", true);
        $("#" + parentdivId + " select").prop("disabled", true);
        $("#" + parentdivId + " textarea").prop("disabled", true);
        $("#" + parentdivId + " .actioninput").prop("disabled", true);
        $("#" + parentdivId + "  a.actioninput").addClass("disable-anchor");
    }
}
function VoidSuccessCallback(response) {
    if (response.status) {
        window.location.href = viewPageRedirectURL
    }
    else
        showAlert("error", response.message);
}

function Print(id) {

    $.ajax({
        type: "GET",
        async: false,
        url: ROOTPath + "/ClosingEntry/Print",
        data: { 'ID': id},
        success: function (response) {
            if (response.status) {
                hideLoading();
                var printWindow = window.open('', '_blank');
                printWindow.document.write(response.data);
                printWindow.document.close();
                setTimeout(function () { printWindow.print(); }, 20);
                printWindow.onfocus = function () { setTimeout(function () { printWindow.close(); }, 20); }
                return false;
            }
            else {
                showAlert('error', response.data);
                return false;
            }
        }
    });
    return true;
}
