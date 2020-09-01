import * as React from "react"
import * as ReactDOM from "react-dom"
import { Remote } from "../../../../Common/Remote/Remote";
import { KendoGrid } from "../../../../Common/Grid/KendoGrid";
import { Dialog, DialogProps, DialogActionsBar } from "@progress/kendo-react-dialogs";
declare var $: any;
declare var window: any;
declare var formatUSDate: any;
declare var numberWithCommas: any;

export interface IUrls {
    dataUrl: string,
    fieldUrl: string,
    exportUrl: string
}

export interface WbsUrls {
    gridUrls: IUrls,
    ActionUrls: {
        addUrl: string,
        editUrl: string,
        deleteUrl: string
    },
    WbsDictionaryUrls: IUrls,
    WbsDictionaryActionUrl: {
        addUrl: string,
        editUrl: string,
        deleteUrl: string
    }
}

interface IWBSTabPros {
    projectNumber: any;
    urls: WbsUrls
}

interface IWBSTabState {
    showDialog: boolean;
    showAddNewDialog: boolean;
    showEditingDialog: boolean;
    showWBSDetailsDialog: boolean;
    viewData: any;
    wbsNumber: any;
    newDictionary: string;
    projectNumber: any;
    editingDictionary: any;
    switchOn: boolean;
    dataUrl: any;
}

export class WBSTab extends React.Component<IWBSTabPros, IWBSTabState> {
    grid: KendoGrid = null;
    mainGrid: KendoGrid = null;
    constructor(props: any) {
        super(props);
        this.onWBSDictionaryMoreLinkClicked = this.onWBSDictionaryMoreLinkClicked.bind(this);
        this.renderDialog = this.renderDialog.bind(this);
        this.closeDialog = this.closeDialog.bind(this);
        this.onAddNewDictionary = this.onAddNewDictionary.bind(this);
        this.renderAddNewDictionDialog = this.renderAddNewDictionDialog.bind(this);
        this.onSaveAddNewDictionaryDialog = this.onSaveAddNewDictionaryDialog.bind(this);
        this.onCloseAddNewDictionaryDialog = this.onCloseAddNewDictionaryDialog.bind(this);
        this.onNewDictionaryChange = this.onNewDictionaryChange.bind(this);
        this.onUpdateDictionaryDialog = this.onUpdateDictionaryDialog.bind(this);
        this.onCloseEditDictionaryDialog = this.onCloseEditDictionaryDialog.bind(this);
        this.renderEditDictionDialog = this.renderEditDictionDialog.bind(this);
        this.onCloseWBSDetailsDialog = this.onCloseWBSDetailsDialog.bind(this);
        this.refreshGrid = this.refreshGrid.bind(this);

        this.state = {
            showDialog: false,
            showAddNewDialog: false,
            showWBSDetailsDialog: false,
            newDictionary: '',
            viewData: null,
            showEditingDialog: false,
            projectNumber: '',
            wbsNumber: null,
            editingDictionary: '',
            switchOn: false,
            dataUrl: this.props.urls.gridUrls.dataUrl + "&excludeIWOs=" + false
        }
    }

    onWBSDictionaryMoreLinkClicked(e: any) {
        let wbsNumber = e.wbsNumber;
        let projectNumber = e.projectNumber;
        this.setState({
            showDialog: true,
            projectNumber: projectNumber,
            wbsNumber: wbsNumber
        })
    }

    closeDialog(e: any)
    {
        this.setState({ showDialog: false }, this.refreshGrid);
    }

    onAddNewDictionary(data: any) {
        this.setState({ showAddNewDialog: true, newDictionary: '' });
    }

    onSaveAddNewDictionaryDialog(e: any) {
        let sender = this;
        let wbsDictionaryViewModel = {
            WbsNumber: this.state.wbsNumber,
            WbsDictionaryTitle: this.state.newDictionary,
            ProjectNumber: this.props.projectNumber
        }

        Remote.postData(this.props.urls.WbsDictionaryActionUrl.addUrl, wbsDictionaryViewModel, (data: any) => { sender.setState({ showAddNewDialog: false }); sender.grid.refresh(); }, (error: any) => { window.Dialog.alert(error) });
    }
    onCloseAddNewDictionaryDialog(e: any) {
        this.setState({ showAddNewDialog: false });
    }

    onNewDictionaryChange(e: any) {
        this.setState({ newDictionary: e.target.value });
    }

