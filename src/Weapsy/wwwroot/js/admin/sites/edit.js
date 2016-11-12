weapsy.admin.editSite = weapsy.admin.editSite || {};

weapsy.admin.editSite = (function ($) {
    return {
        success: function () {
            $("#siteSaved").show();
            setTimeout(function () {
                $("#siteSaved").hide();
            }, 2000);
        }
    }
}(jQuery));
