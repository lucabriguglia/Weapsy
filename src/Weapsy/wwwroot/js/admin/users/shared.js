weapsy.admin.users = weapsy.admin.users || {};

weapsy.admin.users = (function ($) {
    $('.validate-form').validate({
        rules: {
            email: {
                required: true,
                email: true,
                remote: {
                    param: {
                        url: '/api/user/IsEmailUnique/'
                    },
                    depends: function() {
                        return ($('#email').val() !== $('#originalEmail').val());
                    }
                }
            }
        },
        messages: {
            email: {
                required: 'Enter the email',
                email: 'Enter a valid email',
                remote: jQuery.validator.format('{0} is already in use')
            }
        }
    });

    return {
        redirectToIndex: function () {
            window.location.href = '/admin/user';
        }
    }
}(jQuery));