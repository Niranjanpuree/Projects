import 'core-js';
import * as React from "react"
import * as ReactDOM from "react-dom"
import { Dialog, DialogActionsBar } from '@progress/kendo-react-dialogs';
import { Button } from '@progress/kendo-react-buttons';
import { Upload } from '@progress/kendo-react-upload';
import { Splitter } from '@progress/kendo-react-layout';
import { Remote } from "../Remote/Remote";

declare var window: any;

interface IFileBrowserProps {
    onCloseFileDialog: any
    filePath: string;
    onFileSelected?: any;
}

interface IFileBrowserState {
    panes: any[];
    folders: any[];
    files: any[];
    selectedFolder: string;
}

export class FileBrowser extends React.Component<IFileBrowserProps, IFileBrowserState>{

    constructor(props: IFileBrowserProps) {
        super(props);
        this.state = {
            panes: [
                { size: '30%', resizable: true },
                { resizable: true }
            ],
            folders: [],
            files: [],
            selectedFolder: ''
        }
        this.renderBrowser = this.renderBrowser.bind(this);
        this.onLayoutChange = this.onLayoutChange.bind(this);
        this.renderFolder = this.renderFolder.bind(this);
        this.onFolderClick = this.onFolderClick.bind(this);
        this.renderFiles = this.renderFiles.bind(this);
        this.onFileClick = this.onFileClick.bind(this);

    }

    async componentDidMount() {
        let folders = await Remote.getAsync("/richeditor/folders");
        if (folders.ok) {
            let json: any[] = await folders.json();
            json.map((v: any) => {
                v.selected = false
            })
            this.setState({ folders: json }, this.forceUpdate)
        }
        else {
            let msg = Remote.parseErrorMessage(folders);
            window.Dialog.alert(msg, "Error");
        }
    }

    async fetchFiles() {
        let response = await Remote.getAsync('/RichEditor/Files/' + this.state.selectedFolder);
        if (response.ok) {
            let files = await response.json();
            this.setState({ files: files }, this.forceUpdate);
        }
        else {
            let msg = Remote.parseErrorMessage(response);
            window.Dialog.alert(msg, "Error");
        }
    }

    render() {
        return (<Dialog width={900} height={573}>
            {this.renderBrowser()}
            <DialogActionsBar>
                <div className="col-12 filemanager">
                    <Upload />
                </div>
                <Button >Update</Button>
                <Button onClick={this.props.onCloseFileDialog}>Cancel</Button>
            </DialogActionsBar>
        </Dialog>)
    }

    onLayoutChange(e: any) {
        this.setState({
            panes: e
        });
    }

    async onFolderClick(e: any) {
        e.preventDefault();
        var index = e.target.getAttribute("itemid");
        var items: any[] = this.state.folders;
        items.map((v: any) => { v.selected = false });
        items[index].selected = true;
        this.setState({ folders: items, selectedFolder: items[index].folderName }, async () => {
            await this.fetchFiles();
        })
    }

    renderFolder() {
        let arr: any[] = [];
        this.state.folders.map((v: any, i: number) => {
            if (v.selected) {
                arr.push(<a href="#" key={i} itemID={i+""} className="col-12" onClick={this.onFolderClick}><i className="k-icon k-i-folder-open"></i> {v.folderName}</a>)
            }
            else {
                arr.push(<a href="#" key={i} itemID={i + ""} className="col-12" onClick={this.onFolderClick}><i className="k-icon k-i-folder"></i> {v.folderName}</a>)
            }
        })
        return arr;
    }

    renderFiles() {
        let arr: any[] = [];
        this.state.files.map((v: any, i: number) => {
            arr.push(<a href="#" key={i} itemID={i + ""} className="col-3 row text-center file-link" onClick={this.onFileClick}><i className="k-icon k-i-image col-12" style={{ fontSize: '50px' }}></i> <span className="col-12 text-center" style={{ whiteSpace: 'normal' }}>{v.fileName}</span></a>)
        })
        return arr;
    }

    onFileClick(e: any) {
        e.preventDefault();
        var index = e.currentTarget.getAttribute("itemid");
        var items: any[] = this.state.files;
        var item = items[index];
        if (this.props.onFileSelected) {
            this.props.onFileSelected(item);
        }
    }

    renderBrowser() {

        return (<div>
            <div className="col-12">
                <h4 className="alert-heading">File Browser</h4>
            </div>
            <Splitter
                style={{ height: 350, width: "100%" }}
                panes={this.state.panes}
                orientation={'horizontal'}
                onLayoutChange={this.onLayoutChange}
            >
                <div className="pane-content" style={{ overflowX: 'none' }}>
                    <div className="col-12 row">{this.renderFolder()}</div>
                </div>
                <div className="pane-content">
                    <div className="col-12 row">{this.renderFiles()}</div>
                </div>
            </Splitter>
        </div>)
    }

}