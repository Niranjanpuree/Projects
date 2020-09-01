import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";

export interface IEngineVin {
    engineVinId?: number;
    engineVinName?: string;
    lastUpdateDate?: Date;
    engineConfigCount?: number;
    vehicleToEngineConfigCount?: number;
    comment?: string;
    isSelected?: boolean;
    changeRequestId?: number;
    attachments?: any[];
}

export interface IEngineVinChangeRequestReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IEngineVin;
    entityCurrent?: IEngineVin;
    requestorComments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}