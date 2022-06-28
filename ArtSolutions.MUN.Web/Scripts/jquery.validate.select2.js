$.validator.setDefaults({
    highlight: function (element, errorClass, validClass) {
        if ($(element).hasClass('select2dropdown'))
            $(element).next('span').find('.select2-selection').addClass('select2-validation-error');
        else
            $(element).addClass('input-validation-error');
    },
    unhighlight: function (element, errorClass, validClass) {
        if ($(element).hasClass('select2dropdown'))
            $(element).next('span').find('.select2-selection').removeClass('select2-validation-error');
        else
            $(element).removeClass('input-validation-error');
    },
    errorPlacement: function (error, element) {
        if (element.parent('.input-group').length) {
            error.insertAfter(element.parent());
        } else if (element.hasClass('select2dropdown')) {
            error.insertAfter(element.next('span'));
        } else {
            error.insertAfter(element);
        }
    }
});

$(document).on('change', '.select2dropdown', function () {
    if ($(this).hasClass('required'))
        $(this).valid();
});