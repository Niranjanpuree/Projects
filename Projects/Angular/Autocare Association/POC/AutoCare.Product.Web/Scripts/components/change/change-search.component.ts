import { Component, OnInit, EventEmitter, Output, ViewChild }                   from "@angular/core";
import { Router }                                            from "@angular/router";
import { ModalComponent}                                      from 'ng2-bs3-modal/ng2-bs3-modal';
import { ChangeSearchService }                                                  from "./change-search.service";
import { SharedService }                                                        from "../shared/shared.service";
import { IFacet }                                                               from "../shared/shared.model";
import { NavigationService }                                                    from "../shared/navigation.service";
import { IChangeRequest, IAffectedList }                      from "./change.model";
import { IChangeRequestSearchViewModel, IChangeRequestSearchInputModel,
         SearchType, IChangeEntity, IChangeStatus}                              from "./change-search.model";
import { ChangeService  }                                                       from "./change.service";
import { IUser }                                                                from "./user.model";
import { UserService }                                                          from "./user.service";
import { AcGridComponent }                                                      from '../../lib/aclibs/ac-grid/ac-grid';
import { ToastsManager }                                                        from "../../lib/aclibs/ng2-toastr/ng2-toastr";
import { IChangeRequestBulkReview, IChangeRequestReview,
    ChangeRequestStatus, IAssignReviewer }                                      from "../changeRequestReview/changeRequestReview.model";
import { ICommentsStaging }                                                     from "../changeRequestReview/commentsStaging.model";
import { ConstantsWarehouse }                                                   from "../constants-warehouse";
import { ILikeStaging }                                                         from "../changeRequestReview/likeStaging.model";
import { UserLikesComponent }                                  from "../changeRequestReview/userLikes.component";

import { NKDatetime, NKDatetimeModule } from "../../lib/aclibs/ng2-datetime/ng2-datetime";

@Component({
    selector: 'changeSearch-comp',
    templateUrl: 'app/templates/change/change-search.component.html',
    providers: [ChangeSearchService, UserService, ChangeService]
})

export class ChangeSearchComponent implements OnInit {
    private errorMessage: string;
    private selectedCR: IChangeRequest[] = [];
    private affectedList: IAffectedList[] = [];
    private changeRequestReviews: IChangeRequestBulkReview[] = [];
    private reviewComment: ICommentsStaging = { comment: "" };
    private requestorComments: ICommentsStaging[] = [];
    private changeRequestSearchViewModel: IChangeRequestSearchViewModel;
    private changeRequestSearchInputModel: IChangeRequestSearchInputModel;
    private allChangeEntities: IChangeEntity[] = [];
    private distinctCRChangeTypes: IChangeEntity[] = [];
    private allSubmitsBy: IUser[] = [];
    private usersForAssignment: IUser[] = [];
    private allUsersForAssignment: IUser[] = [];
    private showLoadingGif: boolean = false;
    private isSelectAllChangeRequests: boolean;
    //Change search filter queries
    private entityQuery: string;
    private submittedQuery: string;
    private assigneeQuery: string;
    //Required to disable buttons based upon selections
    private disablePreliminary: boolean = true;
    private disableApprove: boolean = true;
    private disableReject: boolean = true;
    private disableAssign: boolean = true;
    private assignedReviewer: IAssignReviewer = { assignedToUserId: "-1", changeRequestIds: [] };
    private allLikedBy: ILikeStaging[] = [];
    private thresholdRecordCount: number = 100;

    private selectedChangeRequestIdForSearch: string;
    private submittedDateFrom: string;
    private submittedDateTo: string;

    //Facets
    private changeTypeFacets: IFacet[];
    private changeEntityFacets: IFacet[];
    private requestByFacets: IFacet[];
    private statusFacets: IFacet[];
    private assigneeFacets: IFacet[];

    private changeTypeList: IFacet[] = [];

    private refreshOnLoadFlag: boolean = false;
    private searchOnLoadFlag: boolean = false;

    @ViewChild('RejectPopup') rejectPopup: ModalComponent;
    @ViewChild('PreliminaryPopup') preliminaryPopup: ModalComponent;
    @ViewChild('ApprovePopup') approvePopup: ModalComponent;
    @ViewChild('AssignPopup') assignPopup: ModalComponent;
    @ViewChild('commentPopupModel') commentPopupModel: ModalComponent;
    @ViewChild('likedByPopupModel') likedByPopupModel: ModalComponent;
    @ViewChild("changeRequestGrid") changeRequestGrid: AcGridComponent;
    @ViewChild("likeComponent")
    likeComponent: UserLikesComponent;

