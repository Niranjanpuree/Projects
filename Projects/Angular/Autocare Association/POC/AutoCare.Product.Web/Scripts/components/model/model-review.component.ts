﻿import { Component, OnInit, ViewChild }                               from "@angular/core";
import { ToastsManager }                                              from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { ModelService }                                               from "./model.service";
import { VehicleTypeService }                                           from "../vehicleType/vehicleType.service";
import { IModelChangeRequestReview }                                  from "./model.model";
import { ConstantsWarehouse }                                         from "../constants-warehouse";
import { Router, ActivatedRoute }                  from "@angular/router";
import { IChangeRequestReview, ChangeRequestStatus, IAssignReviewer } from "../changeRequestReview/changeRequestReview.model";
import { ICommentsStaging }                                           from "../changeRequestReview/commentsStaging.model";
import { ILikeStaging }                                               from "../changeRequestReview/likeStaging.model";
import { LikeStagingService }                                         from "../changeRequestReview/likeStaging.service";
import { AcFileUploader }                                             from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { SharedService }                                              from '../shared/shared.service';
import { NavigationService }                                          from '../shared/navigation.service';
import { IUser }                                                      from "../change/user.model";
import { UserService }                                                from "../change/user.service";
import { ChangeService  }                                             from "../change/change.service";
import { IIdentityInfo}                                               from "../shared/shared.model";
import { Observable }    from 'rxjs/Observable';
import { UserLikesComponent }                                  from "../changeRequestReview/userLikes.component";

@Component({
    selector: 'model-review-comp',
    templateUrl: 'app/templates/model/model-review.component.html',
    providers: [ModelService, VehicleTypeService, LikeStagingService, UserService, ChangeService],
})

export class ModelReviewComponent implements OnInit {
    modelChangeRequest: IModelChangeRequestReview;
    likeStaging: ILikeStaging = { changeRequestId: 0, likedBy: "Me", likeStatus: "like" };
    likeStagingGet: ILikeStaging = { changeRequestId: 0, likedBy: "", likeStatus: "", likeCount: 0 };
    isPreliminaryApproveChecked: boolean = false;
    isApproveChecked: boolean = false;
    isRejectChecked: boolean = false;
    changeRequestReview: IChangeRequestReview = { changeRequestId: 0, reviewedBy: "Me", reviewStatus: ChangeRequestStatus.Submitted };
    reviewComment: ICommentsStaging = { comment: "" };
    reviewCompleted: Boolean = false;
    isLiked: Boolean = true;
    showLoadingGif: boolean = false;
    backNavigation: string = "/dashboard";
    backNavigationText: string = "Return to Dashboard";
    usersForAssignment: IUser[] = [];
    assignedReviewer: IAssignReviewer = { assignedToUserId: "-1", changeRequestIds: [] };
    //private _identityInfo: IIdentityInfo = { customerId: "", email: "", isAdmin: false, isRequestor: false, isResearcher: false };
    showPrelimRadio: boolean = true;

    @ViewChild("newAcFileUploader")
    newAcFileUploader: AcFileUploader;

    @ViewChild("existingAcFileUploader")
    existingAcFileUploader: AcFileUploader;

    @ViewChild("likeComponent")
    likeComponent: UserLikesComponent;

    constructor(private modelService: ModelService, private vehicleTypeService: VehicleTypeService, private likeStagingService: LikeStagingService, private toastr: ToastsManager,
        private route: ActivatedRoute, private router: Router, private sharedService: SharedService, private userService: UserService,
        private changeService: ChangeService, private navgationService: NavigationService) {
        if (this.navgationService.backRoute) {
            this.backNavigation = this.navgationService.backRoute;
            if (this.backNavigation.indexOf('change') > 0)
                this.backNavigationText = "Return to Change Requests";
            else
                this.backNavigationText = "Return to Reference Data";
        }
    }

