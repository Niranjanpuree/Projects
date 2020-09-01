import 'react-app-polyfill/ie11';
import * as React from "react";
import * as ReactDOM from "react-dom";
import { KendoGrid } from "../../Common/Grid/KendoGrid";
import { Remote } from "../../Common/Remote/Remote";
import { generatePath } from "react-router";
import { Condition } from "../Entities/Condition";
import { ProjectDetails, IUrls, DetailUrls } from "./ProjectDetails/ProjectDetails"
import { WbsUrls } from "./ProjectDetails/Tabs/WBSTab"

declare var window: any;
declare var $: any;
declare var reloadRevenuerecognition: any;

//var siteBaseUrl: string = '/pfs'; 
var siteBaseUrl: string = 'http://localhost:59440'; 

export class IconSetting
{
    field: string;
    value: any;
    icon: string;
    css: string;
    constructor(field: string, value: any, icon: string, css: string)
    {
        this.field = field;
        this.value = value;
        this.icon = icon;
        this.css = css;
    }
}

export class RowIconSettings
{
    column: string;
    icons: IconSetting[] = [];
}

function showProjectFinancialSystem() {
    let rowMenu = [
        {
            text: "Details", icon: "folder-open", action: (data: any, grid: any) => {
                window.location = siteBaseUrl + "/project/details/" + data.dataItem.projectNumber;
            }
        },
        {
            text: "Favorite", icon: "star-outline", conditions: [{ field: 'isFavorite', value: true }], action: (data: any, grid: any) => {
                var ids: any[] = [];
                ids.push(data.dataItem.projectId);
                Remote.postData(siteBaseUrl + '/project/unfavorite', data.dataItem, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
            }
        },
        {
            text: "Favorite", icon: "star", conditions: [{ field: 'isFavorite', value: false }], action: (data: any, grid: any) => {
                var ids: any[] = [];
                ids.push(data.dataItem.projectId);
                Remote.postData(siteBaseUrl + '/project/favorite', data.dataItem, (e: any) => { grid.refresh(data.menuController) }, (err: any) => { window.Dialog.alert(err) });
            }
        }
    ];
    let rowIconSettings: RowIconSettings = new RowIconSettings();
    rowIconSettings.column = "projectNumber";
    rowIconSettings.icons.push(new IconSetting("isFavorite", true, "", "favorite-icon"));
    let gridMenu = [
        { text: 'Export to PDF', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
        { text: 'Export to Excel', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() } },
    ];

    var additionalFilters = [
        { name: 'My Contracts', value: 'MyContract', default: true },
        { name: 'My Favorite', value: 'MyFavorite', default: false }
    ];

    ReactDOM.render(<KendoGrid dataURL={siteBaseUrl + "/project/Get"} fieldUrl={siteBaseUrl + "/GridFields/PFS-Project"} exportFieldUrl={siteBaseUrl + "/Export/PFS-Project"} itemNavigationUrl={siteBaseUrl + "/project/Details/"} identityField="projectNumber" showAdvanceSearchBox={true} advancedSearchEntity={["PFS-Project"]} rowIconSettings={rowIconSettings} additionalFilters={additionalFilters} pageSize={20} rowMenus={rowMenu} gridMenu={gridMenu} gridWidth={document.getElementById("taskOrderList").clientWidth} parent="taskOrderList" />, document.getElementById("taskOrderList"));
}

function showProjectDetailsScreen(projectNumber: any, currency: string)
{
    let urls: DetailUrls = {
        Mod: { dataUrl: siteBaseUrl + '/projectmod/Get?projectNumber=' + projectNumber, fieldUrl: siteBaseUrl + '/GridFields/PFS-ProjectMod', exportUrl: siteBaseUrl + '/Export/PFS-ProjectMod' },
        Billing: { dataUrl: '', fieldUrl: '', exportUrl: '' },
        Cost: { dataUrl: siteBaseUrl + '/cost/Get?projectNumber=' + projectNumber, fieldUrl: siteBaseUrl + '/GridFields/PFS-Cost', exportUrl: siteBaseUrl + '/Export/PFS-Cost' },
        Labor: { dataUrl: siteBaseUrl + '/labor/Get?projectNumber=' + projectNumber, fieldUrl: siteBaseUrl + '/GridFields/PFS-Labor', exportUrl: siteBaseUrl + '/Export/PFS-Labor' },
        PO: { dataUrl: siteBaseUrl + '/po/Get?projectNumber=' + projectNumber, fieldUrl: siteBaseUrl + '/GridFields/PFS-PO', exportUrl: siteBaseUrl + '/Export/PFS-PO' },
        RAM: { dataUrl: '', fieldUrl: '', exportUrl: '' },
        Revenue: { dataUrl: '', fieldUrl: '', exportUrl: '' },
        VendorPayment: { dataUrl: siteBaseUrl + '/vendorPayment/Get?projectNumber=' + projectNumber, fieldUrl: siteBaseUrl + '/GridFields/PFS-VendorPayment', exportUrl: siteBaseUrl + '/Export/PFS-VendorPayment' },
        WBS: {
            gridUrls: { dataUrl: siteBaseUrl + '/wbs/get?projectNumber=' + projectNumber, fieldUrl: siteBaseUrl + '/GridFields/PFS-Wbs', exportUrl: siteBaseUrl + '/Export/PFS-Wbs' },
            ActionUrls: { addUrl: '', deleteUrl: '', editUrl: '' },
            WbsDictionaryUrls: { dataUrl: siteBaseUrl + '/wbsdictionary/get', exportUrl: siteBaseUrl + '/Export/PFS-WbsDictionary', fieldUrl: siteBaseUrl + '/GridFields/PFS-WbsDictionary' },
            WbsDictionaryActionUrl: { addUrl: siteBaseUrl + "/wbsdictionary/add", editUrl: siteBaseUrl + "/wbsdictionary/edit", deleteUrl: siteBaseUrl + "/wbsdictionary/delete/" }
        }
    };
    ReactDOM.render(<ProjectDetails currency={currency} selectedTabIndex={0} projectId={projectNumber} components={urls} />, document.getElementById("projectDetails"));
}

window.siteBaseUrl = siteBaseUrl;

window.pfs = { showProjectFinancialSystem: showProjectFinancialSystem, showProjectDetailsScreen: showProjectDetailsScreen }

export { window }