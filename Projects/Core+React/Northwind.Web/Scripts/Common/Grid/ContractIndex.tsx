import * as React from "react";
import * as ReactDOM from "react-dom";
// import '@progress/kendo-theme-bootstrap/dist/all.css';
// import '../../../wwwroot/scss/custom/northwind.scss';
import { KendoGrid } from "./KendoGrid";
import { KendoGroupableGrid } from "./KendoGroupableGrid";
import { KendoGroupableGrid1 } from "./KendoGroupableGrid1";
import { GridOptions } from "./GridOptions"
import { Remote } from "../Remote/Remote";
import { generatePath } from "react-router";
declare var window: any;


function loadContractList() {

    let kendoGrid: any = {}

    let userRowMenu = [
        {
            text: "Details", icon: "info", conditions: [{ field: 'isContract', value: true }], action: (data: any, grid: any) => {
                window.location = "/contract/Details/" + data.dataItem.id
            }
        },
        {
            text: "Edit", icon: "pencil", conditions: [{ field: 'isContract', value: true }], action: (data: any, grid: any) => {
                window.location = "/contract/Edit/" + data.dataItem.id
            }
        },
        {
            text: "Delete", icon: "delete", conditions: [{ field: 'isContract', value: true }], action: (data: any, grid: any) => {
                let ids: any[] = [];
                ids.push(data.dataItem.id);
                window.Dialog.confirm({
                    text: "Are you sure you want to  delete contract?",
                    title: "Confirm",
                    ok: function (e: any) {
                        Remote.postData('/contract/Delete', ids, (e: any) => { grid.refresh() }, (err: any) => { window.Dialog.alert(err) });
                    },
                    cancel: function (e: any) {
                    }
                });
            }
        },
        {
            text: "Disable", icon: "cancel", conditions: [{ field: 'isContract', value: true }, { field: 'isActive', value: true }], action: (data: any, grid: any) => {
                var ids: any[] = [];
                ids.push(data.dataItem.id);
                window.Dialog.confirm({
                    text: "Are you sure you want to  disable contract?",
                    title: "Confirm",
                    ok: function (e: any) {
                        Remote.postData('/contract/Disable', ids, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
                    },
                    cancel: function (e: any) {
                    }
                });
            }
        },
        {
            text: "Enable", icon: "check", conditions: [{ field: 'isContract', value: true }, { field: 'isActive', value: false }], action: (data: any, grid: any) => {
                var ids: any[] = [];
                ids.push(data.dataItem.id);
                window.Dialog.confirm({
                    text: "Are you sure you want to  enable contract?",
                    title: "Confirm",
                    ok: function (e: any) {
                        Remote.postData('/contract/Enable', ids, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
                    },
                    cancel: function (e: any) {
                    }
                });
            }
        },
        {
            text: "Add Mod", icon: "plus", conditions: [{ field: 'isContract', value: true }], action: (data: any, grid: any) => {
                data.url = "/contractModification/add?contractGuid=" + data.dataItem.id;
                data.submitURL = "/contractModification/add";

                var options = {
                    title: 'Add Mod',
                    events: [
                        {
                            text: "Save",
                            primary: true,
                            action: function (e: any, values: any) {
                                grid.refresh()
                            }
                        },
                        {
                            text: "Cancel",
                            action: function (e: any) {
                            }
                        }
                    ]
                };
                window.Dialog.openDialogSubmit(data, options);
            }
        },
        {
            text: "Add Mod", icon: "plus", conditions: [{ field: 'isContract', value: false }], action: (data: any, grid: any) => {
                data.url = "/projectModification/add?projectGuid=" + data.dataItem.id;
                data.submitURL = "/projectModification/add";

                var options = {
                    title: 'Add Mod',
                    events: [
                        {
                            text: "Save",
                            primary: true,
                            action: function (e: any, values: any) {
                                grid.refresh()
                            }
                        },
                        {
                            text: "Cancel",
                            action: function (e: any) {
                            }
                        }
                    ]
                };
                window.Dialog.openDialogSubmit(data, options);
            }
        },
        {
            text: "Add Task Order", icon: "plus", conditions: [{ field: 'isContract', value: true }], action: (data: any, grid: any) => {
                window.location = "/project/add?contractGuid=" + data.dataItem.contractGuid
            }
        },
        {
            text: "Details", icon: "info", conditions: [{ field: 'isContract', value: false }], action: (data: any, grid: any) => {
                window.location = "/project/Details/" + data.dataItem.projectGuid
            }
        },
        {
            text: "Edit", icon: "pencil", conditions: [{ field: 'isContract', value: false }], action: (data: any, grid: any) => {
                window.location = "/project/Edit/" + data.dataItem.projectGuid
            }
        },
        {
            text: "Delete", icon: "delete", conditions: [{ field: 'isContract', value: false }], action: (data: any, grid: any) => {
                let ids: any[] = [];
                ids.push(data.dataItem.projectGuid);
                window.Dialog.confirm({
                    text: "Are you sure you want to  delete project?",
                    title: "Confirm",
                    ok: function (e: any) {
                        Remote.postData('/project/Delete', ids, (e: any) => { grid.refresh() }, (err: any) => { window.Dialog.alert(err) });
                    },
                    cancel: function (e: any) {
                    }
                });
            }
        },
        {
            text: "Disable", icon: "cancel", conditions: [{ field: 'isContract', value: false }, { field: 'isActive', value: true }], action: (data: any, grid: any) => {
                var ids: any[] = [];
                ids.push(data.dataItem.id);
                window.Dialog.confirm({
                    text: "Are you sure you want to  disable contract?",
                    title: "Confirm",
                    ok: function (e: any) {
                        Remote.postData('/contract/Disable', ids, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
                    },
                    cancel: function (e: any) {
                    }
                });
            }
        },
        {
            text: "Enable", icon: "check", conditions: [{ field: 'isContract', value: false }, { field: 'isActive', value: false }], action: (data: any, grid: any) => {
                var ids: any[] = [];
                ids.push(data.dataItem.id);
                window.Dialog.confirm({
                    text: "Are you sure you want to  enable contract?",
                    title: "Confirm",
                    ok: function (e: any) {
                        Remote.postData('/contract/Enable', ids, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
                    },
                    cancel: function (e: any) {
                    }
                });
            }
        }
    ];

    let userGridMenu = [
        { text: 'Export to PDF', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
        { text: 'Export to Excel', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() } },
        { text: 'Delete', icon: 'delete', action: (data: any, grid: any) => { grid.exportToExcel() } },
        { text: 'Disable', icon: 'cancel', action: (data: any, grid: any) => { grid.exportToExcel() } },
        { text: 'Enable', icon: 'check', action: (data: any, grid: any) => { grid.exportToExcel() } }
    ];

    let userAddButton = {
        text: "Add Contract",
        icon: "plus-sm",
        action: (data: any, grid: any) => {
            window.location = "/contract/add";
        }
    }
    var hideColumns = ['projectNumber'];
    var switchButton = {
        onLabel: "Hide Projects",
        offLabel: "Show Projects",
        checked: false,
        action: (status: any) => {
            if (status === true) {
                ReactDOM.unmountComponentAtNode(document.getElementById("ContractGrid1"));
                switchButton.checked = true;
                let grd1: any = ReactDOM.render(<KendoGroupableGrid key="1" dataUrl={"/contract/GetContracts?switchOn=1"} columnUrl={"/GridFields/Contract"} exportFieldUrl="/Export/Contract" identityField="contractGuid" groupField="contractNumber" rowMenus={userRowMenu} hideColumns={hideColumns} gridMenu={userGridMenu} switchButton={switchButton} showGridAction={true} showSearchBox={false} addRecord={userAddButton} showGroupHeader={true} container="ContractGrid1" />, document.getElementById("ContractGrid1"));
                grd1.refresh();
                return grd1;
            }
            else {
                ReactDOM.unmountComponentAtNode(document.getElementById("ContractGrid1"));
                switchButton.checked = false;
                let grd: any = ReactDOM.render(<KendoGrid key="1" dataURL={"/contract/GetContracts"} fieldUrl={"/GridFields/Contract"} exportFieldUrl="/Export/Contract" identityField="contractGuid" rowMenus={userRowMenu} hideColumns={hideColumns} gridMenu={userGridMenu} showGridAction={true} showSearchBox={true} addRecord={userAddButton} parent="ContractGrid" switchButton={switchButton} gridWidth={document.getElementById("ContractGrid1").clientWidth} />, document.getElementById("ContractGrid1"));
                grd.refresh();
                return grd;
            }
        }
    }

    return ReactDOM.render(<KendoGrid key="1" dataURL={"/contract/GetContracts"} fieldUrl={"/GridFields/Contract"} exportFieldUrl="/Export/Contract" identityField="contractGuid" rowMenus={userRowMenu} hideColumns={hideColumns} gridMenu={userGridMenu} showGridAction={true} showSearchBox={true} addRecord={userAddButton} parent="ContractGrid" switchButton={switchButton} gridWidth={document.getElementById("ContractGrid1").clientWidth} />, document.getElementById("ContractGrid1"));
}

//export default loadContractList;