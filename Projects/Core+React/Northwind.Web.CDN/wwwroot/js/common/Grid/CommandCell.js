"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var kendo_react_buttons_1 = require("@progress/kendo-react-buttons");
var CommandCell = /** @class */ (function (_super) {
    __extends(CommandCell, _super);
    function CommandCell(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            menus: []
        };
        _this.menuClicked = _this.menuClicked.bind(_this);
        return _this;
    }
    CommandCell.prototype.componentDidMount = function () {
        var arr = [];
        for (var i in this.props.menuItems) {
            if (this.props.menuItems[i].icon) {
                arr.push({
                    text: this.props.menuItems[i].text,
                    icon: this.props.menuItems[i].icon
                });
            }
            else {
                arr.push(this.props.menuItems[i].text);
            }
        }
        this.setState({ menus: arr });
    };
    CommandCell.prototype.menuClicked = function (e) {
        e.dataItem = this.props.dataItem;
        this.props.menuItems[e.itemIndex].action(e, this.props.parent);
    };
    CommandCell.prototype.render = function () {
        return (React.createElement(kendo_react_buttons_1.DropDownButton, { text: "", className: ".k-i-more-vertical Unicode: e031", items: this.state.menus, onItemClick: this.menuClicked }));
    };
    return CommandCell;
}(React.Component));
exports.CommandCell = CommandCell;
//# sourceMappingURL=CommandCell.js.map