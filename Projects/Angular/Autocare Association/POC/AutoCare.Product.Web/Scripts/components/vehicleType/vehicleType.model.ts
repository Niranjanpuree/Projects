import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IVehicleType {
    id?: number;
    name?: string;
    vehicleTypeGroupId?: number;
    vehicleTypeGroupName?: string;
    lastUpdateDate?: Date;
    modelCount?: number;
    baseVehicleCount?: number;
    vehicleCount?: number;
    comment?: string;
    isSelected?: boolean;
    changeType?: string;
    changeRequestId?: number;
    attachments?: any[];
}

export interface IVehicleTypeChangeRequestStagingReview extends IReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IVehicleType;
    entityCurrent?: IVehicleType;
    //requestorComments?: Array<ICommentsStaging>;
    //reviewerComments?: Array<ICommentsStaging>;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}