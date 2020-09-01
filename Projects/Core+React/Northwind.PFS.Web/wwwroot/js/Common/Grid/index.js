"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
require("react-app-polyfill/ie11");
const React = require("react");
const ReactDOM = require("react-dom");
const KendoGrid_1 = require("./KendoGrid");
function showProjectFinancialSystem() {
    //Group grid setting start
    let groupRowMenu = [
        {
            text: "Edit", icon: "pencil", action: (data, grid) => {
                let editView = {
                    text: "Edit 'Group'",
                    getUrl: "/IAM/Group/Edit/" + data.dataItem.groupGuid,
                    postUrl: "/IAM/Group/Edit",
                    method: 'post',
                    buttons: [
                        { primary: true, requireValidation: true, text: "Save", action: (e, o) => { } },
                        { primary: false, requireValidation: true, text: "Cancel", action: (e, o) => { } }
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
            text: "Delete", icon: "delete", action: (data, grid) => {
                let deleteView = {
                    text: "Delete Confirmation",
                    getUrl: "/IAM/Group/Delete/" + data.dataItem.groupGuid,
                    postUrl: "/IAM/Group/Delete",
                    method: 'post',
                    buttons: [
                        { primary: true, requireValidation: true, text: "Yes", action: (e, o) => { } },
                        { text: "No", action: (e, o) => { } }
                    ],
                    redirect: false,
                    updateView: true,
                    action: function (data) {
                    }
                };
                grid.openSubmittableForm(deleteView);
            }
        },
        {
            text: "View Details", icon: "folder-open", action: (data, grid) => {
                let detailsView = {
                    text: "User Details",
                    getUrl: "/IAM/Group/Details/" + data.dataItem.groupGuid,
                    buttons: [
                        { text: "Ok", action: (e, o) => { } }
                    ],
                    redirect: false,
                    updateView: false,
                    action: function (data) {
                    }
                };
                grid.openSubmittableForm(detailsView);
            }
        }
    ];
    let groupGridMenu = [
        { text: 'Export to PDF', icon: 'pdf', action: (data, grid) => { grid.exportToPDF(); } },
        { text: 'Export to Excel', icon: 'excel', action: (data, grid) => { grid.exportToExcel(); } },
        {
            text: 'Delete Groups', icon: 'delete', action: (data, grid) => {
                grid.getSelectedItems((items) => {
                    if (items.length > 0) {
                        let deleteMassView = {
                            text: "Delete Confirmation",
                            getUrl: "/IAM/Group/DeleteBatch",
                            postData: items,
                            getMethod: 'post',
                            postUrl: "/IAM/Group/DeleteBatchPost",
                            method: 'post',
                            buttons: [
                                { primary: true, requireValidation: true, text: "Yes", action: (e, o) => { } },
                                { text: "No", action: (e, o) => { } }
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
                });
            }
        }
    ];
    let groupAddButton = {
        text: "Add Group",
        icon: "plus-sm",
        getUrl: "/IAM/Group/Add",
        postUrl: "/IAM/Group/Add",
        method: 'post',
        redirect: false,
        buttons: [
            { primary: true, requireValidation: true, text: "Save", action: (e, o) => { } },
            { primary: false, requireValidation: true, text: "Save And New", action: (e, o) => { } },
            { primary: true, text: "Cancel", action: (e, o) => { } },
        ],
        updateView: true,
        action: function (data) {
        }
    };
    //Group Grid Settings end
    //Group Grid
    ReactDOM.render(React.createElement(KendoGrid_1.KendoGrid, { dataURL: "/IAM/Group/GroupList", fieldUrl: "/GridFields/Group", exportFieldUrl: "/Export/Group", identityField: "groupGuid", rowMenus: groupRowMenu, gridMenu: groupGridMenu, gridWidth: document.getElementById("groupGrid").clientWidth, parent: "groupGrid" }), document.getElementById("groupGrid"));
}
window.pfs = { showProjectFinancialSystem: showProjectFinancialSystem };
//# sourceMappingURL=index.js.map