
export interface IRegion {
    id?: number;
    name?: string;
    parentId?: number;
    regionAbbr?: string;
    regionAbbr_2?: string;
    parent?: IParentRegion;
    vehicleCount?: number;
    comment?: string;
    changeType?: string;
    changeRequestId?: number;
    attachments?: any[];
    isSelected?: boolean;
}

export interface IParentRegion {
    id?: number;
    name?: string;
    regionAbbr?: string;
    regionAbbr_2?: string;
}

export interface IRegionViewModel extends IRegion {
    id?: number;
    name?: string;
    lastUpdateDate?: string;
    changeRequestExists?: boolean;
    baseVehicleCount?: number;
    vehicleCount?: number;
}

import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IRegionChangeRequestStagingReview extends IReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IRegion;
    entityCurrent?: IRegion;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}
