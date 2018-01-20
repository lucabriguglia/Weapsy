weapsy.admin.moduleTypeIndex = weapsy.admin.moduleTypeIndex || {};

weapsy.admin.moduleTypeIndex = (function ($) {
    var moduleTypeIdToDelete;

    $('.delete-moduleType').click(function () {
        moduleTypeIdToDelete = $(this).attr("data-id");
    });

    $('#confirmDelete').click(function () {
        $('#deletingModuleType').show();
        $.ajax({
            url: "/api/moduleType/" + moduleTypeIdToDelete,
            type: "DELETE"
        }).done(function () {
            $('#deletingModuleType').hide();
            window.location.href = '/admin/moduleType'; // to do: just remove row
        }).fail(function () {
            $('#deletingModuleType').hide();
        });
    });
}(jQuery));