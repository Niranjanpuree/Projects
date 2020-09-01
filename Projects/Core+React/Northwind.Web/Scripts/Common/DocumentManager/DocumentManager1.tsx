import * as React from "react"
import * as ReactDOM from "react-dom"
import { Remote } from "../Remote/Remote"
import { TreeView } from '@progress/kendo-react-treeview';
import { Splitter } from '@progress/kendo-react-layout';
import { ContextMenu } from "../ContextMenu/ContextMenu";
import { KendoDialog } from "../Dialog/Dialog";
import { Upload, UploadOnStatusChangeEvent, UploadFileStatus } from '@progress/kendo-react-upload';
import { Input } from "@progress/kendo-react-inputs";
import { Button } from "@progress/kendo-react-buttons";
import { Guid } from "guid-typescript";
import { Grid, GridColumn as Column } from '@progress/kendo-react-grid';
import { FileCommandCell } from './style';
declare var window: any;
declare var $: any;
declare var formatUSDatetime: any;

interface IDocumentManagerProps {
    rootFolderName: string,
    parent: any;
    basePath: string;
    apiFolderPath: string;
    apiFilePath: string;
    uploadUrl: string;
    operationConfirmationUrl?: string;
    operationUrl?: string;
    resourceId: Guid;
    resourceType: string;
    showSearchBox: boolean;
    hasManagePermission: boolean;
}

interface IDocumentManagerState {
    treeNodes: any[];
    files: any[];
    panes: any[];
    contextMenu: any;
    copiedElement: any;
    cutElement: any;
    paneContextPath: string;
    contextPath: string;
    dialogProps: any;
    showCreateFolderDialog: boolean;
    showProgressBar: boolean;
    uploadingFiles: any[];
    contextMenuTopMargin: number,
    contextMenuLeftMargin: number,
    fileId: Guid,
    viewtype: string;
    buttontext: string;
    fileData: any;
    selectedItem: any;
    expandedItems: any[];
}

export class DocumentManager1 extends React.Component<IDocumentManagerProps, IDocumentManagerState> {
    static instance: DocumentManager1;
    dialog: any;
    treeNodeParents: any[];
    uploader: Upload;
    searchField: Input;
    uploaderDragLeave: boolean;
    uploader1: Upload;

    constructor(props: any) {
        super(props);
        this.onLayoutChange = this.onLayoutChange.bind(this);
        this.onInit = this.onInit.bind(this);
        this.deepMap = this.deepMap.bind(this);
        this.onTreeItemClick = this.onTreeItemClick.bind(this);
        this.folderAndFileMap = this.folderAndFileMap.bind(this);
        this.iconClassName = this.iconClassName.bind(this);
        this.onContextMenu = this.onContextMenu.bind(this);
        this.renderContextMenu = this.renderContextMenu.bind(this);
        this.onCopy = this.onCopy.bind(this);
        this.onContextMenuMouseOut = this.onContextMenuMouseOut.bind(this);
        this.handleFileContextMenu = this.handleFileContextMenu.bind(this);
        this.onPaste = this.onPaste.bind(this);
        this.onDelete = this.onDelete.bind(this);
        this.onRename = this.onRename.bind(this);
        this.handlePaneContextMenu = this.handlePaneContextMenu.bind(this);
        this.registerDialog = this.registerDialog.bind(this);
        this.onCreateFolder = this.onCreateFolder.bind(this);
        this.loadFileOnPane = this.loadFileOnPane.bind(this);
        this.onDeleteFolder = this.onDeleteFolder.bind(this);
        this.onRenameFolder = this.onRenameFolder.bind(this);
        this.onTreeContextMenu = this.onTreeContextMenu.bind(this);
        this.onFolderDoubleClick = this.onFolderDoubleClick.bind(this);
        this.handleFileDoubleClick = this.handleFileDoubleClick.bind(this);
        this.handleFolderDoubleClick = this.handleFolderDoubleClick.bind(this);
        this.deepScan = this.deepScan.bind(this);
        this.expandAllParents = this.expandAllParents.bind(this);
        this.deepSearch = this.deepSearch.bind(this);
        this.onDragging = this.onDragging.bind(this);
        this.onDraggingComplete = this.onDraggingComplete.bind(this);
        this.onCut = this.onCut.bind(this);
        this.onDeleteFile = this.onDeleteFile.bind(this);
        this.onRenameFile = this.onRenameFile.bind(this);
        this.onDocumentSearch = this.onDocumentSearch.bind(this);
        this.onDocumentSearchPressed = this.onDocumentSearchPressed.bind(this);
        this.onDownloadDocument = this.onDownloadDocument.bind(this);
        this.onUploadProgress = this.onUploadProgress.bind(this);
        this.onUploadFileAdded = this.onUploadFileAdded.bind(this);
        this.onCreateFolderButtonClick = this.onCreateFolderButtonClick.bind(this);
        this.urlencode = this.urlencode.bind(this);
        this.onDownloadDocumentClick = this.onDownloadDocumentClick.bind(this);
        this.onBreadcrumClick = this.onBreadcrumClick.bind(this);
        this.rowRender = this.rowRender.bind(this);
        this.onGridRowClick = this.onGridRowClick.bind(this);
        this.onChangeViewButtonClick = this.onChangeViewButtonClick.bind(this);
        this.onBeforeUpload = this.onBeforeUpload.bind(this);
        this.onGridViewContextMenu = this.onGridViewContextMenu.bind(this);
        this.onFileClicked = this.onFileClicked.bind(this);
        this.onFolderClicked = this.onFolderClicked.bind(this);
        this.isExpandedItem = this.isExpandedItem.bind(this);
        this.onExpandChange = this.onExpandChange.bind(this);
        this.onExpandAll = this.onExpandAll.bind(this);
        this.onCollapseAll = this.onCollapseAll.bind(this);


        DocumentManager1.instance = this;

        let dialogProps = {
            actionText: '',
            dialogTitle: '',
            buttons: [{}],
            dialogHeight: '50%',
            dialogWidth: '50%',
            getUrl: '',
            method: '',
            postUrl: '',
            uniqueKey: '',
            visible: false,
            register: this.registerDialog
        }

        this.state = {
            treeNodes: [],
            files: [],
            panes: [
                { size: '30%', resizable: true },
                { resizable: true }
            ],
            contextMenu: null,
            copiedElement: null,
            paneContextPath: this.props.basePath,
            contextPath: this.props.basePath,
            showCreateFolderDialog: false,
            dialogProps: dialogProps,
            showProgressBar: true,
            cutElement: null,
            uploadingFiles: [],
            contextMenuLeftMargin: 25,
            contextMenuTopMargin: 300,
            fileId: null,
            viewtype: 'list',
            buttontext: 'Details',
            fileData: [],
            selectedItem: null,
            expandedItems: []
        };

        this.onInit();
    }

