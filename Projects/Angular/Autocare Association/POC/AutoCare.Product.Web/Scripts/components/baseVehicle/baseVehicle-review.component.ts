import { Component, OnInit, ViewChild }                               from "@angular/core";
import { ToastsManager }                                              from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import {  ModalComponent }                           from "ng2-bs3-modal/ng2-bs3-modal";
import { BaseVehicleService }                                         from "./baseVehicle.service";
import { IBaseVehicleChangeRequestStagingReview }                     from "./baseVehicle.model";
import { ConstantsWarehouse }                                         from "../constants-warehouse";
import { Router, ActivatedRoute }                  from "@angular/router";
import { ReviewerCommentsComponent}                                   from "../changeRequestReview/reviewerComments.component";
import { IChangeRequestReview, ChangeRequestStatus, IAssignReviewer } from "../changeRequestReview/changeRequestReview.model";
import { VehicleService }                                             from "../vehicle/vehicle.service";
import { IMake}                                                       from '../make/make.model';
import { MakeService}                                                 from '../make/make.service';
import { IModel}                                                      from '../model/model.model';
import { ModelService}                                                from '../model/model.service';
import { ICommentsStaging }                                           from "../changeRequestReview/commentsStaging.model";
import { ILikeStaging}                                                from "../changeRequestReview/likeStaging.model";
import { LikeStagingService}                                          from "../changeRequestReview/likeStaging.service";
import { SharedService}                                               from '../shared/shared.service';
import { NavigationService }                                          from '../shared/navigation.service';
import { AcFileUploader }                                             from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { LoadingGifComponent }                                        from '../shared/loadingGif.component';
import { IUser }                                                      from "../change/user.model";
import { UserService }                                                from "../change/user.service";
import { ChangeService  }                                             from "../change/change.service";
import { IIdentityInfo }                                              from "../shared/shared.model";
import { Observable }    from 'rxjs/Observable';
import { UserLikesComponent }                                  from "../changeRequestReview/userLikes.component";

@Component({
    selector: 'baseVehicle-review-comp',
    templateUrl: 'app/templates/baseVehicle/baseVehicle-review.component.html',
    providers: [BaseVehicleService, VehicleService, ModelService, MakeService, LikeStagingService, UserService, ChangeService],
})

export class BaseVehicleReviewComponent implements OnInit {
    baseVehicleChangeRequest: IBaseVehicleChangeRequestStagingReview;
    isPreliminaryApproveChecked: boolean = false;
    isApproveChecked: boolean = false;
    isRejectChecked: boolean = false;
    changeRequestReview: IChangeRequestReview = { changeRequestId: 0, reviewedBy: "Me", reviewStatus: ChangeRequestStatus.Submitted };
    reviewComment: ICommentsStaging = { comment: "" };
    likeStaging: ILikeStaging = { changeRequestId: 0, likedBy: "", likeStatus: "", likeCount: 0 };
    likeStagingGet: ILikeStaging = { changeRequestId: 0, likedBy: "", likeStatus: "", likeCount: 0 };
    showLoadingGif: boolean = false;
    backNavigation: string = "/dashboard";
    backNavigationText: string = "Return to Dashboard";
    private isReviewCompleted: boolean = false;
    private isLiked: boolean = false;
    //private _identityInfo: IIdentityInfo = { customerId: "", email: "", isAdmin: false, isRequestor: false, isResearcher: false };
    usersForAssignment: IUser[] = [];
    assignedReviewer: IAssignReviewer = { assignedToUserId: "-1", changeRequestIds: [] };
    showPrelimRadio: boolean = true;
    allLikedBy: ILikeStaging[] = [];

    @ViewChild("viewAffectedVehiclesPopup")
    public viewAffectedVehiclesPopup: ModalComponent;

    @ViewChild("newAcFileUploader")
    newAcFileUploader: AcFileUploader;

    @ViewChild("existingAcFileUploader")
    existingAcFileUploader: AcFileUploader;

    @ViewChild("likeComponent")
    likeComponent: UserLikesComponent;

    constructor(private baseVehicleService: BaseVehicleService, private vehicleService: VehicleService,
        private makeService: MakeService, private modelService: ModelService, private toastr: ToastsManager,
        private route: ActivatedRoute, private router: Router, private likeStagingService: LikeStagingService,
        private sharedService: SharedService, private userService: UserService, private changeService: ChangeService, private navgationService: NavigationService) {
        if (this.navgationService.backRoute) {
            this.backNavigation = this.navgationService.backRoute;
            if (this.backNavigation.indexOf('change') > 0)
                this.backNavigationText = "Return to Change Requests";
            else
                this.backNavigationText = "Return to Vehicle Search";
        }
    }

