"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const KendoGrid_1 = require("../../../../Common/Grid/KendoGrid");
const KendoGroupableGrid_1 = require("../../../../Common/Grid/KendoGroupableGrid");
const kendo_react_dateinputs_1 = require("@progress/kendo-react-dateinputs");
const kendo_react_buttons_1 = require("@progress/kendo-react-buttons");
class POTab extends React.Component {
    constructor(props) {
        super(props);
        this.switchButton = {
            onLabel: "By Vendor",
            offLabel: "By PO ID",
            checked: false,
            action: (status) => {
                this.setState({ switchOn: status });
                this.searchParams = this.kendoGrid.getSearchParameters();
                this.externalDataState = this.kendoGrid.state.dataState;
            }
        };
        this.switchButton1 = {
            onLabel: "By Vendor",
            offLabel: "By PO ID",
            checked: true,
            action: (status) => {
                this.setState({ switchOn: status });
                this.searchParams = this.kendoGroupableGrid.getSearchParameters();
                this.externalDataState = this.kendoGroupableGrid.state.gridState;
            }
        };
        this.switchButtonIWO = {
            onLabel: "Exclude IWOs",
            offLabel: "Include IWOs",
            checked: false,
            action: (status) => {
                if (this.startDate && this.endDate) {
                    this.setState({ excludeIWOs: status, dataUrl: this.props.dataUrl + "&startDate=" + this.getCompleteDate(this.startDate) + "&endDate=" + this.getCompleteDate(this.endDate) + "&excludeIWOs=" + status }, this.kendoGrid.refresh);
                }
                else {
                    this.setState({ excludeIWOs: status, dataUrl: this.props.dataUrl + "&excludeIWOs=" + status }, this.kendoGrid.refresh);
                }
            }
        };
        this.switchButtonIWO1 = {
            onLabel: "Exclude IWOs",
            offLabel: "Include IWOs",
            checked: false,
            action: (status) => {
                if (this.startDate && this.endDate) {
                    this.setState({ excludeIWOs: status, dataUrl: this.props.dataUrl + "&startDate=" + this.getCompleteDate(this.startDate) + "&endDate=" + this.getCompleteDate(this.endDate) + "&excludeIWOs=" + status }, () => { this.kendoGroupableGrid.refresh(null); });
                }
                else {
                    this.setState({ excludeIWOs: status, dataUrl: this.props.dataUrl + "&excludeIWOs=" + status }, () => { this.kendoGroupableGrid.refresh(null); });
                }
            }
        };
        this.state = {
            series: [],
            switchOn: false,
            dataUrl: this.props.dataUrl + "&excludeIWOs=" + false,
            excludeIWOs: false
        };
        this.onGridLoaded = this.onGridLoaded.bind(this);
        this.onGroupableGridLoaded = this.onGroupableGridLoaded.bind(this);
        this.onSearch = this.onSearch.bind(this);
        this.onClearDate = this.onClearDate.bind(this);
        this.onStartDateChange = this.onStartDateChange.bind(this);
        this.onEndDateChange = this.onEndDateChange.bind(this);
        this.createRowMenu = this.createRowMenu.bind(this);
        this.switchButton.action = this.switchButton.action.bind(this);
        this.switchButton1.action = this.switchButton1.action.bind(this);
        this.switchButtonIWO.action = this.switchButtonIWO.action.bind(this);
        this.switchButtonIWO1.action = this.switchButtonIWO1.action.bind(this);
    }
    onGridLoaded(e) {
    }
    onGroupableGridLoaded(e) {
    }
    onSearch(e) {
        if (this.state.excludeIWOs) {
            this.setState({ dataUrl: this.props.dataUrl + "&startDate=" + this.getCompleteDate(this.startDate) + "&endDate=" + this.getCompleteDate(this.endDate) + "&excludeIWOs=" + this.state.excludeIWOs }, () => { this.kendoGroupableGrid.refresh(null); });
        }
        else {
            this.setState({ dataUrl: this.props.dataUrl + "&startDate=" + this.getCompleteDate(this.startDate) + "&endDate=" + this.getCompleteDate(this.endDate) + "&excludeIWOs=" + this.state.excludeIWOs }, this.kendoGrid.refresh);
        }
    }
    onClearDate(e) {
        this.startDate = null;
        this.endDate = null;
        this.enableSearch = false;
        if (this.state.switchOn) {
            this.setState({ dataUrl: this.props.dataUrl }, () => { this.kendoGroupableGrid.refresh(null); });
        }
        else {
            this.setState({ dataUrl: this.props.dataUrl }, this.kendoGrid.refresh);
        }
    }
    onStartDateChange(e) {
        this.startDate = e.value;
        if (this.startDate != null && this.endDate != null) {
            if (this.startDate >= this.endDate) {
                this.enableSearch = false;
            }
            else {
                this.enableSearch = true;
            }
        }
        else {
            this.enableSearch = false;
        }
        this.forceUpdate();
    }
    onEndDateChange(e) {
        this.endDate = e.value;
        if (this.startDate != null && this.endDate != null) {
            if (this.startDate >= this.endDate) {
                this.enableSearch = false;
            }
            else {
                this.enableSearch = true;
            }
        }
        else {
            this.enableSearch = false;
        }
        this.forceUpdate();
    }
    getCompleteDate(d) {
        return this.getPadString(d.getMonth() + 1) + "-" + this.getPadString(d.getDate()) + "-" + d.getFullYear();
    }
    getPadString(d) {
        if (d > 9)
            return d + "";
        else
            return "0" + d;
    }
    createRowMenu() {
        let sender = this;
        let groupRowMenu = [
            {
                text: "View PO Details", icon: "folder-open", action: (data, grid) => {
                }
            },
            {
                text: "View Transaction Details", icon: "folder-open", action: (data, grid) => {
                }
            }
        ];
        return groupRowMenu;
    }
    render() {
        let gridMenu = [
            { text: 'Export to PDF', icon: 'pdf', action: (data, grid) => { grid.exportToPDF(); } },
            { text: 'Export to Excel', icon: 'excel', action: (data, grid) => { grid.exportToExcel(); } },
        ];
        let rowMenu = this.createRowMenu();
        if (this.state.switchOn === false) {
            let switchButtons = [];
            this.switchButtonIWO.checked = this.state.excludeIWOs;
            switchButtons.push(this.switchButton);
            switchButtons.push(this.switchButtonIWO);
            return (React.createElement("div", { id: "costGrid", className: "cost-grid" },
                React.createElement("div", { className: "row", style: { position: 'absolute', zIndex: 1 } },
                    React.createElement("div", { style: { display: 'block', width: '587px', margin: '0 auto -50px 558px' } },
                        React.createElement("div", { style: { "whiteSpace": "nowrap", paddingTop: '7px', display: 'inline-flex' } }, "Start Date:"),
                        React.createElement("div", { style: { width: "150px", display: 'inline-flex', paddingLeft: '10px' } },
                            React.createElement(kendo_react_dateinputs_1.DatePicker, { className: "form-control", onChange: this.onStartDateChange, value: this.startDate })),
                        React.createElement("div", { style: { "whiteSpace": "nowrap", paddingTop: '7px', display: 'inline-flex', paddingLeft: '10px' } }, "End Date:"),
                        React.createElement("div", { style: { width: "150px", display: 'inline-flex', paddingLeft: '10px' } },
                            React.createElement(kendo_react_dateinputs_1.DatePicker, { className: "form-control", onChange: this.onEndDateChange, value: this.endDate })),
                        React.createElement("div", { style: { width: "75px", display: 'inline-flex', paddingLeft: '10px' } },
                            React.createElement(kendo_react_buttons_1.Button, { onClick: this.onSearch, disabled: !this.enableSearch }, "Show")),
                        React.createElement("div", { style: { width: "75px", display: 'inline-flex', paddingLeft: '10px' } },
                            React.createElement(kendo_react_buttons_1.Button, { onClick: this.onClearDate, disabled: this.startDate === null && this.endDate === null }, "Clear")))),
                React.createElement("div", { className: "row" },
                    React.createElement("div", { className: "col-12" },
                        React.createElement(KendoGrid_1.KendoGrid, { ref: (c) => { this.kendoGrid = c; }, externalDataState: this.externalDataState, key: "poGrid", rowMenus: rowMenu, showColumnMenu: false, showAddNewButton: false, showAdvanceSearchBox: true, advancedSearchEntity: ["PFS-PO"], gridMenu: gridMenu, searchParameters: this.searchParams, identityField: "poCommitmentId", fieldUrl: this.props.fieldUrl, exportFieldUrl: this.props.exportUrl, dataURL: this.state.dataUrl, switchButton: switchButtons, parent: "costGrid", onGridLoaded: this.onGridLoaded })))));
        }
        else {
            let switchButtons = [];
            this.switchButtonIWO1.checked = this.state.excludeIWOs;
            switchButtons.push(this.switchButton1);
            switchButtons.push(this.switchButtonIWO1);
            return (React.createElement("div", { id: "costGrid", className: "cost-grid" },
                React.createElement("div", { className: "row", style: { position: 'absolute', zIndex: 1 } },
                    React.createElement("div", { style: { display: 'block', width: '587px', margin: '0 auto -50px 558px' } },
                        React.createElement("div", { style: { "whiteSpace": "nowrap", paddingTop: '7px', display: 'inline-flex' } }, "Start Date:"),
                        React.createElement("div", { style: { width: "150px", display: 'inline-flex', paddingLeft: '10px' } },
                            React.createElement(kendo_react_dateinputs_1.DatePicker, { className: "form-control", onChange: this.onStartDateChange, value: this.startDate })),
                        React.createElement("div", { style: { "whiteSpace": "nowrap", paddingTop: '7px', display: 'inline-flex', paddingLeft: '10px' } }, "End Date:"),
                        React.createElement("div", { style: { width: "150px", display: 'inline-flex', paddingLeft: '10px' } },
                            React.createElement(kendo_react_dateinputs_1.DatePicker, { className: "form-control", onChange: this.onEndDateChange, value: this.endDate })),
                        React.createElement("div", { style: { width: "75px", display: 'inline-flex', paddingLeft: '10px' } },
                            React.createElement(kendo_react_buttons_1.Button, { onClick: this.onSearch, disabled: !this.enableSearch }, "Show")),
                        React.createElement("div", { style: { width: "75px", display: 'inline-flex', paddingLeft: '10px' } },
                            React.createElement(kendo_react_buttons_1.Button, { onClick: this.onClearDate, disabled: this.startDate === null && this.endDate === null }, "Clear")))),
                React.createElement("div", { className: "row" },
                    React.createElement("div", { className: "col-12" },
                        React.createElement(KendoGroupableGrid_1.KendoGroupableGrid, { ref: (c) => { this.kendoGroupableGrid = c; }, externalDataState: this.externalDataState, key: "poGrid", rowMenus: rowMenu, groupField: "vendorName", showColumnMenu: false, showAdvancedSearchDialog: true, advancedSearchEntity: ["PFS-PO"], searchParameters: this.searchParams, groupTotalFields: ["totalAmount", "voucheredAmount", "balance"], gridMenu: gridMenu, identityField: "poCommitmentId", columnUrl: this.props.fieldUrl, exportFieldUrl: this.props.exportUrl, dataUrl: this.state.dataUrl, showSearchBox: true, showGridAction: true, currencySymbol: "USD", switchButton: switchButtons, onGridLoaded: this.onGroupableGridLoaded, container: "costGrid" })))));
        }
    }
}
exports.POTab = POTab;
//# sourceMappingURL=POTab.js.map