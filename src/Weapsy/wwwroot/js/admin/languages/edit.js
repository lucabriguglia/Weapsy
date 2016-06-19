weapsy.admin.editLanguage = weapsy.admin.editLanguage || {};

weapsy.admin.editLanguage = (function ($) {
    var id = $('#id').val();

    $('#editLanguageForm').validate({
        submitHandler: function (form) {
            $('#savingLanguage').show();
            $('#languageSaved').hide();

            var language = {
                id: id,
                name: $('#name').val(),
                cultureName: $('#cultureName').val(),
                url: $('#url').val()
            };

            $.ajax({
                url: '/api/language/' + id + '/update',
                data: JSON.stringify(language),
                type: 'PUT',
                dataType: 'json',
                contentType: 'application/json'
            }).done(function () {
                window.location.href = '/admin/language';
            }).fail(function () {
                $('#savingLanguage').hide();
            });
        }
    });

    $('#activateLanguage').click(function () {
        $('#savingLanguage').show();
        $.ajax({
            url: "/api/language/" + id + "/activate",
            type: "PUT"
        }).done(function () {
            $('#savingLanguage').hide();
            window.location.href = '/admin/language';
        }).fail(function () {
            $('#savingLanguage').hide();
        });
    });

    $('#hideLanguage').click(function () {
        $('#savingLanguage').show();
        $.ajax({
            url: "/api/language/" + id + "/hide",
            type: "PUT"
        }).done(function () {
            $('#savingLanguage').hide();
            window.location.href = '/admin/language';
        }).fail(function () {
            $('#savingLanguage').hide();
        });
    });

    $('#confirmDelete').click(function () {
        $('#savingLanguage').show();
        $.ajax({
            url: "/api/language/" + id,
            type: "DELETE"
        }).done(function () {
            $('#savingLanguage').hide();
            window.location.href = '/admin/language';
        }).fail(function () {
            $('#savingLanguage').hide();
        });
    });
}(jQuery));