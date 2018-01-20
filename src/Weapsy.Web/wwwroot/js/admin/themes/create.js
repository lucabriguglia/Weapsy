weapsy.admin.createTheme = weapsy.admin.createTheme || {};

weapsy.admin.createTheme = (function ($) {
    return {
        loading: function () {
            weapsy.utils.showLoading("Creating New Theme");
        }
    }
}(jQuery));