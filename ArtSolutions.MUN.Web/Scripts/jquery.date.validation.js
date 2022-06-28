extendJQueryDateValidation();
function extendJQueryDateValidation() {
    // fix date validation for chrome
    jQuery.extend(jQuery.validator.methods, {
        date: function (value, element) {
            return this.optional(element) ||
            !/Invalid|NaN/.test($.fn.datepicker.DPGlobal.parseDate(value, __dateFormat, __culture));
        }
    });
}