    onUpdateDictionaryDialog(e: any) {
        let sender = this;
        var wbsDictionaryRow = this.state.viewData.dataItem;
        let wbsDictionaryViewModel = {
            wbsDictionaryGuid: wbsDictionaryRow.wbsDictionaryGuid,
            wbsNumber: sender.state.wbsNumber,
            projectNumber: sender.state.projectNumber,
            WbsDictionaryTitle: sender.state.newDictionary,
        }
        
        Remote.postData(this.props.urls.WbsDictionaryActionUrl.editUrl, wbsDictionaryViewModel, (data: any) => { sender.setState({ showEditingDialog: false }); sender.grid.refresh(); }, (error: any) => { window.Dialog.alert(error) });
    }

    onCloseEditDictionaryDialog(e: any) {
        this.setState({ showEditingDialog: false });
    }

    onCloseWBSDetailsDialog(e: any)
    {
        this.setState({ showWBSDetailsDialog: false });
    }

    switchButton = {
        onLabel: "Exclude IWOs",
        offLabel: "Include IWOs",
        checked: false,
        action: (status: any) =>
        {
            this.setState({ switchOn: status, dataUrl: this.props.urls.gridUrls.dataUrl + "&excludeIWOs=" + status }, this.mainGrid.refresh)
        }
    }

    renderAddNewDictionDialog() {
        if (this.state.showAddNewDialog) {
            let sender = this;
            let prp: DialogProps = {
                height: '232px',
                width: '70%',
                title: 'Add WBS Dictionary',
                onClose: (e: any) => {
                    sender.setState({ showAddNewDialog: false });
                },
                closeIcon: true
            }
            return (<Dialog {...prp}>
                <div className="">
                    <label htmlFor="title" className="">WBS Dictionary Title</label>
                    <textarea name="title" className="form-control" rows={3} defaultValue={this.state.newDictionary} onChange={this.onNewDictionaryChange}></textarea>
                </div>
                <DialogActionsBar>
                    <button className="k-button k-primary" onClick={this.onSaveAddNewDictionaryDialog}>Save</button>
                    <button className="k-button" onClick={this.onCloseAddNewDictionaryDialog}>Cancel</button>
                </DialogActionsBar>
            </Dialog>)
        }
    }

    renderEditDictionDialog() {
        if (this.state.showEditingDialog) {
            let sender = this;
            let prp: DialogProps = {
                height: '232px',
                width: '70%',
                title: 'Edit WBS Dictionary',
                onClose: (e: any) => {
                    sender.setState({ showEditingDialog: false });
                },
                closeIcon: true
            }
            return (<Dialog {...prp}>
                <div className="">
                    <label htmlFor="title" className="">WBS Dictionary Title</label>
                    <textarea name="title" className="form-control" rows={3} defaultValue={this.state.newDictionary} onChange={this.onNewDictionaryChange}></textarea>
                </div>
                <DialogActionsBar>
                    <button className="k-button k-primary" onClick={this.onUpdateDictionaryDialog}>Save</button>
                    <button className="k-button" onClick={this.onCloseEditDictionaryDialog}>Cancel</button>
                </DialogActionsBar>
            </Dialog>)
        }
    }

    renderWBSDetailsDialog()
    {
        if (this.state.showWBSDetailsDialog) {
            let sender = this;
            let prp: DialogProps = {
                height: '40%',
                width: '50%',
                title: 'Edit WBS Dictionary',
                onClose: (e: any) =>
                {
                    sender.setState({ showEditingDialog: false });
                },
                closeIcon: true
            }
            return (<Dialog {...prp}>
                <div className="container">
                    <div className="row">
                        <div className="col-12">
                            <h2 className="col-12 row">Project/Mod Details</h2>
                            <div className="col-sm-12 form-group row">
                                <label className="control-label control-label-read col-4">Mod Number</label>
                                <div className="form-value col-8">
                                    {this.state.viewData.wbsNumber}
                                </div>
                            </div>
                            <div className="col-sm-12 form-group row">
                                <label className="control-label control-label-read col-4">Title</label>
                                <div className="form-value col-8">
                                    {this.state.viewData.allowCharging}
                                </div>
                            </div>
                            <div className="col-sm-12 form-group row">
                                <label className="control-label control-label-read col-4">Description</label>
                                <div className="form-value col-8">
                                    {this.state.viewData.wbsDescription}
                                </div>
                            </div>
                            
                        </div>
                    </div>
                </div>
                <DialogActionsBar>
                    <button className="k-button" onClick={this.onCloseWBSDetailsDialog}>Cancel</button>
                </DialogActionsBar>
            </Dialog>)
        }
    }

