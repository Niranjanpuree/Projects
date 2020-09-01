import * as React from "react";
import * as ReactDOM from "react-dom";
import { FolderTree } from "./FolderTree"
import { Dialog, DialogActionsBar } from "@progress/kendo-react-dialogs"
import { Button } from "@progress/kendo-react-buttons";
import { Upload } from "@progress/kendo-react-upload";
import { FileUploadGrid } from "../FolderTree/FileUploadGrid";
import { Guid } from "guid-typescript";
import { Remote } from "../../Common/Remote/Remote";
declare var window: any;
declare var document: any;

interface IFileUploadDialogProps {
    parentDomId: any;
    resourceId?: any;
    resourceKey: string;
    updatedBy?: string;
    updatedOn?: string;
    filesize?: string;
    filePath?: string;
    resourceType: string;
    isTreeTemplate: boolean;
    pathPrefixName: string;
    selectedFolderItem: any;
    onCloseDialog: Function;
}

interface IFileUploadDialogStates {
    uploadedFiles?: any[];
    resetGridPageWhenFileAdd?: boolean;
    showFileUploadDialog?: boolean;
}
export class FileUploadDialog extends React.Component<IFileUploadDialogProps, IFileUploadDialogStates> {
    folderTree: FolderTree;
    uploader: Upload;
    fileUploadGrid: FileUploadGrid;

    constructor(props: any) {
        super(props);
        this.onAdd = this.onAdd.bind(this);
        this.onSubmitFiles = this.onSubmitFiles.bind(this);
        this.onFileRemove = this.onFileRemove.bind(this);
        this.onCancel = this.onCancel.bind(this);
        this.onItemChange = this.onItemChange.bind(this);
        this.state = {
            uploadedFiles: [],
            resetGridPageWhenFileAdd: false,
            showFileUploadDialog: false,
        }
    }

    onCancel(e: any) {
        this.props.onCloseDialog(null);
    }

    onAdd = (event: any) => {
        //recently added files ...
        let mapFileObject = event.affectedFiles.map((x: any) => {
            let id = Guid.create();
            return {
                contractResourceFileGuid: id.toString(),
                uploadFileName: x.name,
                file: x,
            };
        })

        let arr: any[] = [];
        arr = arr.concat(mapFileObject);
        arr.forEach((value: any, index: any) => {
            value.resourceId = this.props.resourceId,
                value.filePath = value.filePath || '',
                value.masterFolderGuid = this.props.selectedFolderItem !== null ? this.props.selectedFolderItem.id : ''
        });

        //files retrived from database..
        var newAppendedarr = arr.concat(this.state.uploadedFiles);
        this.setState({ uploadedFiles: newAppendedarr, resetGridPageWhenFileAdd: true })
    }

    onItemChange(item: any, index: any, sender: any) {
        let arr = sender.state.uploadedFiles;
        arr[index] = item;
        sender.setState({
            uploadedFiles: arr
        })
    }

    onFileRemove(fileData: any, index: any, parent: any) {
        let sender = parent;
        sender.setState({
            showLoading: true,
        })
        var file = this.state.uploadedFiles[index];

        let data: any[] = sender.state.uploadedFiles;
        data.splice(index, 1);

        sender.setState({
            uploadedFiles: data,
            showLoading: false,
        }, sender.forceUpdate);
    }

    onSubmitFiles(resourceId: any, pathPrefixName: string) {

        if (this.state.uploadedFiles.length === 0) {
            window.Dialog.alert("Please select a file to upload");
            return false;
        }

        let sender = this;
        let form = new FormData();

        for (var index = 0; index < sender.state.uploadedFiles.length; index++) {
            var element = sender.state.uploadedFiles[index];
            if (element.file) {
                form.append(`file${index}`, element.file.getRawFile());
            }

            let description = encodeURIComponent(element.description || '');
            let concatedVal = element.contractResourceFileGuid + ',' +
                description + ',' +
                element.updatedOn + ',' +
                element.updatedBy + ',' +
                element.fileSize + ',' +
                this.props.selectedFolderItem.id  //master folder id..
            form.append("fileInfo", concatedVal);
        }

        // property resourceId is for the edit and detail module file attachment case  
        // onSubmitFiles resource Id for the new module attachment..
        form.append("resourceId", this.props.resourceId || resourceId);
        form.append("resourceKey", this.props.resourceType);
        form.append("pathPrefixName", pathPrefixName);
        form.append("isTreeTemplate", this.props.isTreeTemplate ? "true" : "false");

        let owner = this;

        Remote.postPlainFormData("/ContractResourceFile/UploadSaveFileInFolderTreeNode", form,
            (response: any) => {
                if (response.ok) {
                    //close dialog after fileupload process end and refresh tree nodes....
                    this.props.onCloseDialog(this.props.selectedFolderItem);

                } else if (response.status == 401) {
                    alert("Some error occurred ");
                }
            },
            (err: any) => { window.Dialog.alert(err) });
    }

    render() {
        return (<div>
            <Dialog title="File Upload" width="70%" height="auto" className="dialog-custom">
                <div className="col-sm-12">
                    <Upload
                        ref={(c) => { this.uploader = c; }}
                        autoUpload={false}
                        batch={false}
                        multiple={true}
                        defaultFiles={[]}
                        withCredentials={true}
                        onAdd={this.onAdd}
                        onBeforeUpload={(e) => {
                            e.headers = {
                                'X-CSRF-TOKEN': Remote.getCookieValue('X-CSRF-TOKEN'),
                                'RequestVerificationToken': Remote.getCookieValue('RequestVerificationToken')
                            }
                        }}
                        //onRemove={this.onRemove}
                        //saveUrl={'/ContractResourceFile/UploadSaveFileInFolderTreeNode'}
                        //removeUrl={'/ContractResourceFile/DeleteUploadedFile'}
                        showFileList={false}
                        showActionButtons={false}
                    />

                    {this.state.uploadedFiles.length > 0 && <FileUploadGrid ref={(c) => { this.fileUploadGrid = c }}
                        resetGridPageWhenFileAdd={this.state.resetGridPageWhenFileAdd}
                        onChildItemChange={this.onItemChange}
                        selectedFileData={this.state.uploadedFiles}
                        parentDomId={this.props.parentDomId}
                        container={this}
                        onFileRemove={this.onFileRemove} />
                    }
                </div>
                <DialogActionsBar>
                    <Button primary={true} onClick={() => this.onSubmitFiles(this.props.resourceId, this.props.pathPrefixName)} type="button">Add</Button>
                    <Button onClick={this.onCancel} type="button">Cancel</Button>
                </DialogActionsBar>
            </Dialog >}
        </div>);
    }
}