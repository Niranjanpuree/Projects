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
var httpHelper_1 = require('./httpHelper');
var jwtHelper_1 = require('./jwtHelper');
var compiler_1 = require('@angular/compiler');
var AuthenticationService = (function () {
    function AuthenticationService(_httpHelper, _runtimeCompiler) {
        this._httpHelper = _httpHelper;
        this._runtimeCompiler = _runtimeCompiler;
        this._jwtHelper = new jwtHelper_1.JwtHelper();
        this._jwtHelper = new jwtHelper_1.JwtHelper();
        this._runtimeCompiler.clearCache();
    }
    AuthenticationService.prototype.getToken = function () {
        //jQuery.ajax({
        //    url: 'http://localhost:60797/account/gettoken',
        //    success: function (result) {
        //        debugger;
        //        if (result.isOk == false) alert(result.message);
        //    },
        //    async: false
        //});
        return this._httpHelper.getToken('account/gettoken');
    };
    AuthenticationService.prototype.login = function () {
        location.href = 'account/login?ReturnUrl=' + window.btoa(this.redirectUrl);
    };
    AuthenticationService.prototype.logout = function () {
        sessionStorage.removeItem('jwtToken');
        //this._httpHelper.post('/Account/LogOff', null);
        //location.href = 'account/logoff?ReturnUrl=' + window.btoa('account/login');
    };
    AuthenticationService.prototype.isLoggedIn = function () {
        var token = sessionStorage.getItem('jwtToken');
        if (!token) {
            return false;
        }
        if (this._jwtHelper.isTokenExpired(token)) {
            sessionStorage.removeItem('jwtToken');
            return false;
        }
        return true;
    };
    AuthenticationService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [httpHelper_1.HttpHelper, compiler_1.RuntimeCompiler])
    ], AuthenticationService);
    return AuthenticationService;
}());
exports.AuthenticationService = AuthenticationService;
