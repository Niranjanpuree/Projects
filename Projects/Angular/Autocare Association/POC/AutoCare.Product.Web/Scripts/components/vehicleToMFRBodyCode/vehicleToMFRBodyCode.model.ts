import { IVehicle } from "../vehicle/vehicle.model";
import {IMfrBodyCode} from "../mfrBodyCode/mfrBodyCode.model";

export interface IVehicleToMfrBodyCode {
    id?: number;
    numberOfMfrBodyCodesAssociation?: number;
    changeRequestId?: number;
    vehicleId?: number;
    mfrBodyCode?:IMfrBodyCode;
    vehicle?: IVehicle;
    comment?: string;
    attachments?: any[]; 
    mfrBodyCodeId?: number;
    mfrBodyCodeName?: string;
    isSelected?: boolean;
   
}
import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IVehicleToMfrBodyCodeChangeRequestStagingReview extends IReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IVehicleToMfrBodyCode;
    entityCurrent?: IVehicleToMfrBodyCode;
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}
