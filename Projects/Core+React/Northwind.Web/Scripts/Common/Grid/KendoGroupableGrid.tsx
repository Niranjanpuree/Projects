import 'core-js';
import * as React from "react"
import { Grid, GridColumn as Column, GridNoRecords } from '@progress/kendo-react-grid';
import { Input, Switch } from "@progress/kendo-react-inputs";
import { CommandCell } from "./CommandCell";
import { KendoDialog } from "../Dialog/Dialog"
import { SplitButton, Button } from "@progress/kendo-react-buttons";
import { GridColumnMenu } from "./GridColumnMenu";
import { process, DataResult } from '@progress/kendo-data-query';
import { Remote } from "../Remote/Remote";
import { AdvancedSearchDialog } from "../../Component/AdvancedSearch/AdvancedSearchDialog";
import { Condition, Attribute, Operator } from "../../Component/Entities/Condition";
import { SearchPills } from "./SearchPills";
import { ComboBox } from '@progress/kendo-react-dropdowns';

declare var window: any;
declare var numberWithCommas: any;
declare var $: any;
declare var formatUSDate: any;
declare var Dialog: any;
declare var getTodayDate: any;

interface IGridProps {
    printTitle?: string;
    printedBy?: any;
    dataUrl: string;
    columnUrl: string;
    groupField: string;
    container?: string;
    rowMenus?: any;
    itemNavigationUrl?: any[];
    navigationFields?: any[];
    identityField: string;
    hideColumns?: any[];
    showSearchBox: boolean;
    showGridAction: boolean;
    gridMenu: any[];
    addRecord?: any;
    exportFieldUrl: string;
    showGroupHeader?: boolean;
    currencySymbol?: string;
    switchButton?: any;
    sortable?: boolean;
    groupTotalFields?: string[];
    groupTotalLabelField?: string;
    rowIconSettings?: RowIconSettings;
    searchParameters?: any;
    additionalFilters?: any[];
    showColumnMenu?: boolean;
    pagination?: any;
    onGridLoaded?: any;
    showAdvancedSearchDialog?: any;
    advancedSearchEntity?: any[];
    externalDataState?: any;
    sortField?: any;
    sortOrder?: any;
    gridHeight?: any;
    description?: any;
    searchPlaceHolder?: string;
    gridMessage?: string;
}

interface IGridState {
    gridWidth: number;
    parentWidth: number;
    gridState: any;
    gridStateTemplate: any;
    fields: any[];
    allFields: any[];
    selectedItems: [];
    persistentSelection: any[];
    redirectUrl: string;
    searchText: string;
    dialogProps: any;
    loading: boolean;
    currencySymbol: string;
    isSwitchOn: any;
    dataLength: number;
    sortable: boolean;
    advancedSearchConditions: any[];
    additionalFilterValue: any;
    gridHeaderChecked: boolean;
    pagination: any;
    clientHeight: any;
    showAdvancedSearchDialog: any;
    rowMenus: any[];
    allowAddButton: boolean;
    allowGridMenu: any[];
    gridMessage: string;
}

export class IconSetting {
    field: string;
    value: any;
    icon: string;
    css: string;
    constructor(field: string, value: any, icon: string, css: string) {
        this.field = field;
        this.value = value;
        this.icon = icon;
        this.css = css;
    }
}

export class RowIconSettings {
    column: string;
    icons: IconSetting[] = [];
}

export class KendoGroupableGrid extends React.Component<IGridProps, IGridState> {

    static instance: KendoGroupableGrid = null;
    grid: Grid;
    searchText: any;
    dialog: any;
    columnMenus: any[];

    constructor(props: IGridProps) {
        super(props);
        KendoGroupableGrid.instance = this;
        let groups: any[] = [];
        let searchValue = '';
        let initialSearchParams: Condition[] = [];
        let tmpAdditionalFilter: any = { value: '' };
        if (props.searchParameters) {
            if (props.searchParameters.searchValue) {
                searchValue = props.searchParameters.searchValue
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
                tmpAdditionalFilter = this.props.additionalFilters.filter((v: any, i: any) => {
                    return v.default == true;
                })[0] || { name: '', value: '' };
            }
        }

        let aggr: any[] = [];

        this.props.groupTotalFields.map((f) => {
            aggr.push({ field: f, aggregate: 'sum' })
        })

        aggr.push({ field: this.props.groupField, aggregate: 'sum' })

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
        }

        let dState: any = { take: 10, skip: 0, searchValue: searchValue, dir: this.props.sortOrder || 'asc', sortField: this.props.sortField || '', additionalFilter: tmpAdditionalFilter.value, group: groups, data: [], total: 0 };
        if (this.props.externalDataState) {
            dState.take = this.props.externalDataState.take || dState.take;
            dState.skip = this.props.externalDataState.skip || dState.skip;
            dState.searchValue = this.props.externalDataState.searchValue || dState.searchValue;
            dState.dir = this.props.externalDataState.dir || dState.dir;
            dState.sortField = this.props.externalDataState.sortField || dState.sortField;
            dState.additionalFilter = this.props.externalDataState.additionalFilter || dState.additionalFilter;
        }

        if (this.props.searchParameters) {
            if (this.props.searchParameters.searchValue) {
                dState.searchValue = this.props.searchParameters.searchValue
            }
        }

        this.state = {
            gridWidth: document.getElementById(this.props.container) ? document.getElementById(this.props.container).clientWidth : 0,
            clientHeight: this.props.gridHeight ? this.props.gridHeight : document.getElementById(this.props.container) ? document.getElementById(this.props.container).clientHeight : 0,
            gridState: dState,
            gridStateTemplate: {
                searchValue: '',
                additionalFilter: '',
                sortField: this.props.sortField || '',
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
            searchText: dState.searchValue,
            dialogProps: dialogProps,
            loading: false,
            currencySymbol: props.currencySymbol,
            allFields: [],
            isSwitchOn: this.props.switchButton ? this.props.switchButton.checked : false,
            dataLength: 0,
            sortable: this.props.sortable || false,
            advancedSearchConditions: initialSearchParams,
            additionalFilterValue: tmpAdditionalFilter,
            gridHeaderChecked: false,
            pagination: props.pagination === undefined ? { pageSizes: [10, 20, 50, 100] } : props.pagination,
            showAdvancedSearchDialog: this.props.showAdvancedSearchDialog ? this.props.showAdvancedSearchDialog : false,
            rowMenus: [],
            allowAddButton: false,
            allowGridMenu: [],
            gridMessage: this.props.gridMessage || "Unable to find record on selected criteria."
        }

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
    }

