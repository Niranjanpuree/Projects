export interface IBrakeSystem {
    id?: number;
    name?: string;
    lastUpdateDate?: Date;
    brakeConfigCount?: number;
    vehicleToBrakeConfigCount?: number;
    comment?: string;
    isSelected?: boolean;
    changeRequestId?: number;
    attachments?: any[];
}

import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IBrakeSystemChangeRequestReview extends IReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IBrakeSystem;
    entityCurrent?: IBrakeSystem;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}
