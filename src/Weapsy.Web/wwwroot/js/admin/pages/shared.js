weapsy.admin.pages = weapsy.admin.pages || {};

weapsy.admin.pages = (function ($) {
    $('.validate-form').validate({
        rules: {
            name: {
                required: true,
                rangelength: [1, 100],
                regex: /^[A-Za-z\d\s_-]+$/,
                remote: {
                    param: {
                        url: '/api/page/IsPageNameUnique/'
                    },
                    depends: function() {
                        return ($('#name').val() !== $('#originalName').val());
                    }
                }
            },
            url: {
                required: true,
                rangelength: [1, 200],
                regex: /^[A-Za-z/\d_-]+$/,
                remote: {
                    param: {
                        url: '/api/page/IsPageSlugUnique/'
                    },
                    depends: function () {
                        return ($('#url').val() !== $('#originalUrl').val());
                    }
                }
            }
        },
        messages: {
            name: {
                required: 'Enter the page name',
                rangelength: jQuery.validator.format('Enter between {0} and {1} characters'),
                regex: 'Enter only letters, numbers, underscores and hyphens',
                remote: jQuery.validator.format('{0} is already in use')
            },
            url: {
                required: 'Enter the url for the page',
                rangelength: jQuery.validator.format('Enter between {0} and {1} characters'),
                regex: 'Enter only letters, numbers, underscores and hyphens with no spaces',
                remote: jQuery.validator.format('{0} is already in use')
            }
        }
    });

    $(".validate-localisation-url").each(function () {
        $(this).rules("add", {
            required: false,
            rangelength: [1, 200],
            regex: /^[A-Za-z\d_-]+$/,
            messages: {
                rangelength: jQuery.validator.format('Enter between {0} and {1} characters'),
                regex: 'Enter only letters, numbers, underscores and hyphens with no spaces'
            }
        });
    });

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
            window.location.href = '/admin/page';
        }
    }
}(jQuery));