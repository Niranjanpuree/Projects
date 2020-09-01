"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const Remote_1 = require("../../../../Common/Remote/Remote");
const PieChart_1 = require("./PieChart");
const KendoGrid_1 = require("../../../../Common/Grid/KendoGrid");
const kendo_react_dateinputs_1 = require("@progress/kendo-react-dateinputs");
const kendo_react_buttons_1 = require("@progress/kendo-react-buttons");
class LaborTab extends React.Component {
    constructor(props) {
        super(props);
        this.startDate = null;
        this.endDate = null;
        this.enableSearch = false;
        this.switchButton = {
            onLabel: "Exclude IWOs",
            offLabel: "Include IWOs",
            checked: false,
            action: (status) => {
                if (this.startDate && this.endDate) {
                    this.setState({ switchOn: status, dataUrl: this.props.dataUrl + "&startDate=" + this.getCompleteDate(this.startDate) + "&endDate=" + this.getCompleteDate(this.endDate) + "&excludeIWOs=" + status }, this.wbsGrid.refresh);
                }
                else {
                    this.setState({ switchOn: status, dataUrl: this.props.dataUrl + "&excludeIWOs=" + status }, this.wbsGrid.refresh);
                }
            }
        };
        this.onGridLoaded = this.onGridLoaded.bind(this);
        this.onPieChartChange = this.onPieChartChange.bind(this);
        this.onBarChartChange = this.onBarChartChange.bind(this);
        this.loadPieGraphData = this.loadPieGraphData.bind(this);
        this.loadBarGraphData = this.loadBarGraphData.bind(this);
        this.onStartDateChange = this.onStartDateChange.bind(this);
        this.onEndDateChange = this.onEndDateChange.bind(this);
        this.onSearch = this.onSearch.bind(this);
        this.onClearDate = this.onClearDate.bind(this);
        this.state = {
            series: [],
            fyAndPeriod: [],
            dataUrl: this.props.dataUrl + "&excludeIWOs=" + false,
            switchOn: false
        };
    }
    componentDidMount() {
        return __awaiter(this, void 0, void 0, function* () {
            let result = yield Remote_1.Remote.getAsync(window.siteBaseUrl + "/labor/GetFiscalYearAndPeriod");
            if (result.ok) {
                let data = yield result.json();
                if (data.length > 0) {
                    this.selectedFY = data[0];
                    if (data[0].options && data[0].options) {
                        this.selectedPeriod = data[0].options[0];
                    }
                }
                this.setState({ fyAndPeriod: data }, () => __awaiter(this, void 0, void 0, function* () {
                    if (data.length > 0) {
                        yield this.loadPieGraphData();
                        yield this.loadBarGraphData();
                    }
                }));
            }
        });
    }
    loadPieGraphData() {
        return __awaiter(this, void 0, void 0, function* () {
            let searchParam = this.wbsGrid.getSearchParameters();
            let url = this.wbsGrid.getDataUrl().replace("/Get", "/GetPieChart");
            let result = yield Remote_1.Remote.postDataAsync(url, searchParam.advancedSearchConditions);
            if (result.ok) {
                let data = yield result.json();
                this.pieChart.updateChart(data);
            }
        });
    }
    loadBarGraphData() {
        return __awaiter(this, void 0, void 0, function* () {
            let searchParam = this.wbsGrid.getSearchParameters();
            let url = this.wbsGrid.getDataUrl().replace("/Get", "/GetBarChart");
            let result1 = yield Remote_1.Remote.postDataAsync(url, searchParam.advancedSearchConditions);
            if (result1.ok) {
                let data = yield result1.json();
                this.barChart.updateChart(data);
            }
        });
    }
    onGridLoaded(data) {
        this.loadPieGraphData();
        this.loadBarGraphData();
    }
    onPieChartChange(option1, option2) {
        return __awaiter(this, void 0, void 0, function* () {
            //await this.loadPieGraphData(option1, option2);
        });
    }
    onBarChartChange(option1) {
        return __awaiter(this, void 0, void 0, function* () {
            //await this.loadBarGraphData(option1);
        });
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
    onSearch(e) {
        this.setState({ dataUrl: this.props.dataUrl + "&startDate=" + this.getCompleteDate(this.startDate) + "&endDate=" + this.getCompleteDate(this.endDate) + "&excludeIWOs=" + this.state.switchOn }, this.wbsGrid.refresh);
    }
    onClearDate(e) {
        this.startDate = null;
        this.endDate = null;
        this.enableSearch = false;
        this.setState({ dataUrl: this.props.dataUrl }, this.wbsGrid.refresh);
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
    render() {
        let gridMenu = [
            { text: 'Export to PDF', icon: 'pdf', action: (data, grid) => { grid.exportToPDF(); } },
            { text: 'Export to Excel', icon: 'excel', action: (data, grid) => { grid.exportToExcel(); } }
        ];
        return (React.createElement("div", { id: "laborGrid" },
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
                    React.createElement(KendoGrid_1.KendoGrid, { ref: (c) => { this.wbsGrid = c; }, key: "wbsGrid", showColumnMenu: false, showGridAction: true, showSearchBox: true, gridMenu: gridMenu, showAdvanceSearchBox: true, advancedSearchEntity: ["PFS-Labor"], identityField: "id", fieldUrl: this.props.fieldUrl, exportFieldUrl: this.props.exportUrl, dataURL: this.state.dataUrl, switchButton: this.switchButton, onGridLoaded: this.onGridLoaded, parent: "laborGrid" }))),
            React.createElement("div", { className: "row" },
                React.createElement("div", { className: "col-6" },
                    React.createElement(PieChart_1.PieChart, { key: "pie1", ref: (c) => { this.pieChart = c; }, field: "value", SeriesType: "pie", categoryField: "name", data: this.state.series, title: "Labor", options: this.state.fyAndPeriod, selectedOption1: this.selectedFY, selectedOption2: this.selectedPeriod, onSelectionChange: this.onPieChartChange })),
                React.createElement("div", { className: "col-6" },
                    React.createElement(PieChart_1.PieChart, { key: "column1", ref: (c) => { this.barChart = c; }, field: "value", SeriesType: "column+line", categoryField: "name", data: this.state.series, title: "Labor", options: this.state.fyAndPeriod, selectedOption1: this.selectedFY, onSelectionChange: this.onBarChartChange })))));
    }
}
exports.LaborTab = LaborTab;
//# sourceMappingURL=LaborTab.js.map