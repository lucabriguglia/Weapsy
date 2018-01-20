weapsy.admin.apps = weapsy.admin.apps || {};

weapsy.admin.apps = (function ($) {
    $('.validate-form').validate({
        rules: {
            name: {
                required: true,
                rangelength: [1, 100],
                regex: /^[A-Za-z\d\s_-]+$/,
                remote: {
                    param: {
                        url: '/api/app/IsAppNameUnique/'
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
                        url: '/api/app/IsAppFolderUnique/'
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
                required: 'Enter the App Folder',
                rangelength: jQuery.validator.format(window.rangeLengthMessage),
                regex: 'Enter only letters, numbers, underscores and hyphens',
                remote: jQuery.validator.format(window.alreadyInUseMessage)
            }
        }
    });

    return {
        redirectToIndex: function () {
            window.location.href = '/admin/app';
        }
    }
}(jQuery));