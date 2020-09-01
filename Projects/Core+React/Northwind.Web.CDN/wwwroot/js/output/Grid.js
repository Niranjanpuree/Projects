var $;
var GridExtention = /** @class */ (function () {
    function GridExtention() {
    }
    GridExtention.prototype.contructor = function ($) {
    };
    GridExtention.prototype.loadGridMenu = function (grid) {
        var ui = undefined;
        if (grid && grid.data) {
            ui = grid.data("kendoGrid");
        }
        else {
            ui = grid;
        }

        var gridData = [];
        if (grid.dataSource) {
            gridData = grid.dataSource;
        } else if (grid.id) {
            gridData = $("#" + grid.id).data("kendoGrid").dataSource._data;
        } else {
            gridData = $("#" + grid[0].id).data("kendoGrid").dataSource._data;
        }
        var items = [];
        ui.getOptions().events.forEach(function (item, index) {
            item.icon = item.icon || ''
            items.push({
                type: "button",
                icon: item.icon,
                text: item.title,
                overflow: "always",
                click: eval("ui.getOptions().events[" + index + "]." + item.event)
            });
        });
        grid.find(".gridToolBar").each(function (rowIndex) {
            var titems = [];
            var menuCnt = 0
            for (var d in items) {
                if (items[d].text == 'Enable' && gridData[rowIndex].isActiveStatus == 'Active') {
                    continue;
                }
                if (items[d].text == 'Disable' && gridData[rowIndex].isActiveStatus == 'Inactive') {
                    continue;
                }
                items[d].id = "mnu" + rowIndex + "_" + menuCnt;
                items[d].attributes = { "key": items[d].id };
                items[d].attributes = { "actionText": "Edit User" };
                items[d].attributes = { "dialogTitle": "Edit User" };
                items[d].attributes = { "dialogWidth": "50%" };
                items[d].attributes = { "dialogHeight": "50%" };
                items[d].attributes = { "method": "post" };
                items[d].attributes = { "getURL": "/admin/grid/add" };
                items[d].attributes = { "postURL": "/admin/grid/add" };
                titems.push(items[d]);
                titems[titems.length - 1].data = ui._data[rowIndex];
                menuCnt++;
            }
            $(this).kendoToolBar({ items: titems });
        });
    };

    GridExtention.prototype.loadGridMenuContractWithProjectMenu = function (grid) {
        var ui = undefined;
        if (grid && grid.data) {
            ui = grid.data("kendoGrid");
        }
        else {
            ui = grid;
        }
        var gridData = [];
        if (grid.dataSource) {
            gridData = grid.dataSource;
//            grid.dataSource._pageSize = gridData.length;
        } else if (grid.id) {
            gridData = $("#" + grid.id).data("kendoGrid").dataSource._data;
//            $("#" + grid.id).data("kendoGrid").dataSource._pageSize = gridData.length;
        } else {
            gridData = $("#" + grid[0].id).data("kendoGrid").dataSource._data;
//            $("#" + grid[0].id).data("kendoGrid").dataSource._pageSize = gridData.length;
        }

        var itemsContract = [];
        ui.getOptions().events.forEach(function (item, index) {
            item.icon = item.icon || ''
            if (item.objectType == 'project') {
                return;
            }
            itemsContract.push({
                type: "button",
                icon: item.icon,
                text: item.title,
                overflow: "always",
                click: eval("ui.getOptions().events[" + index + "]." + item.event)
            });
        });

        var itemsProject = [];
        ui.getOptions().events.forEach(function (item, index) {
            if (item.objectType == 'contract') {
                return;
            }
            item.icon = item.icon || ''

            itemsProject.push({
                type: "button",
                icon: item.icon,
                text: item.title,
                overflow: "always",
                click: eval("ui.getOptions().events[" + index + "]." + item.event)
            });
        });
        grid.find(".gridToolBar").each(function (rowIndex) {
            var titems = [];
            var menuCnt = 0
            if (gridData[rowIndex].isContract == 'Yes') {
                var items = itemsContract;
                for (var d in items) {
                    if (items[d].text == 'Add Task Order' && gridData[rowIndex].idiqContract =='No') {
                        continue;
                    }
                    if (items[d].text == 'Enable' && gridData[rowIndex].isActiveStatus == 'Active') {
                        continue;
                    }
                    if (items[d].text == 'Disable' && gridData[rowIndex].isActiveStatus == 'Inactive') {
                        continue;
                    }
                    items[d].id = "mnu" + rowIndex + "_" + menuCnt;
                    items[d].attributes = { "key": items[d].id };
                    items[d].attributes = { "actionText": "Edit User" };
                    items[d].attributes = { "dialogTitle": "Edit User" };
                    items[d].attributes = { "dialogWidth": "50%" };
                    items[d].attributes = { "dialogHeight": "50%" };
                    items[d].attributes = { "method": "post" };
                    items[d].attributes = { "getURL": "/admin/grid/add" };
                    items[d].attributes = { "postURL": "/admin/grid/add" };
                    titems.push(items[d]);
                    titems[titems.length - 1].data = ui._data[rowIndex];
                    menuCnt++;
                }
            } else {
                var items = itemsProject;
                for (var d in items) {
                    if (items[d].text == 'Enable' && gridData[rowIndex].isActiveStatus == 'Active') {
                        continue;
                    }
                    if (items[d].text == 'Disable' && gridData[rowIndex].isActiveStatus == 'Inactive') {
                        continue;
                    }
                    items[d].id = "mnu" + rowIndex + "_" + menuCnt;
                    items[d].attributes = { "key": items[d].id };
                    items[d].attributes = { "actionText": "Edit User" };
                    items[d].attributes = { "dialogTitle": "Edit User" };
                    items[d].attributes = { "dialogWidth": "50%" };
                    items[d].attributes = { "dialogHeight": "50%" };
                    items[d].attributes = { "method": "post" };
                    items[d].attributes = { "getURL": "/admin/grid/add" };
                    items[d].attributes = { "postURL": "/admin/grid/add" };
                    titems.push(items[d]);
                    titems[titems.length - 1].data = ui._data[rowIndex];
                    menuCnt++;
                }
            }

            $(this).kendoToolBar({ items: titems });
        });
    };
    return GridExtention;
}());