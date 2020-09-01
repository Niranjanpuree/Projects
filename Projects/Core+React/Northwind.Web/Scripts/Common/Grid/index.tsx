import 'react-app-polyfill/ie11';
import * as React from "react";
import * as ReactDOM from "react-dom";

import { KendoGrid } from "./KendoGrid";
import { KendoGroupableGrid, RowIconSettings, IconSetting } from "./KendoGroupableGrid";
import { KendoGroupableGrid1 } from "./KendoGroupableGrid1";
import { GridOptions } from "./GridOptions"
import { Remote } from "../Remote/Remote";
import { generatePath } from "react-router";
import { DistributionListDialog } from "../../Component/DistributionList/DistributionListDialog";
import { Condition } from "../../Component/Entities/Condition";
import { GroupManagement } from "../../Component/GroupManagement/GroupManagement"
import { KendoDialog } from '../Dialog/Dialog';
import { debug } from 'webpack';

declare var window: any;
declare var $: any;
declare var reloadRevenuerecognition: any;
declare var groupPermissionView: any;


if (document.getElementById("groupGrid")) {
    //Group grid setting start
    let groupRowMenu = [

        {
            text: "Manage User(s) and Permission", resource: 'Admin', resourceAction: 'List', icon: "plus", action: (data: any, grid: any) => {

                //let detailsView = {
                //    text: "User Assigment to Group",
                //    getUrl: "/IAM/Group/Details/" + data.dataItem.groupGuid,
                //    method: 'post',
                //    postUrl: "/IAM/Group/ResourceGroup?groupGuid=" + data.dataItem.groupGuid,
                //    buttons: [
                //        { text: "Save", primary: true, requireValidation: true, action: (e: any, o: any) => { } },
                //        { text: "Close", action: (e: any, o: any) => { } }
                //    ],
                //    redirect: false,
                //    updateView: true,
                //    action: function (data: any) {

                //    }
                //}
                //grid.openSubmittableForm(detailsView, { dialogWidth: '70%', dialogHeight: '70%' });
                window.location = "/IAM/Group/Details/" + data.dataItem.groupGuid

            }
        },
        {
            text: "Edit", icon: "pencil", resource: 'Admin', resourceAction: 'List', action: (data: any, grid: any) => {
                let editView = {
                    text: "Edit 'Group'",
                    getUrl: "/IAM/Group/Edit/" + data.dataItem.groupGuid,
                    postUrl: "/IAM/Group/Edit",
                    method: 'post',
                    buttons: [
                        { primary: true, requireValidation: true, text: "Save", action: (e: any, o: any) => { groupPermissionView.onSubmit() } },
                        { primary: false, requireValidation: false, text: "Cancel", action: (e: any, o: any) => { } }
                    ],
                    redirect: false,
                    updateView: true,
                    action: function (data: any) {

                    }
                }
                let g: KendoGrid = grid;
                grid.openSubmittableForm(editView)
            }
        },
        {
            text: "Delete", icon: "delete", resource: 'Admin', resourceAction: 'List', action: (data: any, grid: any) => {
                let deleteView = {
                    text: "Delete Confirmation",
                    getUrl: "/IAM/Group/Delete/" + data.dataItem.groupGuid,
                    postUrl: "/IAM/Group/Delete",
                    method: 'post',
                    buttons: [
                        { primary: true, requireValidation: true, text: "Yes", action: (e: any, o: any) => { } },
                        { text: "No", action: (e: any, o: any) => { } }
                    ],
                    redirect: false,
                    updateView: true,
                    action: function (data: any) {

                    }
                }
                grid.openSubmittableForm(deleteView);
            }
        }
    ];

    let groupGridMenu = [
        { text: 'Export to PDF', resource: 'Admin', resourceAction: 'List', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
        { text: 'Export to Excel', resource: 'Admin', resourceAction: 'List', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() } },
        {
            text: 'Delete Groups', resource: 'Admin', resourceAction: 'List', icon: 'delete', action: (data: any, grid: any) => {
                grid.getSelectedItems((items: any[]) => {
                    if (items.length > 0) {
                        let deleteMassView = {
                            text: "Delete Confirmation",
                            getUrl: "/IAM/Group/DeleteBatch",
                            postData: items,
                            getMethod: 'post',
                            postUrl: "/IAM/Group/DeleteBatchPost",
                            method: 'post',
                            buttons: [
                                { primary: true, requireValidation: true, text: "Yes", action: (e: any, o: any) => { } },
                                { text: "No", action: (e: any, o: any) => { } }
                            ],
                            redirect: false,
                            updateView: true,
                            action: function (data: any) {

                            }
                        }
                        grid.openSubmittableForm(deleteMassView);
                    }
                    else {
                        window.Dialog.alert("Please select group/groups first to delete.")
                    }
                });

            }
        }
    ];

    let groupAddButton = {
        resource: 'Admin',
        resourceAction: 'List',
        text: "Add Group",
        icon: "plus-sm",
        getUrl: "/IAM/Group/Add",
        postUrl: "/IAM/Group/Add",
        method: 'post',
        redirect: false,
        buttons: [
            { primary: true, requireValidation: true, text: "Save", action: (e: any, o: any) => { } },
            { primary: false, requireValidation: true, saveAndNew: true, text: "Save And New", action: (e: any, o: any) => { } },
            { primary: true, text: "Cancel", action: (e: any, o: any) => { } },
        ],
        updateView: true,
        action: function (data: any) {

        }
    }
    //Group Grid Settings end
    //Group Grid
    ReactDOM.render(<KendoGrid dataURL="/IAM/Group/GroupList" fieldUrl="/GridFields/Group" exportFieldUrl="/Export/Group" identityField="groupGuid" rowMenus={groupRowMenu} gridMenu={groupGridMenu} addRecord={groupAddButton} advancedSearchEntity={["Group"]} gridWidth={document.getElementById("groupGrid").clientWidth} parent="groupGrid" />, document.getElementById("groupGrid"));
}

if (document.getElementById("userGrid")) {
    //User grid setting start
    let kendoGrid: any = {}
    let userRowMenu = [
        {
            text: "View Details", icon: "folder-open", action: (data: any, grid: any) => {
                let detailsView = {
                    text: "User Details : " + data.dataItem.name,
                    getUrl: "/IAM/User/Details/" + data.dataItem.userGuid,
                    buttons: [
                        { text: "Ok", action: (e: any, o: any) => { } }
                    ],
                    redirect: false,
                    updateView: false,
                    action: function (data: any) {

                    }
                }
                grid.openSubmittableForm(detailsView, { dialogWidth: '70%', dialogHeight: '70%' });
            }
        }
    ];

    let userGridMenu = [
        { text: 'Export to PDF', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
        { text: 'Export to Excel', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() } }
    ];

    let userAddButton = {
        text: "Add User",
        icon: "plus-sm",
        getUrl: "/IAM/User/Add",
        postUrl: "/IAM/User/Add",
        method: 'post',
        redirect: false,
        buttons: [
            { primary: true, requireValidation: true, text: "Save", action: (e: any, o: any) => { } },
            {
                primary: false, requireValidation: true, text: "Save And New", action: (e: any, o: any) => {
                    kendoGrid.addRecordClicked({})
                }
            },
            { primary: false, text: "Cancel", action: (e: any, o: any) => { } },
        ],
        updateView: true,
        action: function (data: any) {

        }
    }
    //User Grid Settings end
    //User Grid
    kendoGrid = ReactDOM.render(<KendoGrid dataURL="/IAM/User/GetUsers" identityField="userGuid" fieldUrl="/GridFields/User" rowMenus={userRowMenu} exportFieldUrl="/Export/User" advancedSearchEntity={["User"]} gridMenu={userGridMenu} gridWidth={document.getElementById("userGrid").clientWidth} parent="userGrid" />, document.getElementById("userGrid"));
}

function loadNotification(key: any, resourceId: any, showNotifyInstructionText?: boolean, showSuccessMessageAndNotifyInstructionText?: boolean) {
    //call the notification component..
    ReactDOM.unmountComponentAtNode(document.getElementById('distributionList'));
    ReactDOM.render(<DistributionListDialog notificationTemplateKey={key}
        resourceId={resourceId}
        showCreateDistributionLink={true}
        showAdditionalNote={true}
        showOnNotifySkipButton={false}
        showAddNewButton={false}
        showAdditionalIndividualRecipient={true}
        showNotifyInstructionText={showNotifyInstructionText}
        showSuccessMessageAndNotifyInstructionText={showSuccessMessageAndNotifyInstructionText}
        parentDomId='distributionList' />,
        document.getElementById('distributionList'));
}

function loadFileUpload(resourceId: any, uploadPath: any, overRideProp: boolean, parentid?: any, contractResourceFileKey?: string, ContentResourceGuid?: any, callback?: any) {
    //$("#loadingFileUpload").show();

    //var uploader = window.loadFileUpload.pageView.loadFileUpload('fileUpload', isCallBackNecessary,false, resourceId);
    ////finally saved the uploaded file in the server
    //uploader.onSubmitFiles(uploadPath);
    window.uploaderMod.onSubmitFiles(resourceId, uploadPath, overRideProp, parentid, contractResourceFileKey, ContentResourceGuid, callback);
}


