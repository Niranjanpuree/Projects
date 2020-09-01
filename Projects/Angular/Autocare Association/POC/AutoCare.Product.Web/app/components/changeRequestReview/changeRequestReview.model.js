"use strict";
(function (ChangeRequestStatus) {
    ChangeRequestStatus[ChangeRequestStatus["Submitted"] = 0] = "Submitted";
    ChangeRequestStatus[ChangeRequestStatus["Deleted"] = 1] = "Deleted";
    ChangeRequestStatus[ChangeRequestStatus["PreliminaryApproved"] = 2] = "PreliminaryApproved";
    ChangeRequestStatus[ChangeRequestStatus["Approved"] = 3] = "Approved";
    ChangeRequestStatus[ChangeRequestStatus["Rejected"] = 4] = "Rejected";
})(exports.ChangeRequestStatus || (exports.ChangeRequestStatus = {}));
var ChangeRequestStatus = exports.ChangeRequestStatus;
