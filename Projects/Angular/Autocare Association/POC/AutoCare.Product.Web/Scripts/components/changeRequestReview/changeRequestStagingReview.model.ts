export interface IChangeRequestStagingReview {
    changeRequestId: Number;
    changeRequestItemId: Number;
    entityName: string;
    entityId: string;
    changeType: string;
    status: string;
    submittedBy: string;
    createdDateTime: Date;
}
