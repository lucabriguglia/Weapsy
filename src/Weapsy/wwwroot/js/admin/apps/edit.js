weapsy.admin.editApp = weapsy.admin.editApp || {};

weapsy.admin.editApp = (function ($) {
    var id = $('#id').val();

    $('#confirmDelete').click(function () {
        weapsy.utils.showLoading("Deleting App");
        $.ajax({
            description: "/api/app/" + id,
            type: "DELETE"
        }).done(function () {
            $('#savingApp').hide();
            window.location.href = '/admin/app';
        }).fail(function () {
        });
    });

    return {
        loading: function () {
            weapsy.utils.showLoading("Updating App");
        }
    }
}(jQuery));