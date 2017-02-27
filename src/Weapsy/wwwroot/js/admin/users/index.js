weapsy.admin.userIndex = weapsy.admin.userIndex || {};

weapsy.admin.userIndex = (function ($, ko) {
    function User(data) {
        this.id = ko.observable(data.id);
        this.firstName = ko.observable(data.firstName);
        this.surname = ko.observable(data.surname);
        this.email = ko.observable(data.email);
        this.roles = ko.observable(data.roles);
        this.editUrl = "/admin/user/edit/" + data.id;
        this.rolesUrl = "/admin/user/roles/" + data.id;
    }

    function usersViewModel(recordsPerPage) {
        var self = this;

        self.pageIndex = ko.observable(0);
        self.maxPageIndex = ko.observable();

        self.startIndex = ko.observable();
        //Subrata: 27.09.2016
        //ko.observable(n) - "n" denotes records / page.
        self.numberOfUsers = ko.observable(recordsPerPage);
        
        self.users = ko.observableArray([]);
        self.allPages = ko.observableArray([]);
        self.totalRecords = ko.observable();

        self.userToDelete = ko.observable();
        self.confirmDeleteUserMessage = ko.observable();

        self.loadUsers = function () {
            self.startIndex(self.pageIndex() * self.numberOfUsers());
            $.getJSON("/api/user/admin-list?startIndex=" + self.startIndex() + "&numberOfUsers=" + self.numberOfUsers(), function (data) {
                var mappedUsers = $.map(data.users, function (item) { return new User(item) });
                self.users(mappedUsers);
                self.totalRecords = data.totalRecords;
                self.maxPageIndex(data.numberOfPages - 1);
                var pages = [];
                for (i = 0; i <= self.maxPageIndex() ; i++) {
                    pages.push({ pageNumber: (i + 1) });
                }
                self.allPages(pages);
            });
        }

        self.previousPage = function () {
            if (self.pageIndex() > 0) {
                self.pageIndex(self.pageIndex() - 1);
                self.loadUsers();
            }
        };

        self.nextPage = function () {
            if (self.pageIndex() < self.maxPageIndex()) {
                self.pageIndex(self.pageIndex() + 1);
                self.loadUsers();
            }
        };

        self.moveToPage = function (index) {
            self.pageIndex(index);
            self.loadUsers();
        };

        self.confirmUserToDelete = function (user) {
            self.userToDelete(user);
            self.confirmDeleteUserMessage("Are you sure you want to delete " + user.email() + "?");
        }

        self.deleteUser = function () {
            $.ajax({
                url: "/api/user/" + self.userToDelete().id(),
                type: "DELETE"
            }).done(function () {
                window.location.href = '/admin/user';
            });
        }

        self.loadUsers();
    }

    return {
        UsersViewModel: function () {
            // 0 for All records (no pagination)
            return usersViewModel(25);
        }
    }
}(jQuery, ko));

ko.applyBindings(new weapsy.admin.userIndex.UsersViewModel());