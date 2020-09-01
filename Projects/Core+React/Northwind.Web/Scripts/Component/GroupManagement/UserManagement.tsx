import * as React from "react";
import { Remote } from "../../Common/Remote/Remote";
import { Dialog, DialogActionsBar } from "@progress/kendo-react-dialogs";
import { Button } from "@progress/kendo-react-buttons";
import { TabStrip, TabStripContent, TabStripTab } from "@progress/kendo-react-layout";
import { GroupDetails } from "./Tabs/GroupDetails"
import { GroupUsers } from "./Tabs/GroupUsers"
import { GroupPolicy } from "./Tabs/GroupPolicy"
import { UsersInfo } from "./Tabs/UsersInfo"
import { UserPermission } from "./tabs/userpermission";
import { UsersGroup } from "./tabs/usersgroup";
declare var window: any;

interface IUserManagementProps
{
    userGuid: string;
    userName?: string;
    
}

interface IUserManagementState
{
    tabIndex: number;
    accessGroupCount?: number;
    accessPermissionCount?: number;
   
}

export class UserManagement extends React.Component<IUserManagementProps, IUserManagementState>
{
    isComponentLoaded: boolean;
    constructor(props: IUserManagementProps)
    {
        super(props);

        this.state = {
            tabIndex: 0,
            accessGroupCount: 0,
            accessPermissionCount:0
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
        this.getGroupCount();
        this.getPermissionCount();
        
    }

    getGroupCount() {
        let sender = this;
        Remote.get("/iam/user/AssignedGroups/" + this.props.userGuid, (data: any) => {
           sender.setState({ accessGroupCount: data.count});
        }, (data: any) => { window.Dialog.alert(data); })
    }

    getPermissionCount() {
        let sender = this;
        Remote.get("/iam/user/AssignedGroupsCount/" + this.props.userGuid, (data: any) => {
            sender.setState({ accessPermissionCount: data.count });
        }, (data: any) => { window.Dialog.alert(data); })
    }

    onTabSelect(e: any)
    {
        this.setState({ tabIndex: e.selected });
    }

    render()
    {
        return (<div>
            <TabStrip onSelect={this.onTabSelect} selected={this.state.tabIndex}>
               
                <TabStripTab title="Basic Information">
                    <UsersInfo userGuid={this.props.userGuid}   />
                </TabStripTab>
                <TabStripTab title={"Group Membership (" +this.state.accessGroupCount+")"}>
                    <UsersGroup userGuid={this.props.userGuid} userName={this.props.userName} />
                </TabStripTab>
                <TabStripTab title={"Effective Permissions (" +this.state.accessPermissionCount+")"}>
                    <UserPermission groupGuid={this.props.userGuid} userName={this.props.userName} />
                </TabStripTab>
               
            </TabStrip>    
            </div>);
    }

}