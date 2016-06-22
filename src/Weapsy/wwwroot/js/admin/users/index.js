weapsy.admin.userIndex = weapsy.admin.userIndex || {};

weapsy.admin.userIndex = (function ($) {
    var userIdToDelete;

    $('.delete-page').click(function () {
        userIdToDelete = $(this).attr("data-id");
    });

    $('#confirmDelete').click(function () {
        $.ajax({
            url: "/api/user/" + userIdToDelete,
            type: "DELETE"
        }).done(function () {
            window.location.href = '/admin/user';
        }).fail(function () {
        });
    });
}(jQuery));