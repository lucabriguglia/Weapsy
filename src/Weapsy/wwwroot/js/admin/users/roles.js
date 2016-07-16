weapsy.admin.userRoles = weapsy.admin.userRoles || {};

weapsy.admin.userRoles = (function ($) {
    $(document).ready(function () {
        init();
    });

    function init() {
        //$("#available-roles, #user-roles").sortable({
        //    connectWith: ".role-group"
        //}).disableSelection();

        //$(".role").draggable({
        //    connectToSortable: ".role-group",
        //    helper: "clone",
        //    revert: "invalid"
        //});

        $("#available-roles, #user-roles").sortable({
            handle: ".handle",
            placeholder: "placeholder",
            connectWith: ".role-group",
            revert: true,
            stop: function (event, ui) {
                var pageId = $("#page").attr("data-page-id");

                //var moduleTypeId = $(ui.item.context).attr("data-module-type-id");

                //var command = {};

                //var zones = [];
                //$(".zone").each(function () {
                //    var name = $(this).attr("data-zone-name");
                //    var modules = [];
                //    $(this).find(".module").each(function () {
                //        var moduleId = $(this).attr("data-module-id");
                //        modules.push(moduleId);
                //    });
                //    zones.push({
                //        name: name, 
                //        modules: modules
                //    });
                //});

                //command = {
                //    pageId: pageId,
                //    zones: zones
                //};

                //$.ajax({
                //    url: "/api/page/" + pageId + "/reorder-modules",
                //    type: "PUT",
                //    data: JSON.stringify(command),
                //    dataType: 'json',
                //    contentType: 'application/json'
                //}).done(function () {
                //});

            }
        });
    }
}(jQuery));