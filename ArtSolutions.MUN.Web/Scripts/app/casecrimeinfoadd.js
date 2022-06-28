$(document).ready(function () {
    $("#btnCancelCrimInfo").click(function () {
        $(".e-arrow-sans-left").css("z-index", "0");
        $('#dvCrimeInfoModel').modal('hide');
    });
    $("#btnAddCrimInfo").click(function () {
        $.validator.addMethod('selectRule', function (value, element) {
            return parseInt($(element).val()) > 0; // return bool here if valid or not.
        }, requiredValidationMsg);

        var isFormValid = $('#frmCrimeInfo').valid();
        if (isFormValid) {
            SaveCrimInfo();
        }

        return isFormValid;
    });
    $('#dvCrimeInfoModel').on('shown.bs.modal', function () {
        GetCountry(function (responce) {
            var tempString = "";
            tempString += "<option value=\"-1\">" + dropDownSelectMessage + "</option>";
            for (var i in responce) {
                tempString += "<option value=\"" + responce[i].ID + "\">" + responce[i].Name + "</option>";
            }
            $("#ddlCRIMCountry").empty();
            $("#ddlCRIMCountry").html(tempString);
            $("#ddlCRIMCountry").trigger("change");

            $("#ddlCRIMState").empty();
            $("#ddlCRIMState").html("<option value=\"-1\">" + dropDownSelectMessage + "</option>");
            $("#ddlCRIMState").trigger("change");

            $("#txtCRIMTelephone").ForceNumericOnly();
        });

       
    })
    $("#ddlCRIMCountry").change(function () {
        var countryID = parseInt($(this).val());
        if (countryID > 0) {
            GetState(countryID, function (responce) {
                var tempString = "";
                tempString += "<option value=\"-1\">" + dropDownSelectMessage + "</option>";
                for (var i in responce) {
                    tempString += "<option value=\"" + responce[i].ID + "\">" + responce[i].Name + "</option>";
                }
                $("#ddlCRIMState").empty();
                $("#ddlCRIMState").html(tempString);
                $("#ddlCRIMState").trigger("change");
            });
        }
    });
    $("#txtCRIMReferenceDate").datepicker({
        todayHighlight: true,
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        format: __dateFormat,
        language: __culture,
    });
    $("#txtCRIMReferenceDate").datepicker('update', new Date());
    ForceNumeric();
});



function ForceNumeric() {
    $("#txtCRIMTelephone").ForceNumericOnly();
    $("#txtCRIMFax").ForceNumericOnly();
    $("#txtCRIMZipPostalCode").ForceNumericOnly();
}

function ClearCrimeInfoControl() {
    $("#txtCRIMFirstName,#txtCRIMLastName,#txtCRIMTelephone,#txtCRIMFax,#txtCRIMAddress1,#txtCRIMAddress2,#txtCRIMZipPostalCode,#txtCRIMComments").val("");
    $("#ddlCRIMCountry,#ddlCRIMState").val("-1");
    $("#ddlCRIMCountry,#ddlCRIMState").trigger("change");
}