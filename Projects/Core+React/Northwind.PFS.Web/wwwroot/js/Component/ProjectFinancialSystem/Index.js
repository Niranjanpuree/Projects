"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
require("react-app-polyfill/ie11");
const React = require("react");
const ReactDOM = require("react-dom");
const KendoGrid_1 = require("../../Common/Grid/KendoGrid");
const Remote_1 = require("../../Common/Remote/Remote");
const ProjectDetails_1 = require("./ProjectDetails/ProjectDetails");
//var siteBaseUrl: string = '/pfs'; 
var siteBaseUrl = 'http://localhost:59440';
class IconSetting {
    constructor(field, value, icon, css) {
        this.field = field;
        this.value = value;
        this.icon = icon;
        this.css = css;
    }
}
exports.IconSetting = IconSetting;
class RowIconSettings {
    constructor() {
        this.icons = [];
    }
}
exports.RowIconSettings = RowIconSettings;
function showProjectFinancialSystem() {
    let rowMenu = [
        {
            text: "Details", icon: "folder-open", action: (data, grid) => {
                window.location = siteBaseUrl + "/project/details/" + data.dataItem.projectNumber;
            }
        },
        {
            text: "Favorite", icon: "star-outline", conditions: [{ field: 'isFavorite', value: true }], action: (data, grid) => {
                var ids = [];
                ids.push(data.dataItem.projectId);
                Remote_1.Remote.postData(siteBaseUrl + '/project/unfavorite', data.dataItem, (e) => { grid.refresh(data.menuController); }, (err) => { window.Dialog.alert(err); });
            }
        },
        {
            text: "Favorite", icon: "star", conditions: [{ field: 'isFavorite', value: false }], action: (data, grid) => {
                var ids = [];
                ids.push(data.dataItem.projectId);
                Remote_1.Remote.postData(siteBaseUrl + '/project/favorite', data.dataItem, (e) => { grid.refresh(data.menuController); }, (err) => { window.Dialog.alert(err); });
            }
        }
    ];
    let rowIconSettings = new RowIconSettings();
    rowIconSettings.column = "projectNumber";
    rowIconSettings.icons.push(new IconSetting("isFavorite", true, "", "favorite-icon"));
    let gridMenu = [
        { text: 'Export to PDF', icon: 'pdf', action: (data, grid) => { grid.exportToPDF(); } },
        { text: 'Export to Excel', icon: 'excel', action: (data, grid) => { grid.exportToExcel(); } },
    ];
    var additionalFilters = [
        { name: 'My Contracts', value: 'MyContract', default: true },
        { name: 'My Favorite', value: 'MyFavorite', default: false }
    ];
    ReactDOM.render(React.createElement(KendoGrid_1.KendoGrid, { dataURL: siteBaseUrl + "/project/Get", fieldUrl: siteBaseUrl + "/GridFields/PFS-Project", exportFieldUrl: siteBaseUrl + "/Export/PFS-Project", itemNavigationUrl: siteBaseUrl + "/project/Details/", identityField: "projectNumber", showAdvanceSearchBox: true, advancedSearchEntity: ["PFS-Project"], rowIconSettings: rowIconSettings, additionalFilters: additionalFilters, pageSize: 20, rowMenus: rowMenu, gridMenu: gridMenu, gridWidth: document.getElementById("taskOrderList").clientWidth, parent: "taskOrderList" }), document.getElementById("taskOrderList"));
}
function showProjectDetailsScreen(projectNumber, currency) {
    let urls = {
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
    ReactDOM.render(React.createElement(ProjectDetails_1.ProjectDetails, { currency: currency, selectedTabIndex: 0, projectId: projectNumber, components: urls }), document.getElementById("projectDetails"));
}
window.siteBaseUrl = siteBaseUrl;
window.pfs = { showProjectFinancialSystem: showProjectFinancialSystem, showProjectDetailsScreen: showProjectDetailsScreen };
//# sourceMappingURL=Index.js.map