    renderDialog() {
        if (this.state.showDialog) {
            let sender = this;
            let prp: DialogProps = {
                height: '75%',
                width: '50%',
                title: 'WBS Dictionary',
                onClose: (e: any) => {
                    sender.setState({ showDialog: false });
                },
                closeIcon: true
            }
            let gridMenu = [
                { text: 'Export to PDF', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
                { text: 'Export to Excel', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() } }
            ]
            let groupAddButton = {
                text: "Add Dictionary",
                icon: "plus-sm",
                redirect: false,
                updateView: true,
                action: function (data: any)
                {
                    sender.onAddNewDictionary(data)
                }
            }
            let rowMenu = [
                {
                    text: "Edit", icon: "pencil", action: (data: any, grid: any) =>
                    {
                        sender.setState({ viewData: data, showEditingDialog: true, newDictionary: data.dataItem.wbsDictionaryTitle })
                    }
                },
                {
                    text: "Delete", icon: "delete", action: (data: any, grid: any) => {

                        let ids: any[] = [];
                        ids.push(data.dataItem.wbsDictionaryGuid)
                        let senderData = data;
                        window.Dialog.confirm({
                            text: "Are you sure you want to  delete wbs dictionary?",
                            title: "Confirm",
                            ok: function (e: any) {
                                Remote.postData(sender.props.urls.WbsDictionaryActionUrl.deleteUrl, ids, (data: any) => { grid.refresh(); }, (error: any) => { });
                            },
                            cancel: function (e: any) {
                            }
                        });
                    }
                }]
            return (<Dialog {...prp}>

                <div className="" id="wbsGridList">
                    <KendoGrid ref={(c) => this.grid = c} key="wbsDictionaryGrid" showColumnMenu={false} showAddNewButton={false} showAdvanceSearchBox={false} rowMenus={rowMenu} gridMenu={gridMenu} addRecord={groupAddButton} identityField="wbsDictionaryGuid" fieldUrl={this.props.urls.WbsDictionaryUrls.fieldUrl} exportFieldUrl={this.props.urls.WbsDictionaryUrls.exportUrl} dataURL={this.props.urls.WbsDictionaryUrls.dataUrl + "?projectNumber=" + this.state.projectNumber + "&wbsNumber=" + this.state.wbsNumber} parent="wbsGridList" />
                </div>

                <DialogActionsBar>
                    <button className="k-button k-primary" onClick={this.closeDialog}>Cancel</button>
                </DialogActionsBar>
            </Dialog>)
        }
    }

    refreshGrid()
    {
        this.mainGrid.refresh();
    }

    render() {
        let sender = this;
        let gridMenu = [
            { text: 'Export to PDF', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
            { text: 'Export to Excel', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() } },
            {
                text: 'Add WBS', icon: 'plus', action: (data: any, grid: any) =>
                {
                    
                }
            }
        ]
        let rowMenu = [
            {
                text: "Add Dictionaries", conditions: [{ field: 'wbsDictionaryTitle.length', value: 0 }], icon: "plus", action: (data: any, grid: any) =>
                {
                    let wbsNumber = data.dataItem.wbs;
                    let projectNumber = data.dataItem.projectNumber;
                    this.setState({
                        showDialog: true,
                        projectNumber: projectNumber,
                        wbsNumber: wbsNumber
                    })
                }
            },
            {
                text: "Edit Dictionaries", conditions: [{ field: 'wbsDictionaryTitle.length>0', value: true }] , icon: "edit", action: (data: any, grid: any) =>
                {
                    let wbsNumber = data.dataItem.wbs;
                    let projectNumber = data.dataItem.projectNumber;
                    this.setState({
                        showDialog: true,
                        projectNumber: projectNumber,
                        wbsNumber: wbsNumber
                    })
                }
            }]
        return (
            <div id="wbsGrid" className="wbs-grid">
                <KendoGrid key="wbsGrid" ref={(c) => { this.mainGrid = c; }} showColumnMenu={false} showAddNewButton={false} showAdvanceSearchBox={true} advancedSearchEntity={["PFS-Wbs"]} rowMenus={rowMenu} gridMenu={gridMenu} identityField="number" fieldUrl={this.props.urls.gridUrls.fieldUrl} exportFieldUrl={this.props.urls.gridUrls.exportUrl} dataURL={this.state.dataUrl} onMoreLinkClicked={this.onWBSDictionaryMoreLinkClicked} switchButton={this.switchButton} parent="wbsGrid" />
                {this.renderDialog()}
                {this.renderAddNewDictionDialog()}
                {this.renderEditDictionDialog()}
            </div>
        );
    }
}