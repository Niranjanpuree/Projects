"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const Remote_1 = require("../../../../Common/Remote/Remote");
const kendo_react_dialogs_1 = require("@progress/kendo-react-dialogs");
const KendoGrid_1 = require("../../../../Common/Grid/KendoGrid");
var IDialogContentType;
(function (IDialogContentType) {
    IDialogContentType[IDialogContentType["WbsAddView"] = 0] = "WbsAddView";
    IDialogContentType[IDialogContentType["WbsEditView"] = 1] = "WbsEditView";
    IDialogContentType[IDialogContentType["WbsAddDictionaryView"] = 2] = "WbsAddDictionaryView";
    IDialogContentType[IDialogContentType["WbsEditDictionaryView"] = 3] = "WbsEditDictionaryView";
    IDialogContentType[IDialogContentType["WbsViewDictionaryView"] = 4] = "WbsViewDictionaryView";
})(IDialogContentType = exports.IDialogContentType || (exports.IDialogContentType = {}));
class WbsDictionaryList extends React.Component {
    constructor(props) {
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
        };
    }
    onSubmit(e) {
    }
    onNewDictionaryChange(e) {
        this.setState({ newDictionary: e.target.value });
    }
    onUpdateDictionaryDialog(e) {
        this.setState({ showEditingDialog: false });
    }
    onCloseEditDictionaryDialog(e) {
        this.setState({ showEditingDialog: false });
    }
    onCloseAddNewDictionaryDialog(e) {
        this.setState({ showAddNewDialog: false });
    }
    onSaveAddNewDictionaryDialog(e) {
        let sender = this;
        let wbsDictionaryViewModel = {
            WbsGuid: this.props.wbsGuid,
            WbsDictionaryTitle: this.state.newDictionary,
            ProjectId: this.props.projectId,
            Who: "",
        };
        Remote_1.Remote.postData("/pfs/wbsdictionary/add", wbsDictionaryViewModel, (data) => { sender.setState({ showDialog: false }); this.grid.refresh(); }, (error) => { window.Dialog.alert("Error", error); });
    }
    onAddNewDictionary(data) {
        this.setState({ view: IDialogContentType.WbsAddDictionaryView, showDialog: true });
    }
    renderEditDictionDialog() {
        return (React.createElement("div", { className: "container" },
            React.createElement("div", { className: "row" },
                React.createElement("div", { className: "col-12" },
                    React.createElement("label", { htmlFor: "title", className: "col-12 row" }, "WBS Dictionary Title"),
                    React.createElement("textarea", { name: "title", className: "col-12", rows: 11, defaultValue: this.state.newDictionary, onChange: this.onNewDictionaryChange })))));
    }
    renderAddNewDictionaryDialog() {
        return (React.createElement("div", { className: "container" },
            React.createElement("div", { className: "row" },
                React.createElement("div", { className: "col-12" },
                    React.createElement("label", { htmlFor: "title", className: "col-12 row" }, "WBS Dictionary Title"),
                    React.createElement("textarea", { name: "title", className: "col-12", rows: 11, defaultValue: this.state.newDictionary, onChange: this.onNewDictionaryChange })))));
    }
    renderDialog() {
        if (this.state.showDialog) {
            let sender = this;
            let prp = {
                height: '40%',
                width: '50%',
                title: 'Add WBS Dictionary',
                onClose: (e) => {
                },
                closeIcon: true
            };
            let content = "";
            if (this.state.view == IDialogContentType.WbsAddDictionaryView) {
                content = this.renderAddNewDictionaryDialog();
            }
            else if (this.state.view == IDialogContentType.WbsEditDictionaryView) {
                content = this.renderEditDictionDialog();
            }
            return (React.createElement(kendo_react_dialogs_1.Dialog, Object.assign({}, prp),
                content,
                React.createElement(kendo_react_dialogs_1.DialogActionsBar, null,
                    React.createElement("button", { className: "k-button k-primary", onClick: sender.onSaveAddNewDictionaryDialog }, "Save"),
                    React.createElement("button", { className: "k-button", onClick: sender.onCloseAddNewDictionaryDialog }, "Cancel"))));
        }
    }
    render() {
        let sender = this;
        let prp = {
            height: '70%',
            width: '50%',
            title: 'WBS Dictionary',
            onClose: (e) => {
                sender.setState({ showDialog: false });
            },
            closeIcon: true
        };
        let gridMenu = [
            { text: 'Export to PDF', icon: 'pdf', action: (data, grid) => { grid.exportToPDF(); } },
            { text: 'Export to Excel', icon: 'excel', action: (data, grid) => { grid.exportToExcel(); } }
        ];
        let groupAddButton = {
            text: "Add Dictionary",
            icon: "plus-sm",
            redirect: false,
            updateView: true,
            action: function (data) {
                sender.onAddNewDictionary(data);
            }
        };
        let rowMenu = [
            {
                text: "Edit", icon: "pencil", action: (data, grid) => {
                    sender.setState({ editingDictionary: data, showDialog: true, newDictionary: data.dataItem.wbsDictionaryTitle, view: IDialogContentType.WbsEditDictionaryView });
                }
            },
            {
                text: "Delete", icon: "delete", action: (data, grid) => {
                }
            }
        ];
        return (React.createElement("div", { className: "row" },
            React.createElement("div", { className: "col-12" }, "WBS Dictionary"),
            React.createElement("div", { className: "col-12", id: "wbsGridList" },
                React.createElement(KendoGrid_1.KendoGrid, { ref: (c) => { this.grid = c; }, key: "wbsDictionaryGrid", showColumnMenu: false, showAddNewButton: false, showAdvanceSearchBox: false, rowMenus: rowMenu, gridMenu: gridMenu, addRecord: groupAddButton, identityField: "id", fieldUrl: this.props.wbsDictionary.fieldUrl, exportFieldUrl: this.props.wbsDictionary.exportUrl, dataURL: this.props.wbsDictionary.dataUrl, parent: "wbsGridList" })),
            this.renderDialog()));
    }
}
exports.WbsDictionaryList = WbsDictionaryList;
//# sourceMappingURL=WbsDictionaryList.js.map