"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
class SearchPills extends React.Component {
    constructor(props) {
        super(props);
        this.isDate = this.isDate.bind(this);
        this.getValue = this.getValue.bind(this);
        this.onPillDelete = this.onPillDelete.bind(this);
        this.onClearAllPills = this.onClearAllPills.bind(this);
    }
    isDate(value) {
        try {
            var d = new Date(value);
            if (d.getDate())
                return true;
            else
                return false;
        }
        catch (e) {
            return false;
        }
    }
    getValue(value) {
        if (!value)
            return "";
        if (this.isDate(value)) {
            return "'" + value.toLocaleDateString("en-US") + "'";
        }
        else if (value.name) {
            return "'" + value.name.toString() + "'";
        }
        return value;
    }
    onPillDelete(e) {
        if (e.target && e.currentTarget.getAttribute("itemid")) {
            if (this.props.onPillDelete) {
                this.props.onPillDelete(this.props.conditions[e.currentTarget.getAttribute("itemid")]);
            }
        }
    }
    onClearAllPills(e) {
        if (this.props.onClearAll) {
            this.props.onClearAll();
        }
    }
    render() {
        return (React.createElement("div", { className: "search-pills-container" },
            this.props.conditions.map((condition, index) => {
                if (typeof (condition.Value) === "string") {
                    let value2 = "";
                    if (condition.Value2) {
                        value2 = " And '" + condition.Value2 + "'";
                    }
                    return React.createElement("div", { key: "pill-" + index, className: "badge badge-pill badge-secondary" },
                        condition.Attribute.AttributeTitle,
                        " ",
                        condition.Operator.OperatorTitle,
                        " '",
                        condition.Value,
                        "' ",
                        value2,
                        " ",
                        React.createElement("a", { href: "#", itemID: index.toString(), className: "pill-close", onClick: this.onPillDelete },
                            React.createElement("i", { className: "material-icons" }, "close")));
                }
                else if (typeof (condition.Value) == "object" && condition.Value.length) {
                    let str = "";
                    for (let i in condition.Value) {
                        if (str != "")
                            str += ", ";
                        str += condition.Value[i].name;
                    }
                    return React.createElement("div", { key: "pill-" + index, className: "badge badge-pill badge-secondary" },
                        condition.Attribute.AttributeTitle,
                        " ",
                        condition.Operator.OperatorTitle,
                        " '",
                        str,
                        "' ",
                        React.createElement("a", { href: "#", itemID: index.toString(), className: "pill-close", onClick: this.onPillDelete },
                            React.createElement("i", { className: "material-icons" }, "close")));
                }
                else if (typeof (condition.Value) == "object" && !condition.Value.length) {
                    var value2 = "";
                    if (this.getValue(condition.Value2)) {
                        value2 = " AND '" + this.getValue(condition.Value2) + "'";
                    }
                    return React.createElement("div", { key: "pill-" + index, className: "badge badge-pill badge-secondary" },
                        condition.Attribute.AttributeTitle,
                        " ",
                        condition.Operator.OperatorTitle,
                        " ",
                        this.getValue(condition.Value),
                        " ",
                        value2,
                        " ",
                        React.createElement("a", { href: "#", itemID: index.toString(), className: "pill-close", onClick: this.onPillDelete },
                            React.createElement("i", { className: "material-icons" }, "close")));
                }
            }),
            this.props.conditions.length > 0 && React.createElement("div", { className: "badge badge-pill badge-secondary" },
                "Clear All ",
                React.createElement("a", { href: "#", className: "pill-close", onClick: this.onClearAllPills },
                    React.createElement("i", { className: "material-icons" }, "close")))));
    }
}
exports.SearchPills = SearchPills;
//# sourceMappingURL=SearchPills.js.map