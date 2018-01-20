weapsy.admin.editLanguage = weapsy.admin.editLanguage || {};

weapsy.admin.editLanguage = (function ($) {
    var id = $('#id').val();

    $('#editLanguageForm').validate({
        submitHandler: function (form) {
            weapsy.utils.showLoading("Updating Language");

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
            });
        }
    });

    $('#activateLanguage').click(function () {
        weapsy.utils.showLoading("Activating Language");
        $.ajax({
            url: "/api/language/" + id + "/activate",
            type: "PUT"
        }).done(function () {
            window.location.href = '/admin/language';
        });
    });

    $('#hideLanguage').click(function () {
        weapsy.utils.showLoading("Hiding Language");
        $.ajax({
            url: "/api/language/" + id + "/hide",
            type: "PUT"
        }).done(function () {
            window.location.href = '/admin/language';
        });
    });

    $('#confirmDelete').click(function () {
        weapsy.utils.showLoading("Deleting Language");
        $.ajax({
            url: "/api/language/" + id,
            type: "DELETE"
        }).done(function () {
            window.location.href = '/admin/language';
        });
    });
}(jQuery));