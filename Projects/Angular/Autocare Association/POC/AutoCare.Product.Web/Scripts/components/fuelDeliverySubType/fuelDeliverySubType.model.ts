export interface IFuelDeliverySubType {
    id?: number;
    name?: string;
    lastUpdateDate?: string;
    isSelected?: boolean;
    fuelDeliveryConfigCount?: number;
    comment?: string;
    changeType?: string;
    changeRequestId?: number;
    attachments?: any[]; 
}

import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IFuelDeliverySubTypeChangeRequestStagingReview extends IReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IFuelDeliverySubType;
    entityCurrent?: IFuelDeliverySubType;
    //requestorComments?: Array<ICommentsStaging>;
    //reviewerComments?: Array<ICommentsStaging>;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}
