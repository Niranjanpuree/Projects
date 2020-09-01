"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
class AdvancedSearchHeader extends React.Component {
    render() {
        return (React.createElement("div", { className: "row form-group" },
            React.createElement("div", { className: "col-sm-3" },
                React.createElement("label", { className: "font-weight-bold mb-0", htmlFor: "conditionAttribute" }, "Attribute")),
            React.createElement("div", { className: "col-sm-3" },
                React.createElement("label", { className: "font-weight-bold mb-0", htmlFor: "Operator" }, "Operator")),
            React.createElement("div", { className: "col-sm-3" },
                React.createElement("label", { className: "font-weight-bold mb-0", htmlFor: "Value" }, "Value"))));
    }
}
exports.AdvancedSearchHeader = AdvancedSearchHeader;
//# sourceMappingURL=AdvancedSearchHeader.js.map