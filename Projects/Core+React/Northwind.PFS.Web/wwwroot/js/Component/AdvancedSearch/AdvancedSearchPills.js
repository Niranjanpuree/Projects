"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
class AdvancedSearchPills extends React.Component {
    constructor(props) {
        super(props);
        this.isDate = this.isDate.bind(this);
        this.getValue = this.getValue.bind(this);
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
    render() {
        return (React.createElement("div", { className: "badge badge-pill badge-secondary" }, this.props.conditions.map((condition, index) => {
            if (typeof (condition.Value) === "string") {
                let value2 = "";
                if (condition.Value2) {
                    value2 = " And '" + condition.Value2 + "'";
                }
                return React.createElement("div", { key: "pill-" + index },
                    condition.Attribute.AttributeTitle,
                    " ",
                    condition.Operator.OperatorTitle,
                    " '",
                    condition.Value,
                    "' ",
                    value2);
            }
            else if (typeof (condition.Value) == "object" && condition.Value.length) {
                let str = "";
                for (let i in condition.Value) {
                    if (str != "")
                        str += ", ";
                    str += condition.Value[i].name;
                }
                return React.createElement("div", { key: "pill-" + index },
                    condition.Attribute.AttributeTitle,
                    " ",
                    condition.Operator.OperatorTitle,
                    " '",
                    str,
                    "'");
            }
            else if (typeof (condition.Value) == "object" && !condition.Value.length) {
                var value2 = "";
                if (this.getValue(condition.Value2)) {
                    value2 = " AND '" + this.getValue(condition.Value2) + "'";
                }
                return React.createElement("div", { key: "pill-" + index },
                    condition.Attribute.AttributeTitle,
                    " ",
                    condition.Operator.OperatorTitle,
                    " ",
                    this.getValue(condition.Value),
                    " ",
                    value2);
            }
        })));
    }
}
exports.AdvancedSearchPills = AdvancedSearchPills;
//# sourceMappingURL=AdvancedSearchPills.js.map