import * as React from "react"
import * as ReactDOM from "react-dom"
import { Remote } from "../../../../Common/Remote/Remote";
import { KendoGrid } from "../../../../Common/Grid/KendoGrid";
import { Chart, ChartLegend, ChartSeries, ChartSeriesItem, ChartTitle, ChartCategoryAxis, ChartCategoryAxisItem, ChartCategoryAxisTitle, ChartTooltip, ChartSeriesLabels } from '@progress/kendo-react-charts';
import { exact } from "prop-types";
import { SeriesType } from "@progress/kendo-react-charts/dist/npm/common/property-types";
import { DatePicker } from "@progress/kendo-react-dateinputs";
import { Button } from "@progress/kendo-react-buttons";
import { DropDownList } from "@progress/kendo-react-dropdowns";

declare var $: any;
declare var window: any;

export enum SearchOption
{
    DateRange,
    FYPeriod,
    Fiscalyear
}

interface IPieChartProps
{
    title: string;
    data: any[];
    options?: any[];
    selectedOption1?: any;
    selectedOption2?: any;
    field: string;
    categoryField: string;
    SeriesType: string;
    onSelectionChange?: any;
    searchOption?: SearchOption;
}

interface IPieChartState
{
    series: any[];
    
}

export class PieChart extends React.Component<IPieChartProps, IPieChartState> {
    seriesColors: any[];
    startDate: Date;
    endDate: Date;
    selectedOption1: any;
    selectedOption2: any;

