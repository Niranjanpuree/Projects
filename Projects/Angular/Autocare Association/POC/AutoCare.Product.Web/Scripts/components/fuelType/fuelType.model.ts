import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";

export interface IFuelType {
    id?: number;
    name?: string;
    lastUpdateDate?: Date;
    engineConfigCount?: number;
    vehicleToEngineConfigCount?: number;
    comment?: string;
    isSelected?: boolean;
    changeRequestId?: number;
    attachments?: any[];
}

export interface IFuelTypeChangeRequestReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IFuelType;
    entityCurrent?: IFuelType;
    requestorComments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}