"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const KendoGroupableGrid_1 = require("../../../../Common/Grid/KendoGroupableGrid");
const kendo_react_dialogs_1 = require("@progress/kendo-react-dialogs");
class ProjectDetailsTab extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            showDialog: false,
            viewRecord: null
        };
        this.createRowMenu = this.createRowMenu.bind(this);
        this.closeDialog = this.closeDialog.bind(this);
        this.renderModDetailsDialog = this.renderModDetailsDialog.bind(this);
    }
    createRowMenu() {
        let sender = this;
        let groupRowMenu = [
            {
                text: "Details", icon: "folder-open", action: (data, grid) => {
                    sender.setState({ showDialog: true, viewRecord: data.dataItem });
                }
            }
        ];
        return groupRowMenu;
    }
    closeDialog(e) {
        this.setState({ showDialog: false });
    }
    renderModDetailsDialog() {
        if (this.state.showDialog) {
            let sender = this;
            let prp = {
                height: '50%',
                width: '70%',
                title: 'Add WBS Dictionary',
                onClose: (e) => {
                    sender.setState({ showDialog: false });
                },
                closeIcon: true
            };
            let attachments = this.state.viewRecord.attachments;
            return (React.createElement(kendo_react_dialogs_1.Dialog, Object.assign({}, prp),
                React.createElement("div", { className: "row" },
                    React.createElement("div", { className: "col-12" },
                        React.createElement("h4", { className: "" }, "Project/Mod Details"),
                        React.createElement("div", { className: "row" },
                            React.createElement("div", { className: "form-group col-sm-4" },
                                React.createElement("label", { className: "control-label control-label-read" }, "Mod Number"),
                                React.createElement("div", { className: "form-value" }, this.state.viewRecord.modNumber)),
                            React.createElement("div", { className: "form-group col-sm-4" },
                                React.createElement("label", { className: "control-label control-label-read" }, "Title"),
                                React.createElement("div", { className: "form-value" }, this.state.viewRecord.title)),
                            React.createElement("div", { className: "form-group col-sm-4" },
                                React.createElement("label", { className: "control-label control-label-read" }, "Award Date"),
                                React.createElement("div", { className: "form-value" }, formatUSDate(this.state.viewRecord.awardDate))),
                            React.createElement("div", { className: "form-group col-sm-4" },
                                React.createElement("label", { className: "control-label control-label-read" }, "POP Start Date"),
                                React.createElement("div", { className: "form-value" }, formatUSDate(this.state.viewRecord.popStartDate))),
                            React.createElement("div", { className: "form-group col-sm-4" },
                                React.createElement("label", { className: "control-label control-label-read" }, "POP End Date"),
                                React.createElement("div", { className: "form-value" }, formatUSDate(this.state.viewRecord.popEndDate))),
                            React.createElement("div", { className: "form-group col-sm-4" },
                                React.createElement("label", { className: "control-label control-label-read" }, "Award Amount"),
                                React.createElement("div", { className: "form-value" }, this.state.viewRecord.currency + " " + numberWithCommas(this.state.viewRecord.awardAmount))),
                            React.createElement("div", { className: "form-group col-sm-4" },
                                React.createElement("label", { className: "control-label control-label-read" }, "Funded Amount"),
                                React.createElement("div", { className: "form-value" }, this.state.viewRecord.currency + " " + numberWithCommas(this.state.viewRecord.fundedAmount))),
                            React.createElement("div", { className: "form-group col-sm-12" },
                                React.createElement("label", { className: "control-label control-label-read" }, "Description"),
                                React.createElement("div", { className: "form-value" }, this.state.viewRecord.description)),
                            React.createElement("div", { className: "form-group col-sm-12" },
                                React.createElement("label", { className: "control-label control-label-read" }, "Attachments"),
                                React.createElement("div", { className: "form-value" }, attachments.map((v, index) => {
                                    return (React.createElement("div", { key: "download" + index, className: "col-12 row" },
                                        React.createElement("a", { href: v.downloadLink, target: "_blank" }, v.title)));
                                })))))),
                React.createElement(kendo_react_dialogs_1.DialogActionsBar, null,
                    React.createElement("button", { className: "k-button k-primary", onClick: this.closeDialog }, "Cancel"))));
        }
    }
    render() {
        let gridMenu = [
            { text: 'Export to PDF', icon: 'pdf', action: (data, grid) => { grid.exportToPDF(); } },
            { text: 'Export to Excel', icon: 'excel', action: (data, grid) => { grid.exportToExcel(); } }
        ];
        let groupTotalFields = ["proj_v_cst_amt", "proj_f_cst_amt"];
        let groupRowMenu = this.createRowMenu();
        let rowIconSettings = new KendoGroupableGrid_1.RowIconSettings();
        rowIconSettings.column = "contractNumber";
        rowIconSettings.icons.push(new KendoGroupableGrid_1.IconSetting("isFavorite", true, "", "favorite-icon"));
        return (React.createElement("div", { id: "projectDetailsTab" },
            React.createElement("div", { className: "row" }),
            React.createElement(KendoGroupableGrid_1.KendoGroupableGrid, { groupTotalFields: groupTotalFields, rowMenus: groupRowMenu, currencySymbol: this.props.currency, showColumnMenu: false, rowIconSettings: rowIconSettings, gridMenu: gridMenu, showGridAction: true, showSearchBox: true, showAdvancedSearchDialog: true, advancedSearchEntity: ["PFS-ProjectMod"], identityField: "proj_mod_id", groupField: "projectNumber", columnUrl: this.props.fieldUrl, exportFieldUrl: this.props.exportUrl, dataUrl: this.props.dataUrl, container: "groupableGrid" }),
            this.renderModDetailsDialog()));
    }
}
exports.ProjectDetailsTab = ProjectDetailsTab;
//# sourceMappingURL=ProjectDetailsTab.js.map