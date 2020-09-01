import { IVehicle } from "../vehicle/vehicle.model";
import { IBedConfig } from "../bedConfig/bedConfig.model";

export interface IVehicleToBedConfig {
    id?: number;
    numberOfBedAssociation?: number;
    changeRequestId?: number;
    isSelected?: boolean;
    vehicleId?: number;
    bedConfigId?: number;
    bedConfig?: IBedConfig;
    vehicle?: IVehicle;
    comment?: string;
    attachments?: any[]; 
    bedLengthId ?: number;
    length ?: string;
    bedLengthMetric ?: number;
    bedTypeId ?: number;
    bedTypeName ?: string;
}

import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IVehicleToBedConfigChangeRequestStagingReview extends IReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IVehicleToBedConfig;
    entityCurrent?: IVehicleToBedConfig;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}
