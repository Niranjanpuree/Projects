"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const kendo_react_charts_1 = require("@progress/kendo-react-charts");
const kendo_react_dateinputs_1 = require("@progress/kendo-react-dateinputs");
const kendo_react_buttons_1 = require("@progress/kendo-react-buttons");
const kendo_react_dropdowns_1 = require("@progress/kendo-react-dropdowns");
var SearchOption;
(function (SearchOption) {
    SearchOption[SearchOption["DateRange"] = 0] = "DateRange";
    SearchOption[SearchOption["FYPeriod"] = 1] = "FYPeriod";
    SearchOption[SearchOption["Fiscalyear"] = 2] = "Fiscalyear";
})(SearchOption = exports.SearchOption || (exports.SearchOption = {}));
class PieChart extends React.Component {
    constructor(props) {
        super(props);
        this.seriesColors = ["#500bda", "#3366cc", "#dd2c00", "#ff9500", "#109618", "#990099", "#3b3eac", "#0099c6", "#dd4477", "#66aa00", "#b82e2e", "#316395", "#994499", "#22aa99", "#aaaa11"];
        let selectedOption1 = this.props.options && this.props.options.length > 0 ? this.props.options[0] : { options: [] };
        let selectedOption2 = selectedOption1.options && selectedOption1.options.length > 0 ? selectedOption1.options[0] : {};
        if (this.props.selectedOption1) {
            selectedOption1 = this.props.selectedOption1;
        }
        if (this.props.selectedOption2) {
            selectedOption2 = this.props.selectedOption2;
        }
        this.selectedOption1 = selectedOption1;
        this.selectedOption2 = selectedOption2;
        this.state = {
            series: []
        };
        this.updateChart = this.updateChart.bind(this);
        this.renderDateRange = this.renderDateRange.bind(this);
        this.onStartDateChange = this.onStartDateChange.bind(this);
        this.onEndDateChange = this.onEndDateChange.bind(this);
        this.onSearch = this.onSearch.bind(this);
        this.onSearch1 = this.onSearch1.bind(this);
        this.onSearch2 = this.onSearch2.bind(this);
        this.selectOption1Change = this.selectOption1Change.bind(this);
        this.selectOption2Change = this.selectOption2Change.bind(this);
        this.updateSelections = this.updateSelections.bind(this);
        this.updateSelection = this.updateSelection.bind(this);
    }
    updateSelections(option1, option2) {
        this.selectedOption1 = option1;
        this.selectedOption2 = option2;
        this.forceUpdate();
    }
    updateSelection(option1) {
        this.selectedOption1 = option1;
        this.forceUpdate();
    }
    updateChart(data) {
        this.setState({ series: data }, this.forceUpdate);
    }
    onStartDateChange(e) {
        this.startDate = e.value;
    }
    onEndDateChange(e) {
        this.endDate = e.value;
    }
    onSearch(e) {
        if (this.props.onSelectionChange) {
            this.props.onSelectionChange(this.startDate, this.endDate, this);
        }
    }
    onSearch1(e) {
        if (this.props.onSelectionChange) {
            this.props.onSelectionChange(this.selectedOption1, this.selectedOption2, this);
        }
    }
    onSearch2(e) {
        if (this.props.onSelectionChange) {
            this.props.onSelectionChange(this.selectedOption1, this);
        }
    }
    selectOption1Change(e) {
        let selectedItem = e.target.value;
        this.selectedOption1 = selectedItem;
        if (this.props.searchOption.toString() === SearchOption.Fiscalyear.toString()) {
            this.props.onSelectionChange(this.selectedOption1, this);
        }
        else {
            if (!this.selectedOption2 && this.selectedOption1.options)
                this.selectedOption2 = this.selectedOption1.options[0];
            this.props.onSelectionChange(this.selectedOption1, this.selectedOption2, this);
        }
    }
    selectOption2Change(e) {
        let selectedItem = e.target.value;
        this.selectedOption2 = selectedItem;
        this.props.onSelectionChange(this.selectedOption1, this.selectedOption2);
    }
    renderDateRange() {
        if (this.props.searchOption && this.props.searchOption.toString() === SearchOption.DateRange.toString()) {
            return (React.createElement("div", { style: { padding: "15px 0" } },
                React.createElement("div", { style: { width: '587px', margin: 'auto' } },
                    React.createElement("div", { style: { "whiteSpace": "nowrap", paddingTop: '7px', display: 'inline-flex' } }, "Start Date:"),
                    React.createElement("div", { style: { width: "150px", display: 'inline-flex', paddingLeft: '10px' } },
                        React.createElement(kendo_react_dateinputs_1.DatePicker, { className: "form-control", onChange: this.onStartDateChange, value: this.startDate })),
                    React.createElement("div", { style: { "whiteSpace": "nowrap", paddingTop: '7px', display: 'inline-flex', paddingLeft: '10px' } }, "End Date:"),
                    React.createElement("div", { style: { width: "150px", display: 'inline-flex', paddingLeft: '10px' } },
                        React.createElement(kendo_react_dateinputs_1.DatePicker, { className: "form-control", onChange: this.onEndDateChange, value: this.endDate })),
                    React.createElement("div", { style: { width: "150px", display: 'inline-flex', paddingLeft: '10px' } },
                        React.createElement(kendo_react_buttons_1.Button, { onClick: this.onSearch }, "Show")))));
        }
        else if (this.props.searchOption && this.props.searchOption.toString() === SearchOption.FYPeriod.toString()) {
            if (!this.selectedOption1) {
                this.selectedOption1 = { options: [] };
            }
            else if (!this.selectedOption1.options) {
                this.selectedOption1.options = [];
            }
            return (React.createElement("div", { style: { padding: "15px 0" } },
                React.createElement("div", { style: { width: '300px', margin: 'auto' } },
                    React.createElement("div", { style: { width: "150px", display: 'inline-flex', paddingLeft: '10px' } },
                        React.createElement(kendo_react_dropdowns_1.DropDownList, { onChange: this.selectOption1Change, defaultItem: this.selectedOption1, data: this.props.options, dataItemKey: "value", textField: "name", label: "Fiscal Year" })),
                    React.createElement("div", { style: { width: "150px", display: 'inline-flex', paddingLeft: '10px' } },
                        React.createElement(kendo_react_dropdowns_1.DropDownList, { onChange: this.selectOption2Change, defaultItem: this.selectedOption2, label: "Period", data: this.selectedOption1.options, dataItemKey: "value", textField: "name" })))));
        }
        else if (this.props.searchOption && this.props.searchOption.toString() === SearchOption.Fiscalyear.toString()) {
            return (React.createElement("div", { style: { padding: "15px 0" } },
                React.createElement("div", { style: { width: '150px', margin: 'auto' } },
                    React.createElement("div", { style: { width: "150px", display: 'inline-flex', paddingLeft: '10px' } },
                        React.createElement(kendo_react_dropdowns_1.DropDownList, { onChange: this.selectOption1Change, data: this.props.options, defaultItem: this.selectedOption1, dataItemKey: "value", textField: "name", label: "Fiscal Year" })))));
        }
    }
    render() {
        if (this.seriesColors.length < this.state.series.length) {
            let i = 0;
            while (this.seriesColors.length < this.state.series.length) {
                this.seriesColors.push(this.seriesColors[i]);
                i++;
            }
        }
        let series = [];
        if (this.props.SeriesType === 'column') {
            let s = [];
            for (let i in this.seriesColors) {
                let a = [];
                s.push(this.seriesColors[i]);
            }
            series = s;
        }
        else {
            series = this.seriesColors;
        }
        let renderTooltip = (context) => {
            const { category, series, value } = context.point || context;
            return (React.createElement("div", null,
                category,
                " ",
                value));
        };
        const labelContent = (e) => (`${e.category}: \n ${e.value}`);
        const labelContentColumn = (e) => (`${e.value}`);
        if (this.props.SeriesType === "column") {
            let data = [];
            let column = [];
            for (let i in this.state.series) {
                data.push(this.state.series[i][this.props.field]);
                column.push(this.state.series[i][this.props.categoryField]);
            }
            return (React.createElement("div", null,
                this.renderDateRange(),
                React.createElement(kendo_react_charts_1.Chart, null,
                    React.createElement(kendo_react_charts_1.ChartTitle, { text: this.props.title }),
                    React.createElement(kendo_react_charts_1.ChartCategoryAxis, null,
                        React.createElement(kendo_react_charts_1.ChartCategoryAxisItem, { categories: column })),
                    React.createElement(kendo_react_charts_1.ChartSeries, null, data.map((d, index) => {
                        let d1 = [d];
                        return (React.createElement(kendo_react_charts_1.ChartSeriesItem, { key: "column-" + index, type: "column", gap: 2, color: series[index], spacing: 0.25, data: d1 },
                            React.createElement(kendo_react_charts_1.ChartSeriesLabels, { position: "outsideEnd", background: "none", content: labelContentColumn })));
                    })))));
        }
        else if (this.props.SeriesType === "column+line") {
            let data = [];
            let column = [];
            let lines = [];
            for (let i in this.state.series) {
                data.push(this.state.series[i][this.props.field]);
                column.push(this.state.series[i][this.props.categoryField]);
                lines.push(this.state.series[i][this.props.field]);
            }
            return (React.createElement("div", null,
                this.renderDateRange(),
                React.createElement(kendo_react_charts_1.Chart, null,
                    React.createElement(kendo_react_charts_1.ChartTitle, { text: this.props.title }),
                    React.createElement(kendo_react_charts_1.ChartTooltip, { render: renderTooltip }),
                    React.createElement(kendo_react_charts_1.ChartCategoryAxis, null,
                        React.createElement(kendo_react_charts_1.ChartCategoryAxisItem, { categories: column })),
                    React.createElement(kendo_react_charts_1.ChartSeries, null,
                        React.createElement(kendo_react_charts_1.ChartSeriesItem, { type: "line", data: data }),
                        React.createElement(kendo_react_charts_1.ChartSeriesItem, { type: "column", data: data },
                            React.createElement(kendo_react_charts_1.ChartSeriesLabels, { position: "outsideEnd", background: "none", content: labelContentColumn }))))));
        }
        else {
            return (React.createElement("div", null,
                this.renderDateRange(),
                React.createElement(kendo_react_charts_1.Chart, { seriesColors: series },
                    React.createElement(kendo_react_charts_1.ChartLegend, { visible: false }),
                    React.createElement(kendo_react_charts_1.ChartTooltip, { render: renderTooltip }),
                    React.createElement(kendo_react_charts_1.ChartTitle, { text: this.props.title }),
                    React.createElement(kendo_react_charts_1.ChartSeries, null,
                        React.createElement(kendo_react_charts_1.ChartSeriesItem, { type: "pie", data: this.state.series, field: this.props.field, categoryField: this.props.categoryField },
                            React.createElement(kendo_react_charts_1.ChartSeriesLabels, { position: "outsideEnd", background: "none", content: labelContent }))))));
        }
    }
}
exports.PieChart = PieChart;
//# sourceMappingURL=PieChart.js.map