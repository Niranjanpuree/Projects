import * as React from "react"
import { TreeView } from "@progress/kendo-react-treeview";
import { Remote } from "../../Common/Remote/Remote";
import { Upload } from "@progress/kendo-react-upload";
import { Guid } from "guid-typescript";
import { FileUploadGrid } from "./FileUploadGrid"
import { FileUploadDialog } from "./FileUploadDialog"

declare var window: any;
declare var $: any;

interface IFolderTreeProps {
    parentDomId: any;
    treeNodeUrl: string;
    resourceType: string;
    resourceId?: any;
    moduleType?: string;
    subResourceType?: string;
    isTreeTemplate: boolean;
    pathPrefixName: string;
}

interface IFolderTreeState {
    show: false,
    treeNodes: any[],
    selectedItem: any,
    recentlySelectedItem: any,
    returnUrl: string,
    uploadedFiles: any[];
    files: any[],
    showFileUploadGrid?: boolean;
    showFileUploadDialog?: boolean;
    resetGridPageWhenFileAdd?: boolean;
}

export class FolderTree extends React.Component<IFolderTreeProps, IFolderTreeState> {
    uploader: Upload;
    selectedItem: any = undefined;
    fileUploadGrid: FileUploadGrid;

    constructor(props: any) {
        super(props);

        this.onPathSelected = this.onPathSelected.bind(this);
        this.onItemChange = this.onItemChange.bind(this);
        this.onShowDialog = this.onShowDialog.bind(this);
        this.onCloseDialog = this.onCloseDialog.bind(this);
        this.onMouseOver = this.onMouseOver.bind(this);
        this.onMouseOut = this.onMouseOut.bind(this);

        this.state = {
            show: false,
            treeNodes: [],
            selectedItem: null,
            recentlySelectedItem: null,
            returnUrl: '',
            uploadedFiles: [],
            files: [],
            showFileUploadGrid: false,
            showFileUploadDialog: false,
            resetGridPageWhenFileAdd: false,
        };
    }

    componentDidMount() {
        this.onInit();
    }

    deepMap(arr: any, sender: any) {
        let a: any[] = [];
        if (arr == undefined)
            return a;
        if (isNaN(arr['length'])) {
            let e: any[] = [];
            e.push(arr);
            arr = e;
        }
        arr.map((d: any, i: number) => {
            var selectedItemId = (this.state.recentlySelectedItem !== null) ? this.state.recentlySelectedItem.id : Guid.EMPTY;
            var selectedItemParentId = (this.state.recentlySelectedItem !== null) ? this.state.recentlySelectedItem.parentId : Guid.EMPTY;
            var id = d.folderStructureFolderGuid || d.contractResourceFileGuid;

            var isOpened = (d.parentId === Guid.EMPTY) ? true : (d.parentId == selectedItemParentId) ? true : (id === selectedItemId) ? true : (id === selectedItemParentId) ? true : false;
                a.push({
                    text: d.name || d.uploadFileName,
                    id: id,
                    parentId: d.parentId,
                    icon: d.isFile ? 'file' : 'folder',
                    relativePath: d.filePath,
                    description: d.description,
                    type: d.type,
                    expanded: true,
                    opened: isOpened,
                    //fileID: d.contractResourceFileGuid,
                    isFile: d.isFile,
                    items: sender.deepMap(d.children || d.subfolders, sender),
                    readonly: d.isReadOnly
                });
        });
        return a;
    }

    onInit() {
        let sender = this;

        if (this.props.isTreeTemplate) {
            Remote.get(this.props.treeNodeUrl, (data: any[]) => {
                let collect: any[] = this.deepMap(data, sender);
                sender.setState({ treeNodes: collect });
            }, (data: any) => { window.Dialog.alert(data); })
        } else {
            Remote.get("/DocumentManager/GetFilesAndFolderTree?id=" + this.props.pathPrefixName + "&resourceType=" + this.props.resourceType + "&resourceId=" + this.props.resourceId, (data: any[]) => {
                let collect: any[] = this.deepMap(data, sender);
                sender.setState({ treeNodes: collect });
            }, (data: any) => { window.Dialog.alert(data); })
        }
    }

    refreshTreeNode() {
        this.onInit();
    }

    onItemClick = (event: any) => {
        if (this.selectedItem) {
            this.selectedItem.selected = false;
        }
        event.item.selected = true;
        this.selectedItem = event.item;
        this.setState({ selectedItem: event.item });
        this.forceUpdate();
    }

    onExpandChange = (event: any) => {
        event.item.opened = !event.item.opened;
        this.forceUpdate();
    }

    onPathSelected(): boolean {
        if (this.state.selectedItem === null) {
            window.Dialog.alert("Please select a folder to upload file");
            return false;
        }
        return true;
    }

    onItemChange(item: any, index: any, sender: any) {
        let arr = sender.state.uploadedFiles;
        arr[index] = item;
        sender.setState({
            uploadedFiles: arr
        })
    }

    onShowDialog() {
        if (this.state.selectedItem === null) {
            window.Dialog.alert("Please select a folder to upload the file(s)");
            return false;
        }
        else {
            this.setState({ showFileUploadDialog: true })
        }
    }

    onCloseDialog(recentlySelectedItem: any) {
        this.setState({ showFileUploadDialog: false, selectedItem: null, recentlySelectedItem: recentlySelectedItem })
        this.refreshTreeNode();
    }

    onMouseOver(item: any) {
        if (item.isFile) {
            var htmldetail = "<div class='popover-detail bottom active'>";
            htmldetail += "<div class='popover-detail-container'>";
            htmldetail += "<div><label>File Name :</label>" + item.text + "</div>";
            htmldetail += "<div><label>File Description :</label>" + item.description + "</div>";
            htmldetail += "</div>";
            htmldetail += "</div>";
            var className = item.id;
            $('.' + className).html(htmldetail);
        }
    }

    onMouseOut(item: any) {
        if (item.isFile) {
            $(".files").find(".active").removeClass("active");
        }
    }

    render() {
        return (
            <div>
                <div className="alert alert-secondary">
                    <h6 className="text-center mb-0">Please select destination folder from left pane to upload the file(s)</h6>
                </div>
                <div className="col-12">
                <div className="row">
                    <div className="pane-content col-sm-4">
                        <TreeView
                            data={this.state.treeNodes}
                            expandIcons={true}
                            onExpandChange={this.onExpandChange}
                            onItemClick={this.onItemClick}
                            expandField="opened"
                            itemRender={(props) => {
                                return (<div itemID={props.item.relativePath}>
                                    <span className={`k-icon k-i-${props.item.icon} icon-font`} key='0'></span>
                                    <span className={props.item.text} onMouseOver={() => { this.onMouseOver(props.item) }} onMouseOut={() => { this.onMouseOut(props.item) }} title={props.item.description}>{props.item.text} </span>
                                    <div className={`files ${props.item.id}`}></div>
                                </div>)
                            }
                            }
                        />

                    </div>
                    <div className="col-sm-8">
                        <div className="text-right">
                            <button className="btn btn-primary" onClick={this.onShowDialog}>Add File(s)</button>
                        </div>
                        {this.state.showFileUploadDialog && <FileUploadDialog {...this.props} resourceKey={this.props.resourceType} selectedFolderItem={this.state.selectedItem} onCloseDialog={this.onCloseDialog} />}
                    </div>
                </div>
                </div>
            </div>
        );
    }
} 