function loadProjectAndMod(projectGuid: string, contractGuid: string, currencySymbol: string, pdfTitle: string, userFullname: string) {
    //User grid setting start
    let kendoGrid: any = {}

    let userRowMenu = [
        {
            text: "Details", resource: 'Contract', resourceAction: 'Details', icon: "info", conditions: [{ field: 'isMod', value: true }], action: (data: any, grid: any) => {
                data.url = "/ProjectModification/Details/" + data.dataItem.id;
                var options = {
                    title: 'Details of ' + data.dataItem.title + ' - ' + data.dataItem.mod,
                    height: '85%',
                    events: [
                        {
                            text: "Edit",
                            primary: true,
                            requireValidation: false,
                            action: function () {
                                loadModificationEdit(data, grid, "ProjectModification");
                            }
                        },
                        {
                            text: "Cancel",
                            action: function (e: any) {
                                $('html').removeClass('htmlClass');
                            }
                        }
                    ]
                };
                window.Dialog.openDialog(data, options);
            }
        },
        {
            text: "Edit", resource: 'Contract', resourceAction: 'Edit', icon: "pencil", conditions: [{ field: 'isMod', value: true }], action: (data: any, grid: any) => {
                data.url = "/ProjectModification/Edit/" + data.dataItem.id;
                data.submitURL = "/ProjectModification/Edit";
                let senderData = data;
                var options = {
                    title: 'Edit Mod : ' + data.dataItem.title + ' - ' + data.dataItem.mod,
                    height: '85%',
                    events: [
                        {
                            text: "Save",
                            primary: true,
                            action: function (e: any, values: any) {

                                loadFileUpload(values.contractGuid, values.uploadPath, false, '', 'ContractModification', values.resourceId)

                                //loadNotification("ProjectModification.Edit", data.dataItem.id, false, true);
                                grid.refresh(senderData.menuController);
                                if (reloadRevenuerecognition) {
                                    reloadRevenuerecognition(values);
                                }
                            }
                        },
                        {
                            text: "Cancel",
                            action: function (e: any) {
                                $('html').removeClass('htmlClass');
                            }
                        }
                    ]
                };
                window.Dialog.openDialogSubmit(data, options);
            }
        },
        {
            text: "Delete", resource: 'Contract', resourceAction: 'Delete', icon: "delete", conditions: [{ field: 'isMod', value: true }], action: (data: any, grid: any) => {
                let ids: any[] = [];
                ids.push(data.dataItem.id)
                let senderData = data;
                window.Dialog.confirm({
                    text: "Are you sure you want to  delete Mod?",
                    title: "Confirm",
                    ok: function (e: any) {
                        Remote.postData('/ProjectModification/Delete', ids, (data: any) => { grid.refresh(senderData.menuController); }, (err: any) => { window.Dialog.alert(err) })
                    },
                    cancel: function (e: any) {
                    }
                });
            }
        },
        {
            text: "Details", resource: 'Contract', resourceAction: 'Details', icon: "info", conditions: [{ field: 'isMod', value: false }], action: (data: any, grid: any) => {
                window.location = "/Project/Details/" + data.dataItem.id
            }
        },
        {
            text: "Edit", resource: 'Contract', resourceAction: 'Edit', icon: "pencil", conditions: [{ field: 'isMod', value: false }], action: (data: any, grid: any) => {
                window.location = "/Project/Edit/" + data.dataItem.id
            }
        },
        {
            text: "Delete", resource: 'Contract', resourceAction: 'Delete', icon: "delete", conditions: [{ field: 'isMod', value: false }], action: (data: any, grid: any) => {
                let ids: any[] = [];
                ids.push(data.dataItem.id);
                window.Dialog.confirm({
                    text: "Are you sure you want to  delete Project?",
                    title: "Confirm",
                    ok: function (e: any) {
                        Remote.postData('/Project/Delete', ids, (e: any) => { grid.refresh() }, (err: any) => { window.Dialog.alert(err) });
                    },
                    cancel: function (e: any) {
                    }
                });
            }
        },
        {
            text: "Disable", resource: 'Contract', resourceAction: 'Edit', icon: "cancel", conditions: [{ field: 'isMod', value: false }, { field: 'status', value: true }], action: (data: any, grid: any) => {
                var ids: any[] = [];
                ids.push(data.dataItem.id);
                window.Dialog.confirm({
                    text: "Are you sure you want to  disable Project?",
                    title: "Confirm",
                    ok: function (e: any) {
                        Remote.postData('/Project/Disable', ids, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
                    },
                    cancel: function (e: any) {
                    }
                });
            }
        },
        {
            text: "Enable", resource: 'Contract', resourceAction: 'Edit', icon: "check", conditions: [{ field: 'isMod', value: false }, { field: 'status', value: false }], action: (data: any, grid: any) => {
                var ids: any[] = [];
                ids.push(data.dataItem.id);
                window.Dialog.confirm({
                    text: "Are you sure you want to  enable Project?",
                    title: "Confirm",
                    ok: function (e: any) {
                        Remote.postData('/Project/Enable', ids, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
                    },
                    cancel: function (e: any) {
                    }
                });
            }
        },
        {
            text: "Add Mod", resource: 'Contract', resourceAction: 'Add', icon: "plus", conditions: [{ field: 'isMod', value: false }], action: (data: any, grid: any) => {

                data.url = "/projectModification/add?projectGuid=" + data.dataItem.id;
                data.submitURL = "/projectModification/add";

                var options = {
                    title: 'Add Mod :' + data.dataItem.projectNumber + ' ' + data.dataItem.title,
                    height: '85%',
                    events: [
                        {
                            text: "Save",
                            primary: true,
                            action: function (e: any, values: any) {

                                loadFileUpload(values.contractGuid, values.uploadPath, false, '', 'ContractModification', values.resourceId)

                                //loadNotification("ProjectModification.Create", values.resourceId, false, true);
                                grid.refresh();
                            }
                        },
                        {
                            text: "Cancel",
                            action: function (e: any) {
                                $('html').removeClass('htmlClass');
                            }
                        }
                    ]
                };
                window.Dialog.openDialogSubmit(data, options);
            }
        }
    ];

    let userGridMenu = [
        { text: 'Export to PDF', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
        { text: 'Export to Excel', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() } }
    ];

    let userAddButton = {
        text: "Add Mod",
        icon: "plus-sm",
        resource: 'Contract', resourceAction: 'Add',
        action: function (data: any, grid: any) {

            let dlgData = { url: '', submitURL: '' };
            dlgData.url = "/projectModification/add?projectGuid=" + projectGuid;
            dlgData.submitURL = "/projectModification/add";

            var options = {
                type: 'post',
                title: 'Add Mod',
                height: '85%',
                events: [
                    {
                        text: "Save",
                        primary: true,
                        action: function (e: any, values: any) {

                            loadFileUpload(values.contractGuid, values.uploadPath, false, '', 'ContractModification', values.resourceId)

                            //loadNotification("ProjectModification.Create", values.resourceId, false, true);
                            grid.refresh();
                            if (reloadRevenuerecognition) {
                                reloadRevenuerecognition(values);
                            }
                        }
                    },
                    {
                        text: "Cancel",
                        action: function (e: any) {
                        }
                    }
                ]
            };
            window.Dialog.openDialogSubmit(dlgData, options);
        }
    }

    var hideColumns = ['projectNumber'];
    var groupTotalFields = ['amount', "fundingAmount"]
    return ReactDOM.render(<KendoGroupableGrid gridHeight={400} printTitle={pdfTitle} printedBy={userFullname} key="1" dataUrl={"/Project/GetProjectWithMods?projectGuid=" + projectGuid} columnUrl={"/Project/GetProjectWithModFields?contractGuid=" + contractGuid} exportFieldUrl={"/Project/GetProjectWithModReportFields?contractGuid=" + contractGuid} identityField="id" groupField="projectNumber" rowMenus={userRowMenu} hideColumns={hideColumns} gridMenu={userGridMenu} showColumnMenu={false} showGridAction={true} showSearchBox={false} addRecord={userAddButton} showGroupHeader={false} currencySymbol={currencySymbol} groupTotalFields={groupTotalFields} groupTotalLabelField="endDate" container="projectAndModList" />, document.getElementById("projectAndModList"));
    //return ReactDOM.render(<KendoGrid dataURL={"/Project/GetProjectWithMods?projectGuid=" + projectGuid} fieldUrl="/Project/GetProjectWithModFields" hideColumns={hideColumns} exportFieldUrl="/Project/GetProjectWithModReportFields" identityField="id" rowMenus={userRowMenu} gridMenu={userGridMenu} showGridAction={true} showSearchBox={false} addRecord={userAddButton} gridWidth={document.getElementById("projectAndModList").clientHeight} parent={"projectAndModList"} />, document.getElementById("projectAndModList"));
}


function loadProjectsAndMods(ContractGuid: string, currencySymbol: string, pdfTitle: string, userFullname: string) {
    //User grid setting start
    let kendoGrid: any = {}

    let userRowMenu = [
        {
            text: "Details", resource: 'Contract', resourceAction: 'Details', icon: "info", conditions: [{ field: 'isMod', value: true }], action: (data: any, grid: any) => {
                data.url = "/ProjectModification/Details/" + data.dataItem.id;
                var options = {
                    title: 'Details of ' + data.dataItem.title + ' - ' + data.dataItem.mod,
                    height: '85%',
                    events: [
                        {
                            text: "Cancel",
                            action: function (e: any) {
                                $('html').removeClass('htmlClass');
                            }
                        }
                    ]
                };
                window.Dialog.openDialog(data, options);
            }
        },
        {
            text: "Edit", resource: 'Contract', resourceAction: 'Edit', icon: "pencil", conditions: [{ field: 'isMod', value: true }], action: (data: any, grid: any) => {
                data.url = "/ProjectModification/Edit/" + data.dataItem.id;
                data.submitURL = "/ProjectModification/Edit";
                let senderData = data;
                var options = {
                    title: 'Edit Mod : ' + data.dataItem.title + ' - ' + data.dataItem.mod,
                    height: '85%',
                    events: [
                        {
                            text: "Save",
                            primary: true,
                            action: function (e: any, values: any) {

                                loadFileUpload(values.contractGuid, values.uploadPath, false, '', 'ContractModification', values.resourceId)

                                //loadNotification("ProjectModification.Edit", data.dataItem.id, false, true);
                                grid.refresh(senderData.menuController);
                            }
                        },
                        {
                            text: "Cancel",
                            action: function (e: any) {
                                $('html').removeClass('htmlClass');
                            }
                        }
                    ]
                };
                window.Dialog.openDialogSubmit(data, options);
            }
        },
        {
            text: "Delete", resource: 'Contract', resourceAction: 'Delete', icon: "delete", conditions: [{ field: 'isMod', value: true }], action: (data: any, grid: any) => {
                let ids: any[] = [];
                ids.push(data.dataItem.id)
                let senderData = data;
                window.Dialog.confirm({
                    text: "Are you sure you want to  delete Mod?",
                    title: "Confirm",
                    ok: function (e: any) {
                        Remote.postData('/ProjectModification/Delete', ids, (data: any) => { grid.refresh(senderData.menuController); }, (err: any) => { window.Dialog.alert(err) })
                    },
                    cancel: function (e: any) {
                    }
                });
            }
        },
        {
            text: "Details", resource: 'Contract', resourceAction: 'Details', icon: "info", conditions: [{ field: 'isMod', value: false }], action: (data: any, grid: any) => {
                window.location = "/Project/Details/" + data.dataItem.id
            }
        },
        {
            text: "Edit", resource: 'Contract', resourceAction: 'Edit', icon: "pencil", conditions: [{ field: 'isMod', value: false }], action: (data: any, grid: any) => {
                window.location = "/Project/Edit/" + data.dataItem.id
            }
        },
        {
            text: "Delete", resource: 'Contract', resourceAction: 'Delete', icon: "delete", conditions: [{ field: 'isMod', value: false }], action: (data: any, grid: any) => {
                let ids: any[] = [];
                ids.push(data.dataItem.id);
                window.Dialog.confirm({
                    text: "Are you sure you want to  delete Project?",
                    title: "Confirm",
                    ok: function (e: any) {
                        Remote.postData('/Project/Delete', ids, (e: any) => { grid.refresh() }, (err: any) => { window.Dialog.alert(err) });
                    },
                    cancel: function (e: any) {
                    }
                });
            }
        },
        {
            text: "Disable", resource: 'Contract', resourceAction: 'Edit', icon: "cancel", conditions: [{ field: 'isMod', value: false }, { field: 'status', value: true }], action: (data: any, grid: any) => {
                var ids: any[] = [];
                ids.push(data.dataItem.id);
                window.Dialog.confirm({
                    text: "Are you sure you want to  disable Project?",
                    title: "Confirm",
                    ok: function (e: any) {
                        Remote.postData('/Project/Disable', ids, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
                    },
                    cancel: function (e: any) {
                    }
                });
            }
        },
        {
            text: "Enable", resource: 'Contract', resourceAction: 'Edit', icon: "check", conditions: [{ field: 'isMod', value: false }, { field: 'status', value: false }], action: (data: any, grid: any) => {
                var ids: any[] = [];
                ids.push(data.dataItem.id);
                window.Dialog.confirm({
                    text: "Are you sure you want to  enable Project?",
                    title: "Confirm",
                    ok: function (e: any) {
                        Remote.postData('/Project/Enable', ids, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
                    },
                    cancel: function (e: any) {
                    }
                });
            }
        },
        {
            text: "Add Mod", resource: 'Contract', resourceAction: 'Add', icon: "plus", conditions: [{ field: 'isMod', value: false }], action: (data: any, grid: any) => {

                grid.senderData = data;
                data.url = "/projectModification/add?projectGuid=" + data.dataItem.id;
                data.submitURL = "/projectModification/add";
                var options = {
                    title: 'Add Mod :' + data.dataItem.projectNumber + ' ' + data.dataItem.title,
                    height: '85%',
                    events: [
                        {
                            text: "Save",
                            primary: true,
                            action: function (e: any, values: any) {
                                grid.refresh(grid.senderData.menuController);

                                loadFileUpload(values.contractGuid, values.uploadPath, false, '', 'ContractModification', values.resourceId)
                            }
                        },
                        {
                            text: "Cancel",
                            action: function (e: any) {
                                $('html').removeClass('htmlClass');
                            }
                        }
                    ]
                };
                window.Dialog.openDialogSubmit(data, options);
            }
        }
    ];

    let userGridMenu = [
        { text: 'Export to PDF', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
        { text: 'Export to Excel', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() } }
    ];

    let userAddButton = {
        text: "Add Task Order",
        icon: "plus-sm",
        action: function (data: any, grid: any) {
            window.location = "/project/add?contractGuid=" + ContractGuid;
        }
    }

    var hideMenus = ['projectNumber'];
    var groupTotalField = ["amount", "fundingAmount"];
    return ReactDOM.render(<KendoGroupableGrid key="g6" printTitle={pdfTitle} printedBy={userFullname} dataUrl={"/Project/GetProjectsWithMods?contractGuid=" + ContractGuid} columnUrl={"/Project/GetProjectWithModFields?contractGuid=" + ContractGuid} exportFieldUrl={"/Project/GetProjectWithModReportFields?contractGuid=" + ContractGuid} showColumnMenu={false} identityField="id" groupField="projectNumber" rowMenus={userRowMenu} hideColumns={hideMenus} gridMenu={userGridMenu} showGridAction={true} showSearchBox={false} currencySymbol={currencySymbol} addRecord={userAddButton} groupTotalFields={groupTotalField} groupTotalLabelField="endDate" pagination={false} container="projectAndModList" />, document.getElementById("projectAndModList"));
}


function loadContractMods(ContractGuid: string, currencySymbol: string, pdfTitle: string, userFullname: string) {
    //User grid setting start
    let kendoGrid: any = {}


    let userRowMenu = [
        {
            text: "Details", resource: 'Contract', resourceAction: 'Details', icon: "info", conditions: [{ field: 'isMod', value: true }], action: (data: any, grid: any) => {
                data.url = "/ContractModification/Details/" + data.dataItem.id;
                var options = {
                    title: 'Details of ' + data.dataItem.title + ' - ' + data.dataItem.mod,
                    height: '85%',
                    events: [
                        {
                            text: "Edit",
                            primary: true,
                            requireValidation: false,
                            action: function () {
                                loadModificationEdit(data, grid, "ContractModification");
                            }
                        },
                        {
                            text: "Cancel",
                            action: function (e: any) {
                                $('html').removeClass('htmlClass');
                            }
                        }
                    ]
                };
                window.Dialog.openDialog(data, options);
            }
        },
        {
            text: "Edit", resource: 'Contract', resourceAction: 'Edit', icon: "pencil", conditions: [{ field: 'isMod', value: true }], action: (data: any, grid: any) => {
                data.url = "/ContractModification/Edit/" + data.dataItem.id;
                data.submitURL = "/ContractModification/Edit";
                let senderData = data;
                var options = {
                    title: 'Edit Mod : ' + data.dataItem.title + ' - ' + data.dataItem.mod,
                    height: '85%',
                    events: [
                        {
                            text: "Save",
                            primary: true,
                            action: function (e: any, values: any) {

                                loadFileUpload(values.contractGuid, values.uploadPath, false, '', 'ContractModification', values.resourceId)

                                //loadNotification("ContractModification.Edit", data.dataItem.id, false, true);
                                grid.refresh(senderData.menuController);
                                if (reloadRevenuerecognition) {
                                    reloadRevenuerecognition(values);
                                }
                            }
                        },
                        {
                            text: "Cancel",
                            action: function (e: any) {
                                $('html').removeClass('htmlClass');
                            }
                        }
                    ]
                };
                window.Dialog.openDialogSubmit(data, options);
            }
        },
        {
            text: "Delete", resource: 'Contract', resourceAction: 'Delete', icon: "delete", conditions: [{ field: 'isMod', value: true }], action: (data: any, grid: any) => {
                let ids: any[] = [];
                ids.push(data.dataItem.id)
                let senderData = data;
                window.Dialog.confirm({
                    text: "Are you sure you want to  delete Mod?",
                    title: "Confirm",
                    ok: function (e: any) {
                        Remote.postData('/ContractModification/Delete',
                            ids,
                            (data: any) => { grid.refresh(senderData.menuController); },
                            (err: any) => { window.Dialog.alert(err) });
                    },
                    cancel: function (e: any) {
                        $('html').removeClass('htmlClass');
                    }
                });
            }
        },
        {
            text: "Details", resource: 'Contract', resourceAction: 'Details', icon: "info", conditions: [{ field: 'isMod', value: false }], action: (data: any, grid: any) => {
                window.location = "/contract/Details/" + data.dataItem.id
            }
        },
        {
            text: "Edit", resource: 'Contract', resourceAction: 'Edit', icon: "pencil", conditions: [{ field: 'isMod', value: false }], action: (data: any, grid: any) => {
                window.location = "/contract/Edit/" + data.dataItem.id;
            }
        },
        {
            text: "Delete", resource: 'Contract', resourceAction: 'Delete', icon: "delete", conditions: [{ field: 'isMod', value: false }], action: (data: any, grid: any) => {
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
            text: "Disable", resource: 'Contract', resourceAction: 'Edit', icon: "cancel", conditions: [{ field: 'isMod', value: false }, { field: 'status', value: true }], action: (data: any, grid: any) => {
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
            text: "Enable", resource: 'Contract', resourceAction: 'Edit', icon: "check", conditions: [{ field: 'isMod', value: false }, { field: 'status', value: false }], action: (data: any, grid: any) => {
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
            text: "Add Mod", resource: 'Contract', resourceAction: 'Add', icon: "plus", conditions: [{ field: 'isMod', value: false }], action: (data: any, grid: any) => {
                data.url = "/contractModification/add?contractGuid=" + data.dataItem.id;
                data.submitURL = "/contractModification/add";
                var options = {
                    title: 'Add Mod',
                    events: [
                        {
                            text: "Save",
                            primary: true,
                            action: function (e: any, values: any) {
                                $("#loadingFileUpload").show();
                                loadFileUpload(values.contractGuid, values.uploadPath, false, '', 'ContractModification', values.resourceId)
                                $("#loadingFileUpload").hide();
                                grid.refresh();
                            }
                        },
                        {
                            text: "Cancel",
                            action: function (e: any) {
                                $('html').removeClass('htmlClass');
                            }
                        }
                    ]
                };
                window.Dialog.openDialogSubmit(data, options);
            }
        }
    ];

    let userGridMenu = [
        { text: 'Export to PDF', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
        { text: 'Export to Excel', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() } }
    ];

    let userAddButton = {
        text: "Add Mod",
        icon: "plus-sm",
        resource: 'Contract', resourceAction: 'Add',
        action: function (data: any, grid: any) {
            var dlgData: any = { url: '', submitURL: '', getMethod: '', method: '' };
            dlgData.url = "/contractModification/add?contractGuid=" + ContractGuid;
            dlgData.submitURL = "/contractModification/add";
            dlgData.getMethod = "get";
            dlgData.method = "post";
            var options = {
                title: 'Add Mod',
                height: '85%',
                events: [
                    {
                        text: "Save",
                        primary: true,
                        action: function (e: any, values: any) {
                            loadFileUpload(values.contractGuid, values.uploadPath, false, '', 'ContractModification', values.resourceId)

                            //loadNotification("ContractModification.Create", values.resourceId, false, true);
                            grid.refresh();
                            if (reloadRevenuerecognition) {
                                reloadRevenuerecognition(values);
                            }
                        }
                    },
                    {
                        text: "Cancel",
                        action: function (e: any) {
                            $('html').removeClass('htmlClass');
                        }
                    }
                ]
            };
            window.Dialog.openDialogSubmit(dlgData, options);
        }
    }
    var hideColumns = ['projectNumber'];
    var groupSumFields = ["amount", "fundingAmount"];
    return ReactDOM.render(<KendoGroupableGrid1 key="g2" printTitle={pdfTitle} printedBy={userFullname} dataUrl={"/Contract/GetContractWithMod?contractGuid=" + ContractGuid} columnUrl={"/Project/GetProjectWithModFields?contractGuid=" + ContractGuid} exportFieldUrl={"/Project/GetProjectWithModReportFields?contractGuid=" + ContractGuid} showColumnMenu={false} identityField="id" groupField="projectNumber" groupTotalFields={groupSumFields} groupTotalLabelField="endDate" rowMenus={userRowMenu} hideColumns={hideColumns} gridMenu={userGridMenu} showGridAction={true} showSearchBox={false} addRecord={userAddButton} showGroupHeader={false} currencySymbol={currencySymbol} container="contractModificationGrid" />, document.getElementById("contractModificationGrid"));
}
//loads contract Notice
function loadContractNotice(ContractGuid: string, projectNumber: string, pdfTitle: string, userFullname: string) {
    //User grid setting start
    let kendoGrid: any = {}
    let userRowMenu = [
        {
            text: "Details", resource: 'ContractNotice', resourceAction: 'Details', icon: "info", action: (data: any, grid: any) => {
                data.url = "/ContractNotice/Details/" + data.dataItem.contractNoticeGuid,
                    data.submitURL = "";
                let senderData = data;
                var options = {
                    title: projectNumber + " : " + data.dataItem.noticeType + ' Details',
                    height: '85%',
                    events: [

                        {
                            text: "Close",
                            action: function (e: any) {
                            }
                        }
                    ]
                };
                window.Dialog.openDialogSubmit(data, options);
            }
        },
        {
            text: "Edit", resource: 'ContractNotice', resourceAction: 'Edit', icon: "pencil", action: (data: any, grid: any) => {
                data.url = "/ContractNotice/Edit/" + data.dataItem.contractNoticeGuid;
                data.submitURL = "/ContractNotice/Edit";
                let senderData = data;
                var options = {
                    title: projectNumber + " : " + 'Edit ' + data.dataItem.noticeType,
                    height: '85%',
                    events: [
                        {
                            text: "Save",
                            primary: true,
                            action: function (e: any, values: any) {

                                loadFileUpload(values.resourceId, values.uploadPath, true, values.parentid, values.contractResourceFileKey, values.contentResourceGuid, () => {
                                    grid.refresh();
                                })

                            }
                        },
                        {
                            text: "Cancel",
                            action: function (e: any) {
                                $('html').removeClass('htmlClass');
                            }
                        }
                    ]
                };
                window.Dialog.openDialogSubmit(data, options);
            }
        },
    ];
    let userGridMenu = [
        { text: 'Export to PDF', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
        { text: 'Export to Excel', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() } }
    ];


    let userAddButton = {
        text: "Add Notice",
        icon: "plus-sm",
        resource: 'ContractNotice', resourceAction: 'Add',
        action: function (data: any, grid: any) {
            var dlgData: any = { url: '', submitURL: '', getMethod: '', method: '' };
            dlgData.url = "/contractNotice/add?contractGuid=" + ContractGuid;
            dlgData.submitURL = "/contractNotice/add";
            dlgData.getMethod = "get";
            dlgData.method = "post";
            var options = {
                title: 'Add Notice',
                height: '85%',
                events: [
                    {
                        text: "Save",
                        primary: true,
                        action: function (e: any, values: any) {
                            loadFileUpload(values.resourceId, values.uploadPath, false, values.parentid, values.contractResourceFileKey, values.contentResourceGuid, () => {
                                grid.refresh();
                            })
                        }
                    },
                    {
                        text: "Cancel",
                        action: function (e: any) {
                            $('html').removeClass('htmlClass');
                        }
                    }
                ]
            };
            window.Dialog.openDialogSubmit(dlgData, options);
        }
    }

    var hideColumns = ['NoticeDescription'];

    return ReactDOM.render(<KendoGrid printTitle={pdfTitle} printedBy={userFullname} key="10" dataURL={"/contractNotice/GetContractNotice?ContractGuid=" + ContractGuid + ""} showSearchBox={false} showAdvanceSearchBox={false} fieldUrl={"/GridFields/ContractNotice"} exportFieldUrl="/Export/ContractNotice" identityField="contractNoticeGuid" rowMenus={userRowMenu} showGridAction={true} addRecord={userAddButton} hideColumns={hideColumns} gridMenu={userGridMenu} parent="contractNoticeGrid" gridWidth={document.getElementById("contractNoticeGrid").clientWidth} />, document.getElementById("contractNoticeGrid"));
}

function loadPolicy() {
    let kendoGrid: any = {}
    let userRowMenu = [
        {
            text: "Details", icon: "folder-open", action: (data: any, grid: any) => {
                window.location = "/IAM/Policy/Details/" + data.dataItem.policyGuid;
            }
        },
        {
            text: "Edit", icon: "pencil", action: (data: any, grid: any) => {
                window.location = "/IAM/Policy/Edit/" + data.dataItem.policyGuid;
            }
        },
        {
            text: "Delete", icon: "delete", action: (data: any, grid: any) => {
                window.Dialog.confirm({
                    text: "Are you sure you want to  delete policy?",
                    title: "Confirm",
                    ok: async function (e: any) {
                        let result = await Remote.getAsync("/IAM/Policy/Delete/" + data.dataItem.policyGuid)
                        if (result.ok) {
                            let json = await result.json();
                            if (json.status === true) {
                                grid.refresh();
                            }
                            else {
                                window.Dialog.alert(json.message, "Error");
                            }
                        }
                        else {

                        }
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
        {
            text: 'Delete All', icon: 'trash', action: (data: any, grid: KendoGrid) => {
                grid.getSelectedItems(async (data: any[]) => {
                    let result = await Remote.postDataAsync("/IAM/Policy/Delete", data);
                    if (result.ok) {
                        let json = await result.json();
                        if (json.status) {
                            grid.refresh();
                        }
                        else {
                            window.Dialog.alert(json.message, "Error");
                        }
                    }
                    else {
                        let data = await result.json();
                    }
                })
            }
        }
    ];

    let userAddButton = {
        text: "Add Policy",
        icon: "plus-sm",
        url: "/IAM/Policy/Add",
        redirect: true,
        action: function (data: any) {

        }
    }
    //User Grid Settings end
    //User Grid
    kendoGrid = ReactDOM.render(<KendoGrid dataURL="/IAM/Policy/Get" identityField="policyGuid" advancedSearchEntity={["Policy"]} fieldUrl="/GridFields/Policy" addRecord={userAddButton} exportFieldUrl="/Export/Policy" rowMenus={userRowMenu} gridMenu={userGridMenu} gridWidth={document.getElementById("policyGrid").clientWidth} parent="policyGrid" />, document.getElementById("policyGrid"));
}

function loadContractList(pdfTitle: string, userFullname: string) {
    let gridMessage = "No Contracts found based upon the selected criteria.  Please select an alternative filter criteria or view all available Contracts with the All Contracts filter.";
    let kendoGrid: any = {}

    let rowMenuContractProjectList = [
        {
            text: "Details", href: "`/contract/Details/${row.data.contractGuid}`", title: 'Detailed information about contract is available. View FAR Clauses, Add a request to set up a project in Cost point, Add WBS, Employee Billing Rates. Upload Files etc.', resource: 'Contract', resourceAction: 'Details', icon: "info", conditions: [{ field: 'isContract', value: true }], action: (data: any, grid: any) => {
                window.location = "/contract/Details/" + data.dataItem.contractGuid
            }
        },
        {
            text: "Edit", href: "`/contract/Edit/${row.data.contractGuid}`", title: 'Modify existing fields for contracts.', resource: 'Contract', resourceAction: 'Edit', icon: "track-changes", conditions: [{ field: 'isContract', value: true }], action: (data: any, grid: any) => {
                window.location = "/contract/Edit/" + data.dataItem.contractGuid
            }
        },
        {
            text: "Add Task Order", resource: 'Contract', resourceAction: 'Add', title: "Add a new taskorder", icon: "plus", conditions: [{ field: 'isContract', value: true }, { field: 'isIDIQContract', value: true }], action: (data: any, grid: any) => {
                window.location = "/project/add?contractGuid=" + data.dataItem.contractGuid
            }
        },
        {
            text: "Add Mod", title: 'Add a new modification that is awarded by customer, admin mods etc.', resource: 'Contract', resourceAction: 'Add', icon: "plus", conditions: [{ field: 'isContract', value: true }], action: (data: any, grid: any) => {
                data.url = "/contractModification/add?contractGuid=" + data.dataItem.contractGuid;
                data.submitURL = "/contractModification/add";

                var options = {
                    title: 'Add Mod',
                    height: '85%',
                    events: [
                        {
                            text: "Save",
                            primary: true,
                            action: function (e: any, values: any) {
                                loadFileUpload(values.contractGuid, values.uploadPath, false, '', 'ContractModification', values.resourceId)
                                $("#loadingFileUpload").show();
                                //loadNotification("ContractModification.Create", values.resourceId);
                                grid.refresh();
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
            text: "Favorite", title: 'Add this contract to you favorite, so you can easily find them later. Once added, view them by going to filter drop down above the list and selecting  "My Favorites".', icon: "star-outline", conditions: [{ field: 'isContract', value: true }, { field: 'isFavorite', value: false }], action: (data: any, grid: any) => {
                var ids: any[] = [];
                ids.push(data.dataItem.contractGuid);
                Remote.postData('/contract/SetAsFavouriteContract', ids, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
            }
        },
        {
            text: "Favorite", title: 'Remove this contract from "My Favorites".', icon: "star", conditions: [{ field: 'isContract', value: true }, { field: 'isFavorite', value: true }], action: (data: any, grid: any) => {
                var ids: any[] = [];
                ids.push(data.dataItem.contractGuid);
                Remote.postData('/contract/RemoveAsFavouriteContract', ids, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
            }
        },
        {
            text: "Details", href: "`/project/Details/${row.data.contractGuid}`", title: 'Detailed information about taskorder is available. View FAR Clauses, Add a request to set up a project in Cost point, Add WBS, Employee Billing Rates. Upload Files etc.', resource: 'Contract', resourceAction: 'Details', icon: "info", conditions: [{ field: 'isContract', value: false }], action: (data: any, grid: any) => {
                window.location = "/project/Details/" + data.dataItem.contractGuid
            }
        },
        {
            text: "Edit", href: "`/project/Edit/${row.data.contractGuid}`", title: 'Modify existing fields for task orders', icon: "track-changes", conditions: [{ field: 'isContract', value: false }], action: (data: any, grid: any) => {
                window.location = "/project/Edit/" + data.dataItem.contractGuid
            }
        },
        {
            text: "Add Mod", title: 'Add a new modification that is awarded by customer, admin mods etc.', resource: 'Contract', resourceAction: 'Add', icon: "plus", conditions: [{ field: 'isContract', value: false }], action: (data: any, grid: any) => {
                data.url = "/projectModification/add?projectGuid=" + data.dataItem.contractGuid;
                data.submitURL = "/projectModification/add";
                var options = {
                    title: 'Add Mod',
                    height: '85%',
                    events: [
                        {
                            text: "Save",
                            primary: true,
                            action: function (e: any, values: any) {

                                loadFileUpload(values.contractGuid, values.uploadPath, false, '', 'ContractModification', values.resourceId)
                                $("#loadingFileUpload").show();
                                //loadNotification("ProjectModification.Create", values.resourceId, false, true);
                                grid.refresh();
                            }
                        },
                        {
                            text: "Cancel",
                            action: function (e: any) {
                                $('html').removeClass('htmlClass');
                            }
                        }
                    ]
                };
                window.Dialog.openDialogSubmit(data, options);
            },
        },
        {
            text: "Favorite", title: 'Add this taskorder to you favorite, so you can easily find them later. Once added, view them by going to filter drop down above the list and selecting  "My Favorites"', icon: "star-outline", conditions: [{ field: 'isContract', value: false }, { field: 'isFavorite', value: false }], action: (data: any, grid: any) => {
                var ids: any[] = [];
                ids.push(data.dataItem.contractGuid);
                Remote.postData('/contract/SetAsFavouriteContract', ids, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
            }
        },
        {
            text: "Favorite", title: 'Remove this taskorder from "My Favorites"', icon: "star", conditions: [{ field: 'isContract', value: false }, { field: 'isFavorite', value: true }], action: (data: any, grid: any) => {
                var ids: any[] = [];
                ids.push(data.dataItem.contractGuid);
                Remote.postData('/contract/RemoveAsFavouriteContract', ids, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
            }
        },
        {
            text: "Notify", icon: "notification", title: "Select users to notify about this contract. They will receive an email.", conditions: [{ field: 'isContract', value: true }], action: (data: any, grid: any) => {
                loadNotification("Contract.Notify", data.dataItem.contractGuid, true, false);
            }
        },
        {
            text: "Notify", icon: "notification", title: "Select users to notify about this contract. They will receive an email.", conditions: [{ field: 'isContract', value: false }], action: (data: any, grid: any) => {
                loadNotification("Project.Notify", data.dataItem.contractGuid, true, false);
            }
        },
        //{
        //    text: "Disable", resource: 'Contract', resourceAction: 'Edit', icon: "cancel", conditions: [{ field: 'isContract', value: true }, { field: 'isActive', value: true }], action: (data: any, grid: any) => {
        //        var ids: any[] = [];
        //        ids.push(data.dataItem.contractGuid);
        //        window.Dialog.confirm({
        //            text: "Are you sure you want to  disable contract?",
        //            title: "Confirm",
        //            ok: function (e: any) {
        //                Remote.postData('/contract/Disable', ids, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
        //            },
        //            cancel: function (e: any) {
        //            }
        //        });
        //    }
        //},
        //{
        //    text: "Enable", resource: 'Contract', resourceAction: 'Edit', icon: "check", conditions: [{ field: 'isContract', value: true }, { field: 'isActive', value: false }], action: (data: any, grid: any) => {
        //        var ids: any[] = [];
        //        ids.push(data.dataItem.contractGuid);
        //        window.Dialog.confirm({
        //            text: "Are you sure you want to  enable contract?",
        //            title: "Confirm",
        //            ok: function (e: any) {
        //                Remote.postData('/contract/Enable', ids, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
        //            },
        //            cancel: function (e: any) {
        //            }
        //        });
        //    }
        //},
        //{
        //    text: "Disable", resource: 'Contract', resourceAction: 'Edit', icon: "cancel", conditions: [{ field: 'isContract', value: false }, { field: 'isActive', value: true }], action: (data: any, grid: any) => {
        //        var ids: any[] = [];
        //        ids.push(data.dataItem.contractGuid);
        //        window.Dialog.confirm({
        //            text: "Are you sure you want to  disable contract?",
        //            title: "Confirm",
        //            ok: function (e: any) {
        //                Remote.postData('/project/Disable', ids, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
        //            },
        //            cancel: function (e: any) {
        //            }
        //        });
        //    }
        //},
        //{
        //    text: "Enable", resource: 'Contract', resourceAction: 'Edit', icon: "check", conditions: [{ field: 'isContract', value: false }, { field: 'isActive', value: false }], action: (data: any, grid: any) => {
        //        var ids: any[] = [];
        //        ids.push(data.dataItem.contractGuid);
        //        window.Dialog.confirm({
        //            text: "Are you sure you want to  enable contract?",
        //            title: "Confirm",
        //            ok: function (e: any) {
        //                Remote.postData('/project/Enable', ids, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
        //            },
        //            cancel: function (e: any) {
        //            }
        //        });
        //    }
        //},
        {
            text: "Delete", title: 'Delete this contract.', resource: 'Contract', resourceAction: 'Delete', icon: "delete", conditions: [{ field: 'isContract', value: true }], action: (data: any, grid: any) => {
                let ids: any[] = [];
                ids.push(data.dataItem.contractGuid);
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
            text: "Delete", title: 'Delete this taskorder.', resource: 'Contract', resourceAction: 'Delete', icon: "delete", conditions: [{ field: 'isContract', value: false }], action: (data: any, grid: any) => {
                let ids: any[] = [];
                ids.push(data.dataItem.contractGuid);
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
            text: "Begin Closeout", title: 'Begin the project close out process.', href: "`/ContractCloseOut/Add/${row.data.contractGuid}`", conditions: [{ field: 'isIDIQContract', value: false }], resource: 'Contract', resourceAction: 'Edit', icon: "track-changes", action: (data: any, grid: any) => {
                window.location = "/ContractCloseOut/Add/" + data.dataItem.contractGuid
            }
        },
        {
            text: "DCAA Contract Brief", href: "`Contract/ContractBrief?contractGuid=${row.data.contractGuid}`", title: 'DCAA Contract Brief.', resource: 'Contract', resourceAction: 'Edit', icon: "track-changes", action: (data: any, grid: any) => {
                window.location = "/Contract/ContractBrief?contractGuid=" + data.dataItem.contractGuid
            }
        }
    ];

    let rowMenuContractList = [
        {
            text: "Details", href: "`/contract/Details/${row.data.contractGuid}`", title: 'Detailed information about contract is available. View FAR Clauses, Add a request to set up a project in Cost point, Add WBS, Employee Billing Rates. Upload Files etc.', resource: 'Contract', resourceAction: 'Details', icon: "info", action: (data: any, grid: any) => {
                window.location = "/contract/Details/" + data.dataItem.contractGuid
            }
        },
        {
            text: "Edit", href: "`/contract/Edit/${row.data.contractGuid}`", title: 'Detailed information about contract is available. View FAR Clauses, Add a request to set up a project in Cost point, Add WBS, Employee Billing Rates. Upload Files etc.', resource: 'Contract', resourceAction: 'Edit', icon: "track-changes", action: (data: any, grid: any) => {
                window.location = "/contract/Edit/" + data.dataItem.contractGuid
            }
        },
        {
            text: "Add Task Order", resource: 'Contract', title: "Add a new taskorder", resourceAction: 'Add', icon: "plus", conditions: [{ field: 'isIDIQContract', value: true }], action: (data: any, grid: any) => {
                window.location = "/project/add?contractGuid=" + data.dataItem.contractGuid;
            }
        },
        {
            text: "Add Mod", title: 'Add a new modification that is awarded by customer, admin mods etc.', resource: 'Contract', resourceAction: 'Add', icon: "plus", action: (data: any, grid: any) => {
                data.url = "/contractModification/add?contractGuid=" + data.dataItem.contractGuid;
                data.submitURL = "/contractModification/add";
                var options = {
                    title: 'Add Mod',
                    height: '85%',
                    events: [
                        {
                            text: "Save",
                            primary: true,
                            action: function (e: any, values: any) {

                                loadFileUpload(values.contractGuid, values.uploadPath, false, '', 'ContractModification', values.resourceId)
                                $("#loadingFileUpload").show();
                                //loadNotification("ContractModification.Create", values.resourceId, false, true);
                                grid.refresh();
                            }
                        },
                        {
                            text: "Cancel",
                            action: function (e: any) {
                                $('html').removeClass('htmlClass');
                            }
                        }
                    ]
                };
                window.Dialog.openDialogSubmit(data, options);
            },
        },
        {
            text: "Favorite", title: 'Add this contract to you favorite, so you can easily find them later. Once added, view them by going to filter drop down above the list and selecting  "My Favorites".', icon: "star-outline", conditions: [{ field: 'isFavorite', value: false }], action: (data: any, grid: any) => {
                var ids: any[] = [];
                ids.push(data.dataItem.contractGuid);
                Remote.postData('/contract/SetAsFavouriteContract', ids, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
            }
        },
        {
            text: "Favorite", title: 'Remove this contract from "My Favorites".', icon: "star", conditions: [{ field: 'isFavorite', value: true }], action: (data: any, grid: any) => {
                var ids: any[] = [];
                ids.push(data.dataItem.contractGuid);
                Remote.postData('/contract/RemoveAsFavouriteContract', ids, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
            }
        },

        {
            text: "Notify", icon: "notification", title: "Select users to notify about this contract. They will receive an email.", action: (data: any, grid: any) => {
                loadNotification("Contract.Notify", data.dataItem.contractGuid, true, false);
            }
        },
        //{
        //    text: "Disable", resource: 'Contract', resourceAction: 'Edit', icon: "cancel", conditions: [{ field: 'status', value: 'Active' }], action: (data: any, grid: any) => {
        //        var ids: any[] = [];
        //        ids.push(data.dataItem.contractGuid);
        //        window.Dialog.confirm({
        //            text: "Are you sure you want to  disable contract?",
        //            title: "Confirm",
        //            ok: function (e: any) {
        //                Remote.postData('/contract/Disable', ids, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
        //            },
        //            cancel: function (e: any) {
        //            }
        //        });
        //    }
        //},
        //{
        //    text: "Enable", resource: 'Contract', resourceAction: 'Edit', icon: "check", conditions: [{ field: 'status', value: 'InActive' }], action: (data: any, grid: any) => {
        //        var ids: any[] = [];
        //        ids.push(data.dataItem.contractGuid);
        //        window.Dialog.confirm({
        //            text: "Are you sure you want to  enable contract?",
        //            title: "Confirm",
        //            ok: function (e: any) {
        //                Remote.postData('/contract/Enable', ids, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
        //            },
        //            cancel: function (e: any) {
        //            }
        //        });
        //    }
        //},
        {
            text: "Delete", title: 'Delete this contract.', resource: 'Contract', resourceAction: 'Delete', icon: "delete", action: (data: any, grid: any) => {
                let ids: any[] = [];
                ids.push(data.dataItem.contractGuid);
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
            text: "Begin Closeout", href: "`/ContractCloseOut/Add/${row.data.contractGuid}`", title: 'Begin the project close out process.', conditions: [{ field: 'isIDIQContract', value: false }], resource: 'Contract', resourceAction: 'Edit', icon: "track-changes", action: (data: any, grid: any) => {
                window.location = "/ContractCloseOut/Add/" + data.dataItem.contractGuid
            }
        },
        {
            text: "DCAA Contract Brief", href: "`Contract/ContractBrief?contractGuid=${row.data.contractGuid}`", title: 'DCAA Contract Brief.', resource: 'Contract', resourceAction: 'Edit', icon: "track-changes", action: (data: any, grid: any) => {
                window.location = "/Contract/ContractBrief?contractGuid=" + data.dataItem.contractGuid
            }
        }
    ];

    let userGridMenu = [
        { text: 'Export to PDF', title: 'Click here to export data in PDF', resource: 'Contract', resourceAction: 'Export', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
        { text: 'Export to Excel', title: 'Click here to export data in Excel', resource: 'Contract', resourceAction: 'Export', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() } },
        {
            text: 'Delete', title: 'Click here to delete the selected data', resource: 'Contract', resourceAction: 'Delete', icon: 'delete', action: (data: any, grid: any) => {

                grid.getSelectedItems((items: any[]) => {
                    if (items.length == 0) {
                        window.Dialog.alert("Please select at least a row to delete");
                        return;
                    }
                    let idArr: any[] = [];
                    let i: number = 0;
                    for (i; i < items.length; i++) {
                        idArr.push(items[i].contractGuid);
                    }
                    window.Dialog.confirm({
                        text: "Are you sure you want to delete selected contracts?",
                        title: "Confirm",
                        ok: function (e: any) {
                            Remote.postData("/contract/delete", idArr, (data: any) => { grid.refresh(); }, (error: any) => { });

                        },
                        cancel: function (e: any) {
                        }
                    });
                });

            }
        },
        //{
        //    text: 'Disable', title: 'Click here to disable selected data', resource: 'Contract', resourceAction: 'Edit', icon: 'cancel', action: (data: any, grid: any) => {
        //        grid.getSelectedItems((items: any[]) => {
        //            if (items.length == 0) {
        //                window.Dialog.alert("Please select at least a row to disable");
        //                return;
        //            }
        //            let idArr: any[] = [];
        //            let i: number = 0;
        //            for (i; i < items.length; i++) {
        //                idArr.push(items[i].contractGuid);
        //            }
        //            window.Dialog.confirm({
        //                text: "Are you sure you want to disable selected contracts?",
        //                title: "Confirm",
        //                ok: function (e: any) {
        //                    Remote.postData("/contract/disable", idArr, (data: any) => { grid.refresh(); }, (error: any) => { });
        //                },
        //                cancel: function (e: any) {
        //                }
        //            });
        //        });
        //    }
        //},
        //{
        //    text: 'Enable', resource: 'Contract', resourceAction: 'Edit', icon: 'check', action: (data: any, grid: any) => {
        //        grid.getSelectedItems((items: any[]) => {
        //            if (items.length == 0) {
        //                window.Dialog.alert("Please select at least a row to enable");
        //                return;
        //            }
        //            let idArr: any[] = [];
        //            let i: number = 0;
        //            for (i; i < items.length; i++) {
        //                idArr.push(items[i].contractGuid);
        //            }
        //            window.Dialog.confirm({
        //                text: "Are you sure you want to enable selected contracts?",
        //                title: "Confirm",
        //                ok: function (e: any) {
        //                    Remote.postData("/contract/enable", idArr, (data: any) => { grid.refresh(); }, (error: any) => { });
        //                },
        //                cancel: function (e: any) {
        //                }
        //            });
        //        });

        //    }
        //}
    ];

    let userAddButton = {
        text: "Add Contract",
        resource: 'Contract', resourceAction: 'Add',
        icon: "plus-sm",
        action: (data: any, grid: any) => {
            window.location = "/contract/add";
        }
    }

    var additionalFilters = [
        { name: 'My Contracts', value: 'MyContract', default: true, sortField: 'updatedOn', sortOrder: 'desc' },
        { name: 'All Contracts', value: 'all', default: false, sortField: 'updatedOn', sortOrder: 'desc' },
        { name: 'My Favorite', value: 'MyFavorite', default: false, sortField: 'updatedOn', sortOrder: 'desc' },
        { name: 'Recently Viewed', value: 'RecentlyViewed', default: false, sortField: 'updatedOn', sortOrder: 'desc' }
    ];

    let rowIconSettings: RowIconSettings = new RowIconSettings();
    rowIconSettings.column = "contractNumber";
    rowIconSettings.icons.push(new IconSetting("isIDIQContract", true, "", "idiq-icon"));
    rowIconSettings.icons.push(new IconSetting("isContract", true, "", "contract-icon"));
    rowIconSettings.icons.push(new IconSetting("isContract", false, "", "project-icon"));
    rowIconSettings.icons.push(new IconSetting("isFavorite", true, "", "favorite-icon"));

    let storedParam: any = undefined;
    let param = sessionStorage.getItem(pdfTitle);
    if (param) {
        storedParam = JSON.parse(param);
    }

    var hideColumns: any[] = [""];
    var currentGrid: any = null;
    var switchButton = {
        onLabel: "Hide Task Orders",
        offLabel: "Show Task Orders",
        checked: false,
        action: (status: any) => {
            if (status === true) {
                openContractWithTaskOrder();
            }
            else {
                openContractOnly();
            }
        }
    }

    let openContractWithTaskOrder = () => {
        let navUrl = [{
            field: 'isContract', value: true, url: "/Contract/Details/"
        }, { field: 'isContract', value: false, url: "/Project/Details/" }];
        let grid: KendoGrid = currentGrid;
        let dataState = grid && grid.state && grid.state.dataState ? grid.state.dataState : undefined;
        let groupTotalField = ["awardAmount", "fundingAmount"];
        let searchParams: any = currentGrid && currentGrid.getSearchParameters() ? currentGrid.getSearchParameters() : undefined;
        if (dataState === undefined && storedParam) {
            dataState = storedParam.dataState || storedParam.gridState
            searchParams = storedParam.advancedSearchConditions

            if (storedParam.additionalFilterValue) {
                let additionalItem = additionalFilters.filter((v: any, i: number) => {
                    if (v.value === storedParam.additionalFilterValue.value) {
                        return v;
                    }
                })
                if (additionalItem.length > 0) {
                    additionalFilters.map((v: any, i: any) => { v.default = false; })
                    additionalItem[0].default = true;
                }

            }
        }
        if (currentGrid) {
            ReactDOM.unmountComponentAtNode(document.getElementById("ContractGrid1"));
        }

        switchButton.checked = true;
        currentGrid = ReactDOM.render(<KendoGroupableGrid printTitle={pdfTitle} printedBy={userFullname} gridMessage={gridMessage} description="<div class='icon-defination'><img src='/img/contract.png' alt=''> <b>Contract</b><img src='/img/idiq.png' alt=''> <b>IDIQ Contract </b><img src='/img/project.png' alt=''> <b>Task Order</b> </div><div class='ml-3' style='font-weight: 500;font-size: 14px;color: #6c757d;'>The Award Amount and Funded Amount shown are cumulative.</div>" searchPlaceHolder="Contract Number or Project Number or Title" itemNavigationUrl={navUrl} navigationFields={['projectNumber']} sortField="updatedOn" key="10" externalDataState={dataState} advancedSearchEntity={["Contract"]} dataUrl={"/contract/GetContracts?switchOn=1"} searchParameters={searchParams} rowIconSettings={rowIconSettings} columnUrl={"/GridFields/Contract"} additionalFilters={additionalFilters} exportFieldUrl="/Export/Contract" identityField="contractGuid" showAdvancedSearchDialog={true} showColumnMenu={true} groupField="contractNumber" rowMenus={rowMenuContractProjectList} hideColumns={[]} gridMenu={userGridMenu} switchButton={switchButton} showGridAction={true} showSearchBox={true} addRecord={userAddButton} showGroupHeader={true} groupTotalFields={groupTotalField} groupTotalLabelField="popEnd" container="ContractGrid1" />, document.getElementById("ContractGrid1"));
        if (currentGrid)
            currentGrid.refresh();
    }

    let openContractOnly = () => {
        let grid: KendoGroupableGrid = currentGrid;
        let dataState = grid && grid.state && grid.state.gridState ? grid.state.gridState : undefined;
        let searchParams: any = currentGrid && currentGrid.getSearchParameters() ? currentGrid.getSearchParameters() : undefined;
        if (storedParam && (storedParam.gridState || storedParam.dataState)) {
            dataState = storedParam.gridState || storedParam.dataState
            searchParams = storedParam.advancedSearchConditions
            if (storedParam.additionalFilterValue) {
                let additionalItem = additionalFilters.filter((v: any, i: number) => {
                    if (v.value === storedParam.additionalFilterValue.value) {
                        return v;
                    }
                })
                if (additionalItem.length > 0) {
                    additionalFilters.map((v: any, i: any) => { v.default = false; })
                    additionalItem[0].default = true;
                }

            }
        }
        if (currentGrid) {
            ReactDOM.unmountComponentAtNode(document.getElementById("ContractGrid1"));
        }
        switchButton.checked = false;
        currentGrid = ReactDOM.render(<KendoGrid printTitle={pdfTitle} printedBy={userFullname} gridMessage={gridMessage} description="<div class='icon-defination'><img src='/img/contract.png' alt=''> <b>Contract</b><img src='/img/idiq.png' alt=''> <b>IDIQ Contract </b> </div><div class='ml-3' style='font-weight: 500;font-size: 14px;color: #6c757d;'>The Award Amount and Funded Amount shown are cumulative.</div>" searchPlaceHolder="Contract Number or Project Number or Title" itemNavigationUrl="/Contract/Details/" navigationFields={['projectNumber']} sortField="updatedOn" key="10" externalDataState={dataState} dataURL={"/contract/GetContracts"} advancedSearchEntity={["Contract"]} searchParameters={searchParams} fieldUrl={"/GridFields/Contract"} rowIconSettings={rowIconSettings} exportFieldUrl="/Export/Contract" additionalFilters={additionalFilters} identityField="contractGuid" rowMenus={rowMenuContractList} hideColumns={hideColumns} gridMenu={userGridMenu} showGridAction={true} showSearchBox={true} addRecord={userAddButton} parent="ContractGrid1" switchButton={switchButton} gridWidth={document.getElementById("ContractGrid1").clientWidth} />, document.getElementById("ContractGrid1"));
        if (currentGrid)
            currentGrid.refresh();
    }

    if (storedParam && storedParam.isSwitchOn) {
        openContractWithTaskOrder();
    }
    else if (storedParam) {
        openContractOnly();
    }
    else {
        currentGrid = ReactDOM.render(<KendoGrid printTitle={pdfTitle} printedBy={userFullname} gridMessage={gridMessage} description="<div class='icon-defination'><img src='/img/contract.png' alt=''> <b>Contract</b><img src='/img/idiq.png' alt=''> <b>IDIQ Contract </b> </div><div class='ml-3' style='font-weight: 500;font-size: 14px;color: #6c757d;'>The Award Amount and Funded Amount shown are cumulative.</div>" searchPlaceHolder="Contract Number or Project Number or Title" itemNavigationUrl="/Contract/Details/" navigationFields={['projectNumber']} sortField="updatedOn" sortOrder="desc" key="10" dataURL={"/contract/GetContracts"} advancedSearchEntity={["Contract"]} fieldUrl={"/GridFields/Contract"} exportFieldUrl="/Export/Contract" rowIconSettings={rowIconSettings} additionalFilters={additionalFilters} identityField="contractGuid" rowMenus={rowMenuContractList} hideColumns={hideColumns} gridMenu={userGridMenu} showGridAction={true} showSearchBox={true} addRecord={userAddButton} parent="ContractGrid1" switchButton={switchButton} gridWidth={document.getElementById("ContractGrid1").clientWidth} />, document.getElementById("ContractGrid1"));
        return currentGrid;
    }

}

function loadAuditLog(pdfTitle: string, userFullname: string) {
    let kendoGrid: any = {}

    let auditRowMenu = [
        {
            text: "Details", icon: "info", action: (data: any, grid: any) => {
                window.location = data.dataItem.additionalInformationURl
            }
        },
    ];

    let userGridMenu = [
        { text: 'Export to PDF', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
        { text: 'Export to Excel', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() } }
    ];

    //User Grid Settings end
    //User Grid
    kendoGrid = ReactDOM.render(<div className="auditLog_grid"><KendoGrid printTitle={pdfTitle} printedBy={userFullname} gridHeight="750px" dataURL="/AuditLog/Get" showSearchBox={true} showColumnMenu={false} showAdvanceSearchBox={true} advancedSearchEntity={["AuditLog"]} identityField="auditLogGuid" fieldUrl="/GridFields/AuditLog" exportFieldUrl="/Export/AuditLog" gridMenu={userGridMenu} gridWidth={document.getElementById("auditLogGrid").clientWidth} parent="auditLogGrid" /></div>, document.getElementById("auditLogGrid"));
}

function loadEventLog(pdfTitle: string, userFullname: string) {
    let kendoGrid: any = {}

    let eventRowMenu = [
        {
            text: "Details", icon: "info", action: (data: any, grid: any) => {
                data.url = "/EventLog/Details?eventGuid=" + data.dataItem.eventGuid;
                var options = {
                    title: 'Event Log Details',
                    height: '85%',
                    events: [
                        {
                            text: "Cancel",
                            action: function (e: any) {
                                $('html').removeClass('htmlClass');
                            }
                        }
                    ]
                };
                window.Dialog.openDialog(data, options);
            },
        },
    ];

    let userGridMenu = [
        { text: 'Export to PDF', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
        { text: 'Export to Excel', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() } }
    ];

    //User Grid Settings end
    //User Grid
    kendoGrid = ReactDOM.render(<div className="auditLog_grid"><KendoGrid printTitle={pdfTitle} printedBy={userFullname} gridHeight="750px" dataURL="/EventLog/Get" showSearchBox={true} showColumnMenu={false} showAdvanceSearchBox={false} advancedSearchEntity={["EventLog"]} identityField="EventGuid" fieldUrl="/GridFields/EventLog" exportFieldUrl="/Export/EventLog" gridMenu={userGridMenu} gridWidth={document.getElementById("eventLogGrid").clientWidth} rowMenus={eventRowMenu} parent="eventLogGrid" /></div>, document.getElementById("eventLogGrid"));
}

function loadFarContractType(pdfTitle: string, userFullname: string) {
    let kendoGrid: any = {}
    let simpleViewMenu = [
        {
            text: "Details", icon: "info", action: (data: any, grid: any) => {
                data.url = "/Admin/FarContractType/Detail?id=" + data.dataItem.farContractTypeGuid;
                var options = {
                    title: 'Details',
                    height: '85%',
                    events: [
                        {
                            text: "Cancel",
                            action: function (e: any) {
                            }
                        }
                    ]
                };
                window.Dialog.openDialog(data, options);
            },
        },
        {
            text: "Edit", icon: "track-changes", action: (data: any, grid: any) => {
                data.url = "/Admin/FarContractType/Edit?id=" + data.dataItem.farContractTypeGuid;
                data.submitURL = "/Admin/FarContractType/Edit/";
                var options = {
                    title: 'Edit - ' + data.dataItem.title,
                    height: '85%',
                    events: [
                        {
                            text: "Save",
                            primary: true,
                            action: function (e: any, values: any) {
                                grid.refresh();
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
            },
        },
        {
            text: "Delete", icon: "delete", action: (data: any, grid: any) => {
                let ids: any[] = [];
                ids.push(data.dataItem.farContractTypeGuid)
                window.Dialog.confirm({
                    text: "Are you sure you want to  delete (" + data.dataItem.title + ") ?",
                    title: "Confirm",
                    ok: function (e: any) {
                        Remote.postData('/Admin/FarContractType/Delete', ids, (data: any) => { grid.refresh(); }, (err: any) => { window.Dialog.alert(err) })
                    },
                    cancel: function (e: any) {
                    }
                });
            }
        }
    ];
    let addButton = {
        text: "Add Contract Type",
        icon: "plus-sm",
        action: (data: any, grid: any) => {
            data.url = "/Admin/FarContractType/Add";
            data.submitURL = "/Admin/FarContractType/Add/";
            var options = {
                title: 'Add Contract Type',
                height: '85%',
                events: [
                    {
                        text: "Save",
                        primary: true,
                        action: function (e: any, values: any) {
                            grid.refresh();
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
    }
    let userGridMenu = [
        { text: 'Export to PDF', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
        {
            text: 'Export to Excel', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() }
        },
        {
            text: 'Delete', icon: 'delete', action: (data: any, grid: any) => {

                grid.getSelectedItems((items: any[]) => {
                    if (items.length == 0) {
                        window.Dialog.alert("Please select at least a row to delete");
                        return;
                    }
                    let idArr: any[] = [];
                    let i: number = 0;
                    for (i; i < items.length; i++) {
                        idArr.push(items[i].farContractTypeGuid);
                    }
                    window.Dialog.confirm({
                        text: "Are you sure you want to delete the selected rows ?",
                        title: "Confirm",
                        ok: function (e: any) {
                            Remote.postData("/Admin/FarContractType/Delete", idArr, (data: any) => { grid.refresh(); }, (error: any) => { });

                        },
                        cancel: function (e: any) {
                        }
                    });
                });

            }
        },
    ];

    kendoGrid = ReactDOM.render(<div className="farContractType_grid"><KendoGrid printTitle={pdfTitle} printedBy={userFullname} gridHeight="750px" dataURL="/Admin/FarContractType/Get" showSearchBox={true} showColumnMenu={false} showAdvanceSearchBox={false} identityField="Code" fieldUrl="/GridFields/FarContractType" exportFieldUrl="/Export/FarContractType" rowMenus={simpleViewMenu} addRecord={addButton} gridMenu={userGridMenu} gridWidth={document.getElementById("farContractTypeGrid").clientWidth} parent="farContractTypeGrid" /></div>, document.getElementById("farContractTypeGrid"));
}

function loadFarClause(pdfTitle: string, userFullname: string) {
    let kendoGrid: any = {}

    let farClauseAddButton = {
        text: "Add Far Clause", icon: "plus-sm",
        action: (data: any, grid: any) => {
            data.url = "/Admin/FarClause/Add/";
            data.submitURL = "/Admin/FarClause/Add/";
            let senderData = data;
            var options = {
                title: 'Add Far Clause',
                height: '80%',
                events: [
                    {
                        text: "Save",
                        primary: true,
                        action: function (e: any, values: any) {
                            grid.refresh();
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
    }

    let farClauseRowMenu = [
        {
            text: "Details", icon: "info", action: (data: any, grid: any) => {
                data.url = "/Admin/FarClause/Detail/" + data.dataItem.farClauseGuid
                var options = {
                    title: 'Details : ' + data.dataItem.number + ' - ' + data.dataItem.title,
                    height: '80%',
                    events: [
                        {
                            text: "Cancel",
                            action: function (e: any) {
                            }
                        }
                    ]
                };
                window.Dialog.openDialog(data, options);
            }
        },
        {
            text: "Edit", icon: "pencil", action: (data: any, grid: any) => {
                data.url = "/Admin/FarClause/Edit/" + data.dataItem.farClauseGuid
                data.submitURL = "/Admin/FarClause/Edit/";
                let senderData = data;
                var options = {
                    title: 'Edit : ' + data.dataItem.number + ' - ' + data.dataItem.title,
                    height: '80%',
                    events: [
                        {
                            text: "Save",
                            primary: true,
                            action: function (e: any, values: any) {
                                grid.refresh();
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
            text: "Delete", icon: "delete", action: (data: any, grid: any) => {
                let ids: any[] = [];
                ids.push(data.dataItem.farClauseGuid)
                let senderData = data;
                window.Dialog.confirm({
                    text: "Are you sure you want to delete far clause?",
                    title: "Confirm",
                    ok: function (e: any) {
                        Remote.postData('/Admin/FarClause/Delete', ids, (data: any) => { grid.refresh(); }, (err: any) => { window.Dialog.alert(err) })
                    },
                    cancel: function (e: any) {
                    }
                });
            }
        }
    ];

    let farClauseGridMenu = [
        { text: 'Export to PDF', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
        { text: 'Export to Excel', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() } },
        {
            text: 'Delete', icon: 'delete', action: (data: any, grid: any) => {

                grid.getSelectedItems((items: any[]) => {
                    if (items.length == 0) {
                        window.Dialog.alert("Please select at least a row to delete")
                        return;
                    } else {
                        window.Dialog.confirm({
                            text: "Are you sure you want to  far clause(s) ?",
                            title: "Confirm",
                            ok: function (e: any) {
                                let ids: any[] = [];
                                let i: number = 0;
                                for (i; i < items.length; i++) {
                                    ids.push(items[i].farClauseGuid);
                                }
                                Remote.postData('/Admin/FarClause/Delete', ids, (e: any) => { grid.refresh() }, (err: any) => { window.Dialog.alert(err) });
                            },
                            cancel: function (e: any) {
                            }
                        });
                    }
                    grid.reloadData();
                });
            }
        }
    ];

    kendoGrid = ReactDOM.render(<div className="farclause_grid"><KendoGrid printTitle={pdfTitle} printedBy={userFullname} gridHeight="750px" dataURL="/Admin/FarClause/Get" showSearchBox={true} showAdvanceSearchBox={false} showColumnMenu={false} rowMenus={farClauseRowMenu} identityField="farClauseGuid" fieldUrl="/GridFields/FarClause" exportFieldUrl="/Export/FarClause" addRecord={farClauseAddButton} gridMenu={farClauseGridMenu} gridWidth={document.getElementById("farClauseGrid").clientWidth} parent="farClauseGrid" /></div>, document.getElementById("farClauseGrid"));
}

function loadModificationEdit(data: any, grid: any, controllerName: string) {
    data.url = "/" + controllerName + "/Edit/" + data.dataItem.id;
    data.submitURL = "/" + controllerName + "/Edit";
    let senderData = data;
    var options = {
        title: 'Edit Mod : ' + data.dataItem.title + ' - ' + data.dataItem.mod,
        height: '85%',
        events: [
            {
                text: "Save",
                primary: true,
                action: function (e: any, values: any) {

                    loadFileUpload(values.contractGuid, values.uploadPath, false, '', 'ContractModification', values.resourceId)

                    grid.refresh(senderData.menuController);
                    if (reloadRevenuerecognition) {
                        reloadRevenuerecognition(values);
                    }
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
//loads contract NoticeDetails
function loadContractNoticeDetails(contractNoticeGuid: string, contractNoticeType: string, pdfTitle: string, userFullname: string) {
    var hideColumns = ['NoticeType', 'NoticeDescription', 'IssuedDate', 'Notice Type'];
    return ReactDOM.render(<KendoGrid printTitle={pdfTitle} printedBy={userFullname} key="110" dataURL={"/contractNotice/GetDetailsByNoticeType?Noticetype=" + contractNoticeType + "&ResourceId=" + contractNoticeGuid + ""} showSearchBox={false} showColumnMenu={false} hideColumns={hideColumns} showAdvanceSearchBox={false} fieldUrl={"/GridFields/ContractNoticeDetails"} identityField="ContractNoticeGuid" showGridAction={false} gridWidth={document.getElementById("ContractNoticeDetail").clientWidth} parent="ContractNoticeDetail" />, document.getElementById("ContractNoticeDetail"));
}

function loadArticle(pdfTitle: string, userFullname: string) {
    let kendoGrid: any = {}
    var showDraftedGrid: boolean = false;
    var currentGrid: any = null;

    let articleAddButton = {

        text: "Add Article", icon: "plus-sm",
        action: (data: any, grid: any) => {
            window.location = "/Admin/HomeSection/AddArticle/";
        }
    }

    let articleRowMenu = [
        {
            text: "Edit", icon: "pencil", action: (data: any, grid: any) => {
                window.location = "/Admin/HomeSection/EditArticle/" + data.dataItem.id;
            }

        },
        {
            text: "Delete", icon: "delete", action: (data: any, grid: any) => {
                window.Dialog.confirm({
                    text: "Are you sure you want to delete this Article?",
                    title: "Confirm",
                    ok: function (e: any) {
                        window.location = "/Admin/HomeSection/DeleteArticle/" + data.dataItem.id;
                    },
                     cancel: function (e: any) {
                    }
                })
            }
        }
    ];

    let articleGridMenu = [
        { text: 'Export to PDF', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
        { text: 'Export to Excel', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() } },
        {
            text: 'Delete', icon: 'delete', action: (data: any, grid: any) => {

                grid.getSelectedItems((items: any[]) => {
                    if (items.length == 0) {
                        window.Dialog.alert("Please select at least a row to delete")
                        return;
                    } else {
                        window.Dialog.confirm({
                            text: "Are you sure you want to delete the selected Article(s) ?",
                            title: "Confirm",
                            ok: function (e: any) {
                                let ids: any[] = [];
                                let i: number = 0;
                                for (i; i < items.length; i++) {
                                    ids.push(items[i].id);
                                }
                                Remote.postData('/Admin/HomeSection/DeleteMultiple', ids, (e: any) => { grid.refresh() }, (err: any) => { window.Dialog.alert(err) });
                            },
                            cancel: function (e: any) {
                            }
                        });
                    }
                    grid.reloadData();
                });
            }
        }
    ];

    let openDraftedArticles = () => {
        switchButton.checked = true;
        currentGrid = ReactDOM.render(<KendoGrid printTitle={pdfTitle} printedBy={userFullname} searchPlaceHolder="Search By Title" sortField="updatedOn" sortOrder="desc" key="10" dataURL={"/Admin/HomeSection/GetArticle?showDraft=1"} fieldUrl={"/GridFields/Article"} exportFieldUrl="/Export/Article" additionalFilters={additionalFilters} identityField="articleId" rowMenus={articleRowMenu} gridMenu={articleGridMenu} showGridAction={true} showSearchBox={true} addRecord={articleAddButton} parent="articleGrid" switchButton={switchButton} gridWidth={document.getElementById("articleGrid").clientWidth} />, document.getElementById("articleGrid"));
        if (currentGrid)
            currentGrid.refresh();
    }

    let openPublishedArticles = () => {
        switchButton.checked = false;
        currentGrid = ReactDOM.render(<KendoGrid printTitle={pdfTitle} printedBy={userFullname} searchPlaceHolder="Search By Title" sortField="updatedOn" sortOrder="desc" key="10" dataURL={"/Admin/HomeSection/GetArticle"} fieldUrl={"/GridFields/Article"} exportFieldUrl="/Export/Article" additionalFilters={additionalFilters} identityField="articleId" rowMenus={articleRowMenu}
            gridMenu={articleGridMenu} showGridAction={true} showSearchBox={true} addRecord={articleAddButton} parent="articleGrid" switchButton={switchButton} gridWidth={document.getElementById("articleGrid").clientWidth} />, document.getElementById("articleGrid"));
        if (currentGrid)
            currentGrid.refresh();
    }

    var additionalFilters = [
        { name: 'All', value: 'All', default: true, sortField: 'title', sortOrder: 'desc' },
        { name: 'More To Know', value: 'More To Know', default: true, sortField: 'title', sortOrder: 'desc' },
        { name: 'Values Corner', value: 'Values Corner', default: false, sortField: 'title', sortOrder: 'desc' },
        { name: 'What You Need To Know', value: 'What You Need To Know', default: false, sortField: 'title', sortOrder: 'desc' },
    ];

    var switchButton = {
        onLabel: "Show Drafts",
        offLabel: "Show Drafts",
        checked: false,
        action: (status: any) => {
            if (status === true) {
                openDraftedArticles();
            }
            else {
                openPublishedArticles();
            }
        }
    }

    currentGrid = ReactDOM.render(<KendoGrid printTitle={pdfTitle} printedBy={userFullname} searchPlaceHolder="Search By Title" sortField="updatedOn" sortOrder="desc" key="10" dataURL={"/Admin/HomeSection/GetArticle"} fieldUrl={"/GridFields/Article"} exportFieldUrl="/Export/Article" additionalFilters={additionalFilters} identityField="articleId" rowMenus={articleRowMenu} gridMenu={articleGridMenu} showGridAction={true} showSearchBox={true} addRecord={articleAddButton} parent="articleGrid" switchButton={switchButton} gridWidth={document.getElementById("articleGrid").clientWidth} />, document.getElementById("articleGrid"));
    return currentGrid;
}

window.project = { modlist: { loadProjectAndMode: loadProjectAndMod, loadProjectsAndMods: loadProjectsAndMods, loadContractMods: loadContractMods } }

window.contract = { contractList: { loadContractList: loadContractList } }

if (window.iam && window.iam.policy) {
    window.iam.policy = { loadPolicy: loadPolicy }
}

else {
    window.iam = { ...window.iam, policy: { loadPolicy: loadPolicy } }
}

window.auditLog = { auditLogList: loadAuditLog }
window.eventLog = { eventLogList: loadEventLog }

window.farClause = { farClauseList: loadFarClause }

window.article = { articleList: loadArticle }

window.farContractType = { farContractTypeList: loadFarContractType }
window.contractNotice = { loadContractNotice: loadContractNotice, loadContractNoticeDetails: loadContractNoticeDetails }


export { window }