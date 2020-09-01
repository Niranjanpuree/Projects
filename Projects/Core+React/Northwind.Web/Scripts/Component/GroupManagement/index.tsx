import 'react-app-polyfill/ie11';
import * as React from "react";
import * as ReactDOM from "react-dom";
import { GroupManagement } from "./GroupManagement";
import { UserManagement } from './userManagement';
declare var window: any;

function loadGroupDetails(groupGuid: string, id: string, showSaveButton: any)
{
    return ReactDOM.render(<GroupManagement groupGuid={groupGuid} showSaveButton={showSaveButton} />, document.getElementById(id));
}
function loadUserDetails(groupGuid: string, id: string, userName: string) {
    return ReactDOM.render(<UserManagement userGuid={groupGuid} userName={userName}/>, document.getElementById(id));
}

window.groupManagement = {
    loadGroupDetails: loadGroupDetails
}

if (window.iam && window.iam.usergroup)
    window.iam.usergroup = { loadGroupDetails: loadGroupDetails, loadUserDetails: loadUserDetails }
else
    window.iam = { ...window.iam, usergroup: { loadGroupDetails: loadGroupDetails, loadUserDetails: loadUserDetails } }