import 'core-js';
import * as React from "react"
import * as ReactDOM from "react-dom"
import { Grid, GridColumn as Column } from '@progress/kendo-react-grid';
import { Input, Switch } from "@progress/kendo-react-inputs";
import { CommandCell1 as CommandCell } from "./CommandCell1";
import { GridDataLoader1 as GridDataLoader } from "./GridDataLoader1"
import { KendoDialog } from "../Dialog/Dialog"
import { SplitButton, Button } from "@progress/kendo-react-buttons";
import { Redirect, BrowserRouter } from 'react-router-dom';
import { GridColumnMenu1 as GridColumnMenu } from "./GridColumnMenu1";
import { Remote } from "../Remote/Remote";

declare var window: any;

declare var $: any;
declare var Dialog: any;

interface IGridProps {
    dataURL: string;
    fieldUrl: string;
    exportFieldUrl?: string;
    rowMenus?: any;
    gridMenu?: any[];
    addRecord?: any;
    gridWidth: number;
    itemNavigationUrl?: string;
    identityField: string;
    showSearchBox?: boolean;
    showAdvanceSearchBox?: boolean;
    showGridAction?: boolean;
    showSelection?: boolean;
    parent?: any;
    gridHeight?: string;
    hideColumns?: any[];
    switchButton?: any;
    selectionField?: string;
    addNewDistribution?: any;
    showAddNewButton?: boolean;
    showColumnMenu?: boolean;
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
}

export class KendoGrid1 extends React.Component<IGridProps, IGridState> {
    static instance: KendoGrid1 = null;
    gridElement: any;
    dataLoader: any;
    dialog: any;
    searchText: any;
    columnFilterCell: any;
    componentLoaded: boolean = false;

    constructor(props: IGridProps) {
        super(props);
        this.gridElement = React.createRef();
        KendoGrid1.instance = this;
        let buttons: any[] = [];

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
            dataState: { take: 10, skip: 0, searchValue: '', dir: 'asc', sortField: '' },
            data: { data: [], total: 0 },
            dialogProps: dialogProps,
            redirect: false,
            submittableFormParam: {},
            persistentSelection: [],
            gridStyle: { height: this.props.gridHeight || '500px' },
            sort: [],
            currentColumnMenu: null,
            isSwitchOn: this.props.switchButton ? this.props.switchButton.checked : false,
            gridHeaderChecked: false
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

        let prop: any = this.props;
        if (prop.gridStyle) {
            this.setState({ gridStyle: prop.gridStyle });
        }
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
        KendoGrid1.instance.dialog = e;
    }

    openDialog(param: any) {
        KendoGrid1.instance.dialog.props = {
            ...KendoGrid1.instance.dialog.props,
            dialogTitle: param.dialogTitle,
            buttons: param.buttons,
            getUrl: param.getUrl,
            getMethod: param.getMethod || 'get',
            postData: param.postData || [],
            method: param.method,
            postUrl: param.postUrl,
        }
        this.setState({ dialogProps: KendoGrid1.instance.dialog.props }, KendoGrid1.instance.dialog.openForm);
    }

    getSelectedItems() {
        let items: any[] = [];
        let rows = this.state.persistentSelection;

        for (var i = 0; i < rows.length; i++) {
            if (rows[i].selected == true) {
                items.push(rows[i]);
            }
        }
        return items;
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
        KendoGrid1.instance.closeDialog();
    }

    clearSelection() {
        this.setState({ persistentSelection: [] });
        let data: any[] = [];
        this.state.data.data.forEach((item: any) => {
            item.selected = false;
            data.push(item)
        })
        this.setState({
            data: {
                ...this.state.data,
                data: data
            }
        }, this.forceUpdate)
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
        this.dataLoader.reloadData();
        this.forceUpdate();
    }

    _selectionChange(data: any) {
        this.managePersitentSelection(data);
        this.forceUpdate();
    }

