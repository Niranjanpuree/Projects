import * as React from "react";
import { Remote } from "../../Common/Remote/Remote";
import { Dialog, DialogActionsBar } from "@progress/kendo-react-dialogs";
import { Button } from "@progress/kendo-react-buttons";
import { TabStrip, TabStripContent, TabStripTab } from "@progress/kendo-react-layout";
import { GroupDetails } from "./Tabs/GroupDetails"
import { GroupUsers } from "./Tabs/GroupUsers"
import { GroupPolicy } from "./Tabs/GroupPolicy"
import { GroupPermission } from "./Tabs/GroupPermission"
declare var window: any;

interface IGroupManagementProps
{
    groupGuid: string;
    showSaveButton: any;
}

interface IGroupManagementState
{
    tabIndex: number;
}

export class GroupManagement extends React.Component<IGroupManagementProps, IGroupManagementState>
{
    isComponentLoaded: boolean;
    constructor(props: IGroupManagementProps)
    {
        super(props);

        this.state = {
            tabIndex: 0
        }

        this.isComponentLoaded = false;
        this.onTabSelect = this.onTabSelect.bind(this);
    }

    componentWillMount()
    {
        this.isComponentLoaded = false;
    }

    componentDidMount()
    {
        this.isComponentLoaded = true;
        
    }

    

    onTabSelect(e: any)
    {
        this.setState({ tabIndex: e.selected });
    }

    render()
    {
        return (<div>
            <TabStrip onSelect={this.onTabSelect} selected={this.state.tabIndex}>
                {/*<TabStripTab title="Details">
                    <GroupDetails groupGuid={this.props.groupGuid}/>
                </TabStripTab>*/}
                <TabStripTab title="Users">
                    <GroupUsers groupGuid={this.props.groupGuid} showSaveButton={this.props.showSaveButton}  />
                </TabStripTab>
                <TabStripTab title="Permissions">
                    <GroupPermission groupGuid={this.props.groupGuid} showSaveButton={this.props.showSaveButton} />
                </TabStripTab>
                {/*<TabStripTab title="Policy">
                    <GroupPolicy groupGuid={this.props.groupGuid} />
                </TabStripTab>*/}
            </TabStrip>    
            </div>);
    }

}