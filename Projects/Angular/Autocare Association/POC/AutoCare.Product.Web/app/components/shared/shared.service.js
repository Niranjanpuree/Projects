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
var jwtHelper_1 = require('../jwtHelper');
var shared_model_1 = require('./shared.model');
var SharedService = (function () {
    function SharedService() {
        this.referenceDataActiveSubMenuGroupSelected = '';
        this._jwtHelper = new jwtHelper_1.JwtHelper();
        this._identityInfo = { customerId: "", email: "", isAdmin: false, isRequestor: false, isResearcher: false };
        //For Submenu accroding to mainMenuSelected
        this.selectedMenuHeaderItem = { selectedMainHeaderMenuItem: "VCDB" };
    }
    // getter and setter for token: ITokenModel
    SharedService.prototype.getTokenModel = function () {
        // clear token
        this._token = null;
        // note: if user directly access url
        if (!this._token) {
            var sessionToken = sessionStorage.getItem("jwtToken");
            if (sessionToken) {
                this._token = this._jwtHelper.decodeToken(sessionToken);
            }
        }
        // todo: check session expire, this is don't when calling canActivate at authorize.service
        return this._token;
    };
    SharedService.prototype.setTokenModel = function (token) {
        this._token = token;
    };
    SharedService.prototype.getIdentityInfo = function (submittedBy) {
        var token = this.getTokenModel();
        // clear _identityInfo
        this._identityInfo = { customerId: "", email: "", isAdmin: false, isRequestor: false, isResearcher: false };
        if (token) {
            this._identityInfo.customerId = token.customer_id;
            this._identityInfo.email = token.email;
            // note: assumption: that admin/ researcher/ user all can be requestor.
            // if requestor, then this is precedence.
            // if current user == stagingItem.submittedby then requestor else visitor
            if (submittedBy == token.customer_id) {
                this._identityInfo.isRequestor = true;
            }
            //if (!this._identityInfo.isRequestor) {
            switch (token.role) {
                case (shared_model_1.Role[shared_model_1.Role.admin]):
                    this._identityInfo.isAdmin = true;
                    break;
                case (shared_model_1.Role[shared_model_1.Role.researcher]):
                    this._identityInfo.isResearcher = true;
                    break;
                case (shared_model_1.Role[shared_model_1.Role.user]):
                    // if current user == stagingItem.submittedby then requestor else visitor
                    if (submittedBy == token.customer_id) {
                        this._identityInfo.isRequestor = true;
                    }
                    break;
                default:
                    break;
            }
        }
        return this._identityInfo;
    };
    // clone method : start
    SharedService.prototype.clone = function (source) {
        var result = source, i, len;
        if (!source
            || source instanceof Number
            || source instanceof String
            || source instanceof Boolean) {
            return result;
        }
        else if (Object.prototype.toString.call(source).slice(8, -1) === 'Array') {
            result = [];
            var resultLen = 0;
            for (i = 0, len = source.length; i < len; i++) {
                result[resultLen++] = this.clone(source[i]);
            }
        }
        else if (typeof source == 'object') {
            result = {};
            for (i in source) {
                if (source.hasOwnProperty(i)) {
                    result[i] = this.clone(source[i]);
                }
            }
        }
        return result;
    };
    ;
    SharedService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [])
    ], SharedService);
    return SharedService;
}());
exports.SharedService = SharedService;
