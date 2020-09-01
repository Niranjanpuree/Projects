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
var LoadingPanel = /** @class */ (function (_super) {
    __extends(LoadingPanel, _super);
    function LoadingPanel() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    LoadingPanel.prototype.render = function () {
        return (React.createElement("div", { className: "k-loading-mask" },
            React.createElement("span", { className: "k-loading-text" }, "Loading"),
            React.createElement("div", { className: "k-loading-image" }),
            React.createElement("div", { className: "k-loading-color" })));
        //const gridContent = document && document.querySelector('.k-grid-content');
        //return gridContent ? ReactDOM.createPortal(loadingPanel, gridContent) : loadingPanel;
        //return loadingPanel;
    };
    return LoadingPanel;
}(React.Component));
exports.LoadingPanel = LoadingPanel;
//# sourceMappingURL=LoadingPanel.js.map