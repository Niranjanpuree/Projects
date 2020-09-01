"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const kendo_react_buttons_1 = require("@progress/kendo-react-buttons");
class BootstrapRowMenu extends React.Component {
    constructor(props) {
        super(props);
        this.renderMenu = this.renderMenu.bind(this);
        this.onItemClick = this.onItemClick.bind(this);
    }
    render() {
        if (this.props.dataItem && this.props.dataItem.aggregates) {
            return (React.createElement("label", null));
        }
        else {
            return (React.createElement("div", { className: "dropdown dropdown-custom", style: { display: "inline" } },
                React.createElement("button", { className: "btn dropdown-toggle", type: "button", "data-toggle": "dropdown" },
                    React.createElement("div", { className: "k-widget k-dropdown-button k-i-more-vertical" })),
                React.createElement("div", { className: "dropdown-menu dropdown-menu-right" }, this.renderMenu())));
        }
    }
    renderMenu() {
        let rows = [];
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
                rows.push(React.createElement(kendo_react_buttons_1.Button, { key: "key" + i, className: "dropdown-item", itemID: i, onClick: this.onItemClick },
                    React.createElement("i", { className: "k-icon mr-2 k-i-" + this.props.items[i].icon }),
                    this.props.items[i].text));
            }
        }
        return rows;
    }
    onItemClick(e) {
        let index = e.target.getAttribute("itemid");
        e.itemIndex = index;
        this.props.onItemClick(e, this.props.items[index], this.props.dataItem);
    }
}
exports.BootstrapRowMenu = BootstrapRowMenu;
//# sourceMappingURL=BootstrapRowMenu.js.map