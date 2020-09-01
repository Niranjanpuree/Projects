"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
require("core-js");
const React = require("react");
const kendo_react_grid_1 = require("@progress/kendo-react-grid");
const kendo_react_inputs_1 = require("@progress/kendo-react-inputs");
const CommandCell_1 = require("./CommandCell");
const GridDataLoader_1 = require("./GridDataLoader");
const Dialog_1 = require("../Dialog/Dialog");
const kendo_react_buttons_1 = require("@progress/kendo-react-buttons");
const GridColumnMenu_1 = require("./GridColumnMenu");
const Remote_1 = require("../Remote/Remote");
const AdvancedSearchDialog_1 = require("../../Component/AdvancedSearch/AdvancedSearchDialog");
const SearchPills_1 = require("./SearchPills");
const Condition_1 = require("../../Component/Entities/Condition");
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
class KendoGrid extends React.Component {
    constructor(props) {
        super(props);
        this.componentLoaded = false; //uses for data loaded
        this.isComponentMounted = false;
        this.gridElement = React.createRef();
        KendoGrid.instance = this;
        let buttons = [];
        this.isComponentMounted = false;
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
            tmpAdditionalFilter = this.props.additionalFilters.filter((v, i) => {
                return v.default == true;
            })[0] || { name: '', value: '' };
        }
        buttons.push({
            primary: true,
            requireValidation: false,
            text: "",
            action: this.printPDF
        });
        buttons.push({
            primary: false,
            requireValidation: false,
            text: "",
            action: this.onDialogCancel
        });
        let dialogProps = {
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
            register: this.registerDialog
        };
        let dState = { take: this.props.pageSize || 10, skip: 0, searchValue: searchValue, dir: 'asc', sortField: '', additionalFilter: tmpAdditionalFilter.value };
        if (this.props.externalDataState) {
            dState.take = this.props.externalDataState.take || dState.take;
            dState.skip = this.props.externalDataState.skip || dState.skip;
            dState.searchValue = this.props.externalDataState.searchValue || dState.searchValue;
            dState.dir = this.props.externalDataState.dir || dState.dir;
            dState.sortField = this.props.externalDataState.sortField || dState.sortField;
            dState.additionalFilter = this.props.externalDataState.additionalFilter || dState.additionalFilter;
        }
        this.state = {
            gridFields: [],
            fields: [],
            allFields: [],
            showSelection: this.props.showSelection == undefined || this.props.showSelection == true,
            sortField: '',
            parentWidth: this.props.gridWidth,
            pageable: {
                buttonCount: 5,
                info: true,
                type: 'numeric',
                pageSizes: true,
                previousNext: true
            },
            dataState: dState,
            data: { data: [], total: 0 },
            dialogProps: dialogProps,
            redirect: false,
            submittableFormParam: {},
            persistentSelection: [],
            gridStyle: { height: this.props.gridHeight || '500px' },
            sort: [],
            currentColumnMenu: null,
            isSwitchOn: this.props.switchButton && this.props.switchButton.length ? this.__getSwitchButtonChecked(this.props.switchButton) : this.props.switchButton ? this.props.switchButton.checked : false,
            gridHeaderChecked: false,
            advancedSearchConditions: initialSearchParams,
            searchText: '',
            additionalFilterValue: tmpAdditionalFilter,
            resetPageIndex: false,
        };
        this.selectionChange = this.selectionChange.bind(this);
        this.headerSelectionChange = this.headerSelectionChange.bind(this);
        this.onSortChange = this.onSortChange.bind(this);
        this.onPageChange = this.onPageChange.bind(this);
        this.setDefaultSortField = this.setDefaultSortField.bind(this);
        this.onPageSizeChange = this.onPageSizeChange.bind(this);
        this.onSearchFieldKeyPress = this.onSearchFieldKeyPress.bind(this);
        this.onSearchButtonClicked = this.onSearchButtonClicked.bind(this);
        this.exportPDFClick = this.exportPDFClick.bind(this);
        this.exportExcelClick = this.exportExcelClick.bind(this);
        this.getCommandCell = this.getCommandCell.bind(this);
        this.dataRecieved = this.dataRecieved.bind(this);
        this.printPDF = this.printPDF.bind(this);
        this.onDialogCancel = this.onDialogCancel.bind(this);
        this.printExcel = this.printExcel.bind(this);
        this.onGridMenuClick = this.onGridMenuClick.bind(this);
        this.onAddActionComplete = this.onAddActionComplete.bind(this);
        this.addRecordClicked = this.addRecordClicked.bind(this);
        this.exportToPDF = this.exportToPDF.bind(this);
        this.exportToExcel = this.exportToExcel.bind(this);
        this.openSubmittableForm = this.openSubmittableForm.bind(this);
        this._showSubmittableForm = this._showSubmittableForm.bind(this);
        this.onSubmittableFormSuccess = this.onSubmittableFormSuccess.bind(this);
        this.closeDialog = this.closeDialog.bind(this);
        this.registerDialog = this.registerDialog.bind(this);
        this.clearSelection = this.clearSelection.bind(this);
        this.hasBeenSelected = this.hasBeenSelected.bind(this);
        this.managePersitentSelection = this.managePersitentSelection.bind(this);
        this.findSelectedIndex = this.findSelectedIndex.bind(this);
        this._selectionChange = this._selectionChange.bind(this);
        this.renderSearchBoxAndActions = this.renderSearchBoxAndActions.bind(this);
        this.renderMoreButtons = this.renderMoreButtons.bind(this);
        this.renderSearchBox = this.renderSearchBox.bind(this);
        this.renderSelectionField = this.renderSelectionField.bind(this);
        this.reloadData = this.reloadData.bind(this);
        this.refresh = this.refresh.bind(this);
        this.loadGridColumns = this.loadGridColumns.bind(this);
        this.isHiddenField = this.isHiddenField.bind(this);
        this.renderSwitchButton = this.renderSwitchButton.bind(this);
        this.onSwitchChange = this.onSwitchChange.bind(this);
        this.getGridData = this.getGridData.bind(this);
        this.getGridDataToPost = this.getGridDataToPost.bind(this);
        this.onColumnReorder = this.onColumnReorder.bind(this);
        this.renderAdvancedSearch = this.renderAdvancedSearch.bind(this);
        this.onAdvancedSearchApply = this.onAdvancedSearchApply.bind(this);
        this.onConditionChange = this.onConditionChange.bind(this);
        this.onSearchFillsDelete = this.onSearchFillsDelete.bind(this);
        this.getSearchParameters = this.getSearchParameters.bind(this);
        this.onAdditionalFilterChange = this.onAdditionalFilterChange.bind(this);
        this.onClearAllPills = this.onClearAllPills.bind(this);
        this.makeQueryString = this.makeQueryString.bind(this);
        this.downloadPDF = this.downloadPDF.bind(this);
        this.downloadExcel = this.downloadExcel.bind(this);
        this.loadAddDataForSelectAll = this.loadAddDataForSelectAll.bind(this);
        this.printFieldValueAsList = this.printFieldValueAsList.bind(this);
        this.onMoreLinkClicked = this.onMoreLinkClicked.bind(this);
        this.onRowClick = this.onRowClick.bind(this);
        this.getPixel = this.getPixel.bind(this);
        this.getDataUrl = this.getDataUrl.bind(this);
        this.updateExternalSearch = this.updateExternalSearch.bind(this);
        let prop = this.props;
        if (prop.gridStyle) {
            this.setState({ gridStyle: prop.gridStyle });
        }
        if (this.props.externalFilters) {
            for (let i in this.props.externalFilters) {
                this.state.advancedSearchConditions.push(this.props.externalFilters[i]);
            }
        }
    }
    __getSwitchButtonChecked(a) {
        let chked = [];
        a.map((v, i) => { chked.push(v.checked); });
        return chked;
    }
    updateExternalSearch(param) {
        if (param) {
            for (let i in param) {
                let f = this.state.advancedSearchConditions.findIndex((v, i) => { return v.Additional && v.Additional === true; });
                if (f > -1) {
                    this.state.advancedSearchConditions[f] = param[i];
                }
                else {
                    this.state.advancedSearchConditions.push(param[i]);
                }
            }
            this.forceUpdate();
        }
    }
    getDataUrl() {
        return this.dataLoader.getDataUrl();
    }
    getSearchParameters() {
        return {
            searchValue: this.state.dataState.searchValue,
            advancedSearchConditions: this.state.advancedSearchConditions,
            additionalFilter: this.state.additionalFilterValue
        };
    }
    getGridData() {
        return this.state.data;
    }
    isHiddenField(name) {
        if (this.props.hideColumns) {
            for (let i = 0; i < this.props.hideColumns.length; i++) {
                if (this.props.hideColumns[i] == name)
                    return true;
            }
        }
        return false;
    }
    registerDialog(e) {
        KendoGrid.instance.dialog = e;
    }
    openDialog(param) {
        KendoGrid.instance.dialog.props = Object.assign({}, KendoGrid.instance.dialog.props, { dialogTitle: param.dialogTitle, buttons: param.buttons, getUrl: param.getUrl, getMethod: param.getMethod || 'get', postData: param.postData || [], method: param.method, postUrl: param.postUrl });
        this.setState({ dialogProps: KendoGrid.instance.dialog.props }, KendoGrid.instance.dialog.openForm);
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
    getGridDataToPost() {
        if (this.state.gridHeaderChecked) {
            let excludeItems = [];
            let includeItems = [];
            for (let i in this.state.persistentSelection) {
                if (this.state.persistentSelection[i].selected === false) {
                    excludeItems.push(this.state.persistentSelection[i]);
                }
            }
            return {
                selectedAll: true,
                excludeList: excludeItems,
                includeList: includeItems
            };
        }
        else {
            let excludeItems = [];
            let includeItems = [];
            for (let i in this.state.persistentSelection) {
                if (this.state.persistentSelection[i].selected === true) {
                    includeItems.push(this.state.persistentSelection[i]);
                }
            }
            return {
                selectedAll: false,
                excludeList: excludeItems,
                includeList: includeItems
            };
        }
    }
    closeDialog() {
    }
    onDialogCancel(e) {
        this.clearSelection();
        KendoGrid.instance.closeDialog();
    }
    clearSelection() {
        let data = [];
        this.state.data.data.forEach((item) => {
            item.selected = false;
            data.push(item);
        });
        this.setState({
            persistentSelection: [],
            gridHeaderChecked: false,
            data: Object.assign({}, this.state.data, { data: data })
        }, this.forceUpdate);
        var inputs = document.getElementsByTagName("input");
        if (inputs.length) {
            for (let i = 0; i < inputs.length; i++) {
                if (inputs[i].getAttribute("type") === "checkbox") {
                    inputs[i].checked = false;
                }
            }
        }
    }
    managePersitentSelection(item) {
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
    selectionChange(event) {
        event.dataItem.selected = !event.dataItem.selected;
        this.managePersitentSelection(event.dataItem);
    }
    _selectionChange(data) {
        this.managePersitentSelection(data);
    }
    headerSelectionChange(event) {
        this.setState({ persistentSelection: [] });
        const checked = event.syntheticEvent.target.checked;
        let gridData = this.state.data.data;
        gridData.forEach((item) => {
            item.selected = checked;
        });
        this.setState({
            gridHeaderChecked: event.syntheticEvent.target.checked,
            data: Object.assign({}, this.state.data, { data: gridData })
        });
    }
    componentWillMount() {
        this.isComponentMounted = false;
    }
    componentDidMount() {
        this.isComponentMounted = true;
        if (this.isComponentMounted) {
            if (this.props.parent != undefined) {
                let width = 0;
                if ($(this.props.parent).closest(".gridparent").length > 0) {
                    while (width == 0) {
                        width = $(this.props.parent).closest(".gridparent").width();
                    }
                }
                else if ($(this.props.parent).closest(".container").length > 0) {
                    while (width == 0) {
                        width = $(this.props.parent).closest(".container").width();
                    }
                }
                this.setState({ parentWidth: width }, this.forceUpdate);
            }
            Remote_1.Remote.get(this.props.fieldUrl, (result) => {
                let data = [];
                result = result.sort((a, b) => parseFloat(a.orderIndex) - parseFloat(b.orderIndex));
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
                    row.show = !this.isHiddenField(row.fieldName) && row.visibleToGrid === true;
                    data.push(row);
                }
                let gridFields = data.filter(c => c.show == true && (c.visibleToGrid == undefined || c.visibleToGrid == true));
                this.setState({ fields: data, allFields: result, gridFields: gridFields });
                this.setDefaultSortField(data);
            }, (error) => {
                window.Dialog.alert(error);
            });
        }
    }
    setDefaultSortField(fields) {
        for (let fld in fields) {
            if (fields[fld].isDefaultSortField) {
                this.setState({ sortField: fields[fld].fieldName });
            }
        }
    }
    updateColumnVisibility(columns) {
        let arr = [];
        columns = columns.sort((a, b) => {
            if (parseInt(a.orderIndex) < parseInt(b.orderIndex))
                return -1;
            else
                return 1;
        });
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
        this.setState({ gridFields: arr }, this.forceUpdate);
    }
    onSortChange(e) {
        e.nativeEvent.preventDefault();
        e.nativeEvent.stopPropagation();
        this.clearSelection();
        this.closeDialog();
        let evt = e;
        if (e.sort.length == 0) {
            evt.sort.push({
                field: this.state.sort[0].field,
                dir: 'asc'
            });
        }
        for (let s in evt.sort) {
            if (parseInt(s + "") > 0)
                break;
            let sort = evt.sort[s];
            this.setState({
                dataState: Object.assign({}, this.state.dataState, { sortField: sort.field, dir: sort.dir }),
                sort: evt.sort
            }, this.forceUpdate);
        }
    }
    onPageChange(e) {
        this.componentLoaded = false;
        this.setState({ dataState: Object.assign({}, this.state.dataState, { skip: e.page.skip, take: e.page.take, searchValue: this.state.dataState.searchValue, dir: this.state.dataState.dir, sortField: this.state.dataState.sortField }) }, this.forceUpdate);
    }
    onPageSizeChange(e) {
        this.setState({
            dataState: Object.assign({}, this.state.dataState, { take: e.target.value })
        }, this.forceUpdate);
    }
    onSearchFieldKeyPress(e) {
        if (e.which == 13) {
            this.setState({
                dataState: Object.assign({}, this.state.dataState, { skip: 0, searchValue: e.target.value })
            }, this.forceUpdate);
        }
    }
    onSearchButtonClicked(e) {
        this.setState({
            dataState: Object.assign({}, this.state.dataState, { skip: 0, searchValue: this.searchText.value })
        }, this.forceUpdate);
    }
    getCommandCell(e) {
        if (!this.props.rowMenus)
            return;
        let key = (new Date()).getTime();
        return (React.createElement("td", { className: "rowMenu" },
            React.createElement(CommandCell_1.CommandCell, { key: key, menuItems: this.props.rowMenus, dataItem: e.dataItem, parent: this })));
    }
    onAdvancedSearchApply(conditions) {
        this.setState({ advancedSearchConditions: conditions, dataState: Object.assign({}, this.state.dataState, { skip: 0, method: 'post' }) }, this.forceUpdate);
    }
    onConditionChange(conditions) {
    }
    renderAdvancedSearch() {
        if (this.props.showAdvanceSearchBox != false)
            return (React.createElement(AdvancedSearchDialog_1.AdvancedSearchDialog, { ref: (c) => { this.advancedSearchDialog = c; }, resourceIds: this.props.advancedSearchEntity, onApply: this.onAdvancedSearchApply, selectedConditions: this.state.advancedSearchConditions, onConditionChange: this.onConditionChange }));
        else
            return (React.createElement("span", null));
    }
    renderFields() {
        let fields = [];
        let deductionColumnWidth = 120;
        if (this.state.showSelection == false)
            deductionColumnWidth = 70;
        if (!this.props.rowMenus) {
            deductionColumnWidth -= 70;
        }
        let width = "150";
        let columns = this.state.gridFields;
        columns = columns.filter(c => (c.show == true && (c.visibleToGrid == undefined || c.visibleToGrid == true)) || c.fieldName == 'menu');
        columns = columns.sort((a, b) => {
            if (a.orderIndex < b.orderIndex)
                return -1;
            else
                return 1;
        });
        let menuField = columns.filter(c => (c.fieldName == 'menu'));
        if (menuField.length === 0 && this.props.rowMenus) {
            let arr = [];
            arr.push({
                fieldName: 'menu',
                fieldLabel: ' ',
                isSortable: false,
                isFilterable: false,
                orderIndex: 0,
                showHeaderText: false
            });
            for (let i = 0; i < columns.length; i++) {
                arr.push(columns[i]);
            }
            columns = arr;
        }
        let totalColumns = deductionColumnWidth === 120 ? columns.length - 1 : columns.length;
        columns.forEach((e, index) => {
            let percent = e.columnWidth ? e.columnWidth : (100 / totalColumns);
            width = this.getPixel(percent, deductionColumnWidth) + "";
            if (e.columnMinimumWidth > width) {
                width = e.columnMinimumWidth;
            }
            if (index == 0 && this.props.rowMenus) {
                fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: "gridfield" + index, field: e.fieldName, title: e.fieldLabel, sortable: e.isSortable, filterable: e.isFilterable, format: e.format, orderIndex: e.orderIndex, width: "50px", cell: this.getCommandCell, className: e.gridColumnCss, headerClassName: e.gridColumnCss }));
            }
            else {
                if (index == columns.length - 1 && e.show && this.props.showColumnMenu !== false) {
                    if (e.clickable && this.props.itemNavigationUrl) {
                        fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: "gridfield" + index, field: e.fieldName, title: e.fieldLabel, sortable: e.isSortable, format: e.format, filterable: e.isFilterable, orderIndex: e.orderIndex, width: e.visibleToGrid == true ? width + "px" : "0px", className: e.gridColumnCss, headerClassName: e.gridColumnCss, columnMenu: props => React.createElement(GridColumnMenu_1.GridColumnMenu, Object.assign({ gridColumns: this.state.gridFields }, props, { grid: this, columns: this.state.allFields })), cell: (props) => {
                                const value = props.dataItem[props.field];
                                const currency = props.dataItem['currency'] || '';
                                let css = "";
                                if (this.props.rowIconSettings && props.field == this.props.rowIconSettings.column) {
                                    this.props.rowIconSettings.icons.forEach((v) => {
                                        if (props.dataItem[v.field] === v.value) {
                                            css += " " + v.css;
                                        }
                                    });
                                }
                                if (e.format === "{0:c}") {
                                    return (React.createElement("td", { className: e.gridColumnCss + " " + css },
                                        React.createElement("a", { href: this.props.itemNavigationUrl + props.dataItem[this.props.identityField] }, (value === null) ? '' : currency + ' ' + numberWithCommas(props.dataItem[props.field]))));
                                }
                                else if (e.format === "{0:d}") {
                                    return (React.createElement("td", { className: e.gridColumnCss + " " + css },
                                        React.createElement("a", { href: this.props.itemNavigationUrl + props.dataItem[this.props.identityField] }, (value === null) ? '' : formatUSDate(props.dataItem[props.field], "en"))));
                                }
                                else {
                                    return (React.createElement("td", null,
                                        React.createElement("a", { href: this.props.itemNavigationUrl + props.dataItem[this.props.identityField] }, props.dataItem[props.field])));
                                }
                            } }));
                    }
                    else {
                        if (e.format === "{0:c}") {
                            fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: "gridfield" + index, field: e.fieldName, title: e.fieldLabel, sortable: e.isSortable, format: e.format, filterable: e.isFilterable, orderIndex: e.orderIndex, width: e.visibleToGrid == true ? width + "px" : "0px", className: e.gridColumnCss, headerClassName: e.gridColumnCss, columnMenu: props => React.createElement(GridColumnMenu_1.GridColumnMenu, Object.assign({ gridColumns: this.state.gridFields }, props, { grid: this, columns: this.state.allFields })), cell: (prop) => {
                                    const value = prop.dataItem[prop.field];
                                    const currency = prop.dataItem['currency'] || '';
                                    return (React.createElement("td", { className: e.gridColumnCss },
                                        " ",
                                        (value === null) ? '' : currency + ' ' + numberWithCommas(prop.dataItem[prop.field])));
                                } }));
                        }
                        else {
                            fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: "gridfield" + index, field: e.fieldName, title: e.fieldLabel, sortable: e.isSortable, format: e.format, filterable: e.isFilterable, orderIndex: e.orderIndex, width: e.visibleToGrid == true ? width + "px" : "0px", className: e.gridColumnCss, headerClassName: e.gridColumnCss, columnMenu: props => React.createElement(GridColumnMenu_1.GridColumnMenu, Object.assign({ gridColumns: this.state.gridFields }, props, { grid: this, columns: this.state.allFields })) }));
                        }
                    }
                }
                else if (e.show) {
                    if (e.clickable && this.props.itemNavigationUrl) {
                        fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: "gridfield" + index, field: e.fieldName, title: e.fieldLabel, sortable: e.isSortable, format: e.format, filterable: e.isFilterable, orderIndex: e.orderIndex, width: e.visibleToGrid == true ? width + "px" : "0px", className: e.gridColumnCss, headerClassName: e.gridColumnCss, cell: (props) => {
                                let css = "";
                                const value = props.dataItem[props.field];
                                css = e.gridColumnCss;
                                const currency = props.dataItem['currency'] || '';
                                if (this.props.rowIconSettings && props.field == this.props.rowIconSettings.column) {
                                    this.props.rowIconSettings.icons.forEach((v) => {
                                        if (props.dataItem[v.field] === v.value) {
                                            css += " " + v.css;
                                        }
                                    });
                                }
                                if (e.format === "{0:d}") {
                                    return (React.createElement("td", { className: css },
                                        React.createElement("a", { href: this.props.itemNavigationUrl + props.dataItem[this.props.identityField] }, (value === null) ? '' : formatUSDate(value))));
                                }
                                else if (e.format === "{0:c}") {
                                    return (React.createElement("td", { className: css },
                                        React.createElement("a", { href: this.props.itemNavigationUrl + props.dataItem[this.props.identityField] }, (value === null) ? '' : currency + ' ' + numberWithCommas(value))));
                                }
                                else {
                                    return (React.createElement("td", { className: css },
                                        React.createElement("a", { href: this.props.itemNavigationUrl + props.dataItem[this.props.identityField] }, value)));
                                }
                            } }));
                    }
                    else {
                        if (e.format === "{0:c}") {
                            fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: "gridfield" + index, field: e.fieldName, title: e.fieldLabel, sortable: e.isSortable, format: e.format, filterable: e.isFilterable, orderIndex: e.orderIndex, width: e.visibleToGrid == true ? width + "px" : "0px", className: e.gridColumnCss, headerClassName: e.gridColumnCss, cell: (prop) => {
                                    const value = prop.dataItem[prop.field];
                                    const currency = prop.dataItem['currency'] || '';
                                    let css = "";
                                    if (this.props.rowIconSettings && prop.field == this.props.rowIconSettings.column) {
                                        this.props.rowIconSettings.icons.forEach((v) => {
                                            if (prop.dataItem[v.field] === v.value) {
                                                css += " " + v.css;
                                            }
                                        });
                                    }
                                    return (React.createElement("td", { className: e.gridColumnCss + " " + css },
                                        " ",
                                        (value === null) ? '' : currency + ' ' + numberWithCommas(prop.dataItem[prop.field])));
                                } }));
                        }
                        else if (e.format === "{0:d}") {
                            fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: "gridfield" + index, field: e.fieldName, title: e.fieldLabel, sortable: e.isSortable, format: e.format, filterable: e.isFilterable, orderIndex: e.orderIndex, width: e.visibleToGrid == true ? width + "px" : "0px", className: e.gridColumnCss, headerClassName: e.gridColumnCss, cell: (prop) => {
                                    const value = prop.dataItem[prop.field];
                                    let css = "";
                                    if (this.props.rowIconSettings && prop.field == this.props.rowIconSettings.column) {
                                        this.props.rowIconSettings.icons.forEach((v) => {
                                            if (prop.dataItem[v.field] === v.value) {
                                                css += " " + v.css;
                                            }
                                        });
                                    }
                                    return (React.createElement("td", { className: e.gridColumnCss + " " + css },
                                        " ",
                                        (value === null) ? '' : formatUSDate(prop.dataItem[prop.field], "en")));
                                } }));
                        }
                        else if (e.format === "{0:dt}") {
                            fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: "gridfield" + index, field: e.fieldName, title: e.fieldLabel, sortable: e.isSortable, format: e.format, filterable: e.isFilterable, orderIndex: e.orderIndex, width: e.visibleToGrid == true ? width + "px" : "0px", className: e.gridColumnCss, headerClassName: e.gridColumnCss, cell: (prop) => {
                                    const value = prop.dataItem[prop.field];
                                    let css = "";
                                    if (this.props.rowIconSettings && prop.field == this.props.rowIconSettings.column) {
                                        this.props.rowIconSettings.icons.forEach((v) => {
                                            if (prop.dataItem[v.field] === v.value) {
                                                css += " " + v.css;
                                            }
                                        });
                                    }
                                    return (React.createElement("td", { className: e.gridColumnCss + " " + css },
                                        " ",
                                        (value === null) ? '' : formatUSDatetime(prop.dataItem[prop.field])));
                                } }));
                        }
                        else if (e.format === "{0:html}") {
                            fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: "gridfield" + index, field: e.fieldName, title: e.fieldLabel, sortable: e.isSortable, format: e.format, filterable: e.isFilterable, orderIndex: e.orderIndex, width: e.visibleToGrid == true ? width + "px" : "0px", className: e.gridColumnCss, headerClassName: e.gridColumnCss, cell: (prop) => {
                                    const value = prop.dataItem[prop.field];
                                    let css = "";
                                    if (this.props.rowIconSettings && prop.field == this.props.rowIconSettings.column) {
                                        this.props.rowIconSettings.icons.forEach((v) => {
                                            if (prop.dataItem[v.field] === v.value) {
                                                css += " " + v.css;
                                            }
                                        });
                                    }
                                    return (React.createElement("td", { className: e.gridColumnCss + " " + css, dangerouslySetInnerHTML: { __html: prop.dataItem[prop.field] } }));
                                } }));
                        }
                        else if (e.format === "list") {
                            fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: "gridfield" + index, field: e.fieldName, title: e.fieldLabel, sortable: e.isSortable, format: e.format, filterable: e.isFilterable, orderIndex: e.orderIndex, width: e.visibleToGrid == true ? width + "px" : "0px", className: e.gridColumnCss, headerClassName: e.gridColumnCss, cell: (prop) => {
                                    const value = prop.dataItem[prop.field];
                                    let css = "";
                                    if (this.props.rowIconSettings && prop.field == this.props.rowIconSettings.column) {
                                        this.props.rowIconSettings.icons.forEach((v) => {
                                            if (prop.dataItem[v.field] === v.value) {
                                                css += " " + v.css;
                                            }
                                        });
                                    }
                                    return (React.createElement("td", { className: e.gridColumnCss + " " + css }, this.printFieldValueAsList(value, prop.dataIndex - 1)));
                                } }));
                        }
                        else if (index == 1) {
                            fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: "gridfield" + index, field: e.fieldName, title: e.fieldLabel, sortable: e.isSortable, format: e.format, filterable: e.isFilterable, orderIndex: e.orderIndex, width: e.visibleToGrid == true ? width + "px" : "0px", className: e.gridColumnCss, headerClassName: e.gridColumnCss, cell: (prop) => {
                                    let css = "";
                                    if (this.props.rowIconSettings && prop.field == this.props.rowIconSettings.column) {
                                        this.props.rowIconSettings.icons.forEach((v) => {
                                            if (prop.dataItem[v.field] === v.value) {
                                                css += " " + v.css;
                                            }
                                        });
                                    }
                                    return (React.createElement("td", { className: css }, prop.dataItem[prop.field]));
                                } }));
                        }
                        else {
                            fields.push(React.createElement(kendo_react_grid_1.GridColumn, { key: "gridfield" + index, field: e.fieldName, title: e.fieldLabel, sortable: e.isSortable, format: e.format, filterable: e.isFilterable, orderIndex: e.orderIndex, width: e.visibleToGrid == true ? width + "px" : "0px", className: e.gridColumnCss, headerClassName: e.gridColumnCss, cell: (props) => {
                                    let css = e.gridColumnCss;
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
                                    return (React.createElement("td", { className: css }, value));
                                } }));
                        }
                    }
                }
            }
        });
        return fields;
    }
    printFieldValueAsList(list, dataIndex) {
        let dIndex = dataIndex;
        let items = [];
        for (let i in list) {
            if (parseInt(i) < 3) {
                items.push(React.createElement("span", { key: i, className: "badge badge-pill badge-secondary mr-1" }, list[i]));
            }
        }
        if (list.length > 3) {
            items.push(React.createElement("span", { key: 4, className: "btn btn-sm btn-secondary more" },
                React.createElement("a", { href: "#", itemID: dIndex, onClick: this.onMoreLinkClicked }, "More")));
        }
        return items;
    }
    printPDF(e, columns) {
        KendoGrid.instance.closeDialog();
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
                title: unescape(columns.fieldLabels[fieldIndex])
            });
        }
        let sender = this;
        this.getSelectedItems((result) => {
            if (this.state.gridHeaderChecked) {
                this.loadAddDataForSelectAll((result) => {
                    let data = result.result || result;
                    let deSelected = this.state.persistentSelection.filter((v) => { return v.selected === false; });
                    let nData = [];
                    for (let i = 0; i < data.length; i++) {
                        let index = deSelected.findIndex((v) => {
                            return v[this.props.identityField] == data[i][this.props.identityField];
                        });
                        if (index < 0) {
                            nData.push(data[i]);
                        }
                    }
                    sender.downloadPDF(nData, gridColumns);
                    sender.clearSelection();
                });
            }
            else {
                let rows = result.result || result;
                for (var i = 0; i < rows.length; i++) {
                    if (rows[i].selected == true) {
                        data.push(rows[i]);
                    }
                }
                sender.downloadPDF(data, gridColumns);
                sender.clearSelection();
            }
        });
    }
    loadAddDataForSelectAll(callback) {
        let dState = JSON.parse(JSON.stringify(this.state.dataState));
        dState.take = this.state.data.total;
        dState.skip = 0;
        let queryString = this.makeQueryString(dState);
        let caller = this;
        let separator = "?";
        if (this.props.dataURL.indexOf("?") >= 0) {
            separator = "&";
        }
        let url = this.props.dataURL + separator + queryString + "&t=" + (new Date()).getTime();
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
    exportToPDF() {
        this.exportPDFClick(this.props.exportFieldUrl);
    }
    exportToExcel() {
        this.exportExcelClick(this.props.exportFieldUrl);
    }
    exportPDFClick(fieldUrl) {
        let selected1 = this.state.persistentSelection.filter((v) => {
            return v.selected == true;
        });
        if (selected1.length == 0 && this.state.gridHeaderChecked == false) {
            window.Dialog.alert("Please select at least a row to export to PDF.");
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
        this.openDialog({
            dialogTitle: 'Export to PDF',
            getUrl: fieldUrl,
            buttons: buttons,
            method: 'get'
        });
    }
    printExcel(e, columns) {
        KendoGrid.instance.closeDialog();
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
                title: unescape(columns.fieldLabels[fieldIndex])
            });
        }
        let sender = this;
        this.getSelectedItems((result) => {
            if (this.state.gridHeaderChecked) {
                this.loadAddDataForSelectAll((result) => {
                    let data = result.result;
                    let deSelected = this.state.persistentSelection.filter((v) => { return v.selected === false; });
                    let nData = [];
                    for (let i = 0; i < data.length; i++) {
                        let index = deSelected.findIndex((v) => {
                            return v[this.props.identityField] == data[i][this.props.identityField];
                        });
                        if (index < 0) {
                            nData.push(data[i]);
                        }
                    }
                    sender.downloadExcel(nData, gridColumns);
                    sender.clearSelection();
                });
            }
            else {
                let rows = result.result || result;
                for (var i = 0; i < rows.length; i++) {
                    if (rows[i].selected == true) {
                        data.push(rows[i]);
                    }
                }
                sender.downloadExcel(data, gridColumns);
                sender.clearSelection();
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
        this.openDialog({
            dialogTitle: 'Export to Excel',
            getUrl: fieldUrl,
            buttons: buttons,
            method: 'get'
        });
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
    hasBeenSelected(niddle) {
        for (let i in this.state.persistentSelection) {
            let nd = JSON.parse(JSON.stringify(niddle));
            let nd1 = JSON.parse(JSON.stringify(this.state.persistentSelection[i]));
            nd = Object.assign({}, nd, { selected: true });
            nd1 = Object.assign({}, nd1, { selected: true });
            if (JSON.stringify(nd1) == JSON.stringify(nd)) {
                return this.state.persistentSelection[i].selected;
            }
            else if (this.state.gridHeaderChecked) {
                return true;
            }
        }
        return false;
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
    dataRecieved(e, loader) {
        if (this.props.onGridLoaded) {
            this.props.onGridLoaded(e.data);
        }
        let data = [];
        if (this.props.selectionField && this.componentLoaded === false) {
            let selectedData = this.state.persistentSelection || [];
            for (let i in e.data) {
                if ((e.data[i][this.props.selectionField] === true || this.state.gridHeaderChecked) && this.findSelectedIndex(e.data[i]) == -1) {
                    selectedData.push(Object.assign({}, e.data[i], { selected: true }));
                }
            }
            this.setState({
                persistentSelection: selectedData
            }, () => {
                for (let i in e.data) {
                    let isSelected = false;
                    let index = this.findSelectedIndex(e.data[i]);
                    if (this.state.gridHeaderChecked === false) {
                        isSelected = this.hasBeenSelected(e.data[i]);
                    }
                    else if (index >= 0 && this.state.gridHeaderChecked == true) {
                        isSelected = this.state.persistentSelection[index].selected;
                    }
                    else {
                        isSelected = true;
                    }
                    data.push(Object.assign({}, e.data[i], { selected: isSelected }));
                }
                if (this.state.currentColumnMenu) {
                    this.setState({ data: { total: e.total, data: data } }, this.state.currentColumnMenu.refresh);
                }
                else {
                    this.setState({ data: { total: e.total, data: data } });
                }
                this.dataLoader = loader;
            });
            this.componentLoaded = true;
        }
        else {
            for (let i in e.data) {
                let isSelected = false;
                let index = this.findSelectedIndex(e.data[i]);
                if (this.state.gridHeaderChecked === false) {
                    isSelected = this.hasBeenSelected(e.data[i]);
                }
                else if (index >= 0 && this.state.gridHeaderChecked == true) {
                    isSelected = this.state.persistentSelection[index].selected;
                }
                else {
                    isSelected = true;
                }
                data.push(Object.assign({}, e.data[i], { selected: isSelected }));
            }
            if (this.state.currentColumnMenu) {
                this.setState({ data: { total: e.total, data: data } }, this.state.currentColumnMenu.refresh);
            }
            else {
                this.setState({ data: { total: e.total, data: data } });
            }
            this.dataLoader = loader;
        }
    }
    dataStateChange(e) {
        this.setState(Object.assign({}, this.state, { currentColumnMenu: null, dataState: e.data }));
    }
    onGridMenuClick(e) {
        let index = e.itemIndex;
        this.props.gridMenu[index].action(e, this);
    }
    renderGridMenu() {
        let menus = [];
        if (!this.props.gridMenu)
            return menus;
        for (let i in this.props.gridMenu) {
            menus.push(React.createElement("a", { key: "gridmenu{i}", className: "dropdown-item", href: "javascript:void(0)", onClick: this.onGridMenuClick }, this.props.gridMenu[i].text));
        }
        return menus;
    }
    addRecordClicked(e) {
        if (this.props.addRecord.redirect && this.props.addRecord.redirect == true) {
            KendoGrid.instance.setState({ redirect: true }, KendoGrid.instance.forceUpdate);
        }
        else if (!this.props.addRecord.getUrl || this.props.addRecord.getUrl === "") {
            this.props.addRecord.action(e, KendoGrid.instance);
        }
        else {
            let buttons = [];
            this.setState({ submittableFormParam: this.props.addRecord });
            for (let i in this.props.addRecord.buttons) {
                let b = this.props.addRecord.buttons[i];
                buttons.push({
                    primary: b.primary,
                    requireValidation: b.requireValidation,
                    text: b.text,
                    buttonIndex: i,
                    saveAndNew: b.saveAndNew ? b.saveAndNew : false,
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
    }
    onAddActionComplete(e) {
    }
    renderRedirect() {
        if (this.props.addRecord && this.props.addRecord.redirect && this.props.addRecord.redirect == true && this.state.redirect) {
            window.location = this.props.addRecord.url;
        }
    }
    renderAddRecordButton() {
        if (this.props.addRecord && this.props.addRecord.text != '') {
            return (React.createElement(kendo_react_buttons_1.Button, { primary: true, onClick: this.addRecordClicked }, this.props.addRecord.text)); //icon={this.props.addRecord.icon}
        }
        return null;
    }
    openSubmittableForm(param, dialogProps) {
        if (dialogProps) {
            this.setState({ dialogProps: Object.assign({}, this.state.dialogProps, dialogProps) }, () => { this._showSubmittableForm(param); });
        }
        else
            this._showSubmittableForm(param);
    }
    reloadData() {
        this.dataLoader.reloadData();
    }
    refresh(culumnMenu = null) {
        if (culumnMenu != null) {
            this.clearSelection();
            this.setState({ currentColumnMenu: culumnMenu, persistentSelection: [], gridHeaderChecked: false }, this.dataLoader.reloadData);
        }
        else {
            this.clearSelection();
            this.dataLoader.reloadData();
        }
    }
    onSubmittableFormSuccess(param, obj) {
        this.dataLoader.reloadData();
        if (param.button) {
            this.state.submittableFormParam.buttons[parseInt(param.button.buttonIndex)].action(param, obj);
        }
        this.forceUpdate();
    }
    _showSubmittableForm(param) {
        let buttons = [];
        this.setState({ submittableFormParam: param });
        for (let i in param.buttons) {
            let b = param.buttons[i];
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
    }
    onSearchFillsDelete(condition) {
        if (condition.Operator.OperatorId === null) {
            this.setState({
                dataState: Object.assign({}, this.state.dataState, { searchValue: '' }),
                searchText: ''
            });
        }
        else {
            let conditions = [];
            for (let i in this.state.advancedSearchConditions) {
                if (JSON.stringify(this.state.advancedSearchConditions[i]) != JSON.stringify(condition)) {
                    conditions.push(this.state.advancedSearchConditions[i]);
                }
            }
            this.advancedSearchDialog.updateSearchConditions(conditions);
            this.setState({ advancedSearchConditions: conditions });
        }
    }
    onAdditionalFilterChange(e) {
        let value = this.props.additionalFilters[e.itemIndex];
        this.setState({ additionalFilterValue: value, dataState: Object.assign({}, this.state.dataState, { additionalFilter: value.value }) }, this.forceUpdate);
    }
    onMoreLinkClicked(e) {
        if (this.props.onMoreLinkClicked) {
            let itemId = e.target.getAttribute("itemid");
            let data = this.state.data.data[itemId];
            this.props.onMoreLinkClicked(data);
        }
    }
    renderSearchBox() {
        return (React.createElement("div", { className: "search-form-r" },
            this.renderAdditionalFilters(),
            React.createElement("div", { className: "input-group mb-3 mb-sm-0" },
                React.createElement(kendo_react_inputs_1.Input, { autoFocus: true, onKeyPress: this.onSearchFieldKeyPress, placeholder: "Search", ref: (c) => { this.searchText = c; }, onChange: c => { this.setState({ searchText: c.target.value }); }, value: this.state.searchText, className: "form-control k-textbox" }),
                React.createElement("div", { className: "input-group-append" },
                    React.createElement("div", { className: "input-group-text" },
                        React.createElement("a", { href: "javascript:void(0)", onClick: this.onSearchButtonClicked, className: "k-icon k-i-search" }, "\u00A0")))),
            this.renderAdvancedSearch(),
            React.createElement("div", { className: "clearfix" })));
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
    renderMoreButtons() {
        if (this.props.showGridAction == undefined || this.props.showGridAction == true) {
            return (React.createElement("div", { className: "text-right" },
                this.renderSwitchButton(),
                this.renderAddRecordButton(),
                " \u00A0",
                React.createElement(kendo_react_buttons_1.SplitButton, { text: "Actions", className: "actions-dropdown", items: this.props.gridMenu, onItemClick: this.onGridMenuClick })));
        }
    }
    onClearAllPills(e) {
        if (this.advancedSearchDialog) {
            this.advancedSearchDialog.updateSearchConditions([]);
        }
        this.setState({
            dataState: Object.assign({}, this.state.dataState, { searchValue: '' }),
            searchText: '',
            advancedSearchConditions: []
        }, this.forceUpdate);
    }
    renderSearchBoxAndActions() {
        let searchConditions = [];
        if (this.state.dataState.searchValue) {
            let cond = new Condition_1.Condition();
            cond.Attribute = new Condition_1.Attribute(null, null, "Any field", 0, false, "");
            cond.Operator = new Condition_1.Operator(null, 0, "Contains", 0);
            cond.ConditionId = null;
            cond.Value = this.state.dataState.searchValue;
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
        else if (this.props.showSearchBox == false && this.props.showGridAction == true) {
            return (React.createElement("div", { className: "row mb-3" },
                React.createElement("div", { className: "col" }),
                React.createElement("div", { className: "col-md-12 col-lg-auto" }, this.renderMoreButtons())));
        }
        else if (this.props.additionalFilters && this.props.additionalFilters.length > 0) {
            return (React.createElement("div", { className: "row mb-3" },
                React.createElement("div", { className: "col mb-md-3 mb-lg-0" }, this.renderAdditionalFilters()),
                React.createElement("div", { className: "col-md-12 col-lg-auto" })));
        }
    }
    renderAdditionalFilters() {
        if (this.props.additionalFilters && this.props.additionalFilters.length > 0) {
            return (React.createElement("div", { className: "float-left mr-3 mb-3 mb-sm-0 filterButtonContainer" },
                React.createElement(kendo_react_buttons_1.SplitButton, { buttonClass: "filterButton", popupSettings: { popupClass: "filterButton" }, text: this.state.additionalFilterValue.name, key: "value", textField: "name", className: "actions-dropdown", items: this.props.additionalFilters, onItemClick: this.onAdditionalFilterChange })));
        }
        else {
            return (React.createElement(React.Fragment, null));
        }
    }
    renderSelectionField() {
        if (this.state.showSelection == true) {
            return (React.createElement(kendo_react_grid_1.GridColumn, { field: "selected", width: "50px" }));
        }
    }
    loadGridColumns(columnMenu) {
        let sender = this;
        Remote_1.Remote.get(this.props.fieldUrl, (result) => {
            let data = [];
            result = result.sort((a, b) => parseFloat(a.orderIndex) - parseFloat(b.orderIndex));
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
                row.show = !sender.isHiddenField(row.fieldName) && row.visibleToGrid == true;
                data.push(row);
            }
            sender.setState({ fields: data });
            sender.dataLoader.reloadData();
        }, (err) => {
            window.Dialog.alert(err);
        });
    }
    onRowClick(e) {
        if (this.props.onRowClick) {
            this.props.onRowClick(e);
        }
    }
    onColumnReorder(e) {
        let that = this;
        console.log(this.gridElement.columns);
        setTimeout(() => {
            console.log(that.gridElement.columns);
        }, 10);
    }
    getPixel(percent, adjust) {
        let wd = $("#" + this.props.parent).width();
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
        let gridStyle = Object.assign({}, this.state.gridStyle, { width: '100%' });
        return (React.createElement("div", { className: "kendoGrid" },
            this.renderSearchBoxAndActions(),
            React.createElement(kendo_react_grid_1.Grid, Object.assign({ ref: (c) => { this.gridElement = c; }, style: gridStyle, selectedField: this.state.showSelection ? "selected" : "" }, this.state.data, this.state.dataState, { sortable: true, onSortChange: this.onSortChange, sort: this.state.sort, resizable: true, onSelectionChange: this.selectionChange, onHeaderSelectionChange: this.headerSelectionChange, reorderable: true, onColumnReorder: this.onColumnReorder, onPageChange: this.onPageChange, pageable: this.state.pageable, onDataStateChange: this.dataStateChange, onRowClick: this.onRowClick }),
                this.renderSelectionField(),
                this.renderFields()),
            React.createElement(GridDataLoader_1.GridDataLoader, { ref: (c) => { this.dataLoader = c; }, baseURL: this.props.dataURL, method: this.state.dataState.method || 'get', switchStatus: this.state.isSwitchOn, dataState: this.state.dataState, onDataRecieved: this.dataRecieved, postValue: this.state.advancedSearchConditions }),
            this.renderRedirect(),
            React.createElement(Dialog_1.KendoDialog, Object.assign({}, this.state.dialogProps))));
    }
}
KendoGrid.instance = null;
exports.KendoGrid = KendoGrid;
//# sourceMappingURL=KendoGrid.js.map