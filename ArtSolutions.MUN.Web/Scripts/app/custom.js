// Following code is added to set focus(mainly for select2) when modal is loaded.
// This will also resolve IE issue of Up Down Arrow key inside select2.
$.fn.modal.Constructor.prototype.enforceFocus = function () { };

function showProgress() { }
function hideProgress() { }
var $thisbtn;
function showButtonProgress(e, btnText) {
    var $thisbtn = $("#" + e);
    $thisbtn.html('<i class="fa fa-refresh fa-spin"></i> ' + $thisbtn.val()).attr('disabled', true);
}
function hideButtonProgress(e, btnText) {
    $thisbtn = $("#" + e);
    $thisbtn.html($thisbtn.val()).attr('disabled', false);
}
function showAlert(type, message, timeout) {
    toastr.clear();
    switch (type) {
        case "success":
            toastr.success(message, null, { "positionClass": "toast-top-center", "progressBar": true, });
            break;
        case "error":
            toastr.error(message, null, { "positionClass": "toast-top-center", "progressBar": true, });
            break;
        case "warning":
            toastr.warning(message, null, { "positionClass": "toast-top-center", "progressBar": true, });
            break;
        case "info":
            toastr.info(message, null, { "positionClass": "toast-top-center", "progressBar": true, "timeOut": timeout });
            break;
        default:
            break;
    }
}
function callAlert(response) {
    if (response != null && response != undefined && response.status != null && response.status != undefined) {
        if (response.status) type = "success";
        else type = "error";
        showAlert(type, response.message);
    }
}

function showLoading() {
    if ($("body #loading_container").length <= 0) {
        $('body').append("<div id=\"loading_container\" class=\"overlay\">\
                            <div class=\"overlay-content\">\
                                <div class='sk-spinner sk-spinner-circle'>\
                                    <div class='sk-circle1 sk-circle'></div>\
                                    <div class='sk-circle2 sk-circle'></div>\
                                    <div class='sk-circle3 sk-circle'></div>\
                                    <div class='sk-circle4 sk-circle'></div>\
                                    <div class='sk-circle5 sk-circle'></div>\
                                    <div class='sk-circle6 sk-circle'></div>\
                                    <div class='sk-circle7 sk-circle'></div>\
                                    <div class='sk-circle8 sk-circle'></div>\
                                    <div class='sk-circle9 sk-circle'></div>\
                                    <div class='sk-circle10 sk-circle'></div>\
                                    <div class='sk-circle11 sk-circle'></div>\
                                    <div class='sk-circle12 sk-circle'></div>\
                                </div>\
                            </div>\
                         </div>");
        $("#loading_container").show();
    } else {
        $("#loading_container").show();
    }

}

function hideLoading() {
    $("#loading_container").hide();
}
function CheckFormate(evt, t) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if ($('#type').val() == "NUMBER") {
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }
    else {
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123))
            return true;
        return false;
    }
}
function showAjaxAlert(xhr) {
    var message = xhr.responseText.match(/.*<title.*>([\s\S]*)<\/title>.*/);
    showAlert("error", (message.length > 0) ? message[1] : xhr.statusText);
}

$(".reportMenu li").on("click", function () {
    $('a', $(this)).tab('show');
});

$('#txtSearch').keypress(function (e) {
    var key = e.which;
    if (key == 13)  // the enter key code
    {
        $('#btnSearch').click();
        return false;
    }
});

function getSelectionStart(o) {
    if (o.createTextRange && document.selection) {
        var r = document.selection.createRange().duplicate()
        r.moveEnd('character', o.value.length)
        if (r.text == '') return o.value.length
        return o.value.lastIndexOf(r.text)
    } else return o.selectionStart
}

$(document).on("change", ".inputdecimalwithminus", function (evt) {

    //var number = /^[-+]?\d+[\.]?\d{0,2}?$/;
    var number = new RegExp("^[-+]?\\d+[\\" + __currencyDecimalSeparator + "]?\\d{0," + __decimalPoints + "}?$")
    if (!number.test($(this).val())) {
        $(this).val('');
        $(this).focus();
    }
});

