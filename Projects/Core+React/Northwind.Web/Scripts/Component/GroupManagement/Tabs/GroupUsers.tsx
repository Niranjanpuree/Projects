import * as React from "react";
import { Remote } from "../../../Common/Remote/Remote";
import { Dialog, DialogActionsBar } from "@progress/kendo-react-dialogs";
import { Button } from "@progress/kendo-react-buttons";
import { TabStrip, TabStripContent, TabStripTab } from "@progress/kendo-react-layout";
import { KendoGrid } from "../../../Common/Grid/KendoGrid";
import { AutoComplete } from "../../../Common/AutoComplete/AutoComplete"
import { Guid } from "guid-typescript";
declare var window: any;
declare var $: any;

interface IGroupUsersProps {
    groupGuid: string;
    showSaveButton: any;
}

interface IGroupUsersState {
    groupName: string;
    selectedUser: any;
    groupDetail: any;
}

export class GroupUsers extends React.Component<IGroupUsersProps, IGroupUsersState>
{
    isComponentLoaded: boolean;
    grid: KendoGrid;
    autoComplete: AutoComplete;
    constructor(props: IGroupUsersProps) {
        super(props);
        this.state = {
            groupName: '',
            selectedUser: null,
            groupDetail: {}
        }
        this.loadGroup = this.loadGroup.bind(this);
        this.onAutoCompleteChange = this.onAutoCompleteChange.bind(this);
        this.onAssign = this.onAssign.bind(this);
    }

    componentWillMount() {
        this.isComponentLoaded = false;
    }

    async componentDidMount() {
        this.props.showSaveButton(false);
        this.isComponentLoaded = true;
        if (this.isComponentLoaded) {
            await this.loadGroup()
            await this.loadGroupDetails();
        }
    }

    async loadGroupDetails() {
        var response = await Remote.getAsync("/IAM/Group/GetGroup/" + this.props.groupGuid);
        if (response.ok) {
            var groupDetail = await response.json();
            this.setState({ groupDetail: groupDetail });
        }
        else {
            window.Dialog.alert("Error", response.statusText);
        }
    }

    async loadGroup() {
        var response1 = await Remote.getAsync("/IAM/Group/GetGroup/" + this.props.groupGuid);
        if (response1.ok) {
            let json = await response1.json();
            this.setState({ groupName: json.cn }, this.forceUpdate)
        }
    }

    onAutoCompleteChange(e: any) {
        this.setState({ selectedUser: e });
    }

    async onAssign(e: any) {
        if (this.state.selectedUser === null) {
            window.Dialog.alert("Please select a user", "Error");
            return;
        }

        let result = await Remote.postDataAsync("/iam/group/AssignToGroup/" + this.props.groupGuid, this.state.selectedUser);
        if (result.ok) {
            this.setState({ selectedUser: null }, () => { this.grid.refresh(), this.autoComplete.clearValue(), this.autoComplete.clearSelection(); })
        }
        else {
            let error = JSON.parse(await result.text());
            window.Dialog.alert(error[""].errors[0].errorMessage, "Error");
        }

    }


    removeGroup(grid: any, groupGuid: string) {
        let sender = this;
        grid.getSelectedItems((items: any[]) => {
            console.log(items)
         if (items.length > 0) {
             items[0].groupGuid = groupGuid;
        let deleteMassView = {
            text: "Delete Confirmation",
            getUrl: "/IAM/Group/DeleteUserBatch",
            postData: items,
            getMethod: 'post',
            postUrl: "/IAM/Group/DeleteUserBatchPost",
            method: 'post',
            buttons: [
                {
                    primary: true, requireValidation: true, text: "Yes", action: (e: any, o: any) => {
                        sender.grid.refresh();
                    }
                },
                { text: "No", action: (e: any, o: any) => { } }
            ],
            redirect: false,
            updateView: true,
            action: function (data: any) {
           
            }
        }
        grid.openSubmittableForm(deleteMassView);
    }
    else {
        window.Dialog.alert("Please select users first to Remove From Group.")
    }
});

 }
   
    render() {
        let rowMenu = [
            {
                text: "Unassign", icon: "delete", action: async (data: any, grid: any) => {
                    let data1 = data.dataItem;
                    data1.groupGuid = this.props.groupGuid;
                    let result = await Remote.postDataAsync("/iam/group/UnassignUser", data1)
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
            <div id="GroupUser" className="GroupUser gridparent row groups-manage-tab">
                <div className="col-12">
                    <div className="form-group row align-items-center">
                        
                        <h5 className="mb-3 mb-md-0 col">Assign User to Group</h5>
                        <div className="col-md-6 d-flex">
                            <div className="input-group">
                                <AutoComplete id="autoCUsers" ref={(c) => { this.autoComplete = c; }} dataUrl={"/iam/group/UnssignedUsers?groupGuid=" + this.props.groupGuid} className="form-control" onChange={this.onAutoCompleteChange} onClick={this.onAutoCompleteChange} displayField="name" placeHolder="Please type" valueField="value" allowMultiple={true} />
                                <div className="input-group-append">
                                    <button className="btn btn-primary" disabled={this.state.selectedUser === null} onClick={this.onAssign}>Assign</button>
                                </div>
                            </div>
                            <input type="button" className="ml-2 btn btn-secondary" onClick={(m) => this.removeGroup(this.grid, this.props.groupGuid)} value="Remove User"/>
                        </div>

                    </div>
                </div>
                <div className="col-12" id="GroupUserGrid">
                    <KendoGrid ref={(c) => { this.grid = c; }} dataURL={"/iam/group/AssignedUsers?id=" + this.props.groupGuid} fieldUrl="/GridFields/GroupUser" exportFieldUrl="/Export/GroupUser" gridHeight="150" identityField="groupUserGuid" showAddNewButton={false}  showColumnMenu={false} showSearchBox={false} showAdvanceSearchBox={false} showGridAction={false} parent="GroupUserGrid" />
            </div>
            </div>
        );
    }
}