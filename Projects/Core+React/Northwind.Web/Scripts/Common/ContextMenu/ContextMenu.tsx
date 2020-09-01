import * as React from 'react';
import { Button } from "@progress/kendo-react-buttons";
declare var $: any;

interface ContextMenuProps {
    menus: any[];
    sender: any;
    style: any;
    item: any;
}

interface ContextMenuState {
}

export class ContextMenu extends React.Component<ContextMenuProps, ContextMenuState> {
    constructor(props: any) {
        super(props);
        this.onMouseOut = this.onMouseOut.bind(this);
        this.onClick = this.onClick.bind(this);
    }

    onClick(e: any) {
        let b: Button = e.target;
        let index = e.target.getAttribute("itemid");
        if (this.props.menus && this.props.menus[index] && this.props.menus[index].action)
            this.props.menus[index].action(e, this.props.item);
    }

    onMouseOut(e: any) {
        this.props.sender.onContextMenuMouseOut(e, this.props.item);
    }

    renderMenu() {
        let menus: any[] = [];
        this.props.menus.map((item: any, index: number) => {
            menus.push(<Button type="button" className="col-sm-12 context-menu-item" style={{ textAlign: 'left', display: 'block' }} icon={item.icon} key={index} itemID={index + ""} onClick={this.onClick}>{item.text}</Button>)
        });
        return menus;
    }

    render() {
        return (
            <div id="context_menu_container" className="row context-menu" style={this.props.style} onMouseLeave={this.onMouseOut}>
               {this.renderMenu()}
            </div>
        )
    }
}