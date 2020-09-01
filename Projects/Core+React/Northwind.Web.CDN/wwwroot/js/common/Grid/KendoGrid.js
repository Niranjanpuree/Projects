"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var kendo_react_grid_1 = require("@progress/kendo-react-grid");
var kendo_react_inputs_1 = require("@progress/kendo-react-inputs");
var CommandCell_1 = require("./CommandCell");
var GridDataLoader_1 = require("./GridDataLoader");
var Dialog_1 = require("../Dialog/Dialog");
var kendo_react_buttons_1 = require("@progress/kendo-react-buttons");
var react_router_dom_1 = require("react-router-dom");
var KendoGrid = /** @class */ (function (_super) {
    __extends(KendoGrid, _super);
    function KendoGrid(props) {
        var _this = _super.call(this, props) || this;
        _this.gridElement = React.createRef();
        KendoGrid.instance = _this;
        var buttons = [];
        buttons.push({
            primary: true,
            requireValidation: false,
            text: "",
            action: _this.printPDF
        });
        buttons.push({
            primary: false,
            requireValidation: false,
            text: "",
            action: _this.onDialogCancel
        });
        var dialogProps = {
            actionText: '',
            dialogTitle: '',
            buttons: buttons,
            dialogHeight: '50%',
            dialogWidth: '50%',
            getUrl: '',
            method: '',
            postUrl: '',
            uniqueKey: '',
            visible: false,
            register: _this.registerDialog
        };
        _this.state = {
            fields: [],
            sortField: '',
            pageable: {
                buttonCount: 5,
                info: true,
                type: 'numeric',
                pageSizes: true,
                previousNext: true
            },
            dataState: { take: 10, skip: 0, searchValue: '', dir: 'asc', sortField: '' },
            data: { data: [], total: 0 },
            dialogProps: dialogProps,
            redirect: false,
            submittableFormParam: {},
            persistentSelection: [],
            gridStyle: { height: '500px' },
            sort: []
        };
        _this.selectionChange = _this.selectionChange.bind(_this);
        _this.headerSelectionChange = _this.headerSelectionChange.bind(_this);
        _this.onSortChange = _this.onSortChange.bind(_this);
        _this.onPageChange = _this.onPageChange.bind(_this);
        _this.setDefaultSortField = _this.setDefaultSortField.bind(_this);
        _this.onPageSizeChange = _this.onPageSizeChange.bind(_this);
        _this.onSearchFieldKeyPress = _this.onSearchFieldKeyPress.bind(_this);
        _this.onSearchButtonClicked = _this.onSearchButtonClicked.bind(_this);
        _this.exportPDFClick = _this.exportPDFClick.bind(_this);
        _this.exportExcelClick = _this.exportExcelClick.bind(_this);
        _this.getCommandCell = _this.getCommandCell.bind(_this);
        _this.dataRecieved = _this.dataRecieved.bind(_this);
        _this.printPDF = _this.printPDF.bind(_this);
        _this.onDialogCancel = _this.onDialogCancel.bind(_this);
        _this.printExcel = _this.printExcel.bind(_this);
        _this.onGridMenuClick = _this.onGridMenuClick.bind(_this);
        _this.onAddActionComplete = _this.onAddActionComplete.bind(_this);
        _this.addRecordClicked = _this.addRecordClicked.bind(_this);
        _this.exportToPDF = _this.exportToPDF.bind(_this);
        _this.exportToExcel = _this.exportToExcel.bind(_this);
        _this.openSubmittableForm = _this.openSubmittableForm.bind(_this);
        _this._showSubmittableForm = _this._showSubmittableForm.bind(_this);
        _this.onSubmittableFormSuccess = _this.onSubmittableFormSuccess.bind(_this);
        _this.closeDialog = _this.closeDialog.bind(_this);
        _this.registerDialog = _this.registerDialog.bind(_this);
        _this.clearSelection = _this.clearSelection.bind(_this);
        _this.hasBeenSelected = _this.hasBeenSelected.bind(_this);
        _this.managePersitentSelection = _this.managePersitentSelection.bind(_this);
        _this.findSelectedIndex = _this.findSelectedIndex.bind(_this);
        _this._selectionChange = _this._selectionChange.bind(_this);
        var prop = _this.props;
        if (prop.gridStyle) {
            _this.setState({ gridStyle: prop.gridStyle });
        }
        return _this;
    }
    KendoGrid.prototype.registerDialog = function (e) {
        KendoGrid.instance.dialog = e;
    };
    KendoGrid.prototype.openDialog = function (param) {
        KendoGrid.instance.dialog.props = __assign({}, KendoGrid.instance.dialog.props, { dialogTitle: param.dialogTitle, buttons: param.buttons, getUrl: param.getUrl, getMethod: param.getMethod || 'get', postData: param.postData || [], method: param.method, postUrl: param.postUrl });
        this.setState({ dialogProps: KendoGrid.instance.dialog.props }, KendoGrid.instance.dialog.openForm);
    };
    KendoGrid.prototype.getSelectedItems = function () {
        var items = [];
        var rows = this.state.persistentSelection;
        for (var i = 0; i < rows.length; i++) {
            if (rows[i].selected == true) {
                items.push(rows[i]);
            }
        }
        return items;
    };
    KendoGrid.prototype.closeDialog = function () {
    };
    KendoGrid.prototype.onDialogCancel = function (e) {
        this.clearSelection();
        KendoGrid.instance.closeDialog();
    };
    KendoGrid.prototype.clearSelection = function () {
        this.setState({ persistentSelection: [] });
        var data = [];
        this.state.data.data.forEach(function (item) {
            item.selected = false;
            data.push(item);
        });
        this.setState({
            data: __assign({}, this.state.data, { data: data })
        }, this.forceUpdate);
    };
    KendoGrid.prototype.managePersitentSelection = function (item) {
        var existing = this.state.persistentSelection;
        if (item.selected) {
            if (this.hasBeenSelected(item) == false) {
                existing.push(item);
            }
        }
        else {
            if (this.hasBeenSelected(item) == true) {
                var index = this.findSelectedIndex(item);
                if (index > -1) {
                    existing.splice(parseInt(index + ""), 1);
                }
            }
        }
        this.setState({ persistentSelection: existing });
    };
    KendoGrid.prototype.selectionChange = function (event) {
        event.dataItem.selected = !event.dataItem.selected;
        this.managePersitentSelection(event.dataItem);
        this.forceUpdate();
    };
    KendoGrid.prototype._selectionChange = function (data) {
        this.managePersitentSelection(data);
        this.forceUpdate();
    };
    KendoGrid.prototype.headerSelectionChange = function (event) {
        var _this = this;
        var checked = event.syntheticEvent.target.checked;
        var gridData = this.state.data.data;
        gridData.forEach(function (item) {
            item.selected = checked;
            _this._selectionChange(item);
        });
        this.setState({
            data: __assign({}, this.state.data, { data: gridData })
        });
        this.forceUpdate();
    };
    KendoGrid.prototype.componentDidMount = function () {
        var _this = this;
        fetch(this.props.fieldUrl, {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: "GET"
        })
            .then(function (result) { return result.json(); })
            .then(function (result) {
            var data = [];
            for (var i in result) {
                if (i == "0") {
                    data.push({
                        fieldName: 'menu',
                        fieldLabel: ' ',
                        isSortable: false,
                        isFilterable: false,
                        orderIndex: 1,
                        showHeaderText: false
                    });
                }
                var row = result[i];
                row.orderIndex = data.length;
                data.push(row);
            }
            _this.setState({ fields: data });
            _this.setDefaultSortField(data);
        });
    };
    KendoGrid.prototype.setDefaultSortField = function (fields) {
        for (var fld in fields) {
            if (fields[fld].isDefaultSortField) {
                this.setState({ sortField: fields[fld].fieldName });
            }
        }
    };
    KendoGrid.prototype.onSortChange = function (e) {
        this.clearSelection();
        this.closeDialog();
        var evt = e;
        if (e.sort.length == 0) {
            evt.sort.push({
                field: this.state.sort[0].field,
                dir: 'asc'
            });
        }
        for (var s in evt.sort) {
            if (parseInt(s + "") > 0)
                break;
            var sort = evt.sort[s];
            this.setState({
                dataState: __assign({}, this.state.dataState, { sortField: sort.field, dir: sort.dir }),
                sort: evt.sort
            }, this.forceUpdate);
        }
    };
    KendoGrid.prototype.onPageChange = function (e) {
        this.setState({ dataState: { skip: e.page.skip, take: e.page.take, searchValue: this.state.dataState.searchValue, dir: this.state.dataState.dir, sortField: this.state.dataState.sortField } }, this.forceUpdate);
    };
    KendoGrid.prototype.onPageSizeChange = function (e) {
        this.setState({
            dataState: __assign({}, this.state.dataState, { take: e.target.value })
        }, this.forceUpdate);
    };
    KendoGrid.prototype.onSearchFieldKeyPress = function (e) {
        if (e.which == 13) {
            this.setState({
                dataState: __assign({}, this.state.dataState, { searchValue: e.target.value })
            }, this.forceUpdate);
        }
    };
    KendoGrid.prototype.onSearchButtonClicked = function (e) {
        this.setState({
            dataState: __assign({}, this.state.dataState, { searchValue: this.searchText.value })
        }, this.forceUpdate);
    };
    KendoGrid.prototype.getCommandCell = function (e) {
        return (React.createElement("td", null,
            React.createElement(CommandCell_1.CommandCell, { menuItems: this.props.rowMenus, dataItem: e.dataItem, parent: this })));
    };
    KendoGrid.prototype.renderFields = function () {
        var _this = this;
        var fields = [];
        var width = 150;
        if (this.props.gridWidth > 0) {
            width = (this.props.gridWidth - 120) / (this.state.fields.length - 1);
        }
        this.state.fields.forEach(function (e, index) {
            if (index == 0) {
                fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: "gridfield" + index, field: e.fieldName, title: e.fieldLabel, sortable: e.isSortable, filterable: e.isFilterable, orderIndex: e.orderIndex, width: "50px", cell: _this.getCommandCell }));
            }
            else {
                fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: "gridfield" + index, field: e.fieldName, title: e.fieldLabel, sortable: e.isSortable, filterable: e.isFilterable, orderIndex: e.orderIndex, width: width + "px" }));
            }
        });
        return fields;
    };
    KendoGrid.prototype.printPDF = function (e, columns) {
        KendoGrid.instance.closeDialog();
        var gridColumns = [];
        var data = [];
        for (var i = 0; i < columns.selectedOptions.length; i++) {
            var fieldIndex = columns.fieldNames.indexOf(columns.selectedOptions[i]);
            gridColumns.push({
                field: columns.selectedOptions[i],
                title: unescape(columns.fieldLabels[fieldIndex])
            });
        }
        var rows = this.getSelectedItems();
        for (var i = 0; i < rows.length; i++) {
            if (rows[i].selected == true) {
                data.push(rows[i]);
            }
        }
        var grid = $("<div id='pdfGrid'></div>");
        grid.kendoGrid({
            toolbar: ["pdf"],
            pdf: {
                allPages: true,
                avoidLinks: true,
                paperSize: "A4",
                margin: { top: "2cm", left: "1cm", right: "1cm", bottom: "1cm" },
                landscape: true,
                repeatHeaders: true,
                template: $("#page-template").html(),
                scale: 0.8
            },
            dataSource: data,
            height: 550,
            sortable: true,
            pageable: {
                refresh: true,
                pageSizes: true,
                buttonCount: 5
            },
            columns: gridColumns
        });
        grid.data("kendoGrid").saveAsPDF();
        this.clearSelection();
    };
    KendoGrid.prototype.exportToPDF = function (fieldUrl) {
        this.exportPDFClick(fieldUrl);
    };
    KendoGrid.prototype.exportToExcel = function (fieldUrl) {
        this.exportExcelClick(fieldUrl);
    };
    KendoGrid.prototype.exportPDFClick = function (fieldUrl) {
        var count = this.getSelectedItems().length;
        if (count == 0) {
            window.Dialog.alert("Please select at least a row to export to PDF.");
            return;
        }
        var buttons = [];
        buttons.push({
            primary: true,
            requireValidation: false,
            text: "Export",
            action: this.printPDF
        });
        buttons.push({
            primary: false,
            requireValidation: false,
            text: "Cancel",
            action: this.onDialogCancel
        });
        this.openDialog({
            dialogTitle: 'Export to PDF',
            getUrl: fieldUrl,
            buttons: buttons,
            method: 'get'
        });
    };
    KendoGrid.prototype.printExcel = function (e, columns) {
        KendoGrid.instance.closeDialog();
        var gridColumns = [];
        var data = [];
        for (var i = 0; i < columns.selectedOptions.length; i++) {
            var fieldIndex = columns.fieldNames.indexOf(columns.selectedOptions[i]);
            gridColumns.push({
                field: columns.selectedOptions[i],
                title: unescape(columns.fieldLabels[fieldIndex])
            });
        }
        var rows = this.getSelectedItems();
        for (var i = 0; i < rows.length; i++) {
            if (rows[i].selected == true) {
                data.push(rows[i]);
            }
        }
        var grid = $("<div id='pdfGrid'></div>");
        grid.kendoGrid({
            toolbar: ["pdf", "excel"],
            pdf: {
                allPages: true,
                avoidLinks: true,
                paperSize: "A4",
                margin: { top: "2cm", left: "1cm", right: "1cm", bottom: "1cm" },
                landscape: true,
                repeatHeaders: true,
                template: $("#page-template").html(),
                scale: 0.8
            },
            dataSource: data,
            height: 550,
            sortable: true,
            pageable: {
                refresh: true,
                pageSizes: true,
                buttonCount: 5
            },
            columns: gridColumns
        });
        grid.data("kendoGrid").saveAsExcel();
        this.clearSelection();
    };
    KendoGrid.prototype.exportExcelClick = function (fieldUrl) {
        var count = this.getSelectedItems().length;
        if (count == 0) {
            window.Dialog.alert("Please select at least a row to export to excel.");
            return;
        }
        var buttons = [];
        buttons.push({
            primary: true,
            requireValidation: false,
            text: "Export",
            action: this.printExcel
        });
        buttons.push({
            primary: false,
            requireValidation: false,
            text: "Cancel",
            action: this.onDialogCancel
        });
        this.openDialog({
            dialogTitle: 'Export to Excel',
            getUrl: fieldUrl,
            buttons: buttons,
            method: 'get'
        });
    };
    KendoGrid.prototype.hasBeenSelected = function (niddle) {
        for (var i in this.state.persistentSelection) {
            var nd = JSON.parse(JSON.stringify(niddle));
            var nd1 = JSON.parse(JSON.stringify(this.state.persistentSelection[i]));
            nd = __assign({}, nd, { selected: true });
            nd1 = __assign({}, nd1, { selected: true });
            if (JSON.stringify(nd1) == JSON.stringify(nd)) {
                return true;
            }
        }
        return false;
    };
    KendoGrid.prototype.findSelectedIndex = function (niddle) {
        for (var i in this.state.persistentSelection) {
            var nd = JSON.parse(JSON.stringify(niddle));
            var nd1 = JSON.parse(JSON.stringify(this.state.persistentSelection[i]));
            nd = __assign({}, nd, { selected: true });
            nd1 = __assign({}, nd1, { selected: true });
            if (JSON.stringify(nd1) == JSON.stringify(nd)) {
                return i;
            }
        }
        return -1;
    };
    KendoGrid.prototype.dataRecieved = function (e, loader) {
        var data = [];
        for (var i in e.data) {
            data.push(__assign({}, e.data[i], { selected: this.hasBeenSelected(e.data[i]) }));
        }
        this.setState({ data: { total: e.total, data: data } });
        this.dataLoader = loader;
    };
    KendoGrid.prototype.dataStateChange = function (e) {
        this.setState(__assign({}, this.state, { dataState: e.data }));
    };
    KendoGrid.prototype.onGridMenuClick = function (e) {
        var index = e.itemIndex;
        this.props.gridMenu[index].action(e, this);
    };
    KendoGrid.prototype.renderGridMenu = function () {
        var menus = [];
        for (var i in this.props.gridMenu) {
            menus.push(React.createElement("a", { key: "gridmenu{i}", className: "dropdown-item", href: "javascript:void(0)", onClick: this.onGridMenuClick }, this.props.gridMenu[i].text));
        }
        return menus;
    };
    KendoGrid.prototype.addRecordClicked = function (e) {
        if (this.props.addRecord.redirect && this.props.addRecord.redirect == true) {
            KendoGrid.instance.setState({ redirect: true }, KendoGrid.instance.forceUpdate);
        }
        else {
            var buttons = [];
            this.setState({ submittableFormParam: this.props.addRecord });
            for (var i in this.props.addRecord.buttons) {
                var b = this.props.addRecord.buttons[i];
                buttons.push({
                    primary: b.primary,
                    requireValidation: b.requireValidation,
                    text: b.text,
                    buttonIndex: i,
                    action: b.requireValidation ? this.onSubmittableFormSuccess : this.onDialogCancel
                });
            }
            this.openDialog({
                dialogTitle: this.props.addRecord.text,
                buttons: buttons,
                getUrl: this.props.addRecord.getUrl,
                method: this.props.addRecord.method,
                postUrl: this.props.addRecord.postUrl
            });
        }
    };
    KendoGrid.prototype.onAddActionComplete = function (e) {
    };
    KendoGrid.prototype.renderRedirect = function () {
        if (this.props.addRecord.redirect && this.props.addRecord.redirect == true && this.state.redirect) {
            return (React.createElement(react_router_dom_1.BrowserRouter, null,
                React.createElement(react_router_dom_1.Redirect, { to: this.props.addRecord.url })));
        }
    };
    KendoGrid.prototype.renderAddRecordButton = function () {
        if (this.props.addRecord && this.props.addRecord.text != '') {
            return (React.createElement(kendo_react_buttons_1.Button, { primary: true, onClick: this.addRecordClicked, icon: this.props.addRecord.icon }, this.props.addRecord.text));
        }
        return null;
    };
    KendoGrid.prototype.openSubmittableForm = function (param) {
        this._showSubmittableForm(param);
    };
    KendoGrid.prototype.onSubmittableFormSuccess = function (param, obj) {
        this.dataLoader.reloadData();
        if (param.button) {
            this.state.submittableFormParam.buttons[parseInt(param.button.buttonIndex)].action(param, obj);
        }
        this.forceUpdate();
    };
    KendoGrid.prototype._showSubmittableForm = function (param) {
        var buttons = [];
        this.setState({ submittableFormParam: param });
        for (var i in param.buttons) {
            var b = param.buttons[i];
            buttons.push({
                primary: b.primary,
                requireValidation: b.requireValidation,
                text: b.text,
                buttonIndex: i,
                action: b.requireValidation ? this.onSubmittableFormSuccess : this.onDialogCancel
            });
        }
        this.openDialog({
            dialogTitle: param.text,
            buttons: buttons,
            getUrl: param.getUrl,
            getMethod: param.getMethod || 'get',
            postData: param.postData || [],
            method: param.method,
            postUrl: param.postUrl
        });
    };
    KendoGrid.prototype.render = function () {
        var _this = this;
        return (React.createElement("div", null,
            React.createElement("div", { className: "row mt-12" },
                React.createElement("div", { className: "col text-right mr-1" },
                    React.createElement("div", { className: "dropdown float-left ml-1" },
                        React.createElement(kendo_react_inputs_1.Input, { autoFocus: true, onKeyPress: this.onSearchFieldKeyPress, placeholder: "Search", ref: function (c) { _this.searchText = c; } }),
                        "\u00A0",
                        React.createElement(kendo_react_buttons_1.Button, { type: "button", primary: true, onClick: this.onSearchButtonClicked }, "Search"),
                        "\u00A0",
                        React.createElement("a", { href: "javascript:void(0)", onClick: this.onSearchButtonClicked }, "Advanced Search"),
                        React.createElement("br", null)),
                    React.createElement("div", { className: "dropdown float-right ml-1" },
                        this.renderAddRecordButton(),
                        " \u00A0",
                        React.createElement(kendo_react_buttons_1.SplitButton, { text: "More Actions", items: this.props.gridMenu, onItemClick: this.onGridMenuClick })))),
            React.createElement(kendo_react_grid_1.Grid, __assign({ ref: function (c) { _this.gridElement = c; }, style: this.state.gridStyle, selectedField: "selected" }, this.state.data, this.state.dataState, { sortable: true, onSortChange: this.onSortChange, sort: this.state.sort, persistSelection: true, onSelectionChange: this.selectionChange, onHeaderSelectionChange: this.headerSelectionChange, onPageChange: this.onPageChange, pageable: this.state.pageable, onDataStateChange: this.dataStateChange }),
                React.createElement(kendo_react_grid_1.GridColumn, { field: "selected", width: "50px" }),
                this.renderFields()),
            React.createElement(GridDataLoader_1.GridDataLoader, { baseURL: this.props.dataURL, method: "get", dataState: this.state.dataState, onDataRecieved: this.dataRecieved }),
            this.renderRedirect(),
            React.createElement(Dialog_1.KendoDialog, __assign({}, this.state.dialogProps))));
    };
    KendoGrid.instance = null;
    return KendoGrid;
}(React.Component));
exports.KendoGrid = KendoGrid;
//# sourceMappingURL=KendoGrid.js.map