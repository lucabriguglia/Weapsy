weapsy.admin.userIndex = weapsy.admin.userIndex || {};

weapsy.admin.userIndex = (function ($, ko) {
    function User(data) {
        this.id = ko.observable(data.id);
        this.email = ko.observable(data.email);
        this.editUrl = "/admin/user/edit/" + data.id;
        this.rolesUrl = "/admin/user/roles/" + data.id;
    }

    function usersViewModel() {
        var self = this;

        self.users = ko.observableArray([]);

        self.loadUsers = function () {
            $.getJSON("/api/user/", function (data) {
                var mappedUsers = $.map(data, function (item) { return new User(item) });
                self.users(mappedUsers);
            });
        }
    }

    var userIdToDelete;

    $('.delete-page').click(function () {
        userIdToDelete = $(this).attr("data-id");
    });

    $('#confirmDelete').click(function () {
        $.ajax({
            url: "/api/user/" + userIdToDelete,
            type: "DELETE"
        }).done(function () {
            window.location.href = '/admin/user';
        }).fail(function () {
        });
    });

    return {
        UsersViewModel: function () {
            return usersViewModel();
        }
    }
}(jQuery, ko));

ko.applyBindings(new weapsy.admin.userIndex.UsersViewModel());