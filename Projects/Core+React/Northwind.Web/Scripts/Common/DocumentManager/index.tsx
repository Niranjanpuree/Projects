import * as React from "react";
import * as ReactDOM from "react-dom";
import { DocumentManager } from "./DocumentManager";
import { Guid } from "guid-typescript";
declare var window: any;

export function loadDocumentManager(basePath: any, rootFolderName: string, resourceID: Guid, parent: string, otherParent: string, showSearchBox: boolean, hasManagePermission: boolean) {
    debugger
    let key = (new Date()).getTime();
    let resourcetype = 'Contract'
    if (otherParent !== "") {
        ReactDOM.unmountComponentAtNode(document.getElementById(otherParent))
    }
    ReactDOM.render(<DocumentManager key={parent + key} showSearchBox={showSearchBox} basePath={basePath} resourceType={resourcetype} rootFolderName={rootFolderName} apiFolderPath="/DocumentManager/GetFolderTree" apiFilePath="/DocumentManager/GetFilesAndFolders" uploadUrl={`/DocumentManager/Upload?resourcetype=${resourcetype}&id=`} operationConfirmationUrl="/DocumentManager/Operation" operationUrl="/DocumentManager/Operation" resourceId={resourceID} hasManagePermission={hasManagePermission} parent={document.getElementById(parent)} />, document.getElementById(parent));
}

export function loadDocumentManager1(basePath: any, rootFolderName: string, resourceID: Guid, parent: string, showSearchBox: boolean, hasManagePermission: boolean) {
    let key = (new Date()).getTime();
    let resourcetype = 'Contract'
    ReactDOM.render(<DocumentManager key={parent + key} showSearchBox={showSearchBox} basePath={basePath} resourceType={resourcetype} rootFolderName={rootFolderName} apiFolderPath="/DocumentManager/GetFolderTree" apiFilePath="/DocumentManager/GetFilesAndFolders" uploadUrl={`/DocumentManager/Upload?resourcetype=${resourcetype}&id=`} operationConfirmationUrl="/DocumentManager/Operation" operationUrl="/DocumentManager/Operation" resourceId={resourceID} hasManagePermission={hasManagePermission} parent={document.getElementById(parent)} />, document.getElementById(parent));
}

export function loadProjectDocumentManager(basePath: any, rootFolderName: string, resourceID: Guid, showSearchBox: boolean, hasManagePermission: boolean) {
    let resourcetype = 'Project'
    let key = (new Date()).getTime();
    ReactDOM.render(<DocumentManager key={"key" + key} showSearchBox={showSearchBox} basePath={basePath} resourceType={resourcetype} rootFolderName={rootFolderName} apiFolderPath="/DocumentManager/GetFolderTree" apiFilePath="/DocumentManager/GetFilesAndFolders" uploadUrl={`/DocumentManager/Upload?resourcetype=${resourcetype}&id=`} operationConfirmationUrl="/DocumentManager/Operation" operationUrl="/DocumentManager/Operation" resourceId={resourceID} hasManagePermission={hasManagePermission} parent={document.getElementById("projectDocumentManagement")} />, document.getElementById("projectDocumentManagement"));
}

window.admin = { documentManagement: { loadDocumentManager: loadDocumentManager, loadDocumentManager1: loadDocumentManager1, loadDocumentManagerProject: loadProjectDocumentManager } };