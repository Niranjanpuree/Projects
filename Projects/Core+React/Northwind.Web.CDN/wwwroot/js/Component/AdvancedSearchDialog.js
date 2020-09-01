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
var kendo_react_dialogs_1 = require("@progress/kendo-react-dialogs");
var AdvancedSearch_1 = require("./AdvancedSearch");
var kendo_react_buttons_1 = require("@progress/kendo-react-buttons");
var AdvancedSearchDialog = /** @class */ (function (_super) {
    __extends(AdvancedSearchDialog, _super);
    function AdvancedSearchDialog(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            visible: false
        };
        _this.toggleDialog = _this.toggleDialog.bind(_this);
        return _this;
    }
    AdvancedSearchDialog.prototype.toggleDialog = function () {
        var currentState = this.state.visible;
        this.setState({ visible: !currentState });
    };
    AdvancedSearchDialog.prototype.render = function () {
        return (React.createElement("div", null,
            !this.state.visible && React.createElement("a", { href: "#", onClick: this.toggleDialog }, "Click here to select"),
            this.state.visible && React.createElement(kendo_react_dialogs_1.Dialog, { title: "Advanced Search", width: "80%", height: "70%" },
                React.createElement("div", { className: "container" },
                    React.createElement(AdvancedSearch_1.AdvancedSearch, null)),
                React.createElement(kendo_react_dialogs_1.DialogActionsBar, null,
                    React.createElement(kendo_react_buttons_1.Button, { primary: true }, "Apply"),
                    React.createElement(kendo_react_buttons_1.Button, { onClick: this.toggleDialog }, "Cancel")))));
    };
    return AdvancedSearchDialog;
}(React.Component));
exports.AdvancedSearchDialog = AdvancedSearchDialog;
//# sourceMappingURL=AdvancedSearchDialog.js.map