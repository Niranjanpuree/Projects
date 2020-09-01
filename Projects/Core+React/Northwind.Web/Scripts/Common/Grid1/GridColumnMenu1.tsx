import * as React from 'react';
import { DropDownList } from '@progress/kendo-react-dropdowns';
import {
    GridFilterCellProps,
    GridColumnMenuSort,
    GridColumnMenuFilter, GridColumnMenuProps, GridColumnMenuItemGroup, GridColumnMenuItemContent, GridColumnMenuItem
} from '@progress/kendo-react-grid';
import { process } from '@progress/kendo-data-query';
import { Button } from '@progress/kendo-react-buttons';

interface CustomColumnMenuProps extends GridColumnMenuProps {
    gridColumns: any[];
    columns: any[];
    grid: any;
}

interface GridColumnMenuState {
    selectedColumns: any[];
    columns: any[];
    columnsExpanded: boolean;
    filterExpanded: boolean;
}

export class GridColumnMenu1 extends React.Component<CustomColumnMenuProps, GridColumnMenuState> {
    selectedColumns: any[] = []
    constructor(props: any) {
        super(props);
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

    onSubmit(event: any) {
        event.preventDefault();
        this.props.grid.updateColumnVisibility(this.state.selectedColumns);
        this.setState({
            columnsExpanded: false,
            filterExpanded: false
        }, this.forceUpdate);
    }

    onToggleColumn(e: any) {
        this.setState(this.sortColumns(e));
    }

    sortColumns(e: any): any {
        let columns: any[] = this.state.selectedColumns;
        let notSelected: any[] = [];
        this.state.columns.forEach((col: any, idx: number) => {
            if (parseInt(e.currentTarget.getAttribute("itemid")) == idx) {
                col.show = !col.show;
                if (col.show) {
                    let c = columns.find((d: any) => {
                        if (d.fieldName == col.fieldName)
                            return true;
                    });
                    if (!c) {
                        col.orderIndex = columns.length;
                        columns.push(col);
                    }
                }
                else {
                    let c = columns.find((d: any) => {
                        if (d.fieldName == col.fieldName)
                            return true;
                    });
                    if (c) {
                        let cols: any[] = [];
                        columns.forEach((d) => {
                            if (d.fieldName != col.fieldName)
                                cols.push(d);
                        })
                        columns = cols;
                    }
                }
            }
        })

        this.state.columns.forEach((col: any) => {
            let c = columns.find((d) => {
                if (d.fieldName == col.fieldName)
                    return true;
            })
            if (!c) {
                notSelected.push(col);
            }
        })

        notSelected = notSelected.sort((a: any, b: any) => {
            if (a.fieldName < b.fieldName)
                return -1;
            else
                return 1;
        })
        return {
            selectedColumns: columns,
            columns: columns.concat(notSelected)
        };
    }

    componentDidMount() {
        if (this.state.columns.length > 0)
            return;
        let selectedcolumns: any[] = []
        let currentSelected: any[] = [];
        this.props.gridColumns.forEach((col: any, idx: number) => {
            if (col.fieldName != 'menu' && col.fieldLabel != ' ') {
                col.orderIndex = idx;
                col.visibleToGrid = true;
                selectedcolumns.push(col)
                currentSelected.push(col)
            }
        });
        let notSelected: any[] = [];
        selectedcolumns = selectedcolumns.sort((a: any, b: any) => {
            if (a.fieldName < b.fieldName)
                return -1;
            else
                return 1;
        })

        this.props.columns.forEach((col: any) => {
            let c = selectedcolumns.find((d) => {
                if (d.fieldName == col.fieldName)
                    return true;
            })
            if (!c) {
                notSelected.push(col);
            }
        })
        notSelected = notSelected.sort((a: any, b: any) => {
            if (a.fieldName < b.fieldName)
                return -1;
            else
                return 1;
        })
        selectedcolumns = selectedcolumns.concat(notSelected);
        this.setState({ columns: selectedcolumns, selectedColumns: currentSelected });
    }

    onCheckedChange(e: any) {
        let arr = this.state.columns;
        //arr[idx].show = !arr[idx].show;
        this.setState({ columns: arr });
    }

    render() {
        let lastcolumn: any = {};
        const oneVisibleColumn = this.state.columns.filter(c => c.show).length === 1;
        if (oneVisibleColumn == true) {
            lastcolumn = this.state.columns.filter(c => c.show)[0]
        }
        return (
            <div>
                <GridColumnMenuItemGroup>
                    <GridColumnMenuItemContent show={true}>
                        <div className={'k-column-list-wrapper'}>
                            <form noValidate onSubmit={this.onSubmit}>
                                <div className={'k-column-list'}>
                                    {this.state.columns.map((column, idx) =>
                                        (
                                            <div key={idx} itemID={idx + ""} className={'k-column-list-item'} onClick={this.onToggleColumn}>
                                                <input
                                                    id={"column-visiblity-show-${idx}"}
                                                    name={`column-visiblity-show-${idx}`}
                                                    className="k-checkbox"
                                                    type="checkbox"
                                                    readOnly={true}
                                                    disabled={oneVisibleColumn && lastcolumn == column}
                                                    checked={column.show}
                                                    onChange={this.onCheckedChange}
                                                />
                                                <label
                                                    htmlFor={`column-visiblity-show-${idx}`}
                                                    className="k-checkbox-label"
                                                    style={{ userSelect: 'none' }}
                                                >
                                                    {column.fieldLabel}
                                                </label>
                                            </div>
                                        )
                                    )}
                                </div>
                                <div className="p-2 text-right">
                                    <Button type="submit"> Ok </Button>
                                </div>
                            </form>
                        </div>
                    </GridColumnMenuItemContent>
                </GridColumnMenuItemGroup>
            </div>
        );
    }
}