$(document).on("focusout", ".Currencyinputdecimal,.Currencyinputdecimalwithminus", function (e) {
    if ($(this).val() == "") {
        $(this).val(CurrencyGlobalFormat(0));
    }
    if ($(e.relatedTarget).hasClass("closemodal")) {
        return true;
    }
    else {
        $(this).val(CurrencyGlobalFormat(GlobalConvertToDecimal($(this).val())));
    }
});

$(document).on("keypress", ".inputdecimalwithminus", function (evt) {
    var decimalSeparatorCharCode = __currencyDecimalSeparator == ',' ? 44 : 46;
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = $(this).val().split(__currencyDecimalSeparator);
    if (charCode != decimalSeparatorCharCode && charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 45) {
        return false;
    }

    if (number.length > 1 && charCode == decimalSeparatorCharCode) {
        return false;
    }

    //If we set maxlength attribute on textbox then take its max length otherwise take max length 12
    var maxLength = $(this).prop('maxLength'); //Fetch maxlength of textbox

    maxLength = maxLength > 0 ? maxLength : 12;

    //07-Jan-2019
    if (number.length == 1 && number[0].length < maxLength)
        return true;
    //

    if ((number[0].length >= (maxLength - (parseInt(__decimalPoints) + 1))) && charCode != decimalSeparatorCharCode && number.length == 1) //Restrict user based on maxlength attribute (here we subtract 3 for point and 2 digits after point)
        return false;

    var caratPos = getSelectionStart($(this)[0]);
    if (charCode == 45 && caratPos != 0)
        return false;
    if (charCode == 45 && $(this).val().split('-').length > 1)
        return false;

    var dotPos = $(this).val().indexOf(__currencyDecimalSeparator);

    if (caratPos > dotPos && dotPos > -1 && (number[1].length > parseInt(__decimalPoints) - 1)) {
        return false;
    }
    if ($(this).val().length >= maxLength)
        return false;

    return true;
});

$(document).on("keypress", ".Currencyinputdecimalwithminus", function (evt) {
    var decimalSeparatorCharCode = __currencyDecimalSeparator == ',' ? 44 : 46;
    var CommaSeparatorCharCode = __currencyDecimalSeparator == ',' ? 46 : 44;
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = $(this).val().split(__currencyDecimalSeparator);
    if (charCode != CommaSeparatorCharCode && charCode != decimalSeparatorCharCode && charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 45) {
        return false;
    }

    if (number.length > 1 && charCode == decimalSeparatorCharCode) {
        return false;
    }

    //If we set maxlength attribute on textbox then take its max length otherwise take max length 12
    var maxLength = $(this).prop('maxLength'); //Fetch maxlength of textbox

    maxLength = maxLength > 0 ? maxLength : 12;

    //07-Jan-2019
    if (number.length == 1 && number[0].length < maxLength)
        return true;
    //

    if ((number[0].length >= (maxLength - (parseInt(__decimalPoints) + 1))) && charCode != decimalSeparatorCharCode && number.length == 1) //Restrict user based on maxlength attribute (here we subtract 3 for point and 2 digits after point)
        return false;

    var caratPos = getSelectionStart($(this)[0]);
    if (charCode == 45 && caratPos != 0)
        return false;
    if (charCode == 45 && $(this).val().split('-').length > 1)
        return false;

    var dotPos = $(this).val().indexOf(__currencyDecimalSeparator);

    if (caratPos > dotPos && dotPos > -1 && (number[1].length > parseInt(__decimalPoints) - 1)) {
        return false;
    }
    if ($(this).val().length >= maxLength)
        return false;

    return true;
});


