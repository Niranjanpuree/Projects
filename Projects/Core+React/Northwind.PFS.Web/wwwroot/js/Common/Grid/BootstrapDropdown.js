"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const kendo_react_buttons_1 = require("@progress/kendo-react-buttons");
class BootstrapDropdown extends React.Component {
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
                React.createElement("button", { className: "btn btn-primary dropdown-toggle", type: "button", "data-toggle": "dropdown" },
                    this.props.isRowMenu && React.createElement("div", { className: "k-widget k-dropdown-button k-i-more-vertical" },
                        React.createElement("button", { className: "k-button", "aria-haspopup": "true", "aria-expanded": "false", "aria-label": " dropdownbutton" })),
                    this.props.label,
                    this.props.label && !this.props.isRowMenu && React.createElement("span", { className: "caret" })),
                React.createElement("div", { className: "dropdown-menu dropdown-menu-right" }, this.renderMenu())));
        }
    }
    renderMenu() {
        let rows = [];
        for (let i in this.props.items) {
            rows.push(React.createElement(kendo_react_buttons_1.Button, { key: "key" + i, className: "dropdown-item", itemID: i, onClick: this.onItemClick },
                React.createElement("i", { className: "k-icon mr-2  k-i-" + this.props.items[i].icon }),
                this.props.items[i].text));
        }
        return rows;
    }
    onItemClick(e) {
        let index = e.target.getAttribute("itemid");
        e.itemIndex = index;
        this.props.onItemClick(e, this.props.items[index], this.props.dataItem);
    }
}
exports.BootstrapDropdown = BootstrapDropdown;
//# sourceMappingURL=BootstrapDropdown.js.map