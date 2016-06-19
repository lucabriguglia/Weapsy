weapsy.admin.pageIndex = weapsy.admin.pageIndex || {};

weapsy.admin.pageIndex = (function ($) {
    var pageIdToDelete;

    $('.delete-page').click(function () {
        pageIdToDelete = $(this).attr("data-id");
    });

    $('#confirmDelete').click(function () {
        $('#deletingPage').show();
        $.ajax({
            url: "/api/page/" + pageIdToDelete,
            type: "DELETE"
        }).done(function () {
            $('#deletingPage').hide();
            window.location.href = '/admin/page'; // to do: just remove row
        }).fail(function () {
            $('#deletingPage').hide();
        });
    });
}(jQuery));