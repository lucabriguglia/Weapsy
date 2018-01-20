weapsy.admin.appIndex = weapsy.admin.appIndex || {};

weapsy.admin.appIndex = (function ($) {
    var appIdToDelete;

    $('.delete-app').click(function () {
        appIdToDelete = $(this).attr("data-id");
    });

    $('#confirmDelete').click(function () {
        $('#deletingPage').show();
        $.ajax({
            url: "/api/app/" + appIdToDelete,
            type: "DELETE"
        }).done(function () {
            $('#deletingApp').hide();
            window.location.href = '/admin/app'; // to do: just remove row
        }).fail(function () {
            $('#deletingApp').hide();
        });
    });
}(jQuery));