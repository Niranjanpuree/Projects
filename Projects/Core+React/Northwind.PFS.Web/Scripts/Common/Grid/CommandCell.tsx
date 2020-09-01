import * as React from 'react';
import * as ReactDOM from "react-dom"
import { DropDownButton } from '@progress/kendo-react-buttons';

interface IGridCellProps
{
    menuItems: any[];
    dataItem: any;
    parent: any;
}

interface IGridCellState
{
    menus: any[];
}

export class CommandCell extends React.Component<IGridCellProps, IGridCellState>  {
    _isMounted = false;
    constructor(props: IGridCellProps)
    {
        super(props);
        this.state = {
            menus: []
        }
        this.menuClicked = this.menuClicked.bind(this);
        this.renderMenuItem = this.renderMenuItem.bind(this);
        this.refresh = this.refresh.bind(this);
    }

    componentDidMount()
    {
        this._isMounted = true;
        this.refresh();
    }

    refresh()
    {
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
                if (this._isMounted) {
                    this.setState({ menus: arr }, this.forceUpdate);
                }

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
                if (this._isMounted) {
                    this.setState({ menus: arr }, this.forceUpdate);
                }

            }
        }
    }

    componentWillUnmount()
    {
        this._isMounted = false;
    }

    shouldComponentUpdate()
    {
        this.refresh();
        return false;
    }

    menuClicked(e: any)
    {
        let idx = e.target.getAttribute("itemid");
        e.dataItem = this.props.dataItem
        let index = this.state.menus[idx].itemIndex;
        e.menuController = this;
        this.props.menuItems[index].action(e, this.props.parent);
    }

    renderMenuItem()
    {
        let arr: any[] = [];
        this.state.menus.map((v: any, i: any) =>
        {
            arr.push(<a key={"k" + i} className="dropdown-item" href="javascript:void(0)" itemID={i} onClick={this.menuClicked}><i className={"k-icon k-i-"+v.icon}></i> {v.text}</a>)
        }
        )
        return arr;
    }

    render()
    {
        let id = "kendoGrid" + (new Date()).getTime();
        if (this.props.dataItem.aggregates) {
            return (<label></label>)
        }
        else {
            return (
                <div className="dropdown show">
                    <a className="k-i-more-vertical k-icon dropdown-toggle1" href="#" role="button" id={id} data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"></a>
                    <div className="dropdown-menu" aria-labelledby={id}>
                        {this.renderMenuItem()}
                    </div>
                </div>
            );
        }
    }
}