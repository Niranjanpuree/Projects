import { IChangeStatus } from './change-search.model';
import { IUser }         from "./user.model";

export interface IChangeRequest {
    id?: number;
    changeRequestTypeId?: number;
    entity?: string;
    entityId?: string;
    changeType?: string;
    taskControllerId?: number;
    requestedBy?: IUser;
    createdDateTime?: string;
    updatedDateTime?: string;
    status?: IChangeStatus;
    statusText?: string;
    likes?: number;
    assignee?: IUser;
    isSelected: boolean;
    commentExists?: boolean;
    changeContent?: string;
}

//export interface IAssociatedCount {
//    baseVehicleCount?: number;
//    vehicleCount?: number;
//}

export interface IAffectedList {
    type?: string;
    count?: number;
}
