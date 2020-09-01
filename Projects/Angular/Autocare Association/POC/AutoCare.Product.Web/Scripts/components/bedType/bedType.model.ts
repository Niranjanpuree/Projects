import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";

export interface IBedType {
    id?: number;
    name?: string;
    lastUpdateDate?: Date;
    bedConfigCount?: number;
    vehicleToBedConfigCount?: number;
    comment?: string;
    isSelected?: boolean;
    changeRequestId?: number;
    attachments?: any[];
}

export interface IBedTypeChangeRequestReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IBedType;
    entityCurrent?: IBedType;
    requestorComments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}