import * as React from "react";
import { Remote } from "../../../Common/Remote/Remote";
import { KendoGrid } from "../../../Common/Grid/KendoGrid";
import { AutoComplete } from "../../../Common/AutoComplete/AutoComplete"
declare var window: any;
declare var $: any;

interface IUsersGroupProps {
    userGuid: string;
    userName : string;
}

interface IUsersGroupState {
    groupName: string;
    selectedGroup: any;
    groupDetail: any;
}

export class UsersGroup extends React.Component<IUsersGroupProps, IUsersGroupState>
{
    isComponentLoaded: boolean;
    grid: KendoGrid;
    autoComplete: AutoComplete;
    constructor(props: IUsersGroupProps) {
        super(props);
        this.state = {
            groupName: '',
            selectedGroup: null,
            groupDetail: {}
        }
        //this.loadGroup = this.loadGroup.bind(this);
        this.onAutoCompleteChange = this.onAutoCompleteChange.bind(this);
        this.onAssign = this.onAssign.bind(this);
    }

    componentWillMount() {
        this.isComponentLoaded = false;
    }

    async componentDidMount() {
       
        this.isComponentLoaded = true;
        if (this.isComponentLoaded) {
            //await this.loadGroup()
            //await this.loadGroupDetails();
        }
    }

    //async loadGroupDetails() {
    //    var response = await Remote.getAsync("/IAM/Group/GetGroup/" + this.props.userGuid);
    //    if (response.ok) {
    //        var groupDetail = await response.json();
    //        this.setState({ groupDetail: groupDetail });
    //    }
    //    else {
    //        window.Dialog.alert("Error", response.statusText);
    //    }
    //}

    //async loadGroup() {
    //    var response1 = await Remote.getAsync("/IAM/Group/GetGroup/" + this.props.userGuid);
    //    if (response1.ok) {
    //        let json = await response1.json();
    //        this.setState({ groupName: json.cn }, this.forceUpdate)
    //    }
    //}

    onAutoCompleteChange(e: any) {
        this.setState({ selectedGroup: e });

    }

    async onAssign(e: any) {
        console.log(this.state.selectedGroup);
        if (this.state.selectedGroup === null) {
            window.Dialog.alert("Please select a Group", "Error");
            return;
        }

        let result = await Remote.postDataAsync("/iam/user/AssignToGroup/" + this.props.userGuid, this.state.selectedGroup);
        if (result.ok) {
            this.setState({ selectedGroup: null }, () => { this.grid.refresh(), this.autoComplete.clearValue(), this.autoComplete.clearSelection(); })
        }
        else {
            let error = JSON.parse(await result.text());
            window.Dialog.alert(error[""].errors[0].errorMessage, "Error");
        }

    }


    removeGroup(grid: any, userGuid: string) {
        let sender = this;
     grid.getSelectedItems((items: any[]) => {
         if (items.length > 0) {
             items[0].userGuid = userGuid;
        let deleteMassView = {
            text: "Delete Confirmation",
            getUrl: "/IAM/User/DeleteGroupBatch",
            postData: items,
            getMethod: 'post',
            postUrl: "/IAM/User/DeleteGroupBatchPost",
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
        window.Dialog.alert("Please select groups first to unassign for user.")
    }
});

 }
   
    render() {
        let rowMenu = [
            {
                text: "Unassign", icon: "delete", action: async (data: any, grid: any) => {
                    let data1 = data.dataItem;
                    data1.groupGuid = this.props.userGuid;
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
            <div id="GroupUser" className="GroupUser gridparent row">
              
                <div className="col-12">
                    <div className="form-group row align-items-center">
                        <p className="col-12">Following are the groups that {this.props.userName} belongs to. To assign {this.props.userName} to new groups please start typing the group name in the box below. You can type in multiple groups and then click on Assign</p>
                        <div className="col-sm-7 mb-3 mb-sm-0">
                            <div className="input-group">
                                <AutoComplete id="autoCUsers" ref={(c) => { this.autoComplete = c; }} dataUrl={"/iam/user/UnassignedGroups?userGuid=" + this.props.userGuid} className="form-control" onChange={this.onAutoCompleteChange} onClick={this.onAutoCompleteChange} displayField="cn" placeHolder="Please type" valueField="value" allowMultiple={true} />
                                <div className="input-group-append">
                                    <button className="btn btn-primary" disabled={this.state.selectedGroup === null} onClick={this.onAssign}>Assign</button>
                                </div>
                            </div>
                        </div>
                        <div className="col text-right"><input type="button" className="ml-2 btn btn-secondary" onClick={(m) => this.removeGroup(this.grid, this.props.userGuid)} value="Remove Group" /></div>
                        
                      
                    </div>
                </div>
                <div className="col-12" id="GroupUserGrid">
                    <KendoGrid ref={(c) => { this.grid = c; }} dataURL={"/iam/user/AssignedGroups/" + this.props.userGuid} fieldUrl="/GridFields/UserGroup" exportFieldUrl="/Export/GroupUser" identityField="groupUserGuid" showAddNewButton={false} showColumnMenu={false} showSearchBox={false} showAdvanceSearchBox={false}  showGridAction={false} parent="GroupUserGrid" />
                </div>
            </div>
        );
    }
}