$(document).on("keypress", ".inputdecimalForPercentageFix", function (evt) {
    var decimalSeparatorCharCode = __currencyDecimalSeparator == ',' ? 44 : 46;
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = $(this).val().split(__currencyDecimalSeparator);
    if ((charCode != decimalSeparatorCharCode && charCode > 31 && (charCode < 48 || charCode > 57))) {
        return false;
    }

    if ((number.length > 1 && charCode == decimalSeparatorCharCode)) {
        return false;
    }

    var attr = $(this).attr('maxLength');
    if (typeof attr == undefined) {
        $(this).attr('maxLength', "12")
    }

    //If we set maxlength attribute on textbox then take its max length otherwise take max length 12
    var maxLength = $(this).prop('maxLength'); //Fetch maxlength of textbox

    maxLength = maxLength > 0 ? maxLength : 12;

    //31-Dec-2018 
    if (number.length == 1 && number[0].length < maxLength)
        return true;
    //

    if ((number[0].length >= (maxLength - (parseInt(5) + 1))) && charCode != decimalSeparatorCharCode && number.length == 1) //Restrict user based on maxlength attribute (here we subtract 3 for point and 2 digits after point)
        return false;

    var caratPos = getSelectionStart($(this)[0]);
    var dotPos = $(this).val().indexOf(__currencyDecimalSeparator);

    if (caratPos > dotPos && dotPos > -1 && (number[1].length > parseInt(5) - 1)) {
        return false;
    }

    //if ($(this).val().length >= maxLength) ChangeSet = 12858
    //    return false;

    return true;
});

$(document).on("keypress", ".inputdecimal", function (evt) {
    var decimalSeparatorCharCode = __currencyDecimalSeparator == ',' ? 44 : 46;
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = $(this).val().split(__currencyDecimalSeparator);
    if ((charCode != decimalSeparatorCharCode && charCode > 31 && (charCode < 48 || charCode > 57))) {
        return false;
    }

    if ((number.length > 1 && charCode == decimalSeparatorCharCode)) {
        return false;
    }

    var attr = $(this).attr('maxLength');
    if (typeof attr == undefined) {
        $(this).attr('maxLength', "12")
    }

    //If we set maxlength attribute on textbox then take its max length otherwise take max length 12
    var maxLength = $(this).prop('maxLength'); //Fetch maxlength of textbox

    maxLength = maxLength > 0 ? maxLength : 12;

    //31-Dec-2018 
    if (number.length == 1 && number[0].length < maxLength)
        return true;
    //

    if ((number[0].length >= (maxLength - (parseInt(__decimalPoints) + 1))) && charCode != decimalSeparatorCharCode && number.length == 1) //Restrict user based on maxlength attribute (here we subtract 3 for point and 2 digits after point)
        return false;

    var caratPos = getSelectionStart($(this)[0]);
    var dotPos = $(this).val().indexOf(__currencyDecimalSeparator);

    if (caratPos > dotPos && dotPos > -1 && (number[1].length > parseInt(__decimalPoints) - 1)) {
        return false;
    }

    //if ($(this).val().length >= maxLength) ChangeSet = 12858
    //    return false;

    return true;
});
$(document).on("cut copy paste", ".Currencyinputdecimal", function (e) {
    e.preventDefault();
});
$(document).on("keypress", ".Currencyinputdecimal", function (evt) {
    var decimalSeparatorCharCode = __currencyDecimalSeparator == ',' ? 44 : 46;
    var CommaSeparatorCharCode = __currencyDecimalSeparator == ',' ? 46 : 44;
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = $(this).val().split(__currencyDecimalSeparator);
    if ((charCode != CommaSeparatorCharCode && charCode != decimalSeparatorCharCode && charCode > 31 && (charCode < 48 || charCode > 57))) {
        return false;
    }

    if ((number.length > 1 && charCode == decimalSeparatorCharCode)) {
        return false;
    }

    var attr = $(this).attr('maxLength');
    if (typeof attr == undefined) {
        $(this).attr('maxLength', "12")
    }

    //If we set maxlength attribute on textbox then take its max length otherwise take max length 12
    var maxLength = $(this).prop('maxLength'); //Fetch maxlength of textbox

    maxLength = maxLength > 0 ? maxLength : 12;

    //31-Dec-2018 
    if ((number.length == 1) && number[0].length < maxLength)
        return true;

    if ((number.length == 2) && number[0].length > maxLength)
        return false;

    if ((number[0].length >= (maxLength - (parseInt(__decimalPoints) + 1))) && charCode != decimalSeparatorCharCode && number.length == 1) //Restrict user based on maxlength attribute (here we subtract 3 for point and 2 digits after point)
        return false;

    var caratPos = getSelectionStart($(this)[0]);
    var dotPos = $(this).val().indexOf(__currencyDecimalSeparator);

    if (caratPos > dotPos && dotPos > -1 && (number[1].length > parseInt(__decimalPoints) - 1)) {
        return false;
    }

    //if ($(this).val().length >= maxLength) ChangeSet = 12858
    //    return false;

    return true;
});


