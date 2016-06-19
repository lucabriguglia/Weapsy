weapsy.admin.moduleTypes = weapsy.admin.moduleTypes || {};

weapsy.admin.moduleTypes = (function ($) {
    $('.validate-form').validate({
        rules: {
            name: {
                required: true,
                rangelength: [1, 100],
                regex: /^[A-Za-z\d\s_-]+$/,
                remote: {
                    param: {
                        url: '/api/moduleType/IsModuleTypeNameUnique/'
                    },
                    depends: function() {
                        return ($('#name').val() !== $('#originalName').val());
                    }
                }
            },
            viewName: {
                required: true,
                rangelength: [1, 100],
                remote: {
                    param: {
                        url: '/api/moduleType/IsModuleTypeViewNameUnique/'
                    },
                    depends: function () {
                        return ($('#viewName').val() !== $('#originalViewName').val() && $('#viewType').val() == "1");
                    }
                }
            }
        },
        messages: {
            name: {
                required: 'Enter the module type name',
                rangelength: jQuery.validator.format('Enter between {0} and {1} characters'),
                regex: 'Enter only letters, numbers, underscores and hyphens',
                remote: jQuery.validator.format('{0} is already in use')
            },
            viewName: {
                required: 'Enter the ViewName',
                rangelength: jQuery.validator.format('Enter between {0} and {1} characters'),
                remote: jQuery.validator.format('{0} is already in use')
            }
        }
    });

    return {
        redirectToIndex: function () {
            window.location.href = '/admin/moduleType';
        }
    }
}(jQuery));