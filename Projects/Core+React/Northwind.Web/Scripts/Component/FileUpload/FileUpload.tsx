import * as React from "react"
import * as ReactDOM from "react-dom"
import { Remote } from "../../Common/Remote/Remote"
import { Upload } from '@progress/kendo-react-upload';
import { FileUploadGrid } from "./FileUploadGrid"
import { Guid } from "guid-typescript";
import SpinnerPage from "./SpinnerPage";
import { NotificationToast } from "./NotificationToast";

declare var window: any;
declare var $: any;

interface IFileUploadProps {
    parentDomId: any;
    resourceId?: any;
    resourceKey: string;
    updatedBy?: string;
    updatedOn?: string;
    filesize?: string;
    filePath?: string;
    isRenderedFromExternalDialog?: boolean;  //rendered through mod dialog..
    isRenderedFromFileUploadDialog?: boolean; //rendered from same fileupload dialog..
    isAddPage?: boolean;
    isEditPage?: boolean;
    isDetailPage?: boolean;//base page
    isDialogDetailPage?: boolean;
    onCloseFileUploadDialog?: Function;
    isSubmitCallBack: boolean;
    onSubmitCallBack?: Function;
    isCalledFromNotice?: boolean;
    autoUpload?: boolean;
}

interface IFileUploadState {
    files: any[],
    events: any[],
    fileObject: any,
    filePreviews: {},
    uploadedFiles: any[];
    resetGridPageWhenFileAdd: boolean;
    showLoading: boolean;
    showNotificationToast: boolean;
    showFileUploadControl?: boolean;
    showFileUploadGrid?: boolean;
    autoUpload?: boolean;
}

export class FileUpload extends React.Component<IFileUploadProps, IFileUploadState> {
    uploader: Upload;
    static instance: FileUpload;
    fileUploadGrid: FileUploadGrid;
    constructor(props: any) {
        super(props);
        this.onAdd = this.onAdd.bind(this);
        this.onItemChange = this.onItemChange.bind(this);
        this.state = {
            files: [],
            events: [],
            fileObject: '',
            filePreviews: {},
            uploadedFiles: [],
            resetGridPageWhenFileAdd: false,
            showLoading: false,
            showNotificationToast: false,
            showFileUploadControl: false,
            showFileUploadGrid: false,
            autoUpload: this.props.autoUpload || true,
        };
        this.onFileRemove = this.onFileRemove.bind(this);
        this.onSubmitFiles = this.onSubmitFiles.bind(this);
        this.onRefresh = this.onRefresh.bind(this);
        FileUpload.instance = this;
    }

    onRefresh() {
        this.getAllFilesByContentResourceId();
    }

    renderNotificationToast() {
        return <NotificationToast messageType="Success" messageText="Succesfully submitted" />
    }

    render() {
        return (
            <div>

                <div className="d-flex justify-content-between align-items-center mb-3">
                    {
                        this.state.uploadedFiles.length === 0 &&
                        !this.props.isRenderedFromFileUploadDialog &&
                        !this.props.isAddPage &&
                        !this.props.isDialogDetailPage &&
                        <h5 className="text-muted">Please click the Add more Files button to upload file(s).</h5>
                    }
                    {this.props.isDetailPage && !this.props.isRenderedFromFileUploadDialog && !this.props.isRenderedFromExternalDialog &&
                        <button id="openFileUploadDialog" className="btn btn-primary">Add more file(s)</button>}
                </div>
                {
                    this.state.uploadedFiles.length === 0 &&
                    !this.props.isRenderedFromFileUploadDialog &&
                    this.props.isDialogDetailPage &&
                    this.props.isRenderedFromExternalDialog &&
                    <h5 className="text-muted mb-2">No files has been added. Please click  button to upload file(s).</h5>
                }
                {this.state.showLoading && <SpinnerPage />}

                {this.state.showFileUploadControl &&
                    <Upload
                        ref={(c) => { this.uploader = c; }}
                        autoUpload={this.state.autoUpload}
                        batch={false}
                        multiple={true}
                        defaultFiles={[]}
                        withCredentials={true}
                        onBeforeUpload={(e) => {
                            e.headers = {
                                'X-CSRF-TOKEN': Remote.getCookieValue('X-CSRF-TOKEN'),
                                'RequestVerificationToken': Remote.getCookieValue('RequestVerificationToken')
                            }
                        }}
                        onAdd={this.onAdd}
                        //saveUrl={'/ContractResourceFile/UploadSaveFile'}
                        //removeUrl={'/ContractResourceFile/DeleteUploadedFile'}
                        showFileList={false}
                    />
                }

                {this.state.uploadedFiles.length > 0 && <FileUploadGrid ref={(c) => { this.fileUploadGrid = c }}
                    resetGridPageWhenFileAdd={this.state.resetGridPageWhenFileAdd}
                    onChildItemChange={this.onItemChange}
                    selectedFileData={this.state.uploadedFiles}
                    parentDomId={this.props.parentDomId}
                    container={this}
                    isDetailPage={this.props.isDetailPage}
                    isDialogDetailPage={this.props.isDialogDetailPage}
                    isAddPage={this.props.isAddPage}
                    isEditPage={this.props.isEditPage}
                    isRenderedFromExternalDialog={this.props.isRenderedFromExternalDialog}
                    isRenderedFromFileUploadDialog={this.props.isRenderedFromFileUploadDialog}
                    onFileRemove={this.onFileRemove} />
                }

            </div>
        );
    }


