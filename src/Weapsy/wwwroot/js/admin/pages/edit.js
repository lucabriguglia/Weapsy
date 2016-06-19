weapsy.admin.editPage = weapsy.admin.editPage || {};

weapsy.admin.editPage = (function ($) {
    var id = $('#id').val();

    $('#activatePage').click(function () {
        $('#savingPage').show();
        $.ajax({
            url: "/api/page/" + id + "/activate",
            type: "PUT"
        }).done(function () {
            $('#savingPage').hide();
            window.location.href = '/admin/page';
        }).fail(function () {
            $('#savingPage').hide();
        });
    });

    $('#hidePage').click(function () {
        $('#savingPage').show();
        $.ajax({
            url: "/api/page/" + id + "/hide",
            type: "PUT"
        }).done(function () {
            $('#savingPage').hide();
            window.location.href = '/admin/page';
        }).fail(function () {
            $('#savingPage').hide();
        });
    });

    $('#confirmDelete').click(function () {
        $('#savingPage').show();
        $.ajax({
            url: "/api/page/" + id,
            type: "DELETE"
        }).done(function () {
            $('#savingPage').hide();
            window.location.href = '/admin/page';
        }).fail(function () {
            $('#savingPage').hide();
        });
    });
}(jQuery));
