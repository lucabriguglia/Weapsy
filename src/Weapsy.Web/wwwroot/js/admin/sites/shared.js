weapsy.admin.sites = weapsy.admin.sites || {};

weapsy.admin.sites = (function ($) {
    $('.validate-form').validate();

    $(".validate-title").each(function () {
        $(this).rules("add", {
            required: false,
            rangelength: [1, 250],
            messages: {
                rangelength: jQuery.validator.format('Enter between {0} and {1} characters')
            }
        });
    });

    $(".validate-meta-description .validate-meta-keywords").each(function () {
        $(this).rules("add", {
            required: false,
            rangelength: [1, 500],
            messages: {
                rangelength: jQuery.validator.format('Enter between {0} and {1} characters')
            }
        });
    });

    return {
        redirectToIndex: function () {
            window.location.href = '/admin/site';
        }
    }
}(jQuery));