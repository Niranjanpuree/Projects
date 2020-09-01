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
const KendoGrid_1 = require("../../../../Common/Grid/KendoGrid");
const Condition_1 = require("../../../Entities/Condition");
class VendorPaymentTab extends React.Component {
    constructor(props) {
        super(props);
        this.switchButton = {
            onLabel: "Exclude IWOs",
            offLabel: "Include IWOs",
            checked: false,
            action: (status) => {
                this.setState({ switchOn: status, dataUrl: this.props.dataUrl + "&excludeIWOs=" + status }, this.grid.refresh);
            }
        };
        this.state = {
            vendors: [],
            selectedVendors: [],
            switchOn: false,
            dataUrl: this.props.fieldUrl + "&excludeIWOs=" + false
        };
        this.renderMultiSelect = this.renderMultiSelect.bind(this);
        this.bindMultiselectbox = this.bindMultiselectbox.bind(this);
        this.onGridLoaded = this.onGridLoaded.bind(this);
        this.onAddVendor = this.onAddVendor.bind(this);
        this.switchButton.action = this.switchButton.action.bind(this);
    }
    componentDidMount() {
        return __awaiter(this, void 0, void 0, function* () {
            yield this.bindMultiselectbox();
        });
    }
    bindMultiselectbox() {
        return __awaiter(this, void 0, void 0, function* () {
            let sender = this;
            let result = yield Remote_1.Remote.postDataAsync(siteBaseUrl + "/ResourceAttribute/GetOptions", { attributeId: 'ec03a28d-2f91-4ce3-9805-5cc1b4017033', isEntityLookup: true, entity: 'vendorPayment-vendor' });
            if (result.ok) {
                let json = yield result.json();
                this.setState({ vendors: json }, () => {
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
                });
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
        });
    }
    onGridLoaded(e) {
    }
    renderMultiSelect() {
        var vendors = this.state.vendors || [];
        return (React.createElement("div", { className: "col-4", style: { margin: 'auto' } },
            React.createElement("select", { id: "options", multiple: true }, vendors.map((v, i) => {
                return (React.createElement("option", { key: i + "", value: v.value }, v.name));
            })),
            React.createElement("select", { id: "selectedOptions", name: "selectedOptions", multiple: true, onChange: () => { alert(''); } }),
            React.createElement("button", { onClick: this.onAddVendor }, "Add")));
    }
    onAddVendor(e) {
        let selected = this.listBox;
        let arr = [];
        let items = selected.dataItems();
        for (let i = 0; i < items.length; i++) {
            arr.push({ name: items[i].text, value: items[i].value });
        }
        var condition = new Condition_1.Condition();
        condition.Attribute = new Condition_1.Attribute("1", "vendor", "Vendor", 33, false, "");
        condition.ConditionId = "";
        condition.IsEntity = false;
        condition.Operator = new Condition_1.Operator("", 33, "In", 1);
        condition.Value = arr;
        condition.Additional = true;
        let conditions = [];
        conditions.push(condition);
        this.setState({ selectedVendors: conditions }, this.forceUpdate);
        this.grid.updateExternalSearch(conditions);
    }
    render() {
        let sender = this;
        let gridMenu = [
            { text: 'Export to PDF', icon: 'pdf', action: (data, grid) => { grid.exportToPDF(); } },
            { text: 'Export to Excel', icon: 'excel', action: (data, grid) => { grid.exportToExcel(); } },
        ];
        return (React.createElement("div", null,
            React.createElement("div", { className: "row" },
                React.createElement("div", { className: "col-12" }, this.renderMultiSelect()),
                React.createElement("div", { id: "gridVendorPayment", className: "col-12" },
                    React.createElement(KendoGrid_1.KendoGrid, { ref: (c) => { this.grid = c; }, key: "poGrid", showColumnMenu: false, showAddNewButton: false, showAdvanceSearchBox: true, gridMenu: gridMenu, advancedSearchEntity: ["PFS-VendorPayment"], identityField: "id", fieldUrl: this.props.fieldUrl, exportFieldUrl: this.props.exportUrl, dataURL: this.state.dataUrl, externalFilters: this.state.selectedVendors, switchButton: this.switchButton, onGridLoaded: this.onGridLoaded, parent: "gridVendorPayment" })))));
    }
}
exports.VendorPaymentTab = VendorPaymentTab;
//# sourceMappingURL=VendorPaymentTab.js.map