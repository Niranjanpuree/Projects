import * as React from "react"
import * as ReactDOM from "react-dom"
import { Remote } from "../../../../Common/Remote/Remote";
import { PieChart } from "./PieChart";
import { DatePicker } from "@progress/kendo-react-dateinputs";
import { Button } from "@progress/kendo-react-buttons";
declare var $: any;
declare var window: any;

interface IRevenueTabPros
{
    projectId: any;
}

interface IRevenueTabState
{
    revenueSeries: any[];
    billingSeries: any[];
}

export class RevenueTab extends React.Component<IRevenueTabPros, IRevenueTabState> {
    revenueChart: PieChart;
    billingsChart: PieChart;
    startDate: Date;
    endDate: Date;
    enableSearch: any;

    constructor(props: any)
    {
        super(props);
        this.state = {
            revenueSeries: [],
            billingSeries: []
        }
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

    componentDidMount()
    {
        this.loadData();
    }

    loadData()
    {
        Promise.all([this.loadRevenue(), this.loadBillings()]).then((results: any[]) =>
        {
            this.revenueChart.updateChart(results[0]);
            this.billingsChart.updateChart(results[1])
        }).catch((reason: any) =>
        {
             
        })
    }

    async loadRevenue()
    {
        let result = await Remote.getAsync(window.siteBaseUrl + "/Revenue/Get?projectid=" + this.props.projectId);
        let json = await result.json();
        return Promise.resolve(json);
    }

    async loadBillings()
    {
        let result = await Remote.getAsync(window.siteBaseUrl + "/Billings/Get?projectid=" + this.props.projectId);
        let json = await result.json();
        return Promise.resolve(json);
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
        this.loadData();
    }

    onClearDate(e: any)
    {
        this.startDate = null;
        this.endDate = null;
        this.enableSearch = false;
        this.forceUpdate();
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

    render()
    {
        let sender = this;
        let gridMenu = [
            { text: 'Export to PDF', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
            { text: 'Export to Excel', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() } },
        ]
        return (
            <div>
                <div className="row">
                    <div style={{ padding: "15px 0", display: 'block', width: '587px', margin: 'auto' }}>
                        <div style={{ "whiteSpace": "nowrap", paddingTop: '7px', display: 'inline-flex' }}>Start Date:</div>
                        <div style={{ width: "150px", display: 'inline-flex', paddingLeft: '10px' }}><DatePicker className="form-control" onChange={this.onStartDateChange} value={this.startDate} /></div>
                        <div style={{ "whiteSpace": "nowrap", paddingTop: '7px', display: 'inline-flex', paddingLeft: '10px' }}>End Date:</div>
                        <div style={{ width: "150px", display: 'inline-flex', paddingLeft: '10px' }}><DatePicker className="form-control" onChange={this.onEndDateChange} value={this.endDate} /></div>
                        <div style={{ width: "75px", display: 'inline-flex', paddingLeft: '10px' }}><Button onClick={this.onSearch} disabled={!this.enableSearch}>Show</Button></div>
                        <div style={{ width: "75px", display: 'inline-flex', paddingLeft: '10px' }}><Button onClick={this.onClearDate} disabled={this.startDate === null && this.endDate === null}>Clear</Button></div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-6">
                        <PieChart key="column1" ref={(c) => { this.revenueChart = c; }} field="amount" SeriesType="column+line" categoryField="name" data={this.state.revenueSeries} title="Revenue" />
                    </div>
                    <div className="col-6">
                        <PieChart key="column2" ref={(c) => { this.billingsChart = c; }} field="amount" SeriesType="column+line" categoryField="name" data={this.state.billingSeries} title="Billings" />
                    </div>
                </div>
            </div>
        );
    }
}