import * as React from "react"
import * as ReactDOM from "react-dom"
import { Remote } from "../../../../Common/Remote/Remote";
import { KendoGrid } from "../../../../Common/Grid/KendoGrid";
import { PieChart } from "./PieChart";
import { KendoGroupableGrid } from "../../../../Common/Grid/KendoGroupableGrid";
import { DatePicker } from "@progress/kendo-react-dateinputs";
import { Button } from "@progress/kendo-react-buttons";
declare var $: any;
declare var window: any;

interface IPOTabPros
{
    projectId: any;
    dataUrl: string;
    fieldUrl: string;
    exportUrl: string;
}

interface IPOTabState
{
    series: any[];
    switchOn: boolean;
    dataUrl: string;
    excludeIWOs: boolean;
}

export class POTab extends React.Component<IPOTabPros, IPOTabState> {
    pieChart: PieChart;
    barChart: PieChart;
    kendoGrid: KendoGrid;
    kendoGroupableGrid: KendoGroupableGrid;
    searchParams: any;
    externalDataState: any;
    startDate: any;
    endDate: any;
    enableSearch: any;
    
    constructor(props: any)
    {
        super(props);
        this.state = {
            series: [],
            switchOn: false,
            dataUrl: this.props.dataUrl + "&excludeIWOs=" + false,
            excludeIWOs: false
        }
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
    
    switchButton = {
        onLabel: "By Vendor",
        offLabel: "By PO ID",
        checked: false,
        action: (status: any) =>
        {
            this.setState({ switchOn: status })
            this.searchParams = this.kendoGrid.getSearchParameters();
            this.externalDataState = this.kendoGrid.state.dataState;
        }
    }

    switchButton1 = {
        onLabel: "By Vendor",
        offLabel: "By PO ID",
        checked: true,
        action: (status: any) =>
        {
            this.setState({ switchOn: status })
            this.searchParams = this.kendoGroupableGrid.getSearchParameters();
            this.externalDataState = this.kendoGroupableGrid.state.gridState;
        }
    }

    switchButtonIWO = {
        onLabel: "Exclude IWOs",
        offLabel: "Include IWOs",
        checked: false,
        action: (status: any) =>
        {
            if (this.startDate && this.endDate) {
                this.setState({ excludeIWOs: status, dataUrl: this.props.dataUrl + "&startDate=" + this.getCompleteDate(this.startDate) + "&endDate=" + this.getCompleteDate(this.endDate) + "&excludeIWOs=" + status }, this.kendoGrid.refresh)
            }
            else {
                this.setState({ excludeIWOs: status, dataUrl: this.props.dataUrl + "&excludeIWOs=" + status }, this.kendoGrid.refresh)
            }

        }
    }

    switchButtonIWO1 = {
        onLabel: "Exclude IWOs",
        offLabel: "Include IWOs",
        checked: false,
        action: (status: any) =>
        {
            if (this.startDate && this.endDate) {
                this.setState({ excludeIWOs: status, dataUrl: this.props.dataUrl + "&startDate=" + this.getCompleteDate(this.startDate) + "&endDate=" + this.getCompleteDate(this.endDate) + "&excludeIWOs=" + status }, () => { this.kendoGroupableGrid.refresh(null) })
            }
            else {
                this.setState({ excludeIWOs: status, dataUrl: this.props.dataUrl + "&excludeIWOs=" + status }, () => { this.kendoGroupableGrid.refresh(null) })
            }

        }
    }


    onGridLoaded(e: any)
    {
        
    }

    onGroupableGridLoaded(e: any)
    {
        
    }

    onSearch(e: any)
    {
        if (this.state.excludeIWOs) {
            this.setState({ dataUrl: this.props.dataUrl + "&startDate=" + this.getCompleteDate(this.startDate) + "&endDate=" + this.getCompleteDate(this.endDate) + "&excludeIWOs=" + this.state.excludeIWOs }, () => { this.kendoGroupableGrid.refresh(null) });
        }
        else {
            this.setState({ dataUrl: this.props.dataUrl + "&startDate=" + this.getCompleteDate(this.startDate) + "&endDate=" + this.getCompleteDate(this.endDate) + "&excludeIWOs=" + this.state.excludeIWOs }, this.kendoGrid.refresh);
        }
        
    }

    onClearDate(e: any)
    {
        this.startDate = null;
        this.endDate = null;
        this.enableSearch = false;
        if (this.state.switchOn) {
            this.setState({ dataUrl: this.props.dataUrl }, () => { this.kendoGroupableGrid.refresh(null) });
        }
        else {
            this.setState({ dataUrl: this.props.dataUrl }, this.kendoGrid.refresh);
        }
        
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

    createRowMenu()
    {
        let sender = this;
        let groupRowMenu = [
            {
                text: "View PO Details", icon: "folder-open", action: (data: any, grid: any) =>
                {
                    
                }
            },
            {
                text: "View Transaction Details", icon: "folder-open", action: (data: any, grid: any) =>
                {

                }
            }
        ];
        return groupRowMenu;
    }

    render()
    {
        let gridMenu = [
            { text: 'Export to PDF', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
            { text: 'Export to Excel', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() } },
        ]
        let rowMenu = this.createRowMenu();
        if (this.state.switchOn === false) {
            let switchButtons: any[] = [];
            this.switchButtonIWO.checked = this.state.excludeIWOs;
            switchButtons.push(this.switchButton)
            switchButtons.push(this.switchButtonIWO)
            return (
                <div id="costGrid" className="cost-grid">
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
                            <KendoGrid ref={(c) => { this.kendoGrid = c; }} externalDataState={this.externalDataState} key="poGrid" rowMenus={rowMenu} showColumnMenu={false} showAddNewButton={false} showAdvanceSearchBox={true} advancedSearchEntity={["PFS-PO"]} gridMenu={gridMenu} searchParameters={this.searchParams} identityField="poCommitmentId" fieldUrl={this.props.fieldUrl} exportFieldUrl={this.props.exportUrl} dataURL={this.state.dataUrl} switchButton={switchButtons} parent="costGrid" onGridLoaded={this.onGridLoaded} />
                        </div>
                    </div>
                </div>
            );
        }
        else {
            let switchButtons: any[] = [];
            this.switchButtonIWO1.checked = this.state.excludeIWOs;
            switchButtons.push(this.switchButton1)
            switchButtons.push(this.switchButtonIWO1)
            return (
                <div id="costGrid" className="cost-grid">
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
                            <KendoGroupableGrid ref={(c) => { this.kendoGroupableGrid = c; }} externalDataState={this.externalDataState} key="poGrid" rowMenus={rowMenu} groupField="vendorName" showColumnMenu={false} showAdvancedSearchDialog={true} advancedSearchEntity={["PFS-PO"]} searchParameters={this.searchParams} groupTotalFields={["totalAmount", "voucheredAmount", "balance"]} gridMenu={gridMenu} identityField="poCommitmentId" columnUrl={this.props.fieldUrl} exportFieldUrl={this.props.exportUrl} dataUrl={this.state.dataUrl} showSearchBox={true} showGridAction={true} currencySymbol="USD" switchButton={switchButtons} onGridLoaded={this.onGroupableGridLoaded} container="costGrid" />
                        </div>
                    </div>
                </div>
            );
        }
        
    }
}