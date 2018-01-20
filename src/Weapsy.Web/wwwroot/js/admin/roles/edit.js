weapsy.admin.editRole = weapsy.admin.editRole || {};

weapsy.admin.editRole = (function ($) {
    var id = $('#id').val();

    $('#confirmDelete').click(function () {
        $('#savingRole').show();
        $.ajax({
            url: "/api/role/" + id,
            type: "DELETE"
        }).done(function () {
            $('#savingRole').hide();
            window.location.href = '/admin/role';
        }).fail(function () {
            $('#savingRole').hide();
        });
    });
}(jQuery));
