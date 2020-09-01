"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
class LoadingPanel extends React.Component {
    render() {
        return (React.createElement("div", { className: "k-loading-mask" },
            React.createElement("span", { className: "k-loading-text" }, "Loading"),
            React.createElement("div", { className: "k-loading-image" }),
            React.createElement("div", { className: "k-loading-color" })));
        //const gridContent = document && document.querySelector('.k-grid-content');
        //return gridContent ? ReactDOM.createPortal(loadingPanel, gridContent) : loadingPanel;
        //return loadingPanel;
    }
}
exports.LoadingPanel = LoadingPanel;
//# sourceMappingURL=LoadingPanel.js.map