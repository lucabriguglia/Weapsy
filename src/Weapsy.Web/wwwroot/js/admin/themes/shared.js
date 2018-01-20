weapsy.admin.themes = weapsy.admin.themes || {};

weapsy.admin.themes = (function ($) {
    $('.validate-form').validate({
        rules: {
            name: {
                required: true,
                rangelength: [1, 100],
                regex: /^[A-Za-z\d\s_-]+$/,
                remote: {
                    param: {
                        url: '/api/theme/IsThemeNameUnique/'
                    },
                    depends: function () {
                        return ($('#name').val() !== $('#originalName').val());
                    }
                }
            },
            folder: {
                required: true,
                rangelength: [1, 100],
                regex: /^[A-Za-z\.\d_-]+$/,
                remote: {
                    param: {
                        url: '/api/theme/IsThemeFolderUnique/'
                    },
                    depends: function () {
                        return ($('#folder').val() !== $('#originalFolder').val());
                    }
                }
            }
        },
        messages: {
            name: {
                required: window.nameRequired,
                rangelength: jQuery.validator.format(window.rangeLengthMessage),
                regex: 'Enter only letters, numbers, underscores and hyphens',
                remote: jQuery.validator.format(window.alreadyInUseMessage)
            },
            folder: {
                required: 'Enter the Theme Folder',
                rangelength: jQuery.validator.format(window.rangeLengthMessage),
                regex: 'Enter only letters, numbers, underscores and hyphens',
                remote: jQuery.validator.format(window.alreadyInUseMessage)
            }
        }
    });

    return {
        redirectToIndex: function () {
            window.location.href = '/admin/theme';
        }
    }
}(jQuery));