    constructor(props: IPieChartProps)
    {
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

    updateSelections(option1: any, option2: any)
    {
        this.selectedOption1 = option1;
        this.selectedOption2 = option2;
        this.forceUpdate();
    }

    updateSelection(option1: any)
    {
        this.selectedOption1 = option1;
        this.forceUpdate();
    }

    updateChart(data: any[])
    {
        this.setState({ series: data}, this.forceUpdate)
    }

    onStartDateChange(e: any)
    {
        this.startDate = e.value;
    }

    onEndDateChange(e: any)
    {
        this.endDate = e.value;
    }

    onSearch(e: any)
    {
        if (this.props.onSelectionChange) {
            this.props.onSelectionChange(this.startDate, this.endDate, this);
        }
    }

    onSearch1(e: any)
    {
        if (this.props.onSelectionChange) {
            this.props.onSelectionChange(this.selectedOption1, this.selectedOption2, this);
        }
    }

    onSearch2(e: any)
    {
        if (this.props.onSelectionChange) {
            this.props.onSelectionChange(this.selectedOption1, this);
        }
    }

    selectOption1Change(e: any)
    {
        let selectedItem = e.target.value;
        this.selectedOption1 = selectedItem;
        if (this.props.searchOption.toString() === SearchOption.Fiscalyear.toString()) {
            this.props.onSelectionChange(this.selectedOption1, this);
        }
        else {
            if (!this.selectedOption2 && this.selectedOption1.options)
                this.selectedOption2 = this.selectedOption1.options[0]
            this.props.onSelectionChange(this.selectedOption1, this.selectedOption2, this);
        }
    }

    selectOption2Change(e: any)
    {
        let selectedItem = e.target.value;
        this.selectedOption2 = selectedItem;
        this.props.onSelectionChange(this.selectedOption1, this.selectedOption2);
    }

    renderDateRange()
    {
        if (this.props.searchOption && this.props.searchOption.toString() === SearchOption.DateRange.toString()) {
            return (<div style={{ padding: "15px 0" }}>
                <div style={{ width: '587px', margin: 'auto' }}>
                    <div style={{ "whiteSpace": "nowrap", paddingTop: '7px', display: 'inline-flex' }}>Start Date:</div>
                    <div style={{ width: "150px", display: 'inline-flex', paddingLeft: '10px' }}><DatePicker className="form-control" onChange={this.onStartDateChange} value={this.startDate} /></div>
                    <div style={{ "whiteSpace": "nowrap", paddingTop: '7px', display: 'inline-flex', paddingLeft: '10px' }}>End Date:</div>
                    <div style={{ width: "150px", display: 'inline-flex', paddingLeft: '10px' }}><DatePicker className="form-control" onChange={this.onEndDateChange} value={this.endDate} /></div>
                    <div style={{ width: "150px", display: 'inline-flex', paddingLeft: '10px' }}><Button onClick={this.onSearch}>Show</Button></div>
                </div>
            </div>)
        }
        else if (this.props.searchOption && this.props.searchOption.toString() === SearchOption.FYPeriod.toString()) {
            if (!this.selectedOption1) {
                this.selectedOption1 = { options: [] }
            }
            else if (!this.selectedOption1.options) {
                this.selectedOption1.options = []
            }
            return (<div style={{ padding: "15px 0" }}>
                <div style={{ width: '300px', margin: 'auto' }}>
                    <div style={{ width: "150px", display: 'inline-flex', paddingLeft: '10px' }}>
                        <DropDownList onChange={this.selectOption1Change} defaultItem={this.selectedOption1} data={this.props.options} dataItemKey="value" textField="name" label="Fiscal Year">
                            
                        </DropDownList>
                    </div>
                    <div style={{ width: "150px", display: 'inline-flex', paddingLeft: '10px' }}><DropDownList onChange={this.selectOption2Change} defaultItem={this.selectedOption2} label="Period" data={this.selectedOption1.options} dataItemKey="value" textField="name" ></DropDownList></div>
                    
                </div>
            </div>)
        }
        else if (this.props.searchOption && this.props.searchOption.toString() === SearchOption.Fiscalyear.toString()) {
            return (<div style={{ padding: "15px 0" }}>
                <div style={{ width: '150px', margin: 'auto' }}>
                    <div style={{ width: "150px", display: 'inline-flex', paddingLeft: '10px' }}><DropDownList onChange={this.selectOption1Change} data={this.props.options} defaultItem={this.selectedOption1} dataItemKey="value" textField="name" label="Fiscal Year"></DropDownList></div>
                    
                </div>
            </div>)
        }
    }

    render()
    {
        if (this.seriesColors.length < this.state.series.length) {
            let i = 0;
            while (this.seriesColors.length < this.state.series.length) {
                this.seriesColors.push(this.seriesColors[i]);
                i++;
            }
        }
        let series: any[] = [];
        if (this.props.SeriesType === 'column') {
            let s: any[] = [];
            for (let i in this.seriesColors) {
                let a: any[] = [];
                s.push(this.seriesColors[i])
            }
            series = s;
        }
        else {
            series = this.seriesColors;
        }
        let renderTooltip = (context: any) =>
        {
            const { category, series, value } = context.point || context;
            return (<div>{category} {value}</div>);
        };

        const labelContent = (e: any) => (`${e.category}: \n ${e.value}`);
        const labelContentColumn = (e: any) => (`${e.value}`);
        if (this.props.SeriesType === "column") {
            let data: any[] = [];
            let column: any[] = [];
            for (let i in this.state.series) {
                data.push(this.state.series[i][this.props.field])
                column.push(this.state.series[i][this.props.categoryField])
            }
            return (<div>
                {this.renderDateRange()}
                <Chart>
                    <ChartTitle text={this.props.title} />
                    <ChartCategoryAxis>
                        <ChartCategoryAxisItem categories={column} />
                    </ChartCategoryAxis>
                    <ChartSeries>
                        {data.map((d, index: any) =>
                        {
                            let d1: any[] = [d];
                            return (<ChartSeriesItem key={"column-" + index} type="column" gap={2} color={series[index]} spacing={0.25} data={d1}>
                                <ChartSeriesLabels
                                    position="outsideEnd"
                                    background="none"
                                    content={labelContentColumn} />
                            </ChartSeriesItem>
                            )
                        })}
                    </ChartSeries>
                </Chart>
            </div>)
        }
        else if (this.props.SeriesType === "column+line") {
            let data: any[] = [];
            let column: any[] = [];
            let lines: any[] = [];
            for (let i in this.state.series) {
                data.push(this.state.series[i][this.props.field])
                column.push(this.state.series[i][this.props.categoryField])
                lines.push(this.state.series[i][this.props.field])
            }
            return (<div>
                {this.renderDateRange()}
                <Chart>
                    <ChartTitle text={this.props.title} />
                    <ChartTooltip render={renderTooltip} />
                    <ChartCategoryAxis>
                        <ChartCategoryAxisItem categories={column} />
                    </ChartCategoryAxis>
                    <ChartSeries>
                        <ChartSeriesItem type="line" data={data} />
                        <ChartSeriesItem type="column" data={data}>
                            <ChartSeriesLabels
                                position="outsideEnd"
                                background="none"
                                content={labelContentColumn} />
                        </ChartSeriesItem>
                    </ChartSeries>
                </Chart>
            </div>)
        }
        else {
            

            return (
                <div>
                    {this.renderDateRange()}
                    <Chart seriesColors={series} >
                        <ChartLegend visible={false} />
                        <ChartTooltip render={renderTooltip} />
                        <ChartTitle text={this.props.title} />
                        <ChartSeries>
                            <ChartSeriesItem type="pie" data={this.state.series} field={this.props.field} categoryField={this.props.categoryField}>
                                <ChartSeriesLabels
                                    position="outsideEnd"
                                    background="none"
                                    content={labelContent} />
                            </ChartSeriesItem>
                        </ChartSeries>
                    </Chart>
                </div>
            )
        }
        
        
    }
}