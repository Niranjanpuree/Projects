import * as React from "react";
import * as ReactDOM from "react-dom";
export class AdvancedSearchHeader extends React.Component {
    render() {
        return (<div className="row form-group">
            <div className="col-sm-3">
                <label className="font-weight-bold mb-0" htmlFor="conditionAttribute">Attribute</label>
            </div>
            <div className="col-sm-3">
                <label className="font-weight-bold mb-0" htmlFor="Operator">Operator</label>
            </div>
            <div className="col-sm-3">
                <label className="font-weight-bold mb-0" htmlFor="Value">Value</label>
            </div>

        </div>);

    }
}