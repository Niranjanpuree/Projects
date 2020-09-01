import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";

export interface IEngineDesignation {
    engineDesignationId?: number;
    engineDesignationName?: string;
    lastUpdateDate?: Date;
    engineConfigCount?: number;
    vehicleToEngineConfigCount?: number;
    comment?: string;
    isSelected?: boolean;
    changeRequestId?: number;
    attachments?: any[];
}

export interface IEngineDesignationChangeRequestReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IEngineDesignation;
    entityCurrent?: IEngineDesignation;
    requestorComments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}