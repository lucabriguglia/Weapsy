weapsy.admin.editPage = weapsy.admin.editPage || {};

weapsy.admin.editPage = (function ($) {
    var id = $('#id').val();

    $('#activatePage').click(function () {
        weapsy.utils.showLoading("Activating Page");
        $.ajax({
            url: "/api/page/" + id + "/activate",
            type: "PUT"
        }).done(function () {
            window.location.href = '/admin/page';
        }).fail(function () {
        });
    });

    $('#hidePage').click(function () {
        weapsy.utils.showLoading("Hiding Page");
        $.ajax({
            url: "/api/page/" + id + "/hide",
            type: "PUT"
        }).done(function () {
            window.location.href = '/admin/page';
        }).fail(function () {
        });
    });

    $('#confirmDelete').click(function () {
        weapsy.utils.showLoading("Deleting Page");
        $.ajax({
            url: "/api/page/" + id,
            type: "DELETE"
        }).done(function () {
            window.location.href = '/admin/page';
        }).fail(function () {
        });
    });

    return {
        loading: function () {
            weapsy.utils.showLoading("Updating Page");
        }
    }
}(jQuery));
