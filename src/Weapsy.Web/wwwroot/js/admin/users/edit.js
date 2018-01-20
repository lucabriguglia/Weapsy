weapsy.admin.editUser = weapsy.admin.editUser || {};

weapsy.admin.editUser = (function ($) {
    var id = $('#id').val();

    $('#confirmDelete').click(function () {
        $('#savingUser').show();
        $.ajax({
            url: "/api/user/" + id,
            type: "DELETE"
        }).done(function () {
            $('#savingUser').hide();
            window.location.href = '/admin/user';
        }).fail(function () {
            $('#savingUser').hide();
        });
    });
}(jQuery));