////$(document).on("keypress", ".inputdecimal", function (evt) {
////    var decimalSeparatorCharCode = __currencyDecimalSeparator == ',' ? 44 : 46;

////    var charCode = (evt.which) ? evt.which : event.keyCode;
////    var number = $(this).val().split(__currencyDecimalSeparator);
////    if (charCode != decimalSeparatorCharCode && charCode > 31 && (charCode < 48 || charCode > 57)) {
////        return false;
////    }

////    if (number.length > 1 && charCode == decimalSeparatorCharCode) {
////        return false;
////    }

////    //If we set maxlength attribute on textbox then take its max length otherwise take max length 12
////    var maxLength = $(this).prop('maxLength'); //Fetch maxlength of textbox

////    maxLength = maxLength > 0 ? maxLength : 12;

////    if ((number[0].length >= (maxLength - 3)) && charCode != decimalSeparatorCharCode && number.length == 1) //Restrict user based on maxlength attribute (here we subtract 3 for point and 2 digits after point)
////        return false;

////    var caratPos = getSelectionStart($(this)[0]);
////    var dotPos = $(this).val().indexOf(__currencyDecimalSeparator);
////    if ($(this).hasClass('inputdecimalmaxScale')) {
////        if (caratPos > dotPos && dotPos > -1 && (number[1].length > parseInt(5) - 1)) {
////            return false;
////        }
////    }
////    else {
////        if (caratPos > dotPos && dotPos > -1 && (number[1].length > parseInt(2) - 1)) {
////            return false;
////        }
////    }

////    if ($(this).val().length >= maxLength)
////        return false;

////    return true;
////});

$(document).on("keypress", ".inputnumber", function (e) {
    var charCode = (e.which) ? e.which : event.keyCode
    if (charCode < 48 || charCode > 57)
        return false;
    return true;
});

$(document).on("keypress", ".inputnumberwithhyphen", function (e) {
    var charCode = (e.which) ? e.which : event.keyCode;
    if ((charCode < 48 || charCode > 57) && charCode != 45)
        return false;
    return true;
});