    ngOnInit() {
        this.showLoadingGif = true;
        // Load existing base vechile with reference from RouteParams
        const id = Number(this.route.snapshot.params["id"]);
        this.baseVehicleService.getChangeRequestStaging(id).subscribe(
            result => {
                this.baseVehicleChangeRequest = result;
                // setup authorization components
                //this._identityInfo = this.sharedService.getIdentityInfo(String(this.baseVehicleChangeRequest.stagingItem.submittedBy));
                //if (this.baseVehicleChangeRequest.stagingItem.status === "Approved" || this.baseVehicleChangeRequest.stagingItem.status === "Deleted" ||
                //    this.baseVehicleChangeRequest.stagingItem.status === "Rejected") {
                //    this.isReviewCompleted = true;
                //} else if (this.baseVehicleChangeRequest.stagingItem.status === "PreliminaryApproved") {
                //    this.showPrelimRadio = false;
                //    if (this._identityInfo.isResearcher)
                //        this.isReviewCompleted = true;
                //}

                // check if Preliminary Approved.
                if (this.baseVehicleChangeRequest.stagingItem.status == "PreliminaryApproved") {
                    this.showPrelimRadio = false;
                }

                if (this.baseVehicleChangeRequest.stagingItem.changeType != "Replace") {
                    // staging
                    this.makeService.getById(this.baseVehicleChangeRequest.entityStaging.makeId).subscribe(m => {
                        this.baseVehicleChangeRequest.entityStaging.makeName = m.name;
                        this.modelService.getModelDetail(this.baseVehicleChangeRequest.entityStaging.modelId).subscribe(m => {
                            this.baseVehicleChangeRequest.entityStaging.modelName = m.name;

                            // set review's changerequestid
                            this.changeRequestReview.changeRequestId = this.baseVehicleChangeRequest.stagingItem.changeRequestId;

                            //this.getLikes(id);
                            this.likeStagingService.getLikeDetails(id).subscribe(result => {
                                this.likeStagingGet = result;
                                if (this.likeStagingGet.likeStatus == "Like") {// && this.likeStagingGet.likedBy == this._identityInfo.customerId) {
                                    this.isLiked = true;
                                } else if (!this.likeStagingGet.likedBy) {
                                    this.isLiked = false;
                                }
                                this.showLoadingGif = false;
                            }),
                                error => {
                                    this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                                    this.showLoadingGif = false;
                                };
                        }, error => {
                            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                            this.showLoadingGif = false;
                        });
                    }, error => {
                        this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                        this.showLoadingGif = false;
                    });
                    // existing
                    if (this.baseVehicleChangeRequest.entityCurrent != null) {
                        this.makeService.getById(this.baseVehicleChangeRequest.entityCurrent.makeId).subscribe(m => {
                            this.baseVehicleChangeRequest.entityCurrent.makeName = m.name;
                            this.modelService.getModelDetail(this.baseVehicleChangeRequest.entityCurrent.modelId).subscribe(m => {
                                this.baseVehicleChangeRequest.entityCurrent.modelName = m.name;
                            }, error => {
                                this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                                this.showLoadingGif = false;
                            });
                        }, error => {
                            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                            this.showLoadingGif = false;
                        });
                    }
                } else {
                    //this.getLikes(id);
                    this.likeStagingService.getLikeDetails(id).subscribe(result => {
                        this.likeStagingGet = result;
                        if (this.likeStagingGet.likeStatus == "Like") {// && this.likeStagingGet.likedBy == this._identityInfo.customerId) {
                            this.isLiked = true;
                        } else if (!this.likeStagingGet.likedBy) {
                            this.isLiked = false;
                        }
                        this.showLoadingGif = false;
                    }),
                        error => {
                            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
                            this.showLoadingGif = false;
                        };
                }
            },
            error => {
                this.toastr.warning(<any>error, "Change request review counldn't be loaded.");
                this.showLoadingGif = false;
            });
        this.usersForAssignment = this.userService.getUsersForAssignment();
    }

    onReviewerChange() {
        if (this.assignedReviewer.assignedToUserId !== "-1" && !this.isPreliminaryApproveChecked) {
            this.isApproveChecked = false;
            this.isRejectChecked = false;
        }
    }

