"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
require("core-js");
const React = require("react");
const kendo_react_grid_1 = require("@progress/kendo-react-grid");
const kendo_react_inputs_1 = require("@progress/kendo-react-inputs");
const CommandCell_1 = require("./CommandCell");
const Dialog_1 = require("../Dialog/Dialog");
const kendo_react_buttons_1 = require("@progress/kendo-react-buttons");
const GridColumnMenu_1 = require("./GridColumnMenu");
const kendo_data_query_1 = require("@progress/kendo-data-query");
const Remote_1 = require("../Remote/Remote");
const AdvancedSearchDialog_1 = require("../../Component/AdvancedSearch/AdvancedSearchDialog");
const Condition_1 = require("../../Component/Entities/Condition");
const SearchPills_1 = require("./SearchPills");
class IconSetting {
    constructor(field, value, icon, css) {
        this.field = field;
        this.value = value;
        this.icon = icon;
        this.css = css;
    }
}
exports.IconSetting = IconSetting;
class RowIconSettings {
    constructor() {
        this.icons = [];
    }
}
exports.RowIconSettings = RowIconSettings;
class KendoGroupableGrid extends React.Component {
    constructor(props) {
        super(props);
        this.dataStateChange = (event) => {
            let aggr = [];
            this.props.groupTotalFields.map((f) => {
                aggr.push({ field: f, aggregate: 'sum' });
            });
            aggr.push({ field: this.props.groupField, aggregate: 'sum' });
            let groups = [];
            groups.push({
                field: this.props.groupField, aggregates: aggr
            });
            this.setState({ gridState: Object.assign({}, this.state.gridState, { group: groups }) });
        };
        KendoGroupableGrid.instance = this;
        let groups = [];
        let searchValue = '';
        let initialSearchParams = [];
        let tmpAdditionalFilter = { value: '' };
        if (props.searchParameters) {
            if (props.searchParameters.searchValue) {
                searchValue = props.searchParameters.searchValue;
            }
            if (props.searchParameters.advancedSearchConditions) {
                initialSearchParams = props.searchParameters.advancedSearchConditions;
            }
            if (props.searchParameters.additionalFilter) {
                tmpAdditionalFilter = props.searchParameters.additionalFilter;
            }
        }
        if (this.props.additionalFilters && this.props.additionalFilters.length > 0 && tmpAdditionalFilter.value == '') {
            if (this.props.additionalFilters && this.props.additionalFilters.length > 0 && tmpAdditionalFilter.value == '') {
                tmpAdditionalFilter = this.props.additionalFilters.filter((v, i) => {
                    return v.default == true;
                })[0] || { name: '', value: '' };
            }
        }
        let aggr = [];
        this.props.groupTotalFields.map((f) => {
            aggr.push({ field: f, aggregate: 'sum' });
        });
        aggr.push({ field: this.props.groupField, aggregate: 'sum' });
        groups.push({
            field: this.props.groupField, aggregates: aggr
        });
        let dialogProps = {
            actionText: '',
            dialogTitle: '',
            buttons: new Array(),
            dialogHeight: '50%',
            dialogWidth: '50%',
            getUrl: '',
            getMethod: 'get',
            method: 'post',
            postUrl: '',
            uniqueKey: '',
            visible: false
        };
        let dState = { take: 10, skip: 0, searchValue: searchValue, dir: 'asc', sortField: '', additionalFilter: tmpAdditionalFilter.value, group: groups, data: [], total: 0 };
        if (this.props.externalDataState) {
            dState.take = this.props.externalDataState.take || dState.take;
            dState.skip = this.props.externalDataState.skip || dState.skip;
            dState.searchValue = this.props.externalDataState.searchValue || dState.searchValue;
            dState.dir = this.props.externalDataState.dir || dState.dir;
            dState.sortField = this.props.externalDataState.sortField || dState.sortField;
            dState.additionalFilter = this.props.externalDataState.additionalFilter || dState.additionalFilter;
        }
        this.state = {
            gridWidth: document.getElementById(this.props.container) ? document.getElementById(this.props.container).clientWidth : 0,
            clientHeight: document.getElementById(this.props.container) ? document.getElementById(this.props.container).clientHeight : 0,
            gridState: dState,
            gridStateTemplate: {
                searchValue: '',
                additionalFilter: '',
                sortField: '',
                dir: 'asc',
                skip: 0,
                take: 10,
                group: groups,
                data: [],
                total: 0
            },
            fields: [],
            parentWidth: 0,
            selectedItems: [],
            persistentSelection: [],
            redirectUrl: '',
            searchText: '',
            dialogProps: dialogProps,
            loading: false,
            currencySymbol: props.currencySymbol,
            allFields: [],
            isSwitchOn: this.props.switchButton && this.props.switchButton.length ? this.__getSwitchButtonChecked(this.props.switchButton) : this.props.switchButton ? this.props.switchButton.checked : false,
            dataLength: 0,
            sortable: this.props.sortable || false,
            advancedSearchConditions: initialSearchParams,
            additionalFilterValue: tmpAdditionalFilter,
            gridHeaderChecked: false,
            pagination: props.pagination === undefined ? { pageSizes: [10, 20, 50, 100] } : props.pagination,
            showAdvancedSearchDialog: this.props.showAdvancedSearchDialog ? this.props.showAdvancedSearchDialog : false,
            gridStyle: { height: this.props.gridHeight || '500px' },
        };
        this.loadGridData = this.loadGridData.bind(this);
        this.loadGridColumns = this.loadGridColumns.bind(this);
        this.getCommandCell = this.getCommandCell.bind(this);
        this.renderFields = this.renderFields.bind(this);
        this.renderSelectionField = this.renderSelectionField.bind(this);
        this.gridHeaderCellRenderer = this.gridHeaderCellRenderer.bind(this);
        this.headerCheckboxClicked = this.headerCheckboxClicked.bind(this);
        this.GridCellRenderer = this.GridCellRenderer.bind(this);
        this.expandChange = this.expandChange.bind(this);
        this.gridSelectionChange = this.gridSelectionChange.bind(this);
        this.refresh = this.refresh.bind(this);
        this.updateColumnVisibility = this.updateColumnVisibility.bind(this);
        this.onHeaderSelectionChange = this.onHeaderSelectionChange.bind(this);
        this.findSelectedIndex = this.findSelectedIndex.bind(this);
        this._selectionChange = this._selectionChange.bind(this);
        this.hasBeenSelected = this.hasBeenSelected.bind(this);
        this.managePersitentSelection = this.managePersitentSelection.bind(this);
        this.renderSearchBoxAndActions = this.renderSearchBoxAndActions.bind(this);
        this.renderSearchBox = this.renderSearchBox.bind(this);
        this.onSearchButtonClicked = this.onSearchButtonClicked.bind(this);
        this.onSearchFieldKeyPress = this.onSearchFieldKeyPress.bind(this);
        this.renderMoreButtons = this.renderMoreButtons.bind(this);
        this.addRecordClicked = this.addRecordClicked.bind(this);
        this.renderAddRecordButton = this.renderAddRecordButton.bind(this);
        this.onGridMenuClick = this.onGridMenuClick.bind(this);
        this.exportToPDF = this.exportToPDF.bind(this);
        this.exportExcelClick = this.exportExcelClick.bind(this);
        this.exportToExcel = this.exportToExcel.bind(this);
        this.exportPDFClick = this.exportPDFClick.bind(this);
        this.printExcel = this.printExcel.bind(this);
        this.printPDF = this.printPDF.bind(this);
        this.getSelectedItems = this.getSelectedItems.bind(this);
        this.onDialogCancel = this.onDialogCancel.bind(this);
        this.openDialog = this.openDialog.bind(this);
        this.clearSelection = this.clearSelection.bind(this);
        this.dataStateChange = this.dataStateChange.bind(this);
        this.renderLoadingPanel = this.renderLoadingPanel.bind(this);
        this.renderSwitchButton = this.renderSwitchButton.bind(this);
        this.onSwitchChange = this.onSwitchChange.bind(this);
        this.onPageChange = this.onPageChange.bind(this);
        this.onSortChange = this.onSortChange.bind(this);
        this.renderAdvancedSearch = this.renderAdvancedSearch.bind(this);
        this.onAdvancedSearchApply = this.onAdvancedSearchApply.bind(this);
        this.onConditionChange = this.onConditionChange.bind(this);
        this.getSearchParameters = this.getSearchParameters.bind(this);
        this.onSearchFillsDelete = this.onSearchFillsDelete.bind(this);
        this.renderAdditionalFilters = this.renderAdditionalFilters.bind(this);
        this.onAdditionalFilterChange = this.onAdditionalFilterChange.bind(this);
        this.onClearAllPills = this.onClearAllPills.bind(this);
        this.manageDataSelection = this.manageDataSelection.bind(this);
        this.loadAddDataForSelectAll = this.loadAddDataForSelectAll.bind(this);
        this.makeQueryString = this.makeQueryString.bind(this);
        this.getDataUrl = this.getDataUrl.bind(this);
    }
    __getSwitchButtonChecked(a) {
        let chked = [];
        a.map((v, i) => { chked.push(v.checked); });
        return chked;
    }
    getDataUrl() {
        return this.url;
    }
    getSearchParameters() {
        return {
            searchValue: this.state.gridState.searchValue,
            advancedSearchConditions: this.state.advancedSearchConditions,
            additionalFilter: this.state.additionalFilterValue
        };
    }
    exportToPDF() {
        this.exportPDFClick(this.props.exportFieldUrl);
    }
    exportToExcel() {
        this.exportExcelClick(this.props.exportFieldUrl);
    }
    exportPDFClick(fieldUrl) {
        let selected1 = this.state.persistentSelection.filter((v) => { return v.selected === true; });
        if (selected1.length == 0 && this.state.gridHeaderChecked === false) {
            window.Dialog.alert("Please select at least a row to export to excel.");
            return;
        }
        let buttons = [];
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
        this.setState({
            dialogProps: Object.assign({}, this.state.dialogProps, { dialogTitle: 'Export to PDF', getUrl: fieldUrl, buttons: buttons, method: 'get', visible: true })
        }, KendoGroupableGrid.instance.dialog.openForm);
    }
    printPDF(e, columns) {
        let sender = this;
        var gridColumns = [];
        var data = [];
        if (typeof (columns.selectedOptions) == "string") {
            let value = columns.selectedOptions;
            columns.selectedOptions = [];
            columns.selectedOptions.push(value);
        }
        if (!columns.selectedOptions) {
            window.Dialog.alert("Please select fields to export", "Notification");
            return;
        }
        for (var i = 0; i < columns.selectedOptions.length; i++) {
            let fieldIndex = columns.fieldNames.indexOf(columns.selectedOptions[i]);
            gridColumns.push({
                field: columns.selectedOptions[i],
                title: unescape(columns.fieldLabels[fieldIndex]),
                format: columns.fieldFormats[fieldIndex],
                filter: columns.fieldFilters[fieldIndex]
            });
        }
        this.getSelectedItems((result) => {
            let rows = result.result || result;
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].selected == true) {
                    data.push(rows[i]);
                }
            }
        });
        if (this.state.gridHeaderChecked) {
            this.loadAddDataForSelectAll((result) => {
                let data = result.result || result;
                let deSelected = this.state.persistentSelection.filter((v) => { return v.selected === false; });
                let nData = [];
                for (let i = 0; i < data.length; i++) {
                    let index = deSelected.findIndex((v) => {
                        return v[sender.props.identityField] == data[i][sender.props.identityField];
                    });
                    if (index < 0) {
                        nData.push(data[i]);
                    }
                }
                sender.downloadPDF(nData, gridColumns);
                sender.setState({ loading: false });
            });
        }
        else {
            this.downloadPDF(data, gridColumns);
            this.clearSelection();
            this.setState({ dialogProps: Object.assign({}, this.state.dialogProps, { visible: false }) });
        }
    }
    downloadPDF(data, gridColumns) {
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
    }
    clearSelection() {
        let isSelected = false;
        if (this.state.gridState && this.state.gridState.data && this.state.gridState.data.data) {
            for (let i = 0; i < this.state.gridState.data.data.length; i++) {
                for (let j = 0; j < this.state.gridState.data.data[i].items.length; j++) {
                    this.state.gridState.data.data[i].items[j].selected = isSelected;
                    this.managePersitentSelection(this.state.gridState.data.data[i].items[j]);
                }
            }
            this.setState({ gridState: Object.assign({}, this.state.gridState, { data: Object.assign({}, this.state.gridState.data, { data: this.state.gridState.data.data }) }), persistentSelection: [], gridHeaderChecked: false }, this.loadGridData);
        }
    }
    printExcel(e, columns) {
        let sender = this;
        var gridColumns = [];
        var data = [];
        if (typeof (columns.selectedOptions) == "string") {
            let value = columns.selectedOptions;
            columns.selectedOptions = [];
            columns.selectedOptions.push(value);
        }
        if (!columns.selectedOptions) {
            window.Dialog.alert("Please select fields to export", "Notification");
            return;
        }
        for (var i = 0; i < columns.selectedOptions.length; i++) {
            let fieldIndex = columns.fieldNames.indexOf(columns.selectedOptions[i]);
            gridColumns.push({
                field: columns.selectedOptions[i],
                title: unescape(columns.fieldLabels[fieldIndex]),
                format: columns.fieldFormats[fieldIndex],
                filter: columns.fieldFilters[fieldIndex]
            });
        }
        this.getSelectedItems((result) => {
            let rows = result.result || result;
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].selected == true) {
                    data.push(rows[i]);
                }
            }
            if (this.state.gridHeaderChecked) {
                this.loadAddDataForSelectAll((result) => {
                    let data = result.result;
                    let deSelected = this.state.persistentSelection.filter((v) => { return v.selected === false; });
                    let nData = [];
                    for (let i = 0; i < data.length; i++) {
                        let index = deSelected.findIndex((v) => {
                            return v[sender.props.identityField] == data[i][sender.props.identityField];
                        });
                        if (index < 0) {
                            nData.push(data[i]);
                        }
                    }
                    sender.downloadExcel(nData, gridColumns);
                    sender.setState({ loading: false });
                });
            }
            else {
                this.downloadExcel(data, gridColumns);
                this.clearSelection();
                this.setState({ dialogProps: Object.assign({}, this.state.dialogProps, { visible: false }) });
            }
        });
    }
    downloadExcel(data, gridColumns) {
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
    }
    loadAddDataForSelectAll(callback) {
        let dState = JSON.parse(JSON.stringify(this.state.gridState));
        dState.take = this.state.gridState.total;
        dState.skip = 0;
        let queryString = this.makeQueryString(dState);
        let caller = this;
        let separator = "?";
        if (this.props.dataUrl.indexOf("?") >= 0) {
            separator = "&";
        }
        let url = this.props.dataUrl + separator + queryString + "&t=" + (new Date()).getTime();
        if (this.state.advancedSearchConditions.length == 0) {
            Remote_1.Remote.get(url, (result) => {
                callback(result);
            }, (error) => { window.Dialog.alert(error, "Error"); });
        }
        else {
            Remote_1.Remote.postData(url, this.state.advancedSearchConditions, (result) => {
                callback(result);
            }, (error) => { window.Dialog.alert(error, "Error"); });
        }
    }
    getSelectedItems(callback = null) {
        if (this.state.gridHeaderChecked && callback) {
            this.loadAddDataForSelectAll((result) => {
                let data = [];
                for (let i = 0; i < result.result.length; i++) {
                    let index = this.state.persistentSelection.findIndex((v) => { return v.selected == false && v[this.props.identityField] == result.result[i][this.props.identityField]; });
                    if (index < 0) {
                        data.push(result.result[i]);
                    }
                }
                callback(data);
            });
        }
        else if (callback) {
            let items = [];
            let rows = this.state.persistentSelection;
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].selected == true) {
                    items.push(rows[i]);
                }
            }
            callback(items);
        }
        else {
            let items = [];
            let rows = this.state.persistentSelection;
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].selected == true) {
                    items.push(rows[i]);
                }
            }
            return items;
        }
    }
    makeQueryString(state) {
        let data = "";
        for (let i in state) {
            if (data != "")
                data += "&";
            data += i + "=" + eval("state." + i);
        }
        return data;
    }
    exportExcelClick(fieldUrl) {
        let selected1 = this.state.persistentSelection.filter((v) => { return v.selected === true; });
        if (selected1.length == 0 && this.state.gridHeaderChecked === false) {
            window.Dialog.alert("Please select at least a row to export to excel.");
            return;
        }
        let buttons = [];
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
        this.setState({
            dialogProps: Object.assign({}, this.state.dialogProps, { dialogTitle: 'Export to Excel', getUrl: fieldUrl, buttons: buttons, method: 'get', visible: true })
        }, KendoGroupableGrid.instance.dialog.openForm);
    }
    onDialogCancel(e) {
    }
    openDialog(options) {
        return (React.createElement(Dialog_1.KendoDialog, Object.assign({}, options)));
    }
    refresh(columnMenu) {
        this.clearSelection();
        this.loadGridColumns(columnMenu);
    }
    componentDidMount() {
        let width = $(this.props.container).parent().width();
        this.setState({ gridWidth: width ? width : 0, clientHeight: document.getElementById("groupableGrid") ? document.getElementById("groupableGrid").clientHeight : 0 });
        this.loadGridColumns(null);
    }
    loadGridColumns(columnMenu) {
        let sender = this;
        this.setState({ loading: true }, () => {
            this.renderLoadingPanel();
            Remote_1.Remote.get(this.props.columnUrl, (result) => {
                let data = [];
                result = result.sort((a, b) => parseFloat(a.orderIndex) - parseFloat(b.orderIndex));
                sender.setState({ allFields: result });
                for (let i in result) {
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
                    let row = result[i];
                    row.orderIndex = data.length;
                    row.show = (this.props.hideColumns === undefined || this.props.hideColumns.indexOf(row.fieldName) === -1) && row.visibleToGrid == true;
                    data.push(row);
                }
                sender.setState({ fields: data }, sender.forceUpdate);
                sender.loadGridData(columnMenu);
            }, (err) => {
                window.Dialog.alert(err);
            });
        });
    }
    getCommandCell(e) {
        if (!this.props.rowMenus) {
            return;
        }
        return (React.createElement("td", { style: { overflow: 'visible' } },
            React.createElement(CommandCell_1.CommandCell, { menuItems: this.props.rowMenus, dataItem: e.dataItem, parent: this })));
    }
    updateColumnVisibility(columns) {
        let arr = [];
        arr.push({
            fieldName: 'menu',
            fieldLabel: ' ',
            isSortable: false,
            isFilterable: false,
            orderIndex: 1,
            showHeaderText: false
        });
        for (let i in columns) {
            columns[i].visibleToGrid = columns[i].show;
            columns[i].orderIndex = parseInt(i) + 1;
            arr.push(columns[i]);
        }
        arr = arr.sort((a, b) => parseFloat(a.orderIndex) - parseFloat(b.orderIndex));
        this.setState({ fields: arr });
    }
    renderFields() {
        let fields = [];
        let deductionColumnWidth = 150;
        let width = 150 + "";
        let columns = this.state.fields.filter(c => c.show == true);
        /*
        if (this.state.gridWidth > 0) {
            width = (this.state.gridWidth - deductionColumnWidth) / (columns.length)
        }
        if (this.state.parentWidth > 0) {
            width = (this.state.parentWidth - deductionColumnWidth) / (columns.length)
        }
        if (width < 150)
            width = 150;*/
        columns = this.state.fields.filter(c => c.show == true || c.fieldName == 'menu');
        let totalColumns = deductionColumnWidth === 150 ? columns.length - 1 : columns.length;
        columns.forEach((e, index) => {
            let percent = e.columnWidth ? e.columnWidth : (100 / totalColumns);
            width = this.getPixel(percent, deductionColumnWidth) + "";
            if (e.columnMinimumWidth > width) {
                width = e.columnMinimumWidth;
            }
            if (index == 0 && this.props.rowMenus) {
                fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: index, field: e.fieldName, title: e.fieldLabel, sortable: e.isSortable, filterable: e.isFilterable, orderIndex: e.orderIndex, width: "50px", cell: this.getCommandCell, className: e.gridColumnCss, headerClassName: e.gridColumnCss }));
            }
            else {
                if (index == columns.length - 1 && e.show && !this.props.showColumnMenu == false) {
                    if (e.clickable && this.props.itemNavigationUrl) {
                        fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: index, field: e.fieldName, filter: e.type, format: e.format, title: e.fieldLabel, sortable: e.isSortable, filterable: e.isFilterable, orderIndex: e.orderIndex, width: width + "px", className: e.gridColumnCss, headerClassName: e.gridColumnCss, columnMenu: props => React.createElement(GridColumnMenu_1.GridColumnMenu, Object.assign({ gridColumns: columns }, props, { grid: this, columns: this.state.allFields })), cell: (props) => {
                                return (React.createElement("td", null,
                                    React.createElement("a", { href: this.props.itemNavigationUrl + props.dataItem[this.props.identityField] }, props.dataItem[props.field])));
                            } }));
                    }
                    else {
                        if (e.format === "{0:d}") {
                            fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: index, field: e.fieldName, title: e.fieldLabel, filter: e.type, format: e.format, sortable: e.isSortable, filterable: e.isFilterable, orderIndex: e.orderIndex, width: width + "px", className: e.gridColumnCss, headerClassName: e.gridColumnCss, columnMenu: props => React.createElement(GridColumnMenu_1.GridColumnMenu, Object.assign({ gridColumns: columns }, props, { grid: this, columns: this.state.allFields })), cell: (prop) => {
                                    const value = prop.dataItem[prop.field];
                                    return (React.createElement("td", { className: e.gridColumnCss },
                                        " ",
                                        formatUSDate(value, "en")));
                                } }));
                        }
                        else if (e.format === "{0:c}") {
                            fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: index, field: e.fieldName, title: e.fieldLabel, filter: e.type, format: e.format, sortable: e.isSortable, filterable: e.isFilterable, orderIndex: e.orderIndex, width: width + "px", className: e.gridColumnCss, headerClassName: e.gridColumnCss, columnMenu: props => React.createElement(GridColumnMenu_1.GridColumnMenu, Object.assign({ gridColumns: columns }, props, { grid: this, columns: this.state.allFields })), cell: (prop) => {
                                    const value = prop.dataItem[prop.field];
                                    let currency = prop.dataItem['currency'] || '';
                                    let money = prop.dataItem[prop.field] || 0;
                                    if (prop.rowType === 'groupFooter') {
                                        money = prop.dataItem.aggregates[prop.field].sum;
                                        currency = prop.dataItem.items[0]['currency'];
                                    }
                                    return (React.createElement("td", { className: e.gridColumnCss },
                                        " ",
                                        currency + ' ' + numberWithCommas(money.toFixed(2))));
                                } }));
                        }
                        else {
                            fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: index, field: e.fieldName, title: e.fieldLabel, filter: e.type, format: e.format, sortable: e.isSortable, filterable: e.isFilterable, orderIndex: e.orderIndex, width: width + "px", className: e.gridColumnCss, headerClassName: e.gridColumnCss, columnMenu: props => React.createElement(GridColumnMenu_1.GridColumnMenu, Object.assign({ gridColumns: columns }, props, { grid: this, columns: this.state.allFields })), cell: (prop) => {
                                    let css = e.gridColumnCss + " ";
                                    const value = prop.dataItem[prop.field];
                                    let money = prop.dataItem[prop.field];
                                    if (prop.rowType === 'groupFooter' && prop.dataItem.aggregates[prop.field] && prop.dataItem.aggregates[prop.field].sum && !isNaN(money)) {
                                        money = prop.dataItem.aggregates[prop.field].sum;
                                    }
                                    return (React.createElement("td", { className: css },
                                        " ",
                                        money));
                                } }));
                        }
                    }
                }
                else if (e.show) {
                    if (e.clickable && this.props.itemNavigationUrl) {
                        fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: index, field: e.fieldName, title: e.fieldLabel, filter: e.type, format: e.format, sortable: e.isSortable, filterable: e.isFilterable, orderIndex: e.orderIndex, width: width + "px", className: e.gridColumnCss, headerClassName: e.gridColumnCss, cell: (props) => {
                                return (React.createElement("td", null,
                                    React.createElement("a", { href: this.props.itemNavigationUrl + props.dataItem[this.props.identityField] }, props.dataItem[props.field])));
                            } }));
                    }
                    else {
                        if (e.format === "{0:c}") {
                            fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: index, field: e.fieldName, title: e.fieldLabel, filter: e.type, format: e.format, sortable: e.isSortable, filterable: e.isFilterable, orderIndex: e.orderIndex, width: width + "px", className: e.gridColumnCss, headerClassName: e.gridColumnCss, cell: (prop) => {
                                    const value = prop.dataItem[prop.field];
                                    let currency = prop.dataItem['currency'] || '';
                                    let money = prop.dataItem[prop.field] || 0;
                                    if (prop.rowType === 'groupFooter') {
                                        money = prop.dataItem.aggregates[prop.field].sum || 0;
                                        currency = prop.dataItem.items[0]['currency'] || '';
                                    }
                                    return (React.createElement("td", { className: e.gridColumnCss },
                                        " ",
                                        currency + ' ' + numberWithCommas(money.toFixed(2))));
                                } }));
                        }
                        else if (e.format === "{0:d}") {
                            fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: index, field: e.fieldName, title: e.fieldLabel, filter: e.type, format: e.format, sortable: e.isSortable, filterable: e.isFilterable, orderIndex: e.orderIndex, width: width + "px", className: e.gridColumnCss, headerClassName: e.gridColumnCss, cell: (prop) => {
                                    const value = prop.dataItem[prop.field];
                                    return (React.createElement("td", { className: e.gridColumnCss },
                                        " ",
                                        formatUSDate(value, "en")));
                                } }));
                        }
                        else {
                            fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: index, field: e.fieldName, title: e.fieldLabel, filter: e.type, format: e.format, sortable: e.isSortable, filterable: e.isFilterable, orderIndex: e.orderIndex, width: width + "px", className: e.gridColumnCss, headerClassName: e.gridColumnCss, cell: (props) => {
                                    let css = e.gridColumnCss + " ";
                                    if (this.props.rowIconSettings && props.field == this.props.rowIconSettings.column) {
                                        this.props.rowIconSettings.icons.forEach((v) => {
                                            if (props.dataItem[v.field] === v.value) {
                                                css += " " + v.css;
                                            }
                                        });
                                    }
                                    let value = props.dataItem[props.field];
                                    if (value === true) {
                                        value = "Yes";
                                    }
                                    else if (value === false) {
                                        value = "No";
                                    }
                                    if (props.rowType === 'groupFooter' && props.dataItem.aggregates[props.field] && props.dataItem.aggregates[props.field].sum) {
                                        value = props.dataItem.aggregates[props.field].sum;
                                        if (isNaN(value)) {
                                            value = props.dataItem.value;
                                        }
                                    }
                                    return (React.createElement("td", { className: css }, value));
                                } }));
                        }
                    }
                }
            }
        });
        return fields;
    }
    onPageChange(e) {
        this.setState({ gridState: Object.assign({}, this.state.gridState, { skip: e.page.skip, take: e.page.take, searchValue: this.state.gridState.searchValue, dir: this.state.gridState.dir, sortField: this.state.gridState.sortField }) }, this.loadGridData);
    }
    loadGridData(columnMenu = undefined) {
        this.setState({ loading: true });
        let sender = this;
        let sign = "?";
        if (this.props.dataUrl.indexOf("?") >= 0) {
            sign = "&";
        }
        let additional = "";
        if (this.state.gridState.additionalFilter) {
            additional = "&additionalFilter=" + this.state.gridState.additionalFilter;
        }
        if (this.state.gridState.method === 'post' || (this.props.searchParameters && this.props.searchParameters.advancedSearchConditions && this.props.searchParameters.advancedSearchConditions.length > 0)) {
            let queryString = sign + "searchValue=" + this.state.gridState.searchValue + "&take=" + this.state.gridState.take + "&skip=" + this.state.gridState.skip + "&sortField=" + this.state.gridState.sortField + "&dir=" + this.state.gridState.dir + additional;
            let postValue = this.state.advancedSearchConditions;
            this.url = this.props.dataUrl + queryString;
            Remote_1.Remote.postData(this.url, postValue, (data) => {
                let gState = sender.state.gridStateTemplate;
                gState.sortField = sender.state.gridState.sortField;
                gState.dir = sender.state.gridState.dir;
                gState.take = data.length || data.result.length;
                let result = this.manageDataSelection(data.result);
                let grdData = kendo_data_query_1.process(result, gState);
                if (sender.props.onGridLoaded) {
                    sender.props.onGridLoaded(data.result);
                }
                sender.setState({ gridState: Object.assign({}, sender.state.gridState, { data: grdData, total: data.count }), dataLength: data.result.length, loading: false });
                if (columnMenu) {
                    columnMenu.refresh();
                }
            }, (err) => {
                window.Dialog.alert(err);
            });
        }
        else {
            let queryString = sign + "searchValue=" + this.state.gridState.searchValue + "&take=" + this.state.gridState.take + "&skip=" + this.state.gridState.skip + "&sortField=" + this.state.gridState.sortField + "&dir=" + this.state.gridState.dir + additional;
            this.url = this.props.dataUrl + queryString;
            Remote_1.Remote.get(this.url, (data) => {
                let gState = sender.state.gridStateTemplate;
                gState.sortField = sender.state.gridState.sortField;
                gState.dir = sender.state.gridState.dir;
                gState.take = data.length;
                let result = this.manageDataSelection(data.result);
                result.map((v, i) => {
                    v.sortIndex = i;
                });
                let grdData = kendo_data_query_1.process(result, gState);
                grdData.data.sort((a, b) => {
                    let aindex = result.findIndex((v, i) => { if (v[sender.props.groupField] == a.value)
                        return i; });
                    let bindex = result.findIndex((v, i) => { if (v[sender.props.groupField] == b.value)
                        return i; });
                    return aindex < bindex ? -1 : 1;
                });
                sender.setState({ gridState: Object.assign({}, sender.state.gridState, { data: grdData, total: data.count }), dataLength: data.result.length, loading: false });
                if (columnMenu) {
                    columnMenu.refresh();
                }
                if (sender.props.onGridLoaded) {
                    sender.props.onGridLoaded(data.result);
                }
            }, (err) => {
                window.Dialog.alert(err);
            });
        }
    }
    manageDataSelection(data) {
        for (let i in data) {
            let index = this.state.persistentSelection.findIndex((v) => { return v[this.props.identityField] === data[i][this.props.identityField]; });
            if (index < 0) {
                if (this.state.gridHeaderChecked) {
                    data[i].selected = true;
                }
                else {
                    data[i].selected = false;
                }
            }
            else {
                data[i].selected = this.state.persistentSelection[index].selected;
            }
        }
        return data;
    }
    renderSelectionField() {
        return (React.createElement(kendo_react_grid_1.GridColumn, { field: "selected", width: "50px" }));
    }
    headerCheckboxClicked(e) {
    }
    GridCellRenderer(td, cellProps) {
        if (cellProps.rowType === 'groupFooter') {
            let index = this.props.groupTotalFields.findIndex((v, index) => { return v === cellProps.field; });
            if (index > 0) {
                let currency = this.state.currencySymbol;
                if (!currency) {
                    for (var i = 0; i < cellProps.dataItem.items.length; i++) {
                        if (cellProps.dataItem.items[i].currency) {
                            currency = cellProps.dataItem.items[i].currency;
                        }
                    }
                }
                currency = currency ? currency : '';
                return (React.createElement("td", null));
            }
            else if (cellProps.field === this.props.groupTotalLabelField) {
                return (React.createElement("td", null, "Total"));
            }
            return (React.createElement("td", null));
        }
        else if (cellProps.rowType === 'groupHeader') {
            if (cellProps.field === 'value' && this.props.showGroupHeader !== false) {
                return td;
            }
            else if (cellProps.field === 'selected') {
                return (React.createElement("td", null));
            }
            else if (cellProps.field === 'contactNumber') {
                return (React.createElement("td", null));
            }
            else if (cellProps.field === 'fundedAmount') {
                return (React.createElement("td", null));
            }
            else if (this.props.showGroupHeader === false) {
                return (React.createElement("td", null));
            }
        }
        else if (cellProps.rowType == "data") {
            if (this.props.rowIconSettings) {
                let favoriteIconCSS = "";
                if (cellProps.field === this.props.rowIconSettings.column) {
                    if (cellProps.columnIndex == 3) {
                        if (cellProps.dataItem['isFavorite']) {
                            favoriteIconCSS = "favorite_icon";
                        }
                    }
                    for (let i in this.props.rowIconSettings.icons) {
                        let css = this.props.rowIconSettings.icons[i].css + " " + favoriteIconCSS;
                        if (cellProps.dataItem[this.props.rowIconSettings.icons[i].field] === this.props.rowIconSettings.icons[i].value) {
                            return (React.createElement("td", { className: css },
                                this.props.rowIconSettings.icons[i].icon,
                                " ",
                                cellProps.dataItem[cellProps.field]));
                        }
                    }
                }
            }
        }
        return td;
    }
    expandChange(event) {
        event.dataItem[event.target.props.expandField] = event.value;
        this.setState({
            gridState: Object.assign({}, this.state.gridState, { data: Object.assign({}, this.state.gridState.data), dataState: this.state.gridState.dataaState })
        });
    }
    componentWillUpdate() {
        if (this.state.gridWidth != document.getElementById(this.props.container).clientWidth) {
            this.setState({ gridWidth: document.getElementById(this.props.container).clientWidth });
            return true;
        }
        return false;
    }
    gridHeaderCellRenderer(e, e1) {
        if (e1.field == "selected") {
            return (React.createElement("label", null));
        }
        return e;
    }
    gridSelectionChange(e) {
        let isSelected = e.syntheticEvent.target.checked;
        for (let i = 0; i < this.state.gridState.data.data.length; i++) {
            for (let j = 0; j < this.state.gridState.data.data[i].items.length; j++) {
                if (eval("this.state.gridState.data.data[" + i + "].items[" + j + "]." + this.props.identityField) == eval("e.dataItem." + this.props.identityField)) {
                    this.state.gridState.data.data[i].items[j].selected = isSelected;
                    this.managePersitentSelection(this.state.gridState.data.data[i].items[j]);
                }
            }
        }
        this.setState({ gridState: Object.assign({}, this.state.gridState, { data: Object.assign({}, this.state.gridState.data, { data: this.state.gridState.data.data }) }) });
    }
    findSelectedIndex(niddle) {
        for (let i in this.state.persistentSelection) {
            let nd = JSON.parse(JSON.stringify(niddle));
            let nd1 = JSON.parse(JSON.stringify(this.state.persistentSelection[i]));
            nd = Object.assign({}, nd, { selected: true });
            nd1 = Object.assign({}, nd1, { selected: true });
            if (JSON.stringify(nd1) == JSON.stringify(nd)) {
                return parseInt(i);
            }
        }
        return -1;
    }
    onHeaderSelectionChange(event) {
        const checked = event.syntheticEvent.target.checked;
        let gridData = this.state.gridState.data.data;
        gridData.forEach((aggregate) => {
            aggregate.items.forEach((item) => {
                item.selected = checked;
            });
        });
        this.setState({
            gridHeaderChecked: event.syntheticEvent.target.checked,
            gridState: Object.assign({}, this.state.gridState, { data: Object.assign({}, this.state.gridState.data, { data: gridData }) }),
            persistentSelection: []
        });
    }
    _selectionChange(data) {
        this.managePersitentSelection(data);
    }
    managePersitentSelection(item) {
        //let existing = this.state.persistentSelection;
        //if (item.selected) {
        //    if (this.hasBeenSelected(item) == false) {
        //        existing.push(item)
        //    }
        //}
        //else {
        //    if (this.hasBeenSelected(item) == true) {
        //        let index = this.findSelectedIndex(item)
        //        if (index > -1) {
        //            existing.splice(parseInt(index + ""), 1);
        //        }
        //    }
        //}
        //this.setState({ persistentSelection: existing });
        let existing = this.state.persistentSelection || [];
        let index = this.findSelectedIndex(item);
        if (index === -1) {
            existing.push(item);
        }
        else {
            existing[index] = item;
        }
        this.setState({ persistentSelection: existing });
    }
    hasBeenSelected(niddle) {
        for (let i in this.state.persistentSelection) {
            let nd = JSON.parse(JSON.stringify(niddle));
            let nd1 = JSON.parse(JSON.stringify(this.state.persistentSelection[i]));
            nd = Object.assign({}, nd, { selected: true });
            nd1 = Object.assign({}, nd1, { selected: true });
            if (JSON.stringify(nd1) == JSON.stringify(nd)) {
                return true;
            }
        }
        return false;
    }
    onGridMenuClick(e) {
        let index = e.itemIndex;
        this.props.gridMenu[index].action(e, this);
    }
    onSwitchChange(e) {
        if (this.props.switchButton.length) {
            let switches = this.state.isSwitchOn;
            if (!this.state.isSwitchOn.length) {
                switches = [];
                for (let i in this.props.switchButton) {
                    switches.push(false);
                }
            }
            for (let i in this.props.switchButton) {
                if (this.props.switchButton[i].offLabel === e.target.props.offLabel) {
                    switches[i] = e.target.value;
                    this.setState({ isSwitchOn: switches });
                    this.props.switchButton[i].action(e.target.value);
                }
            }
        }
        else {
            this.setState({ isSwitchOn: e.target.value });
            this.props.switchButton.action(e.target.value);
        }
    }
    renderSwitchButton() {
        if (this.props.switchButton != undefined) {
            if (this.props.switchButton.length) {
                let switchButtons = this.props.switchButton;
                let items = [];
                switchButtons.map((v, i) => {
                    items.push(React.createElement("div", { key: "switch" + i, className: "switch-btn" },
                        React.createElement(kendo_react_inputs_1.Switch, { offLabel: v.offLabel, onLabel: v.onLabel, className: "switchButton", checked: this.state.isSwitchOn[i], onChange: this.onSwitchChange }),
                        this.state.isSwitchOn ? v.onLabel : v.offLabel));
                });
                return items;
            }
            else {
                return (React.createElement("div", { className: "switch-btn" },
                    React.createElement(kendo_react_inputs_1.Switch, { offLabel: this.props.switchButton.offLabel, onLabel: this.props.switchButton.onLabel, className: "switchButton", checked: this.state.isSwitchOn, onChange: this.onSwitchChange }),
                    this.state.isSwitchOn ? this.props.switchButton.onLabel : this.props.switchButton.offLabel));
            }
        }
    }
    renderAddRecordButton() {
        if (this.props.addRecord && this.props.addRecord.text != '') {
            return (React.createElement(kendo_react_buttons_1.Button, { primary: true, onClick: this.addRecordClicked }, this.props.addRecord.text)); //icon={this.props.addRecord.icon}
        }
        return null;
    }
    addRecordClicked(e) {
        this.props.addRecord.action(e, this);
    }
    renderMoreButtons() {
        if (this.props.showGridAction == undefined || this.props.showGridAction == true) {
            return (React.createElement("div", { className: "text-right" },
                this.renderSwitchButton(),
                this.renderAddRecordButton(),
                " \u00A0",
                React.createElement(kendo_react_buttons_1.SplitButton, { text: "Actions", className: "actions-dropdown", items: this.props.gridMenu, onItemClick: this.onGridMenuClick })));
        }
    }
    onSearchFieldKeyPress(e) {
        if (e.which == 13) {
            this.setState({
                gridState: Object.assign({}, this.state.gridState, { searchText: this.searchText.value })
            }, this.loadGridData);
        }
    }
    onSearchButtonClicked(e) {
        this.setState({
            gridState: Object.assign({}, this.state.gridState, { searchValue: this.searchText.value })
        }, this.loadGridData);
    }
    renderSearchBox() {
        return (React.createElement("div", { className: "search-form-r" },
            this.renderAdditionalFilters(),
            React.createElement("div", { className: "input-group mb-3 mb-sm-0" },
                React.createElement(kendo_react_inputs_1.Input, { autoFocus: true, onKeyPress: this.onSearchFieldKeyPress, placeholder: "Search", ref: (c) => { this.searchText = c; }, className: "form-control" }),
                React.createElement("div", { className: "input-group-append" },
                    React.createElement("div", { className: "input-group-text" },
                        React.createElement("a", { href: "javascript:void(0)", onClick: this.onSearchButtonClicked, className: "k-icon k-i-search" }, "\u00A0")))),
            this.renderAdvancedSearch(),
            React.createElement("div", { className: "clearfix" })));
    }
    onSearchFillsDelete(condition) {
        if (condition.Operator.OperatorId === null) {
            this.setState({
                gridState: Object.assign({}, this.state.gridState, { searchValue: '' }),
                searchText: ''
            }, this.loadGridData);
        }
        else {
            let conditions = [];
            for (let i in this.state.advancedSearchConditions) {
                if (JSON.stringify(this.state.advancedSearchConditions[i]) != JSON.stringify(condition)) {
                    conditions.push(this.state.advancedSearchConditions[i]);
                }
            }
            this.setState({ advancedSearchConditions: conditions }, this.loadGridData);
        }
    }
    onClearAllPills(e) {
        this.setState({
            gridState: Object.assign({}, this.state.gridState, { searchValue: '' }),
            searchText: '',
            advancedSearchConditions: []
        }, this.loadGridData);
    }
    renderSearchBoxAndActions() {
        let searchConditions = [];
        if (this.state.gridState.searchValue) {
            let cond = new Condition_1.Condition();
            cond.Attribute = new Condition_1.Attribute(null, null, "Any field", 0, false, "");
            cond.Operator = new Condition_1.Operator(null, 0, "Contains", 0);
            cond.ConditionId = null;
            cond.Value = this.state.gridState.searchValue;
            searchConditions.push(cond);
        }
        for (let i in this.state.advancedSearchConditions) {
            searchConditions.push(this.state.advancedSearchConditions[i]);
        }
        if (this.props.showSearchBox == undefined || this.props.showSearchBox == true) {
            return (React.createElement("div", { className: "row mb-3" },
                React.createElement("div", { className: "col mb-md-3 mb-lg-0" }, this.renderSearchBox()),
                React.createElement("div", { className: "col-md-12 col-lg-auto" }, this.renderMoreButtons()),
                React.createElement("div", { className: "col-md-12" },
                    React.createElement(SearchPills_1.SearchPills, { conditions: searchConditions, onPillDelete: this.onSearchFillsDelete, onClearAll: this.onClearAllPills }))));
        }
        else if (this.props.showGridAction) {
            return (React.createElement("div", { className: "row mb-3" },
                React.createElement("div", { className: "col mb-md-3 mb-lg-0" }),
                React.createElement("div", { className: "col-md-12 col-lg-auto" }, this.renderMoreButtons())));
        }
        else {
            return (React.createElement("div", { className: "row mb-3" },
                React.createElement("div", { className: "col mb-md-3 mb-lg-0" }, this.renderAdditionalFilters()),
                React.createElement("div", { className: "col-md-12 col-lg-auto" })));
        }
    }
    renderDialog() {
        return (React.createElement(Dialog_1.KendoDialog, Object.assign({ ref: (c) => { this.dialog = c; } }, this.state.dialogProps)));
    }
    renderLoadingPanel() {
        if (this.state.loading) {
            return (React.createElement("div", { className: "k-loading-mask" },
                React.createElement("span", { className: "k-loading-text" }, "Loading"),
                React.createElement("div", { className: "k-loading-image" }),
                React.createElement("div", { className: "k-loading-color" })));
        }
    }
    onSortChange(e) {
        this.setState({
            gridState: Object.assign({}, this.state.gridState, { sortField: e.sort[0].field, dir: e.sort[0].dir === this.state.gridState.dir && e.sort[0].dir === 'asc' ? 'desc' : 'asc' })
        }, this.loadGridData);
    }
    onAdvancedSearchApply(conditions) {
        this.setState({ advancedSearchConditions: conditions, gridState: Object.assign({}, this.state.gridState, { method: 'post' }) }, this.loadGridData);
    }
    onConditionChange(conditions) {
    }
    onAdditionalFilterChange(e) {
        let value = this.props.additionalFilters[e.itemIndex];
        this.setState({ additionalFilterValue: value, gridState: Object.assign({}, this.state.gridState, { additionalFilter: value.value }) }, this.loadGridData);
    }
    renderAdvancedSearch() {
        if (this.state.showAdvancedSearchDialog) {
            return (React.createElement(AdvancedSearchDialog_1.AdvancedSearchDialog, { resourceIds: this.props.advancedSearchEntity, selectedConditions: this.state.advancedSearchConditions, onApply: this.onAdvancedSearchApply, onConditionChange: this.onConditionChange }));
        }
    }
    renderAdditionalFilters() {
        if (this.props.additionalFilters && this.props.additionalFilters.length > 0) {
            return (React.createElement("div", { className: "float-left mr-3 filterButtonContainer" },
                React.createElement(kendo_react_buttons_1.SplitButton, { buttonClass: "filterButton", popupSettings: { popupClass: "filterButton" }, text: this.state.additionalFilterValue.name, key: "value", textField: "name", className: "actions-dropdown", items: this.props.additionalFilters, onItemClick: this.onAdditionalFilterChange })));
        }
        else {
            return (React.createElement("div", { className: "" }));
        }
    }
    getPixel(percent, adjust) {
        let wd = $("#" + this.props.container).width();
        wd = wd - adjust;
        if (!isNaN(wd)) {
            wd = (wd / 100) * percent;
            return wd;
        }
        else {
            return percent;
        }
    }
    render() {
        let gridStyle = Object.assign({}, this.state.gridStyle, { width: '100%', maxHeight: this.state.clientHeight });
        return (React.createElement("div", { id: "groupableGrid" },
            this.renderSearchBoxAndActions(),
            React.createElement(kendo_react_grid_1.Grid, Object.assign({ ref: (c) => { this.grid = c; }, sortable: true, onSortChange: this.onSortChange, style: gridStyle, selectedField: "selected" }, this.state.gridState, { groupable: { footer: 'visible' }, onExpandChange: this.expandChange, onPageChange: this.onPageChange, onHeaderSelectionChange: this.onHeaderSelectionChange, expandField: "expanded", pageable: this.state.pagination, onDataStateChange: this.dataStateChange, cellRender: this.GridCellRenderer, onSelectionChange: this.gridSelectionChange, pageSize: this.state.dataLength }),
                this.renderSelectionField(),
                this.renderFields()),
            this.renderDialog(),
            this.renderLoadingPanel()));
    }
}
KendoGroupableGrid.instance = null;
exports.KendoGroupableGrid = KendoGroupableGrid;
//# sourceMappingURL=KendoGroupableGrid.js.map