    headerSelectionChange(event: any) {
        const checked = event.syntheticEvent.target.checked;
        let gridData = this.state.data.data;
        this.dataLoader.reloadData();
        gridData.forEach((item: any) => {
            item.selected = checked
            this._selectionChange(item);
        });
        this.setState({
            gridHeaderChecked: event.syntheticEvent.target.checked,
            data: {
                ...this.state.data,
                data: gridData
            }
        });
        this.forceUpdate();
    }

    componentDidMount() {
        if (this.props.parent != undefined) {
            let width = 0;
            while (width == 0) {
                width = $(this.props.parent).closest(".container").width();
            }
            this.setState({ parentWidth: width }, this.forceUpdate);
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
            this.setState({ fields: data, allFields: result, gridFields: gridFields });
            this.setDefaultSortField(data);
        }, (error: any) => {
            window.Dialog.alert(error, "Error");
        });
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
            }, this.forceUpdate);
        }
    }

    onPageChange(e: any) {
        this.componentLoaded = false;
        this.setState({ dataState: { skip: e.page.skip, take: e.page.take, searchValue: this.state.dataState.searchValue, dir: this.state.dataState.dir, sortField: this.state.dataState.sortField } }, this.forceUpdate)
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
                    searchValue: e.target.value
                }
            }, this.forceUpdate);
        }
    }

    onSearchButtonClicked(e: any) {
        this.setState({
            dataState: {
                ...this.state.dataState,
                searchValue: this.searchText.value
            }
        }, this.forceUpdate);
    }

    getCommandCell(e: any) {
        if (!this.props.rowMenus)
            return;
        return (<td><CommandCell menuItems={this.props.rowMenus} dataItem={e.dataItem} parent={this} /></td>);
    }

    renderFields() {
        let fields: any[] = [];
        let deductionColumnWidth = 120;
        if (this.state.showSelection == false)
            deductionColumnWidth = 70;
        if (!this.props.rowMenus) {
            deductionColumnWidth -= 70;
        }
        let width = 150;
        let columns = this.state.gridFields;
        if (this.props.gridWidth > 0) {
            width = (this.props.gridWidth - deductionColumnWidth) / (columns.length)
        }
        if ($("#" + this.props.parent).width && $("#" + this.props.parent).width() > 0) {
            width = ($("#" + this.props.parent).width() - deductionColumnWidth) / (columns.length)
        }
        if (width < 150)
            width = 150;
        columns = this.state.fields.filter(c => (c.show == true && (c.visibleToGrid == undefined || c.visibleToGrid == true)) || c.fieldName == 'menu');
        columns = columns.sort((a: any, b: any) => {
            if (a.orderIndex < b.orderIndex)
                return -1;
            else
                return 1;
        })

        columns.forEach((e: any, index: number) => {
            if (index == 0 && this.props.rowMenus) {
                fields.push(<Column key={"gridfield" + index} field={e.fieldName} title={e.fieldLabel} sortable={e.isSortable} filterable={e.isFilterable} format={e.format} orderIndex={e.orderIndex} width={"50px"} cell={this.getCommandCell} className={e.gridColumnCss} headerClassName={e.gridColumnCss} />)
            }
            else {
                if (index == columns.length - 1 && e.show && this.props.showColumnMenu !== false) {
                    if (e.clickable && this.props.itemNavigationUrl) {
                        fields.push(<Column key={"gridfield" + index} field={e.fieldName} title={e.fieldLabel} sortable={e.isSortable} format={e.format} filterable={e.isFilterable} orderIndex={e.orderIndex} width={e.visibleToGrid == true ? width + "px" : "0px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss}
                            columnMenu={
                                (props: any) => {
                                    return <GridColumnMenu
                                        gridColumns={this.state.gridFields}
                                        {...props}
                                        grid={this}
                                        columns={this.state.allFields} />
                                }
                            }
                            cell={
                                (props: any) => {
                                    return (<td>
                                        <a href={this.props.itemNavigationUrl + eval("props.dataItem." + this.props.identityField)}>
                                            {eval("props.dataItem." + props.field)}
                                        </a>
                                    </td>)
                                }
                            }
                        />)
                    }
                    else {
                        if (e.format === "{0:c}") {
                            fields.push(<Column key={"gridfield" + index} field={e.fieldName} title={e.fieldLabel} sortable={e.isSortable} format={e.format} filterable={e.isFilterable} orderIndex={e.orderIndex} width={e.visibleToGrid == true ? width + "px" : "0px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss}
                                columnMenu={
                                    (props: any) => {
                                        return <GridColumnMenu
                                            gridColumns={this.state.gridFields}
                                            {...props}
                                            grid={this}
                                            columns={this.state.allFields} />
                                    }
                                }
                                cell={(prop: any) => {
                                    const value = prop.dataItem[prop.field];
                                    const currency = prop.dataItem['currency'] || '';
                                    return (
                                        <td className={e.gridColumnCss}> {
                                            (value === null) ? '' : currency + ' ' + prop.dataItem[prop.field].toFixed(2)}
                                        </td>
                                    );
                                }}
                            />)
                        }
                        else {
                            fields.push(<Column key={"gridfield" + index} field={e.fieldName} title={e.fieldLabel} sortable={e.isSortable} format={e.format} filterable={e.isFilterable} orderIndex={e.orderIndex} width={e.visibleToGrid == true ? width + "px" : "0px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss}
                                columnMenu={
                                    (props: any) => {
                                        return <GridColumnMenu
                                            gridColumns={this.state.gridFields}
                                            {...props}
                                            grid={this}
                                            columns={this.state.allFields} />
                                    }
                                }
                            />)
                        }
                    }
                }
                else if (e.show) {

                    if (e.clickable && this.props.itemNavigationUrl) {
                        fields.push(<Column key={"gridfield" + index} field={e.fieldName} title={e.fieldLabel} sortable={e.isSortable} format={e.format} filterable={e.isFilterable} orderIndex={e.orderIndex} width={e.visibleToGrid == true ? width + "px" : "0px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} cell={
                            (props: any) => {
                                return (<td>
                                    <a href={this.props.itemNavigationUrl + eval("props.dataItem." + this.props.identityField)}>
                                        {eval("props.dataItem." + props.field)}
                                    </a>
                                </td>)
                            }
                        } />)
                    }
                    else {
                        if (e.format === "{0:c}") {
                            fields.push(<Column key={"gridfield" + index} field={e.fieldName} title={e.fieldLabel} sortable={e.isSortable} format={e.format} filterable={e.isFilterable} orderIndex={e.orderIndex} width={e.visibleToGrid == true ? width + "px" : "0px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} cell={(prop) => {
                                const value = prop.dataItem[prop.field];
                                const currency = prop.dataItem['currency'] || '';
                                return (
                                    <td className={e.gridColumnCss}> {
                                        (value === null) ? '' : currency + ' ' + prop.dataItem[prop.field].toFixed(2)}
                                    </td>
                                );
                            }} />)
                        }
                        else {
                            fields.push(<Column key={"gridfield" + index} field={e.fieldName} title={e.fieldLabel} sortable={e.isSortable} format={e.format} filterable={e.isFilterable} orderIndex={e.orderIndex} width={e.visibleToGrid == true ? width + "px" : "0px"} className={e.gridColumnCss} headerClassName={e.gridColumnCss} />)
                        }
                    }

                }

            }

        })
        return fields;
    }

    printPDF(e: any, columns: any) {
        KendoGrid1.instance.closeDialog();
        var gridColumns: any[] = [];
        var data: any[] = [];
        for (var i = 0; i < columns.selectedOptions.length; i++) {
            let fieldIndex = columns.fieldNames.indexOf(columns.selectedOptions[i])
            gridColumns.push({
                field: columns.selectedOptions[i],
                title: unescape(columns.fieldLabels[fieldIndex])
            });
        }

        let rows = this.getSelectedItems();

        for (var i = 0; i < rows.length; i++) {
            if (rows[i].selected == true) {
                data.push(rows[i]);
            }
        }

        var grid = $("<div id='pdfGrid'></div>");
        grid.KendoGrid1({
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
        grid.data("KendoGrid1").saveAsPDF();
        this.clearSelection();
    }

    exportToPDF() {
        this.exportPDFClick(this.props.exportFieldUrl);
    }

    exportToExcel() {
        this.exportExcelClick(this.props.exportFieldUrl);
    }

    exportPDFClick(fieldUrl: string) {
        let count = this.getSelectedItems().length;

        if (count == 0) {
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
        KendoGrid1.instance.closeDialog();
        var gridColumns: any[] = [];
        var data: any[] = [];
        for (var i = 0; i < columns.selectedOptions.length; i++) {
            let fieldIndex = columns.fieldNames.indexOf(columns.selectedOptions[i])
            gridColumns.push({
                field: columns.selectedOptions[i],
                title: unescape(columns.fieldLabels[fieldIndex])
            });
        }

        let rows = this.getSelectedItems()

        for (var i = 0; i < rows.length; i++) {
            if (rows[i].selected == true) {
                data.push(rows[i]);
            }
        }
        var grid = $("<div id='pdfGrid'></div>");
        grid.KendoGrid1({
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
        grid.data("KendoGrid1").saveAsExcel();
        this.clearSelection();
    }

    exportExcelClick(fieldUrl: string) {
        let count = this.getSelectedItems().length;
        if (count == 0) {
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
        let data: any[] = [];
        if (this.props.selectionField && this.componentLoaded === false) {
            let selectedData: any[] = this.state.persistentSelection || [];
            for (let i in e.data) {
                if ((eval("e.data[i]." + this.props.selectionField) === true || this.state.gridHeaderChecked) && this.findSelectedIndex(e.data[i]) == -1) {
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
                        data.push({
                            ...e.data[i],
                            selected: this.hasBeenSelected(e.data[i])
                        })
                    }
                    if (this.state.currentColumnMenu) {
                        this.setState({ data: { total: e.total, data: data } }, this.state.currentColumnMenu.refresh);
                    } else {

                        this.setState({ data: { total: e.total, data: data } });
                    }
                    this.dataLoader = loader;
                });
            this.componentLoaded = true;
        } else {
            for (let i in e.data) {
                data.push({
                    ...e.data[i],
                    selected: this.hasBeenSelected(e.data[i]) || this.state.gridHeaderChecked
                })
            }
            if (this.state.currentColumnMenu) {
                this.setState({ data: { total: e.total, data: data } }, this.state.currentColumnMenu.refresh);
            } else {

                this.setState({ data: { total: e.total, data: data } });
            }
            this.dataLoader = loader;
        }
    }

    dataStateChange(e: any) {
        this.setState({
            ...this.state,
            currentColumnMenu: null,
            dataState: e.data
        });
    }

    onGridMenuClick(e: any) {
        let index = e.itemIndex;
        this.props.gridMenu[index].action(e, this);
    }

    renderGridMenu() {
        let menus: any[] = [];
        if (!this.props.gridMenu)
            return menus;
        for (let i in this.props.gridMenu) {
            menus.push(<a key="gridmenu{i}" className="dropdown-item" href="javascript:void(0)" onClick={this.onGridMenuClick}>{this.props.gridMenu[i].text}</a>)
        }
        return menus;
    }

    addRecordClicked(e: any) {
        if (this.props.addRecord.redirect && this.props.addRecord.redirect == true) {
            KendoGrid1.instance.setState({ redirect: true }, KendoGrid1.instance.forceUpdate)
        }
        else if (!this.props.addRecord.getUrl || this.props.addRecord.getUrl === "") {
            this.props.addRecord.action(e, KendoGrid1.instance);
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
            return (<BrowserRouter><Redirect to={this.props.addRecord.url} /></BrowserRouter>)
        }
    }

    renderAddRecordButton() {
        if (this.props.addRecord && this.props.addRecord.text != '') {
            return (<Button primary={true} onClick={this.addRecordClicked}>{this.props.addRecord.text}</Button>) //icon={this.props.addRecord.icon}
        }
        return null;
    }

    openSubmittableForm(param: any) {
        this._showSubmittableForm(param)
    }

    reloadData() {
        this.dataLoader.reloadData();
    }

    refresh(culumnMenu: any = null) {
        if (culumnMenu != null) {
            this.setState({ currentColumnMenu: culumnMenu });
        }
        this.dataLoader.reloadData();
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

    renderSearchBox() {
        return (
            <div className="search-form-r">
                <div className="input-group">
                    <Input autoFocus={true} onKeyPress={this.onSearchFieldKeyPress} placeholder="Search" ref={(c) => { this.searchText = c }} className="form-control" />
                    <div className="input-group-append">
                        <div className="input-group-text">
                            <a href={"javascript:void(0)"} onClick={this.onSearchButtonClicked} className="k-icon k-i-search">&nbsp;</a>
                        </div>
                    </div>
                </div>
                {this.props.showAdvanceSearchBox && <a href={"javascript:void(0)"} onClick={this.onSearchButtonClicked} className="btn btn-link">Advanced Search</a>}
            </div>
        )
    }

    onSwitchChange(e: any) {
        this.setState({ isSwitchOn: e.target.value })
        if (this.props.switchButton) {
            this.props.switchButton.action(e.target.value);
        }
    }

    renderSwitchButton() {
        if (this.props.switchButton != undefined) {
            return (<div className="switch-btn"><Switch offLabel={this.props.switchButton.offLabel} onLabel={this.props.switchButton.onLabel} className="switchButton" checked={this.state.isSwitchOn} onChange={this.onSwitchChange} />Task Order</div>);
        }
    }

    renderMoreButtons(showGridAction: any) {
        if (this.props.showGridAction == undefined || this.props.showGridAction == true) {
            return (
                <div className="text-right">
                    {this.props.showAddNewButton && <Button primary={true} onClick={this.props.addNewDistribution}>Add New</Button>} &nbsp;
                     <SplitButton text="Actions" items={this.props.gridMenu} onItemClick={this.onGridMenuClick} />
                </div>
            );
        }
    }

    renderSearchBoxAndActions() {
        if (this.props.showSearchBox == undefined || this.props.showSearchBox == true) {
            return (
                <div className={"row mb-3"}>
                    <div className="col-sm-8">
                        {this.renderSearchBox()}

                    </div>
                    <div className="col-sm-4">
                        {this.renderMoreButtons(this.props.showGridAction)}
                    </div>
                </div>
            )
        }
        else if (this.props.showSearchBox == false) {
            return (<div className={"row mb-3"}>
                <div className="col-sm-8">

                </div>

                <div className="col-sm-4">
                    {this.renderMoreButtons(this.props.showGridAction)}
                </div>
            </div>)
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

    render() {
        return (
            <div>
                {this.renderSearchBoxAndActions()}
                <Grid ref={(c) => { this.gridElement = c }} style={this.state.gridStyle} selectedField={this.state.showSelection ? "selected" : ""} {...this.state.data} {...this.state.dataState} sortable={true} onSortChange={this.onSortChange} sort={this.state.sort} resizable={true}
                    onSelectionChange={this.selectionChange} onHeaderSelectionChange={this.headerSelectionChange} onPageChange={this.onPageChange} pageable={this.state.pageable} onDataStateChange={this.dataStateChange}>
                    {this.renderSelectionField()}
                    {this.renderFields()}
                </Grid>
                <GridDataLoader
                    ref={(c) => { this.dataLoader = c }}
                    baseURL={this.props.dataURL}
                    method="get"
                    switchStatus={this.state.isSwitchOn}
                    dataState={this.state.dataState}
                    onDataRecieved={this.dataRecieved}
                />
                {this.renderRedirect()}
                <KendoDialog {...this.state.dialogProps}></KendoDialog>
            </div>
        );
    }
}