    onViewAffectedVehicles() {
        if (!this.baseVehicleChangeRequest || !this.baseVehicleChangeRequest.entityCurrent) {
            return;
        }

        this.viewAffectedVehiclesPopup.open("lg");
        if (!this.baseVehicleChangeRequest.entityCurrent.vehicles) {
            this.vehicleService.getVehiclesByBaseVehicleId(this.baseVehicleChangeRequest.entityCurrent.id).subscribe(m => {
                this.baseVehicleChangeRequest.entityCurrent.vehicles = m;
                this.baseVehicleChangeRequest.entityCurrent.vehicleCount = m.length;
            }, error => this.toastr.warning(<any>error));
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
            if (this.assignedReviewer.assignedToUserId !== "-1") {
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
                this.changeRequestReview.reviewStatus = ChangeRequestStatus[this.baseVehicleChangeRequest.stagingItem.status];
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

            let baseVehicleIdentity: string = this.baseVehicleChangeRequest.entityStaging.makeName + ", " + this.baseVehicleChangeRequest.entityStaging.modelName;

            this.baseVehicleService.submitChangeRequestReview(this.baseVehicleChangeRequest.stagingItem.changeRequestId, this.changeRequestReview).subscribe(
                result => {
                    if (result) {
                        let toasterMessage = null;
                        if (this.changeRequestReview.reviewStatus != ChangeRequestStatus.Deleted) {
                            toasterMessage = {
                                title: ConstantsWarehouse.reviewSubmitSuccessTitle,
                                body: `Your review for ${this.baseVehicleChangeRequest.stagingItem.changeType} ${this.baseVehicleChangeRequest.stagingItem.entityName} : ${baseVehicleIdentity} change request ID  '${this.baseVehicleChangeRequest.stagingItem.changeRequestId}'  was submitted successfully.`
                            };
                        } else {
                            toasterMessage = {
                                title: ConstantsWarehouse.reviewDeleteSuccessTitle,
                                body: `Your delete request for ${this.baseVehicleChangeRequest.stagingItem.changeType} ${this.baseVehicleChangeRequest.stagingItem.entityName} : ${baseVehicleIdentity}  change request ID '${this.baseVehicleChangeRequest.stagingItem.changeRequestId}' was submitted successfully.`
                            };
                        }

                        this.toastr.success(toasterMessage.body, toasterMessage.title);

                        if (this.assignedReviewer.assignedToUserId !== "-1")
                            this.onAssignReviewer();

                        else
                            this.router.navigateByUrl(this.backNavigation);
                    } else {
                        let toasterMessage = null;
                        if (this.changeRequestReview.reviewStatus != ChangeRequestStatus.Deleted) {
                            toasterMessage = {
                                title: ConstantsWarehouse.reviewSubmitErrorTitle,
                                body: `Your review for ${this.baseVehicleChangeRequest.stagingItem.changeType} ${this.baseVehicleChangeRequest.stagingItem.entityName} : ${baseVehicleIdentity}  change request ID  '${this.baseVehicleChangeRequest.stagingItem.changeRequestId}' failed.`
                            };
                        } else {
                            toasterMessage = {
                                title: ConstantsWarehouse.reviewDeleteErrorTitle,
                                body: `Your delete request for ${this.baseVehicleChangeRequest.stagingItem.changeType} ${this.baseVehicleChangeRequest.stagingItem.entityName} : ${baseVehicleIdentity}  change request ID  '${this.baseVehicleChangeRequest.stagingItem.changeRequestId}' failed.`
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
                            body: `Your review for ${this.baseVehicleChangeRequest.stagingItem.changeType} ${this.baseVehicleChangeRequest.stagingItem.entityName} : ${baseVehicleIdentity} failed.\n${<any>error.toString()}.`
                        };
                    } else {
                        toasterMessage = {
                            title: ConstantsWarehouse.reviewSubmitErrorTitle,
                            body: `Your delete request for ${this.baseVehicleChangeRequest.stagingItem.changeType} ${this.baseVehicleChangeRequest.stagingItem.entityName} : ${baseVehicleIdentity} failed.\n${<any>error.toString()}.`
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
            this.toastr.warning(<any>error, "Attachment couldn't be uploaded.");
            this.showLoadingGif = false;
        });
    }

    private onAssignReviewer() {
        var user = this.usersForAssignment.filter(x => x.id === this.assignedReviewer.assignedToUserId)[0];
        this.assignedReviewer.assignedToRoleId = user.roleId;
        this.assignedReviewer.changeRequestIds[0] = this.baseVehicleChangeRequest.stagingItem.changeRequestId;

        let baseVehicleIdentity: string = this.baseVehicleChangeRequest.entityStaging.makeName + ", " + this.baseVehicleChangeRequest.entityStaging.modelName;
        this.changeService.assignReviewer(this.assignedReviewer).subscribe(result => {
            if (result) {
                let toasterMessage = {
                    title: `Your review was assigned to ${user.user}`,
                    body: `Your review for ${this.baseVehicleChangeRequest.stagingItem.changeType} ${this.baseVehicleChangeRequest.stagingItem.entityName} : ${baseVehicleIdentity} assigned successfully.`
                }
                this.toastr.success(toasterMessage.body, toasterMessage.title);

                // redirect to search result
                this.router.navigateByUrl(this.backNavigation);
            } else {
                let toasterMessage = {
                    title: "Your review couldn't be assigned.",
                    body: `Your assignment for ${this.baseVehicleChangeRequest.stagingItem.changeType} ${this.baseVehicleChangeRequest.stagingItem.entityName} : ${baseVehicleIdentity} failed.`
                }
                this.toastr.warning(toasterMessage.body, toasterMessage.title);
            }
        },
            error => {
                this.toastr.warning(`Your assignment for ${this.baseVehicleChangeRequest.stagingItem.changeType} 
                 ${this.baseVehicleChangeRequest.stagingItem.entityName} : ${baseVehicleIdentity} failed.\n${error}.`, "Your review couldn't be assigned.");
            });
    }

    onCancel() {
        // redirect to search result
        this.router.navigateByUrl(this.backNavigation);
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
            if (this.likeStagingGet.likeStatus == "Like") {// && this.likeStagingGet.likedBy == this._identityInfo.customerId) {
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
}
