import {IVehicleToBodyStyleConfig} from "../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig.model";

export interface IBodyStyleConfig {
    id?: number;
    bodyNumDoorsId?: number;
    numDoors?: string;
    bodyTypeId?: number;
    bodyTypeName?: string;
    isSelected?: boolean;
    vehicleToBodyStyleConfigCount?:number;
    comment?: string;
    changeRequestId?: number;
    vehicleToBodyStyleConfigs?: IVehicleToBodyStyleConfig[];
    attachments?: any[]; 
}

import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IBodyStyleConfigChangeRequestReview extends IReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IBodyStyleConfig;
    entityCurrent?: IBodyStyleConfig;
    replacementVehicleToBrakeConfigs?: IVehicleToBodyStyleConfig[];
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}