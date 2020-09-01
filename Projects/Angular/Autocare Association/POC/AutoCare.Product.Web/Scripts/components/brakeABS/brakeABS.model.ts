export interface IBrakeABS {
    id?: number;
    name?: string;
    lastUpdateDate?: Date;
    brakeConfigCount?: number;
    vehicleToBrakeConfigCount?: number;
    comment?: string;
    isSelected?: boolean;
    changeRequestId?: number;
    attachments?: any[]; 
}

import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IBrakeABSChangeRequestReview extends IReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IBrakeABS;
    entityCurrent?: IBrakeABS;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}