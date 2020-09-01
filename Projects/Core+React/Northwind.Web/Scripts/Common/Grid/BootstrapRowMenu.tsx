import * as React from 'react';
import * as ReactDOM from "react-dom"
import { Button } from '@progress/kendo-react-buttons';

interface IBootstrapRowMenuProps {
    items: any[];
    onItemClick: any;
    dataItem?: any;
}

interface IBootstrapRowMenuState {

}

export class BootstrapRowMenu extends React.Component<IBootstrapRowMenuProps, IBootstrapRowMenuState>  {

    constructor(props: IBootstrapRowMenuProps) {
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
                <button className="btn dropdown-toggle" type="button" data-toggle="dropdown">
                    <div className="k-widget k-dropdown-button k-i-more-vertical">

                    </div>
                </button>
                <div className="dropdown-menu dropdown-menu-right">
                    {this.renderMenu()}
                </div>
            </div>)
        }

    }

    renderMenu() {
        let rows: any[] = [];
        for (let i in this.props.items) {
            let menu = this.props.items[i];
            let isVisible = !menu.conditions;
            if (isVisible === false) {
                let allTrue = true;
                for (let j = 0; j < menu.conditions.length; j++) {
                    if (this.props.dataItem[menu.conditions[j].field] !== menu.conditions[j].value) {
                        allTrue = false;
                        break;
                    }
                }
                isVisible = allTrue;
            }
            let index = i;
            if (isVisible) {
                rows.push(<Button key={"key" + i} className="dropdown-item" itemID={i} onClick={this.onItemClick}>
                    <i className={"k-icon mr-2 k-i-" + this.props.items[i].icon}></i>
                    {this.props.items[i].text}</Button>)

            }

        }
        return rows;
    }

    onItemClick(e: any) {
        let index = e.target.getAttribute("itemid");
        e.itemIndex = index;
        this.props.onItemClick(e, this.props.items[index], this.props.dataItem);
    }
}