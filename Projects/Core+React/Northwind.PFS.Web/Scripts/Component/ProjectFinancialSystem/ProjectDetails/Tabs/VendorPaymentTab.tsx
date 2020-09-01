import * as React from "react"
import * as ReactDOM from "react-dom"
import { Remote } from "../../../../Common/Remote/Remote";
import { KendoGrid } from "../../../../Common/Grid/KendoGrid";
import { Condition, Attribute, Operator } from "../../../Entities/Condition";
declare var $: any;
declare var window: any;
declare var siteBaseUrl: any;

interface IVendorPaymentTabPros
{
    projectId: any;
    fieldUrl: string;
    exportUrl: string;
    dataUrl: string;
}

interface IVendorPaymentTabState
{
    vendors: any[];
    selectedVendors: any[];
    switchOn: boolean;
    dataUrl: any;
}

export class VendorPaymentTab extends React.Component<IVendorPaymentTabPros, IVendorPaymentTabState> {
    listBox: any;
    grid: KendoGrid;
    constructor(props: any)
    {
        super(props);
        this.state = {
            vendors: [],//{ name: 'ABC Co', value: 1 }, { name: 'XYZ Co', value: 2 }, { name: 'Microsoft Inc', value: 3}, { name: 'Google Inc', value: 4 }, { name: 'ECC Inc', value: 5 }, { name: 'Apple Inc', value: 6 }, { name: 'Yahoo Inc', value: 7 }, { name: 'Zebra Inc', value: 8 }, { name: 'Okta Inc', value: 9 }, { name: 'Bottlers Inc', value: 10 }],
            selectedVendors: [],
            switchOn: false,
            dataUrl: this.props.fieldUrl + "&excludeIWOs=" + false
        }

        this.renderMultiSelect = this.renderMultiSelect.bind(this);
        this.bindMultiselectbox = this.bindMultiselectbox.bind(this);
        this.onGridLoaded = this.onGridLoaded.bind(this);
        this.onAddVendor = this.onAddVendor.bind(this);
        this.switchButton.action = this.switchButton.action.bind(this);
    }

    async componentDidMount()
    {
        await this.bindMultiselectbox()
    }

    async bindMultiselectbox()
    {
        let sender = this;
        let result = await Remote.postDataAsync(siteBaseUrl + "/ResourceAttribute/GetOptions", { attributeId: 'ec03a28d-2f91-4ce3-9805-5cc1b4017033', isEntityLookup: true, entity: 'vendorPayment-vendor' });
        if (result.ok) {
            let json = await result.json();
            this.setState({ vendors: json }, () =>
            {
                $("#options").kendoListBox({
                    disabled: false,
                    selectable: "multiple",
                    draggable: true,
                    connectWith: "selectedOptions",
                    toolbar: {
                        tools: ["transferTo", "transferFrom", "transferAllTo", "transferAllFrom"]
                    }
                });
                this.listBox = $("#selectedOptions").kendoListBox({ draggable: true }).data("kendoListBox");
            })
        }
        else {
            $("#options").kendoListBox({
                disabled: false,
                selectable: "multiple",
                draggable: true,
                connectWith: "selectedOptions",
                toolbar: {
                    tools: ["transferTo", "transferFrom", "transferAllTo", "transferAllFrom"]
                }
            });
            this.listBox = $("#selectedOptions").kendoListBox({ draggable: true }).data("kendoListBox");
        }
    }

    onGridLoaded(e: any)
    {
        
    }

    renderMultiSelect()
    {
        var vendors: any[] = this.state.vendors || [];
        return (<div className="col-4" style={{margin:'auto'}}>
            <select id="options" multiple>
                {vendors.map((v: any, i: number) =>
                {
                    return (<option key={i+""} value={v.value}>{v.name}</option>)
                })}
            </select>
            <select id="selectedOptions" name="selectedOptions" multiple onChange={() => { alert('') }}></select>
            <button onClick={this.onAddVendor}>Add</button>
        </div>)
    }

    onAddVendor(e: any)
    {
        let selected = this.listBox;
        let arr: any[] = [];
        let items = selected.dataItems();
        for (let i = 0; i < items.length; i++) {
            arr.push({ name: items[i].text, value: items[i].value });
        }
        var condition = new Condition();
        condition.Attribute = new Attribute("1", "vendor", "Vendor", 33, false, "");
        condition.ConditionId = "";
        condition.IsEntity = false;
        condition.Operator = new Operator("", 33, "In", 1)
        condition.Value = arr
        condition.Additional = true;
        let conditions: Condition[] = [];
        conditions.push(condition);
        this.setState({ selectedVendors: conditions }, this.forceUpdate);
        this.grid.updateExternalSearch(conditions);
    }

    switchButton = {
        onLabel: "Exclude IWOs",
        offLabel: "Include IWOs",
        checked: false,
        action: (status: any) =>
        {
            this.setState({ switchOn: status, dataUrl: this.props.dataUrl + "&excludeIWOs=" + status }, this.grid.refresh)
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
            <div>
                <div className="row">
                    <div className="col-12">
                        {this.renderMultiSelect()}
                    </div>
                    <div id="gridVendorPayment" className="col-12">
                        <KendoGrid ref={(c) => { this.grid = c; }} key="poGrid" showColumnMenu={false} showAddNewButton={false} showAdvanceSearchBox={true} gridMenu={gridMenu} advancedSearchEntity={["PFS-VendorPayment"]} identityField="id" fieldUrl={this.props.fieldUrl} exportFieldUrl={this.props.exportUrl} dataURL={this.state.dataUrl} externalFilters={this.state.selectedVendors} switchButton={this.switchButton} onGridLoaded={this.onGridLoaded} parent="gridVendorPayment" />
                    </div>
                </div>
            </div>
        );
    }
}