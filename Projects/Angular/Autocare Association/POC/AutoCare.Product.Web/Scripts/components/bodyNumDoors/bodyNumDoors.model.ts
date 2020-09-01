export interface IBodyNumDoors {
    id?: number;
    numDoors?: string;
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

export interface IBodyNumDoorsChangeRequestReview extends IReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IBodyNumDoors ;
    entityCurrent?: IBodyNumDoors;
    comments?: Array<ICommentsStaging>;
   attachments?: Array<IAttachment>;
}
