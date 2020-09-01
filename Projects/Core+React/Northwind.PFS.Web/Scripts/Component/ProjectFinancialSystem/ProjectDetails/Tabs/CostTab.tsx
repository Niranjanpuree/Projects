import * as React from "react"
import * as ReactDOM from "react-dom"
import { Remote } from "../../../../Common/Remote/Remote";
import { PieChart, SearchOption } from "./PieChart";
import { KendoGrid } from "../../../../Common/Grid/KendoGrid";
import { DatePicker } from "@progress/kendo-react-dateinputs";
import { Button } from "@progress/kendo-react-buttons";
declare var $: any;
declare var window: any;

interface ICostTabPros
{
    projectId: any;
    dataUrl: string;
    fieldUrl: string;
    exportUrl: string;
}

interface ICostTabState
{
    series: any[],
    fyAndPeriod: any[]
    dataUrl: string;
    switchOn: boolean;
}

export class CostTab extends React.Component<ICostTabPros, ICostTabState> {
    pieChart: PieChart;
    barChart: PieChart;
    selectedFY: any;
    selectedPeriod: any;
    costGrid: KendoGrid;
    endDate: any;
    enableSearch: boolean;
    startDate: any;

    constructor(props: any)
    {
        super(props);
        this.state = {
            series: [],
            fyAndPeriod: [],
            dataUrl: this.props.dataUrl + "&excludeIWOs=" + false,
            switchOn: false
        }
        this.onGridLoaded = this.onGridLoaded.bind(this);
        this.onPieChartChange = this.onPieChartChange.bind(this);
        this.onBarChartChange = this.onBarChartChange.bind(this);
        this.loadPieGraphData = this.loadPieGraphData.bind(this);
        this.loadBarGraphData = this.loadBarGraphData.bind(this);
        this.onStartDateChange = this.onStartDateChange.bind(this);
        this.onEndDateChange = this.onEndDateChange.bind(this);
        this.onSearch = this.onSearch.bind(this);
        this.onClearDate = this.onClearDate.bind(this);
    }

    async componentDidMount()
    {
        let result = await Remote.getAsync(window.siteBaseUrl + "/labor/GetFiscalYearAndPeriod");
        if (result.ok) {
            let data = await result.json();
            if (data.length > 0) {
                this.selectedFY = data[0];
                if (data[0].options && data[0].options) {
                    this.selectedPeriod = data[0].options[0];
                }
            }

        }
    }

    async loadPieGraphData()
    {
        let searchParam = this.costGrid.getSearchParameters();
        let url = this.costGrid.getDataUrl().replace("/Get", "/GetPieChart");
        let result = await Remote.postDataAsync(url, searchParam.advancedSearchConditions);
        if (result.ok) {
            let data = await result.json();
            this.pieChart.updateChart(data);
        }
    }

    async loadBarGraphData()
    {
        let searchParam = this.costGrid.getSearchParameters();
        let url = this.costGrid.getDataUrl().replace("/Get", "/GetBarChart");
        let result1 = await Remote.postDataAsync(url, searchParam.advancedSearchConditions);
        if (result1.ok) {
            let data = await result1.json();
            this.barChart.updateChart(data);
        }
    }

    onGridLoaded(data: any[])
    {
        this.loadPieGraphData();
        this.loadBarGraphData();
    }

    async onPieChartChange(option1: any, option2: any)
    {
        
    }

    async onBarChartChange(option1: any)
    {
       
    }

    onStartDateChange(e: any)
    {
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
        this.forceUpdate()
    }

    onEndDateChange(e: any)
    {
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
        this.forceUpdate()
    }

    onSearch(e: any)
    {
         
        this.setState({ dataUrl: this.props.dataUrl + "&startDate=" + this.getCompleteDate(this.startDate) + "&endDate=" + this.getCompleteDate(this.endDate) }, this.costGrid.refresh);
    }

