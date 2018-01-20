weapsy.admin.createLanguage = weapsy.admin.createLanguage || {};

weapsy.admin.createLanguage = (function ($) {
    $('#createLanguageForm').validate({
        submitHandler: function (form) {
            weapsy.utils.showLoading("Creating Language");
            var language = {
                name: $('#name').val(),
                cultureName: $('#cultureName').val(),
                url: $('#url').val()
            };

            $.ajax({
                url: '/api/language',
                data: JSON.stringify(language),
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json'
            }).done(function () {
                window.location.href = '/admin/language';
            }).fail(function () {
            });
        }
    });
}(jQuery));