import {IVehicleToBedConfig} from "../vehicleToBedConfig/vehicleToBedConfig.model";

export interface IBedConfig {
    id?: number;
    bedLengthId?: number;
    length?: string;
    bedLengthMetric?: string;
    bedTypeId?: number;
    bedTypeName?: string;
    isSelected?: boolean;
    vehicleToBedConfigCount?:number;
    comment?: string;
    changeRequestId?: number;
    vehicleToBedConfigs?: IVehicleToBedConfig[];
    attachments?: any[]; 
}

import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IBedConfigChangeRequestReview extends IReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IBedConfig;
    entityCurrent?: IBedConfig;
    replacementVehicleToBedConfigs?: IVehicleToBedConfig[];
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}