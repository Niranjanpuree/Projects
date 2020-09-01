import * as React from "react"
import * as ReactDOM from "react-dom"
import { Remote } from "../../../../Common/Remote/Remote";
import { TabStrip, TabStripTab, PanelBar, PanelBarItem, PanelBarUtils, Menu, MenuItem, MenuItemModel, MenuItemLink, MenuItemArrow, Splitter } from '@progress/kendo-react-layout'
import { process } from "@progress/kendo-data-query";
import { Dialog, DialogProps, DialogActionsBar } from "@progress/kendo-react-dialogs";
import { KendoGrid } from "../../../../Common/Grid/KendoGrid";
import { concat } from "@progress/kendo-data-query/dist/npm/transducers";

declare var window: any;

export interface IUrls
{
    dataUrl: string,
    fieldUrl: string,
    exportUrl: string
}

export enum IDialogContentType
{
    WbsAddView,
    WbsEditView,
    WbsAddDictionaryView,
    WbsEditDictionaryView,
    WbsViewDictionaryView
}

interface IWbsDictionaryListProps
{
    wbsGuid: string;
    projectId: string;
    data: any;
    onSubmit: any;
    wbsDictionary: IUrls;
}

interface IWbsDictionaryListState
{
    showDialog: boolean;
    editingDictionary: any;
    showEditingDialog: boolean;
    newDictionary: string;
    showAddNewDialog: boolean;
    view: IDialogContentType;
}

export class WbsDictionaryList extends React.Component<IWbsDictionaryListProps, IWbsDictionaryListState> {

    grid: KendoGrid;
    constructor(props: IWbsDictionaryListProps)
    {
        super(props);
        this.onSubmit = this.onSubmit.bind(this);
        this.onAddNewDictionary = this.onAddNewDictionary.bind(this);
        this.onNewDictionaryChange = this.onNewDictionaryChange.bind(this);
        this.onUpdateDictionaryDialog = this.onUpdateDictionaryDialog.bind(this);
        this.onCloseEditDictionaryDialog = this.onCloseEditDictionaryDialog.bind(this);
        this.onSaveAddNewDictionaryDialog = this.onSaveAddNewDictionaryDialog.bind(this);
        this.onCloseAddNewDictionaryDialog = this.onCloseAddNewDictionaryDialog.bind(this);
        this.renderAddNewDictionaryDialog = this.renderAddNewDictionaryDialog.bind(this);
        this.renderDialog = this.renderDialog.bind(this);

        this.state = {
            editingDictionary: {},
            newDictionary: '',
            showAddNewDialog: false,
            showDialog: false,
            showEditingDialog: false,
            view: null
        }
    }

    onSubmit(e: any)
    {
         
    }


    onNewDictionaryChange(e: any)
    {
        this.setState({ newDictionary: e.target.value });
    }

    onUpdateDictionaryDialog(e: any)
    {
        this.setState({ showEditingDialog: false });
    }

    onCloseEditDictionaryDialog(e: any)
    {
        this.setState({ showEditingDialog: false });
    }

    onCloseAddNewDictionaryDialog(e: any)
    {
        this.setState({ showAddNewDialog: false });
    }


    onSaveAddNewDictionaryDialog(e: any)
    {
        let sender = this;
        let wbsDictionaryViewModel = {
            WbsGuid: this.props.wbsGuid,
            WbsDictionaryTitle: this.state.newDictionary,
            ProjectId: this.props.projectId,
            Who: "",//todo
        }

        Remote.postData("/pfs/wbsdictionary/add", wbsDictionaryViewModel, (data: any) => { sender.setState({ showDialog: false }); this.grid.refresh(); }, (error: any) => { window.Dialog.alert("Error", error); });
        
    }

    onAddNewDictionary(data: any)
    {
        this.setState({ view: IDialogContentType.WbsAddDictionaryView, showDialog: true });
    }

    renderEditDictionDialog()
    {
        return (
            <div className="container">
                <div className="row">
                    <div className="col-12">
                        <label htmlFor="title" className="col-12 row">WBS Dictionary Title</label>
                        <textarea name="title" className="col-12" rows={11} defaultValue={this.state.newDictionary} onChange={this.onNewDictionaryChange}></textarea>
                    </div>
                </div>
            </div>)
    }

    renderAddNewDictionaryDialog()
    {
        return (<div className="container">
            <div className="row">
                <div className="col-12">
                    <label htmlFor="title" className="col-12 row">WBS Dictionary Title</label>
                    <textarea name="title" className="col-12" rows={11} defaultValue={this.state.newDictionary} onChange={this.onNewDictionaryChange}></textarea>
                </div>
            </div>
        </div>)
    }

    renderDialog()
    {
        if (this.state.showDialog) {
            let sender = this;
            let prp: DialogProps = {
                height: '40%',
                width: '50%',
                title: 'Add WBS Dictionary',
                onClose: (e: any) =>
                {

                },
                closeIcon: true
            }
            let content: any = "";
            if (this.state.view == IDialogContentType.WbsAddDictionaryView) {
                content = this.renderAddNewDictionaryDialog();
            }
            else if (this.state.view == IDialogContentType.WbsEditDictionaryView) {
                content = this.renderEditDictionDialog();
            }
            return (<Dialog {...prp}>
                {content}
                <DialogActionsBar>
                    <button className="k-button k-primary" onClick={sender.onSaveAddNewDictionaryDialog}>Save</button>
                    <button className="k-button" onClick={sender.onCloseAddNewDictionaryDialog}>Cancel</button>
                </DialogActionsBar>
            </Dialog>)
        }
    }

    render()
    {
        let sender = this;
        let prp: DialogProps = {
            height: '70%',
            width: '50%',
            title: 'WBS Dictionary',
            onClose: (e: any) =>
            {
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
                    sender.setState({ editingDictionary: data, showDialog: true, newDictionary: data.dataItem.wbsDictionaryTitle, view: IDialogContentType.WbsEditDictionaryView })
                }
            },
            {
                text: "Delete", icon: "delete", action: (data: any, grid: any) =>
                {
                     
                }
            }]
        return (
            <div className="row">
                <div className="col-12">WBS Dictionary</div>
                <div className="col-12" id="wbsGridList">
                    <KendoGrid ref={(c) => { this.grid = c; }} key="wbsDictionaryGrid" showColumnMenu={false} showAddNewButton={false} showAdvanceSearchBox={false} rowMenus={rowMenu} gridMenu={gridMenu} addRecord={groupAddButton} identityField="id" fieldUrl={this.props.wbsDictionary.fieldUrl} exportFieldUrl={this.props.wbsDictionary.exportUrl} dataURL={this.props.wbsDictionary.dataUrl} parent="wbsGridList" />
                </div>
                {this.renderDialog()}
            </div>
        )
    }
}