import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";
import {IVehicleToWheelBase} from "../vehicleToWheelBase/vehicleToWheelBase.model";

export interface IWheelBase {
    id?: number;
    base?: string;
    wheelBaseMetric?: string;
    lastUpdateDate?: Date;
    vehicleToWheelBaseCount?: number;
    comment?: string;
    isSelected?: boolean;
    changeRequestId?: number;
    attachments?: any[];
    vehicleToWheelBases?: IVehicleToWheelBase[];
}

export interface IWheelBaseChangeRequestStagingReview extends IReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IWheelBase;
    entityCurrent?: IWheelBase;
    replacementVehicleToWheelBases?: IVehicleToWheelBase[];
    requestorComments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}