    onClearDate(e: any)
    {
        this.startDate = null;
        this.endDate = null;
        this.enableSearch = false;
        this.setState({ dataUrl: this.props.dataUrl }, this.costGrid.refresh);
    }

    getCompleteDate(d: any)
    {
        return this.getPadString(d.getMonth() + 1) + "-" + this.getPadString(d.getDate()) + "-" + d.getFullYear();
    }

    getPadString(d: any)
    {
        if (d > 9)
            return d + "";
        else
            return "0" + d;
    }

    switchButton = {
        onLabel: "Exclude IWOs",
        offLabel: "Include IWOs",
        checked: false,
        action: (status: any) =>
        {
            if (this.startDate && this.endDate) {
                this.setState({ switchOn: status, dataUrl: this.props.dataUrl + "&startDate=" + this.getCompleteDate(this.startDate) + "&endDate=" + this.getCompleteDate(this.endDate) + "&excludeIWOs=" + status }, this.costGrid.refresh)
            }
            else {
                this.setState({ switchOn: status, dataUrl: this.props.dataUrl + "&excludeIWOs=" + status }, this.costGrid.refresh)
            }

        }
    }

    render()
    {

        let sender = this;
        let gridMenu = [
            { text: 'Export to PDF', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
            { text: 'Export to Excel', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() } },
        ]
        return (
            <div id="poGrid" className="po-grid">
                <div className="row" style={{ position: 'absolute', zIndex: 1 }}>
                    <div style={{ display: 'block', width: '587px', margin: '0 auto -50px 558px' }}>
                        <div style={{ "whiteSpace": "nowrap", paddingTop: '7px', display: 'inline-flex' }}>Start Date:</div>
                        <div style={{ width: "150px", display: 'inline-flex', paddingLeft: '10px' }}><DatePicker className="form-control" onChange={this.onStartDateChange} value={this.startDate} /></div>
                        <div style={{ "whiteSpace": "nowrap", paddingTop: '7px', display: 'inline-flex', paddingLeft: '10px' }}>End Date:</div>
                        <div style={{ width: "150px", display: 'inline-flex', paddingLeft: '10px' }}><DatePicker className="form-control" onChange={this.onEndDateChange} value={this.endDate} /></div>
                        <div style={{ width: "75px", display: 'inline-flex', paddingLeft: '10px' }}><Button onClick={this.onSearch} disabled={!this.enableSearch}>Show</Button></div>
                        <div style={{ width: "75px", display: 'inline-flex', paddingLeft: '10px' }}><Button onClick={this.onClearDate} disabled={this.startDate === null && this.endDate === null}>Clear</Button></div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-12">
                        <KendoGrid ref={(c) => { this.costGrid = c; }} key="poGrid" showColumnMenu={false} showAddNewButton={false} showAdvanceSearchBox={true} advancedSearchEntity={["PFS-Cost"]} gridMenu={gridMenu} identityField="id" fieldUrl={this.props.fieldUrl} exportFieldUrl={this.props.exportUrl} dataURL={this.state.dataUrl} switchButton={this.switchButton} onGridLoaded={this.onGridLoaded} parent="poGrid" />
                    </div>
                </div>
                <div className="row">
                    <div className="col-6">
                        <PieChart key="pie1" ref={(c) => { this.pieChart = c; }} field="value" SeriesType="pie" categoryField="name" data={this.state.series} title="Cost" options={this.state.fyAndPeriod} selectedOption1={this.selectedFY} selectedOption2={this.selectedPeriod} onSelectionChange={this.onPieChartChange}  />
                    </div>
                    <div className="col-6">
                        <PieChart key="column1" ref={(c) => { this.barChart = c; }} field="value" SeriesType="column+line" categoryField="name" data={this.state.series} title="Cost" options={this.state.fyAndPeriod} selectedOption1={this.selectedFY} onSelectionChange={this.onBarChartChange} />
                    </div>
                </div>
            </div>
        );
    }
}