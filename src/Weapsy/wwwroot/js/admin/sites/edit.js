weapsy.admin.editSite = weapsy.admin.editSite || {};

weapsy.admin.editSite = (function ($) {
    return {
        loading: function () {
            weapsy.utils.showLoading("Updating Site Settings");
        },
        success: function () {
            weapsy.utils.showSuccess("Site Settings Updated");
        }
    }
}(jQuery));
