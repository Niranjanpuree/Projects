"use strict";
(function (SearchType) {
    SearchType[SearchType["None"] = 0] = "None";
    SearchType[SearchType["GeneralSearch"] = 1] = "GeneralSearch";
    SearchType[SearchType["SearchByDriveTypeId"] = 2] = "SearchByDriveTypeId";
})(exports.SearchType || (exports.SearchType = {}));
var SearchType = exports.SearchType;
