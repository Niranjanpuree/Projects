"use strict";
(function (SearchType) {
    SearchType[SearchType["None"] = 0] = "None";
    SearchType[SearchType["GeneralSearch"] = 1] = "GeneralSearch";
    SearchType[SearchType["SearchByChangeRequestId"] = 2] = "SearchByChangeRequestId";
})(exports.SearchType || (exports.SearchType = {}));
var SearchType = exports.SearchType;
