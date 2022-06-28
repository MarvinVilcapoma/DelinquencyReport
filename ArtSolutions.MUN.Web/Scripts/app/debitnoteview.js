
function ValidateWorkFlowStatus($this) {
    swal({
        title: swalTitleText,
        type: "warning",
        showCancelButton: true,
        cancelButtonText: cancelBtnText,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: $($this).text(),
        closeOnConfirm: true
    }, function (confirmed) {
        if (confirmed) {
            $.ajax({
                url: ROOTPath + "/DebitNote/Update",
                data: { 'ID': $("#DebitNoteID").val(), 'WorkFlowStatusID': $($this).val() },
                dataType: 'json',
                type: 'POST',
                success: function (result) {
                    if (result.status) {
                        showAlert("success", result.message);
                        window.location.reload();
                    }
                    else {
                        showAlert("error", result.message);
                    }
                }
            });
        }
        else {
            return false;
        }
    });

}
