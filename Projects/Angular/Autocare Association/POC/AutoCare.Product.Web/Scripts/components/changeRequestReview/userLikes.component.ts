import { Component, Input, ViewChild }                  from "@angular/core";
import { ILikeStaging }                          from "./likeStaging.model";
import { LikeStagingService }                                         from "./likeStaging.service";
import { ModalComponent }                           from "ng2-bs3-modal/ng2-bs3-modal";

@Component({
    selector: 'user-likes-comp',
    templateUrl: 'app/templates/changeRequestReview/userLikes.component.html',
    providers: [LikeStagingService],
})

export class UserLikesComponent {
    private likes: Array<ILikeStaging> = [];
    private errorMessage: String = "";

    @ViewChild('likedByPopupModel')
    likedByPopupModel: ModalComponent;

    constructor(private likeStagingService: LikeStagingService) {
    }

    showAllLikedBy(id: number) {
        this.likeStagingService.getAllLikedBy(id).subscribe(result => {
            this.likes = result;
            this.likedByPopupModel.open("md");
        });
    }
}