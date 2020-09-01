import {IVehicleToMfrBodyCode} from "../vehicleToMfrBodyCode/vehicleToMfrBodyCode.model";

export interface IMfrBodyCode {
    id?: number;
    name?: string;
    isSelected?: boolean;
    vehicleToMfrBodyCodeCount?:number;
    comment?: string;
    changeType?: string;
    changeRequestId?: number;
    vehicleToMfrBodyCodes?: IVehicleToMfrBodyCode[];
    attachments?: any[]; 
    lastUpdateDate?: Date;
}

import { ICommentsStaging } from "../changeRequestReview/commentsStaging.model";
import { IChangeRequestStagingReview } from "../changeRequestReview/changeRequestStagingReview.model";
import { IAttachment } from "../changeRequestReview/attachment.model";
import { IReview } from "../changeRequestReview/changeRequestReview.model";

export interface IMfrBodyCodeChangeRequestReview extends IReview {
    stagingItem: IChangeRequestStagingReview;
    entityStaging: IMfrBodyCode;
    entityCurrent?: IMfrBodyCode;
    replacementVehicleToMFRBodyCodes?: IVehicleToMfrBodyCode[];
    comments?: Array<ICommentsStaging>;
    attachments?: Array<IAttachment>;
}