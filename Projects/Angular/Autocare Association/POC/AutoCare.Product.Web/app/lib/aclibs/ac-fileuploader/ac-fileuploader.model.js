"use strict";
var AcFile = (function () {
    function AcFile() {
        this.fileSize = 0;
        this.fileStatus = exports.fileStatusEnum.pending;
        this.percentage = 0;
        this.progress = 0 + "% 0";
        this.canDelete = false;
    }
    return AcFile;
}());
exports.AcFile = AcFile;
exports.fileStatusEnum = {
    pending: 0,
    uploaded: 1,
    failed: 2,
    deleted: 3
};
