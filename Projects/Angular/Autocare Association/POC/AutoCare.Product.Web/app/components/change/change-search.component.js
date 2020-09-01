"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require("@angular/core");
var router_1 = require("@angular/router");
var ng2_bs3_modal_1 = require('ng2-bs3-modal/ng2-bs3-modal');
var change_search_service_1 = require("./change-search.service");
var shared_service_1 = require("../shared/shared.service");
var navigation_service_1 = require("../shared/navigation.service");
var change_search_model_1 = require("./change-search.model");
var change_service_1 = require("./change.service");
var user_service_1 = require("./user.service");
var ac_grid_1 = require('../../lib/aclibs/ac-grid/ac-grid');
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var changeRequestReview_model_1 = require("../changeRequestReview/changeRequestReview.model");
var constants_warehouse_1 = require("../constants-warehouse");
var userLikes_component_1 = require("../changeRequestReview/userLikes.component");
var ChangeSearchComponent = (function () {
    function ChangeSearchComponent(sharedService, router, changeSearchService, userService, toastr, changeService, navigationService) {
        this.sharedService = sharedService;
        this.router = router;
        this.changeSearchService = changeSearchService;
        this.userService = userService;
        this.toastr = toastr;
        this.changeService = changeService;
        this.navigationService = navigationService;
        this.selectedCR = [];
        this.affectedList = [];
        this.changeRequestReviews = [];
        this.reviewComment = { comment: "" };
        this.requestorComments = [];
        this.allChangeEntities = [];
        this.distinctCRChangeTypes = [];
        this.allSubmitsBy = [];
        this.usersForAssignment = [];
        this.allUsersForAssignment = [];
        this.showLoadingGif = false;
        //Required to disable buttons based upon selections
        this.disablePreliminary = true;
        this.disableApprove = true;
        this.disableReject = true;
        this.disableAssign = true;
        this.assignedReviewer = { assignedToUserId: "-1", changeRequestIds: [] };
        this.allLikedBy = [];
        this.thresholdRecordCount = 100;
        this.changeTypeList = [];
        this.refreshOnLoadFlag = false;
        this.searchOnLoadFlag = false;
        this.datepickerOpts = {
            startDate: new Date(1900, 1, 1),
            autoclose: true,
            todayBtn: 'linked',
            todayHighlight: true,
            assumeNearbyYear: true,
            format: 'D, d MM yyyy'
        };
    }
    ChangeSearchComponent.prototype.ngOnInit = function () {
        this.isSelectAllChangeRequests = false;
        var token = this.sharedService.getTokenModel();
        this.changeRequestSearchViewModel = {
            facets: { changeTypes: [], changeEntities: [], requestsBy: [], statuses: [], assignees: [] },
            result: { changeRequests: [] },
            canBulkSubmit: false, isAdmin: false,
            searchType: change_search_model_1.SearchType.None
        };
        if (this.sharedService.changeRequestSearchViewModel != null) {
            this.changeRequestSearchViewModel.facets = this.sharedService.changeRequestSearchViewModel.facets;
            this.assigneeFacets = this.changeRequestSearchViewModel.facets.assignees.slice();
            this.changeTypeFacets = this.changeRequestSearchViewModel.facets.changeTypes.slice();
            this.changeEntityFacets = this.changeRequestSearchViewModel.facets.changeEntities.slice();
            this.requestByFacets = this.changeRequestSearchViewModel.facets.requestsBy.slice();
            this.statusFacets = this.changeRequestSearchViewModel.facets.statuses.slice();
            if (this.sharedService.changeRequestSearchViewModel.searchType === change_search_model_1.SearchType.SearchByChangeRequestId) {
                this.selectedChangeRequestIdForSearch = this.sharedService.changeRequestSearchViewModel.result.changeRequests[0].id.toString();
                this.searchByChangeRequestId();
            }
            else if (this.sharedService.changeRequestSearchViewModel.searchType === change_search_model_1.SearchType.GeneralSearch) {
                this.searchChanges();
            }
            else {
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
    };
    ChangeSearchComponent.prototype.onSearchEntityChange = function () {
        var _this = this;
        if (this.changeRequestSearchViewModel.facets.changeEntities != null && this.entityQuery !== undefined && this.entityQuery !== null) {
            this.changeEntityFacets =
                this.changeRequestSearchViewModel.facets.changeEntities.filter(function (item) { return item.name.toLocaleLowerCase().indexOf(_this.entityQuery.toLocaleLowerCase()) >= 0; });
        }
    };
    ChangeSearchComponent.prototype.onSearchSubmittedChange = function () {
        var _this = this;
        if (this.changeRequestSearchViewModel.facets.requestsBy != null && this.submittedQuery !== undefined && this.submittedQuery !== null) {
            this.requestByFacets =
                this.changeRequestSearchViewModel.facets.requestsBy.filter(function (item) { return item.id.toLocaleLowerCase().indexOf(_this.submittedQuery.toLocaleLowerCase()) >= 0; });
        }
    };
    ChangeSearchComponent.prototype.onSearchAssigneeChange = function () {
        var _this = this;
        if (this.changeRequestSearchViewModel.facets.assignees != null && this.assigneeQuery !== undefined && this.assigneeQuery !== null) {
            this.assigneeFacets =
                this.changeRequestSearchViewModel.facets.assignees.filter(function (item) { return item.id.toLocaleLowerCase().indexOf(_this.assigneeQuery.toLocaleLowerCase()) >= 0; });
        }
    };
    ChangeSearchComponent.prototype.searchChanges = function (onPageLoadFlag) {
        var _this = this;
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
        var inputModel = this.getDefaultInputModel();
        if (this.changeRequestSearchViewModel.facets.changeTypes) {
            this.changeRequestSearchViewModel.facets.changeTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.changeTypes.push(m.name); });
        }
        if (this.changeRequestSearchViewModel.facets.changeEntities) {
            this.changeRequestSearchViewModel.facets.changeEntities.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.changeEntities.push(m.name); });
        }
        if (this.changeRequestSearchViewModel.facets.requestsBy) {
            this.changeRequestSearchViewModel.facets.requestsBy.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.requestsBy.push(m.id); });
        }
        if (this.changeRequestSearchViewModel.facets.assignees) {
            this.changeRequestSearchViewModel.facets.assignees.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.assignees.push(m.id); });
        }
        if (this.changeRequestSearchViewModel.facets.statuses) {
            this.changeRequestSearchViewModel.facets.statuses.filter(function (item) { return item.isSelected; })
                .forEach(function (s) { return inputModel.statuses.push(s.name); });
        }
        inputModel.submittedDateFrom = this.submittedDateFrom;
        inputModel.submittedDateTo = this.submittedDateTo;
        this.changeRequestSearchViewModel.searchType = change_search_model_1.SearchType.GeneralSearch;
        this.changeSearchService.search(inputModel).subscribe(function (m) {
            if (m.result) {
                _this.getSearchResult(m);
                _this.crStatusCheck();
                _this.searchOnLoadFlag = true;
                if (onPageLoadFlag) {
                    if (_this.refreshOnLoadFlag)
                        _this.showLoadingGif = false;
                }
                else {
                    _this.showLoadingGif = false;
                }
                $(".drawer-left").css('width', 15);
            }
            else {
                _this.toastr.warning("The search yeilded no result", "No Record Found!!");
                _this.showLoadingGif = false;
            }
        }, function (error) {
            _this.errorMessage = error;
            _this.showLoadingGif = false;
        });
    };
    ChangeSearchComponent.prototype.onCRIdKeyPress = function (event) {
        if (event.keyCode == 13) {
            this.searchByChangeRequestId();
        }
    };
    ChangeSearchComponent.prototype.searchByChangeRequestId = function () {
        var _this = this;
        var changeRequestId = Number(this.selectedChangeRequestIdForSearch);
        if (isNaN(changeRequestId)) {
            this.toastr.warning("Invalid Change request #", constants_warehouse_1.ConstantsWarehouse.validationTitle);
            return;
        }
        this.selectedCR = [];
        this.changeRequestSearchViewModel.searchType = change_search_model_1.SearchType.SearchByChangeRequestId;
        this.showLoadingGif = true;
        this.changeSearchService.searchbyChangeRequestId(this.selectedChangeRequestIdForSearch).subscribe(function (m) {
            if (m.result) {
                _this.getSearchResult(m);
                _this.showLoadingGif = false;
                $(".drawer-left").css('width', 15);
            }
            else {
                _this.toastr.warning("The search yeilded no result", "No Record Found!!");
                _this.showLoadingGif = false;
            }
        }, function (error) {
            _this.errorMessage = error;
            _this.showLoadingGif = false;
        });
    };
    ChangeSearchComponent.prototype.getSearchResult = function (m) {
        this.changeRequestSearchViewModel.result.changeRequests = m.result.changeRequests.map(function (x) {
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
    };
    ChangeSearchComponent.prototype.changeRequestReview = function (changeRequest) {
        this.sharedService.changeRequestSearchViewModel = this.changeRequestSearchViewModel;
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/" + changeRequest.entity.toLowerCase() + "/" + changeRequest.id;
        this.router.navigateByUrl(changeRequestLink);
    };
    ChangeSearchComponent.prototype.onSelectAllChangeRequest = function (event) {
        var _this = this;
        this.isSelectAllChangeRequests = event;
        if (this.changeRequestSearchViewModel.result.changeRequests == null) {
            return;
        }
        if (event) {
            this.changeRequestSearchViewModel.result.changeRequests.forEach(function (item) {
                item.isSelected = event;
                _this.selectedCR.push(item);
                _this.crStatusCheck();
            });
        }
        else {
            this.selectedCR = [];
            this.changeRequestSearchViewModel.result.changeRequests.forEach(function (item) {
                item.isSelected = event;
            });
            this.crStatusCheck();
        }
    };
    ChangeSearchComponent.prototype.crStatusChange = function (approvalType) {
        var _this = this;
        if (this.selectedCR.length > 0) {
            this.affectedList = [];
            this.changeRequestSearchViewModel.facets.changeEntities.forEach(function (item) {
                item.count = _this.selectedCR.filter(function (cr) { return cr.entity === item.name; }).length;
            });
            //Note: This code is to group the change types and get the distinct count
            this.distinctCRChangeTypes = [];
            //append changeType to a new array
            this.changeTypeList = this.changeRequestSearchViewModel.result.changeRequests.filter(function (x) { return x.isSelected; }).map(function (x) {
                return {
                    name: x.changeType
                };
            });
            //Group the change types and get their count
            var types = {}, i, j, currentType;
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
                this.changeService.getAssociatedCount(this.selectedCR).subscribe(function (m) {
                    Object.keys(m).forEach(function (x) {
                        _this.affectedList.push({
                            type: x.toString().substring(0, x.length - 5).toLocaleLowerCase(),
                            count: m[x.toString()]
                        });
                    });
                    _this.showLoadingGif = false;
                });
            }
            switch (approvalType) {
                case "approved":
                    //this.reviewComment.comment = "Bulk Approval";
                    this.changeRequestReviews = this.convertRequestToReview(this.selectedCR, changeRequestReview_model_1.ChangeRequestStatus.Approved);
                    this.approvePopup.open("md");
                    break;
                case "preliminaryapproved":
                    //this.reviewComment.comment = "Bulk Preliminary Approval";
                    this.changeRequestReviews = this.convertRequestToReview(this.selectedCR, changeRequestReview_model_1.ChangeRequestStatus.PreliminaryApproved);
                    this.preliminaryPopup.open("md");
                    break;
                case "rejected":
                    //this.reviewComment.comment = "Bulk Rejection";
                    this.changeRequestReviews = this.convertRequestToReview(this.selectedCR, changeRequestReview_model_1.ChangeRequestStatus.Rejected);
                    this.rejectPopup.open("md");
                    break;
                case "assign":
                    this.assignPopup.open("md");
                    break;
                default:
                    break;
            }
        }
    };
    ChangeSearchComponent.prototype.onConfirm = function (approvalType) {
        var _this = this;
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
                var assignedReviewerDetail = this.usersForAssignment.filter(function (x) { return x.id === _this.assignedReviewer.assignedToUserId; })[0];
                this.assignedReviewer.assignedToRoleId = assignedReviewerDetail.roleId;
                this.selectedCR.forEach(function (x) {
                    _this.assignedReviewer.changeRequestIds.push(x.id);
                });
                this.showLoadingGif = true;
                this.changeService.assignReviewer(this.assignedReviewer).subscribe(function (result) {
                    if (result) {
                        _this.toastr.success("Select Change Request were assigned successfully.", "Your review was assigned to " + assignedReviewerDetail.user);
                        _this.selectedCR.forEach(function (x) {
                            x.assignee = assignedReviewerDetail;
                        });
                        _this.selectedCR = [];
                        _this.changeRequestSearchViewModel.result.changeRequests.forEach(function (x) { return x.isSelected = false; });
                        _this.assignedReviewer.changeRequestIds = [];
                        _this.showLoadingGif = false;
                    }
                    else {
                        _this.toastr.warning("Select Change Request assignment failed.", "Your review was couldn't be submitted.");
                    }
                }, function (error) {
                    _this.errorMessage = error;
                    //console.log(this.errorMessage);
                });
                this.assignPopup.close();
                break;
            default:
                break;
        }
    };
    ChangeSearchComponent.prototype.bulkSubmit = function (approvalType) {
        var _this = this;
        this.showLoadingGif = true;
        var i = 0;
        var count = this.selectedCR.length;
        this.changeRequestReviews.forEach(function (cr) {
            var baseUrl = constants_warehouse_1.ConstantsWarehouse.api[Object.keys(constants_warehouse_1.ConstantsWarehouse.api).filter(function (x) { return x.toLowerCase() === cr.entity.toLowerCase(); }).toString()];
            var changeRequestReview = {
                changeRequestId: cr.changeRequestId,
                reviewStatus: cr.reviewStatus,
                reviewedBy: cr.reviewedBy,
                reviewComment: cr.reviewComment
            };
            var index = _this.changeRequestSearchViewModel.result.changeRequests.findIndex(function (x) { return x.id === cr.changeRequestId; });
            //var index = this.changeRequestSearchViewModel.result.changeRequests.indexOf(this.changeRequestSearchViewModel.result.changeRequests.filter(x => x.id === cr.changeRequestId)[0]);
            _this.changeService.selectedCRApproval(baseUrl, cr.changeRequestId, changeRequestReview).subscribe(function (response) {
                if (response == true) {
                    _this.toastr.success("You review for " + cr.entity + " submitted successfully.", "Change Request Id: " + cr.changeRequestId + " " + approvalType + " successfull!!!");
                    _this.changeRequestSearchViewModel.result.changeRequests[index].isSelected = false;
                    var status = _this.changeRequestSearchViewModel.facets.statuses.filter(function (x) { return x.name.toLowerCase() === approvalType.toLowerCase(); })[0];
                    _this.changeRequestSearchViewModel.result.changeRequests[index].status.status = status.name;
                    if (i++ >= count - 1)
                        _this.showLoadingGif = false;
                }
                else {
                    _this.toastr.warning("You review for " + cr.entity + " failed..", "Change Request Id: " + cr.changeRequestId + " " + approvalType + " failed!!!");
                    if (i++ >= count - 1)
                        _this.showLoadingGif = false;
                }
            }, function (error) {
                _this.toastr.warning("Error occured while reviewing Change Request for " + cr.entity + ".", "Change Request Id: " + cr.changeRequestId + " " + approvalType + " failed!!!");
                _this.showLoadingGif = false;
            });
        });
        this.selectedCR = [];
        this.changeRequestReviews = [];
        this.distinctCRChangeTypes = [];
        this.crStatusCheck();
        //this.searchChanges();
    };
    ChangeSearchComponent.prototype.crStatusCheck = function () {
        if (this.selectedCR.length > 0) {
            // canBulkSubmit is either admin or researcher
            if (this.changeRequestSearchViewModel.canBulkSubmit) {
                // for admin
                if (this.changeRequestSearchViewModel.isAdmin) {
                    if (this.selectedCR.every(function (x) { return x.status.status === "Submitted"; })) {
                        this.disableAssign = false;
                        this.disableApprove = false;
                        this.disablePreliminary = false;
                        this.disableReject = false;
                    }
                    else if (this.selectedCR.every(function (x) { return x.status.status === "PreliminaryApproved"; })) {
                        this.disableAssign = false;
                        this.disableApprove = false;
                        this.disablePreliminary = true;
                        this.disableReject = false;
                    }
                    else if (this.selectedCR.every(function (x) { return (x.status.status === "Submitted" || x.status.status === "PreliminaryApproved"); })) {
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
                    if (this.selectedCR.every(function (x) { return x.status.status === "Submitted"; })) {
                        this.disableAssign = false;
                        this.disablePreliminary = false;
                    }
                    else if (this.selectedCR.every(function (x) { return x.status.status === "PreliminaryApproved"; })) {
                        this.disableAssign = false;
                        this.disablePreliminary = true;
                    }
                    else if (this.selectedCR.every(function (x) { return (x.status.status === "Submitted" || x.status.status === "PreliminaryApproved"); })) {
                        this.disableAssign = false;
                        this.disablePreliminary = true;
                    }
                    else {
                        this.disableAssign = true;
                        this.disablePreliminary = true;
                    }
                }
            }
        }
        else {
            this.disableAssign = true;
            this.disableApprove = true;
            this.disablePreliminary = true;
            this.disableReject = true;
        }
    };
    ChangeSearchComponent.prototype.onCRSelected = function (changeRequest, event) {
        if (event.target.checked) {
            this.selectedCR.push(changeRequest);
            this.crStatusCheck();
        }
        else {
            this.selectedCR = this.selectedCR.filter(function (x) { return x.id !== changeRequest.id; });
            if (this.selectedCR.length > 0) {
                this.crStatusCheck();
            }
        }
        if (changeRequest.isSelected) {
            //unchecked
            this.isSelectAllChangeRequests = false;
        }
        else {
            //checked
            var excludedChangeRequests = this.changeRequestSearchViewModel.result.changeRequests.filter(function (item) { return item.id != changeRequest.id; });
            if (excludedChangeRequests.every(function (item) { return item.isSelected; })) {
                this.isSelectAllChangeRequests = true;
            }
        }
    };
    ChangeSearchComponent.prototype.convertRequestToReview = function (selectedCR, status) {
        var _this = this;
        var changeRequestList = [];
        selectedCR.forEach(function (x) {
            changeRequestList.push({
                changeRequestId: x.id,
                reviewStatus: status,
                reviewedBy: x.requestedBy.id,
                entity: x.entity,
                reviewComment: _this.reviewComment
            });
        });
        return changeRequestList;
    };
    ChangeSearchComponent.prototype.onClearFilters = function () {
        this.assigneeQuery = "";
        this.submittedQuery = "";
        this.entityQuery = "";
        this.selectedChangeRequestIdForSearch = "";
        this.submittedDateFrom = "";
        this.submittedDateTo = "";
        if (this.assigneeFacets.filter(function (item) { return item.isSelected; }).length > 0)
            this.changeRequestSearchViewModel.facets.assignees.forEach(function (x) { return x.isSelected = false; });
        if (this.changeTypeFacets.filter(function (item) { return item.isSelected; }).length > 0)
            this.changeRequestSearchViewModel.facets.changeTypes.forEach(function (x) { return x.isSelected = false; });
        if (this.changeEntityFacets.filter(function (item) { return item.isSelected; }).length > 0)
            this.changeRequestSearchViewModel.facets.changeEntities.forEach(function (x) { return x.isSelected = false; });
        if (this.requestByFacets.filter(function (item) { return item.isSelected; }).length > 0)
            this.changeRequestSearchViewModel.facets.requestsBy.forEach(function (x) { return x.isSelected = false; });
        if (this.statusFacets.filter(function (item) { return item.isSelected; }).length > 0)
            this.changeRequestSearchViewModel.facets.statuses.forEach(function (x) { return x.isSelected = false; });
        this.refreshFacets();
    };
    ChangeSearchComponent.prototype.clearApprovedStatusFilter = function (facet) {
        if (facet.filter(function (x) { return x.isSelected; }).length > 0) {
            this.changeRequestSearchViewModel.facets.statuses.forEach(function (x) { return x.isSelected = false; });
            this.refreshFacets();
        }
    };
    ChangeSearchComponent.prototype.clearChangeTypeFilter = function (facet) {
        if (facet.filter(function (x) { return x.isSelected; }).length > 0) {
            this.changeRequestSearchViewModel.facets.changeTypes.forEach(function (x) { return x.isSelected = false; });
            this.refreshFacets();
        }
    };
    ChangeSearchComponent.prototype.clearChangeEntityFilter = function (facet) {
        this.entityQuery = "";
        this.onSearchEntityChange();
        if (facet.filter(function (x) { return x.isSelected; }).length > 0) {
            this.changeRequestSearchViewModel.facets.changeEntities.forEach(function (x) { return x.isSelected = false; });
            this.refreshFacets();
        }
    };
    ChangeSearchComponent.prototype.clearAssignedToFilter = function (facet) {
        this.assigneeQuery = "";
        this.onSearchAssigneeChange();
        if (facet.filter(function (x) { return x.isSelected; }).length > 0) {
            this.changeRequestSearchViewModel.facets.assignees.forEach(function (x) { return x.isSelected = false; });
            this.refreshFacets();
        }
    };
    ChangeSearchComponent.prototype.clearSubmittedByFilter = function (facet) {
        this.submittedQuery = "";
        this.onSearchSubmittedChange();
        if (facet.filter(function (x) { return x.isSelected; }).length > 0) {
            this.changeRequestSearchViewModel.facets.requestsBy.forEach(function (x) { return x.isSelected = false; });
            this.refreshFacets();
        }
    };
    ChangeSearchComponent.prototype.showRequestorComment = function (id, status) {
        var _this = this;
        this.changeSearchService.getRequestorComment(id, status).subscribe(function (result) {
            _this.requestorComments = result;
            _this.commentPopupModel.open("md");
        });
    };
    ChangeSearchComponent.prototype.onClickLikes = function (id) {
        this.likeComponent.showAllLikedBy(id);
    };
    ChangeSearchComponent.prototype.onItemSelected = function (event, facet) {
        var isChecked = event.target.checked;
        var selectedItem = facet.filter(function (item) { return item.name.toLocaleLowerCase() === event.target.value.toLocaleLowerCase(); })[0];
        selectedItem.isSelected = isChecked;
        this.refreshFacets();
    };
    ChangeSearchComponent.prototype.onUserItemSelected = function (event, facet) {
        var isChecked = event.target.checked;
        var selectedItem = facet.filter(function (item) { return item.id.toLocaleLowerCase() === event.target.value.toLocaleLowerCase(); })[0];
        selectedItem.isSelected = isChecked;
        this.refreshFacets();
    };
    ChangeSearchComponent.prototype.getDefaultInputModel = function () {
        return {
            submittedDateFrom: this.submittedDateFrom,
            submittedDateTo: this.submittedDateTo,
            changeTypes: [],
            changeEntities: [],
            statuses: [],
            requestsBy: [],
            assignees: []
        };
    };
    ChangeSearchComponent.prototype.refreshFacets = function (onPageLoadFlag) {
        var _this = this;
        var inputModel = this.getDefaultInputModel();
        inputModel.submittedDateFrom = this.submittedDateFrom;
        inputModel.submittedDateTo = this.submittedDateTo;
        if (this.changeRequestSearchViewModel.facets.statuses) {
            this.changeRequestSearchViewModel.facets.statuses.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.statuses.push(m.name); });
        }
        if (this.changeRequestSearchViewModel.facets.changeTypes) {
            this.changeRequestSearchViewModel.facets.changeTypes.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.changeTypes.push(m.name); });
        }
        if (this.changeRequestSearchViewModel.facets.changeEntities) {
            this.changeRequestSearchViewModel.facets.changeEntities.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.changeEntities.push(m.name); });
        }
        if (this.changeRequestSearchViewModel.facets.requestsBy) {
            this.changeRequestSearchViewModel.facets.requestsBy.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.requestsBy.push(m.id); });
        }
        if (this.changeRequestSearchViewModel.facets.assignees) {
            this.changeRequestSearchViewModel.facets.assignees.filter(function (item) { return item.isSelected; })
                .forEach(function (m) { return inputModel.assignees.push(m.id); });
        }
        this.showLoadingGif = true;
        this.changeSearchService.refreshFacets(inputModel).subscribe(function (data) {
            _this.updateStatusFacet(data.facets.statuses);
            _this.updateTypeFacet(data.facets.changeTypes);
            _this.updateEntityFacet(data.facets.changeEntities);
            _this.updateRequestByFacet(data.facets.requestsBy);
            _this.updateAssigneeFacet(data.facets.assignees);
            _this.onSearchEntityChange();
            _this.onSearchSubmittedChange();
            _this.onSearchAssigneeChange();
            _this.refreshOnLoadFlag = true;
            if (onPageLoadFlag) {
                if (_this.searchOnLoadFlag)
                    _this.showLoadingGif = false;
            }
            else {
                _this.showLoadingGif = false;
            }
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    ChangeSearchComponent.prototype.updateStatusFacet = function (statuses) {
        var existingSelectedStatuses = this.changeRequestSearchViewModel.facets.statuses.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.changeRequestSearchViewModel.facets.statuses = [];
        for (var _i = 0, statuses_1 = statuses; _i < statuses_1.length; _i++) {
            var item = statuses_1[_i];
            var newItem = {
                name: item,
                isSelected: false
            };
            for (var _a = 0, existingSelectedStatuses_1 = existingSelectedStatuses; _a < existingSelectedStatuses_1.length; _a++) {
                var existingSelectedStatus = existingSelectedStatuses_1[_a];
                if (item === existingSelectedStatus) {
                    newItem.isSelected = true;
                }
            }
            this.changeRequestSearchViewModel.facets.statuses.push(newItem);
        }
        this.statusFacets = this.changeRequestSearchViewModel.facets.statuses.slice();
    };
    ChangeSearchComponent.prototype.updateTypeFacet = function (changeTypes) {
        var existingSelectedTypes = this.changeRequestSearchViewModel.facets.changeTypes.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.changeRequestSearchViewModel.facets.changeTypes = [];
        for (var _i = 0, changeTypes_1 = changeTypes; _i < changeTypes_1.length; _i++) {
            var item = changeTypes_1[_i];
            var newItem = {
                name: item,
                isSelected: false
            };
            for (var _a = 0, existingSelectedTypes_1 = existingSelectedTypes; _a < existingSelectedTypes_1.length; _a++) {
                var existingSelectedType = existingSelectedTypes_1[_a];
                if (item === existingSelectedType) {
                    newItem.isSelected = true;
                }
            }
            this.changeRequestSearchViewModel.facets.changeTypes.push(newItem);
        }
        this.changeTypeFacets = this.changeRequestSearchViewModel.facets.changeTypes.slice();
    };
    ChangeSearchComponent.prototype.updateEntityFacet = function (changeEntities) {
        var existingSelectedEntities = this.changeRequestSearchViewModel.facets.changeEntities.filter(function (item) { return item.isSelected; }).map(function (item) { return item.name; });
        this.changeRequestSearchViewModel.facets.changeEntities = [];
        for (var _i = 0, changeEntities_1 = changeEntities; _i < changeEntities_1.length; _i++) {
            var item = changeEntities_1[_i];
            var newItem = {
                name: item,
                isSelected: false
            };
            for (var _a = 0, existingSelectedEntities_1 = existingSelectedEntities; _a < existingSelectedEntities_1.length; _a++) {
                var existingSelectedEntity = existingSelectedEntities_1[_a];
                if (item === existingSelectedEntity) {
                    newItem.isSelected = true;
                }
            }
            this.changeRequestSearchViewModel.facets.changeEntities.push(newItem);
        }
        this.changeEntityFacets = this.changeRequestSearchViewModel.facets.changeEntities.slice();
        //this.allChangeEntities this.change
    };
    ChangeSearchComponent.prototype.updateRequestByFacet = function (requestsBy) {
        var existingSelectedRequestsBy = this.changeRequestSearchViewModel.facets.requestsBy.filter(function (item) { return item.isSelected; }).map(function (item) { return item.id; });
        this.changeRequestSearchViewModel.facets.requestsBy = [];
        for (var _i = 0, requestsBy_1 = requestsBy; _i < requestsBy_1.length; _i++) {
            var item = requestsBy_1[_i];
            var newItem = {
                id: item,
                isSelected: false
            };
            for (var _a = 0, existingSelectedRequestsBy_1 = existingSelectedRequestsBy; _a < existingSelectedRequestsBy_1.length; _a++) {
                var existingSelectedRequestedBy = existingSelectedRequestsBy_1[_a];
                if (item === existingSelectedRequestedBy) {
                    newItem.isSelected = true;
                }
            }
            this.changeRequestSearchViewModel.facets.requestsBy.push(newItem);
        }
        this.requestByFacets = this.changeRequestSearchViewModel.facets.requestsBy.slice();
    };
    ChangeSearchComponent.prototype.updateAssigneeFacet = function (assignees) {
        var existingSelectedAssignees = this.changeRequestSearchViewModel.facets.assignees.filter(function (item) { return item.isSelected; }).map(function (item) { return item.id; });
        this.changeRequestSearchViewModel.facets.assignees = [];
        for (var _i = 0, assignees_1 = assignees; _i < assignees_1.length; _i++) {
            var item = assignees_1[_i];
            var newItem = {
                id: item,
                isSelected: false
            };
            for (var _a = 0, existingSelectedAssignees_1 = existingSelectedAssignees; _a < existingSelectedAssignees_1.length; _a++) {
                var existingSelectedAssignee = existingSelectedAssignees_1[_a];
                if (item === existingSelectedAssignee) {
                    newItem.isSelected = true;
                }
            }
            this.changeRequestSearchViewModel.facets.assignees.push(newItem);
        }
        this.assigneeFacets = this.changeRequestSearchViewModel.facets.assignees.slice();
    };
    __decorate([
        core_1.ViewChild('RejectPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], ChangeSearchComponent.prototype, "rejectPopup", void 0);
    __decorate([
        core_1.ViewChild('PreliminaryPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], ChangeSearchComponent.prototype, "preliminaryPopup", void 0);
    __decorate([
        core_1.ViewChild('ApprovePopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], ChangeSearchComponent.prototype, "approvePopup", void 0);
    __decorate([
        core_1.ViewChild('AssignPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], ChangeSearchComponent.prototype, "assignPopup", void 0);
    __decorate([
        core_1.ViewChild('commentPopupModel'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], ChangeSearchComponent.prototype, "commentPopupModel", void 0);
    __decorate([
        core_1.ViewChild('likedByPopupModel'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], ChangeSearchComponent.prototype, "likedByPopupModel", void 0);
    __decorate([
        core_1.ViewChild("changeRequestGrid"), 
        __metadata('design:type', ac_grid_1.AcGridComponent)
    ], ChangeSearchComponent.prototype, "changeRequestGrid", void 0);
    __decorate([
        core_1.ViewChild("likeComponent"), 
        __metadata('design:type', userLikes_component_1.UserLikesComponent)
    ], ChangeSearchComponent.prototype, "likeComponent", void 0);
    ChangeSearchComponent = __decorate([
        core_1.Component({
            selector: 'changeSearch-comp',
            templateUrl: 'app/templates/change/change-search.component.html',
            providers: [change_search_service_1.ChangeSearchService, user_service_1.UserService, change_service_1.ChangeService]
        }), 
        __metadata('design:paramtypes', [shared_service_1.SharedService, router_1.Router, change_search_service_1.ChangeSearchService, user_service_1.UserService, ng2_toastr_1.ToastsManager, change_service_1.ChangeService, navigation_service_1.NavigationService])
    ], ChangeSearchComponent);
    return ChangeSearchComponent;
}());
exports.ChangeSearchComponent = ChangeSearchComponent;