    constructor(private sharedService: SharedService,
        private router: Router, private changeSearchService: ChangeSearchService,
        private userService: UserService, private toastr: ToastsManager, private changeService: ChangeService, private navigationService: NavigationService) {
    }

    ngOnInit() {
        this.isSelectAllChangeRequests = false;
        let token = this.sharedService.getTokenModel();

        this.changeRequestSearchViewModel = {
            facets: { changeTypes: [], changeEntities: [], requestsBy: [], statuses: [], assignees: [] },
            result: { changeRequests: [] },
            canBulkSubmit: false, isAdmin: false,
            searchType: SearchType.None
        };

        if (this.sharedService.changeRequestSearchViewModel != null) {
            this.changeRequestSearchViewModel.facets = this.sharedService.changeRequestSearchViewModel.facets;

            this.assigneeFacets = this.changeRequestSearchViewModel.facets.assignees.slice();
            this.changeTypeFacets = this.changeRequestSearchViewModel.facets.changeTypes.slice();
            this.changeEntityFacets = this.changeRequestSearchViewModel.facets.changeEntities.slice();
            this.requestByFacets = this.changeRequestSearchViewModel.facets.requestsBy.slice();
            this.statusFacets = this.changeRequestSearchViewModel.facets.statuses.slice();

            if (this.sharedService.changeRequestSearchViewModel.searchType === SearchType.SearchByChangeRequestId) {
                this.selectedChangeRequestIdForSearch = this.sharedService.changeRequestSearchViewModel.result.changeRequests[0].id.toString();
                this.searchByChangeRequestId();
            } else if (this.sharedService.changeRequestSearchViewModel.searchType === SearchType.GeneralSearch) {
                this.searchChanges();
            } else {
                this.showLoadingGif = false;
            }
        }
        else {
            this.refreshFacets(true);
            this.searchChanges(true);
        }


        //TODO: User are hard coded in  user.service.ts file for now. We need to fetch users from  db or azure in the future.
        this.usersForAssignment = this.userService.getUsersForAssignment();
        this.allUsersForAssignment = this.userService.getAllUserForAssignment();

        // Drawer right start
        {
            var headerht = $('header').innerHeight();
            var navht = $('nav').innerHeight();
            var winht = $(window).height();
            var winwt = 700;

            $(".drawer-left").css('min-height', winht - headerht - navht);
            $(".drawer-left").css('width', winwt);

            $(document).on('click', '.drawer-show', function (event) {
                $(".drawer-left").css('width', winwt);
            });

            $(".drawer-left span").on('click', function () {

                var drawerwt = $(".drawer-left").width();

                if (drawerwt == 15) {
                    $(".drawer-left").css('width', winwt);
                }
                else {
                    $(".drawer-left").css('width', 15);
                }
            });


            $(document).on('click', function (event) {
                if (!$(event.target).closest(".drawer-left").length) {
                    // Hide the menus.
                    var drawerwt = $(".drawer-left").width();
                    if (drawerwt > 15) {
                        $(".drawer-left").css('width', 15);
                    }
                }
            });
        }
        // Drawer right end
    }

    onSearchEntityChange() {
        if (this.changeRequestSearchViewModel.facets.changeEntities != null && this.entityQuery !== undefined && this.entityQuery !== null) {
            this.changeEntityFacets =
                this.changeRequestSearchViewModel.facets.changeEntities.filter(item => item.name.toLocaleLowerCase().indexOf(this.entityQuery.toLocaleLowerCase()) >= 0);
        }
    }

    onSearchSubmittedChange() {
        if (this.changeRequestSearchViewModel.facets.requestsBy != null && this.submittedQuery !== undefined && this.submittedQuery !== null) {
            this.requestByFacets =
                this.changeRequestSearchViewModel.facets.requestsBy.filter(item => item.id.toLocaleLowerCase().indexOf(this.submittedQuery.toLocaleLowerCase()) >= 0);
        }
    }

