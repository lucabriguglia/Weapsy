weapsy.admin.languages = weapsy.admin.languages || {};

weapsy.admin.languages = (function ($) {
    $.validator.setDefaults({
        rules: {
            name: {
                required: true,
                rangelength: [2, 100],
                regex: /^[A-Za-z\d\s_-]+$/,
                remote: {
                    param: {
                        url: '/api/language/IsLanguageNameUnique/'
                    },
                    depends: function() {
                        return ($('#name').val() !== $('#originalName').val());
                    }
                }
            },
            cultureName: {
                required: true,
                remote: {
                    param: {
                        url: '/api/language/IsCultureNameUnique/'
                    },
                    depends: function() {
                        return ($('#cultureName').val() !== $('#originalCultureName').val());
                    }
                }
            },
            url: {
                required: true,
                rangelength: [2, 100],
                regex: /^[A-Za-z\d_-]+$/,
                remote: {
                    param: {
                        url: '/api/language/isLanguageUrlUnique/'
                    },
                    depends: function() {
                        return ($('#url').val() !== $('#originalUrl').val());
                    }
                }
            }
        },
        messages: {
            name: {
                required: 'Enter the language name',
                rangelength: jQuery.validator.format('Enter between {0} and {1} characters'),
                regex: 'Enter only letters, numbers, underscores and hyphens',
                remote: jQuery.validator.format('{0} is already in use')
            },
            cultureName: {
                required: 'Select the culture for the language',
                remote: jQuery.validator.format('{0} is already in use')
            },
            url: {
                required: 'Enter the language url',
                rangelength: jQuery.validator.format('Enter between {0} and {1} characters'),
                regex: 'Enter only letters, numbers, underscores and hyphens',
                remote: jQuery.validator.format('{0} is already in use')
            }
        }
    });
}(jQuery));