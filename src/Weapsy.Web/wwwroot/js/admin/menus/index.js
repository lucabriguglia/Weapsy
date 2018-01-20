weapsy.admin.menus = weapsy.admin.menus || {};

weapsy.admin.menus = (function ($, ko) {
    function Menu(data) {
        this.id = ko.observable(data.id);
        this.name = ko.observable(data.name);
    }

    function MenuItem(data) {
        this.id = ko.observable(data.id);
        this.parentId = ko.observable(data.parentId);
        this.text = ko.observable(data.text);
        this.editUrl = "/admin/menu/item?id=" + data.id;
        this.menuItems = ko.observableArray([]);
    }

    function menusViewModel() {
        var self = this;

        self.menu = ko.observable();
        self.menus = ko.observableArray([]);
        self.menuItems = ko.observableArray([]);
        self.menuItemToDelete = ko.observable();
        self.confirmDeleteMenuItemMessage = ko.observable();
        self.emptyId = "00000000-0000-0000-0000-000000000000";
        self.menuItemsToUpdate = ko.observableArray([]);

        self.loadMenu = function () {
            $.getJSON("/api/menu/admin", function (data) {
                var mappedMenus = $.map(data, function (item) { return new Menu(item) });
                self.menus(mappedMenus);
                if (self.menus().length > 0)
                {
                    self.menu(self.menus()[0]);
                    self.loadMenuItems(self.menu().id());
                }
            });
        }

        self.loadMenuItems = function (menuId) {
            $.getJSON("/api/menu/" + menuId + "/items", function (data) {
                self.menuItems(mapMenuItems(data));
            });
        }

        function mapMenuItems(data) {
            var menuItems = [];
            $.each(data, function () {
                var currentDataItem = this;
                var newMenuItem = new MenuItem(currentDataItem);
                if (currentDataItem.menuItems.length > 0)
                {
                    newMenuItem.menuItems.push.apply(newMenuItem.menuItems, mapMenuItems(currentDataItem.menuItems));
                }
                menuItems.push(newMenuItem);
            });
            return menuItems;
        }

        self.confirmMenuItemToDelete = function (item) {
            self.menuItemToDelete(item);
            self.confirmDeleteMenuItemMessage("Are you sure you want to delete " + item.text() + " and all descendants?");
        }

        self.deleteMenuItem = function () {
            weapsy.utils.showLoading("Deleting Menu Item");
            $.ajax({
                url: "/api/menu/" + self.menu().id() + "/item/" + self.menuItemToDelete().id(),
                type: "DELETE"
            }).done(function () {
                weapsy.utils.showSuccess("Menu Item Deleted");
                self.loadMenuItems(self.menu().id());
            });
        }

        self.loadMenu();

        $("#menuItems").nestedSortable({
            handle: 'div',
            items: 'li',
            toleranceElement: '> div',
            placeholder: 'placeholder',
            forcePlaceholderSize: true,
            opacity: .6,
            stop: function (event, ui) {
                var id = ui.item.context.id;
                var parentId = ui.item.context.parentNode.parentNode.id;
                if (parentId == "") parentId = self.emptyId;

                var menuItemToUpdate = $("#menuItems").find('li#' + id);
                menuItemToUpdate.attr('parentId', parentId);

                var menuItems = [];

                $("#menuItems").find('li').each(function () {
                    var id = $(this).attr("id");
                    var parentId = $(this).attr("parentId");
                    menuItems.push({
                        id: id,
                        parentId: parentId
                    });
                });

                self.menuItemsToUpdate(menuItems);
            }
        });

        $('#confirmReorder').click(function () {
            weapsy.utils.showLoading("Updating Order");

            $.ajax({
                url: "/api/menu/" + self.menu().id() + "/reorder",
                type: "PUT",
                data: JSON.stringify(self.menuItemsToUpdate()),
                dataType: 'json',
                contentType: 'application/json'
            }).done(function () {
                weapsy.utils.showSuccess("Order Updated");
            });
        });
    }

    return {
        MenusViewModel: function() {
            return menusViewModel();
        }
    }
}(jQuery, ko));

ko.applyBindings(new weapsy.admin.menus.MenusViewModel());
