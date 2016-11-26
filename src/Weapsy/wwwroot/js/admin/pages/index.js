weapsy.admin.pageIndex = weapsy.admin.pageIndex || {};

weapsy.admin.pageIndex = (function ($) {
    $('.activate-page').click(function () {
        var pageId = $(this).attr("data-page-id");
        var checked = $(this).is(":checked");
        var action = checked ? "activate" : "hide";
        var loadingText = checked ? "Activating Page" : "Hiding Page";
        var successText = checked ? "Page Activated" : "Page Hidden";
        weapsy.utils.showLoading(loadingText);
        $.ajax({
            url: "/api/page/" + pageId + "/" + action,
            type: "PUT"
        }).done(function () {
            weapsy.utils.showSuccess(successText);
        }).fail(function () {
        });
    });

    var pageIdToDelete;

    $('.delete-page').click(function () {
        pageIdToDelete = $(this).attr("data-id");
    });

    $('#confirmDelete').click(function () {
        weapsy.utils.showLoading("Deleting Page");
        $.ajax({
            url: "/api/page/" + pageIdToDelete,
            type: "DELETE"
        }).done(function () {
            window.location.href = '/admin/page';
        }).fail(function () {
        });
    });
}(jQuery));