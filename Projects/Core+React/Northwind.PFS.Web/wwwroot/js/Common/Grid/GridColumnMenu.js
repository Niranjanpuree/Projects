"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const kendo_react_grid_1 = require("@progress/kendo-react-grid");
const kendo_react_buttons_1 = require("@progress/kendo-react-buttons");
class GridColumnMenu extends React.Component {
    constructor(props) {
        super(props);
        this.selectedColumns = [];
        this.state = {
            selectedColumns: [],
            columns: [],
            columnsExpanded: false,
            filterExpanded: false
        };
        this.onMenuItemClick = this.onMenuItemClick.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
        this.onToggleColumn = this.onToggleColumn.bind(this);
        this.sortColumns = this.sortColumns.bind(this);
        this.onCheckedChange = this.onCheckedChange.bind(this);
    }
    onMenuItemClick() {
        const value = !this.state.columnsExpanded;
        this.setState({
            columnsExpanded: value,
            filterExpanded: value ? false : this.state.filterExpanded
        });
    }
    onSubmit(event) {
        event.preventDefault();
        this.props.grid.updateColumnVisibility(this.state.selectedColumns);
        this.setState({
            columnsExpanded: false,
            filterExpanded: false
        }, this.forceUpdate);
        $(".k-header .k-i-more-vertical").click();
    }
    onToggleColumn(e) {
        this.setState(this.sortColumns(e));
    }
    sortColumns(e) {
        let columns = this.state.selectedColumns;
        let notSelected = [];
        this.state.columns.forEach((col, idx) => {
            if (parseInt(e.currentTarget.getAttribute("itemid")) == idx) {
                col.show = !col.show;
                if (col.show) {
                    let c = columns.find((d) => {
                        if (d.fieldName == col.fieldName)
                            return true;
                    });
                    if (!c) {
                        col.orderIndex = columns.length;
                        columns.push(col);
                    }
                }
                else {
                    let c = columns.find((d) => {
                        if (d.fieldName == col.fieldName)
                            return true;
                    });
                    if (c) {
                        let cols = [];
                        columns.forEach((d) => {
                            if (d.fieldName != col.fieldName)
                                cols.push(d);
                        });
                        columns = cols;
                    }
                }
            }
        });
        this.state.columns.forEach((col) => {
            let c = columns.find((d) => {
                if (d.fieldName == col.fieldName)
                    return true;
            });
            if (!c) {
                notSelected.push(col);
            }
        });
        notSelected = notSelected.sort((a, b) => {
            if (a.fieldName < b.fieldName)
                return -1;
            else
                return 1;
        });
        return {
            selectedColumns: columns,
            columns: columns.concat(notSelected)
        };
    }
    componentDidMount() {
        if (this.state.columns.length > 0)
            return;
        let selectedcolumns = [];
        let currentSelected = [];
        this.props.gridColumns.forEach((col, idx) => {
            if (col.fieldName != 'menu' && col.fieldLabel != ' ') {
                col.orderIndex = idx;
                col.visibleToGrid = true;
                selectedcolumns.push(col);
                currentSelected.push(col);
            }
        });
        let notSelected = [];
        selectedcolumns = selectedcolumns.sort((a, b) => {
            if (a.fieldName < b.fieldName)
                return -1;
            else
                return 1;
        });
        this.props.columns.forEach((col) => {
            let c = selectedcolumns.find((d) => {
                if (d.fieldName == col.fieldName)
                    return true;
            });
            if (!c) {
                notSelected.push(col);
            }
        });
        notSelected = notSelected.sort((a, b) => {
            if (a.fieldName < b.fieldName)
                return -1;
            else
                return 1;
        });
        selectedcolumns = selectedcolumns.concat(notSelected);
        this.setState({ columns: selectedcolumns, selectedColumns: currentSelected });
    }
    onCheckedChange(e) {
        let arr = this.state.columns;
        //arr[idx].show = !arr[idx].show;
        this.setState({ columns: arr });
    }
    render() {
        let lastcolumn = {};
        const oneVisibleColumn = this.state.columns.filter(c => c.show).length === 1;
        if (oneVisibleColumn == true) {
            lastcolumn = this.state.columns.filter(c => c.show)[0];
        }
        return (React.createElement("div", null,
            React.createElement(kendo_react_grid_1.GridColumnMenuItemGroup, null,
                React.createElement(kendo_react_grid_1.GridColumnMenuItemContent, { show: true },
                    React.createElement("div", { className: 'k-column-list-wrapper' },
                        React.createElement("form", { noValidate: true, onSubmit: this.onSubmit },
                            React.createElement("div", { className: 'k-column-list' }, this.state.columns.map((column, idx) => (React.createElement("div", { key: idx, itemID: idx + "", className: 'k-column-list-item', onClick: this.onToggleColumn },
                                React.createElement("input", { id: "column-visiblity-show-${idx}", name: `column-visiblity-show-${idx}`, className: "k-checkbox", type: "checkbox", readOnly: true, disabled: oneVisibleColumn && lastcolumn == column, checked: column.show, onChange: this.onCheckedChange }),
                                React.createElement("label", { htmlFor: `column-visiblity-show-${idx}`, className: "k-checkbox-label", style: { userSelect: 'none' } }, column.fieldLabel))))),
                            React.createElement("div", { className: "p-2 text-right" },
                                React.createElement(kendo_react_buttons_1.Button, { type: "submit" }, " Ok "))))))));
    }
}
exports.GridColumnMenu = GridColumnMenu;
//# sourceMappingURL=GridColumnMenu.js.map