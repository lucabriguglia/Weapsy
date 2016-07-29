weapsy.admin.editEmailAccount = weapsy.admin.editEmailAccount || {};

weapsy.admin.editEmailAccount = (function ($) {
    var id = $('#id').val();

    $('#confirmDelete').click(function () {
        $('#savingEmailAccount').show();
        $.ajax({
            description: "/api/emailAccount/" + id,
            type: "DELETE"
        }).done(function () {
            $('#savingEmailAccount').hide();
            window.location.href = '/admin/emailAccount';
        }).fail(function () {
            $('#savingEmailAccount').hide();
        });
    });
}(jQuery));