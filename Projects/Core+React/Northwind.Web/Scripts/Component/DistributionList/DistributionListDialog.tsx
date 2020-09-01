import * as React from "react";
import * as ReactDOM from "react-dom";
import { DistributionList } from "./DistributionList"
import { Dialog, DialogActionsBar } from "@progress/kendo-react-dialogs"
import { Button } from "@progress/kendo-react-buttons";
import { Remote } from "../../Common/Remote/Remote";
import SpinnerPage from "../FileUpload/SpinnerPage";
declare var window: any;
declare var document: any;

interface IDistributionListDialogProps {
    parentDomId: string,
    notificationTemplateKey: string,
    resourceId: any,
    showCreateDistributionLink: boolean,
    showAdditionalNote: boolean,
    showOnNotifySkipButton: boolean,
    showAddNewButton: boolean,
    showAdditionalIndividualRecipient: boolean,
    notifyCallBack?: Function,
    skipCallBack?: Function,
    showNotifyInstructionText?: boolean,
    showSuccessMessageAndNotifyInstructionText?: boolean,
}

interface IDistributionListDialogStates {
    showDialog: boolean,
    showLoading: boolean,
}
export class DistributionListDialog extends React.Component<IDistributionListDialogProps, IDistributionListDialogStates> {
    distributionList: DistributionList;
    dialog: Dialog;

    constructor(props: any) {
        super(props);
        this.escFunction = this.escFunction.bind(this);
        this.state = {
            showDialog: true,
            showLoading: false,
        }
        $('html').addClass('htmlClass');
    }

    async onNotify(e: any) {
        this.setState({
            showLoading: true
        })
        let sender = this;
        let distributionSelection = this.distributionList.grid.getGridDataToPost();
        let individualRecipients = this.distributionList.multiSelectDropDown.state.value;


        let additionalNotes = this.distributionList.state.additionalNote;
        let notificationTemplateKey = this.distributionList.props.notificationTemplateKey;
        let resourceId = this.props.resourceId;


        if (distributionSelection.selectedAll === false && distributionSelection.includeList.length === 0 && individualRecipients.length === 0) {
            window.Dialog.alert("Please select any distribution list / add individual recipient for notification..");

            this.setState({
                showLoading: false
            })
            return;
        }
        let notificationViewModel = {
            DistributionSelection: distributionSelection,
            AdditionalNotes: additionalNotes,
            NotificationTemplateKey: notificationTemplateKey,
            ResourceId: resourceId,
            IndividualRecipients: individualRecipients
        }

        let response = await Remote.postDataAsync('/Notification/Add', notificationViewModel);

        if (response.ok) {
            window.Dialog.alert("Notified to all user(s)");
            sender.onCancel(e);
        } else {
            let message = await Remote.parseErrorMessage(response);
            window.Dialog.alert(message);
        }
        this.setState({
            showLoading: false
        })
    }

    onCancel(e: any) {
        this.setState({
            showDialog: false
        });
        $('html').removeClass('htmlClass');
    }


    escFunction(event: any) {
        if (event.keyCode === 27 && this.distributionList.state.showDialog === false) {
            this.onCancel('');
        }
    }

    componentDidMount() {
        document.addEventListener("keydown", this.escFunction, false);
    }

    componentWillUnmount() {
        document.removeEventListener("keydown", this.escFunction, false);
    }

    render() {
        return (
            this.state.showDialog && <Dialog ref={(c) => this.dialog = c} title="Notify" width="70%" className="dialog-custom">
                <div>
                    {this.state.showLoading && <SpinnerPage />}
                    <DistributionList
                        ref={(c) => { this.distributionList = c }}
                        notificationTemplateKey={this.props.notificationTemplateKey}
                        resourceId={this.props.resourceId}
                        showCreateDistributionLink={this.props.showCreateDistributionLink}
                        showAdditionalNote={this.props.showAdditionalNote}
                        showOnNotifySkipButton={this.props.showOnNotifySkipButton}
                        showAddNewButton={this.props.showAddNewButton}
                        showAdditionalIndividualRecipient={this.props.showAdditionalIndividualRecipient}
                        notifyCallBack={this.props.notifyCallBack}
                        skipCallBack={this.props.skipCallBack}
                        showNotifyInstructionText={this.props.showNotifyInstructionText}
                        showSuccessMessageAndNotifyInstructionText={this.props.showSuccessMessageAndNotifyInstructionText}
                        isRenderFromParentDialog={true}
                        parentDomId={this.props.parentDomId} />
                </div>
                <DialogActionsBar>
                    <Button primary={true} onClick={() => this.onNotify(this)} type="button">Notify</Button>
                    <Button onClick={() => this.onCancel(this)} type="button">Skip</Button>
                </DialogActionsBar>
            </Dialog >
        );
    }
}