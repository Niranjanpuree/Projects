import * as React from "react"
import * as ReactDOM from "react-dom"
import { Button } from "@progress/kendo-react-buttons";
import { AutoComplete } from '@progress/kendo-react-dropdowns';
import { resetWarningCache } from "prop-types";
import { KendoGrid } from "../Grid/KendoGrid";
import { Remote } from "../Remote/Remote";
declare var window: any;

interface IMultiSelectPanelProps {
    searchField: string;
    searchUrl: string;
    gridWidth: any;
    gridIdentityField: any;
    gridDataUrl: any;
    gridDataFieldUrl: any;
    gridHeight?: string;
    rowMenus: any[];
    postUrl?: string;
    parent?: any;
}

interface IMultiSelectPanelState {
    data: any[];
    value: string;
    selectedValue: any;
    gridHeight: any;
}

export class MultiSelectPanel extends React.Component<IMultiSelectPanelProps, IMultiSelectPanelState> {
    textField: AutoComplete;
    grid: KendoGrid;

    constructor(props: any) {
        super(props);

        this.state = {
            data: [],
            value: '',
            selectedValue: {},
            gridHeight: this.props.gridHeight
        }

        this.onBlur = this.onBlur.bind(this);
        this.onClose = this.onClose.bind(this);
        this.onFocus = this.onFocus.bind(this);
        this.onChange = this.onChange.bind(this);
        this.onOpen = this.onOpen.bind(this);
        this.populateSearchValue = this.populateSearchValue.bind(this);
        this.itemRender = this.itemRender.bind(this);
        this.assignToGroup = this.assignToGroup.bind(this);
    }

    populateSearchValue(str: string) {
        Remote.get(this.props.searchUrl + str, (result: any) => {
            this.setState({ data: result.result }, this.forceUpdate);
        }, (error: any) => { window.Dialog.alert(error, "Error"); })
    }

    onOpen(e: any) {

    }

    onClose(e: any) {
        let txt = this.textField.value;
        for (let i in this.state.data) {
            if (eval("this.state.data[" + i + "]." + this.props.searchField) == txt) {
                this.setState({ selectedValue: this.state.data[i]}, this.forceUpdate)
            }
        }
        this.textField.setState({ value: "" }, this.forceUpdate);
        this.grid.reloadData();
    }

    onFocus(e: any) {

    }

    onBlur(e: any) {

    }

    onChange(e: any) {
        this.populateSearchValue(this.textField.value);
    }

    assignToGroup(e: any) {
        if (this.props.postUrl) {
            let sender = this;
            let index = parseInt(e.target.getAttribute("itemRef"));
            let data = this.state.data[index];
            Remote.postData(this.props.postUrl, data, (result: any) => { setTimeout(function () { sender.grid.reloadData() }, 1000) }, (error: any) => { window.alert(error) });
        }        
    }

    itemRender(li: any, itemProps: any) {
        const index = itemProps.index;
        const itemChildren = <div className="popup-item row"><span className="label">{li.props.children}</span><span className="button"><Button itemRef={index} onClick={this.assignToGroup}>Assign</Button></span></div>;
        return React.cloneElement(li, li.props, itemChildren);
    }

    render() {
        return (<div>
            <AutoComplete
                ref={(c) => { this.textField = c; }}
                textField={this.props.searchField}
                data={this.state.data}
                onOpen={this.onOpen}
                onClose={this.onClose}
                onFocus={this.onFocus}
                onBlur={this.onBlur}
                onChange={this.onChange}
                placeholder={"Type here to search"}
                itemRender={this.itemRender}
                className="autoComplete"
            />
            <KendoGrid ref={(c) => { this.grid = c; }} dataURL={this.props.gridDataUrl} fieldUrl={this.props.gridDataFieldUrl} gridWidth={this.props.gridWidth} identityField={this.props.gridIdentityField} rowMenus={this.props.rowMenus} showGridAction={false} showSearchBox={false} showSelection={false} parent={this.props.parent} gridHeight={this.state.gridHeight} />
        </div>);
    }
}