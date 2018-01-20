weapsy.admin.themeIndex = weapsy.admin.themeIndex || {};

weapsy.admin.themeIndex = (function ($) {
    var themeIdToDelete;

    $('.delete-theme').click(function () {
        themeIdToDelete = $(this).attr("data-id");
    });

    $('#confirmDelete').click(function () {
        $('#deletingPage').show();
        $.ajax({
            url: "/api/theme/" + themeIdToDelete,
            type: "DELETE"
        }).done(function () {
            $('#deletingTheme').hide();
            window.location.href = '/admin/theme'; // to do: just remove row
        }).fail(function () {
            $('#deletingTheme').hide();
        });
    });
}(jQuery));