weapsy.admin.editTheme = weapsy.admin.editTheme || {};

weapsy.admin.editTheme = (function ($) {
    var id = $('#id').val();

    $('#confirmDelete').click(function () {
        weapsy.utils.showLoading("Deleting Theme");
        $.ajax({
            description: "/api/theme/" + id,
            type: "DELETE"
        }).done(function () {
            $('#savingTheme').hide();
            window.location.href = '/admin/theme';
        }).fail(function () {
        });
    });

    return {
        loading: function () {
            weapsy.utils.showLoading("Updating Theme");
        }
    }
}(jQuery));