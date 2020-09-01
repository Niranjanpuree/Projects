import * as React from 'react';
import * as ReactDOM from "react-dom"
import { DropDownButton } from '@progress/kendo-react-buttons';

interface IGridCellProps {
    menuItems: any[];
    dataItem: any;
    parent: any;
}

interface IGridCellState {
    menus: any[];
}

export class CommandCell1 extends React.Component<IGridCellProps, IGridCellState>  {

    constructor(props: IGridCellProps) {
        super(props);
        this.state={
            menus: []
        }
        this.menuClicked = this.menuClicked.bind(this);
        this.refresh = this.refresh.bind(this);
    }

    componentDidMount() {
        this.refresh();
    }

    refresh() {
        let arr: any[] = [];
        for (let i = 0; i < this.props.menuItems.length; i++) {
            let isConditionalMenu = false;
            if (this.props.menuItems[i].conditions) {
                isConditionalMenu = true;
            }
            if (isConditionalMenu) {
                let showMenu = true;
                for (let j = 0; j < this.props.menuItems[i].conditions.length; j++) {
                    const propName = "this.props.dataItem." + this.props.menuItems[i].conditions[j].field;
                    let prop = eval(propName);
                    let value = this.props.menuItems[i].conditions[j].value;
                    if (prop != value) {
                        showMenu = false;
                        j = this.props.menuItems[i].conditions.length;
                    }
                }
                if (showMenu) {
                    if (this.props.menuItems[i].icon) {
                        arr.push({
                            text: this.props.menuItems[i].text,
                            icon: this.props.menuItems[i].icon,
                            itemIndex: i
                        })
                    }
                    else {
                        arr.push({
                            text: this.props.menuItems[i].text,
                            itemIndex: i
                        })
                    }
                }
                this.setState({ menus: arr }, this.forceUpdate);
            }
            else {
                if (this.props.menuItems[i].icon) {
                    arr.push({
                        text: this.props.menuItems[i].text,
                        icon: this.props.menuItems[i].icon,
                        itemIndex: i
                    })
                }
                else {
                    arr.push({
                        text: this.props.menuItems[i].text,
                        itemIndex: i
                    })
                }
                this.setState({ menus: arr }, this.forceUpdate);
            }
        }
    }

    shouldComponentUpdate() {
        this.refresh();
        return false;
    }

    menuClicked(e: any) {
        e.dataItem = this.props.dataItem
        let index = this.state.menus[e.itemIndex].itemIndex;
        e.menuController = this;
        this.props.menuItems[index].action(e, this.props.parent);
    }
    
    render() {
        if (this.props.dataItem.aggregates) {
            return (<label></label>)
        }
        else {
            return (
                <DropDownButton text="" className="k-i-more-vertical" items={this.state.menus} onItemClick={this.menuClicked} />
            );
        }
    }
}