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
var core_1 = require('@angular/core');
var router_1 = require('@angular/router');
var authentication_service_1 = require('./authentication.service');
var jwtHelper_1 = require('./jwtHelper');
var shared_service_1 = require("./shared/shared.service");
var AuthorizeService = (function () {
    function AuthorizeService(authenticationService, sharedService, router) {
        this.token = null;
        this.authenticationService = authenticationService;
        this.router = router;
        this._jwtHelper = new jwtHelper_1.JwtHelper();
        this._sharedService = sharedService;
    }
    AuthorizeService.prototype.canActivate = function (route, state) {
        var _this = this;
        if (this.authenticationService.isLoggedIn()) {
            return true;
        }
        this.authenticationService.getToken().subscribe(function (tokenValue) {
            _this.token = tokenValue;
            if (_this.token) {
                try {
                    if (_this._jwtHelper.decodeToken(_this.token) &&
                        !_this._jwtHelper.isTokenExpired(_this.token)) {
                        sessionStorage.setItem('jwtToken', _this.token);
                        // set token on shared.service
                        _this._sharedService.setTokenModel(_this._jwtHelper.decodeToken(_this.token));
                        //check if ie
                        if ((/MSIE 9/i.test(navigator.userAgent) || (/MSIE 10/i.test(navigator.userAgent)) || /rv:11.0/i.test(navigator.userAgent))) {
                            location.href = "dashboard";
                        }
                        else {
                            _this.router.navigateByUrl('dashboard');
                        }
                        return;
                    }
                }
                catch (e) {
                    throw e;
                }
            }
            // Store the attempted URL for redirecting
            _this.authenticationService.redirectUrl = state.url;
            // Navigate to the login page
            _this.authenticationService.login();
            //this.router.navigate(['account/login']);
        });
        return false;
    };
    AuthorizeService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [authentication_service_1.AuthenticationService, shared_service_1.SharedService, router_1.Router])
    ], AuthorizeService);
    return AuthorizeService;
}());
exports.AuthorizeService = AuthorizeService;
