weapsy.admin.roles = weapsy.admin.roles || {};

weapsy.admin.roles = (function ($) {
    var roleIdToDelete;

    $('.delete-role').click(function () {
        roleIdToDelete = $(this).attr("data-id");
    });

    $('#confirmDelete').click(function () {
        $.ajax({
            url: "/api/role/" + roleIdToDelete,
            type: "DELETE"
        }).done(function () {
            $("#" + roleIdToDelete).remove();
        }).fail(function () {
        });
    });
}(jQuery));