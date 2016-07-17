weapsy.admin.userRoles = weapsy.admin.userRoles || {};

weapsy.admin.userRoles = (function ($) {
    $(document).ready(function () {
        init();
    });

    function init() {
        $("#available-roles, #user-roles").sortable({
            handle: ".handle",
            connectWith: ".connected-list",
            placeholder: "placeholder",
            stop: function (event, ui) {
                var userId = $("#user-id").val();
                var roleName = $(ui.item.context).attr("data-role-name");
                var isAddingRoleToUser = $(ui.item.context).hasClass("available-role");

                var action = isAddingRoleToUser
                    ? "add-to-role"
                    : "remove-from-role";

                $.ajax({
                    url: "/api/user/" + userId + "/" + action,
                    type: "PUT",
                    data: JSON.stringify(roleName),
                    dataType: 'json',
                    contentType: 'application/json'
                }).done(function () {
                });
            }
        }).disableSelection();
    }
}(jQuery));