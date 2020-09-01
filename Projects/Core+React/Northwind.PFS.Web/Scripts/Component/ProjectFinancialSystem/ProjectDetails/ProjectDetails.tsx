import * as React from "react"
import * as ReactDOM from "react-dom"
import { Remote } from "../../../Common/Remote/Remote";
import { TabStrip, TabStripTab, PanelBar, PanelBarItem, PanelBarUtils, Menu, MenuItem, MenuItemModel, MenuItemLink, MenuItemArrow, Splitter } from '@progress/kendo-react-layout'
import { ProjectDetailsTab } from "./Tabs/ProjectDetailsTab"
import { WBSTab, WbsUrls } from "./Tabs/WBSTab"
import { LaborTab } from "./Tabs/LaborTab"
import { BillingTab } from "./Tabs/BillingTab"
import { CostTab } from "./Tabs/CostTab"
import { POTab } from "./Tabs/POTab"
import { RAMTab } from "./Tabs/RAMTab"
import { RevenueTab } from "./Tabs/RevenueTab"
import { VendorPaymentTab } from "./Tabs/VendorPaymentTab"
import { WbsDictionaryList } from "./Views/WbsDictionaryList";
declare var $: any;
declare var window: any;

export interface IUrls
{
    dataUrl: string,
    fieldUrl: string,
    exportUrl: string
}

export interface DetailUrls
{
    Mod: IUrls,    
    WBS: WbsUrls,
    Labor: IUrls,
    Cost: IUrls,
    PO: IUrls,
    Revenue: IUrls,
    VendorPayment: IUrls,
    Billing: IUrls,
    RAM: IUrls
}

interface IProjectDetailsProps {
    selectedTabIndex: number;
    projectId: any;
    components: DetailUrls;
    currency: string;
} 

interface IProjectDetailsState {
    tabs: any[];
    selectedTabIndex: number;
}

export class ProjectDetails extends React.Component<IProjectDetailsProps, IProjectDetailsState> {
    constructor(props: any) {
        super(props);
        let tabs: any[] = [
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

    handleTabSelect(e: any)
    {
        this.setState({ selectedTabIndex: e.selected});
    }

    render()
    {
        
        return (
            <TabStrip selected={this.state.selectedTabIndex} onSelect={this.handleTabSelect}>
                {this.state.tabs.map(item =>
                {
                    if (this.state.selectedTabIndex === 0) {
                        return (
                            <TabStripTab key={"tab" + this.state.selectedTabIndex} title={item.title}>
                                <ProjectDetailsTab currency={this.props.currency} projectId={this.props.projectId} dataUrl={this.props.components.Mod.dataUrl} fieldUrl={this.props.components.Mod.fieldUrl} exportUrl={this.props.components.Mod.exportUrl}  />
                            </TabStripTab>
                        );
                    }
                    else if (this.state.selectedTabIndex === 1) {
                        return (
                            <TabStripTab key={"tab" + this.state.selectedTabIndex} title={item.title}>
                                <WBSTab projectNumber={this.props.projectId} urls={this.props.components.WBS} />
                            </TabStripTab>
                        );
                    }
                    else if (this.state.selectedTabIndex === 2) {
                        return (
                            <TabStripTab key={"tab" + this.state.selectedTabIndex} title={item.title}>
                                <LaborTab currency={this.props.currency} projectId={this.props.projectId} dataUrl={this.props.components.Labor.dataUrl} fieldUrl={this.props.components.Labor.fieldUrl} exportUrl={this.props.components.Labor.exportUrl} />
                            </TabStripTab>
                        );
                    }
                    else if (this.state.selectedTabIndex === 3) {
                        return (
                            <TabStripTab key={"tab" + this.state.selectedTabIndex} title={item.title}>
                                <CostTab projectId={this.props.projectId} dataUrl={this.props.components.Cost.dataUrl} fieldUrl={this.props.components.Cost.fieldUrl} exportUrl={this.props.components.Cost.exportUrl} />
                            </TabStripTab>
                        );
                    }
                    else if (this.state.selectedTabIndex === 4) {
                        return (
                            <TabStripTab key={"tab" + this.state.selectedTabIndex} title={item.title}>
                                <POTab projectId={this.props.projectId} dataUrl={this.props.components.PO.dataUrl} exportUrl={this.props.components.PO.exportUrl} fieldUrl={this.props.components.PO.fieldUrl} />
                            </TabStripTab>
                        );
                    }
                    else if (this.state.selectedTabIndex === 5) {
                        return (
                            <TabStripTab key={"tab" + this.state.selectedTabIndex} title={item.title}>
                                <RevenueTab projectId={this.props.projectId} />
                            </TabStripTab>
                        );
                    }
                    else if (this.state.selectedTabIndex === 6) {
                        return (
                            <TabStripTab key={"tab" + this.state.selectedTabIndex} title={item.title}>
                                <VendorPaymentTab projectId={this.props.projectId} dataUrl={this.props.components.VendorPayment.dataUrl} exportUrl={this.props.components.VendorPayment.exportUrl} fieldUrl={this.props.components.VendorPayment.fieldUrl} />
                            </TabStripTab>
                        );
                    }
                    else if (this.state.selectedTabIndex === 7) {
                        return (
                            <TabStripTab key={"tab" + this.state.selectedTabIndex} title={item.title}>
                                <RAMTab projectId={this.props.projectId} />
                            </TabStripTab>
                        );
                    }
                    return (
                        <TabStripTab title={item.title}>

                        </TabStripTab>
                    );
                
            })}
            </TabStrip>
        );
    }
}