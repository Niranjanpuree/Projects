import { Injectable}                                    from "@angular/core";
import { HttpHelper}                                    from "../httpHelper";
import { ConstantsWarehouse }                           from "../constants-warehouse";
import { IChangeRequest }                               from "./change.model";
import { IChangeRequestSearchInputModel, IChangeEntity, IChangeStatus,
    IChangeRequestSearchViewModel }                from "./change-search.model";
import { IChangeRequestReview }                         from "../changeRequestReview/changeRequestReview.model";
import { ICommentsStaging }                             from "../changeRequestReview/commentsStaging.model";
import CommentsStagingmodel = require("../changeRequestReview/commentsStaging.model");
import { ILikeStaging }                                               from "../changeRequestReview/likeStaging.model";

@Injectable()
export class ChangeSearchService {
    bulkApprovalMessage: string[] = [];

    constructor(private _httpHelper: HttpHelper) {
    }

    search(changeRequestSearchInputModel: IChangeRequestSearchInputModel) {
        //NOTE: <IChangeRequestSearchViewModel> return type would require Observable<T> in post() in httphelper.ts
        return this._httpHelper.post(ConstantsWarehouse.api.changeRequestSearch, changeRequestSearchInputModel);
    }

    searchbyChangeRequestId(changeRequestId: string) {
        return this._httpHelper.get<IChangeRequestSearchViewModel>(ConstantsWarehouse.api.changeRequestSearch + "/changeRequest/" + changeRequestId);
    }

    getRequestorComment(changeRequestId: string, status: string) {
        return this._httpHelper.get<ICommentsStaging[]>(ConstantsWarehouse.api.changeRequestSearch + "/changeRequestId/" + changeRequestId + "/status/" + status);
    }
    getAllLikedBy(changeRequestId: number) {
        return this._httpHelper.get<ILikeStaging[]>(ConstantsWarehouse.api.likeStaging + "/allLikedBy/" + changeRequestId);
    }

    getApprovedStatus() {
        return this._httpHelper.get<IChangeStatus[]>(ConstantsWarehouse.api.changeRequestSearch + "/getStatus");
    }

    getChangeTypes() {
        return this._httpHelper.get<IChangeEntity[]>(ConstantsWarehouse.api.changeRequestSearch + "/getChangeTypes");
    }

    getChangeEntities() {
        return this._httpHelper.get<IChangeEntity[]>(ConstantsWarehouse.api.changeRequestSearch + "/getChangeEntities");
    }

    refreshFacets(changeRequestSearchInputModel: IChangeRequestSearchInputModel) {
        return this._httpHelper.post(ConstantsWarehouse.api.changeRequestSearchFacets, changeRequestSearchInputModel);
    }
}