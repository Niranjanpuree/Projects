import * as React from "react";
import * as ReactDOM from "react-dom";
import { FolderTree } from "./FolderTree";

declare var window: any;
declare var $: any;

function loadFolderTree(fileUploadObj: any) {
    ReactDOM.unmountComponentAtNode(document.getElementById(fileUploadObj.domToRender));
    return ReactDOM.render(<FolderTree resourceType={fileUploadObj.resourceType}
        treeNodeUrl={fileUploadObj.treeNodeUrl}
        pathPrefixName={fileUploadObj.pathPrefixName}
        resourceId={fileUploadObj.resourceId}
        isTreeTemplate={fileUploadObj.isTreeTemplate}
        moduleType={fileUploadObj.moduleType}
        subResourceType={fileUploadObj.subResourceType}
        parentDomId={fileUploadObj.domToRender} />, document.getElementById(fileUploadObj.domToRender));
}

window.loadFolderTree = { loadFolderTreeNode: loadFolderTree };