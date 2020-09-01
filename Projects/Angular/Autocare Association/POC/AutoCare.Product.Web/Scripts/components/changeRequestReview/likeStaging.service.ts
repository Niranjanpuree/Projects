import { Injectable }         from "@angular/core";
import { ConstantsWarehouse } from "../constants-warehouse";
import { HttpHelper }         from "../httpHelper";
import {ILikeStaging} from "../changeRequestReview/likeStaging.model";


@Injectable()
export class LikeStagingService {
    constructor(private httpHelper: HttpHelper) { }

    submitLike(id, data) {
        return this.httpHelper.post(ConstantsWarehouse.api.likeStaging + '/' + id, data);
    }

    getLikeDetails(changeRequestId) {
        return this.httpHelper.get(ConstantsWarehouse.api.likeStaging + '/' + changeRequestId);
    }
    getAllLikedBy(changeRequestId: number) {
        return this.httpHelper.get<ILikeStaging[]>(ConstantsWarehouse.api.likeStaging + "/allLikedBy/" + changeRequestId);
    }
}