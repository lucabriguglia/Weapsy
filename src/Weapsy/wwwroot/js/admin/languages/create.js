weapsy.admin.createLanguage = weapsy.admin.createLanguage || {};

weapsy.admin.createLanguage = (function ($) {
    $('#createLanguageForm').validate({
        submitHandler: function (form) {
            $('#savingLanguage').show();
            $('#languageSaved').hide();

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
                $('#savingLanguage').hide();
            });
        }
    });
}(jQuery));