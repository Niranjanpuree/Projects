import { IVehicle } from "../vehicle/vehicle.model";
import { IWheelBase } from "../wheelBase/wheelBase.model";

export interface IVehicleToWheelBase {
    id?: number;
    numberOfWheelBaseAssociation?: number;
    changeRequestId?: number;
    isSelected?: boolean;
    vehicleId?: number;
    wheelBaseId?: number;
    wheelBaseName?: string;
    vehicle?: IVehicle;
    wheelBase?: IWheelBase;
    comment?: string;
    attachments?: any[]; 
  }

import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IVehicleToWheelBaseChangeRequestStagingReview extends IReview  {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IVehicleToWheelBase;
    entityCurrent?: IVehicleToWheelBase;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}
