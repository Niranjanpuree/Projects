import * as React from "react"
import * as ReactDom from "react-dom"
import { Input, Switch } from "@progress/kendo-react-inputs";
import { KendoDialog } from "../../Common/Dialog/Dialog";
import { Dialog, DialogActionsBar } from "@progress/kendo-react-dialogs";
import { SplitButton, Button } from "@progress/kendo-react-buttons";
import { Remote } from "../../Common/Remote/Remote";
import { EmployeeDirectoryDetail } from "./EmployeeDirectoryDetail";
import { guid } from "@progress/kendo-react-common";
import { EmployeeDirectory } from "./EmployeeDirectory";


declare var $: any;
interface IEmployeeDirectoryBlockProps {
    properties: any;
}
interface IEmployeeDirectoryBlockState {

    showDetails: boolean;
    userGuid: any;
}
export class EmployeeDirectoryBlock extends React.Component<IEmployeeDirectoryBlockProps, IEmployeeDirectoryBlockState>{
    constructor(props: IEmployeeDirectoryBlockProps) {
        super(props);
        this.state = {
            showDetails: false,
            userGuid: ''
        };
        this.OnClickDetail = this.OnClickDetail.bind(this);
        this.renderDialog = this.renderDialog.bind(this);
        this.onDialogClose = this.onDialogClose.bind(this);
    }
    OnClickDetail(val: any) {
        this.setState({ showDetails: true });
        this.setState({ userGuid: val });
    }
    onDialogClose(e: any) {
        this.setState({ showDetails: false }, this.forceUpdate);
    }
    renderDialog() {
        if (this.state.showDetails == true) {
            return (<Dialog onClose={this.onDialogClose} width="80%" height="70%">
                <EmployeeDirectoryDetail userGuid={this.state.userGuid} />
                <DialogActionsBar>
                <Button className="pull-right" onClick={this.onDialogClose} >Close</Button>
                </DialogActionsBar>
                
            </Dialog>)
        }
    }
    render() {
        return (
            <div className="col-sm-6 col-md-4 col-lg-4 col-xl-3">
                {this.renderDialog()}
                <div className="item-block" onClick={() => this.OnClickDetail(this.props.properties.userGuid)}>
                    <div className="employee-item-detail text-center">
                        <div className="employee-img">
                            <img src={'/img/nwg-1600px-w.png'} />
                        </div>
                        <h4 className="employee-name mb-0 text-truncate">{this.props.properties.displayname}</h4>
                        <small className="employee-position">{this.props.properties.jobTitle}</small>
                        <div className="employee-org mt-3 font-weight-bold text-truncate">{this.props.properties.company}</div>
                        <div className="employee-department text-truncate"><label
                            className="font-weight-bold text-muted mr-1">Department:</label>{this.props.properties.department}</div>
                    </div>
                    <div className="employee-contact border-top">
                        <div className="d-flex align-items-center mb-1 employee-org-mail">
                            <i className="material-icons">
                                mail_outline
                                </i>{this.props.properties.workEmail}</div>
                        <div className="row employee-org-phone">
                            <div className="col-md-auto d-flex align-items-center">
                                <i className="material-icons">
                                    phone
                                </i>{this.props.properties.workPhone}
                            </div>
                            <div className="col-md-auto d-flex align-items-center">
                                <i className="material-icons text-right">
                                    stay_primary_portrait
                                </i>{this.props.properties.mobilePhone}
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        );
    }
}