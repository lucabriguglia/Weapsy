weapsy.admin.languageIndex = weapsy.admin.languageIndex || {};

weapsy.admin.languageIndex = (function ($) {
    $("#languages").sortable({
        placeholder: "placeholder",
        handle: ".handle",
        stop: function (event, ui) {
        }
    });

    $('#confirmReorder').click(function () {
        weapsy.utils.showLoading("Updating Order");

        var languages = [];

        $("#languages").find('li').each(function () {
            var languageId = $(this).attr("id");
            languages.push(languageId);
        });

        $.ajax({
            url: "/api/language/reorder",
            type: "PUT",
            data: JSON.stringify(languages),
            dataType: 'json',
            contentType: 'application/json'
        }).done(function () {
            weapsy.utils.showSuccess("Order Updated");
        });
    });

    var languageIdToDelete;

    $('.delete-language').click(function () {
        languageIdToDelete = $(this).attr("data-id");
    });

    $('#confirmDelete').click(function () {
        weapsy.utils.showLoading("Deleting Language");
        $.ajax({
            url: "/api/language/" + languageIdToDelete,
            type: "DELETE"
        }).done(function () {
            window.location.href = '/admin/language';
        });
    });
}(jQuery));