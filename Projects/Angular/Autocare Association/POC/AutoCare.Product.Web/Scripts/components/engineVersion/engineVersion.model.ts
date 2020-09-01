import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";

export interface IEngineVersion {
    engineVersionId?: number;
    engineVersionName?: string;
    lastUpdateDate?: Date;
    engineConfigCount?: number;
    vehicleToEngineConfigCount?: number;
    comment?: string;
    isSelected?: boolean;
    changeRequestId?: number;
    attachments?: any[];
}

export interface IEngineVersionChangeRequestReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IEngineVersion;
    entityCurrent?: IEngineVersion;
    requestorComments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}