import { IVehicle } from "../vehicle/vehicle.model";
import { IBodyStyleConfig } from "../bodyStyleConfig/bodyStyleConfig.model";

export interface IVehicleToBodyStyleConfig {
    id?: number;
    numberOfBodyAssociation?: number;
    changeRequestId?: number;
    isSelected?: boolean;
    vehicleId?: number;
    bodyStyleConfigId?: number;
    bodyStyleConfig?: IBodyStyleConfig;
    vehicle?: IVehicle;
    comment?: string;
    attachments?: any[];
    bodyNumDoorsId?: number;
    bodyNumDoors?: string;
    numDoors?: string;
    bodyTypeId?: number;
    bodyTypeName?: string;
}

import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IVehicleToBodyStyleConfigChangeRequestStagingReview extends IReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IVehicleToBodyStyleConfig;
    entityCurrent?: IVehicleToBodyStyleConfig;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}