    ngOnInit() {
        this.showLoadingGif = true;
        // Load existing base vechile with reference from RouteParams
        const id = Number(this.route.snapshot.params["id"]);
        this.modelService.getChangeRequestStaging(id).subscribe(
            result => {
                this.modelChangeRequest = result;
                var vId = this.modelChangeRequest.entityStaging.vehicleTypeId;
                //this.getLikes(id);
                this.likeStagingService.getLikeDetails(id).subscribe(result => {
                    this.likeStagingGet = result;
                    if (this.likeStagingGet.likeStatus == "Like") {//} && this.likeStagingGet.likedBy == this._identityInfo.customerId) {
                        this.isLiked = true;
                    } else if (!this.likeStagingGet.likedBy) {
                        this.isLiked = false;
                    }
                    if (vId > 0) {
                        this.showLoadingGif = true;
                        this.vehicleTypeService.getVehicleTypeDetail(vId).subscribe(data => {
                            this.modelChangeRequest.entityStaging.vehicleTypeName = data.name;
                            this.showLoadingGif = false;
                        }), error => {
                            this.showLoadingGif = false;
                            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);

                        };
                    }
                    else {
                        this.showLoadingGif = false;
                    }


                }),
                    error => {
                        this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                        this.showLoadingGif = false;
                    };
                //this._identityInfo = this.sharedService.getIdentityInfo(String(this.modelChangeRequest.stagingItem.submittedBy));
                //if (this.modelChangeRequest.stagingItem.status === "Approved" || this.modelChangeRequest.stagingItem.status === "Deleted"
                //    || this.modelChangeRequest.stagingItem.status === "Rejected") {
                //    this.reviewCompleted = true;
                //}
                //else if (this.modelChangeRequest.stagingItem.status === "PreliminaryApproved") {
                //    this.showPrelimRadio = false;
                //    if (this._identityInfo.isResearcher) {
                //        this.reviewCompleted = true;
                //    }
                //}

                // check if Preliminary Approved.
                if (this.modelChangeRequest.stagingItem.status == "PreliminaryApproved") {
                    this.showPrelimRadio = false;
                }
            },
            error => {
                let toasterMessage = {
                    title: "Change request review counldn't be loaded.",
                    body: <any>error.toString()
                };
                this.toastr.success(toasterMessage.body, toasterMessage.title);
                this.showLoadingGif = false;
            }
        );
        this.usersForAssignment = this.userService.getUsersForAssignment();
    }

    onReviewerChange() {
        if (this.assignedReviewer.assignedToUserId !== "-1" && !this.isPreliminaryApproveChecked) {
            this.isApproveChecked = false;
            this.isRejectChecked = false;
        }
    }

    onChange(approved: String) {
        if (this.assignedReviewer.assignedToUserId !== "-1" && approved !== "PreliminaryApprove") {
            this.assignedReviewer.assignedToUserId = "-1";
        }
        if (approved == "PreliminaryApprove") {
            if (!this.isPreliminaryApproveChecked) {
                this.isPreliminaryApproveChecked = true;
                this.isApproveChecked = false;
                this.isRejectChecked = false;
            }
        }
        else if (approved == "Approve") {
            if (!this.isApproveChecked) {
                this.isPreliminaryApproveChecked = false;
                this.isApproveChecked = true;
                this.isRejectChecked = false;
            }
        }
        else if (approved == "Reject") {
            if (!this.isRejectChecked) {
                this.isPreliminaryApproveChecked = false;
                this.isApproveChecked = false;
                this.isRejectChecked = true;
            }
        }
    }

    onSubmitReview(deletebuttonclicked: String) {
        if (!this.validationCheck()) {
            if (this.assignedReviewer.assignedToUserId !== "-1" && deletebuttonclicked != 'delete') {
                // note: the value on this.assignedReviewer.assignedToUserId is -1 by default, but type is string. Default is -1 not "".
                // if assingedReviewer-index is changed, then the action of Submit is assign-reviewer.
                this.onAssignReviewer();
                return;
            }

            else if (!deletebuttonclicked) {
                this.toastr.warning("No changes to be submitted!!", ConstantsWarehouse.validationTitle);
                return;
            }
        }

        // fill change-request-review
        if (deletebuttonclicked == 'delete') {
            this.changeRequestReview.reviewStatus = ChangeRequestStatus.Deleted;
        } else {
            if (this.isPreliminaryApproveChecked) {
                this.changeRequestReview.reviewStatus = ChangeRequestStatus.PreliminaryApproved;
            } else if (this.isApproveChecked) {
                this.changeRequestReview.reviewStatus = ChangeRequestStatus.Approved;
            } else if (this.isRejectChecked) {
                this.changeRequestReview.reviewStatus = ChangeRequestStatus.Rejected;
            } else {
                this.changeRequestReview.reviewStatus = ChangeRequestStatus[this.modelChangeRequest.stagingItem.status];
            }
        }

        this.changeRequestReview.reviewComment = this.reviewComment;

        this.showLoadingGif = true;
        this.newAcFileUploader.uploadAttachments().subscribe(uploadedFiles => {
            if (uploadedFiles && uploadedFiles.length > 0) {
                this.changeRequestReview.attachments = uploadedFiles;
            }

            // new deleted attachments
            if (this.newAcFileUploader.getFilesMarkedToDelete()) {
                if (this.changeRequestReview.attachments) {
                    this.changeRequestReview.attachments = this.changeRequestReview.attachments.concat(this.newAcFileUploader.getFilesMarkedToDelete());
                } else {
                    this.changeRequestReview.attachments = this.newAcFileUploader.getFilesMarkedToDelete();
                }
            }

            // existing attachments
            if (this.existingAcFileUploader.getFilesMarkedToDelete()) {
                if (this.changeRequestReview.attachments) {
                    this.changeRequestReview.attachments = this.changeRequestReview.attachments.concat(this.existingAcFileUploader.getFilesMarkedToDelete());
                } else {
                    this.changeRequestReview.attachments = this.existingAcFileUploader.getFilesMarkedToDelete();
                }
            }

            this.modelService.submitChangeRequestReview(this.modelChangeRequest.stagingItem.changeRequestId, this.changeRequestReview).subscribe(
                result => {
                    if (result) {
                        let toasterMessage = null;
                        if (this.changeRequestReview.reviewStatus != ChangeRequestStatus.Deleted) {
                            toasterMessage = {
                                title: ConstantsWarehouse.reviewSubmitSuccessTitle,
                                body: `Your review for ${this.modelChangeRequest.stagingItem.changeType} ${this.modelChangeRequest.stagingItem.entityName} : ${this.modelChangeRequest.entityStaging.name} change request ID ' ${this.modelChangeRequest.stagingItem.changeRequestId}'  was submitted successfully.`
                            };
                        } else {
                            toasterMessage = {
                                title: ConstantsWarehouse.reviewDeleteSuccessTitle,
                                body: `Your delete request for ${this.modelChangeRequest.stagingItem.changeType} ${this.modelChangeRequest.stagingItem.entityName} : ${this.modelChangeRequest.entityStaging.name} change request ID ' ${this.modelChangeRequest.stagingItem.changeRequestId}' was submitted successfully.`
                            };
                        }

                        this.toastr.success(toasterMessage.body, toasterMessage.title);

                        if (this.assignedReviewer.assignedToUserId !== "-1" && deletebuttonclicked != 'delete')
                            this.onAssignReviewer();
                        else
                            this.router.navigateByUrl(this.backNavigation);

                    } else {
                        let toasterMessage = null;
                        if (this.changeRequestReview.reviewStatus != ChangeRequestStatus.Deleted) {
                            toasterMessage = {
                                title: ConstantsWarehouse.reviewSubmitErrorTitle,
                                body: `Your review for ${this.modelChangeRequest.stagingItem.changeType} ${this.modelChangeRequest.stagingItem.entityName} : ${this.modelChangeRequest.entityStaging.name} change request ID ' ${this.modelChangeRequest.stagingItem.changeRequestId}' failed.`
                            };
                        } else {
                            toasterMessage = {
                                title: ConstantsWarehouse.reviewDeleteErrorTitle,
                                body: `Your delete request for ${this.modelChangeRequest.stagingItem.changeType} ${this.modelChangeRequest.stagingItem.entityName} : ${this.modelChangeRequest.entityStaging.name} change request ID ' ${this.modelChangeRequest.stagingItem.changeRequestId}' failed.`
                            };
                        }
                        this.toastr.warning(toasterMessage.body, toasterMessage.title);
                        this.showLoadingGif = false;
                    }
                },
                error => {
                    let toasterMessage = null;
                    if (this.changeRequestReview.reviewStatus != ChangeRequestStatus.Deleted) {
                        toasterMessage = {
                            title: ConstantsWarehouse.reviewSubmitErrorTitle,
                            body: `Your review for ${this.modelChangeRequest.stagingItem.changeType} ${this.modelChangeRequest.stagingItem.entityName} : ${this.modelChangeRequest.entityStaging.name} change request ID ' ${this.modelChangeRequest.stagingItem.changeRequestId}' failed.\n${<any>error.toString()}.`
                        };
                    } else {
                        toasterMessage = {
                            title: ConstantsWarehouse.reviewSubmitErrorTitle,
                            body: `Your delete request for ${this.modelChangeRequest.stagingItem.changeType} ${this.modelChangeRequest.stagingItem.entityName} : ${this.modelChangeRequest.entityStaging.name} failed.\n${<any>error.toString()}.`
                        };
                    }
                    this.toastr.warning(toasterMessage.body, toasterMessage.title);

                    this.showLoadingGif = false;
                }, () => {
                    this.newAcFileUploader.reset();
                    //this.showLoadingGif = false;
                });
        }, error => {
            this.newAcFileUploader.reset();
            let toasterMessage = {
                title: "Attachment couldn't be uploaded.",
                body: <any>error.toString()
            };
            this.toastr.warning(toasterMessage.body, toasterMessage.title);

            this.showLoadingGif = false;
        });
    }

    private onAssignReviewer() {
        var user = this.usersForAssignment.filter(x => x.id === this.assignedReviewer.assignedToUserId)[0];
        this.assignedReviewer.assignedToRoleId = user.roleId;
        this.assignedReviewer.changeRequestIds[0] = this.modelChangeRequest.stagingItem.changeRequestId;
        this.changeService.assignReviewer(this.assignedReviewer).subscribe(result => {
            if (result) {
                let toasterMessage = {
                    title: `Your review was assigned to ${user.user}`,
                    body: `Your review for ${this.modelChangeRequest.stagingItem.changeType} ${this.modelChangeRequest.stagingItem.entityName} : ${this.modelChangeRequest.entityStaging.name} assigned successfully.`
                }
                this.toastr.success(toasterMessage.body, toasterMessage.title);

                // redirect to search result
                this.router.navigateByUrl(this.backNavigation);
            } else {
                let toasterMessage = {
                    title: "Your review was couldn't be assigned.",
                    body: `Your assignment for ${this.modelChangeRequest.stagingItem.changeType} ${this.modelChangeRequest.stagingItem.entityName} : ${this.modelChangeRequest.entityStaging.name} failed.`
                }
                this.toastr.warning(toasterMessage.body, toasterMessage.title);
            }
        },
            error => {
                let toasterMessage = {
                    title: "Your review was couldn't be assigned.",
                    body: `Your assignment for ${this.modelChangeRequest.stagingItem.changeType} ${this.modelChangeRequest.stagingItem.entityName} : ${this.modelChangeRequest.entityStaging.name} failed.\n${<any>error.toString()}.`
                }
                this.toastr.warning(toasterMessage.body, toasterMessage.title);
            });
    }

    onCancel() {
        // redirect to search result
        this.router.navigateByUrl(this.backNavigation);
    }

    onLikeClick(status: string) {
        const id = Number(this.route.snapshot.params["id"]);
        this.likeStaging.changeRequestId = id;
        this.likeStaging.likeStatus = status;
        //this.likeStaging.likedBy = this._identityInfo.customerId;

        this.isLiked = true;

        this.likeStagingService.submitLike(this.likeStaging.changeRequestId, this.likeStaging).subscribe(
            result => {
                if (result) {
                    this.toastr.success("Your like was submitted successfully.", "Your like was submitted.");
                } else {
                    this.toastr.warning(`Your like for ${this.likeStaging.changeRequestId} failed.`, "Your like couldn't be submitted.");
                }
                this.getLikes(id);

                this.showLoadingGif = false;
            },
            error => {
                let errorMessage: string = JSON.parse(String(error)).message;
                this.toastr.warning(`You like for ${this.likeStaging.changeRequestId} failed.\n${errorMessage}.error`, "Your like couldn't be submitted.");
                this.showLoadingGif = false;
            }
        );
    }

    getLikes(changeRequestId) {
        this.likeStagingService.getLikeDetails(changeRequestId).subscribe(result => {
            this.likeStagingGet = result;
            if (this.likeStagingGet.likeStatus == "Like") {//} && this.likeStagingGet.likedBy == this._identityInfo.customerId) {
                this.isLiked = true;
            }
            else if (!this.likeStagingGet.likedBy) {
                this.isLiked = false;
            }
        }),
            error => {
                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            }
    }

    private onClickLikes(id: number) {
        this.likeComponent.showAllLikedBy(id);
    }

    cleanupComponent(): Observable<boolean> | boolean {
        return this.newAcFileUploader.cleanupAllTempContainers();
    }

    validationCheck(): boolean {
        var status = false;

        //Status Changed
        if (this.isPreliminaryApproveChecked !== false || this.isApproveChecked !== false || this.isRejectChecked !== false) {
            status = true;
            return true;
        }

        //Comments Added
        if (this.reviewComment.comment.length > 0) {
            status = true;
            return true;
        }

        //Attachment Uploaded
        if (this.newAcFileUploader.checkAttachment()) {
            status = true;
            return status;
        }

        return status;
    }

}
