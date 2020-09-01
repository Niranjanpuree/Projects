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
var shared_service_1 = require('../shared/shared.service');
var UserService = (function () {
    function UserService(sharedService) {
        this.sharedService = sharedService;
    }
    UserService.prototype.getSubmittedBy = function () {
        var userList = [];
        userList = this.pushDefaultUsers();
        return userList;
    };
    UserService.prototype.getUsersForAssignment = function () {
        var userList = [];
        var token = this.sharedService.getTokenModel();
        userList = this.pushDefaultUsers();
        return userList.filter(function (x) { return (x.id !== token.customer_id && x.roleId !== "3"); });
    };
    UserService.prototype.getAllUserForAssignment = function () {
        var userList = [];
        userList = this.pushDefaultUsers();
        return userList;
    };
    UserService.prototype.pushDefaultUsers = function () {
        var userList = [];
        userList.push({
            id: "admin@autocare.com",
            user: "Admin",
            roleId: "1",
            isSelected: false
        });
        userList.push({
            id: "researcher@autocare.com",
            user: "Researcher 1",
            roleId: "2",
            isSelected: false
        });
        userList.push({
            id: "researcher2@autocare.com",
            user: "Researcher 2",
            roleId: "2",
            isSelected: false
        });
        userList.push({
            id: "user@autocare.com",
            user: "User",
            roleId: "3",
            isSelected: false
        });
        userList.push({
            id: "user2@autocare.com",
            user: "User 2",
            roleId: "3",
            isSelected: false
        });
        return userList;
    };
    UserService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [shared_service_1.SharedService])
    ], UserService);
    return UserService;
}());
exports.UserService = UserService;
