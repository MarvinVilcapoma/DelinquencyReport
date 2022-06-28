$(document).ready(function () {
    if ($(".select2dropdown").length)
        $(".select2dropdown").select2();
    initializeDatePicker();
    $('#tblJournalDetail').dataTable({
        "oLanguage": {
            "sEmptyTable": nodatamsg,
            oPaginate: {
                sPrevious: previous,
                sNext: next
            }
        },
        "pageLength": pagelen,
        "bFilter": false,
        "bInfo": false,
        "bLengthChange": false,
        "autoWidth": false,
        "ordering": false
    });
});

function initializeDatePicker() {
    $('.periodDate').datepicker({
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture,
    });
}

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
                url: ROOTPath + "/PostingProcess/Update",
                data: { 'ID': $("#PostingProcessID").val(), 'WorkFlowStatusID': $($this).val(), 'Ispost': ($($this).data('ispost')=='True') },
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