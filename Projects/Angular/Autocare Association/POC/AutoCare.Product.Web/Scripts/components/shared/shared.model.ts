export interface IMainHeader {
    selectedMainHeaderMenuItem?: string;
}
// identity model
export interface IIdentityInfo {
    customerId: string;
    email: string;
    isAdmin: boolean;
    isResearcher: boolean;
    isRequestor: boolean;
}

// token model
export interface ITokenModel {
    customer_id?: string;
    unique_name?: string;
    email?: string;
    role?: Role;
    iss?: string;
    aud?: string;
    exp?: number;
    nbf?: number;
}

// role model
export enum Role {
    admin,
    researcher,
    user
}

// models used on search panels
export interface IFacet {
    id?: string;
    name?: string;
    isSelected?: boolean;
    //Count is only applicable to Change Search to count the facets for change Entity
    count?: number;
}

export enum SearchType {
    None,
    GeneralSearch,
    SearchByConfigId,
}