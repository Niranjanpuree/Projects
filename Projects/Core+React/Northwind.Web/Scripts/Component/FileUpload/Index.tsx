import * as React from "react";
import * as ReactDOM from "react-dom";
import { FileUpload } from "./FileUpload";
import { FileUploadDialog } from "./FileUploadDialog";
declare var window: any;
declare var document: any;


//if (document.getElementById("fileUpload")) {
//    ReactDOM.render(<FileUpload parentDomId="fileUpload" />, document.getElementById("fileUpload"));
//}

function loadFileUpload(domToRender: string,
    resourceKey: string,
    isSubmitCallBack: boolean,
    resourceId?: string,
    updatedBy?: string,
    updatedOn?: string,
    filePath?: string,
    isRenderedFromExternalDialog?: boolean,
    isAddPage?: boolean,
    isEditPage?: boolean,
    isDetailPage?: boolean,
    isDialogDetailPage?: boolean,
    onSubmitCallBack?: Function,
    isCalledFromNotice?: boolean,
    autoUpload?:boolean) {
    ReactDOM.unmountComponentAtNode(document.getElementById(domToRender));
    return ReactDOM.render(<FileUpload key={"uploader" + (new Date()).getTime()} parentDomId={domToRender}
        resourceId={resourceId}
        updatedBy={updatedBy}
        updatedOn={updatedOn}
        resourceKey={resourceKey}
        filePath={filePath}
        isRenderedFromExternalDialog={isRenderedFromExternalDialog}
        isAddPage={isAddPage}
        isEditPage={isEditPage}
        isDetailPage={isDetailPage}
        isDialogDetailPage={isDialogDetailPage}
        isSubmitCallBack={isSubmitCallBack}
        onSubmitCallBack={onSubmitCallBack}
        isCalledFromNotice={isCalledFromNotice}
        autoUpload={autoUpload}
    />, document.getElementById(domToRender));
}
window.loadFileUpload = { pageView: { loadFileUpload: loadFileUpload } };

function loadFileUploadDialog(domToRender: string, resourceKey: string, resourceId?: string, updatedBy?: string, updatedOn?: string, filePath?: string) {
    ReactDOM.unmountComponentAtNode(document.getElementById(domToRender));
    return ReactDOM.render(<FileUploadDialog key={"uploader" + (new Date()).getTime()} parentDomId={domToRender}
        resourceId={resourceId}
        updatedBy={updatedBy}
        updatedOn={updatedOn}
        resourceKey={resourceKey}
        filePath={filePath}
    />, document.getElementById(domToRender));
}
window.loadFileUploadDialog = { pageView: { loadFileUploadDialog: loadFileUploadDialog } };