    onSearchAssigneeChange() {
        if (this.changeRequestSearchViewModel.facets.assignees != null && this.assigneeQuery !== undefined && this.assigneeQuery !== null) {
            this.assigneeFacets =
                this.changeRequestSearchViewModel.facets.assignees.filter(item => item.id.toLocaleLowerCase().indexOf(this.assigneeQuery.toLocaleLowerCase()) >= 0);
        }
    }

    searchChanges(onPageLoadFlag?: boolean) {
        this.showLoadingGif = true;
        this.isSelectAllChangeRequests = false;
        this.changeRequestSearchInputModel = {
            changeTypes: [],
            changeEntities: [],
            requestsBy: [],
            statuses: [],
            assignees: []
        };
        this.changeRequestSearchViewModel.result =
            {
                changeRequests: []
            };
        this.selectedCR = [];
        let inputModel = this.getDefaultInputModel();

        if (this.changeRequestSearchViewModel.facets.changeTypes) {
            this.changeRequestSearchViewModel.facets.changeTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.changeTypes.push(m.name));
        }
        if (this.changeRequestSearchViewModel.facets.changeEntities) {
            this.changeRequestSearchViewModel.facets.changeEntities.filter(item => item.isSelected)
                .forEach(m => inputModel.changeEntities.push(m.name));
        }
        if (this.changeRequestSearchViewModel.facets.requestsBy) {
            this.changeRequestSearchViewModel.facets.requestsBy.filter(item => item.isSelected)
                .forEach(m => inputModel.requestsBy.push(m.id));
        }
        if (this.changeRequestSearchViewModel.facets.assignees) {
            this.changeRequestSearchViewModel.facets.assignees.filter(item => item.isSelected)
                .forEach(m => inputModel.assignees.push(m.id));
        }
        if (this.changeRequestSearchViewModel.facets.statuses) {
            this.changeRequestSearchViewModel.facets.statuses.filter(item => item.isSelected)
                .forEach(s => inputModel.statuses.push(s.name));
        }

        inputModel.submittedDateFrom = this.submittedDateFrom;
        inputModel.submittedDateTo = this.submittedDateTo;

