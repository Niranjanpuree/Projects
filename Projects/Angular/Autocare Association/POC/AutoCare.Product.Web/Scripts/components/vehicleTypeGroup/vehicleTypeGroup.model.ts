import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IVehicleTypeGroup {
    id?: number,
    name?: string;
    lastUpdateDate?: Date;
    vehicleTypeCount?: number;
    comment?: string;
    isSelected?: boolean;
    changeType?: string;
    changeRequestId?: number;
    attachments?: any[]; 
}


export interface IVehicleTypeGroupChangeRequestStagingReview extends IReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IVehicleTypeGroup;
    entityCurrent?: IVehicleTypeGroup;
    //requestorComments?: Array<ICommentsStaging>;
    //reviewerComments?: Array<ICommentsStaging>;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}