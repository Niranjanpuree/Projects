import {IVehicle} from '../vehicle/vehicle.model';
import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IBaseVehicle {
    id?: number;
    makeId?: number;
    makeName?: string;
    modelId?: number;
    modelName?: string;
    yearId?: number;
    comment?: string;
    vehicleCount?: number;
    vehicles?: IVehicle[];
    isSelected?: boolean;
    changeRequestId?: number;
    attachments?: any[];
}


export interface IBaseVehicleChangeRequestStagingReview extends IReview  {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IBaseVehicle;
    entityCurrent?: IBaseVehicle;
    replacementVehicles?: IVehicle[];
    //requestorComments?: Array<ICommentsStaging>;
    //reviewerComments?: Array<ICommentsStaging>;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}