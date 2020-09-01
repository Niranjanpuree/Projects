"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var ReactDOM = require("react-dom");
require("@progress/kendo-theme-bootstrap/dist/all.css");
var KendoGrid_1 = require("./KendoGrid");
if (document.getElementById("groupGrid")) {
    //Group grid setting start
    var groupRowMenu = [
        {
            text: "Edit", icon: "pencil", action: function (data, grid) {
                var editView = {
                    text: "Edit 'Group'",
                    getUrl: "/IAM/Group/Edit/" + data.dataItem.groupGuid,
                    postUrl: "/IAM/Group/Edit",
                    method: 'post',
                    buttons: [
                        { primary: true, requireValidation: true, text: "Save", action: function (e, o) {  } },
                        { primary: true, requireValidation: true, text: "Save And New", action: function (e, o) {  } },
                        { primary: false, requireValidation: true, text: "Cancel", action: function (e, o) {  } }
                    ],
                    redirect: false,
                    updateView: true,
                    action: function (data) {
                    }
                };
                grid.openSubmittableForm(editView);
            }
        },
        {
            text: "Delete", icon: "delete", action: function (data, grid) {
                var deleteView = {
                    text: "Delete Confirmation",
                    getUrl: "/IAM/Group/Delete/" + data.dataItem.groupGuid,
                    postUrl: "/IAM/Group/Delete",
                    method: 'post',
                    buttons: [
                        { primary: true, requireValidation: true, text: "Yes", action: function (e, o) {  } },
                        { text: "No", action: function (e, o) {  } }
                    ],
                    redirect: false,
                    updateView: true,
                    action: function (data) {
                    }
                };
                grid.openSubmittableForm(deleteView);
            }
        },
        { text: "Open", icon: "folder-open", action: function (data, grid) {  } }
    ];
    var groupGridMenu = [
        { text: 'Export to PDF', icon: 'pdf', action: function (data, grid) { grid.exportToPDF('/IAM/Group/ReportFields'); } },
        { text: 'Export to Excel', icon: 'excel', action: function (data, grid) { grid.exportToExcel('/IAM/Group/ReportFields'); } },
        {
            text: 'Delete Groups', icon: 'delete', action: function (data, grid) {
                var items = grid.getSelectedItems();
                if (items.length > 0) {
                    var deleteMassView = {
                        text: "Delete Confirmation",
                        getUrl: "/IAM/Group/DeleteBatch",
                        postData: items,
                        getMethod: 'post',
                        postUrl: "/IAM/Group/DeleteBatchPost",
                        method: 'post',
                        buttons: [
                            { primary: true, requireValidation: true, text: "Yes", action: function (e, o) {  } },
                            { text: "No", action: function (e, o) {  } }
                        ],
                        redirect: false,
                        updateView: true,
                        action: function (data) {
                        }
                    };
                    grid.openSubmittableForm(deleteMassView);
                }
                else {
                    window.Dialog.alert("Please select group/groups first to delete.");
                }
            }
        }
    ];
    var groupAddButton = {
        text: "Add Group",
        icon: "plus-sm",
        getUrl: "/IAM/Group/Add",
        postUrl: "/IAM/Group/Add",
        method: 'post',
        redirect: false,
        buttons: [
            { primary: true, requireValidation: true, text: "Save", action: function (e, o) {  } },
            { primary: false, requireValidation: true, text: "Save And New", action: function (e, o) {  } },
            { primary: true, text: "Cancel", action: function (e, o) {  } },
        ],
        updateView: true,
        action: function (data) {
        }
    };
    //Group Grid Settings end
    //Group Grid
    ReactDOM.render(React.createElement(KendoGrid_1.KendoGrid, { dataURL: "/IAM/Group/GroupList", fieldUrl: "/IAM/Group/GridviewFields", rowMenus: groupRowMenu, gridMenu: groupGridMenu, addRecord: groupAddButton, gridWidth: document.getElementById("groupGrid").clientWidth }), document.getElementById("groupGrid"));
}
if (document.getElementById("userGrid")) {
    //User grid setting start
    var userRowMenu = [
        {
            text: "Edit", icon: "pencil", action: function (data, grid) {
                var editView = {
                    text: "Edit 'Group'",
                    getUrl: "/IAM/User/Edit/" + data.dataItem.groupGuid,
                    postUrl: "/IAM/User/Edit",
                    method: 'post',
                    buttons: [
                        { primary: true, requireValidation: true, text: "Save", action: function (e, o) {  } },
                        { primary: true, requireValidation: true, text: "Save And New", action: function (e, o) {  } },
                        { primary: false, requireValidation: true, text: "Cancel", action: function (e, o) {  } }
                    ],
                    redirect: false,
                    updateView: true,
                    action: function (data) {
                    }
                };
                grid.openSubmittableForm(editView);
            }
        },
        {
            text: "Delete", icon: "delete", action: function (data, grid) {
                var deleteView = {
                    text: "Delete Confirmation",
                    getUrl: "/IAM/User/Delete/" + data.dataItem.groupGuid,
                    postUrl: "/IAM/User/Delete",
                    method: 'post',
                    buttons: [
                        { primary: true, requireValidation: true, text: "Yes", action: function (e, o) {  } },
                        { text: "No", action: function (e, o) {  } }
                    ],
                    redirect: false,
                    updateView: true,
                    action: function (data) {
                    }
                };
                grid.openSubmittableForm(deleteView);
            }
        },
        { text: "Open", icon: "folder-open", action: function (data, grid) {  } }
    ];
    var userGridMenu = [
        { text: 'Export to PDF', icon: 'pdf', action: function (data, grid) { grid.exportToPDF('/IAM/User/ReportFields'); } },
        { text: 'Export to Excel', icon: 'excel', action: function (data, grid) { grid.exportToExcel('/IAM/User/ReportFields'); } },
        {
            text: 'Delete Users', icon: 'delete', action: function (data, grid) {
                
                var items = grid.getSelectedItems();
                if (items.length > 0) {
                    var deleteMassView = {
                        text: "Delete Confirmation",
                        getUrl: "/IAM/User/DeleteBatch",
                        postData: items,
                        getMethod: 'post',
                        postUrl: "/IAM/User/DeleteBatchPost",
                        method: 'post',
                        buttons: [
                            { primary: true, requireValidation: true, text: "Yes", action: function (e, o) {  } },
                            { text: "No", action: function (e, o) {  } }
                        ],
                        redirect: false,
                        updateView: true,
                        action: function (data) {
                        }
                    };
                    grid.openSubmittableForm(deleteMassView);
                }
                else {
                    window.Dialog.alert("Please select group/groups first to delete.");
                }
            }
        }
    ];
    var userAddButton = {
        text: "Add User",
        icon: "plus-sm",
        getUrl: "/IAM/User/Add",
        postUrl: "/IAM/User/Add",
        method: 'post',
        redirect: false,
        buttons: [
            { primary: true, requireValidation: true, text: "Save", action: function (e, o) {  } },
            { primary: false, requireValidation: true, text: "Save And New", action: function (e, o) {  } },
            { primary: false, text: "Cancel", action: function (e, o) {  } },
        ],
        updateView: true,
        action: function (data) {
            
        }
    };
    //User Grid Settings end
    //User Grid
    ReactDOM.render(React.createElement(KendoGrid_1.KendoGrid, { dataURL: "/IAM/User/Get", fieldUrl: "/IAM/User/GridviewFields", rowMenus: userRowMenu, gridMenu: userGridMenu, addRecord: userAddButton, gridWidth: document.getElementById("userGrid").clientWidth }), document.getElementById("userGrid"));
}
//# sourceMappingURL=index.js.map