import {IVehicleToDriveType} from "../vehicleToDriveType/vehicleToDriveType.model";

export interface IDriveType {
    id?: number;
    name?: string;
    isSelected?: boolean;
    vehicleToDriveTypeCount?:number;
    comment?: string;
    changeRequestId?: number;
    vehicleToDriveTypes?: IVehicleToDriveType[];
    attachments?: any[]; 
}

import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IDriveTypeChangeRequestReview extends IReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IDriveType;
    entityCurrent?: IDriveType;
    replacementVehicleToDriveTypes?: IVehicleToDriveType[];
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}