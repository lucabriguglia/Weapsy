weapsy.controlPanel = weapsy.controlPanel || {};

weapsy.controlPanel = (function ($) {
    $(document).ready(function () {
        if ($('#control-panel').length) init();
    });

    function init() {
        var margin = $("#control-panel").css("width");
        $("#modal-admin").css("margin-left", margin);
        //$("#wrapper").css("margin-left", margin);

        $(".zone").addClass("edit-mode");

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

                var command = {};

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
            var pageId = $("#page").attr("data-page-id");
            var moduleId = $(this).closest(".module-data").attr("data-module-id");
            var moduleEditUrl = $(this).closest(".module-data").attr("data-module-edit-url");
            $("#modal-title").text("Edit Module");
            $("#modal-body").load(moduleEditUrl, { moduleId: moduleId });
        });

        $('.module-settings').click(function () {
            $("#modal-title").empty();
            $("#modal-body").empty();
            var pageId = $("#page").attr("data-page-id");
            var moduleId = $(this).closest(".module-data").attr("data-module-id");
            $("#modal-title").text("Module Settings");
            $("#modal-body").load("/Admin/Module/Settings"/*, { pageId: pageId, moduleId: moduleId }*/);
        });

        $('.module-remove').click(function () {
            var pageId = $("#page").attr("data-page-id");
            var moduleId = $(this).closest(".module-data").attr("data-module-id");
            var command = {
                pageId: pageId,
                moduleId: moduleId
            };
            $.ajax({
                url: "/api/page/" + pageId + "/remove-module",
                type: "PUT",
                data: JSON.stringify(command),
                dataType: 'json',
                contentType: 'application/json'
            }).done(function () {
                $("#" + moduleId).remove();
            });
        });
    }

    return {
        refresh: function () {
            window.location.reload();
        }
    };
}(jQuery));