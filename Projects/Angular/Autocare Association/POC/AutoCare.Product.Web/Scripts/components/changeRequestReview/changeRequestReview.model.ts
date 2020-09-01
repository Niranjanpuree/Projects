import { ICommentsStaging } from "./commentsStaging.model";

export interface IChangeRequestReview {
    changeRequestId?: Number;
    reviewedBy?: String;
    reviewStatus?: ChangeRequestStatus;
    reviewComment?: ICommentsStaging;
    attachments?: any[]; 
}

export enum ChangeRequestStatus {
    Submitted = 0,
    Deleted = 1,
    PreliminaryApproved = 2,
    Approved = 3,
    Rejected = 4
}

export interface IChangeRequestBulkReview extends IChangeRequestReview {
    entity?: string;
}

export interface IAssignReviewer {
    changeRequestIds?: Number[],
    assignedToUserId?: string,
    assignedToRoleId?: string,
}

export interface IReview {
    canAttach: boolean;
    canDelete: boolean;
    canLike  : boolean;
    canAssign: boolean;
    canReview: boolean;
    canFinal : boolean;
    canSubmit: boolean;
    isCompleted: boolean;
}