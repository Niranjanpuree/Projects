import { IChangeRequest }           from './change.model';
import { IUser }                    from './user.model';
import { IFacet }                   from '../shared/shared.model';

export interface IChangeRequestSearchViewModel {
    facets?: IChangeRequestSearchFacets;
    filters?: IChangeRequestSearchFilters;
    result?: IChangeRequestSearchResult;
    canBulkSubmit: boolean,
    isAdmin: boolean,
    searchType: SearchType;
}

export interface IChangeRequestSearchInputModel {
    changeTypes: string[];
    changeEntities: string[];
    statuses: string[];
    requestsBy: string[];
    assignees: string[];
    submittedDateFrom?: string;
    submittedDateTo?: string;
}

//export interface IFacet {
//    id: string;
//    name: string;
//    count: string;
//    isSelected?: boolean;
//}

export interface IChangeRequestSearchFacets {
    changeTypes: IFacet[];
    changeEntities: IFacet[];
    statuses: IFacet[];
    requestsBy: IFacet[];
    assignees: IFacet[];
}

//TODO: this class needs to be removed after the current refactoring
export interface IChangeRequestSearchFilters {
    changeTypes: IChangeType[];
    changeEntities: IChangeEntity[];
    statuses: IChangeStatus[];
    requestsBy: IUser[];
    assignees: IUser[];
    submittedDateFrom?: string;
    submittedDateTo?: string;
}

export interface IChangeRequestSearchResult {
    changeRequests: IChangeRequest[];
}

export enum SearchType {
    None,
    GeneralSearch,
    SearchByChangeRequestId,
}

export interface IChangeStatus {
    id?: number;
    status?: string;
    isSelected?: boolean;
}

export interface IChangeType {
    name?: string;
    count?: number;
    isSelected?: boolean;
}

export interface IChangeEntity {
    name?: string;
    count?: number;
    isSelected?: boolean;
}