
import * as React from "react";
import * as ReactDOM from "react-dom";
import { MultiSelectPanel } from "./MultiSelectPanel";
import { UserPermission } from "../../component/groupmanagement/tabs/userpermission";
import {UsersGroup } from "../../component/groupmanagement/tabs/usersgroup"
// import '@progress/kendo-theme-bootstrap/dist/all.css';
declare var window: any;

export default function loadUserGroup(userGuid: any) {
    //ReactDOM.render(<UsersGroup userGuid={userGuid}  />, document.getElementById("userGroupSelection"));
}

export function loadGroupUser(groupGuid: any) {
    let rowMenu = [{
        text: "Unassign", icon: "delete", action: (data: any, grid: any) => {
            
            let deleteView = {
                text: "Unassign Confirmation",
                getUrl: "/IAM/Group/UnassignUser/" + groupGuid + "/" + data.dataItem.userGuid,
                postUrl: "/IAM/Group/UnassignUser",
                method: 'post',
                buttons: [
                    { primary: true, requireValidation: true, text: "Yes", action: (e: any, o: any) => {  } },
                    { text: "No", action: (e: any, o: any) => {  } }
                ],
                redirect: false,
                updateView: true,
                action: function (data: any) {

                }
            }
            grid.openSubmittableForm(deleteView);
        }
    }];
    ReactDOM.render(<MultiSelectPanel searchUrl={"/iam/user/Get?take=10&skip=0&sortField=firstname&dir=asc&searchValue="} gridDataUrl={"/iam/group/assignedUsers/" + groupGuid} postUrl={"/iam/group/assigntogroup/" + groupGuid} gridDataFieldUrl="/IAM/User/GridviewFields" searchField="name" gridIdentityField="groupUserGUID" rowMenus={rowMenu} gridWidth={document.getElementById("groupUserSelection").clientWidth} parent={document.getElementById("groupUserSelection")} gridHeight="262px" />, document.getElementById("groupUserSelection"));
}

export function loadUserPermision(domToRender: any, userGuid: any) {
    ReactDOM.unmountComponentAtNode(document.getElementById(domToRender));
    //ReactDOM.render(<UserPermission groupGuid={userGuid} />, document.getElementById("userPermission"));
}

if (window.iam && window.iam.usergroup) {
    window.iam.usergroup = { ...window.iam.usergroup, loadUserGroup: loadUserGroup, loadGroupUser: loadGroupUser, loadUserPermision: loadUserPermision }
}
else
    window.iam = { ...window.iam, usergroup: { loadUserGroup: loadUserGroup, loadGroupUser: loadGroupUser, loadUserPermision: loadUserPermision }};