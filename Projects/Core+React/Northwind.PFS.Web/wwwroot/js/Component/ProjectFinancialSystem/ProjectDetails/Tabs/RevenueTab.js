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
const kendo_react_dateinputs_1 = require("@progress/kendo-react-dateinputs");
const kendo_react_buttons_1 = require("@progress/kendo-react-buttons");
class RevenueTab extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            revenueSeries: [],
            billingSeries: []
        };
        let startDate = new Date();
        let endDate = new Date();
        this.loadBillings = this.loadBillings.bind(this);
        this.loadBillings = this.loadBillings.bind(this);
        this.onStartDateChange = this.onStartDateChange.bind(this);
        this.onEndDateChange = this.onEndDateChange.bind(this);
        this.loadData = this.loadData.bind(this);
        this.onSearch = this.onSearch.bind(this);
        this.onClearDate = this.onClearDate.bind(this);
    }
    componentDidMount() {
        this.loadData();
    }
    loadData() {
        Promise.all([this.loadRevenue(), this.loadBillings()]).then((results) => {
            this.revenueChart.updateChart(results[0]);
            this.billingsChart.updateChart(results[1]);
        }).catch((reason) => {
        });
    }
    loadRevenue() {
        return __awaiter(this, void 0, void 0, function* () {
            let result = yield Remote_1.Remote.getAsync(window.siteBaseUrl + "/Revenue/Get?projectid=" + this.props.projectId);
            let json = yield result.json();
            return Promise.resolve(json);
        });
    }
    loadBillings() {
        return __awaiter(this, void 0, void 0, function* () {
            let result = yield Remote_1.Remote.getAsync(window.siteBaseUrl + "/Billings/Get?projectid=" + this.props.projectId);
            let json = yield result.json();
            return Promise.resolve(json);
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
        this.loadData();
    }
    onClearDate(e) {
        this.startDate = null;
        this.endDate = null;
        this.enableSearch = false;
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
    render() {
        let sender = this;
        let gridMenu = [
            { text: 'Export to PDF', icon: 'pdf', action: (data, grid) => { grid.exportToPDF(); } },
            { text: 'Export to Excel', icon: 'excel', action: (data, grid) => { grid.exportToExcel(); } },
        ];
        return (React.createElement("div", null,
            React.createElement("div", { className: "row" },
                React.createElement("div", { style: { padding: "15px 0", display: 'block', width: '587px', margin: 'auto' } },
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
                React.createElement("div", { className: "col-6" },
                    React.createElement(PieChart_1.PieChart, { key: "column1", ref: (c) => { this.revenueChart = c; }, field: "amount", SeriesType: "column+line", categoryField: "name", data: this.state.revenueSeries, title: "Revenue" })),
                React.createElement("div", { className: "col-6" },
                    React.createElement(PieChart_1.PieChart, { key: "column2", ref: (c) => { this.billingsChart = c; }, field: "amount", SeriesType: "column+line", categoryField: "name", data: this.state.billingSeries, title: "Billings" })))));
    }
}
exports.RevenueTab = RevenueTab;
//# sourceMappingURL=RevenueTab.js.map