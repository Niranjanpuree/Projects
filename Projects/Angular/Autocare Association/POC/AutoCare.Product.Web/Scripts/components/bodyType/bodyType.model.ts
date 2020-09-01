export interface IBodyType {
    id?: number;
    name?: string;
    lastUpdateDate?: Date;
    bodyStyleConfigCount?: number; 
    vehicleToBodyStyleConfigCount?: number; 
    comment?: string;
    isSelected?: boolean;
    changeRequestId?: number;
    attachments?: any[]; 
}

import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IBodyTypeChangeRequestReview extends IReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IBodyType ;
    entityCurrent?: IBodyType;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}
