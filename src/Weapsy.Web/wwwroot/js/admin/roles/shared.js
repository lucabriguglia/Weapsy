weapsy.admin.roles = weapsy.admin.roles || {};

weapsy.admin.roles = (function ($) {
    $('.validate-form').validate({
        rules: {
            name: {
                required: true,
                rangelength: [1, 250],
                regex: /^[A-Za-z\d\s_-]+$/,
                remote: {
                    param: {
                        url: '/api/role/IsRoleNameUnique/'
                    },
                    depends: function() {
                        return ($('#name').val() !== $('#originalName').val());
                    }
                }
            }
        },
        messages: {
            name: {
                required: 'Enter the role name',
                rangelength: jQuery.validator.format('Enter between {0} and {1} characters'),
                regex: 'Enter only letters, numbers, underscores and hyphens',
                remote: jQuery.validator.format('{0} is already in use')
            }
        }
    });

    return {
        redirectToIndex: function () {
            window.location.href = '/admin/role';
        }
    }
}(jQuery));