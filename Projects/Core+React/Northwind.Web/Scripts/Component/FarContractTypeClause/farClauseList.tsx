import * as React from "react"
import * as ReactDOM from "react-dom"
import { Input, Switch } from "@progress/kendo-react-inputs";
import { KendoDialog } from "../../Common/Dialog/Dialog"
import { Dialog, DialogActionsBar } from "@progress/kendo-react-dialogs"
import { SplitButton, Button } from "@progress/kendo-react-buttons";
import { Remote } from "../../Common/Remote/Remote";

declare var window: any;
declare var $: any;

interface IFarClauseProps {
    farClauseList: any;
}

interface IFarClauseState {
    showDetails: boolean
}

export class FarClause extends React.Component<IFarClauseProps, IFarClauseState> {

    constructor(props: IFarClauseProps) {
        super(props);
        this.state = {
            showDetails: false
        }

        this.officeClicked = this.officeClicked.bind(this);
    }

    officeClicked(e: any) {
        this.setState({ showDetails: true });
    }

    render() {
        return (
            <div className="col-sm-6 col-md-4 col-lg-4 col-xl-3">
                <div className="item-block">
                    <h4> <span className="text-primary">  {this.props.farClauseList.number} </span>  </h4>
                    <div className="office-location mt-4">
                        <i className="material-icons">Number</i>
                        <label className="mb-0 text-muted font-weight-bold">Title</label>
                        <p className="mb-0">
                            {this.props.farClauseList.title || "Not Available"}
                        </p>
                    </div>
                </div>
            </div>
        );
    }

}