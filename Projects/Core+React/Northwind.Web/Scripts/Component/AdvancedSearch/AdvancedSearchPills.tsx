import * as React from "react";
import * as ReactDOM from "react-dom";
import { Condition } from "../Entities/Condition"

export interface IAdvancedSearchPillsProps {
    conditions: Array<Condition>
}

export interface IAdvancedSearchPillsState {

}

export class AdvancedSearchPills extends React.Component<IAdvancedSearchPillsProps, IAdvancedSearchPillsState> {

    constructor(props: IAdvancedSearchPillsProps) {
        super(props);

        this.isDate = this.isDate.bind(this);
        this.getValue = this.getValue.bind(this);
    }

    isDate(value: any) {
        try {
            var d = new Date(value);
            if (d.getDate())
                return true;
            else
                return false;
        } catch (e) {
            return false;
        }
    }

    getValue(value: any) {
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

    render()
    {
        return (<div className="badge badge-pill badge-secondary">
            {
                this.props.conditions.map((condition, index) => {
                    if (typeof (condition.Value) === "string") {
                        let value2 = "";
                        if (condition.Value2) {
                            value2 = " And '" + condition.Value2 + "'";
                        }
                        return <div key={"pill-" + index}>{condition.Attribute.AttributeTitle} {condition.Operator.OperatorTitle} '{condition.Value}' {value2}</div>
                    }
                    else if (typeof (condition.Value) == "object" && condition.Value.length) {
                        let str = "";
                        for (let i in condition.Value) {
                            if (str != "")
                                str += ", "
                            str += condition.Value[i].name;
                        }
                        return <div key={"pill-" + index}>{condition.Attribute.AttributeTitle} {condition.Operator.OperatorTitle} '{str}'</div>
                    }
                    else if (typeof (condition.Value) == "object" && !condition.Value.length) {
                        var value2 = "";
                        if (this.getValue(condition.Value2)) {
                            value2 = " AND '" + this.getValue(condition.Value2) + "'";
                        }
                        return <div key={"pill-" + index}>{condition.Attribute.AttributeTitle} {condition.Operator.OperatorTitle} {this.getValue(condition.Value)} {value2}</div>
                    }

                })}
            
        </div>);

    }

}