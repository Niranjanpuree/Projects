export interface IBrakeType {
    id?: number;
    name?: string;
    lastUpdateDate?: Date;
    frontBrakeConfigCount?: number; 
    rearBrakeConfigCount?: number; 
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

export interface IBrakeTypeChangeRequestReview extends IReview  {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IBrakeType ;
    entityCurrent?: IBrakeType;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}
