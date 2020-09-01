import * as React from "react";
import * as ReactDOM from "react-dom";
import { DistributionList } from "./DistributionList"
import { DistributionListDialog } from "./DistributionListDialog";
declare var window: any;
declare var document: any;

function loadDistributionList(domToRender: string,
    notificationTemplateKey: string,
    resourceId: any,
    showCreateDistributionLink: boolean,
    showAdditionalNote: boolean,
    showOnNotifySkipButton: boolean,
    showAddNewButton: boolean,
    showAdditionalIndividualRecipient: boolean,
    notifyCallBack: any,
    skipCallBack: any)
    {
    
    ReactDOM.unmountComponentAtNode(document.getElementById(domToRender));
    ReactDOM.render(<DistributionList notificationTemplateKey={notificationTemplateKey}
        resourceId={resourceId}
        showCreateDistributionLink={showCreateDistributionLink}
        showAdditionalNote={showAdditionalNote}
        showOnNotifySkipButton={showOnNotifySkipButton}
        showAddNewButton={showAddNewButton}
        showAdditionalIndividualRecipient={showAdditionalIndividualRecipient}
        notifyCallBack={notifyCallBack}
        skipCallBack={skipCallBack}
        parentDomId={domToRender} />,
        document.getElementById(domToRender));
}
window.distributionList = { pageView: { loadDistributionList: loadDistributionList } };


function loadDistributionListDialog(domToRender: string,
    notificationTemplateKey: string,
    resourceId: any,
    showCreateDistributionLink: boolean,
    showAdditionalNote: boolean,
    showOnNotifySkipButton: boolean,
    showAddNewButton: boolean,
    showAdditionalIndividualRecipient: boolean,
    notifyCallBack: any,
    skipCallBack: any,
    showSuccessMessageAndNotifyInstructionText?: boolean)
{
    ReactDOM.unmountComponentAtNode(document.getElementById(domToRender));
    ReactDOM.render(<DistributionListDialog notificationTemplateKey={notificationTemplateKey}
        resourceId={resourceId}
        showCreateDistributionLink={showCreateDistributionLink}
        showAdditionalNote={showAdditionalNote}
        showOnNotifySkipButton={showOnNotifySkipButton}
        showAddNewButton={showAddNewButton}
        showAdditionalIndividualRecipient={showAdditionalIndividualRecipient}
        notifyCallBack={notifyCallBack}
        skipCallBack={skipCallBack}
        showSuccessMessageAndNotifyInstructionText={showSuccessMessageAndNotifyInstructionText}
        parentDomId={domToRender} />,
        document.getElementById(domToRender));
}
window.loadDistributionListDialog = { pageView: { loadDistributionListDialog: loadDistributionListDialog } };