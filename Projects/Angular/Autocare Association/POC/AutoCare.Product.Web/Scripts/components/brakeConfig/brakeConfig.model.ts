import {IVehicleToBrakeConfig} from "../vehicleToBrakeConfig/vehicleToBrakeConfig.model";

export interface IBrakeConfig {
    id?: number;
    frontBrakeTypeId?: number;
    frontBrakeTypeName?: string;
    rearBrakeTypeId?: number;
    rearBrakeTypeName?: string;
    brakeSystemId?: number;
    brakeSystemName?: string;
    brakeABSId?: number;
    brakeABSName?: string;
    isSelected?: boolean;
    vehicleToBrakeConfigCount?:number;
    comment?: string;
    changeRequestId?: number;
    vehicleToBrakeConfigs?: IVehicleToBrakeConfig[];
    attachments?: any[]; 
}

import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IBrakeConfigChangeRequestReview extends IReview  {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IBrakeConfig;
    entityCurrent?: IBrakeConfig;
    replacementVehicleToBrakeConfigs?: IVehicleToBrakeConfig[];
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}