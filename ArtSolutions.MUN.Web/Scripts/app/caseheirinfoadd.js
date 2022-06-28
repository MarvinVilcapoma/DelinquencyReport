$(document).ready(function () {
    $("#btnCancelHeirInfo").click(function () {
        $(".e-arrow-sans-left").css("z-index", "0");
        $('#dvHeirInfo').modal('hide');
    });
    $("#btnAddHeirInfo").click(function () {
        $.validator.addMethod('selectRule', function (value, element) {
            return parseInt($(element).val()) > 0; // return bool here if valid or not.
        }, requiredValidationMsg);

        var isFormValid = $('#frmHeirInfo').valid();
        if (isFormValid) {
            SaveHeirInfo();
        }
        return isFormValid;
    });
    $('#dvHeirInfo').on('shown.bs.modal', function () {
        GetCountry(function (result) {
            var tempString = "";
            tempString += "<option value=\"-1\">" + dropDownSelectMessage + "</option>";
            for (var i in result) {
                tempString += "<option value=\"" + result[i].ID + "\">" + result[i].Name + "</option>";
            }
            $("#ddlHeirCountry").empty();
            $("#ddlHeirCountry").html(tempString);
            $("#ddlHeirCountry").trigger("change");

            $("#ddlHeirState").empty();
            $("#ddlHeirState").html("<option value=\"-1\">" + dropDownSelectMessage + "</option>");
            $("#ddlHeirState").trigger("change");

            $("#txtHeirTelephone").ForceNumericOnly();
        });
    })
    $("#ddlHeirCountry").change(function () {
        var countryID = parseInt($(this).val());
        if (countryID > 0) {
            GetState(countryID, function (responce) {
                var tempString = "";
                tempString += "<option value=\"-1\">" + dropDownSelectMessage + "</option>";
                for (var i in responce) {
                    tempString += "<option value=\"" + responce[i].ID + "\">" + responce[i].Name + "</option>";
                }
                $("#ddlHeirState").empty();
                $("#ddlHeirState").html(tempString);
                $("#ddlHeirState").trigger("change");
            });
        }
    });
    ForceNumeric();


});

function GetCountry(callback) {
    $.get(ROOTPath + "/cases/case/GetCountries", null, function (responce) {
        callback(responce);
    });
}
function GetState(countryID, callback) {
    var data = {
        id: countryID
    };
    $.get(ROOTPath + "/cases/case/GetStates", data, function (responce) {
        callback(responce);
    });
}
function ForceNumeric() {
    $("#txtHeirTelephone").ForceNumericOnly();
    $("#txtHeirFax").ForceNumericOnly();
    $("#txtHeirZipPostalCode").ForceNumericOnly();
}

function ClearHeirInfoControl() {
    $("#txtHeirFirstName,#txtHeirLastName,#txtHeirTelephone,#txtHeirFax,#txtHeirAddress1,#txtHeirAddress2,#txtHeirZipPostalCode,#txtHeirComments").val("");
    $("#ddlHeirCountry,#ddlHeirState").val("-1");
    $("#ddlHeirCountry,#ddlHeirState").trigger("change");
}