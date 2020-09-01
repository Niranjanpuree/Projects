
export interface ISubModel {
    id?: number;
    name?: string;
    lastUpdateDate?: Date;
    isSelected?: boolean;
    vehicleCount?: number;
    comment?: string;
    changeRequestId?: number;
    attachments?: any[]; 
}

import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface ISubModelChangeRequestStagingReview extends IReview  {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: ISubModel;
    entityCurrent?: ISubModel;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}