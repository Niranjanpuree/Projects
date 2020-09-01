import * as React from "react";
import * as ReactDOM from "react-dom";
import { Condition } from "../../Component/Entities/Condition"

export interface ISearchPillsProps {
    conditions: Array<Condition>,
    onPillDelete?: any;
    onClearAll?: any;
}

export interface ISearchPillsState {

}

export class SearchPills extends React.Component<ISearchPillsProps, ISearchPillsState> {
    constructor(props: ISearchPillsProps) {
        super(props);

        this.isDate = this.isDate.bind(this);
        this.getValue = this.getValue.bind(this);
        this.onPillDelete = this.onPillDelete.bind(this);
        this.onClearAllPills = this.onClearAllPills.bind(this);
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

    onPillDelete(e: any) {
        if (e.target && e.currentTarget.getAttribute("itemid")) {
            if (this.props.onPillDelete) {
                this.props.onPillDelete(this.props.conditions[e.currentTarget.getAttribute("itemid")])
            }            
        }
    }

    onClearAllPills(e: any) {
        if (this.props.onClearAll) {
            this.props.onClearAll();
        }
    }

    render() {

        return (<div className="search-pills-container">
            {
                this.props.conditions.map((condition, index) => {
                    if (typeof (condition.Value) === "string") {
                        let value2 = "";
                        if (condition.Value2) {
                            value2 = " And '" + condition.Value2 + "'";
                        }
                        return <div key={"pill-" + index} className="badge badge-pill badge-secondary">{condition.Attribute.AttributeTitle} {condition.Operator.OperatorTitle} '{condition.Value}' {value2} <a href="#" itemID={index.toString()} className="pill-close" onClick={this.onPillDelete}><i className="material-icons">close</i></a></div>
                    }
                    else if (typeof (condition.Value) == "object" && condition.Value.length) {
                        let str = "";
                        for (let i in condition.Value) {
                            if (str != "")
                                str += ", "
                            str += condition.Value[i].name;
                        }
                        return <div key={"pill-" + index} className="badge badge-pill badge-secondary">{condition.Attribute.AttributeTitle} {condition.Operator.OperatorTitle} '{str}' <a href="#" itemID={index.toString()} className="pill-close" onClick={this.onPillDelete} ><i className="material-icons">close</i></a></div>
                    }
                    else if (typeof (condition.Value) == "object" && !condition.Value.length) {
                        var value2 = "";
                        if (this.getValue(condition.Value2)) {
                            value2 = " AND '" + this.getValue(condition.Value2) + "'";
                        }
                        return <div key={"pill-" + index} className="badge badge-pill badge-secondary">{condition.Attribute.AttributeTitle} {condition.Operator.OperatorTitle} {this.getValue(condition.Value)} {value2} <a href="#" itemID={index.toString()} className="pill-close" onClick={this.onPillDelete} ><i className="material-icons">close</i></a></div>
                    }

                })
            }
            {
                this.props.conditions.length > 0 && <div className="badge badge-pill badge-secondary">Clear All <a href="#" className="pill-close" onClick={this.onClearAllPills} ><i className="material-icons">close</i></a></div>
            }

        </div>);

    }

}