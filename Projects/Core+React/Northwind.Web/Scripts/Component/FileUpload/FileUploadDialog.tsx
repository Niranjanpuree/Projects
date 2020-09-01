import * as React from "react";
import * as ReactDOM from "react-dom";
import { FileUpload } from "./FileUpload"
import { Dialog, DialogActionsBar } from "@progress/kendo-react-dialogs"
import { Button } from "@progress/kendo-react-buttons";
declare var window: any;
declare var document: any;

interface IFileUploadDialogProps
{
    parentDomId: any;
    resourceId?: any;
    resourceKey: string;
    updatedBy?: string;
    updatedOn?: string;
    filesize?: string;
    filePath?: string;
}

interface IFileUploadDialogStates
{
    showDialog: boolean;
}
export class FileUploadDialog extends React.Component<IFileUploadDialogProps, IFileUploadDialogStates> {
    FileUpload: FileUpload;

    constructor(props: any)
    {
        super(props);
        this.onSubmitFiles = this.onSubmitFiles.bind(this);
        this.onCancel = this.onCancel.bind(this);
        this.state = {
            showDialog: true,
        }
    }

    onSubmitFiles(e: any)
    {
        this.FileUpload.onSubmitFiles(this.props.resourceId, this.props.filePath,false);
    }

    onCancel(e: any)
    {
        this.setState({
            showDialog: false,
        });
    }

    render()
    {
        const { showDialog } = this.state;

        return (<div> {showDialog &&
            <Dialog title="File Upload" width="70%" height="auto" className="dialog-custom">
                <div>
                    <FileUpload
                    ref={(c) => { this.FileUpload = c }}
                    {...this.props} isSubmitCallBack={false} isRenderedFromFileUploadDialog={true} onCloseFileUploadDialog={this.onCancel}
                    />
                </div>
                <DialogActionsBar>
                    <Button primary={true} onClick={this.onSubmitFiles} type="button">Add</Button>
                <Button onClick={this.onCancel} type="button">Cancel</Button>
                </DialogActionsBar>
            </Dialog >}
        </div>);
    }
}