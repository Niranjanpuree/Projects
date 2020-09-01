import { Injectable } from                                  "@angular/core";
import { ConstantsWarehouse } from                          "../constants-warehouse";
import { URLSearchParams } from                             "@angular/http"
import { HttpHelper } from                                  "../httpHelper";
import { Http, Response, Headers, RequestOptions } from     "@angular/http";
import { IChangeRequest }                                                       from "./change.model";
//import { IChangeRequestSearchInputModel, IChangeRequestSearchViewModel }         from "./change-search.model";
import { IChangeRequestReview, IAssignReviewer}    from "../changeRequestReview/changeRequestReview.model";
import { IUser } from "./user.model"

@Injectable()
export class ChangeService {

    constructor(private _httpHelper: HttpHelper) {
    }

    selectedCRApproval(baseUrl: string, id: Number, data: IChangeRequestReview) {
        return this._httpHelper.post(baseUrl + "/changeRequestStaging/" + id, data);
    }

    getAssociatedCount(selectedCR: IChangeRequest[]) {
        return this._httpHelper.post(ConstantsWarehouse.api.changeRequestSearch + "/getAssociatedCount/", selectedCR);
    }

    assignReviewer(assignedReviewer: IAssignReviewer) {
        return this._httpHelper.post(ConstantsWarehouse.api.changeRequestSearch +"/assignReviewer", assignedReviewer);
    }
}