
export interface IYear {
    id?: number;
    baseVehicleCount?: number;
    comment?: string;
    changetype?: string;
    changeRequestId?: number;
    attachments?: any[]; 
}

export interface IYearViewModel extends IYear {
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

export interface IYearChangeRequestStagingReview extends IReview  {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IYear;
    entityCurrent?: IYear;
    //requestorComments?: Array<ICommentsStaging>;
    //reviewerComments?: Array<ICommentsStaging>;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}