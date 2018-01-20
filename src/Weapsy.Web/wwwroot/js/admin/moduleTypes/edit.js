weapsy.admin.editModuleType = weapsy.admin.editModuleType || {};

weapsy.admin.editModuleType = (function ($) {
    var id = $('#id').val();

    $('#confirmDelete').click(function () {
        $('#savingModuleType').show();
        $.ajax({
            description: "/api/moduleType/" + id,
            type: "DELETE"
        }).done(function () {
            $('#savingModuleType').hide();
            window.location.href = '/admin/moduleType';
        }).fail(function () {
            $('#savingModuleType').hide();
        });
    });
}(jQuery));