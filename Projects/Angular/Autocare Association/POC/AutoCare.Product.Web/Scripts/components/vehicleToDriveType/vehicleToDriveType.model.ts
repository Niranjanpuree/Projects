import { IVehicle } from "../vehicle/vehicle.model";
import {IDriveType} from "../driveType/driveType.model";

export interface IVehicleToDriveType {
    id?: number;
    numberOfDriveTypeAssociation?: number;
    changeRequestId?: number;
    isSelected?: boolean;
    vehicleId?: number;
    driveType?: IDriveType;
    vehicle?: IVehicle;
    comment?: string;
    attachments?: any[];
    driveTypeId?: number;
    driveTypeName?: string;
  }

import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IVehicleToDriveTypeChangeRequestStagingReview extends IReview  {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IVehicleToDriveType;
    entityCurrent?: IVehicleToDriveType;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}
