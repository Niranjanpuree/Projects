import * as React from "react"
import * as ReactDOM from "react-dom"
import { Input, Switch } from "@progress/kendo-react-inputs";
import { KendoDialog } from "../../Common/Dialog/Dialog"
import { Dialog, DialogActionsBar } from "@progress/kendo-react-dialogs"
import { SplitButton, Button } from "@progress/kendo-react-buttons";
import { Remote } from "../../Common/Remote/Remote";

declare var window: any;
declare var $: any;

interface IOfficeBlockProps {
    properties: any;
}

interface IOfficeBlockState {
    showDetails: boolean
}

export class OfficeDirectoryBlock extends React.Component<IOfficeBlockProps, IOfficeBlockState> {

    constructor(props: IOfficeBlockProps) {
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
                    <h4> <span className="text-primary">  {this.props.properties.officeCode} </span> <small>{this.props.properties.officeName}</small> </h4>
                    <div className="office-location mt-4">
                        <i className="material-icons">place</i>
                        <label className="mb-0 text-muted font-weight-bold">Physical Address</label>
                        <p className="mb-0">
                            {this.props.properties.physicalAddress || "Not Available"}
                        </p>
                    </div>
                    <div className="office-location mt-4">
                        <i className="material-icons">place</i>
                        <label className="mb-0 text-muted font-weight-bold">Mailing Address</label>
                        <p className="mb-0">
                            {this.props.properties.mailingAddress || "Not Available"}
                        </p>
                    </div>
                    <div className="row office-contact mt-4">
                        <div className="col-md-6 d-flex align-items-center">
                            <i className="material-icons">phone</i>
                            <span>{this.props.properties.phone || "Not Available"}</span>
                        </div>
                        <div className="col-md-6 d-flex align-items-center">
                            <i className="material-icons">print</i>
                            <span>{this.props.properties.fax || "Not Available"}</span>
                        </div>
                    </div>
                </div>
            </div>

        );
    }

}