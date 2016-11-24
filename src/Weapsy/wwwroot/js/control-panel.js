weapsy.controlPanel = weapsy.controlPanel || {};

weapsy.controlPanel = (function ($) {
    $(document).ready(function () {
        if ($('#control-panel').length)
            init();
    });

    function init() {
        $(".zone").addClass("edit-mode");

        $(".zone").each(function () {
            var name = $(this).attr("data-zone-name");
            $(this).prepend('<p class="zone-name">' + name + '</p>');
        });

        $(".moduleType").draggable({
            connectToSortable: ".zone",
            helper: "clone",
            revert: "invalid"
        });

        $(".zone").sortable({
            handle: ".handle",
            placeholder: "placeholder",
            connectWith: ".zone",
            revert: true,
            stop: function (event, ui) {
                var pageId = $("#page").attr("data-page-id");
                var moduleTypeId = $(ui.item.context).attr("data-module-type-id");

                var command;

                if (!moduleTypeId) {
                    var zones = [];
                    $(".zone").each(function () {
                        var name = $(this).attr("data-zone-name");
                        var modules = [];
                        $(this).find(".module").each(function () {
                            var moduleId = $(this).attr("data-module-id");
                            modules.push(moduleId);
                        });
                        zones.push({
                            name: name, 
                            modules: modules
                        });
                    });

                    command = {
                        pageId: pageId,
                        zones: zones
                    };

                    $.ajax({
                        url: "/api/page/" + pageId + "/reorder-modules",
                        type: "PUT",
                        data: JSON.stringify(command),
                        dataType: 'json',
                        contentType: 'application/json'
                    }).done(function () {
                    });
                }
                else {
                    $(ui.item.context).text("Saving...");
                    $(ui.item.context).css("margin-bottom", "10px");

                    var title = $(ui.item.context).attr("data-module-type-title");
                    var zone = $(ui.item[0].parentNode).attr("data-zone-name");
                    var sortOrder = ui.item.index() + 1;

                    command = {
                        pageId: pageId,
                        moduleTypeId: moduleTypeId,
                        zone: zone,
                        sortOrder: sortOrder,
                        title: title
                    };

                    $.ajax({
                        url: "/api/page/" + pageId + "/add-module",
                        type: "PUT",
                        data: JSON.stringify(command),
                        dataType: 'json',
                        contentType: 'application/json'
                    }).done(function () {
                        $(ui.item.context).removeAttr("data-module-type-id");
                        window.location.reload();
                    });
                }
            }
        });

        $('.module-edit').click(function () {
            $("#modal-title").empty();
            $("#modal-body").empty();
            var moduleId = $(this).closest(".module-data").attr("data-module-id");
            var moduleEditUrl = $(this).closest(".module-data").attr("data-module-edit-url");
            $("#modal-title").text("Edit Module");
            $("#modal-body").load("/" + moduleEditUrl, { moduleId: moduleId });
        });

        $('.module-settings').click(function () {
            $("#modal-title").empty();
            $("#modal-body").empty();
            var pageId = $("#page").attr("data-page-id");
            var pageModuleId = $(this).closest(".module-data").attr("data-page-module-id");
            $("#modal-title").text("Module Settings");
            $("#modal-body").load("/Admin/Page/EditModule", { pageId: pageId, pageModuleId: pageModuleId });
        });

        var moduleIdToDelete;

        $('.module-remove').click(function () {
            moduleIdToDelete = $(this).closest(".module-data").attr("data-module-id");
        });

        $('#confirmDeleteModule').click(function () {
            var pageId = $("#page").attr("data-page-id");

            var command = {
                pageId: pageId,
                moduleId: moduleIdToDelete
            };
            $.ajax({
                url: "/api/page/" + pageId + "/remove-module",
                type: "PUT",
                data: JSON.stringify(command),
                dataType: 'json',
                contentType: 'application/json'
            }).done(function () {
                $("#" + moduleIdToDelete).remove();
            });
        });
    }

    return {
        refresh: function () {
            window.location.reload();
        }
    };
}(jQuery));