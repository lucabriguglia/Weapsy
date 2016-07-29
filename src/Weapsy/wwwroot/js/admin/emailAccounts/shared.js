weapsy.admin.emailAccounts = weapsy.admin.emailAccounts || {};

weapsy.admin.emailAccounts = (function ($) {
    $('.validate-form').validate({
        rules: {
            address: {
                required: true,
                rangelength: [1, 250],
                email: true,
                remote: {
                    param: {
                        url: '/api/emailAccount/IsEmailAccountAddressUnique/'
                    },
                    depends: function() {
                        return ($('#address').val() !== $('#originalAddress').val());
                    }
                }
            }
        },
        messages: {
            address: {
                required: 'Enter the email address',
                rangelength: jQuery.validator.format('Enter between {0} and {1} characters'),
                email: 'Enter a valid email address',
                remote: jQuery.validator.format('{0} is already in use')
            }
        }
    });

    return {
        redirectToIndex: function () {
            window.location.href = '/admin/emailAccount';
        }
    }
}(jQuery));