$(document).on("keypress", ".inputpercentage", function (evt) {

    var decimalSeparatorCharCode = __currencyDecimalSeparator == ',' ? 44 : 46;
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = $(this).val().split(__currencyDecimalSeparator);

    if (charCode != decimalSeparatorCharCode && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    if (number.length > 1 && charCode == decimalSeparatorCharCode) {
        return false;
    }

    var attr = $(this).attr('maxLength');
    if (typeof attr == undefined) {
        $(this).attr('maxLength', "12");
    }

    //If we set maxlength attribute on textbox then take its max length otherwise take max length 12
    var maxLength = $(this).prop('maxLength'); //Fetch maxlength of textbox

    maxLength = maxLength > 0 ? maxLength : 12;

    if ((number[0].length >= (maxLength - (parseInt(__decimalPoints) + 1))) && charCode != decimalSeparatorCharCode && number.length == 1) //Restrict user based on maxlength attribute (here we subtract 3 for point and 2 digits after point)
        return false;

    var caratPos = getSelectionStart($(this)[0]);
    var dotPos = $(this).val().indexOf(__currencyDecimalSeparator);

    if (caratPos > dotPos && dotPos > -1 && (number[1].length > parseInt(__decimalPoints) - 1)) {
        return false;
    }

    if (GlobalConvertToDecimal($(this).val().substr(0, caratPos) + String.fromCharCode(charCode) + $(this).val().substr(caratPos)) > 100) {
        return false;
    }

    return true;
});

$(document).on("keypress", ".inputpercentageFixedDecimal", function (evt) {

    var decimalSeparatorCharCode = __currencyDecimalSeparator == ',' ? 44 : 46;
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = $(this).val().split(__currencyDecimalSeparator);

    if (charCode != decimalSeparatorCharCode && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    if (number.length > 1 && charCode == decimalSeparatorCharCode) {
        return false;
    }

    var attr = $(this).attr('maxLength');
    if (typeof attr == undefined) {
        $(this).attr('maxLength', "12");
    }

    //If we set maxlength attribute on textbox then take its max length otherwise take max length 12
    var maxLength = $(this).prop('maxLength'); //Fetch maxlength of textbox

    maxLength = maxLength > 0 ? maxLength : 12;

    if ((number[0].length >= (maxLength - (parseInt(5) + 1))) && charCode != decimalSeparatorCharCode && number.length == 1) //Restrict user based on maxlength attribute (here we subtract 3 for point and 2 digits after point)
        return false;

    var caratPos = getSelectionStart($(this)[0]);
    var dotPos = $(this).val().indexOf(__currencyDecimalSeparator);

    if (caratPos > dotPos && dotPos > -1 && (number[1].length > parseInt(5) - 1)) {
        return false;
    }

    if (GlobalConvertToDecimal($(this).val().substr(0, caratPos) + String.fromCharCode(charCode) + $(this).val().substr(caratPos)) > 100) {
        return false;
    }

    return true;
});

$(document).on("keypress", ".inputphonenumber", function (evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
});

$(document).on("keypress", ".inputzipcode", function (e) {
    var charCode = (e.which) ? e.which : event.keyCode
    if ((charCode < 45 || charCode > 57) || (charCode == 46) || (charCode == 47))
        return false;
    return true;
});

//TaxNumber-CR
$(document).on("keypress", ".inputnumberletterswithhyphen", function (e) {
    var keyCode = (e.which) ? e.which : event.keyCode;
    if ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (keyCode == 45))
        return true;
    return false;
});
$('.inputnumberletterswithhyphen').on('drop', function (event) {
    if (!event.originalEvent.dataTransfer.getData("Text").match(/^[a-zA-Z0-9-]+$/)) {
        event.preventDefault();
    }
});

$('.inputnumberletterswithhyphen').on('paste', function (event) {
    if (!event.originalEvent.clipboardData.getData('Text').match(/^[a-zA-Z0-9-]+$/)) {
        event.preventDefault();
    }
});
//

function NumberToCultureFormat(number) {
    return _currencySymbol + number.toLocaleString(__culture, { minimumFractionDigits: __decimalPoints, maximumFractionDigits: __decimalPoints })
}

function chatShowLoading() {
    if ($("#tab-1 #chat_loading_container").length <= 0) {
        $('#tab-1').append("<div id=\"chat_loading_container\" class=\"chatoverlay\">\
                            <div class=\"overlay-content\">\
                                <div class='sk-spinner sk-spinner-circle'>\
                                    <div class='sk-circle1 sk-circle'></div>\
                                    <div class='sk-circle2 sk-circle'></div>\
                                    <div class='sk-circle3 sk-circle'></div>\
                                    <div class='sk-circle4 sk-circle'></div>\
                                    <div class='sk-circle5 sk-circle'></div>\
                                    <div class='sk-circle6 sk-circle'></div>\
                                    <div class='sk-circle7 sk-circle'></div>\
                                    <div class='sk-circle8 sk-circle'></div>\
                                    <div class='sk-circle9 sk-circle'></div>\
                                    <div class='sk-circle10 sk-circle'></div>\
                                    <div class='sk-circle11 sk-circle'></div>\
                                    <div class='sk-circle12 sk-circle'></div>\
                                </div>\
                            </div>\
                         </div>");
        $("#chat_loading_container").show();
    } else
        $("#chat_loading_container").show();
}

function chatHideLoading() {
    $("#chat_loading_container").hide();
}
// Reset form validation
jQuery.fn.ResetFormValidation =
    function () {
        return this.each(function () {
            var $form = $(this);

            //reset jQuery Validate's internals
            $form.validate().resetForm();

            //reset unobtrusive validation summary, if it exists
            $form.find("[data-valmsg-summary=true]")
                .removeClass("validation-summary-errors")
                .addClass("validation-summary-valid")
                .find("ul").empty();

            //reset unobtrusive field level, if it exists
            $form.find("[data-valmsg-replace]")
                .removeClass("field-validation-error")
                .addClass("field-validation-valid")
                .hide();

        });
    };


function GetLocalDateTime(utcDate) {
    return new Date(utcDate).toString();
}

function fnGetCultureDateForChat(displaydate, isDisplayDateWithTime, isDisplayOnlyTime) {
    /// <summary>Used to set date in current culture format</summary>  
    /// <param name="date" type="date">date which we want to convert in current culture</param>  
    if (displaydate === null) return "";
    $.global.preferCulture(__culture);
    var dt = new Date(displaydate);
    if (isDisplayDateWithTime)
        return $.format(dt, $.global.culture.calendar.patterns.d + " " + $.global.culture.calendar.patterns.t);
    else if (isDisplayOnlyTime)
        return $.format(dt, $.global.culture.calendar.patterns.t);
    else
        return $.format(dt, $.global.culture.calendar.patterns.d);
}

function GetLocalDateTimeForChat(currentdate, utcDate) {

    var date = new Date(utcDate + ' UTC');
    if (date == 'Invalid Date') {
        var myDate = new Date(currentdate);
        return myDate;
    }
    else {
        return date.toString()
    }
}

function createCookie(name, value, days) {
    var expires;

    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toGMTString();
    } else {
        expires = "";
    }
    document.cookie = escape(name) + "=" + escape(value) + expires + "; path=/";
}

function readCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) != -1) {
            var tmp = c.substring(name.length, c.length);

            try {

                if (tmp.indexOf(name) != -1) {
                    return tmp.substring(name.length, c.length);
                }
            } catch (e) {
                return tmp;
            }
            return tmp;
        }
    }
    return null;
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}

function GlobalConvertToDecimal(Number) {
    var RemoveCharCode = __currencyDecimalSeparator == ',' ? '.' : ',';
    var data = jQuery.type("test") != "string" ? Number.toString() : Number;
    if (RemoveCharCode == ',') {
        data = data.replace(/,/g, '');
    }
    else if (RemoveCharCode == '.') {
        data = data.replace(/./g, '');
    }
    return Globalize.parseNumber(data);
}

function GlobalFormatWithText(Value) {
    return _currencySymbol + Globalize.numberFormatter({ minimumFractionDigits: __decimalPoints, maximumFractionDigits: __decimalPoints, useGrouping: false })(Value);
}
function GlobalFormat(Value) {
    //return Globalize.numberFormatter({ minimumFractionDigits: __decimalPoints })(Value);
    return Globalize.numberFormatter({ minimumFractionDigits: __decimalPoints, maximumFractionDigits: __decimalPoints, useGrouping: false })(Value);
}

function GlobalFormatForFixedDecimal(Value, fractionDigits) {
    return Globalize.numberFormatter({ minimumFractionDigits: fractionDigits, maximumFractionDigits: fractionDigits, useGrouping: false })(Value);
}

function CurrencyGlobalFormat(Value) {
    return Globalize.numberFormatter({ minimumFractionDigits: __decimalPoints, maximumFractionDigits: __decimalPoints, useGrouping: false })(Value);
}

function CurrencyGlobalFormatWithText(Value) {
    return _currencySymbol + Globalize.numberFormatter({ minimumFractionDigits: __decimalPoints, maximumFractionDigits: __decimalPoints, useGrouping: false })(Value);
}

