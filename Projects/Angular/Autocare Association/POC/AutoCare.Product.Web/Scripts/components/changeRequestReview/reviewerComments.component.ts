import { Component, Input }                  from "@angular/core";
import { ICommentsStaging }                          from "./commentsStaging.model";


@Component({
    selector: 'reviewer-comments-comp',
    templateUrl: 'app/templates/changeRequestReview/reviewerComments.component.html',
})

export class ReviewerCommentsComponent {
    @Input("comments")
    comments: Array<ICommentsStaging>;
    errorMessage: String = "";

    constructor() {
    }
}
