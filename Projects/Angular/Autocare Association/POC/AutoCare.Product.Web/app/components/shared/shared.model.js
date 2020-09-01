"use strict";
// role model
(function (Role) {
    Role[Role["admin"] = 0] = "admin";
    Role[Role["researcher"] = 1] = "researcher";
    Role[Role["user"] = 2] = "user";
})(exports.Role || (exports.Role = {}));
var Role = exports.Role;
(function (SearchType) {
    SearchType[SearchType["None"] = 0] = "None";
    SearchType[SearchType["GeneralSearch"] = 1] = "GeneralSearch";
    SearchType[SearchType["SearchByConfigId"] = 2] = "SearchByConfigId";
})(exports.SearchType || (exports.SearchType = {}));
var SearchType = exports.SearchType;
