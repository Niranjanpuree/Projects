"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const kendo_react_layout_1 = require("@progress/kendo-react-layout");
const ProjectDetailsTab_1 = require("./Tabs/ProjectDetailsTab");
const WBSTab_1 = require("./Tabs/WBSTab");
const LaborTab_1 = require("./Tabs/LaborTab");
const CostTab_1 = require("./Tabs/CostTab");
const POTab_1 = require("./Tabs/POTab");
const RAMTab_1 = require("./Tabs/RAMTab");
const RevenueTab_1 = require("./Tabs/RevenueTab");
const VendorPaymentTab_1 = require("./Tabs/VendorPaymentTab");
class ProjectDetails extends React.Component {
    constructor(props) {
        super(props);
        let tabs = [
            { title: 'Project Info' },
            { title: 'WBS' },
            { title: 'Labor' },
            { title: 'Cost' },
            { title: 'PO' },
            { title: 'Revenue & Billings' },
            { title: 'Vendor Payment' },
            { title: 'RAM' }
        ];
        this.state = {
            tabs: tabs,
            selectedTabIndex: this.props.selectedTabIndex
        };
        this.handleTabSelect = this.handleTabSelect.bind(this);
    }
    handleTabSelect(e) {
        this.setState({ selectedTabIndex: e.selected });
    }
    render() {
        return (React.createElement(kendo_react_layout_1.TabStrip, { selected: this.state.selectedTabIndex, onSelect: this.handleTabSelect }, this.state.tabs.map(item => {
            if (this.state.selectedTabIndex === 0) {
                return (React.createElement(kendo_react_layout_1.TabStripTab, { key: "tab" + this.state.selectedTabIndex, title: item.title },
                    React.createElement(ProjectDetailsTab_1.ProjectDetailsTab, { currency: this.props.currency, projectId: this.props.projectId, dataUrl: this.props.components.Mod.dataUrl, fieldUrl: this.props.components.Mod.fieldUrl, exportUrl: this.props.components.Mod.exportUrl })));
            }
            else if (this.state.selectedTabIndex === 1) {
                return (React.createElement(kendo_react_layout_1.TabStripTab, { key: "tab" + this.state.selectedTabIndex, title: item.title },
                    React.createElement(WBSTab_1.WBSTab, { projectNumber: this.props.projectId, urls: this.props.components.WBS })));
            }
            else if (this.state.selectedTabIndex === 2) {
                return (React.createElement(kendo_react_layout_1.TabStripTab, { key: "tab" + this.state.selectedTabIndex, title: item.title },
                    React.createElement(LaborTab_1.LaborTab, { currency: this.props.currency, projectId: this.props.projectId, dataUrl: this.props.components.Labor.dataUrl, fieldUrl: this.props.components.Labor.fieldUrl, exportUrl: this.props.components.Labor.exportUrl })));
            }
            else if (this.state.selectedTabIndex === 3) {
                return (React.createElement(kendo_react_layout_1.TabStripTab, { key: "tab" + this.state.selectedTabIndex, title: item.title },
                    React.createElement(CostTab_1.CostTab, { projectId: this.props.projectId, dataUrl: this.props.components.Cost.dataUrl, fieldUrl: this.props.components.Cost.fieldUrl, exportUrl: this.props.components.Cost.exportUrl })));
            }
            else if (this.state.selectedTabIndex === 4) {
                return (React.createElement(kendo_react_layout_1.TabStripTab, { key: "tab" + this.state.selectedTabIndex, title: item.title },
                    React.createElement(POTab_1.POTab, { projectId: this.props.projectId, dataUrl: this.props.components.PO.dataUrl, exportUrl: this.props.components.PO.exportUrl, fieldUrl: this.props.components.PO.fieldUrl })));
            }
            else if (this.state.selectedTabIndex === 5) {
                return (React.createElement(kendo_react_layout_1.TabStripTab, { key: "tab" + this.state.selectedTabIndex, title: item.title },
                    React.createElement(RevenueTab_1.RevenueTab, { projectId: this.props.projectId })));
            }
            else if (this.state.selectedTabIndex === 6) {
                return (React.createElement(kendo_react_layout_1.TabStripTab, { key: "tab" + this.state.selectedTabIndex, title: item.title },
                    React.createElement(VendorPaymentTab_1.VendorPaymentTab, { projectId: this.props.projectId, dataUrl: this.props.components.VendorPayment.dataUrl, exportUrl: this.props.components.VendorPayment.exportUrl, fieldUrl: this.props.components.VendorPayment.fieldUrl })));
            }
            else if (this.state.selectedTabIndex === 7) {
                return (React.createElement(kendo_react_layout_1.TabStripTab, { key: "tab" + this.state.selectedTabIndex, title: item.title },
                    React.createElement(RAMTab_1.RAMTab, { projectId: this.props.projectId })));
            }
            return (React.createElement(kendo_react_layout_1.TabStripTab, { title: item.title }));
        })));
    }
}
exports.ProjectDetails = ProjectDetails;
//# sourceMappingURL=ProjectDetails.js.map