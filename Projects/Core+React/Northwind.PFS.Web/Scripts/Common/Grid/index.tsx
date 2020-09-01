import 'react-app-polyfill/ie11';
import * as React from "react";
import * as ReactDOM from "react-dom";
import { KendoGrid } from "./KendoGrid";
import { KendoGroupableGrid, RowIconSettings, IconSetting } from "./KendoGroupableGrid";
import { Remote } from "../Remote/Remote";
import { generatePath } from "react-router";
import { Condition } from "../../Component/Entities/Condition";

declare var window: any;
declare var $: any;
declare var reloadRevenuerecognition: any;


function showProjectFinancialSystem()
{
    //Group grid setting start
    let groupRowMenu = [
        {
            text: "Edit", icon: "pencil", action: (data: any, grid: any) =>
            {
                let editView = {
                    text: "Edit 'Group'",
                    getUrl: "/IAM/Group/Edit/" + data.dataItem.groupGuid,
                    postUrl: "/IAM/Group/Edit",
                    method: 'post',
                    buttons: [
                        { primary: true, requireValidation: true, text: "Save", action: (e: any, o: any) => { } },
                        { primary: false, requireValidation: true, text: "Cancel", action: (e: any, o: any) => { } }
                    ],
                    redirect: false,
                    updateView: true,
                    action: function (data: any)
                    {

                    }
                }
                grid.openSubmittableForm(editView)
            }
        },
        {
            text: "Delete", icon: "delete", action: (data: any, grid: any) =>
            {
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
                    action: function (data: any)
                    {

                    }
                }
                grid.openSubmittableForm(deleteView);
            }
        },
        {
            text: "View Details", icon: "folder-open", action: (data: any, grid: any) =>
            {
                let detailsView = {
                    text: "User Details",
                    getUrl: "/IAM/Group/Details/" + data.dataItem.groupGuid,
                    buttons: [
                        { text: "Ok", action: (e: any, o: any) => { } }
                    ],
                    redirect: false,
                    updateView: false,
                    action: function (data: any)
                    {

                    }
                }
                grid.openSubmittableForm(detailsView);
            }
        }
    ];

    let groupGridMenu = [
        { text: 'Export to PDF', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
        { text: 'Export to Excel', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() } },
        {
            text: 'Delete Groups', icon: 'delete', action: (data: any, grid: any) =>
            {
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
        text: "Add Group",
        icon: "plus-sm",
        getUrl: "/IAM/Group/Add",
        postUrl: "/IAM/Group/Add",
        method: 'post',
        redirect: false,
        buttons: [
            { primary: true, requireValidation: true, text: "Save", action: (e: any, o: any) => { } },
            { primary: false, requireValidation: true, text: "Save And New", action: (e: any, o: any) => { } },
            { primary: true, text: "Cancel", action: (e: any, o: any) => { } },
        ],
        updateView: true,
        action: function (data: any)
        {

        }
    }
    //Group Grid Settings end
    //Group Grid
    ReactDOM.render(<KendoGrid dataURL="/IAM/Group/GroupList" fieldUrl="/GridFields/Group" exportFieldUrl="/Export/Group" identityField="groupGuid" rowMenus={groupRowMenu} gridMenu={groupGridMenu} gridWidth={document.getElementById("groupGrid").clientWidth} parent="groupGrid" />, document.getElementById("groupGrid"));
}

window.pfs = { showProjectFinancialSystem: showProjectFinancialSystem }

export { window }