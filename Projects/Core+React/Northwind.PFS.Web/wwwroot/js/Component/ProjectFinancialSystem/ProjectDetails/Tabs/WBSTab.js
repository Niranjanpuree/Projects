"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const Remote_1 = require("../../../../Common/Remote/Remote");
const KendoGrid_1 = require("../../../../Common/Grid/KendoGrid");
const kendo_react_dialogs_1 = require("@progress/kendo-react-dialogs");
class WBSTab extends React.Component {
    constructor(props) {
        super(props);
        this.grid = null;
        this.mainGrid = null;
        this.switchButton = {
            onLabel: "Exclude IWOs",
            offLabel: "Include IWOs",
            checked: false,
            action: (status) => {
                this.setState({ switchOn: status, dataUrl: this.props.urls.gridUrls.dataUrl + "&excludeIWOs=" + status }, this.mainGrid.refresh);
            }
        };
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
        };
    }
    onWBSDictionaryMoreLinkClicked(e) {
        let wbsNumber = e.wbsNumber;
        let projectNumber = e.projectNumber;
        this.setState({
            showDialog: true,
            projectNumber: projectNumber,
            wbsNumber: wbsNumber
        });
    }
    closeDialog(e) {
        this.setState({ showDialog: false }, this.refreshGrid);
    }
    onAddNewDictionary(data) {
        this.setState({ showAddNewDialog: true, newDictionary: '' });
    }
    onSaveAddNewDictionaryDialog(e) {
        let sender = this;
        let wbsDictionaryViewModel = {
            WbsNumber: this.state.wbsNumber,
            WbsDictionaryTitle: this.state.newDictionary,
            ProjectNumber: this.props.projectNumber
        };
        Remote_1.Remote.postData(this.props.urls.WbsDictionaryActionUrl.addUrl, wbsDictionaryViewModel, (data) => { sender.setState({ showAddNewDialog: false }); sender.grid.refresh(); }, (error) => { window.Dialog.alert(error); });
    }
    onCloseAddNewDictionaryDialog(e) {
        this.setState({ showAddNewDialog: false });
    }
    onNewDictionaryChange(e) {
        this.setState({ newDictionary: e.target.value });
    }
    onUpdateDictionaryDialog(e) {
        let sender = this;
        var wbsDictionaryRow = this.state.viewData.dataItem;
        let wbsDictionaryViewModel = {
            wbsDictionaryGuid: wbsDictionaryRow.wbsDictionaryGuid,
            wbsNumber: sender.state.wbsNumber,
            projectNumber: sender.state.projectNumber,
            WbsDictionaryTitle: sender.state.newDictionary,
        };
        Remote_1.Remote.postData(this.props.urls.WbsDictionaryActionUrl.editUrl, wbsDictionaryViewModel, (data) => { sender.setState({ showEditingDialog: false }); sender.grid.refresh(); }, (error) => { window.Dialog.alert(error); });
    }
    onCloseEditDictionaryDialog(e) {
        this.setState({ showEditingDialog: false });
    }
    onCloseWBSDetailsDialog(e) {
        this.setState({ showWBSDetailsDialog: false });
    }
    renderAddNewDictionDialog() {
        if (this.state.showAddNewDialog) {
            let sender = this;
            let prp = {
                height: '232px',
                width: '70%',
                title: 'Add WBS Dictionary',
                onClose: (e) => {
                    sender.setState({ showAddNewDialog: false });
                },
                closeIcon: true
            };
            return (React.createElement(kendo_react_dialogs_1.Dialog, Object.assign({}, prp),
                React.createElement("div", { className: "" },
                    React.createElement("label", { htmlFor: "title", className: "" }, "WBS Dictionary Title"),
                    React.createElement("textarea", { name: "title", className: "form-control", rows: 3, defaultValue: this.state.newDictionary, onChange: this.onNewDictionaryChange })),
                React.createElement(kendo_react_dialogs_1.DialogActionsBar, null,
                    React.createElement("button", { className: "k-button k-primary", onClick: this.onSaveAddNewDictionaryDialog }, "Save"),
                    React.createElement("button", { className: "k-button", onClick: this.onCloseAddNewDictionaryDialog }, "Cancel"))));
        }
    }
    renderEditDictionDialog() {
        if (this.state.showEditingDialog) {
            let sender = this;
            let prp = {
                height: '232px',
                width: '70%',
                title: 'Edit WBS Dictionary',
                onClose: (e) => {
                    sender.setState({ showEditingDialog: false });
                },
                closeIcon: true
            };
            return (React.createElement(kendo_react_dialogs_1.Dialog, Object.assign({}, prp),
                React.createElement("div", { className: "" },
                    React.createElement("label", { htmlFor: "title", className: "" }, "WBS Dictionary Title"),
                    React.createElement("textarea", { name: "title", className: "form-control", rows: 3, defaultValue: this.state.newDictionary, onChange: this.onNewDictionaryChange })),
                React.createElement(kendo_react_dialogs_1.DialogActionsBar, null,
                    React.createElement("button", { className: "k-button k-primary", onClick: this.onUpdateDictionaryDialog }, "Save"),
                    React.createElement("button", { className: "k-button", onClick: this.onCloseEditDictionaryDialog }, "Cancel"))));
        }
    }
    renderWBSDetailsDialog() {
        if (this.state.showWBSDetailsDialog) {
            let sender = this;
            let prp = {
                height: '40%',
                width: '50%',
                title: 'Edit WBS Dictionary',
                onClose: (e) => {
                    sender.setState({ showEditingDialog: false });
                },
                closeIcon: true
            };
            return (React.createElement(kendo_react_dialogs_1.Dialog, Object.assign({}, prp),
                React.createElement("div", { className: "container" },
                    React.createElement("div", { className: "row" },
                        React.createElement("div", { className: "col-12" },
                            React.createElement("h2", { className: "col-12 row" }, "Project/Mod Details"),
                            React.createElement("div", { className: "col-sm-12 form-group row" },
                                React.createElement("label", { className: "control-label control-label-read col-4" }, "Mod Number"),
                                React.createElement("div", { className: "form-value col-8" }, this.state.viewData.wbsNumber)),
                            React.createElement("div", { className: "col-sm-12 form-group row" },
                                React.createElement("label", { className: "control-label control-label-read col-4" }, "Title"),
                                React.createElement("div", { className: "form-value col-8" }, this.state.viewData.allowCharging)),
                            React.createElement("div", { className: "col-sm-12 form-group row" },
                                React.createElement("label", { className: "control-label control-label-read col-4" }, "Description"),
                                React.createElement("div", { className: "form-value col-8" }, this.state.viewData.wbsDescription))))),
                React.createElement(kendo_react_dialogs_1.DialogActionsBar, null,
                    React.createElement("button", { className: "k-button", onClick: this.onCloseWBSDetailsDialog }, "Cancel"))));
        }
    }
    renderDialog() {
        if (this.state.showDialog) {
            let sender = this;
            let prp = {
                height: '75%',
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
                        sender.setState({ viewData: data, showEditingDialog: true, newDictionary: data.dataItem.wbsDictionaryTitle });
                    }
                },
                {
                    text: "Delete", icon: "delete", action: (data, grid) => {
                        let ids = [];
                        ids.push(data.dataItem.wbsDictionaryGuid);
                        let senderData = data;
                        window.Dialog.confirm({
                            text: "Are you sure you want to  delete wbs dictionary?",
                            title: "Confirm",
                            ok: function (e) {
                                Remote_1.Remote.postData(sender.props.urls.WbsDictionaryActionUrl.deleteUrl, ids, (data) => { grid.refresh(); }, (error) => { });
                            },
                            cancel: function (e) {
                            }
                        });
                    }
                }
            ];
            return (React.createElement(kendo_react_dialogs_1.Dialog, Object.assign({}, prp),
                React.createElement("div", { className: "", id: "wbsGridList" },
                    React.createElement(KendoGrid_1.KendoGrid, { ref: (c) => this.grid = c, key: "wbsDictionaryGrid", showColumnMenu: false, showAddNewButton: false, showAdvanceSearchBox: false, rowMenus: rowMenu, gridMenu: gridMenu, addRecord: groupAddButton, identityField: "wbsDictionaryGuid", fieldUrl: this.props.urls.WbsDictionaryUrls.fieldUrl, exportFieldUrl: this.props.urls.WbsDictionaryUrls.exportUrl, dataURL: this.props.urls.WbsDictionaryUrls.dataUrl + "?projectNumber=" + this.state.projectNumber + "&wbsNumber=" + this.state.wbsNumber, parent: "wbsGridList" })),
                React.createElement(kendo_react_dialogs_1.DialogActionsBar, null,
                    React.createElement("button", { className: "k-button k-primary", onClick: this.closeDialog }, "Cancel"))));
        }
    }
    refreshGrid() {
        this.mainGrid.refresh();
    }
    render() {
        let sender = this;
        let gridMenu = [
            { text: 'Export to PDF', icon: 'pdf', action: (data, grid) => { grid.exportToPDF(); } },
            { text: 'Export to Excel', icon: 'excel', action: (data, grid) => { grid.exportToExcel(); } },
            {
                text: 'Add WBS', icon: 'plus', action: (data, grid) => {
                }
            }
        ];
        let rowMenu = [
            {
                text: "Add Dictionaries", conditions: [{ field: 'wbsDictionaryTitle.length', value: 0 }], icon: "plus", action: (data, grid) => {
                    let wbsNumber = data.dataItem.wbs;
                    let projectNumber = data.dataItem.projectNumber;
                    this.setState({
                        showDialog: true,
                        projectNumber: projectNumber,
                        wbsNumber: wbsNumber
                    });
                }
            },
            {
                text: "Edit Dictionaries", conditions: [{ field: 'wbsDictionaryTitle.length>0', value: true }], icon: "edit", action: (data, grid) => {
                    let wbsNumber = data.dataItem.wbs;
                    let projectNumber = data.dataItem.projectNumber;
                    this.setState({
                        showDialog: true,
                        projectNumber: projectNumber,
                        wbsNumber: wbsNumber
                    });
                }
            }
        ];
        return (React.createElement("div", { id: "wbsGrid", className: "wbs-grid" },
            React.createElement(KendoGrid_1.KendoGrid, { key: "wbsGrid", ref: (c) => { this.mainGrid = c; }, showColumnMenu: false, showAddNewButton: false, showAdvanceSearchBox: true, advancedSearchEntity: ["PFS-Wbs"], rowMenus: rowMenu, gridMenu: gridMenu, identityField: "number", fieldUrl: this.props.urls.gridUrls.fieldUrl, exportFieldUrl: this.props.urls.gridUrls.exportUrl, dataURL: this.state.dataUrl, onMoreLinkClicked: this.onWBSDictionaryMoreLinkClicked, switchButton: this.switchButton, parent: "wbsGrid" }),
            this.renderDialog(),
            this.renderAddNewDictionDialog(),
            this.renderEditDictionDialog()));
    }
}
exports.WBSTab = WBSTab;
//# sourceMappingURL=WBSTab.js.map