function CurrencyGlobalFormatWithTextAndThousandSeparator(Value) {
    return _currencySymbol + Globalize.numberFormatter({ minimumFractionDigits: __decimalPoints, maximumFractionDigits: __decimalPoints, useGrouping: true })(Value);
}

function GlobalFormatWithTextAndThousandSeparator(Value) {
    return Globalize.numberFormatter({ minimumFractionDigits: __decimalPoints, maximumFractionDigits: __decimalPoints, useGrouping: true })(Value);
}

$(document).on("drop", ".inputdecimal,.inputdecimalwithminus,.inputpercentage,.Currencyinputdecimal,.Currencyinputdecimalwithminus", function (evt) {
    return false;
});

//$(document).on("paste", ".inputdecimal,.inputdecimalwithminus,.inputpercentage", function (evt) {
//    return false;
//});
$(document).on("paste", ".inputdecimal,.inputdecimalwithminus,.inputpercentage,.Currencyinputdecimal,.Currencyinputdecimalwithminus", function (evt) {
    return false;
});

function GetAccountForSelect(id, accountID, accountTypeIDs, placeholder, inputTooShort, noResults, width, multiple) {
    $('#' + id).select2({
        placeholder: placeholder,
        minimumInputLength: 3,
        language: {
            inputTooShort: function () { return inputTooShort; }
            , noResults: function () { return noResults; }
        },
        multiple: multiple == null ? false : multiple,
        allowClear: true,
        width: width == null ? '100%' : width,
        ajax: {
            url: ROOTPath + "/Account/GetAccountForSearch",
            data: function (params) {
                var query = {
                    accountTypeIDs: accountTypeIDs,
                    accountID: accountID,
                    searchText: params.term,
                    isActive: true,
                    pageIndex: params.page || 1,
                    pageSize: pageSize
                };
                return query;
            },
            processResults: function (data, params) {
                if (!multiple) {
                    $('#' + id).text('');
                }

                params.page = params.page || 1;
                return {
                    results: data.AccountList,
                    pagination: {
                        more: (params.page * pageSize) < data.TotalRecord
                    }
                };
            }
        }
    });
}

function GetAccountPropertyForSelect(id, placeholder, inputTooShort, noResults, multiple) {
    $('#' + id).select2({
        placeholder: placeholder,
        minimumInputLength: 3,
        language: {
            inputTooShort: function () { return inputTooShort; }
            , noResults: function () { return noResults; }
        },
        multiple: multiple == null ? false : multiple,
        allowClear: true,
        width: '100%',
        ajax: {
            url: ROOTPath + "/AccountProperty/GetAccountPropertyForSearch",
            data: function (params) {

                var query = {
                    searchText: params.term,
                    pageIndex: params.page || 1,
                    pageSize: pageSize
                };
                return query;
            },
            processResults: function (data, params) {
                if (!multiple) {
                    $('#' + id).text('');
                }

                params.page = params.page || 1;
                return {
                    results: data.AccountPropertyList,
                    pagination: {
                        more: (params.page * pageSize) < data.TotalRecord
                    }
                };
            }
        }
    });
}

function GetAccountPropertyRightForSelect(id, placeholder, inputTooShort, noResults, multiple) {
    $('#' + id).select2({
        placeholder: placeholder,
        minimumInputLength: 3,
        language: {
            inputTooShort: function () { return inputTooShort; }
            , noResults: function () { return noResults; }
        },
        multiple: multiple == null ? false : multiple,
        allowClear: true,
        width: '100%',
        ajax: {
            url: ROOTPath + "/AccountProperty/GetAccountPropertyRightForSearch",
            data: function (params) {

                var query = {
                    searchText: params.term,
                    pageIndex: params.page || 1,
                    pageSize: pageSize
                };
                return query;
            },
            processResults: function (data, params) {
                if (!multiple) {
                    $('#' + id).text('');
                }

                params.page = params.page || 1;
                return {
                    results: data.AccountPropertyList,
                    pagination: {
                        more: (params.page * pageSize) < data.TotalRecord
                    }
                };
            }
        }
    });
}