        this.changeRequestSearchViewModel.searchType = SearchType.GeneralSearch;
        this.changeSearchService.search(inputModel).subscribe(m => {
            if (m.result) {
                this.getSearchResult(<any>m);
                this.crStatusCheck();
                this.searchOnLoadFlag = true;
                if (onPageLoadFlag) {
                    if(this.refreshOnLoadFlag)
                        this.showLoadingGif = false;
                } else {
                    this.showLoadingGif = false;
                }
                $(".drawer-left").css('width', 15);
            }
            else {
                this.toastr.warning("The search yeilded no result", "No Record Found!!");
                this.showLoadingGif = false;
            }
        },
            error => {
                this.errorMessage = <any>error;
                this.showLoadingGif = false;
            });
    }

    onCRIdKeyPress(event) {
        if (event.keyCode == 13) {
            this.searchByChangeRequestId();
        }
    }

    searchByChangeRequestId() {
        let changeRequestId = Number(this.selectedChangeRequestIdForSearch);
        if (isNaN(changeRequestId)) {
            this.toastr.warning("Invalid Change request #", ConstantsWarehouse.validationTitle);
            return;
        }

        this.selectedCR = [];
        this.changeRequestSearchViewModel.searchType = SearchType.SearchByChangeRequestId;
        this.showLoadingGif = true;
        this.changeSearchService.searchbyChangeRequestId(this.selectedChangeRequestIdForSearch).subscribe(m => {
            if (m.result) {
                this.getSearchResult(<any>m);
                this.showLoadingGif = false;
                $(".drawer-left").css('width', 15);
            }
            else {
                this.toastr.warning("The search yeilded no result", "No Record Found!!");
                this.showLoadingGif = false;
            }
        },
            error => {
                this.errorMessage = <any>error;
                this.showLoadingGif = false;
            });

    }

    getSearchResult(m: IChangeRequestSearchViewModel) {
        this.changeRequestSearchViewModel.result.changeRequests = <any>m.result.changeRequests.map(x => {
            return {
                id: x.id,
                entity: x.entity,
                changeRequestTypeId: x.changeRequestTypeId,
                changeType: x.changeType,
                requestedBy: {
                    id: x.requestedBy
                },
                createdDateTime: x.createdDateTime,
                updatedDateTime: x.updatedDateTime,
                status: {
                    id: x.status,
                    status: x.statusText
                },
                likes: x.likes,
                commentExists: x.commentExists,
                assignee: {
                    id: x.assignee
                },
                changeContent: x.changeContent,
                isSelected: false
            };
        });

        // role properties
        this.changeRequestSearchViewModel.canBulkSubmit = m.canBulkSubmit;
        this.changeRequestSearchViewModel.isAdmin = m.isAdmin;

        // refresh
        if (this.changeRequestGrid)
            this.changeRequestGrid.refresh();
    }

    changeRequestReview(changeRequest: IChangeRequest) {
        this.sharedService.changeRequestSearchViewModel = this.changeRequestSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/" + changeRequest.entity.toLowerCase() + "/" + changeRequest.id;
        this.router.navigateByUrl(changeRequestLink);
    }

    onSelectAllChangeRequest(event) {
        this.isSelectAllChangeRequests = event;
        if (this.changeRequestSearchViewModel.result.changeRequests == null) {
            return;
        }
        if (event) {
            this.changeRequestSearchViewModel.result.changeRequests.forEach(item => {
                item.isSelected = event;
                this.selectedCR.push(item);
                this.crStatusCheck();
            });
        }
        else {
            this.selectedCR = [];
            this.changeRequestSearchViewModel.result.changeRequests.forEach(item => {
                item.isSelected = event;
            });
            this.crStatusCheck();
        }
    }

    crStatusChange(approvalType: string) {
        if (this.selectedCR.length > 0) {
            this.affectedList = [];
            this.changeRequestSearchViewModel.facets.changeEntities.forEach(item => {
                item.count = this.selectedCR.filter(cr => cr.entity === item.name).length;
            });

            //Note: This code is to group the change types and get the distinct count
            this.distinctCRChangeTypes = [];
            //append changeType to a new array
            this.changeTypeList = this.changeRequestSearchViewModel.result.changeRequests.filter(x => x.isSelected).map(x => {
                return {
                    name: x.changeType
                };
            });

            //Group the change types and get their count
            var types = {},
                i, j, currentType;
            for (i = 0, j = this.changeTypeList.length; i < j; i++) {
                currentType = this.changeTypeList[i];
                if (!(currentType.name in types)) {
                    types[currentType.name] = { name: currentType.name, count: 0 };
                    this.distinctCRChangeTypes.push(types[currentType.name]);
                }
                types[currentType.name].count++;
            }

            if (approvalType !== "assign") {
                this.showLoadingGif = true;
                this.changeService.getAssociatedCount(this.selectedCR).subscribe(m => {
                    Object.keys(m).forEach(x => {
                        this.affectedList.push({
                            type: x.toString().substring(0, x.length - 5).toLocaleLowerCase(),
                            count: m[x.toString()]
                        });
                    });
                    this.showLoadingGif = false;
                });
            }

            switch (approvalType) {
                case "approved":
                    //this.reviewComment.comment = "Bulk Approval";
                    this.changeRequestReviews = this.convertRequestToReview(this.selectedCR, ChangeRequestStatus.Approved);
                    this.approvePopup.open("md");
                    break;

                case "preliminaryapproved":
                    //this.reviewComment.comment = "Bulk Preliminary Approval";
                    this.changeRequestReviews = this.convertRequestToReview(this.selectedCR, ChangeRequestStatus.PreliminaryApproved);
                    this.preliminaryPopup.open("md");
                    break;

                case "rejected":
                    //this.reviewComment.comment = "Bulk Rejection";
                    this.changeRequestReviews = this.convertRequestToReview(this.selectedCR, ChangeRequestStatus.Rejected);
                    this.rejectPopup.open("md");
                    break;

                case "assign":
                    this.assignPopup.open("md");
                    break;

                default:
                    break;
            }
        }
    }

    onConfirm(approvalType: string) {
        switch (approvalType) {
            case "approved":
                this.bulkSubmit(approvalType);
                this.approvePopup.close();
                break;

            case "preliminaryapproved":
                this.bulkSubmit(approvalType);
                this.preliminaryPopup.close();
                break;

            case "rejected":
                this.bulkSubmit(approvalType);
                this.rejectPopup.close();
                break;

            case "assign":
                var assignedReviewerDetail = this.usersForAssignment.filter(x => x.id === this.assignedReviewer.assignedToUserId)[0];
                this.assignedReviewer.assignedToRoleId = assignedReviewerDetail.roleId;
                this.selectedCR.forEach(x => {
                    this.assignedReviewer.changeRequestIds.push(x.id);
                });

                this.showLoadingGif = true;
                this.changeService.assignReviewer(this.assignedReviewer).subscribe(result => {
                    if (result) {
                        this.toastr.success(
                            "Select Change Request were assigned successfully.",
                            "Your review was assigned to " + assignedReviewerDetail.user
                        );
                        this.selectedCR.forEach(x => {
                            x.assignee = assignedReviewerDetail;
                        });
                        this.selectedCR = [];
                        this.changeRequestSearchViewModel.result.changeRequests.forEach(x => x.isSelected = false);
                        this.assignedReviewer.changeRequestIds = [];

                        this.showLoadingGif = false;
                        // redirect to search result
                        //this.router.navigateByUrl("/change/search");
                    } else {
                        this.toastr.warning("Select Change Request assignment failed.",
                            "Your review was couldn't be submitted.");
                    }
                },
                    error => {
                        this.errorMessage = error;
                        //console.log(this.errorMessage);
                    });

                this.assignPopup.close();
                break;

            default:
                break;
        }

    }

    private bulkSubmit(approvalType: string) {
        this.showLoadingGif = true;
        var i = 0;
        var count = this.selectedCR.length;
        this.changeRequestReviews.forEach(cr => {
            var baseUrl = ConstantsWarehouse.api[Object.keys(ConstantsWarehouse.api).filter(x => x.toLowerCase() === cr.entity.toLowerCase()).toString()];
            var changeRequestReview: IChangeRequestReview =
                {
                    changeRequestId: cr.changeRequestId,
                    reviewStatus: cr.reviewStatus,
                    reviewedBy: cr.reviewedBy,
                    reviewComment: cr.reviewComment
                };
            var index = this.changeRequestSearchViewModel.result.changeRequests.findIndex(x => x.id === cr.changeRequestId);
            //var index = this.changeRequestSearchViewModel.result.changeRequests.indexOf(this.changeRequestSearchViewModel.result.changeRequests.filter(x => x.id === cr.changeRequestId)[0]);
            this.changeService.selectedCRApproval(baseUrl, cr.changeRequestId, changeRequestReview).subscribe(response => {
                if (response == true) {
                    this.toastr.success(
                        "You review for " + cr.entity + " submitted successfully.",
                        "Change Request Id: " + cr.changeRequestId + " " + approvalType + " successfull!!!");
                    this.changeRequestSearchViewModel.result.changeRequests[index].isSelected = false;
                    var status = this.changeRequestSearchViewModel.facets.statuses.filter(x => x.name.toLowerCase() === approvalType.toLowerCase())[0];

                    this.changeRequestSearchViewModel.result.changeRequests[index].status.status = status.name;
                    if (i++ >= count - 1)
                        this.showLoadingGif = false;
                }
                else {
                    this.toastr.warning(
                        "You review for " + cr.entity + " failed..",
                        "Change Request Id: " + cr.changeRequestId + " " + approvalType + " failed!!!");
                    if (i++ >= count - 1)
                        this.showLoadingGif = false;
                }

            }
                , error => {
                    this.toastr.warning("Error occured while reviewing Change Request for " + cr.entity + ".",
                        "Change Request Id: " + cr.changeRequestId + " " + approvalType + " failed!!!");
                    this.showLoadingGif = false;
                });

           
        });

        this.selectedCR = [];
        this.changeRequestReviews = [];
        this.distinctCRChangeTypes = [];
        this.crStatusCheck();
        //this.searchChanges();
    }

    crStatusCheck() {
        if (this.selectedCR.length > 0) {
            // canBulkSubmit is either admin or researcher
            if (this.changeRequestSearchViewModel.canBulkSubmit) {
                // for admin
                if (this.changeRequestSearchViewModel.isAdmin) {
                    if (this.selectedCR.every(x => x.status.status === "Submitted")) {
                        this.disableAssign = false;
                        this.disableApprove = false;
                        this.disablePreliminary = false;
                        this.disableReject = false;
                    } else if (this.selectedCR.every(x => x.status.status === "PreliminaryApproved")) {
                        this.disableAssign = false;
                        this.disableApprove = false;
                        this.disablePreliminary = true;
                        this.disableReject = false;
                    } else if (this.selectedCR.every(x => (x.status.status === "Submitted" || x.status.status === "PreliminaryApproved"))) {
                        this.disableAssign = false;
                        this.disableApprove = true;
                        this.disablePreliminary = true;
                        this.disableReject = false;
                    }
                    else {
                        this.disableAssign = true;
                        this.disableApprove = true;
                        this.disablePreliminary = true;
                        this.disableReject = true;
                    }
                } // for researcher
                else {
                    if (this.selectedCR.every(x => x.status.status === "Submitted")) {
                        this.disableAssign = false;
                        this.disablePreliminary = false;
                    } else if (this.selectedCR.every(x => x.status.status === "PreliminaryApproved")) {
                        this.disableAssign = false;
                        this.disablePreliminary = true;
                    } else if (this.selectedCR.every(x => (x.status.status === "Submitted" || x.status.status === "PreliminaryApproved"))) {
                        this.disableAssign = false;
                        this.disablePreliminary = true;
                    }
                    else {
                        this.disableAssign = true;
                        this.disablePreliminary = true;
                    }
                }
            }
        } else {
            this.disableAssign = true;
            this.disableApprove = true;
            this.disablePreliminary = true;
            this.disableReject = true;
        }
    }

    onCRSelected(changeRequest: IChangeRequest, event) {
        if (event.target.checked) {
            this.selectedCR.push(changeRequest);
            this.crStatusCheck();
        }
        else {
            this.selectedCR = this.selectedCR.filter(x => x.id !== changeRequest.id);
            if (this.selectedCR.length >0) {
                this.crStatusCheck();
            }
        }

        if (changeRequest.isSelected) {
            //unchecked
            this.isSelectAllChangeRequests = false;
        }
        else {
            //checked
            var excludedChangeRequests = this.changeRequestSearchViewModel.result.changeRequests.filter(item => item.id != changeRequest.id);
            if (excludedChangeRequests.every(item => item.isSelected)) {
                this.isSelectAllChangeRequests = true;
            }
        }

    }

    convertRequestToReview(selectedCR: IChangeRequest[], status: ChangeRequestStatus): IChangeRequestBulkReview[] {
        var changeRequestList: IChangeRequestBulkReview[] = [];
        selectedCR.forEach(x => {
            changeRequestList.push({
                changeRequestId: x.id,
                reviewStatus: status,
                reviewedBy: x.requestedBy.id,
                entity: x.entity,
                reviewComment: this.reviewComment
            });
        });
        return changeRequestList;
    }

    onClearFilters() {
        this.assigneeQuery = "";
        this.submittedQuery = "";
        this.entityQuery = "";

        this.selectedChangeRequestIdForSearch = "";
        this.submittedDateFrom = "";
        this.submittedDateTo = "";
        if (this.assigneeFacets.filter(item => item.isSelected).length > 0)
            this.changeRequestSearchViewModel.facets.assignees.forEach(x => x.isSelected = false);
        if (this.changeTypeFacets.filter(item => item.isSelected).length > 0)
            this.changeRequestSearchViewModel.facets.changeTypes.forEach(x => x.isSelected = false);
        if (this.changeEntityFacets.filter(item => item.isSelected).length > 0)
            this.changeRequestSearchViewModel.facets.changeEntities.forEach(x => x.isSelected = false);
        if (this.requestByFacets.filter(item => item.isSelected).length > 0)
            this.changeRequestSearchViewModel.facets.requestsBy.forEach(x => x.isSelected = false);
        if (this.statusFacets.filter(item => item.isSelected).length > 0)
            this.changeRequestSearchViewModel.facets.statuses.forEach(x => x.isSelected = false);

        this.refreshFacets();
    }

    clearApprovedStatusFilter(facet: IFacet[]) {
        if (facet.filter(x=> x.isSelected).length>0) {
            this.changeRequestSearchViewModel.facets.statuses.forEach(x => x.isSelected = false);
            this.refreshFacets();
        }
    }

    clearChangeTypeFilter(facet: IFacet[]) {
        if (facet.filter(x => x.isSelected).length > 0) {
            this.changeRequestSearchViewModel.facets.changeTypes.forEach(x => x.isSelected = false);
            this.refreshFacets();
        }
    }

    clearChangeEntityFilter(facet: IFacet[]) {
        this.entityQuery = "";
        this.onSearchEntityChange();
        if (facet.filter(x => x.isSelected).length > 0) {
            this.changeRequestSearchViewModel.facets.changeEntities.forEach(x => x.isSelected = false);
            this.refreshFacets();
        }
    }

    clearAssignedToFilter(facet: IFacet[]) {
        this.assigneeQuery = "";
        this.onSearchAssigneeChange();
        if (facet.filter(x => x.isSelected).length > 0) {
            this.changeRequestSearchViewModel.facets.assignees.forEach(x => x.isSelected = false);
            this.refreshFacets();
        }
    }

    clearSubmittedByFilter(facet: IFacet[]) {
        this.submittedQuery = "";
        this.onSearchSubmittedChange();
        if (facet.filter(x => x.isSelected).length > 0) {
            this.changeRequestSearchViewModel.facets.requestsBy.forEach(x => x.isSelected = false);
            this.refreshFacets();
        }
    }
    
    showRequestorComment(id: string, status: string) {

        this.changeSearchService.getRequestorComment(id, status).subscribe(result => {
            this.requestorComments = result;
            this.commentPopupModel.open("md");
        });
    }

    private onClickLikes(id: number) {
        this.likeComponent.showAllLikedBy(id);
    }

    onItemSelected(event, facet: IFacet[]) {
        let isChecked = event.target.checked;
        let selectedItem = facet.filter(item => item.name.toLocaleLowerCase() === event.target.value.toLocaleLowerCase())[0];
        selectedItem.isSelected = isChecked;
        this.refreshFacets();
    }

    onUserItemSelected(event, facet: IFacet[]) {
        let isChecked = event.target.checked;
        let selectedItem = facet.filter(item => item.id.toLocaleLowerCase() === event.target.value.toLocaleLowerCase())[0];
        selectedItem.isSelected = isChecked;
        this.refreshFacets();
    }

    getDefaultInputModel() {
        return {
            submittedDateFrom: this.submittedDateFrom,
            submittedDateTo: this.submittedDateTo,
            changeTypes: [],
            changeEntities: [],
            statuses: [],
            requestsBy: [],
            assignees: []
        };
    }

    datepickerOpts = {
        startDate: new Date(1900, 1, 1),
        autoclose: true,
        todayBtn: 'linked',
        todayHighlight: true,
        assumeNearbyYear: true,
        format: 'D, d MM yyyy'
    }

    refreshFacets(onPageLoadFlag?: boolean) {
        let inputModel = this.getDefaultInputModel();
        inputModel.submittedDateFrom = this.submittedDateFrom;
        inputModel.submittedDateTo = this.submittedDateTo;

        if (this.changeRequestSearchViewModel.facets.statuses) {
            this.changeRequestSearchViewModel.facets.statuses.filter(item => item.isSelected)
                .forEach(m => inputModel.statuses.push(m.name));
        }

        if (this.changeRequestSearchViewModel.facets.changeTypes) {
            this.changeRequestSearchViewModel.facets.changeTypes.filter(item => item.isSelected)
                .forEach(m => inputModel.changeTypes.push(m.name));
        }

        if (this.changeRequestSearchViewModel.facets.changeEntities) {
            this.changeRequestSearchViewModel.facets.changeEntities.filter(item => item.isSelected)
                .forEach(m => inputModel.changeEntities.push(m.name));
        }

        if (this.changeRequestSearchViewModel.facets.requestsBy) {
            this.changeRequestSearchViewModel.facets.requestsBy.filter(item => item.isSelected)
                .forEach(m => inputModel.requestsBy.push(m.id));
        }

        if (this.changeRequestSearchViewModel.facets.assignees) {
            this.changeRequestSearchViewModel.facets.assignees.filter(item => item.isSelected)
                .forEach(m => inputModel.assignees.push(m.id));
        }

        this.showLoadingGif = true;
        this.changeSearchService.refreshFacets(inputModel).subscribe(data => {
            this.updateStatusFacet(data.facets.statuses);
            this.updateTypeFacet(data.facets.changeTypes);
            this.updateEntityFacet(data.facets.changeEntities);
            this.updateRequestByFacet(data.facets.requestsBy);
            this.updateAssigneeFacet(data.facets.assignees);

            this.onSearchEntityChange();
            this.onSearchSubmittedChange();
            this.onSearchAssigneeChange();
            this.refreshOnLoadFlag = true;
            if (onPageLoadFlag) {
                if (this.searchOnLoadFlag)
                    this.showLoadingGif = false;
            } else {
                this.showLoadingGif = false;
            }
        }, error => {
            this.toastr.warning(<any>error, ConstantsWarehouse.errorTitle);
            this.showLoadingGif = false;
        });
    }

    updateStatusFacet(statuses) {
        let existingSelectedStatuses = this.changeRequestSearchViewModel.facets.statuses.filter(item => item.isSelected).map(item => item.name);
        this.changeRequestSearchViewModel.facets.statuses = [];

        for (let item of statuses) {
            let newItem = {
                name: item,
                isSelected: false
            };

            for (let existingSelectedStatus of existingSelectedStatuses) {
                if (item === existingSelectedStatus) {
                    newItem.isSelected = true;
                }
            }

            this.changeRequestSearchViewModel.facets.statuses.push(newItem);
        }

        this.statusFacets = this.changeRequestSearchViewModel.facets.statuses.slice();
    }

    updateTypeFacet(changeTypes) {
        let existingSelectedTypes = this.changeRequestSearchViewModel.facets.changeTypes.filter(item => item.isSelected).map(item => item.name);
        this.changeRequestSearchViewModel.facets.changeTypes = [];

        for (let item of changeTypes) {
            let newItem = {
                name: item,
                isSelected: false
            };

            for (let existingSelectedType of existingSelectedTypes) {
                if (item === existingSelectedType) {
                    newItem.isSelected = true;
                }
            }

            this.changeRequestSearchViewModel.facets.changeTypes.push(newItem);
        }

        this.changeTypeFacets = this.changeRequestSearchViewModel.facets.changeTypes.slice();
    }

    updateEntityFacet(changeEntities) {
        let existingSelectedEntities = this.changeRequestSearchViewModel.facets.changeEntities.filter(item => item.isSelected).map(item => item.name);
        this.changeRequestSearchViewModel.facets.changeEntities = [];

        for (let item of changeEntities) {
            let newItem = {
                name: item,
                isSelected: false
            };

            for (let existingSelectedEntity of existingSelectedEntities) {
                if (item === existingSelectedEntity) {
                    newItem.isSelected = true;
                }
            }

            this.changeRequestSearchViewModel.facets.changeEntities.push(newItem);
        }

        this.changeEntityFacets = this.changeRequestSearchViewModel.facets.changeEntities.slice();
        //this.allChangeEntities this.change
    }

    updateRequestByFacet(requestsBy) {
        let existingSelectedRequestsBy = this.changeRequestSearchViewModel.facets.requestsBy.filter(item => item.isSelected).map(item => item.id);
        this.changeRequestSearchViewModel.facets.requestsBy = [];

        for (let item of requestsBy) {
            let newItem = {
                id: item,
                isSelected: false
            };

            for (let existingSelectedRequestedBy of existingSelectedRequestsBy) {
                if (item === existingSelectedRequestedBy) {
                    newItem.isSelected = true;
                }
            }

            this.changeRequestSearchViewModel.facets.requestsBy.push(newItem);
        }

        this.requestByFacets = this.changeRequestSearchViewModel.facets.requestsBy.slice();
    }

    updateAssigneeFacet(assignees) {
        let existingSelectedAssignees = this.changeRequestSearchViewModel.facets.assignees.filter(item => item.isSelected).map(item => item.id);
        this.changeRequestSearchViewModel.facets.assignees = [];

        for (let item of assignees) {
            let newItem = {
                id: item,
                isSelected: false
            };

            for (let existingSelectedAssignee of existingSelectedAssignees) {
                if (item === existingSelectedAssignee) {
                    newItem.isSelected = true;
                }
            }

            this.changeRequestSearchViewModel.facets.assignees.push(newItem);
        }

        this.assigneeFacets = this.changeRequestSearchViewModel.facets.assignees.slice();
    }
}
