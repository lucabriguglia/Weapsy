weapsy.admin.emailAccountIndex = weapsy.admin.emailAccountIndex || {};

weapsy.admin.emailAccountIndex = (function ($) {
    var emailAccountIdToDelete;

    $('.delete-emailAccount').click(function () {
        emailAccountIdToDelete = $(this).attr("data-id");
    });

    $('#confirmDelete').click(function () {
        $('#deletingEmailAccount').show();
        $.ajax({
            url: "/api/emailAccount/" + emailAccountIdToDelete,
            type: "DELETE"
        }).done(function () {
            $('#deletingEmailAccount').hide();
            window.location.href = '/admin/emailAccount'; // to do: just remove row
        }).fail(function () {
            $('#deletingEmailAccount').hide();
        });
    });
}(jQuery));