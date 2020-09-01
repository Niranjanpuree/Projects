import 'core-js';
import * as React from "react"
import * as ReactDOM from "react-dom"
import { Grid, GridColumn as Column, GridNoRecords } from '@progress/kendo-react-grid';
import { Input, Switch } from "@progress/kendo-react-inputs";
import { CommandCell } from "./CommandCell";
import { GridDataLoader } from "./GridDataLoader"
import { KendoDialog } from "../Dialog/Dialog"
import { SplitButton, Button } from "@progress/kendo-react-buttons";
import { Redirect, BrowserRouter } from 'react-router-dom';
import { GridColumnMenu } from "./GridColumnMenu";
import { Remote } from "../Remote/Remote";
import { AdvancedSearchDialog } from "../../Component/AdvancedSearch/AdvancedSearchDialog";
import { SearchPills } from "./SearchPills"
import { Condition, Operator, Attribute } from "../../Component/Entities/Condition";
import { concat } from "@progress/kendo-data-query/dist/npm/transducers";
import { ComboBox } from '@progress/kendo-react-dropdowns';
import { formatDate } from '@telerik/kendo-intl';
import { SplitButtonProps } from '@progress/kendo-react-buttons/dist/npm/ListButton/models/ListButtonProps';

declare var window: any;
declare var numberWithCommas: any;
declare var formatUSDate: any;
declare var formatUSDatetime: any;
declare var getTodayDate: any;
declare var $: any;
declare var Dialog: any;

interface IGridProps {
    printTitle?: string;
    printedBy?: string;
    dataURL: string;
    fieldUrl: string;
    exportFieldUrl?: string;
    rowMenus?: any;
    gridMenu?: any[];
    addRecord?: any;
    gridWidth?: number;
    itemNavigationUrl?: string;
    navigationFields?: any[];
    identityField: string;
    showSearchBox?: boolean;
    showGridAction?: boolean;
    showSelection?: boolean;
    parent?: any;
    gridHeight?: string;
    hideColumns?: any[];
    switchButton?: any;
    selectionField?: string;
    searchParameters?: any;
    additionalFilters?: any[];
    showColumnMenu?: boolean;
    showAddNewButton?: boolean;
    showAdvanceSearchBox?: boolean;
    rowIconSettings?: RowIconSettings;
    advancedSearchEntity?: string[];
    onMoreLinkClicked?: any;
    onGridLoaded?: any;
    pageSize?: any;
    externalDataState?: any;
    onRowClick?: any;
    externalFilters?: any;
    sortField?: any;
    sortOrder?: any;
    description?: any;
    searchPlaceHolder?: string;
    gridMessage?: string;
}

interface IGridState {
    gridFields: any[];
    fields: any[];
    allFields: any[];
    showSelection: boolean;
    sortField: string;
    pageable: any;
    dataState: any;
    data: any;
    dialogProps: any;
    redirect: boolean;
    submittableFormParam: any;
    persistentSelection: any[];
    gridStyle: any;
    sort: any[];
    parentWidth: any;
    currentColumnMenu: any;
    isSwitchOn: boolean;
    gridHeaderChecked: boolean;
    advancedSearchConditions: Condition[];
    searchText: string;
    additionalFilterValue: any;
    resetPageIndex: boolean;
    rowMenus: any[];
    allowAddButton: boolean;
    allowGridMenu: any[];
    isLoading: boolean;
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

export class KendoGrid extends React.Component<IGridProps, IGridState> {
    static instance: KendoGrid = null;
    gridElement: any;
    dataLoader: GridDataLoader;
    dialog: any;
    searchText: any;
    columnFilterCell: any;
    componentLoaded: boolean = false; //uses for data loaded
    advancedSearchDialog: AdvancedSearchDialog;
    isComponentMounted: boolean = false;

    constructor(props: IGridProps) {
        super(props);
        this.gridElement = React.createRef();
        KendoGrid.instance = this;
        let buttons: any[] = [];
        this.isComponentMounted = false;
        let isSwitchOn = this.props.switchButton ? this.props.switchButton.checked : false;

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
            tmpAdditionalFilter = this.props.additionalFilters.filter((v: any, i: any) => {
                return v.default == true;
            })[0] || { name: '', value: '' };
        }

        buttons.push({
            primary: true,
            requireValidation: false,
            text: "",
            action: this.printPDF
        })
        buttons.push({
            primary: false,
            requireValidation: false,
            text: "",
            action: this.onDialogCancel
        })
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
        }

        let dState: any = { take: this.props.pageSize || 10, skip: 0, searchValue: searchValue, dir: this.props.sortOrder || 'asc', sortField: this.props.sortField || '', additionalFilter: tmpAdditionalFilter.value };
        if (this.props.externalDataState) {
            dState.take = this.props.externalDataState.take || dState.take;
            dState.skip = this.props.externalDataState.skip || dState.skip;
            dState.searchValue = this.props.externalDataState.searchValue || dState.searchValue;
            dState.dir = this.props.externalDataState.dir || dState.dir;
            dState.sortField = this.props.externalDataState.sortField || dState.sortField;
            dState.additionalFilter = this.props.externalDataState.additionalFilter || dState.additionalFilter;
        }

        let storedToken = this.getStoredSearchParam();
        if (storedToken) {
            dState = storedToken.dataState;
            tmpAdditionalFilter = storedToken.additionalFilterValue;
            initialSearchParams = storedToken.advancedSearchConditions;
        }

        this.state = {
            gridFields: [],
            fields: [],
            allFields: [],
            showSelection: this.props.showSelection == undefined || this.props.showSelection == true,
            sortField: this.props.sortField || '',
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
            isSwitchOn: isSwitchOn,
            gridHeaderChecked: false,
            advancedSearchConditions: initialSearchParams,
            searchText: '',
            additionalFilterValue: tmpAdditionalFilter,
            resetPageIndex: false,
            rowMenus: [],
            allowAddButton: false,
            allowGridMenu: [],
            isLoading: false,
            gridMessage: this.props.gridMessage || "Unable to find record on selected criteria."
        };

