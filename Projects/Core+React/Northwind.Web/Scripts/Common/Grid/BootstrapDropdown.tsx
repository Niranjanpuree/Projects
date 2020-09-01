import * as React from 'react';
import * as ReactDOM from "react-dom"
import { Button } from '@progress/kendo-react-buttons';

interface IBootstrapDropdownProps {
    label: string;
    isRowMenu?: boolean;
    items: any[];
    onItemClick: any;
    dataItem?: any;
}

interface IBootstrapDropdownState {

}

export class BootstrapDropdown extends React.Component<IBootstrapDropdownProps, IBootstrapDropdownState>  {

    constructor(props: IBootstrapDropdownProps) {
        super(props);
        this.renderMenu = this.renderMenu.bind(this);
        this.onItemClick = this.onItemClick.bind(this);
    }

    render() {
        if (this.props.dataItem && this.props.dataItem.aggregates) {
            return (<label></label>)
        }
        else {
            return (<div className="dropdown dropdown-custom" style={{ display: "inline" }}>
                <button className="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown">
                    {this.props.isRowMenu && <div className="k-widget k-dropdown-button k-i-more-vertical">
                        <button className="k-button" aria-haspopup="true" aria-expanded="false" aria-label=" dropdownbutton"></button>
                    </div>}
                    {this.props.label}
                    {this.props.label && !this.props.isRowMenu && <span className="caret"></span>}</button>
                <div className="dropdown-menu dropdown-menu-right">
                    {this.renderMenu()}
                </div>
            </div>)
        }

    }

    renderMenu() {
        let rows: any[] = [];
        for (let i in this.props.items) {
            rows.push(<Button key={"key" + i} className="dropdown-item" itemID={i} onClick={this.onItemClick}>
                <i className={"k-icon mr-2  k-i-" + this.props.items[i].icon}></i>
                {this.props.items[i].text}</Button>)

        }
        return rows;
    }

    onItemClick(e: any) {
        let index = e.target.getAttribute("itemid");
        e.itemIndex = index;
        this.props.onItemClick(e, this.props.items[index], this.props.dataItem);
    }
}