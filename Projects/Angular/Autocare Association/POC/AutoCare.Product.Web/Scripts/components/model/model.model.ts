import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IModel {
    id?: number;
    name?: string;
    vehicleTypeId?: number;
    vehicleTypeName?: string;
    lastUpdateDate?: Date;
    isSelected?: boolean;
    baseVehicleCount?: number;
    vehicleCount?: number;
    comment?: string;
    changeType?: string;
    changeRequestId?: number;
    attachments?: any[];
    baseVehicleId?: number; //NOTE: Required in base vehicle replace screen
}

export interface IModelChangeRequestReview extends IReview  {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IModel;
    entityCurrent?: IModel;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}