    onInit() {
        let sender = this;
        Remote.get(this.props.apiFolderPath + "?id=" + encodeURIComponent(this.props.basePath) + "&resourceType=" + this.props.resourceType + "&resourceId=" + this.props.resourceId, (data: any[]) => {
            let collect: any[] = this.deepMap(data, sender);
            let withRoot: any[] = [];
            withRoot.push({
                text: this.props.rootFolderName,
                icon: 'folder',
                relativePath: this.props.basePath,
                type: "folder",
                expanded: true,
                items: collect,
            });
            let selectedPath = this.state.paneContextPath || this.props.basePath;
            let fileId = collect.length > 0 ? collect[0].fileID : null;
            if (this.state.fileId != null) {
                fileId = this.state.fileId;
            }
            sender.setState({ treeNodes: collect, paneContextPath: selectedPath, fileId: fileId }, sender.loadFileOnPane);
        }, (data: any) => { window.Dialog.alert(data); })
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
            a.push({
                text: d.uploadFileName,
                description: d.description,
                parentId: d.parentId,
                icon: 'folder',
                relativePath: d.filePath,
                type: d.type,
                expanded: false,
                fileID: d.contractResourceFileGuid,
                isFile: d.isFile,
                items: sender.deepMap(d.subfolders, sender),
                readonly: d.isReadOnly
            });
        });
        return a;
    }

    replace(path: string, find: string, replace: string) {
        let p = path.toLowerCase();
        while (p.indexOf("\\") >= 0) {
            p = p.replace("\\", "/");
        }
        return p;
    }

    deepScan(arr: any[], relativePath: any, parent: any) {
        arr.map((item: any, index: number) => {
            item.parent = parent;
            if (this.replace(item.relativePath, "\\", "/") == this.replace(relativePath, "\\", "/")) {
                item.expanded = true;
                item.selected = true;
                this.expandAllParents(item);
            }
            else if (this.isExpandedItem(item) >= 0) {
                item.expanded = true;
            }
            else {
                item.selected = false;
                item.expanded = false;
            }
            this.deepScan(item.items, relativePath, item);
        })
    }

    isExpandedItem(item: any) {
        let index = this.state.expandedItems.findIndex((v: any, i: number) => {
            if (item.fileID === v.fileID) {
                return v;
            }
            return null;
        })
        return index;
    }

    deepSearch(arr: any[], relativePath: string): any {
        for (let i in arr) {
            if (this.replace(arr[i].relativePath, "\\", "/") === this.replace(relativePath, "\\", "/"))
                return arr[i];
            else {
                let item = this.deepSearch(arr[i].items, relativePath);
                if (item != undefined) {
                    return item;
                }
            }
        }

    }

    expandAllParents(item: any) {
        if (item.parent != null) {
            item.parent.expanded = true;
            this.expandAllParents(item.parent);
        }
    }

    folderAndFileMap(arr: any[], sender: any) {
        let a: any[] = [];
        if (arr == undefined)
            return a;
        arr.map((d: any, i: number) => {
            a.push({
                text: d.name,
                parentId: d.parentId,
                description: d.description,
                icon: sender.getIcon(d),
                relativePath: d.relativePath,
                type: d.type,
                expanded: false,
                items: [],
                readonly: d.isReadOnly
            });
        });
    }

    onLayoutChange(e: any) {
        this.setState({
            panes: e
        });
    }

    onTreeItemClick(e: any) {
        e.item.expanded = !e.item.expanded;
        let expanded: any[] = this.state.expandedItems;

        if (e.item.expanded) {
            expanded.push(e.item);
            this.setState({ paneContextPath: e.item.relativePath, fileId: e.item.fileID, selectedItem: e.item, expandedItems: expanded, showProgressBar: true }, this.loadFileOnPane)
        }
        else {
            expanded.splice(expanded.indexOf(e.item), 1);
            this.setState({ paneContextPath: e.item.relativePath, fileId: e.item.fileID, selectedItem: e.item, expandedItems: expanded, showProgressBar: true }, this.loadFileOnPane);
        }
    }

    onExpandChange(e: any) {
        e.item.expanded = !e.item.expanded;
        let expanded: any[] = this.state.expandedItems;

        if (e.item.expanded) {
            expanded.push(e.item);
            this.setState({ paneContextPath: e.item.relativePath, fileId: e.item.fileID, selectedItem: e.item, expandedItems: expanded, showProgressBar: true }, this.loadFileOnPane)
        }
        else if (expanded[expanded.length - 1]) {
            let items = this.state.treeNodes;
            let cItem = this.deepSearch(items, e.item.relativePath);

            expanded.splice(expanded.indexOf(e.item), 1);
            let item = expanded[expanded.length - 1] || this.state.treeNodes[0];

            this.__expandCollapseAllNodes(cItem.items, false);
            if (cItem) {
                item = cItem;
            }
            this.setState({ treeNodes: items, paneContextPath: expanded[expanded.length - 1].relativePath, fileId: expanded[expanded.length - 1].fileID, selectedItem: expanded[expanded.length - 1], expandedItems: expanded, showProgressBar: true }, this.loadFileOnPane);
        }
    }

    loadFileOnPane() {
        let sender = this;
        Remote.get(sender.props.apiFilePath + "?id=" + encodeURIComponent(sender.state.paneContextPath) + "&resourceType=" + sender.props.resourceType + "&fileId=" + sender.state.fileId, (data: any[]) => {
            let collect: any[] = sender.deepMap(data, sender);
            sender.updateTreeviewSelection(sender.state.paneContextPath);
            sender.setState({ files: collect, fileData: data, showProgressBar: false }, sender.forceUpdate);
        }, (data: any) => { window.Dialog.alert(data); })
    }

    updateTreeviewSelection(relativePath: string) {
        let arr = this.state.treeNodes;
        this.deepScan(arr, relativePath, null)
        this.setState({ treeNodes: arr });
    }

    onFolderClicked(e: any) {
        var index = parseInt(e.currentTarget.getAttribute("itemid"));
        var item = this.state.files[index];
        this.setState({ selectedItem: item });
    }

    onFileClicked(e: any) {
        var index = parseInt(e.currentTarget.getAttribute("itemid"));
        var item = this.state.files[index];
        this.setState({ selectedItem: item });
    }

    renderFile(v: any, index: number) {
        let css = ""
        if (this.state.selectedItem) {
            if (v.fileID === this.state.selectedItem.fileID) {
                css = " selected";
            }
        }
        return (<button key={"file-" + index} itemID={index + ""} type="button" className={" text-center explorer-list" + css} onClick={this.onFileClicked} onDoubleClick={this.onDownloadDocumentClick} onContextMenu={this.onContextMenu}>
            <div className="explorer-button">
                <input type="checkbox" style={{ display: 'none' }} />
                <span className={this.iconClassName(v.text)}></span>
                <small title={v.description}>{v.text}</small>
            </div>
        </button>);
    }

    renderFolder(v: any, index: number) {
        let css = ""
        if (this.state.selectedItem) {
            if (v.fileID === this.state.selectedItem.fileID) {
                css = " selected";
            }
        }

        return (<button key={"folder" + index} itemID={index + ""} type="button" className={"text-center explorer-list" + css} onClick={this.onFolderClicked} onContextMenu={this.onContextMenu} onDoubleClick={this.onFolderDoubleClick}>
            <a title={v.description}><div className="explorer-button">
                <input type="checkbox" style={{ display: 'none' }} />
                <span className="k-icon k-i-folder"></span>
                <small>{v.text}</small>
            </div>
            </a>
        </button>);
    }

    renderContextMenu() {
        return this.state.contextMenu;
    }

    onFolderDoubleClick(e: any) {
        e.preventDefault();
        e.stopPropagation();
        var index = parseInt(e.currentTarget.getAttribute("itemid"));
        var item = this.state.files[index];
        if (item.isFile)
            this.handleFileDoubleClick(e, item)
        else {
            this.handleFolderDoubleClick(e, item)
        }
    }

    handleFileDoubleClick(e: any, item: any) {

    }

    handleFolderDoubleClick(e: any, item: any) {
        this.setState({ paneContextPath: item.relativePath, fileId: item.fileID, showProgressBar: true }, this.loadFileOnPane)
    }

    handleFolderDoubleClickDetail(e: any, item: any) {
        this.setState({ paneContextPath: item.filePath, fileId: item.contractResourceFileGuid, showProgressBar: true }, this.loadFileOnPane)
    }

    handleFileDoubleClickDetail(e: any, item: any) {
        let url = this.props.operationUrl + "?id=" + encodeURIComponent(item.filePath) + "&resourceType=" + this.props.resourceType + "&id1=download"
        Remote.download(url, (data: any) => { }, (error: any) => {
            window.Dialog.alert(error);
        });
    }

    renderFilesAndFolders() {
        let items: any[] = [];
        this.state.files.map((item: any, index: number) => {
            if (item.isFile) {
                items.push(this.renderFile(item, index));
            }
            else {
                items.push(this.renderFolder(item, index))
            }
        });
        if (items.length === 0) {
            items.push(<div key="nocontent" className="col-12 text-center">No Content Found</div>)
        }
        return items;
    }

    is = (fileName: string, ext: string) => new RegExp(`.${ext}\$`).test(fileName);

    iconClassName(text: any) {
        if (this.is(text, 'pdf')) {
            return 'k-icon k-i-file-pdf';
        } else if (this.is(text, 'html')) {
            return 'k-icon k-i-html';
        } else if (this.is(text, 'jpg|png|bmp|gif|tif')) {
            return 'k-icon k-i-image';
        } else if (this.is(text, 'doc|docx')) {
            return 'k-icon k-i-file-doc';
        } else if (this.is(text, 'xls|xlsx')) {
            return 'k-icon k-i-file-xls';
        } else if (this.is(text, 'ppt|pptx')) {
            return 'k-icon k-i-file-ppt';
        }
        else {
            return 'k-icon k-i-file-txt';
        }
    }

    onCut(e: any, item: any) {
        this.setState({ cutElement: item, contextMenu: null, copiedElement: null }, this.onInit);
    }

    onCopy(e: any, item: any) {
        this.setState({ copiedElement: item, contextMenu: null }, this.forceUpdate)
    }
    onChangeViewButtonClick(e: any) {
        let view;
        let text;
        if (this.state.buttontext == 'List') {
            view = 'list'
            text = 'Detail'
        }
        else {
            view = 'detail'
            text = 'List'
        }
        this.setState({

            viewtype: view,
            buttontext: text
        });


    }

    async onPaste(e: any, item: any) {
        let sender = this;
        if (this.state.cutElement && this.state.cutElement.isFile) {
            let destinationPath = item && item.relativePath || this.state.paneContextPath;
            let folder = this.deepSearch(this.state.treeNodes, destinationPath);
            let data = {
                source: this.state.cutElement.relativePath,
                path: destinationPath,
                destinationGuid: folder.fileID,
                destinationParentGuid: folder.parentId,
                fileId: this.state.cutElement.fileID,
                parentId: this.state.fileId,
                operation: 'FileCutPaste'
            }
            var res = await Remote.postDataAsync(this.props.operationUrl, data);
            if (res.ok) {
                this.setState({ copiedElement: null, cutElement: null, contextMenu: null }, this.loadFileOnPane);
            }
            else {
                let data = await Remote.parseErrorMessage(res);
                window.Dialog.alert(data, "Error", () => { sender.setState({ showProgressBar: false, copiedElement: null, cutElement: null, contextMenu: null }, this.forceUpdate); });
            }
        }
        else if (this.state.cutElement && !this.state.cutElement.isFile) {
            let destinationPath = item && item.relativePath || this.state.paneContextPath;
            let folder = this.deepSearch(this.state.treeNodes, destinationPath);
            let data = {
                source: this.state.cutElement.relativePath,
                path: destinationPath,
                destinationGuid: folder.fileID,
                destinationParentGuid: folder.parentId,
                fileId: this.state.cutElement.fileID,
                parentId: this.state.fileId,
                operation: 'FolderCutPaste'
            }
            var res = await Remote.postDataAsync(this.props.operationUrl, data);
            if (res.ok) {
                this.setState({ copiedElement: null, cutElement: null, contextMenu: null, showProgressBar: false }, this.onInit);
            }
            else {
                let data = await Remote.parseErrorMessage(res);
                window.Dialog.alert(data, "Error", () => { sender.setState({ showProgressBar: false, copiedElement: null, cutElement: null, contextMenu: null }, this.forceUpdate); });
            }
        }
        else if (this.state.copiedElement && this.state.copiedElement.isFile) {
            let destinationPath = item && item.relativePath.replace("\\" + item.text, "").replace("/" + item.text, "") || this.state.paneContextPath;
            let folder = this.deepSearch(this.state.treeNodes, destinationPath);
            let data = {
                source: this.state.copiedElement.relativePath,
                path: destinationPath,
                operation: 'FileCopyPaste',
                destinationGuid: folder.fileID,
                destinationParentGuid: folder.parentId,
                fileId: this.state.copiedElement.fileID,
                parentId: this.state.fileId
            }

            var res = await Remote.postDataAsync(this.props.operationUrl, data);
            if (res.ok) {
                this.setState({ fileId: folder.fileID, copiedElement: null, cutElement: null, contextMenu: null }, this.loadFileOnPane)
            }
            else {
                let data = await Remote.parseErrorMessage(res);
                window.Dialog.alert(data, "Error", () => {
                    sender.setState({ showProgressBar: false, copiedElement: null, cutElement: null, contextMenu: null }, this.forceUpdate);
                });
            }
        }
        else if (this.state.copiedElement && !this.state.copiedElement.isFile) {
            let destinationPath = item && item.relativePath || this.state.paneContextPath;
            while (destinationPath.indexOf("\\") >= 0) {
                destinationPath = destinationPath.replace("\\", "/");
            }
            let folder: any = this.deepSearch(this.state.treeNodes, destinationPath);
            let data = {
                source: this.state.copiedElement.relativePath,
                path: destinationPath,
                destinationGuid: folder.fileID,
                destinationParentGuid: folder.parentId,
                operation: 'FolderCopyPaste',
                fileId: this.state.copiedElement.fileID,
                parentId: this.state.fileId
            }

            var res = await Remote.postDataAsync(this.props.operationUrl, data);
            if (res.ok) {
                this.setState({ fileId: folder.fileID, copiedElement: null, cutElement: null, contextMenu: null }, this.onInit)
            }
            else {
                let data = await Remote.parseErrorMessage(res);
                window.Dialog.alert(data, "Error", () => { sender.setState({ showProgressBar: false, copiedElement: null, cutElement: null, contextMenu: null }, this.forceUpdate); });
            }
        }
        //this.setState({ copiedElement: null, cutElement: null, contextMenu: null, showProgressBar: true }, this.forceUpdate)
    }

    onDelete(e: any, item: any) {
        if (!item.isFile) {
            this.setState({ contextMenu: null, fileId: item.fileID, showProgressBar: true }, () => { this.onDeleteFolder(item); })
        }
        else {
            this.setState({ contextMenu: null, fileId: item.fileID, showProgressBar: true }, () => { this.onDeleteFile(item); })
        }
    }

    onRename(e: any, item: any) {
        if (!item.isFile) {
            this.setState({ contextMenu: null, fileId: item.fileID, showProgressBar: true }, () => { this.onRenameFolder(item); this.forceUpdate(); })
        }
        else {
            this.setState({ contextMenu: null, fileId: item.fileID, showProgressBar: true }, () => { this.onRenameFile(item); this.forceUpdate(); })
        }
    }

    onDeleteFile(item: any) {
        let sender = this;
        let buttons = [
            { primary: true, requireValidation: true, text: "Delete", action: (e: any, o: any) => { sender.setState({ paneContextPath: this.state.paneContextPath, fileId: item.parentId, selectedItem: null, showProgressBar: false }, sender.onInit) } },
            { primary: false, text: "Cancel", action: (e: any, o: any) => { sender.setState({ paneContextPath: this.state.paneContextPath, fileId: item.parentId, showProgressBar: false }, sender.forceUpdate) } },
        ]

        let dialogProps = {
            ...this.state.dialogProps,
            dialogHeight: '36%',
            dialogWidth: '37%',
        }

        this.openDialog({
            dialogTitle: "Delete File",
            buttons: buttons,
            getUrl: this.props.operationConfirmationUrl + "?id=" + encodeURIComponent(item.relativePath) + "&resourceType=" + this.props.resourceType + "&id1=DeleteFile&fileId=" + this.state.fileId,
            method: "post",
            postUrl: this.props.operationUrl
        }, dialogProps);
    }

    onDeleteFolder(item: any) {

        let sender = this;
        let arr: string[] = item.relativePath.split("/");
        arr.pop();
        let parent = item.parentId;
        let buttons = [
            { primary: true, requireValidation: true, text: "Delete", action: (e: any, o: any) => { sender.setState({ fileId: parent, showProgressBar: false, selectedItem: null }, sender.onInit) } },
            { primary: false, text: "Cancel", action: (e: any, o: any) => { sender.setState({ showProgressBar: false }, sender.forceUpdate) } },
        ]

        let dialogProps = {
            ...this.state.dialogProps,
            dialogHeight: '36%',
            dialogWidth: '37%',
        }

        this.openDialog({


            dialogTitle: "Delete Folder",
            buttons: buttons,
            getUrl: this.props.operationConfirmationUrl + "?id=" + encodeURIComponent(this.urlencode(item.relativePath)) + "&resourceType=" + this.props.resourceType + "&id1=DeleteFolder" + "&fileId=" + item.fileID,
            method: "post",
            postUrl: this.props.operationUrl
        }, dialogProps);
    }

    onRenameFolder(item: any) {
        let sender = this;
        let arr: string[] = item.relativePath.split("/");
        arr.pop();
        let path = arr.join("/");
        let buttons = [
            {
                primary: true, requireValidation: true, text: "Rename", action: (e: any, o: any) => {
                    sender.setState({ paneContextPath: sender.state.paneContextPath, fileId: item.parentId, showProgressBar: false }, sender.onInit)
                }
            },
            { primary: false, text: "Cancel", action: (e: any, o: any) => { sender.setState({ showProgressBar: false }, sender.forceUpdate) } },
        ]

        let dialogProps = {
            ...this.state.dialogProps,
            dialogHeight: '30%',
            dialogWidth: '35%',
        }

        this.openDialog({
            dialogTitle: "Rename Folder",
            buttons: buttons,
            getUrl: this.props.operationConfirmationUrl + "?id=" + encodeURIComponent(this.urlencode(item.relativePath)) + "&resourceType=" + this.props.resourceType + "&id1=RenameFolder" + "&fileId=" + item.fileID,
            method: "post",
            postUrl: this.props.operationUrl
        }, dialogProps);
    }

    onRenameFile(item: any) {

        let sender = this;
        let arr: string[] = item.relativePath.split("/");
        arr.pop();
        let path = arr.join("/");
        let buttons = [
            { primary: true, requireValidation: true, text: "Rename", action: (e: any, o: any) => { sender.setState({ paneContextPath: path, fileId: item.parentId, showProgressBar: true }, sender.onInit) } },
            { primary: false, text: "Cancel", action: (e: any, o: any) => { sender.setState({ showProgressBar: false }, sender.forceUpdate) } },
        ]

        let dialogProps = {
            ...this.state.dialogProps,
            dialogHeight: '25%',
            dialogWidth: '35%',
        }

        this.openDialog({
            dialogTitle: "Rename File",
            buttons: buttons,
            getUrl: this.props.operationConfirmationUrl + "?id=" + encodeURIComponent(this.urlencode(item.relativePath)) + "&resourceType=" + this.props.resourceType + "&fileId=" + this.state.fileId + "&id1=RenameFile",
            method: "post",
            postUrl: this.props.operationUrl
        }, dialogProps);
    }

    onCreateFolderButtonClick(e: any) {
        this.onCreateFolder(e, null);
    }

    onCreateFolder(e: any, item: any) {
        let sender = this;
        let buttons = [
            { primary: true, requireValidation: true, text: "Create", action: (e: any, o: any) => { sender.setState({ paneContextPath: sender.state.paneContextPath, fileId: item ? item.parentId : this.state.fileId, showProgressBar: true }, sender.onInit) } },
            { primary: false, text: "Cancel", action: (e: any, o: any) => { sender.setState({ showProgressBar: false }, sender.forceUpdate) } },
        ]

        let dialogProps = {
            ...this.state.dialogProps,
            dialogHeight: '32%',
            dialogWidth: '35%',
        }

        if (item == null) {
            this.setState({ contextPath: this.state.paneContextPath, showCreateFolderDialog: true, dialogProps: dialogProps }, this.onInit)
            this.openDialog({
                dialogTitle: "Create Folder",
                buttons: buttons,
                getUrl: this.props.operationConfirmationUrl + "?id=" + encodeURIComponent(this.state.paneContextPath) + "&resourceType=" + this.props.resourceType + "&id1=CreateFolder" + "&resourceId=" + this.props.resourceId + "&parentId=" + this.state.fileId,
                method: "post",
                postUrl: this.props.operationUrl
            }, dialogProps);
        }
        else {
            this.setState({ contextPath: item.relativePath, showCreateFolderDialog: true, dialogProps: dialogProps }, this.onInit)
            this.openDialog({
                dialogTitle: "Create Folder",
                buttons: buttons,
                getUrl: this.props.operationConfirmationUrl + "?id=" + encodeURIComponent(item.relativePath) + "&resourceType=" + this.props.resourceType + "&id1=CreateFolder" + "&resourceId=" + this.props.resourceId + "&parentId=" + item.fileID,
                method: "post",
                postUrl: this.props.operationUrl
            }, dialogProps);
        }

    }

    onContextMenuMouseOut(e: any) {
        this.setState({ contextMenu: null }, this.forceUpdate)
    }

    onContextMenu(e: any) {
        e.preventDefault();
        e.stopPropagation();
        var index = parseInt(e.currentTarget.getAttribute("itemid"));
        var item = this.state.files[index];
        if (item == undefined && isNaN(index)) {
            this.setState({ selectedItem: item });
            this.handlePaneContextMenu(e, item);
        }
        else if (item) {
            this.setState({ selectedItem: item });
            if (item.isFile)
                this.handleFileContextMenu(e, item)
            else {
                this.handleFolderContextMenu(e, item)
            }
        }
    }

    onTreeContextMenu(e: any) {
        e.preventDefault();
        e.stopPropagation();
        var path = e.currentTarget.getAttribute("itemid");
        var item = this.deepSearch(this.state.treeNodes, path);
        if (item != undefined) {
            this.handleFolderContextMenu(e, item)
        }
    }

    handleFileContextMenu(e: any, item: any) {
        var menus: any[] = [];
        if (this.props.hasManagePermission) {
            menus.push({
                text: "Copy",
                icon: 'copy',
                action: this.onCopy
            });
            menus.push({
                text: "Cut",
                icon: 'cut',
                action: this.onCut
            });
            menus.push({
                text: "Delete",
                icon: 'delete',
                action: this.onDelete
            });
            menus.push({
                text: "Rename",
                icon: 'change-manually',
                action: this.onRename
            });
        }
        menus.push({
            text: "Download",
            icon: 'download',
            action: this.onDownloadDocument
        });
        let style = {
            top: (e.clientY - this.state.contextMenuTopMargin) + "px",
            left: (e.clientX - this.state.contextMenuLeftMargin) + "px",
            zIndex: 1000001
        }


        let menu = <ContextMenu menus={menus} sender={this} style={style} item={item}></ContextMenu>
        this.setState({ contextMenu: menu, fileId: item.fileID });

    }

    onDownloadDocument(e: any, item: any) {
        let url = this.props.operationUrl + "?id=" + encodeURIComponent(item.relativePath) + "&resourceType=" + this.props.resourceType + "&id1=download"
        Remote.download(url, (data: any) => { }, (error: any) => {
            window.Dialog.alert(error);
        });
    }

    onDownloadDocumentClick(e: any) {
        let idx = e.currentTarget.getAttribute("itemid");
        var item = this.state.files[idx];
        if (item != undefined) {
            let url = this.props.operationUrl + "?id=" + encodeURIComponent(item.relativePath) + "&resourceType=" + this.props.resourceType + "&id1=download"
            Remote.download(url, (data: any) => { }, (error: any) => {
                window.Dialog.alert(error);
            });
        }

    }

    handleFolderContextMenu(e: any, item: any) {
        var menus: any[] = [];
        if (this.props.hasManagePermission) {
            menus.push({
                text: "Folder",
                icon: 'folder',
                action: this.onCreateFolder
            });
            if (item.relativePath != this.props.basePath) {
                menus.push({
                    text: "Copy",
                    icon: 'copy',
                    action: this.onCopy
                });
            }
            if (this.state.copiedElement != null || this.state.cutElement != null) {
                menus.push({
                    text: "Paste",
                    icon: 'paste',
                    action: this.onPaste
                });
            }
            if (item.relativePath != this.props.basePath && item.readonly == false) {
                menus.push({
                    text: "Cut",
                    icon: 'cut',
                    action: this.onCut
                });
                menus.push({
                    text: "Rename",
                    icon: 'change-manually',
                    action: this.onRename
                });
                menus.push({
                    text: "Delete",
                    icon: 'delete',
                    action: this.onDelete
                });
            }
        }

        let style = {
            top: (e.clientY - this.state.contextMenuTopMargin) + "px",
            left: (e.clientX - this.state.contextMenuLeftMargin) + "px",
            zIndex: 1000001
        }
        if (menus.length > 0) {
            let menu = <ContextMenu menus={menus} sender={this} style={style} item={item}></ContextMenu>
            this.setState({ contextMenu: menu, fileId: item.fileID });
        }
    }

    handlePaneContextMenu(e: any, item: any) {
        var menus: any[] = [];
        menus.push({
            text: "Folder",
            icon: 'folder',
            action: this.onCreateFolder
        });
        if (this.state.copiedElement != null || this.state.cutElement != null) {
            menus.push({
                text: "Paste",
                icon: 'paste',
                action: this.onPaste
            });
        }

        let style = {
            top: (e.clientY - this.state.contextMenuTopMargin) + "px",
            left: (e.clientX - this.state.contextMenuLeftMargin) + "px"
        }
        if (menus.length > 0) {
            let menu = <ContextMenu menus={menus} sender={this} style={style} item={item}></ContextMenu>
            this.setState({ contextMenu: menu });
        }
    }

    registerDialog(e: any) {
        DocumentManager1.instance.dialog = e;
    }

    renderProgressing() {
        if (this.state.showProgressBar) {
            return (<div className="k-loading-mask">
                <span className="k-loading-text">Loading</span>
                <div className="k-loading-image"></div>
                <div className="k-loading-color"></div>
            </div>)
        }
    }

    onDragging(e: any) {
        let sender = this;
        e.preventDefault()
        e.stopPropagation();
        $(".document-manager").eq(0).find(".k-upload").eq(1).css({ display: 'block' });
        $(".k-upload").eq(1).on("dragleave", function (e1: any) {
            console.log("k-upload", "dragleave")
            sender.uploaderDragLeave = true;
            sender.onDraggingComplete(e1)
            if ((e1.pageX && e1.pageX < 10) || (e1.pageY && e1.pageY < 220)) {
                $(".document-manager").eq(0).find(".k-upload").eq(1).css({ display: 'none' });
            }
        })
        $(".k-upload").eq(1).on("dragend", function (e1: any) {
            console.log("k-upload", "dragend")
            sender.uploaderDragLeave = true;
            sender.onDraggingComplete(e1)
            if ((e1.pageX && e1.pageX < 10) || (e1.pageY && e1.pageY < 220)) {
                $(".document-manager").eq(0).find(".k-upload").eq(1).css({ display: 'none' });
            }
        })
        $("body").eq(0).on("dragleave", function (e1: any) {
            if (sender.uploaderDragLeave && $(".document-manager").eq(0).find(".k-upload").eq(1).is(":visible")) {
                sender.uploaderDragLeave = false;
                sender.onDraggingComplete(e1)
            }
            else if ((e1.pageX && e1.pageX < 10) || (e1.pageY && e1.pageY < 220)) {
                $(".document-manager").eq(0).find(".k-upload").eq(1).css({ display: 'none' });
            }
        })
        $("body").eq(0).on("dragenter", function (e1: any) {
            if (!sender.uploaderDragLeave && !$(".document-manager").eq(0).find(".k-upload").eq(1).is(":visible")) {
                $(".document-manager").eq(0).find(".k-upload").eq(1).css({ display: 'block' });
            }
            else if ((e1.pageX && e1.pageX < 10) || (e1.pageY && e1.pageY < 220)) {
                $(".document-manager").eq(0).find(".k-upload").eq(1).css({ display: 'none' });
            }

        })
        return false;
    }

    onDraggingComplete(e: any) {
        $(".document-manager").eq(0).find(".k-upload").eq(1).css({ display: 'none' });
        return false;
    }


    openDialog(param: any, dialogProps: any) {
        DocumentManager1.instance.dialog.props = {
            ...dialogProps,
            dialogTitle: param.dialogTitle,
            buttons: param.buttons,
            getUrl: param.getUrl,
            getMethod: param.getMethod || 'get',
            postData: param.postData || [],
            method: param.method,
            postUrl: param.postUrl,
        }
        this.setState({ dialogProps: DocumentManager1.instance.dialog.props, showProgressBar: true }, DocumentManager1.instance.dialog.openForm);
    }

    onDocumentSearchPressed(e: any) {
        if (e.keyCode == 13) {
            this.onDocumentSearch(e);
        }
    }

    onDocumentSearch(e: any) {
        let sender = this;
        Remote.get(this.props.operationConfirmationUrl + "?id=" + encodeURIComponent(this.props.basePath) + "&resourceType=" + this.props.resourceType + "&parentId=" + this.state.fileId + "&id1=SearchFiles&id2=" + this.searchField.value, (data: any) => {
            let collect: any[] = this.deepMap(data, sender);
            this.setState({
                files: collect
            }, this.forceUpdate)
        }, (err: any) => {
            window.Dialog.alert(err);
        });
    }

    componentDidMount() {
    }

    onUploadProgress(e: any) {
        this.setState({ showProgressBar: true }, this.forceUpdate)
    }

    onBeforeUpload(e: any) {
        let token = Remote.getCookieValue('X-CSRF-TOKEN');
        let reqValToken = Remote.getCookieValue('RequestVerificationToken');
        e.headers = {
            'X-CSRF-TOKEN': token,
            'RequestVerificationToken': reqValToken
        }
        e.additionalData.parentId = this.state.fileId
        this.setState({ showProgressBar: true }, this.forceUpdate)
    }

    onUploadFileAdded(e: any) {
        this.setState({
            uploadingFiles: e.newState,
            showProgressBar: true
        });
        let sender = this;
        if (!e.newState)
            return;
        e.additionalData = {
            parentId: sender.state.fileId
        }
        var formData = new FormData();
        for (var i in e.newState) {
            formData.append("file[" + i + "]", e.newState[i].getRawFile());
        }
    }

    urlencode(str: string) {
        return str.replace("&", "-amp;");
    }

    onBreadcrumClick(e: any) {
        let link = e.currentTarget.getAttribute("rel")
        let item = this.deepSearch(this.state.treeNodes, link);
        let expanded: any[] = this.state.expandedItems;
        expanded.push(item);
        //this.setState({ paneContextPath: e.item.relativePath, fileId: e.item.fileID, selectedItem: e.item, expandedItems: expanded, showProgressBar: true }, this.loadFileOnPane)
        this.setState({ paneContextPath: link, selectedItem: item, fileId: item.fileID, expandedItems: expanded, showProgressBar: true }, this.loadFileOnPane)
    }

    renderBreadCrum() {
        let sp = this.state.paneContextPath.split("/");
        let path = "";
        let output: any[] = [];
        for (let i = 1; i < sp.length; i++) {
            path += "/" + sp[i];
            let link = (<span className="dir-separator" key={i}><a rel={path} href="javascript:void(0)" onClick={this.onBreadcrumClick}>{sp[i]}</a></span>);
            output.push(link);
        }
        return output;
    }

    onGridItemClick(e: any, parent: any) {
        let sender = null;
        if (parent) {
            sender = parent;
        }
        else {
            sender = this;
        }
        if (e.dataItem.isFile)
            sender.handleFileDoubleClick(e, e.dataItem)
        else if (e.dataItem.Type == 'folder') {
            sender.handleFolderDoubleClick(e, e.dataItem)
        }
    }

    onGridRowClick(e: any) {
        if (e.dataItem.isFile)
            this.handleFileDoubleClickDetail(e, e.dataItem)
        else {
            this.handleFolderDoubleClickDetail(e, e.dataItem)
        }
    }

    onGridViewContextMenu(e: any) {

    }

    onFileUploadClick(e: any) {
        $(".fileUploader").find("input[type=file]").eq(0).trigger("click");
    }

    renderDataGrid() {
        return (
            <div onContextMenu={this.onContextMenu}>
                <Grid
                    style={{ height: '400px' }}
                    data={this.state.fileData}
                    scrollable={'none'}
                    rowRender={this.rowRender}
                >
                    <Column field="uploadFileName" title="Name" width="350px" cell={(props: any) => {
                        return (<FileCommandCell itemID={props.dataIndex - 1} dataItem={props.dataItem} stylefunction={this.iconClassName} alignment="Horizontal" onItemClick={this.onGridItemClick} parent={this} onContextMenu={this.onContextMenu} onDoubleClick={this.onFolderDoubleClick}></FileCommandCell>)
                    }} />
                    <Column field="description" title="Description" width="200px" />
                    <Column field="updatedByName" title="Uploaded By" width="200px" />
                    <Column field="updatedOn" title="Date Modified" width="150px" cell={(props: any) => {
                        return (<td>{formatUSDatetime(props.dataItem.updatedOn)}</td>)
                    }} />
                    <Column field="mimeType" title="Type" width="80px" />
                    <Column field="fileSize" title="Size" width="80px" />

                </Grid>
            </div>
        );
    }

    rowRender(trElement: any, dataItem: any) {

        const trProps = {
            ...trElement.props,
            onDoubleClick: () => {
                this.onGridRowClick(dataItem)
            },

        };
        return React.cloneElement(trElement, { ...trProps }, trElement.props.children);
    }

    __expandCollapseAllNodes(nodes: any[], value: any): any {
        nodes.map((v, i) => {
            v.expanded = value;
            if (v.items && v.items.length && v.items.length > 0) {
                this.__expandCollapseAllNodes(v.items, value);
            }
        })
    }

    onExpandAll(e: any) {
        e.preventDefault();
        var d = this.state.treeNodes;
        this.__expandCollapseAllNodes(d, true);
        this.setState({ treeNodes: d });
    }

    onCollapseAll(e: any) {
        e.preventDefault();
        var d = this.state.treeNodes;
        this.__expandCollapseAllNodes(d, false);
        this.setState({ treeNodes: d });
    }

    render() {
        let data;
        if (this.state.viewtype == 'list') {
            data = <div className="pane-content file-explore" onContextMenu={this.onContextMenu} style={{ display: 'table', height: '100%', width: '100%' }}>
                <div key={"parentRow"} className="row no-gutters align-items-start">
                    {this.renderFilesAndFolders()}
                </div>
            </div>
        }
        else if (this.state.viewtype == 'detail') {
            data = this.renderDataGrid();
        }
        return (
            <div className="col-sm-12 document-manager k-dropzone" onDragEnter={this.onDragging} onDrop={this.onDraggingComplete} onMouseUp={this.onDraggingComplete}>
                <div className="row mb-2 document-manager-top-header">
                    {this.props.showSearchBox && <div className="col-md-6 col-lg-4 pl-0">
                        <div className="input-group">
                            <Input ref={c => { this.searchField = c; }} type="text" onKeyDown={this.onDocumentSearchPressed} className="form-control" />
                            <div className="input-group-append">
                                <Button primary={true} onClick={this.onDocumentSearch} className="input-group-text"><i className="k-icon k-i-search"></i></Button>
                            </div>
                        </div>
                    </div>}
                    {this.props.hasManagePermission && <div className="col-sm-6 col-md-8 pr-0 text-right d-none">
                        <Button style={{ display: 'none' }} className={this.state.buttontext} onClick={this.onChangeViewButtonClick}><i className="k-icon k-i-grid-layout"></i></Button>
                        <Button style={{ display: 'none' }} className="mr-2" onClick={this.onCreateFolderButtonClick}>Add Folder</Button>
                        {<Upload
                            ref={(c: any) => { this.uploader1 = c; }}
                            className="fileUploader"
                            batch={false}
                            multiple={true}
                            withCredentials={true}
                            saveUrl={this.props.uploadUrl + encodeURIComponent(this.state.paneContextPath)}
                            autoUpload={true}
                            onAdd={this.onUploadFileAdded}
                            onBeforeUpload={this.onBeforeUpload}
                            onProgress={this.onUploadProgress}
                            onStatusChange={(ev: UploadOnStatusChangeEvent) => {

                                if (ev.response.response.status == true) {
                                    this.setState({ showProgressBar: false }, this.onInit)
                                }
                                else {
                                    this.setState({ showProgressBar: true }, this.forceUpdate)
                                }
                            }}

                        />}
                    </div>}
                </div>
                <div className="row document-management-menu">
                    <div className="col px-2">
                        <div className="file-path w-100">
                            {this.renderBreadCrum()}
                        </div>
                    </div>
                    <div className="col-auto document-management-action px-0">
                        {this.props.hasManagePermission && <button className="btn btn-link" type="button" disabled={!(this.state.selectedItem)} onClick={(e: any) => { this.onCopy(e, this.state.selectedItem); }}>Copy</button>}
                        {this.props.hasManagePermission && (this.state.copiedElement || this.state.cutElement) && <button className="btn btn-link" type="button" onClick={(e: any) => { this.onPaste(e, this.state.selectedItem); }}>Paste</button>}
                        {this.props.hasManagePermission && <button className="btn btn-link" type="button" disabled={!(this.state.selectedItem && this.state.selectedItem.readonly === false)} onClick={(e: any) => { this.onCut(e, this.state.selectedItem); }}>Cut</button>}
                        {this.props.hasManagePermission && <button className="btn btn-link" type="button" disabled={!(this.state.selectedItem && this.state.selectedItem.readonly === false)} onClick={(e: any) => { this.onDelete(e, this.state.selectedItem); }}>Delete</button>}
                        {this.props.hasManagePermission && <button className="btn btn-link" type="button" disabled={!(this.state.selectedItem && this.state.selectedItem.readonly === false)} onClick={(e: any) => { this.onRename(e, this.state.selectedItem); }}>Rename</button>}
                        <button className="btn btn-link" type="button" disabled={!(this.state.selectedItem && this.state.selectedItem.isFile === true)} onClick={(e: any) => { this.onDownloadDocument(e, this.state.selectedItem); }}>Download</button>
                        {this.props.hasManagePermission && <div className="d-inline dropdown">
                            <button className="btn btn-link dropdown-toggle" type="button" id="dropdownMenu2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                <i className="k-icon k-i-plus"></i> Add
                            </button>
                            <div className="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenu2">
                                <button className="dropdown-item" type="button" onClick={this.onCreateFolderButtonClick}>Add Folder</button>
                                <button className="dropdown-item" type="button" onClick={this.onFileUploadClick}>Add FIles</button>
                            </div>
                        </div>}
                        <Button className={this.state.buttontext} type="button" onClick={this.onChangeViewButtonClick}><i className="k-icon k-i-grid-layout"></i></Button>

                    </div>
                </div>
                <div className="row draggingUpload">
                    {<Upload
                        ref={(e) => { this.uploader = e; }}
                        batch={false}
                        multiple={true}
                        withCredentials={true}
                        saveUrl={this.props.uploadUrl + encodeURIComponent(this.state.paneContextPath)}
                        autoUpload={true}
                        onAdd={this.onUploadFileAdded}
                        onBeforeUpload={this.onBeforeUpload}
                        onProgress={this.onUploadProgress}
                        onStatusChange={(ev: UploadOnStatusChangeEvent) => {
                            if (ev.response.response.status == true) {
                                this.setState({ showProgressBar: false }, this.onInit)
                            }
                        }}
                    />}
                </div>
                <div className="row">
                    <Splitter
                        style={{ height: 350, width: "100%" }}
                        panes={this.state.panes}
                        orientation={'horizontal'}
                        onLayoutChange={this.onLayoutChange}
                    >
                        <div className="pane-content" onContextMenu={this.onContextMenu} style={{ display: 'table', height: '100%', width: '100%' }}>
                            <TreeView
                                data={this.state.treeNodes}
                                expandIcons={true}
                                onExpandChange={this.onExpandChange}
                                onCheckChange={this.onTreeItemClick}
                                onItemClick={this.onTreeItemClick}
                                aria-multiselectable={true}
                                itemRender={(props) => {
                                    let css = "";
                                    if (this.state.selectedItem && this.state.selectedItem.fileID === props.item.fileID) {
                                        css = "expanded";
                                    }
                                    return (<div onContextMenu={this.onTreeContextMenu} itemID={props.item.relativePath} className={css}><span className="k-icon k-i-folder icon-font" key='0'></span> <div style={{ display: 'inline-block' }} title={props.item.description} dangerouslySetInnerHTML={{ __html: unescape(props.item.text) }}></div></div>)
                                }
                                }
                            />
                            <div className="expand_collapse">
                                <a href="#" onClick={this.onExpandAll}>Expand All</a> / <a href="#" onClick={this.onCollapseAll}>Collapse All</a>
                            </div>
                        </div>
                        {data}
                    </Splitter>
                </div>
                {this.state.contextMenu != null && this.renderContextMenu()}
                <KendoDialog {...this.state.dialogProps}></KendoDialog>
                {this.renderProgressing()}

            </div>
        );
    }
}