    storeSearchParam(isSwitchOn?: boolean) {
        if (this.props.printTitle && this.props.showSearchBox) {
            let data = {
                advancedSearchConditions: this.state.advancedSearchConditions,
                additionalFilterValue: this.state.additionalFilterValue,
                dataState: {
                    additionalFilter: this.state.gridState.additionalFilter,
                    dir: this.state.gridState.dir,
                    searchValue: this.state.gridState.searchValue,
                    skip: this.state.gridState.skip,
                    sortField: this.state.gridState.sortField,
                    take: this.state.gridState.take
                },
                isSwitchOn: isSwitchOn || this.state.isSwitchOn
            }
            sessionStorage.setItem(this.props.printTitle, JSON.stringify(data))
        }
    }

    getStoredSearchParam() {
        if (this.props.printTitle && this.props.showSearchBox) {
            let param = sessionStorage.getItem(this.props.printTitle);
            if (param) {
                return JSON.parse(param);
            }
        }
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

    exportPDFClick(fieldUrl: string) {
        let selected1 = this.state.persistentSelection.filter((v: any) => { return v.selected === true })
        if (selected1.length == 0 && this.state.gridHeaderChecked === false) {
            window.Dialog.alert("Please select at least a row to export to excel.")
            return;
        }

        let buttons: any[] = [];
        buttons.push({
            primary: true,
            requireValidation: false,
            text: "Export",
            action: this.printPDF
        })
        buttons.push({
            primary: false,
            requireValidation: false,
            text: "Cancel",
            action: this.onDialogCancel
        })

        this.setState({
            dialogProps: {
                ...this.state.dialogProps,
                dialogTitle: 'Export to PDF',
                getUrl: fieldUrl,
                buttons: buttons,
                method: 'get',
                visible: true
            }
        }, KendoGroupableGrid.instance.dialog.openForm)
    }

    printPDF(e: any, columns: any) {
        this.setState({ loading: true }, () => {
            let sender = this;
            var gridColumns: any[] = [];
            var data: any[] = [];
            if (typeof (columns.selectedOptions) == "string") {
                let value = columns.selectedOptions;
                columns.selectedOptions = [];
                columns.selectedOptions.push(value)
            }
            if (!columns.selectedOptions) {
                window.Dialog.alert("Please select fields to export", "Notification");
                return;
            }
            for (var i = 0; i < columns.selectedOptions.length; i++) {
                let fieldIndex = columns.fieldNames.indexOf(columns.selectedOptions[i])
                gridColumns.push({
                    field: columns.selectedOptions[i],
                    title: unescape(columns.fieldLabels[fieldIndex]),
                    format: columns.fieldFormats[fieldIndex],
                    filter: columns.fieldFilters[fieldIndex]
                });
            }
            this.getSelectedItems((result: any) => {
                let rows = result.result || result;
                for (var i = 0; i < rows.length; i++) {
                    if (rows[i].selected == true) {
                        data.push(rows[i]);
                    }
                }
            });

            if (this.state.gridHeaderChecked) {
                this.loadAddDataForSelectAll((result: any) => {
                    let data = result.result || result;
                    let deSelected: any[] = this.state.persistentSelection.filter((v: any) => { return v.selected === false });
                    let nData: any[] = [];
                    for (let i = 0; i < data.length; i++) {
                        let index = deSelected.findIndex((v: any) => {
                            return v[sender.props.identityField] == data[i][sender.props.identityField];
                        })
                        if (index < 0) {
                            nData.push(data[i]);
                        }
                    }
                    sender.downloadPDF(nData, gridColumns)
                    sender.setState({ loading: false });
                })

            }
            else {
                this.downloadPDF(data, gridColumns)
                this.clearSelection();
                this.setState({ dialogProps: { ...this.state.dialogProps, visible: false } });
            }
        })
    }

    downloadPDF(data: any, gridColumns: any) {
        let printedBy = this.props.printedBy || ''
        let pdfTitle = this.props.printTitle || ''
        let landscape = (gridColumns.length > 6);
        if (gridColumns.length > 12 || gridColumns.length < 1) {
            window.Dialog.alert("Fields count must be within the range of 1 to 12.")
            this.setState({ loading: false }, this.forceUpdate);
            return;
        }

        if (data.length > 500) {
            window.Dialog.alert("There are more than 500 records. Export to PDF only supports upto 500 records. Please add additional filters and try again. You can also try exporting to Excel.")
            this.setState({ loading: false }, this.forceUpdate);
            return;
        }
        let today = getTodayDate();
        let sender = this;
        var grid = $("<div id='pdfGrid'></div>");
        grid.kendoGrid({
            toolbar: ["pdf"],
            pdf: {
                allPages: true,
                avoidLinks: true,
                paperSize: "A4",
                margin: { top: "2cm", left: "1cm", right: "1cm", bottom: "1cm" },
                landscape: landscape,
                repeatHeaders: true,
                template: `<div class="page-template"><div class="header"><img src="/img/logo.jpg" width="175" height="40" /> <span class="print-title">${pdfTitle}</span> <div style="float: right" class="printed-by-field">Printed By ${printedBy} on ${today}</div> </div><div class="watermark"></div><div class="footer">Page #: pageNum # of #: totalPages #</div></div>`,
                scale: 0.6
            },
            dataSource: data,
            height: 550,
            sortable: true,
            pageable: {
                refresh: true,
                pageSizes: true,
                buttonCount: 5
            },
            columns: gridColumns,
            pdfExport: function (e: any) {
                e.promise
                    .progress(function (e1: any) {
                    })
                    .done(function () {
                        sender.setState({ loading: false });
                    });
            }
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
            this.setState({ gridState: { ...this.state.gridState, data: { ...this.state.gridState.data, data: this.state.gridState.data.data } }, persistentSelection: [], gridHeaderChecked: false }, () => {
                this.storeSearchParam();
                this.loadGridData();
            });
        }

    }

    printExcel(e: any, columns: any) {
        this.setState({ loading: true }, () => {
            let sender = this;
            var gridColumns: any[] = [];
            var data: any[] = [];
            if (typeof (columns.selectedOptions) == "string") {
                let value = columns.selectedOptions;
                columns.selectedOptions = [];
                columns.selectedOptions.push(value)
            }
            if (!columns.selectedOptions) {
                window.Dialog.alert("Please select fields to export", "Notification");
                return;
            }
            for (var i = 0; i < columns.selectedOptions.length; i++) {
                let fieldIndex = columns.fieldNames.indexOf(columns.selectedOptions[i])
                gridColumns.push({
                    field: columns.selectedOptions[i],
                    title: unescape(columns.fieldLabels[fieldIndex]),
                    format: columns.fieldFormats[fieldIndex],
                    filter: columns.fieldFilters[fieldIndex]
                });
            }
            this.getSelectedItems((result: any) => {
                let rows = result.result || result;
                for (var i = 0; i < rows.length; i++) {
                    if (rows[i].selected == true) {
                        data.push(rows[i]);
                    }
                }
                if (this.state.gridHeaderChecked) {
                    this.loadAddDataForSelectAll((result: any) => {
                        let data = result.result;
                        let deSelected: any[] = this.state.persistentSelection.filter((v: any) => { return v.selected === false });
                        let nData: any[] = [];
                        for (let i = 0; i < data.length; i++) {
                            let index = deSelected.findIndex((v: any) => {
                                return v[sender.props.identityField] == data[i][sender.props.identityField];
                            })
                            if (index < 0) {
                                nData.push(data[i]);
                            }
                        }
                        sender.downloadExcel(nData, gridColumns)
                        sender.setState({ loading: false });
                    })
                }
                else {
                    this.downloadExcel(data, gridColumns)
                    this.clearSelection();
                    this.setState({ dialogProps: { ...this.state.dialogProps, visible: false } });
                }
            })
        })
    }

    downloadExcel(data: any, gridColumns: any) {
        let sender = this;
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
            columns: gridColumns,
            excelExport: function (e: any) {
                sender.setState({ loading: false });
            }
        });
        grid.data("kendoGrid").saveAsExcel();
    }

