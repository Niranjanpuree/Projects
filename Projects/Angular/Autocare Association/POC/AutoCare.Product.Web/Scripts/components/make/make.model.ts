export interface IMake {
    id?: number;
    name?: string;
    lastUpdateDate?: string;
    isSelected?: boolean;
    baseVehicleCount?: number;
    vehicleCount?: number;
    comment?: string;
    changeType?: string;
    changeRequestId?: number;
    attachments?: any[]; 
}

import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IMakeChangeRequestStagingReview extends IReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IMake;
    entityCurrent?: IMake;
    //requestorComments?: Array<ICommentsStaging>;
    //reviewerComments?: Array<ICommentsStaging>;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}