    onItemChange(item: any, index: any, sender: any) {
        let arr = sender.state.uploadedFiles;
        arr[index] = item;
        sender.setState({
            uploadedFiles: arr
        })
    }

    getAllFilesByResourceId() {
        let sender = this;
        Remote.get("/ContractResourceFile/GetFilesByResourceId?resourceId=" + sender.props.resourceId,
            (response: any) => {
                sender.setState({ uploadedFiles: response })
            },
            (err: any) => { window.Dialog.alert(err) });
    }

    getAllFilesByContentResourceId() {
        let sender = this;
        Remote.get("/ContractResourceFile/GetFilesByContentResourceId?resourceId=" + sender.props.resourceId,
            (response: any) => {
                sender.setState({ uploadedFiles: response })
            },
            (err: any) => { window.Dialog.alert(err) });
    }

    componentDidMount() {
        //show fileupload control in   mods add/edit/detail page ..  isDetailPage is base page
        if (this.props.isRenderedFromExternalDialog && this.props.isDetailPage && (this.props.isAddPage || this.props.isEditPage)) {
            this.setState({
                showFileUploadControl: true
            })
        }
        //show fileupload control in  contract/project  add/edit/detail page ..
        else if (!this.props.isRenderedFromExternalDialog && !this.props.isDetailPage && (this.props.isAddPage || this.props.isEditPage)) {
            this.setState({
                showFileUploadControl: true
            })
        }
        //show fileupload control , if add/edit page of mods not 
        else if (this.props.isRenderedFromExternalDialog && !this.props.isDetailPage) {
            this.setState({
                showFileUploadControl: true
            })
        }
        else if (this.props.isRenderedFromFileUploadDialog) {
            this.setState({
                showFileUploadControl: true
            })
        }

        if (!this.props.isRenderedFromFileUploadDialog && !this.props.isCalledFromNotice) {
            this.getAllFilesByContentResourceId();
        }
        if (this.props.isCalledFromNotice) {
            this.getAllFilesByContentResourceId();
        }
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
            value.updatedBy = this.props.updatedBy || 'N/A',
                value.updatedOn = this.props.updatedOn || 'N/A',
                value.resourceId = this.props.resourceId,
                value.filePath = value.filePath || '',
                value.fileSize = this.props.filesize || (value.file.size / 1024 / 1024).toFixed(4) + ' MB'
        });

        //files retrived from database..
        var newAppendedarr = arr.concat(this.state.uploadedFiles);

        this.setState({ uploadedFiles: newAppendedarr, resetGridPageWhenFileAdd: true })

        //for local preview
        const afterStateChange = () => {
            event.affectedFiles
                .filter((file: any) => !file.validationErrors)
                .forEach((file: any) => {
                    const reader = new FileReader();

                    reader.onloadend = (ev) => {
                        this.setState({
                            filePreviews: {
                                ...this.state.filePreviews,
                                [file.uid]: ev.target
                            }
                        });
                    };

                    reader.readAsDataURL(file.getRawFile());
                });
        };

        this.setState({
            files: event.newState,
            events: [
                ...this.state.events,
            ]
        }, afterStateChange);
    }

    onFileRemove(fileData: any, index: any, parent: any) {
        //let sender = FileUpload.instance;
        let sender = parent;
        sender.setState({
            showLoading: true,
        })
        var file = this.state.uploadedFiles[index];

        if (!file.file) {
            Remote.postData("/ContractResourceFile/DeleteUploadedFile?ResourceId=" + file.contractResourceFileGuid + "&FilePath=" + fileData.filePath, '',
                (response: any) => {
                },
                (err: any) => { window.Dialog.alert(err) });
        }

        let data: any[] = sender.state.uploadedFiles;
        data.splice(index, 1);

        sender.setState({
            uploadedFiles: data,
            showLoading: false,
        }, sender.forceUpdate);
    }

    onSubmitFiles(resourceId: any, uploadPath: any, overRideProp: boolean, parentid?: any, contractResourceFileKey?: any, ContentResourceGuid?: any, callback?: any, redirectUrl?: any) {

        let sender = FileUpload.instance;
        //sender.setState({
        //    showLoading: true,
        //})
        let form = new FormData();
        if (sender.state.uploadedFiles.length == 0 && callback !== undefined) {
            $("#loadingFileUpload").hide();
            callback();
            return;
        }

        for (var index = 0; index < sender.state.uploadedFiles.length; index++) {
            var element = sender.state.uploadedFiles[index];
            if (element.file) {
                form.append(`file${index}`, element.file.getRawFile());
            }

            let description = encodeURIComponent(element.description || '');
            let concatedVal = element.contractResourceFileGuid + ',' + description + ',' + element.updatedOn + ',' + element.updatedBy + ',' + element.fileSize
            form.append("fileInfo", concatedVal);
        }
        // property resourceId is for the edit and detail module file attachment case  
        // onSubmitFiles resource Id for the new module attachment..
        if (overRideProp) {
            form.append("resourceId", resourceId || this.props.resourceId);
        }
        else {
            form.append("resourceId", this.props.resourceId || resourceId);
        }
        form.append("resourceKey", this.props.resourceKey);
        form.append("uploadPath", uploadPath);
        form.append("parentid", parentid);
        form.append("contractResourceFileKey", contractResourceFileKey);
        form.append("ContentResourceGuid", ContentResourceGuid);

        let owner = this;

        //if file upload is dialog..
        if (owner.props.isRenderedFromFileUploadDialog) {
            owner.setState({
                showLoading: true,
            })
        }

        $("#loadingFileUpload").show();

        Remote.postPlainFormData("/ContractResourceFile/UploadSaveFile", form,
            (response: any) => {
                if (response.ok) {

                    Remote.get("/ContractResourceFile/GetFilesByResourceId?resourceId=" + owner.props.resourceId,
                        (response: any) => {
                            $("#loadingFileUpload").hide();
                            if (callback !== undefined) {
                                callback();
                                return;
                            }

                            if (owner.props.isSubmitCallBack) {
                                if (contractResourceFileKey === 'ContractModification') {
                                    owner.props.onSubmitCallBack(ContentResourceGuid);
                                } else {
                                    owner.props.onSubmitCallBack(resourceId);
                                }
                            }

                            if (owner.props.isRenderedFromFileUploadDialog)  //when file upload dialog then only refresh file grid ..
                                window.uploader.onRefresh();

                            if (owner.props.isRenderedFromFileUploadDialog) {
                                owner.setState({
                                    showLoading: true,
                                })
                                owner.props.onCloseFileUploadDialog();
                            }
                            else {
                                $("#loadingFileUpload").hide();
                            }
                            if (redirectUrl !== undefined && redirectUrl !== '') {
                                window.location.href = redirectUrl;
                            }
                            //$("#loadingFileUpload").hide();
                        },
                        (err: any) => { window.Dialog.alert(err) });

                    //success message..
                } else if (response.status == 401) {
                    $("#loadingFileUpload").hide();
                    alert("Some error occurred ");
                }
            },
            (err: any) => { $("#loadingFileUpload").hide(); window.Dialog.alert(err) });
    }

    getFiles() {
        //return this.state.files;
        return this.fileUploadGrid.state.data;
    }
} 