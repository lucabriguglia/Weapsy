weapsy.admin.menuItem = weapsy.admin.menuItem || {};

weapsy.admin.menuItem = (function ($, ko) {
    function Menu(data) {
        this.id = ko.observable(data.id);
        this.name = ko.observable(data.name);
    }

    function menuItem(data) {
        this.id = ko.observable(data.id);
        this.parentId = ko.observable(data.parentId);
        this.text = ko.observable(data.text);
        this.menuItems = ko.observableArray([]);
    }

    function MenuItemLocalisation(data) {
        var tabLanguageName = data.languageName;
        if (data.languageStatus != 1)
            tabLanguageName += " *";
        this.tabLanguageName = ko.observable(tabLanguageName);
        this.languageId = ko.observable(data.languageId);
        this.languageName = ko.observable(data.languageName);
        this.languageStatus = ko.observable(data.languageStatus);
        this.text = ko.observable(data.text);
        this.title = ko.observable(data.title);
    }

    function MenuItemPermission(data) {
        this.roleId = ko.observable(data.roleId);
        this.roleName = ko.observable(data.roleName);
        this.selected = ko.observable(data.selected);
        this.disabled = ko.observable(data.disabled);
    }

    function Page(data) {
        this.id = data.id;
        this.name = data.name;
    }

    function Language(data) {
        this.id = data.id;
        this.name = data.name;
        this.status = data.status;
    }

    function menuItemViewModel() {
        var self = this;

        self.menu = ko.observable();
        self.pages = ko.observableArray([]);
        self.languages = ko.observableArray([]);
        self.deletable = ko.observable(false);
        self.confirmDeleteMenuItemMessage = ko.observable();
        self.showEditForm = ko.observable(false);

        self.itemTypes = ko.observableArray([
            { id: 1, name: "Page" },
            { id: 2, name: "Link" },
            { id: 3, name: "Disabled" }
        ]);

        self.menuItemId = ko.observable();
        self.menuItemType = ko.observable();
        self.pageId = ko.observable();
        self.link = ko.observable();
        self.text = ko.observable();
        self.title = ko.observable();
        self.menuItemLocalisations = ko.observableArray([]);
        self.menuItemPermissions = ko.observableArray([]);
        self.menuItemLocalisationsNotActive = ko.computed(function () {
            return ko.utils.arrayFilter(self.menuItemLocalisations(), function (localisation) {
                return localisation.languageStatus() != 1;
            });
        });

        self.emptyId = "00000000-0000-0000-0000-000000000000";

        self.loadMenu = function () {
            $.getJSON("/api/menu/admin", function (data) {
                if (data.length > 0)
                    self.menu(new Menu(data[0]));
            });
        }

        self.loadPages = function () {
            $.getJSON("/api/page/admin-list", function (data) {
                var mappedPages = $.map(data, function (item) { return new Page(item) });
                self.pages(mappedPages);
                self.loadLanguages();
            });
        }

        self.loadLanguages = function () {
            $.getJSON("/api/language", function (data) {
                var mappedLanguages = $.map(data, function (item) { return new Language(item) });
                self.languages(mappedLanguages);
                var id = weapsy.utils.getUrlParameterByName("id");
                if (id)
                    self.editMenuItem(id);
                else
                    self.addMenuItem();
            });
        }

        self.addMenuItem = function () {
            $.getJSON("/api/menu/" + self.menu().id() + "/admin-edit-default-item/", function (data) {
                self.menuItemId(self.emptyId);
                self.menuItemType(self.itemTypes()[0].id);
                self.pageId(self.pages()[0].id);
                self.link("");
                self.text("");
                self.title("");
                self.menuItemLocalisations([]);
                self.menuItemPermissions([]);

                var mappedLocalisations = $.map(data.menuItemLocalisations, function (item) { return new MenuItemLocalisation(item) });
                self.menuItemLocalisations(mappedLocalisations);

                var mappedPermissions = $.map(data.menuItemPermissions, function (item) { return new MenuItemPermission(item) });
                self.menuItemPermissions(mappedPermissions);

                self.showEditForm(true);

                self.setValidator();
            });
        };

        self.editMenuItem = function (id) {
            self.deletable(true);
            $.getJSON("/api/menu/" + self.menu().id() + "/admin-edit-item/" + id, function (data) {
                self.menuItemId(id);
                self.menuItemType(data.type);
                self.pageId(data.pageId);
                self.link(data.link);
                self.text(data.text);
                self.title(data.title);
                self.menuItemLocalisations([]);
                self.menuItemPermissions([]);

                var mappedLocalisations = $.map(data.menuItemLocalisations, function (item) { return new MenuItemLocalisation(item) });
                self.menuItemLocalisations(mappedLocalisations);

                var mappedPermissions = $.map(data.menuItemPermissions, function (item) { return new MenuItemPermission(item) });
                self.menuItemPermissions(mappedPermissions);

                self.showEditForm(true);

                self.setValidator();
            });
        };

        self.setValidator = function() {
            $('#editMenuItemForm').validate({
                rules: {
                    text: {
                        required: true,
                        rangelength: [1, 100]
                    },
                    title: {
                        required: false,
                        rangelength: [1, 200]
                    }
                },
                messages: {
                    text: {
                        required: 'Enter the menu item text',
                        rangelength: jQuery.validator.format('Enter between {0} and {1} characters')
                    },
                    title: {
                        rangelength: jQuery.validator.format('Enter between {0} and {1} characters')
                    }
                },
                submitHandler: function (form) {
                    self.saveMenuItem();
                }
            });

            $(".validate-localisation-text").each(function () {
                $(this).rules("add", {
                    required: false,
                    rangelength: [1, 100],
                    messages: {
                        rangelength: jQuery.validator.format('Enter no more than {1} characters')
                    }
                });
            });

            $(".validate-localisation-title").each(function () {
                $(this).rules("add", {
                    required: false,
                    rangelength: [1, 200],
                    messages: {
                        rangelength: jQuery.validator.format('Enter no more than {1} characters')
                    }
                });
            });
        };

        self.cancel = function () {
            window.location.href = '/admin/menu';
        };

        self.saveMenuItem = function () {
            weapsy.utils.showLoading("Saving Menu Item");

            var localisations = [];
            var permissions = [];

            $.each(self.menuItemLocalisations(), function () {
                var item = this;
                localisations.push({
                    languageId: item.languageId(),
                    text: item.text(),
                    title: item.title()
                });
            });

            $.each(self.menuItemPermissions(), function () {
                var item = this;
                if (item.selected())
                    permissions.push({
                        menuItemId: self.menuItemId(),
                        roleId: item.roleId()
                    });
            });

            var data = {
                menuItemId: self.menuItemId(),
                type: self.menuItemType(),
                pageId: self.pageId(),
                link: self.link(),
                text: self.text(),
                title: self.title(),
                menuItemLocalisations: localisations,
                menuItemPermissions: permissions
            };

            var action = data.menuItemId != self.emptyId ? "update" : "add";

            $.ajax({
                type: 'PUT',
                url: "/api/menu/" + self.menu().id() + "/" + action + "item",
                data: JSON.stringify(data),
                dataType: 'json',
                contentType: 'application/json'
            }).done(function () {
                window.location.href = '/admin/menu';
            });
        }

        self.confirmMenuItemToDelete = function () {
        }

        self.deleteMenuItem = function () {
            weapsy.utils.showLoading("Deleting Menu Item");
            $.ajax({
                url: "/api/menu/" + self.menu().id() + "/item/" + self.menuItemId(),
                type: "DELETE"
            }).done(function () {
                window.location.href = '/admin/menu';
            });
        }

        self.loadMenu();
        self.loadPages();
    }

    return {
        MenuItemViewModel: function () {
            return menuItemViewModel();
        }
    }
}(jQuery, ko));

ko.applyBindings(new weapsy.admin.menuItem.MenuItemViewModel());