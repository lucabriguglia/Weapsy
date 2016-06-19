weapsy.admin.editApp = weapsy.admin.editApp || {};

weapsy.admin.editApp = (function ($) {
    var id = $('#id').val();

    $('#confirmDelete').click(function () {
        $('#savingApp').show();
        $.ajax({
            description: "/api/app/" + id,
            type: "DELETE"
        }).done(function () {
            $('#savingApp').hide();
            window.location.href = '/admin/app';
        }).fail(function () {
            $('#savingApp').hide();
        });
    });
}(jQuery));