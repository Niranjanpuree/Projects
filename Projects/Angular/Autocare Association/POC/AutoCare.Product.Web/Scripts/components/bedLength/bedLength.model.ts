
export interface IBedLength {
    id?: number;
    name?: string;
    length?: string;
    bedLengthMetric?: string,
    lastUpdateDate?: Date;
    bedConfigCount?: number;
    vehicleToBedConfigCount?: number;
    comment?: string;
    isSelected?: boolean;
    changeRequestId?: number;
    attachments?: any[];
}

import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IBedLengthChangeRequestReview extends  IReview{
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IBedLength;
    entityCurrent?: IBedLength;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}
