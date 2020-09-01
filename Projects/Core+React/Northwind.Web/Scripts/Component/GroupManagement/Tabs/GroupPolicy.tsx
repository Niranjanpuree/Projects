import * as React from "react";
import { Remote } from "../../../Common/Remote/Remote";
import { Dialog, DialogActionsBar } from "@progress/kendo-react-dialogs";
import { Button } from "@progress/kendo-react-buttons";
import { TabStrip, TabStripContent, TabStripTab } from "@progress/kendo-react-layout";
import { KendoGrid } from "../../../Common/Grid/KendoGrid";
import { AutoComplete } from "../../../Common/AutoComplete/AutoComplete"
declare var window: any;
declare var $: any;

interface IGroupPolicyProps
{
    groupGuid: string;
}

interface IGroupPolicyState
{
    groupName: string;
    selectedPolicy: any;
}

export class GroupPolicy extends React.Component<IGroupPolicyProps, IGroupPolicyState>
{
    isComponentLoaded: boolean;
    grid: KendoGrid;
    autoComplete: AutoComplete;
    constructor(props: IGroupPolicyProps)
    {
        super(props);
        this.state = {
            groupName: '',
            selectedPolicy: null
        }
        this.loadGroup = this.loadGroup.bind(this);
        this.onAutoCompleteChange = this.onAutoCompleteChange.bind(this);
        this.onAssign = this.onAssign.bind(this);
    }

    componentWillMount()
    {
        this.isComponentLoaded = false;
    }

    async componentDidMount()
    {
        this.isComponentLoaded = true;
        if (this.isComponentLoaded) {
            await this.loadGroup()
        }
    }

    async loadGroup()
    {
        var response1 = await Remote.getAsync("/IAM/Group/GetGroup/" + this.props.groupGuid);
        if (response1.ok) {
            let json = await response1.json();
            this.setState({ groupName: json.cn }, this.forceUpdate)
        }
    }

    onAutoCompleteChange(e: any)
    {
        this.setState({ selectedPolicy: e });
    }

    async onAssign(e: any)
    {
        if (this.state.selectedPolicy === null) {
            window.Dialog.alert("Please select a user", "Error");
            return;
        }
         
        let result = await Remote.postDataAsync("/iam/group/AssignPolicy/" + this.props.groupGuid, this.state.selectedPolicy);
        if (result.ok) {
            this.setState({ selectedPolicy: null }, () => { this.grid.refresh(), this.autoComplete.clearValue() })
        }
        else {
            let error = JSON.parse(await result.text());
            window.Dialog.alert(error[""].errors[0].errorMessage, "Error");
        }

    }

    render()
    {
        let rowMenu = [
            {
                text: "Unassign", icon: "delete", action: async (data: any, grid: any) =>
                {
                    let data1 = data.dataItem;
                    data1.groupGuid = this.props.groupGuid;
                    let result = await Remote.postDataAsync("/iam/group/UnassignPolicy", data1)
                    if (result.ok) {
                        grid.refresh();
                    }
                    else {
                        let error = JSON.parse(await result.text());
                        window.Dialog.alert(error[""].errors[0].errorMessage, "Error");
                    }
                }
            }
        ];
        return (
            <div id="GroupUser" className="GroupUser gridparent row">
                <div className="col-6">
                    <div className="form-group">
                        <label className="control-label">Policies In Group {this.state.groupName}</label>
                    </div>
                </div>
                <div className="col-6">
                    <div className="form-group row">
                        <label className="control-label col-4">Assign Policy to Group</label>
                        <AutoComplete ref={(c) => { this.autoComplete = c; }} dataUrl={"/iam/group/UnassignedPolicy?groupGuid=" + this.props.groupGuid} className="form-control col-6" onChange={this.onAutoCompleteChange} displayField="name" placeHolder="Please type" valueField="policyGuid" id="groupPolicy" />
                        <div className="col-2 text-right">
                            <button className="btn btn-primary" disabled={this.state.selectedPolicy === null} onClick={this.onAssign}>Assign</button>
                        </div>
                    </div>
                </div>
                <div className="col-12" id="GroupPolicyGrid">
                    <KendoGrid ref={(c) => { this.grid = c; }} dataURL={"/iam/group/AssignedPolicy?id=" + this.props.groupGuid} fieldUrl="/GridFields/GroupPolicy" exportFieldUrl="/Export/GroupPolicy" gridHeight="150" identityField="groupPolicyGuid" showAddNewButton={false} rowMenus={rowMenu} showColumnMenu={false} showSearchBox={false} showAdvanceSearchBox={false} showGridAction={false} parent="GroupPolicyGrid" />
                </div>
            </div>
        );
    }
}