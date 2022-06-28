function initializeServiceException() {
    exceptionList = $.parseJSON($("#ServiceExceptionListJSON").val());
    $.each(exceptionList, function (key, value) {
        exceptionList[key].SequenceID = exceptionSeqId;
        exceptionSeqId++;
    });
}
function openExceptionPopup() {
    $("#dvException").html('');
    exceptionModel.ServiceExceptionList = exceptionList;
    $.ajax({
        type: "POST",
        url: ROOTPath + "/Service/GetException",
        data: { 'model': exceptionModel },
        success: function (data) {
            if (data.success == false) {
                showAlert("error", data.message);
            }
            else {

                $("#dvException").html(data);
                $.validator.unobtrusive.parse('#frmException');
                $("#exceptionModal").modal("show");
                $("#txtexpectionvalue").focus();

                if ($("#txtexpectionvalue").val() == 0) {
                    $("#txtexpectionvalue").val(null);
                }
            }
        }
    });
}
function addExceptionRow() {
    if (findIndex(exceptionList, "ExceptionValue", $("#txtexpectionvalue").val()) > -1) {
        showAlert("error", ExceptionValueDuplicated);
        $("#txtexpectionvalue").focus();
        return false;
    }
    //else if (parseFloat($("#txtexpectionvalue").val()) <= 0) {
    //    showAlert("error", ExceptionValueErrMsg);
    //    $("#txtexpectionvalue").focus();
    //    return false;
    //}
    else if (parseFloat($("#txtexpectionvalue").val()) <= 0 || parseFloat($("#txtexpectionvalue").val()) > 100) {
        showAlert("error", ExceptionValueErrMsg);
        $("#txtexpectionvalue").focus();
        return false;
    }
    else if ($("#frmException").valid()) {
        var obj = {};
        obj.ID = 0;
        obj.ExceptionValue = GlobalConvertToDecimal($("#txtexpectionvalue").val());
        obj.Description = $("#txtexceptiondescription").val();
        obj.SequenceID = exceptionSeqId;
        obj.AllowDelete = 1;
        exceptionList.push(obj);
        // Add Detail Row
        $.ajax({
            type: "POST",
            url: ROOTPath + "/Service/AddExceptionDetailrow",
            data: { 'model': obj },
            success: function (response) {
                $('#tbodyExceptionRow').append(response);
                $("#nodatarow").remove();
                $("#txtexpectionvalue").val('');
                $("#txtexceptiondescription").val('');
                $("#txtexpectionvalue").focus();
                $("#spExceptionAdd").addClass("hide");
                $("#spExceptionEdit").removeClass("hide")
                exceptionSeqId = exceptionSeqId + 1;
            }
        });
    }
    else {
        $("#txtexpectionvalue").focus();
        return false;
    }
}
function removeException(ele, seqId) {
    var allowdelete = $(ele).attr("data-allowdelete");
    if (allowdelete == 1) {
        var result = [];
        result = jQuery.grep(exceptionList, function (item) {
            return item.SequenceID == seqId;
        });
        if (result.length > 0) {
            exceptionList = jQuery.grep(exceptionList, function (item) {
                return item.SequenceID != seqId;
            });
            $(ele).parents("tr").remove();
        }
        else
            showAlert("error", DeleteFailedErrMsg);
    }
    else {
        showAlert("error", $(ele).attr("data-nodeletemsg"));
    }
}