    loadAddDataForSelectAll(callback: any) {
        let dState = JSON.parse(JSON.stringify(this.state.gridState));
        dState.take = this.state.gridState.total;
        dState.skip = 0;
        let queryString = this.makeQueryString(dState);
        let caller = this;
        let separator = "?";
        if (this.props.dataUrl.indexOf("?") >= 0) {
            separator = "&"
        }
        let url = this.props.dataUrl + separator + queryString + "&t=" + (new Date()).getTime();
        if (this.state.advancedSearchConditions.length == 0) {
            Remote.get(url, (result: any) => {
                callback(result);
            }, (error: any) => { window.Dialog.alert(error, "Error") });
        }
        else {
            Remote.postData(url, this.state.advancedSearchConditions, (result: any) => {
                callback(result);
            }, (error: any) => { window.Dialog.alert(error, "Error") });
        }
    }

    getSelectedItems(callback: any = null) {
        if (this.state.gridHeaderChecked && callback) {
            this.loadAddDataForSelectAll((result: any) => {
                let data: any[] = [];
                for (let i = 0; i < result.result.length; i++) {
                    let index = this.state.persistentSelection.findIndex((v: any) => { return v.selected == false && v[this.props.identityField] == result.result[i][this.props.identityField] })
                    if (index < 0) {
                        data.push(result.result[i]);
                    }
                }
                callback(data);
            })
        }
        else if (callback) {
            let items: any[] = [];
            let rows = this.state.persistentSelection;
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].selected == true) {
                    items.push(rows[i]);
                }
            }
            callback(items);
        }
        else {
            let items: any[] = [];
            let rows = this.state.persistentSelection;
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].selected == true) {
                    items.push(rows[i]);
                }
            }
            return items;
        }

    }

    makeQueryString(state: any) {
        let data: string = "";
        for (let i in state) {
            if (data != "")
                data += "&"
            data += i + "=" + eval("state." + i)
        }
        return data;
    }

    exportExcelClick(fieldUrl: string) {
        let selected1 = this.state.persistentSelection.filter((v: any) => { return v.selected === true })
        if (selected1.length == 0 && this.state.gridHeaderChecked === false) {
            window.Dialog.alert("Please select at least a row to export to excel.")
            return;
        }

        let buttons: any[] = [];
        buttons.push({
            primary: true,
            requireValidation: false,
            text: "Export",
            action: this.printExcel
        })
        buttons.push({
            primary: false,
            requireValidation: false,
            text: "Cancel",
            action: this.onDialogCancel
        })

        this.setState({
            dialogProps: {
                ...this.state.dialogProps,
                dialogTitle: 'Export to Excel',
                getUrl: fieldUrl,
                buttons: buttons,
                method: 'get',
                visible: true
            }
        }, KendoGroupableGrid.instance.dialog.openForm)
    }

    onDialogCancel(e: any) {

    }

    openDialog(options: any) {
        return (<KendoDialog {...options}></KendoDialog>)
    }

    refresh(columnMenu: any) {
        this.clearSelection();
        this.loadGridColumns(columnMenu);
    }

    async componentDidMount() {
        let allowAddButton = false;
        let authRequest: any[] = [];
        let arrMenu: any[] = [];
        let width = $(this.props.container).parent().width();
        if (this.props.addRecord && this.props.addRecord.resource && this.props.addRecord.resourceAction) {
            arrMenu.push(this.props.addRecord)
            authRequest.push({
                resourceType: this.props.addRecord.resource,
                resourceAction: this.props.addRecord.resourceAction,
                extraParam: 'AddRecord'
            })
        }
        else {
            allowAddButton = true;
        }

        let allowGridMenu: any[] = [];
        if (this.props.gridMenu) {
            this.props.gridMenu.map(async (v: any, i: number) => {
                if (v.resource && v.resourceAction) {
                    arrMenu.push(v)
                    authRequest.push({
                        resourceType: v.resource,
                        resourceAction: v.resourceAction,
                        extraParam: 'GridMenu'
                    })
                }
                else {
                    allowGridMenu.push(v);
                }
            })
        }

        let rowMenus: any[] = [];
        if (this.props.rowMenus && this.props.rowMenus.map) {
            this.props.rowMenus.map(async (v: any, i: number) => {
                if (v.resource && v.resourceAction) {
                    arrMenu.push(v)
                    authRequest.push({
                        resourceType: v.resource,
                        resourceAction: v.resourceAction,
                        extraParam: 'RowMenu'
                    })
                }
                else {
                    rowMenus.push(v);
                }
            })
        }

        if (authRequest.length > 0) {
            let response = await Remote.postDataAsync('/IsAuthorizedAction', authRequest);
            if (response.ok) {
                let json = await response.json();
                if (json.status === true) {
                    let auths: any[] = json.output;
                    auths.map((v: any, i: number) => {
                        if (v.extraParam === "AddRecord" && v.isAuthorized === true) {
                            allowAddButton = true;
                        }
                        else if (v.extraParam === "GridMenu" && v.isAuthorized === true) {
                            allowGridMenu.push(arrMenu[i]);
                        }
                        else if (v.extraParam === "RowMenu" && v.isAuthorized === true) {
                            rowMenus.push(arrMenu[i]);
                        }
                    })
                }
            }
        }

        this.setState({ gridWidth: width ? width : 0, allowAddButton: allowAddButton, allowGridMenu: allowGridMenu, rowMenus: rowMenus, clientHeight: document.getElementById("groupableGrid") ? document.getElementById("groupableGrid").clientHeight : 0 });
        this.loadGridColumns(null);
    }

    loadGridColumns(columnMenu: any) {
        let sender = this;
        this.setState({ loading: true }, () => {
            this.renderLoadingPanel();

            Remote.get(this.props.columnUrl, (result: any) => {
                let data: any[] = [];
                result = result.sort((a: any, b: any) => parseFloat(a.orderIndex) - parseFloat(b.orderIndex));
                sender.setState({ allFields: result })
                for (let i in result) {
                    if (i == "0") {
                        data.push({
                            fieldName: 'menu',
                            fieldLabel: ' ',
                            isSortable: false,
                            isFilterable: false,
                            orderIndex: 1,
                            showHeaderText: false
                        })
                    }
                    let row = result[i];
                    row.orderIndex = data.length;
                    row.show = (this.props.hideColumns === undefined || this.props.hideColumns.indexOf(row.fieldName) === -1) && row.visibleToGrid == true;
                    data.push(row)
                }
                sender.setState({ fields: data }, sender.forceUpdate)
                sender.loadGridData(columnMenu);
            }, (err: any) => {
                window.Dialog.alert(err)
            })
        })

    }

    getCommandCell(e: any) {
        if (!this.props.rowMenus) {
            return;
        }
        return (<td className="rowMenu"><CommandCell menuItems={this.state.rowMenus} dataItem={e.dataItem} parent={this} /></td>);
    }

    updateColumnVisibility(columns: any) {
        let arr: any[] = [];
        arr.push({
            fieldName: 'menu',
            fieldLabel: ' ',
            isSortable: false,
            isFilterable: false,
            orderIndex: 1,
            showHeaderText: false
        })

        for (let i in columns) {
            columns[i].visibleToGrid = columns[i].show;
            columns[i].orderIndex = parseInt(i) + 1;
            arr.push(columns[i])
        }
        arr = arr.sort((a: any, b: any) => parseFloat(a.orderIndex) - parseFloat(b.orderIndex));
        this.setState({ fields: arr });
    }

    renderFields() {
        let fields: any[] = [];
        let deductionColumnWidth = 150;
        let width: string = 150 + "";
        let columns = this.state.fields.filter(c => c.show == true);
        columns = this.state.fields.filter(c => c.show == true || c.fieldName == 'menu');
        let totalColumns = deductionColumnWidth === 150 ? columns.length - 1 : columns.length;

        columns.forEach((e: any, index: number) => {
            let percent = e.columnWidth ? e.columnWidth : (100 / totalColumns)
            width = this.getPixel(percent, deductionColumnWidth) + "";
            if (e.columnMinimumWidth > width) {
                width = e.columnMinimumWidth;
            }

            if (index == 0 && this.props.rowMenus) {
                fields.push(<Column key={index} field={e.fieldName} title={e.fieldLabel} sortable={e.isSortable} filterable={e.isFilterable} orderIndex={e.orderIndex} width={"50px"} cell={this.getCommandCell} className={e.gridColumnCss} headerClassName={e.gridColumnCss} />)
            }
            else {
                if (index == columns.length - 1 && e.show && !this.props.showColumnMenu == false) {
                    if (e.clickable && this.props.itemNavigationUrl && this.props.itemNavigationUrl.length > 0 && (!this.props.navigationFields || (this.props.navigationFields && this.props.navigationFields.indexOf(e.fieldName) >= 0))) {
                        fields.push(<Column key={index} field={e.fieldName} filter={e.type} format={e.format} title={e.fieldLabel} sortable={e.isSortable} filterable={e.isFilterable} orderIndex={e.orderIndex} width={width + "px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} columnMenu={
                            props =>
                                <GridColumnMenu
                                    gridColumns={columns}
                                    {...props}
                                    grid={this}
                                    columns={this.state.allFields}
                                />
                        } cell={
                            (props: any) => {
                                let navUrl = "";
                                this.props.itemNavigationUrl.map((v: any, i: number) => {
                                    if ((props.dataItem.items && props.dataItem.items[0][v.field] === v.value) || (props.dataItem[v.field] !== undefined && props.dataItem[v.field] === v.value)) {
                                        navUrl = v.url;
                                    }
                                })
                                return (<td>
                                    <a href={navUrl + props.dataItem[this.props.identityField]}>
                                        {props.dataItem[props.field]}
                                    </a>
                                </td>)
                            }
                        } />)
                    }
                    else {
                        if (e.format === "{0:d}") {
                            fields.push(<Column key={index} field={e.fieldName} title={e.fieldLabel} filter={e.type} format={e.format} sortable={e.isSortable} filterable={e.isFilterable} orderIndex={e.orderIndex} width={width + "px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} columnMenu={
                                props =>
                                    <GridColumnMenu
                                        gridColumns={columns}
                                        {...props}
                                        grid={this}
                                        columns={this.state.allFields}
                                    />
                            } cell={(prop) => {
                                const value = prop.dataItem[prop.field];
                                return (
                                    <td className={e.gridColumnCss}> {formatUSDate(value, "en")}
                                    </td>
                                );
                            }} />)
                        }
                        else if (e.format === "{0:c}") {
                            fields.push(<Column key={index} field={e.fieldName} title={e.fieldLabel} filter={e.type} format={e.format} sortable={e.isSortable} filterable={e.isFilterable} orderIndex={e.orderIndex} width={width + "px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} columnMenu={
                                props =>
                                    <GridColumnMenu
                                        gridColumns={columns}
                                        {...props}
                                        grid={this}
                                        columns={this.state.allFields}
                                    />
                            } cell={(prop) => {
                                const value = prop.dataItem[prop.field];
                                let currency = prop.dataItem['currency'] || '';
                                let money = prop.dataItem[prop.field] || 0;
                                if (prop.rowType === 'groupFooter') {
                                    money = prop.dataItem.aggregates[prop.field].sum;
                                    currency = prop.dataItem.items[0]['currency']
                                }
                                return (
                                    <td className={e.gridColumnCss}> {currency + ' ' + numberWithCommas(money.toFixed(2))}
                                    </td>
                                );
                            }} />)
                        }
                        else {
                            fields.push(<Column key={index} field={e.fieldName} title={e.fieldLabel} filter={e.type} format={e.format} sortable={e.isSortable} filterable={e.isFilterable} orderIndex={e.orderIndex} width={width + "px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} columnMenu={
                                props =>
                                    <GridColumnMenu
                                        gridColumns={columns}
                                        {...props}
                                        grid={this}
                                        columns={this.state.allFields}
                                    />
                            } cell={(prop: any) => {
                                let css = e.gridColumnCss + " ";
                                const value = prop.dataItem[prop.field];
                                let money = prop.dataItem[prop.field];
                                if (prop.rowType === 'groupFooter' && prop.dataItem.aggregates[prop.field] && prop.dataItem.aggregates[prop.field].sum && !isNaN(money)) {
                                    money = prop.dataItem.aggregates[prop.field].sum;
                                }
                                return (
                                    <td className={css}> {money}
                                    </td>
                                );
                            }} />)
                        }
                    }
                }
                else if (e.show) {
                    if (e.clickable && this.props.itemNavigationUrl && (!this.props.navigationFields || (this.props.navigationFields && this.props.navigationFields.indexOf(e.fieldName) >= 0))) {
                        fields.push(<Column key={index} field={e.fieldName} title={e.fieldLabel} filter={e.type} format={e.format} sortable={e.isSortable} filterable={e.isFilterable} orderIndex={e.orderIndex} width={width + "px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} cell={
                            (props: any) => {
                                let navUrl = "";
                                this.props.itemNavigationUrl.map((v: any, i: number) => {
                                    if ((props.dataItem.items && props.dataItem.items[0][v.field] === v.value) || (props.dataItem[v.field] !== undefined && props.dataItem[v.field] === v.value)) {
                                        navUrl = v.url;
                                    }
                                })
                                return (<td>
                                    <a href={navUrl + props.dataItem[this.props.identityField]}>
                                        {props.dataItem[props.field]}
                                    </a>
                                </td>)
                            }
                        } />)
                    }
                    else {
                        if (e.format === "{0:c}") {
                            fields.push(<Column key={index} field={e.fieldName} title={e.fieldLabel} filter={e.type} format={e.format} sortable={e.isSortable} filterable={e.isFilterable} orderIndex={e.orderIndex} width={width + "px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} cell={(prop) => {
                                const value = prop.dataItem[prop.field];
                                let currency = prop.dataItem['currency'] || '';
                                let money = prop.dataItem[prop.field] || 0;
                                if (prop.rowType === 'groupFooter') {
                                    money = prop.dataItem.aggregates[prop.field].sum || 0;
                                    currency = prop.dataItem.items[0]['currency'] || ''
                                }
                                return (
                                    <td className={e.gridColumnCss}> {currency + ' ' + numberWithCommas(money.toFixed(2))}
                                    </td>
                                );
                            }} />)
                        }
                        else if (e.format === "{0:d}") {
                            fields.push(<Column key={index} field={e.fieldName} title={e.fieldLabel} filter={e.type} format={e.format} sortable={e.isSortable} filterable={e.isFilterable} orderIndex={e.orderIndex} width={width + "px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} cell={(prop) => {
                                const value = prop.dataItem[prop.field];
                                return (
                                    <td className={e.gridColumnCss}> {formatUSDate(value, "en")}
                                    </td>
                                );
                            }} />)
                        }
                        else {
                            fields.push(<Column key={index} field={e.fieldName} title={e.fieldLabel} filter={e.type} format={e.format} sortable={e.isSortable} filterable={e.isFilterable} orderIndex={e.orderIndex} width={width + "px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} cell={(props: any) => {
                                let css = e.gridColumnCss + " ";
                                if (this.props.rowIconSettings && props.field == this.props.rowIconSettings.column) {
                                    this.props.rowIconSettings.icons.forEach((v: IconSetting) => {
                                        if (props.dataItem[v.field] === v.value) {
                                            css += " " + v.css
                                        }
                                    })
                                }
                                let value = props.dataItem[props.field];
                                if (value === true) {
                                    value = "Yes"
                                }
                                else if (value === false) {
                                    value = "No"
                                }
                                if (props.rowType === 'groupFooter' && props.dataItem.aggregates[props.field] && props.dataItem.aggregates[props.field].sum) {
                                    value = props.dataItem.aggregates[props.field].sum;
                                    if (isNaN(value)) {
                                        value = props.dataItem.value
                                    }
                                }
                                return (<td className={css}>{value}</td>);
                            }} />)
                        }

                    }

                }

            }

        })
        return fields;
    }

    onPageChange(e: any) {
        this.setState({ gridState: { ...this.state.gridState, skip: e.page.skip, take: e.page.take, searchValue: this.state.gridState.searchValue, dir: this.state.gridState.dir, sortField: this.state.gridState.sortField } }, () => { this.storeSearchParam(); this.loadGridData() })
    }

    loadGridData(columnMenu: any = undefined) {
        this.storeSearchParam();
        this.setState({ loading: true });
        let sender = this;
        let sign = "?";
        if (this.props.dataUrl.indexOf("?") >= 0) {
            sign = "&"
        }

        let additional = "";
        if (this.state.gridState.additionalFilter) {
            additional = "&additionalFilter=" + this.state.gridState.additionalFilter;
        }

        if (this.state.gridState.method === 'post' || (this.props.searchParameters && this.props.searchParameters.advancedSearchConditions && this.props.searchParameters.advancedSearchConditions.length > 0)) {
            let queryString = sign + "searchValue=" + this.state.gridState.searchValue + "&take=" + this.state.gridState.take + "&skip=" + this.state.gridState.skip + "&sortField=" + this.state.gridState.sortField + "&dir=" + this.state.gridState.dir + additional
            let postValue = this.state.advancedSearchConditions;
            Remote.postData(this.props.dataUrl + queryString, postValue, (data: any) => {
                let gState = sender.state.gridStateTemplate;
                gState.sortField = sender.state.gridState.sortField;
                gState.dir = sender.state.gridState.dir;
                gState.take = data.length || data.result.length;
                let result: any[] = this.manageDataSelection(data.result);
                let grdData = process(result, gState);
                if (sender.props.onGridLoaded) {
                    sender.props.onGridLoaded(data.result)
                }

                grdData.data.sort((a: any, b: any) => {
                    let aindex = result.findIndex((v: any, i: any) => { if (v[sender.props.groupField] == a.value) return i; })
                    let bindex = result.findIndex((v: any, i: any) => { if (v[sender.props.groupField] == b.value) return i; })
                    return aindex < bindex ? -1 : 1;
                })

                sender.setState({ gridState: { ...sender.state.gridState, data: grdData, total: data.count }, dataLength: data.result.length, loading: false })
                if (columnMenu) {
                    columnMenu.refresh()
                }

            }, (err: any) => {
                sender.setState({ loading: false, gridMessage: err }, this.forceUpdate)
            })
        }
        else {
            let queryString = sign + "searchValue=" + this.state.gridState.searchValue + "&take=" + this.state.gridState.take + "&skip=" + this.state.gridState.skip + "&sortField=" + this.state.gridState.sortField + "&dir=" + this.state.gridState.dir + additional
            Remote.get(this.props.dataUrl + queryString, (data: any) => {
                let gState = sender.state.gridStateTemplate;
                gState.sortField = sender.state.gridState.sortField;
                gState.dir = sender.state.gridState.dir;
                gState.take = data.length;
                let result: any[] = this.manageDataSelection(data.result);
                result.map((v: any, i: number) => {
                    v.sortIndex = i;
                })
                let grdData: DataResult = process(result, gState);
                grdData.data.sort((a: any, b: any) => {
                    let aindex = result.findIndex((v: any, i: any) => { if (v[sender.props.groupField] == a.value) return i; })
                    let bindex = result.findIndex((v: any, i: any) => { if (v[sender.props.groupField] == b.value) return i; })
                    return aindex < bindex ? -1 : 1;
                })
                sender.setState({ gridState: { ...sender.state.gridState, data: grdData, total: data.count }, dataLength: data.result.length, loading: false })
                if (columnMenu) {
                    columnMenu.refresh()
                }
                if (sender.props.onGridLoaded) {
                    sender.props.onGridLoaded(data.result)
                }

            }, (err: any) => {
                sender.setState({ loading: false, gridMessage: err }, this.forceUpdate)
            })
        }
    }

    manageDataSelection(data: any[]) {
        for (let i in data) {
            let index = this.state.persistentSelection.findIndex((v: any) => { return v[this.props.identityField] === data[i][this.props.identityField] });
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
        return (<Column field="selected" width="50px" />)
    }

    headerCheckboxClicked(e: any) {

    }

    GridCellRenderer(td: any, cellProps: any) {
        if (cellProps.rowType === 'groupFooter') {
            let index = this.props.groupTotalFields.findIndex((v: any, index: any) => { return v === cellProps.field });
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
                return (
                    <td>
                        {/*currency + " " + eval("cellProps.dataItem.aggregates." + this.props.groupTotalField + ".sum.toLocaleString(navigator.language, { minimumFractionDigits: 2 })")*/}
                    </td>
                );
            }
            else if (cellProps.field === this.props.groupTotalLabelField) {
                return (
                    <td>
                        Total
                    </td>
                );
            }
            return (<td></td>);
        }
        else if (cellProps.rowType === 'groupHeader') {
            if (cellProps.field === 'value' && this.props.showGroupHeader !== false) {
                return td;
            }
            else if (cellProps.field === 'selected') {
                return (<td></td>);
            }
            else if (cellProps.field === 'contactNumber') {
                return (<td></td>);
            }
            else if (cellProps.field === 'fundedAmount') {
                return (<td></td>);
            }
            else if (this.props.showGroupHeader === false) {
                return (<td></td>);
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
                            return (<td className={css}>{this.props.rowIconSettings.icons[i].icon} {cellProps.dataItem[cellProps.field]}</td>)
                        }
                    }
                }
            }
        }
        return td;
    }

    dataStateChange = (event: any) => {
        let aggr: any[] = [];

        this.props.groupTotalFields.map((f) => {
            aggr.push({ field: f, aggregate: 'sum' })
        })

        aggr.push({ field: this.props.groupField, aggregate: 'sum' })

        let groups: any[] = [];
        groups.push({
            field: this.props.groupField, aggregates: aggr
        });
        this.setState({ gridState: { ...this.state.gridState, group: groups } });
    }

    expandChange(event: any) {
        event.dataItem[event.target.props.expandField] = event.value;
        this.setState({
            gridState: {
                ...this.state.gridState,
                data: Object.assign({}, this.state.gridState.data),
                dataState: this.state.gridState.dataaState
            }
        });
    }

    componentWillUpdate() {
        if (this.state.gridWidth != document.getElementById(this.props.container).clientWidth) {
            this.setState({ gridWidth: document.getElementById(this.props.container).clientWidth })
            return true;
        }
        return false;
    }

    gridHeaderCellRenderer(e: any, e1: any) {
        if (e1.field == "selected") {
            return (<label />)
        }
        return e;
    }

    gridSelectionChange(e: any) {
        let isSelected = e.syntheticEvent.target.checked;
        for (let i = 0; i < this.state.gridState.data.data.length; i++) {
            for (let j = 0; j < this.state.gridState.data.data[i].items.length; j++) {
                if (eval("this.state.gridState.data.data[" + i + "].items[" + j + "]." + this.props.identityField) == eval("e.dataItem." + this.props.identityField)) {
                    this.state.gridState.data.data[i].items[j].selected = isSelected;
                    this.managePersitentSelection(this.state.gridState.data.data[i].items[j]);
                }
            }
        }
        this.setState({ gridState: { ...this.state.gridState, data: { ...this.state.gridState.data, data: this.state.gridState.data.data } } });
    }

    findSelectedIndex(niddle: any): number {
        for (let i in this.state.persistentSelection) {
            let nd = JSON.parse(JSON.stringify(niddle))
            let nd1 = JSON.parse(JSON.stringify(this.state.persistentSelection[i]))
            nd = {
                ...nd,
                selected: true
            }
            nd1 = {
                ...nd1,
                selected: true
            }
            if (JSON.stringify(nd1) == JSON.stringify(nd)) {
                return parseInt(i);
            }
        }
        return -1;
    }

    onHeaderSelectionChange(event: any) {
        const checked = event.syntheticEvent.target.checked;
        let gridData = this.state.gridState.data.data;
        gridData.forEach((aggregate: any) => {
            aggregate.items.forEach((item: any) => {
                item.selected = checked
            })
        });
        this.setState({
            gridHeaderChecked: event.syntheticEvent.target.checked,
            gridState: {
                ...this.state.gridState,
                data: {
                    ...this.state.gridState.data,
                    data: gridData
                }
            },
            persistentSelection: []
        });
    }

    _selectionChange(data: any) {
        this.managePersitentSelection(data);
    }

    managePersitentSelection(item: any) {
        let existing = this.state.persistentSelection || [];
        let index: number = this.findSelectedIndex(item);
        if (index === -1) {
            existing.push(item);
        }
        else {
            existing[index] = item;
        }
        this.setState({ persistentSelection: existing });
    }

    hasBeenSelected(niddle: any) {
        for (let i in this.state.persistentSelection) {
            let nd = JSON.parse(JSON.stringify(niddle))
            let nd1 = JSON.parse(JSON.stringify(this.state.persistentSelection[i]))
            nd = {
                ...nd,
                selected: true
            }
            nd1 = {
                ...nd1,
                selected: true
            }
            if (JSON.stringify(nd1) == JSON.stringify(nd)) {
                return true;
            }
        }
        return false;
    }

    onGridMenuClick(e: any) {
        e.nativeEvent.preventDefault()
        let index = e.itemIndex;
        this.props.gridMenu[index].action(e, this);
    }



    onSwitchChange(e: any) {
        this.storeSearchParam(e.target.value);
        if (this.props.switchButton) {
            this.props.switchButton.action(e.target.value);
        }
    }

    renderSwitchButton() {
        if (this.props.switchButton != undefined) {
            return (<div className="switch-btn"><Switch offLabel={this.props.switchButton.offLabel} onLabel={this.props.switchButton.onLabel} className="switchButton" checked={this.state.isSwitchOn} onChange={this.onSwitchChange} />{this.state.isSwitchOn ? this.props.switchButton.offLabel : this.props.switchButton.onLabel}</div>);
            // return (<Switch offLabel={this.props.switchButton.offLabel} onLabel={this.props.switchButton.onLabel} className="switchButton" checked={this.state.isSwitchOn} onChange={this.onSwitchChange} />);
        }
    }

    renderAddRecordButton() {
        if (this.props.addRecord && this.props.addRecord.text != '' && this.state.allowAddButton === true) {
            return (<Button primary={true} onClick={this.addRecordClicked}>{this.props.addRecord.text}</Button>) //icon={this.props.addRecord.icon}
        }
        return null;
    }

    addRecordClicked(e: any) {
        this.props.addRecord.action(e, this);
    }

    renderMoreButtons() {
        if (this.props.showGridAction == undefined || this.props.showGridAction == true) {
            return (
                <div className="text-right">
                    {this.renderSwitchButton()}
                    {this.renderAddRecordButton()} &nbsp;
                    <SplitButton text="Actions" className="actions-dropdown" items={this.state.allowGridMenu} itemRender={(e: any) => {
                        return (<a href="#" title={e.item.title || ''} style={{ color: 'inherit', textDecoration: 'none' }}><span className={"k-icon k-i-" + e.item.icon} /> {e.item.text} </a>)
                    }} onItemClick={this.onGridMenuClick} />
                </div>
            )
        }
    }

    onSearchFieldKeyPress(e: any) {
        if (e.which == 13) {
            this.setState({
                gridState: {
                    ...this.state.gridState,
                    searchValue: this.searchText.value,
                    skip: 0
                },
                searchText: this.searchText.value
            }, this.loadGridData);
        }
    }

    onSearchButtonClicked(e: any) {
        e.nativeEvent.preventDefault()
        this.setState({
            gridState: {
                ...this.state.gridState,
                searchValue: this.searchText.value,
                skip: 0
            }
        }, this.loadGridData);
    }

    renderSearchBox() {
        return (
            <div className="search-form-r">
                {this.renderAdditionalFilters()}
                <div className="input-group mb-3 mb-sm-0">
                    <Input autoFocus={true} onKeyPress={this.onSearchFieldKeyPress} placeholder={this.props.searchPlaceHolder || "Search"} ref={(c) => { this.searchText = c }} className="form-control" />
                    <div className="input-group-append">
                        <div className="input-group-text"><a href={"javascript:void(0)"} onClick={this.onSearchButtonClicked} className="k-icon k-i-search">&nbsp;</a></div>
                    </div>
                </div>
                {this.renderAdvancedSearch()}
                <div className="clearfix"></div>

            </div>
        )
    }

    onSearchFillsDelete(condition: Condition) {
        if (condition.Operator.OperatorId === null) {
            this.setState({
                gridState: {
                    ...this.state.gridState,
                    searchValue: ''
                },
                searchText: ''
            }, this.loadGridData)
        }
        else {
            let conditions: any[] = [];
            for (let i in this.state.advancedSearchConditions) {
                if (JSON.stringify(this.state.advancedSearchConditions[i]) != JSON.stringify(condition)) {
                    conditions.push(this.state.advancedSearchConditions[i])
                }
            }

            this.setState({ advancedSearchConditions: conditions }, this.loadGridData);
        }
    }

    onClearAllPills(e: any) {
        this.setState({
            gridState: {
                ...this.state.gridState,
                searchValue: ''
            },
            searchText: '',
            advancedSearchConditions: []
        }, () => {
            this.storeSearchParam();
            this.loadGridData();
        })
    }

    renderSearchBoxAndActions() {
        let searchConditions: Condition[] = [];
        if (this.state.searchText) {
            let cond = new Condition();
            cond.Attribute = new Attribute(null, null, "Any field", 0, false, "");
            cond.Operator = new Operator(null, 0, "Contains", 0);
            cond.ConditionId = null;
            cond.Value = this.state.searchText;
            searchConditions.push(cond);
        }
        for (let i in this.state.advancedSearchConditions) {
            searchConditions.push(this.state.advancedSearchConditions[i]);
        }
        if (this.props.showSearchBox == undefined || this.props.showSearchBox == true) {
            return (
                <div className={"row mb-3"}>
                    <div className="col mb-md-3 mb-lg-0">
                        {this.renderSearchBox()}

                    </div>
                    <div className="col-md-12 col-lg-auto">
                        {this.renderMoreButtons()}
                    </div>
                    <div className="col-md-12">
                        <SearchPills conditions={searchConditions} onPillDelete={this.onSearchFillsDelete} onClearAll={this.onClearAllPills} />
                    </div>
                </div>
            )
        }
        else if (this.props.showGridAction) {
            return (
                <div className={"row mb-3"}>
                    <div className="col mb-md-3 mb-lg-0">

                    </div>
                    <div className="col-md-12 col-lg-auto">
                        {this.renderMoreButtons()}
                    </div>
                </div>
            )
        }
        else {
            return (
                <div className={"row mb-3"}>
                    <div className="col mb-md-3 mb-lg-0">
                        {this.renderAdditionalFilters()}
                    </div>
                    <div className="col-md-12 col-lg-auto">

                    </div>
                </div>
            )
        }
    }

    renderDialog() {
        return (<KendoDialog ref={(c) => { this.dialog = c }} {...this.state.dialogProps}></KendoDialog>)
    }

    renderLoadingPanel() {
        if (this.state.loading) {
            return (<div className="k-loading-mask">
                <span className="k-loading-text">Loading</span>
                <div className="k-loading-image"></div>
                <div className="k-loading-color"></div>
            </div>)
        }
    }

    onSortChange(e: any) {
        this.setState({
            gridState: {
                ...this.state.gridState,
                sortField: e.sort[0].field,
                dir: e.sort[0].dir === this.state.gridState.dir && e.sort[0].dir === 'asc' ? 'desc' : 'asc',
            }
        }, () => { this.storeSearchParam(); this.loadGridData(); });
    }

    onAdvancedSearchApply(conditions: any[]) {
        this.setState({ advancedSearchConditions: conditions, gridState: { ...this.state.gridState, method: 'post', skip: 0 } }, this.loadGridData);
    }

    onConditionChange(conditions: any[]) {

    }

    onAdditionalFilterChange(e: any) {
        let value = this.props.additionalFilters[e.itemIndex];
        this.setState({ additionalFilterValue: value, gridState: { ...this.state.gridState, additionalFilter: value.value, skip: 0 } }, this.loadGridData);
    }

    renderAdvancedSearch() {
        if (this.state.showAdvancedSearchDialog) {
            return (<AdvancedSearchDialog resourceIds={this.props.advancedSearchEntity} selectedConditions={this.state.advancedSearchConditions} onApply={this.onAdvancedSearchApply} onConditionChange={this.onConditionChange} />);
        }
    }

    renderAdditionalFilters() {
        if (this.props.additionalFilters && this.props.additionalFilters.length > 0) {
            return (<div className="float-left mr-3 filterButtonContainer">
                <SplitButton buttonClass="filterButton" popupSettings={{ popupClass: "filterButton" }} text={this.state.additionalFilterValue.name} key="value" textField="name" className="actions-dropdown" items={this.props.additionalFilters} onItemClick={this.onAdditionalFilterChange} />
            </div>)
        }
        else {
            return (<div className=""></div>)
        }
    }

    getPixel(percent: number, adjust: number) {
        let wd = $("#" + this.props.container).width();
        wd = wd - adjust;
        if (!isNaN(wd)) {
            wd = (wd / 100) * percent
            return wd;
        }
        else {
            return percent;
        }
    }


    render() {
        return (<div id="groupableGrid">
            {this.renderSearchBoxAndActions()}
            {this.props.description && <div className="grid-description" dangerouslySetInnerHTML={{ __html: unescape(this.props.description) }}></div>}
            <Grid ref={(c) => { this.grid = c; }} sortable={true} onSortChange={this.onSortChange} style={{ maxHeight: this.state.clientHeight }} selectedField="selected" {...this.state.gridState} groupable={{ footer: 'visible' }} onExpandChange={this.expandChange} onPageChange={this.onPageChange} onHeaderSelectionChange={this.onHeaderSelectionChange}
                expandField="expanded" pageable={this.state.pagination} onDataStateChange={this.dataStateChange} cellRender={this.GridCellRenderer} onSelectionChange={this.gridSelectionChange} pageSize={this.state.dataLength}  >
                {this.renderSelectionField()}
                {this.renderFields()}
                <GridNoRecords>
                    {this.state.gridMessage}
                </GridNoRecords>
            </Grid>
            {this.renderDialog()}
            {this.renderLoadingPanel()}
        </div>);
    }

}