        this.selectionChange = this.selectionChange.bind(this);
        this.headerSelectionChange = this.headerSelectionChange.bind(this)
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
        this.renderHelpText = this.renderHelpText.bind(this);
        this.renderHelpTextOnly = this.renderHelpTextOnly.bind(this);
        this.storeSearchParam = this.storeSearchParam.bind(this);

        let prop: any = this.props;
        if (prop.gridStyle) {
            this.setState({ gridStyle: prop.gridStyle });
        }
        if (this.props.externalFilters) {
            for (let i in this.props.externalFilters) {
                this.state.advancedSearchConditions.push(this.props.externalFilters[i])
            }

        }
    }

    updateExternalSearch(param: Condition[]) {
        if (param) {
            for (let i in param) {
                let f = this.state.advancedSearchConditions.findIndex((v: any, i: number) => { return v.Additional && v.Additional === true; })
                if (f > -1) {
                    this.state.advancedSearchConditions[f] = param[i];
                }
                else {
                    this.state.advancedSearchConditions.push(param[i])
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

    isHiddenField(name: string) {
        if (this.props.hideColumns) {
            for (let i = 0; i < this.props.hideColumns.length; i++) {
                if (this.props.hideColumns[i] == name)
                    return true;
            }
        }
        return false;
    }

    registerDialog(e: any) {
        KendoGrid.instance.dialog = e;
    }

    openDialog(param: any) {
        KendoGrid.instance.dialog.props = {
            ...KendoGrid.instance.dialog.props,
            dialogTitle: param.dialogTitle,
            buttons: param.buttons,
            getUrl: param.getUrl,
            getMethod: param.getMethod || 'get',
            postData: param.postData || [],
            method: param.method,
            postUrl: param.postUrl,
        }
        this.setState({ dialogProps: KendoGrid.instance.dialog.props }, KendoGrid.instance.dialog.openForm);
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


    getGridDataToPost() {
        if (this.state.gridHeaderChecked) {
            let excludeItems: any[] = [];
            let includeItems: any[] = [];
            for (let i in this.state.persistentSelection) {
                if (this.state.persistentSelection[i].selected === false) {
                    excludeItems.push(this.state.persistentSelection[i])
                }
            }
            return {
                selectedAll: true,
                excludeList: excludeItems,
                includeList: includeItems
            }
        }
        else {
            let excludeItems: any[] = [];
            let includeItems: any[] = [];
            for (let i in this.state.persistentSelection) {
                if (this.state.persistentSelection[i].selected === true) {
                    includeItems.push(this.state.persistentSelection[i])
                }
            }
            return {
                selectedAll: false,
                excludeList: excludeItems,
                includeList: includeItems
            }
        }
    }

    closeDialog() {

    }

    onDialogCancel(e: any) {
        this.clearSelection();
        KendoGrid.instance.closeDialog();
    }

    clearSelection() {
        let data: any[] = [];
        this.state.data.data.forEach((item: any) => {
            item.selected = false;
            data.push(item)
        })
        this.setState({
            persistentSelection: [],
            gridHeaderChecked: false,
            data: {
                ...this.state.data,
                data: data
            }
        }, () => {
            this.storeSearchParam();
            this.forceUpdate();
        })
        var inputs = document.getElementsByTagName("input");
        if (inputs.length) {
            for (let i = 0; i < inputs.length; i++) {
                if (inputs[i].getAttribute("type") === "checkbox") {
                    inputs[i].checked = false;
                }
            }
        }

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

    selectionChange(event: any) {
        event.dataItem.selected = !event.dataItem.selected;
        this.managePersitentSelection(event.dataItem);
    }

    _selectionChange(data: any) {
        this.managePersitentSelection(data);
    }

    headerSelectionChange(event: any) {
        this.setState({ persistentSelection: [] })
        const checked = event.syntheticEvent.target.checked;
        let gridData = this.state.data.data;
        gridData.forEach((item: any) => {
            item.selected = checked
        });
        this.setState({
            gridHeaderChecked: event.syntheticEvent.target.checked,
            data: {
                ...this.state.data,
                data: gridData
            }
        });
    }

    componentWillMount() {
        this.isComponentMounted = false;
    }

    async componentDidMount() {
        let allowAddButton = false;
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

            let authRequest: any[] = [];
            let arrMenu: any[] = [];

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


            Remote.get(this.props.fieldUrl, (result: any) => {
                let data: any[] = [];
                result = result.sort((a: any, b: any) => parseFloat(a.orderIndex) - parseFloat(b.orderIndex));
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
                    row.show = !this.isHiddenField(row.fieldName) && row.visibleToGrid === true;
                    data.push(row)
                }
                let gridFields = data.filter(c => c.show == true && (c.visibleToGrid == undefined || c.visibleToGrid == true));
                this.setState({ fields: data, allFields: result, gridFields: gridFields, rowMenus: rowMenus, allowAddButton: allowAddButton, allowGridMenu: allowGridMenu });
                this.setDefaultSortField(data);
            }, (error: any) => {
                    window.Dialog.alert(error)
                });
        }
    }


    setDefaultSortField(fields: any[]) {
        for (let fld in fields) {
            if (fields[fld].isDefaultSortField) {
                this.setState({ sortField: fields[fld].fieldName })
            }
        }
    }

    updateColumnVisibility(columns: any[]) {
        let arr: any[] = []
        columns = columns.sort((a: any, b: any) => {
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
        })

        for (let i in columns) {
            columns[i].visibleToGrid = columns[i].show;
            columns[i].orderIndex = parseInt(i) + 1;
            arr.push(columns[i])
        }

        this.setState({ gridFields: arr }, this.forceUpdate);
    }

    onSortChange(e: any) {
        e.nativeEvent.preventDefault();
        e.nativeEvent.stopPropagation();
        this.clearSelection();
        this.closeDialog()
        let evt: any = e;
        if (e.sort.length == 0) {
            evt.sort.push({
                field: this.state.sort[0].field,
                dir: 'asc'
            });
        }
        for (let s in evt.sort) {
            if (parseInt(s + "") > 0)
                break;
            let sort: any = evt.sort[s];
            this.setState({
                dataState: {
                    ...this.state.dataState,
                    sortField: sort.field,
                    dir: sort.dir
                },
                sort: evt.sort
            }, () => { this.storeSearchParam(); this.forceUpdate(); });
        }
    }

    onPageChange(e: any) {
        this.componentLoaded = false;
        this.setState({ dataState: { ...this.state.dataState, skip: e.page.skip, take: e.page.take, searchValue: this.state.dataState.searchValue, dir: this.state.dataState.dir, sortField: this.state.dataState.sortField } }, () => { this.storeSearchParam(); this.forceUpdate(); })
    }

    onPageSizeChange(e: any) {
        this.setState({
            dataState: {
                ...this.state.dataState,
                take: e.target.value
            }
        }, this.forceUpdate)
    }

    onSearchFieldKeyPress(e: any) {
        if (e.which == 13) {
            this.setState({
                dataState: {
                    ...this.state.dataState,
                    skip: 0,
                    searchValue: e.target.value
                }
            }, () => {
                this.storeSearchParam();
                this.forceUpdate();
            });
        }
    }

    onSearchButtonClicked(e: any) {
        e.nativeEvent.preventDefault()
        this.setState({
            dataState: {
                ...this.state.dataState,
                skip: 0,
                searchValue: this.searchText.value
            }
        }, () => {
            this.storeSearchParam();
            this.forceUpdate();
        });



    }

    storeSearchParam(isSwitchOn?: boolean) {
        if (this.props.printTitle && this.props.showSearchBox) {
            let data = {
                advancedSearchConditions: this.state.advancedSearchConditions,
                additionalFilterValue: this.state.additionalFilterValue,
                dataState: {
                    additionalFilter: this.state.dataState.additionalFilter,
                    dir: this.state.dataState.dir,
                    searchValue: this.state.dataState.searchValue,
                    skip: this.state.dataState.skip,
                    sortField: this.state.dataState.sortField,
                    take: this.state.dataState.take
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

    getCommandCell(e: any) {
        if (!this.props.rowMenus)
            return;
        let key = (new Date()).getTime();
        return (<td className="rowMenu"><CommandCell key={key} menuItems={this.state.rowMenus} dataItem={e.dataItem} parent={this} /></td>);
    }

    onAdvancedSearchApply(conditions: any[]) {
        this.setState({ advancedSearchConditions: conditions, dataState: { ...this.state.dataState, skip: 0, method: 'post' } }, () => { this.storeSearchParam(); this.forceUpdate(); });
    }

    onConditionChange(conditions: any[]) {

    }

    renderAdvancedSearch() {
        if (this.props.showAdvanceSearchBox != false)
            return (<AdvancedSearchDialog ref={(c) => { this.advancedSearchDialog = c; }} resourceIds={this.props.advancedSearchEntity} onApply={this.onAdvancedSearchApply} selectedConditions={this.state.advancedSearchConditions} onConditionChange={this.onConditionChange} />);
        else
            return (<span></span>)
    }

    renderHelpText(props: any, e: any, otherText: any) {
        if (e.helpText && e.helpText !== "") {
            return `<a href="#" title="${e.helpText}"><span class="k-icon k-i-info" /></a> ` + otherText;
        }
        return otherText;
    }

    renderHelpTextOnly(props: any, e: any) {
        if (e.helpText && e.helpText !== "") {
            return (<a href="#" title={e.helpText}><span className="k-icon k-i-info 111" /></a>);
        }
    }

    renderFields() {
        let fields: any[] = [];
        let deductionColumnWidth = 120;
        if (this.state.showSelection == false)
            deductionColumnWidth = 70;
        if (!this.props.rowMenus) {
            deductionColumnWidth -= 70;
        }
        let width: string = "150";
        let columns = this.state.gridFields;

        columns = columns.filter(c => (c.show == true && (c.visibleToGrid == undefined || c.visibleToGrid == true)) || c.fieldName == 'menu');
        columns = columns.sort((a: any, b: any) => {
            if (a.orderIndex < b.orderIndex)
                return -1;
            else
                return 1;
        })

        let menuField = columns.filter(c => (c.fieldName == 'menu'));
        if (menuField.length === 0 && this.props.rowMenus) {
            let arr: any[] = [];
            arr.push({
                fieldName: 'menu',
                fieldLabel: ' ',
                isSortable: false,
                isFilterable: false,
                orderIndex: 0,
                showHeaderText: false
            });
            for (let i = 0; i < columns.length; i++) {
                arr.push(columns[i])
            }
            columns = arr;
        }

        let totalColumns = deductionColumnWidth === 120 ? columns.length - 1 : columns.length;
        columns.forEach((e: any, index: number) => {
            let percent = e.columnWidth ? e.columnWidth : (100 / totalColumns)
            width = this.getPixel(percent, deductionColumnWidth) + "";
            if (e.columnMinimumWidth > width) {
                width = e.columnMinimumWidth;
            }

            if (index == 0 && this.props.rowMenus) {
                fields.push(<Column key={"gridfield" + index} field={e.fieldName} title={e.fieldLabel} sortable={e.isSortable} filterable={e.isFilterable} format={e.format} orderIndex={e.orderIndex} width={"50px"} cell={this.getCommandCell} className={e.gridColumnCss} headerClassName={e.gridColumnCss} />)
            }
            else {
                if (index == columns.length - 1 && e.show && this.props.showColumnMenu !== false) {
                    if (e.clickable && this.props.itemNavigationUrl && (!this.props.navigationFields || (this.props.navigationFields && this.props.navigationFields.indexOf(e.fieldName) >= 0))) {
                        fields.push(<Column key={"gridfield" + index} field={e.fieldName} title={e.fieldLabel} sortable={e.isSortable} format={e.format} filterable={e.isFilterable} orderIndex={e.orderIndex} width={e.visibleToGrid == true ? width + "px" : "0px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} columnMenu={
                            props =>
                                <GridColumnMenu
                                    gridColumns={this.state.gridFields}
                                    {...props}
                                    grid={this}
                                    columns={this.state.allFields}
                                />
                        } cell={
                            (props: any) => {
                                const value = props.dataItem[props.field];
                                const currency = props.dataItem['currency'] || '';
                                let css = "";

                                if (this.props.rowIconSettings && props.field == this.props.rowIconSettings.column) {
                                    this.props.rowIconSettings.icons.forEach((v: IconSetting) => {
                                        if (props.dataItem[v.field] === v.value) {
                                            css += " " + v.css
                                        }
                                    })
                                }
                                if (e.format === "{0:c}") {
                                    return (
                                        <td className={e.gridColumnCss + " " + css}>
                                            {this.renderHelpTextOnly(props, e)}
                                            <a href={this.props.itemNavigationUrl + props.dataItem[this.props.identityField]}>
                                                {(value === null) ? '' : currency + ' ' + numberWithCommas(props.dataItem[props.field]? props.dataItem[props.field].toFixed(2):'0')}
                                            </a>
                                        </td>
                                    );
                                }
                                else if (e.format === "{0:d}") {
                                    return (
                                        <td className={e.gridColumnCss + " " + css}>
                                            {this.renderHelpTextOnly(props, e)}
                                            <a href={this.props.itemNavigationUrl + props.dataItem[this.props.identityField]}>
                                                {(value === null) ? '' : formatUSDate(props.dataItem[props.field], "en")}
                                            </a>
                                        </td>
                                    );
                                }
                                else {
                                    return (<td className={e.gridColumnCss + " " + css}>
                                        {this.renderHelpTextOnly(props, e)}
                                        <a href={this.props.itemNavigationUrl + props.dataItem[this.props.identityField]}>
                                            {props.dataItem[props.field]}
                                        </a>
                                    </td>)
                                }
                            }
                        } />)
                    }
                    else {
                        if (e.format === "{0:c}") {
                            fields.push(<Column key={"gridfield" + index} field={e.fieldName} title={e.fieldLabel} sortable={e.isSortable} format={e.format} filterable={e.isFilterable} orderIndex={e.orderIndex} width={e.visibleToGrid == true ? width + "px" : "0px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} columnMenu={
                                props =>
                                    <GridColumnMenu
                                        gridColumns={this.state.gridFields}
                                        {...props}
                                        grid={this}
                                        columns={this.state.allFields}
                                    />
                            } cell={(prop) => {
                                const value = prop.dataItem[prop.field];
                                const currency = prop.dataItem['currency'] || '';
                                return (
                                    <td className={e.gridColumnCss} dangerouslySetInnerHTML={{ __html: (value === null) ? '' : this.renderHelpText(prop, e, currency + ' ' + numberWithCommas(prop.dataItem[prop.field]?prop.dataItem[prop.field].toFixed(2):'0')) }} />
                                );
                            }} />)
                        }
                        else {
                            fields.push(<Column key={"gridfield" + index} field={e.fieldName} title={e.fieldLabel} sortable={e.isSortable} format={e.format} filterable={e.isFilterable} orderIndex={e.orderIndex} width={e.visibleToGrid == true ? width + "px" : "0px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} columnMenu={
                                props =>
                                    <GridColumnMenu
                                        gridColumns={this.state.gridFields}
                                        {...props}
                                        grid={this}
                                        columns={this.state.allFields}
                                    />
                            } cell={(props) => { return (<td className={e.gridColumnCss} dangerouslySetInnerHTML={{ __html: this.renderHelpText(props, e, props.dataItem[props.field]) }} />) }} />)
                        }
                    }
                }
                else if (e.show) {

                    if (e.clickable && this.props.itemNavigationUrl && (!this.props.navigationFields || (this.props.navigationFields && this.props.navigationFields.indexOf(e.fieldName) >= 0))) {
                        fields.push(<Column key={"gridfield" + index} field={e.fieldName} title={e.fieldLabel} sortable={e.isSortable} format={e.format} filterable={e.isFilterable} orderIndex={e.orderIndex} width={e.visibleToGrid == true ? width + "px" : "0px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} cell={
                            (props: any) => {
                                let css = "";
                                const value = props.dataItem[props.field];
                                css = e.gridColumnCss
                                const currency = props.dataItem['currency'] || '';
                                if (this.props.rowIconSettings && props.field == this.props.rowIconSettings.column) {
                                    this.props.rowIconSettings.icons.forEach((v: IconSetting) => {
                                        if (props.dataItem[v.field] === v.value) {
                                            css += " " + v.css
                                        }
                                    })
                                }

                                if (e.format === "{0:d}") {
                                    return (<td className={css}>
                                        <a href={this.props.itemNavigationUrl + props.dataItem[this.props.identityField]}>
                                            {(value === null) ? '' : formatUSDate(value)}
                                        </a>
                                    </td>)
                                }
                                else if (e.format === "{0:c}") {
                                    return (<td className={css}>

                                        <a href={this.props.itemNavigationUrl + props.dataItem[this.props.identityField]}>
                                            {(value === null) ? '' : currency + ' ' + numberWithCommas(value.toFixed(2))}
                                        </a>
                                    </td>)
                                }
                                else {
                                    return (<td className={css}>
                                        <a href={this.props.itemNavigationUrl + props.dataItem[this.props.identityField]}>
                                            {value}
                                        </a>
                                    </td>)
                                }
                            }
                        } />)
                    }
                    else {
                        if (e.format === "{0:c}") {
                            fields.push(<Column key={"gridfield" + index} field={e.fieldName} title={e.fieldLabel} sortable={e.isSortable} format={e.format} filterable={e.isFilterable} orderIndex={e.orderIndex} width={e.visibleToGrid == true ? width + "px" : "0px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} cell={(prop) => {
                                const value = prop.dataItem[prop.field];
                                const currency = prop.dataItem['currency'] || '';
                                let css = "";
                                if (this.props.rowIconSettings && prop.field == this.props.rowIconSettings.column) {
                                    this.props.rowIconSettings.icons.forEach((v: IconSetting) => {
                                        if (prop.dataItem[v.field] === v.value) {
                                            css += " " + v.css
                                        }
                                    })
                                }
                                return (
                                    <td className={e.gridColumnCss + " " + css} dangerouslySetInnerHTML={{ __html: (value === null) ? '' : this.renderHelpText(prop, e, currency + ' ' + numberWithCommas(prop.dataItem[prop.field]?prop.dataItem[prop.field].toFixed(2):'0')) }} />
                                );
                            }} />)
                        }
                        else if (e.format === "{0:d}") {
                            fields.push(<Column key={"gridfield" + index} field={e.fieldName} title={e.fieldLabel} sortable={e.isSortable} format={e.format} filterable={e.isFilterable} orderIndex={e.orderIndex} width={e.visibleToGrid == true ? width + "px" : "0px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} cell={(prop) => {
                                const value = prop.dataItem[prop.field];
                                let css = "";
                                if (this.props.rowIconSettings && prop.field == this.props.rowIconSettings.column) {
                                    this.props.rowIconSettings.icons.forEach((v: IconSetting) => {
                                        if (prop.dataItem[v.field] === v.value) {
                                            css += " " + v.css
                                        }
                                    })
                                }
                                return (
                                    <td className={e.gridColumnCss + " " + css} dangerouslySetInnerHTML={{ __html: (value === null) ? '' : this.renderHelpText(prop, e, formatUSDate(prop.dataItem[prop.field], "en")) }} />
                                );
                            }} />)
                        }
                        else if (e.format === "{0:dt}") {
                            fields.push(<Column key={"gridfield" + index} field={e.fieldName} title={e.fieldLabel} sortable={e.isSortable} format={e.format} filterable={e.isFilterable} orderIndex={e.orderIndex} width={e.visibleToGrid == true ? width + "px" : "0px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} cell={(prop) => {
                                const value = prop.dataItem[prop.field];
                                let css = "";
                                if (this.props.rowIconSettings && prop.field == this.props.rowIconSettings.column) {
                                    this.props.rowIconSettings.icons.forEach((v: IconSetting) => {
                                        if (prop.dataItem[v.field] === v.value) {
                                            css += " " + v.css
                                        }
                                    })
                                }
                                return (
                                    <td className={e.gridColumnCss + " " + css} dangerouslySetInnerHTML={{ __html: (value === null) ? '' : this.renderHelpText(prop, e, formatUSDatetime(prop.dataItem[prop.field])) }} />
                                );
                            }} />)
                        }
                        else if (e.format === "{0:html}") {
                            fields.push(<Column key={"gridfield" + index} field={e.fieldName} title={e.fieldLabel} sortable={e.isSortable} format={e.format} filterable={e.isFilterable} orderIndex={e.orderIndex} width={e.visibleToGrid == true ? width + "px" : "0px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} cell={(prop) => {
                                const value = prop.dataItem[prop.field];
                                let css = "";
                                if (this.props.rowIconSettings && prop.field == this.props.rowIconSettings.column) {
                                    this.props.rowIconSettings.icons.forEach((v: IconSetting) => {
                                        if (prop.dataItem[v.field] === v.value) {
                                            css += " " + v.css
                                        }
                                    })
                                }
                                return (
                                    <td className={e.gridColumnCss + " " + css} dangerouslySetInnerHTML={{ __html: prop.dataItem[prop.field] }}></td>
                                );
                            }} />)
                        }
                        else if (e.format === "list") {
                            fields.push(<Column key={"gridfield" + index} field={e.fieldName} title={e.fieldLabel} sortable={e.isSortable} format={e.format} filterable={e.isFilterable} orderIndex={e.orderIndex} width={e.visibleToGrid == true ? width + "px" : "0px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} cell={(prop) => {
                                const value = prop.dataItem[prop.field];
                                let css = "";
                                if (this.props.rowIconSettings && prop.field == this.props.rowIconSettings.column) {
                                    this.props.rowIconSettings.icons.forEach((v: IconSetting) => {
                                        if (prop.dataItem[v.field] === v.value) {
                                            css += " " + v.css
                                        }
                                    })
                                }
                                return (
                                    <td className={e.gridColumnCss + " " + css} dangerouslySetInnerHTML={{ __html: (value === null) ? '' : this.renderHelpText(prop, e, this.printFieldValueAsList(value, prop.dataIndex - 1)) }} />
                                );
                            }} />)
                        }
                        else if (index == 1) {
                            fields.push(<Column key={"gridfield" + index} field={e.fieldName} title={e.fieldLabel} sortable={e.isSortable} format={e.format} filterable={e.isFilterable} orderIndex={e.orderIndex} width={e.visibleToGrid == true ? width + "px" : "0px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} cell={(prop: any) => {
                                let css = "";
                                if (this.props.rowIconSettings && prop.field == this.props.rowIconSettings.column) {
                                    this.props.rowIconSettings.icons.forEach((v: IconSetting) => {
                                        if (prop.dataItem[v.field] === v.value) {
                                            css += " " + v.css
                                        }
                                    })
                                }
                                return (<td className={css} dangerouslySetInnerHTML={{ __html: this.renderHelpText(prop, e, prop.dataItem[prop.field]) }} />);
                            }} />)
                        }
                        else {
                            fields.push(<Column key={"gridfield" + index} field={e.fieldName} title={e.fieldLabel} sortable={e.isSortable} format={e.format} filterable={e.isFilterable} orderIndex={e.orderIndex} width={e.visibleToGrid == true ? width + "px" : "0px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} cell={(props: any) => {
                                let css = e.gridColumnCss;
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
                                return (<td className={css} dangerouslySetInnerHTML={{ __html: this.renderHelpText(props, e, value) }} />);
                            }} />)

                        }
                    }

                }

            }

        })
        return fields;
    }

    printFieldValueAsList(list: any[], dataIndex: any) {
        let dIndex: string = dataIndex;
        let items: any[] = [];
        for (let i in list) {
            if (parseInt(i) < 3) {
                items.push(<span key={i} className="badge badge-pill badge-secondary mr-1">{list[i]}</span>)
            }
        }
        if (list.length > 3) {
            items.push(<span key={4} className="btn btn-sm btn-secondary more"><a href="#" itemID={dIndex} onClick={this.onMoreLinkClicked}>More</a></span>)
        }
        return items;
    }

    printPDF(e: any, columns: any) {
        KendoGrid.instance.closeDialog();
        this.setState({ isLoading: true }, () => {
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
                    title: unescape(columns.fieldLabels[fieldIndex])
                });
            }

            let sender = this;

            this.getSelectedItems((result: any) => {

                if (this.state.gridHeaderChecked) {
                    this.loadAddDataForSelectAll((result: any) => {
                        let data = result.result || result;
                        let deSelected: any[] = this.state.persistentSelection.filter((v: any) => { return v.selected === false });
                        let nData: any[] = [];
                        for (let i = 0; i < data.length; i++) {
                            let index = deSelected.findIndex((v: any) => {
                                return v[this.props.identityField] == data[i][this.props.identityField];
                            })
                            if (index < 0) {
                                nData.push(data[i]);
                            }
                        }
                        sender.downloadPDF(nData, gridColumns);
                        sender.clearSelection();
                    })
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
        })
    }

    loadAddDataForSelectAll(callback: any) {
        let dState = JSON.parse(JSON.stringify(this.state.dataState));
        dState.take = this.state.data.total;
        dState.skip = 0;
        let queryString = this.makeQueryString(dState);
        let caller = this;
        let separator = "?";
        if (this.props.dataURL.indexOf("?") >= 0) {
            separator = "&"
        }
        let url = this.props.dataURL + separator + queryString + "&t=" + (new Date()).getTime();
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

    downloadPDF(data: any, gridColumns: any) {
        let printedBy = this.props.printedBy || ''
        let pdfTitle = this.props.printTitle || ''
        let landscape = (gridColumns.length > 6);
        let sender = this;
        if (gridColumns.length > 12 || gridColumns.length < 1) {
            window.Dialog.alert("Fields count must be within the range of 1 to 12.")
            this.setState({ isLoading: false }, this.forceUpdate);
            return;
        }

        if (data.length > 500) {
            window.Dialog.alert("There are more than 500 records. Export to PDF only supports upto 500 records. Please add additional filters and try again. You can also try exporting to Excel.")
            this.setState({ isLoading: false }, this.forceUpdate);
            return;
        }
        let today = getTodayDate();
        var grid = $("<div id='pdfGrid'></div>");
        grid.kendoGrid({
            toolbar: ["pdf"],
            pdf: {
                allPages: true,
                avoidLinks: true,
                paperSize: "Letter",
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
                        sender.setState({ isLoading: false });
                    });
            }
        });
        grid.data("kendoGrid").saveAsPDF();
    }

    exportToPDF() {
        this.exportPDFClick(this.props.exportFieldUrl);
    }

    exportToExcel() {
        this.exportExcelClick(this.props.exportFieldUrl);
    }

    exportPDFClick(fieldUrl: string) {
        let selected1 = this.state.persistentSelection.filter((v: any) => {
            return v.selected == true
        })

        if (selected1.length == 0 && this.state.gridHeaderChecked == false) {
            window.Dialog.alert("Please select at least a row to export to PDF.")
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

        this.openDialog({
            dialogTitle: 'Export to PDF',
            getUrl: fieldUrl,
            buttons: buttons,
            method: 'get'
        })
    }

    printExcel(e: any, columns: any) {
        KendoGrid.instance.closeDialog();
        this.setState({ isLoading: true }, () => {
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
                    title: unescape(columns.fieldLabels[fieldIndex])
                });
            }

            let sender = this;

            this.getSelectedItems((result: any) => {
                if (this.state.gridHeaderChecked) {
                    this.loadAddDataForSelectAll((result: any) => {
                        let data = result.result;
                        let deSelected: any[] = this.state.persistentSelection.filter((v: any) => { return v.selected === false });
                        let nData: any[] = [];
                        for (let i = 0; i < data.length; i++) {
                            let index = deSelected.findIndex((v: any) => {
                                return v[this.props.identityField] == data[i][this.props.identityField];
                            })
                            if (index < 0) {
                                nData.push(data[i]);
                            }
                        }
                        sender.downloadExcel(nData, gridColumns);
                        sender.clearSelection();
                    })
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
            })
        })
    }

    downloadExcel(data: any, gridColumns: any) {
        let sender = this;
        let today = getTodayDate();
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
                template: `<div class="page-template"><div class="header"><img src="/img/logo.jpg" width="175" height="40" /> <span class="print-title">${this.props.printTitle}</span> <div style="float: right" class="printed-by-field">Printed By ${this.props.printedBy} on ${today}</div> </div><div class="watermark"></div><div class="footer">Page #: pageNum # of #: totalPages #</div></div>`,
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
            excelExport: function (e: any) {
                sender.setState({ isLoading: false });
            }
        });
        grid.data("kendoGrid").saveAsExcel();
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

        this.openDialog({
            dialogTitle: 'Export to Excel',
            getUrl: fieldUrl,
            buttons: buttons,
            method: 'get'
        })
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
                return this.state.persistentSelection[i].selected;
            }
            else if (this.state.gridHeaderChecked) {
                return true;
            }
        }
        return false;
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

    dataRecieved(e: any, loader: any) {
        let gridMessage: string = "";
        if (this.props.onGridLoaded) {
            this.props.onGridLoaded(e.data);
        }

        if (e.message) {
            gridMessage = e.message;
        }
        let data: any[] = [];
        if (this.props.selectionField && this.componentLoaded === false) {
            let selectedData: any[] = this.state.persistentSelection || [];
            for (let i in e.data) {
                if ((e.data[i][this.props.selectionField] === true || this.state.gridHeaderChecked) && this.findSelectedIndex(e.data[i]) == -1) {
                    selectedData.push({
                        ...e.data[i],
                        selected: true
                    });
                }
            }
            this.setState({
                persistentSelection: selectedData
            },
                () => {
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
                        data.push({
                            ...e.data[i],
                            selected: isSelected
                        })
                    }
                    if (this.state.currentColumnMenu) {
                        this.setState({ data: { total: e.total, data: data }, gridMessage: gridMessage }, () => { this.state.currentColumnMenu.refresh(); this.forceUpdate() });
                    } else {
                        this.setState({ data: { total: e.total, data: data }, gridMessage: gridMessage }, this.forceUpdate);
                    }
                    this.dataLoader = loader;
                });
            this.componentLoaded = true;
        } else {
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
                data.push({
                    ...e.data[i],
                    selected: isSelected
                })
            }
            if (this.state.currentColumnMenu) {
                this.setState({ data: { total: e.total, data: data }, gridMessage: gridMessage }, () => { this.state.currentColumnMenu.refresh(); this.forceUpdate() });
            } else {
                if (this.isComponentMounted) {
                    this.setState({ data: { total: e.total, data: data }, gridMessage: gridMessage }, this.forceUpdate);
                }
            }
            this.dataLoader = loader;
        }
    }

    dataStateChange(e: any) {
        this.setState({
            ...this.state,
            currentColumnMenu: null,
            dataState: e.data
        }, this.forceUpdate);
    }

    onGridMenuClick(e: any) {
        e.nativeEvent.preventDefault()
        let index = e.itemIndex;
        this.props.gridMenu[index].action(e, this);
    }

    renderGridMenu() {
        let menus: any[] = [];
        if (!this.props.gridMenu)
            return menus;
        for (let i in this.props.gridMenu) {
            menus.push(<a key="gridmenu{i}" className="dropdown-item" href="#" onClick={this.onGridMenuClick}>{this.props.gridMenu[i].text}</a>)
        }
        return menus;
    }

    addRecordClicked(e: any) {
        if (this.props.addRecord.redirect && this.props.addRecord.redirect == true) {
            KendoGrid.instance.setState({ redirect: true }, KendoGrid.instance.forceUpdate)
        }
        else if (!this.props.addRecord.getUrl || this.props.addRecord.getUrl === "") {
            this.props.addRecord.action(e, KendoGrid.instance);
        }
        else {
            let buttons: any[] = [];
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
            })
        }
    }

    onAddActionComplete(e: any) {

    }

    renderRedirect() {
        if (this.props.addRecord && this.props.addRecord.redirect && this.props.addRecord.redirect == true && this.state.redirect) {
            window.location = this.props.addRecord.url
        }
    }

    renderAddRecordButton() {
        if (this.props.addRecord && this.props.addRecord.text != '' && this.state.allowAddButton === true) {
            return (<Button primary={true} onClick={this.addRecordClicked}>{this.props.addRecord.text}</Button>) //icon={this.props.addRecord.icon}
        }
        return null;
    }

    openSubmittableForm(param: any, dialogProps?: any) {
        if (dialogProps) {
            this.setState({ dialogProps: { ...this.state.dialogProps, ...dialogProps } }, () => { this._showSubmittableForm(param) });
        }
        else
            this._showSubmittableForm(param)
    }

    reloadData() {
        this.dataLoader.reloadData();
    }

    refresh(culumnMenu: any = null) {
        if (culumnMenu != null) {
            this.clearSelection();
            this.setState({ currentColumnMenu: culumnMenu, persistentSelection: [], gridHeaderChecked: false }, this.dataLoader.reloadData);
        }
        else {
            this.clearSelection();
            this.dataLoader.reloadData();
        }

    }

    onSubmittableFormSuccess(param: any, obj: any) {
        this.dataLoader.reloadData();
        if (param.button) {
            this.state.submittableFormParam.buttons[parseInt(param.button.buttonIndex)].action(param, obj);
        }
        this.forceUpdate();
    }

    _showSubmittableForm(param: any) {
        let buttons: any[] = [];
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
        })
    }

    onSearchFillsDelete(condition: Condition) {

        if (condition.Operator.OperatorId === null) {
            this.setState({
                dataState: {
                    ...this.state.dataState,
                    searchValue: ''
                },
                searchText: ''
            }, () => { this.storeSearchParam(); this.forceUpdate(); })
        }
        else {
            let conditions: any[] = [];
            for (let i in this.state.advancedSearchConditions) {
                if (JSON.stringify(this.state.advancedSearchConditions[i]) != JSON.stringify(condition)) {
                    conditions.push(this.state.advancedSearchConditions[i])
                }
            }
            this.advancedSearchDialog.updateSearchConditions(conditions);
            this.setState({ advancedSearchConditions: conditions }, () => { this.storeSearchParam(); this.forceUpdate(); });
        }
    }

    onAdditionalFilterChange(e: any) {
        let value = this.props.additionalFilters[e.itemIndex];
        let sortField = value.sortField ? value.sortField : this.state.dataState.sortField;
        let sortOrder = value.sortOrder ? value.sortOrder : this.state.dataState.dir;
        this.setState({ additionalFilterValue: value, dataState: { ...this.state.dataState, additionalFilter: value.value, sortField: sortField, dir: sortOrder, skip: 0 } }, () => {
            this.storeSearchParam();
            this.forceUpdate();
        });
    }

    onMoreLinkClicked(e: any) {
        e.nativeEvent.preventDefault()
        if (this.props.onMoreLinkClicked) {

            let itemId = e.target.getAttribute("itemid");
            let data = this.state.data.data[itemId];
            this.props.onMoreLinkClicked(data);
        }
    }

    renderSearchBox() {

        return (
            <div className="search-form-r">
                {this.renderAdditionalFilters()}
                <div className="input-group mb-3 mb-sm-0">
                    <Input autoFocus={true} onKeyPress={this.onSearchFieldKeyPress} placeholder={this.props.searchPlaceHolder || "Search"} ref={(c) => { this.searchText = c }} onChange={c => { this.setState({ searchText: c.target.value }) }} value={this.state.searchText} className="form-control k-textbox" />
                    <div className="input-group-append">
                        <div className="input-group-text">
                            <a href={"javascript:void(0)"} onClick={this.onSearchButtonClicked} className="k-icon k-i-search">&nbsp;</a>
                        </div>
                    </div>
                </div>
                {this.renderAdvancedSearch()}
                <div className="clearfix"></div>
            </div>
        )
    }

    onSwitchChange(e: any) {
        this.storeSearchParam(e.target.value);
        this.setState({ isSwitchOn: e.target.value }, () => {
            if (this.props.switchButton) {
                this.props.switchButton.action(e.target.value);
            }
        })
    }

    renderSwitchButton() {
        if (this.props.switchButton != undefined) {
            return (<div className="switch-btn"><Switch offLabel={this.props.switchButton.offLabel} onLabel={this.props.switchButton.onLabel} className="switchButton" checked={this.state.isSwitchOn} onChange={this.onSwitchChange} />{this.state.isSwitchOn ? this.props.switchButton.onLabel : this.props.switchButton.offLabel}</div>);
        }
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

    onClearAllPills(e: any) {
        if (this.advancedSearchDialog) {
            this.advancedSearchDialog.updateSearchConditions([])
        }

        this.setState({
            dataState: {
                ...this.state.dataState,
                searchValue: ''
            },
            searchText: '',
            advancedSearchConditions: []
        }, () => {
            this.storeSearchParam();
            this.forceUpdate();
        })
    }

    renderSearchBoxAndActions() {
        let searchConditions: Condition[] = [];
        if (this.state.dataState.searchValue) {
            let cond = new Condition();
            cond.Attribute = new Attribute(null, null, "Any field", 0, false, "");
            cond.Operator = new Operator(null, 0, "Contains", 0);
            cond.ConditionId = null;
            cond.Value = this.state.dataState.searchValue;
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
        else if (this.props.showSearchBox == false && this.props.showGridAction == true) {
            return (<div className={"row mb-3"}>
                <div className="col">

                </div>
                <div className="col-md-12 col-lg-auto">
                    {this.renderMoreButtons()}
                </div>
            </div>)
        }
        else if (this.props.additionalFilters && this.props.additionalFilters.length > 0) {
            return (<div className={"row mb-3"}>
                <div className="col mb-md-3 mb-lg-0">
                    {this.renderAdditionalFilters()}
                </div>
                <div className="col-md-12 col-lg-auto">

                </div>
            </div>)
        }
    }

    renderAdditionalFilters() {
        if (this.props.additionalFilters && this.props.additionalFilters.length > 0) {
            return (<div className="float-left mr-3 mb-3 mb-sm-0 filterButtonContainer">
                <SplitButton buttonClass="filterButton" popupSettings={{ popupClass: "filterButton" }} text={this.state.additionalFilterValue.name} key="value" textField="name" className="actions-dropdown" items={this.props.additionalFilters} onItemClick={this.onAdditionalFilterChange} />
            </div>)
        }
        else {
            return (<></>)
        }
    }

    renderSelectionField() {
        if (this.state.showSelection == true) {
            return (<Column field="selected" width="50px" />)
        }
    }

    loadGridColumns(columnMenu: any) {
        let sender = this;
        Remote.get(this.props.fieldUrl, (result: any) => {
            let data: any[] = [];
            result = result.sort((a: any, b: any) => parseFloat(a.orderIndex) - parseFloat(b.orderIndex));
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
                row.show = !sender.isHiddenField(row.fieldName) && row.visibleToGrid == true;
                data.push(row)
            }
            sender.setState({ fields: data })
            sender.dataLoader.reloadData();
        }, (err: any) => {
                window.Dialog.alert(err)
            })
    }

    onRowClick(e: any) {
        if (this.props.onRowClick) {
            this.props.onRowClick(e);
        }
    }

    onColumnReorder(e: any) {
        let that = this;
        console.log(this.gridElement.columns)
        setTimeout(() => {
            console.log(that.gridElement.columns)
        }, 10)
    }

    getPixel(percent: number, adjust: number) {
        let wd = $("#" + this.props.parent).width();
        wd = wd - adjust;
        if (!isNaN(wd)) {
            wd = (wd / 100) * percent
            return wd;
        }
        else {
            return percent;
        }
    }

    renderLoading() {
        return (
            <div className="k-loading-mask">
                <span className="k-loading-text">Loading</span>
                <div className="k-loading-image"></div>
                <div className="k-loading-color"></div>
            </div>
        );
    }

    render() {
        let gridStyle = { ...this.state.gridStyle, width: '100%' };
        return (
            <div className="kendoGrid">
                {this.renderSearchBoxAndActions()}
                {this.props.description && <div className="grid-description" dangerouslySetInnerHTML={{ __html: unescape(this.props.description) }}></div>}
                <Grid ref={(c) => { this.gridElement = c }} groupable={false} style={gridStyle} selectedField={this.state.showSelection ? "selected" : ""} {...this.state.data} {...this.state.dataState} sortable={true} onSortChange={this.onSortChange} sort={this.state.sort} resizable={true}
                    onSelectionChange={this.selectionChange} onHeaderSelectionChange={this.headerSelectionChange} reorderable={true} onColumnReorder={this.onColumnReorder} onPageChange={this.onPageChange} pageable={this.state.pageable} onDataStateChange={this.dataStateChange} onRowClick={this.onRowClick}>
                    {this.renderSelectionField()}
                    {this.renderFields()}
                    <GridNoRecords>
                        {this.state.gridMessage}
                    </GridNoRecords>
                </Grid>
                <GridDataLoader
                    ref={(c) => { this.dataLoader = c }}
                    baseURL={this.props.dataURL}
                    method={this.state.dataState.method || 'get'}
                    switchStatus={this.state.isSwitchOn}
                    dataState={this.state.dataState}
                    onDataRecieved={this.dataRecieved}
                    postValue={this.state.advancedSearchConditions}
                />
                {this.renderRedirect()}
                {this.state.isLoading && this.renderLoading()}
                <KendoDialog {...this.state.dialogProps}></KendoDialog>
            </div>
        );
    }
}