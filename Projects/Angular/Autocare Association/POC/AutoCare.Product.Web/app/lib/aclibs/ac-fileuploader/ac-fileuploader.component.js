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
var http_1 = require('@angular/http');
require('rxjs/Rx');
var Observable_1 = require('rxjs/Observable');
var ac_fileuploader_model_1 = require('./ac-fileuploader.model');
var AcFileUploader = (function () {
    function AcFileUploader(http) {
        this.http = http;
        this.totalSizeLimit = 10485760; //default: 10MB
        this.acceptedFileTypes = ".xls,.xlsx, .doc, .docx, .pdf, .jpg, .png, .txt, .jpeg";
        this.maxChunkSize = 256 * 1024; //Default: 256KB
        this.multifileBrowser = true;
        this.showUploadButton = false;
        this.showDeleteButton = false;
        this.maxRetryCount = 3;
        this.retryAfter = 5;
        this.existingFiles = [];
        this.canAttach = true;
        //If this flag is false, then file will not be stored in the Azure until manually calling commit to putblocklist
        this.autoCommitEnabled = false;
        //Emits file information for uploaded files. Invoked for each file upload
        this.fileUploadCompletedEvent = new core_1.EventEmitter();
        //Emits the event with list of files that are not in accepted file types
        this.unsupportedAttachmentsFoundEvent = new core_1.EventEmitter();
        //Emits the event with list of files that are not accepted because the total size reached threshold
        this.maxAttachmentSizeReachedEvent = new core_1.EventEmitter();
        this.acFiles = [];
        this.deletedFiles = [];
        this.maxSizeLimit = 10485760; //10MB
        this.filesSizeSoFar = 0;
        this.filesToBeCancelled = [];
        this.fileStatusEnum = ac_fileuploader_model_1.fileStatusEnum;
        this.fileInputId = 'id-' + Math.random().toString(36).substr(2, 16);
    }
    AcFileUploader.prototype.ngOnInit = function () {
        if (this.canAttach === true) {
            if (!this.uploadApiPath || this.uploadApiPath === "") {
                throw Error("Api path required to upload the file");
            }
        }
        if (this.existingFiles) {
            this.acFiles = this.existingFiles.slice();
            for (var _i = 0, _a = this.acFiles; _i < _a.length; _i++) {
                var file = _a[_i];
                file.fileStatus = this.fileStatusEnum.uploaded;
                file.formattedSize = this.formatBytes(file.fileSize, 2);
            }
        }
    };
    // wrapper function to set existingfiles.
    AcFileUploader.prototype.setAcFiles = function (paramFiles) {
        if (paramFiles) {
            this.acFiles = paramFiles;
        }
        else {
            this.acFiles = this.existingFiles; //.slice();
        }
    };
    AcFileUploader.prototype.reset = function (shouldDeleteTempContainer) {
        this.acFiles = [];
        this.filesSizeSoFar = 0;
        this.filesToBeCancelled = [];
        if (shouldDeleteTempContainer !== false) {
            this.deleteTempContainer();
        }
        this.tempContainerPath = "";
    };
    AcFileUploader.prototype.getAcFiles = function () {
        var _this = this;
        // loop through this.acfiles
        var cloneAcFiles = [];
        this.acFiles.forEach(function (item) {
            var itemAcFile = _this.clone(item);
            var file = item.file;
            itemAcFile.file = file;
            cloneAcFiles.push(itemAcFile);
        });
        return cloneAcFiles;
    };
    AcFileUploader.prototype.clone = function (source) {
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
    AcFileUploader.prototype.cleanupAllTempContainers = function () {
        var _this = this;
        return Observable_1.Observable.create(function (observer) {
            if (!_this.acFiles || _this.acFiles.length == 0) {
                observer.next(true);
                observer.complete();
                return;
            }
            var tempContainers = [];
            for (var _i = 0, _a = _this.acFiles; _i < _a.length; _i++) {
                var acFile = _a[_i];
                if (acFile.tempContainerName) {
                    tempContainers.push(acFile.tempContainerName);
                }
            }
            var index = 0;
            var cleanupCompleted = false;
            var deleteNextTempContainer = function (tempContainerName) {
                this.http.post(this.deleteApiPath, JSON.stringify({ containerName: tempContainerName }), new http_1.RequestOptions({ headers: new http_1.Headers({ 'Content-Type': 'application/json' }) }))
                    .map(function (res) {
                    ++index;
                    if (index < tempContainers.length) {
                        deleteNextTempContainer(tempContainers[index]);
                    }
                    else {
                        cleanupCompleted = true;
                    }
                })
                    .subscribe();
            };
            var checkCleanupCompleted = function () {
                if (cleanupCompleted) {
                    observer.next(true);
                    observer.complete();
                    return;
                }
                setTimeout(checkCleanupCompleted, 100);
            };
            checkCleanupCompleted();
        });
    };
    AcFileUploader.prototype.deleteTempContainer = function () {
        if (this.tempContainerPath) {
            this.http.post(this.deleteApiPath, JSON.stringify({ containerName: this.tempContainerPath }), new http_1.RequestOptions({ headers: new http_1.Headers({ 'Content-Type': 'application/json' }) }))
                .map(function (res) { })
                .subscribe();
        }
    };
    AcFileUploader.prototype.uploadAttachments = function () {
        var _this = this;
        return Observable_1.Observable.create(function (observer) {
            try {
                _this.uploadFiles();
                var self_1 = _this;
                var checkFileUploadCompleted_1 = function () {
                    if (!self_1.getNextFileToUpload()) {
                        var uploadedFiles = self_1.acFiles.filter(function (file) { return file.fileStatus == self_1.fileStatusEnum.uploaded; });
                        //let uploadedFileDetail = [];
                        //for (let uploadedFile of uploadedFiles) {
                        //    uploadedFileDetail.push({
                        //        fileName: uploadedFile.fileName,
                        //        fileSize: uploadedFile.fileSize,
                        //        formattedSize: uploadedFile.formattedSize,
                        //        contentType: uploadedFile.contentType,
                        //        fileExtension: uploadedFile.fileExtension,
                        //        containerName: uploadedFile.containerName,
                        //        directoryPath: uploadedFile.directoryPath,
                        //        chunksIdList: uploadedFile.chunksIdList,
                        //        fileStatus: uploadedFile.fileStatus,
                        //        fileUri: uploadedFile.fileUri,
                        //        attachedBy: uploadedFile.attachedBy,
                        //    });
                        //}
                        observer.next(uploadedFiles);
                        observer.complete();
                    }
                    else {
                        setTimeout(checkFileUploadCompleted_1, 100);
                    }
                };
                checkFileUploadCompleted_1();
            }
            catch (e) {
                observer.error(e);
            }
        });
    };
    AcFileUploader.prototype.deleteAttachments = function () {
        var _this = this;
        return Observable_1.Observable.create(function (observer) {
            try {
                _this.deleteFiles();
                var self_2 = _this;
                var checkFileDeleteCompleted_1 = function () {
                    var filesMarkedToBeDeleted = self_2.acFiles.filter(function (file) { return file.fileStatus == self_2.fileStatusEnum.deleted; });
                    if (!filesMarkedToBeDeleted || filesMarkedToBeDeleted.length == 0) {
                        observer.next(self_2.deletedFiles);
                        observer.complete();
                        return;
                    }
                    else {
                        setTimeout(checkFileDeleteCompleted_1, 100);
                    }
                };
                checkFileDeleteCompleted_1();
            }
            catch (e) {
                observer.error(e);
            }
        });
    };
    AcFileUploader.prototype.getFilesMarkedToDelete = function () {
        var _this = this;
        var filesToBeDeleted = this.acFiles.filter(function (file) { return file.fileStatus == _this.fileStatusEnum.deleted; });
        //let filesToBeDeleted = [];
        //for (let fileToBeDeleted of filesMarkedToBeDeleted) {
        //    filesToBeDeleted.push({
        //        fileName: fileToBeDeleted.fileName,
        //        fileSize: fileToBeDeleted.fileSize,
        //        formattedSize: fileToBeDeleted.formattedSize,
        //        contentType: fileToBeDeleted.contentType,
        //        fileExtension: fileToBeDeleted.fileExtension,
        //        containerName: fileToBeDeleted.containerName,
        //        directoryPath: fileToBeDeleted.directoryPath,
        //        chunksIdList: fileToBeDeleted.chunksIdList,
        //        fileStatus: fileToBeDeleted.fileStatus,
        //        fileUri: fileToBeDeleted.fileUri,
        //        attachedBy: fileToBeDeleted.attachedBy,
        //    });
        //}
        return filesToBeDeleted;
    };
    AcFileUploader.prototype.deleteFiles = function () {
        var _this = this;
        var filesMarkedToBeDeleted = this.acFiles.filter(function (file) { return file.fileStatus == _this.fileStatusEnum.deleted; });
        if (!filesMarkedToBeDeleted || filesMarkedToBeDeleted.length == 0) {
            return;
        }
        var filesToBeDeleted = [];
        for (var _i = 0, filesMarkedToBeDeleted_1 = filesMarkedToBeDeleted; _i < filesMarkedToBeDeleted_1.length; _i++) {
            var file = filesMarkedToBeDeleted_1[_i];
            filesToBeDeleted.push({
                containerName: file.containerName,
                fileName: file.fileName,
                directoryPath: file.directoryPath
            });
        }
        var self = this;
        this.http.post(this.deleteApiPath, JSON.stringify(filesToBeDeleted), new http_1.RequestOptions({ headers: new http_1.Headers({ 'Content-Type': 'application/json' }) }))
            .map(function (res) {
            for (var _i = 0, filesMarkedToBeDeleted_2 = filesMarkedToBeDeleted; _i < filesMarkedToBeDeleted_2.length; _i++) {
                var file = filesMarkedToBeDeleted_2[_i];
                var index = self.acFiles.indexOf(file);
                if (index != -1) {
                    self.acFiles.splice(index, 1);
                    self.deletedFiles.push(file);
                }
                file.fileStatus = self.fileStatusEnum.deleted;
            }
        })
            .subscribe();
    };
    AcFileUploader.prototype.uploadFiles = function () {
        if (!this.acFiles || this.acFiles.length == 0) {
            return;
        }
        var self = this;
        var uploadFile = function () {
            if (self.acFiles.length == 0) {
                return;
            }
            var fileToBeUploaded = self.getNextFileToUpload();
            if (!fileToBeUploaded) {
                return;
            }
            self.sendFile(fileToBeUploaded).subscribe(function (uploaded) {
                if (uploaded) {
                    fileToBeUploaded.fileStatus = ac_fileuploader_model_1.fileStatusEnum.uploaded;
                    fileToBeUploaded.containerName = self.tempContainerPath;
                    fileToBeUploaded.tempContainerName = self.tempContainerPath;
                    var nextFileToBeUploaded = self.getNextFileToUpload();
                    if (!nextFileToBeUploaded) {
                        return;
                    }
                    uploadFile();
                }
            }, function (error) {
                fileToBeUploaded.fileStatus = ac_fileuploader_model_1.fileStatusEnum.failed;
                var nextFileToBeUploaded = self.getNextFileToUpload();
                if (!nextFileToBeUploaded) {
                    return;
                }
            });
        };
        uploadFile();
    };
    AcFileUploader.prototype.getNextFileToUpload = function () {
        if (!this.acFiles || this.acFiles.length == 0) {
            return null;
        }
        var pendingFiles = this.acFiles.filter(function (file) {
            return file.fileStatus === ac_fileuploader_model_1.fileStatusEnum.pending;
        });
        if (!pendingFiles || pendingFiles.length == 0) {
            return null;
        }
        return pendingFiles[0];
    };
    AcFileUploader.prototype.onFilesSelected = function (e) {
        var selectedFiles = e.target.files;
        if (!selectedFiles || selectedFiles.length == 0) {
            return;
        }
        var sizeExceededFiles = "";
        var notAcceptedFiles = "";
        var _loop_1 = function() {
            var currentFile = selectedFiles.item(i);
            var duplicateFile = this_1.acFiles.filter(function (acFile) { return acFile.fileName == currentFile.name; })[0];
            if (!duplicateFile) {
                var fileExtn = currentFile.name.split('.').pop();
                if (this_1.acceptedFileTypes.indexOf(fileExtn) == -1) {
                    if (notAcceptedFiles) {
                        notAcceptedFiles += ", ";
                    }
                    notAcceptedFiles += currentFile.name;
                }
                else if (this_1.filesSizeSoFar + currentFile.size > this_1.maxSizeLimit) {
                    if (sizeExceededFiles) {
                        sizeExceededFiles += ", ";
                    }
                    sizeExceededFiles += currentFile.name;
                }
                else {
                    this_1.filesSizeSoFar += currentFile.size;
                    this_1.acFiles.push({
                        file: currentFile,
                        fileName: currentFile.name,
                        fileSize: currentFile.size,
                        formattedSize: this_1.formatBytes(currentFile.size, 2),
                        contentType: currentFile.type,
                        fileExtension: fileExtn,
                        fileStatus: ac_fileuploader_model_1.fileStatusEnum.pending,
                        canDelete: true,
                    });
                }
            }
        };
        var this_1 = this;
        for (var i = 0; i < selectedFiles.length; i++) {
            _loop_1();
        }
        if (sizeExceededFiles) {
            this.maxAttachmentSizeReachedEvent.emit(sizeExceededFiles);
        }
        if (notAcceptedFiles) {
            this.unsupportedAttachmentsFoundEvent.emit(notAcceptedFiles);
        }
        e.target.value = "";
    };
    AcFileUploader.prototype.removeAttachment = function (file) {
        if (!this.acFiles || this.acFiles.length == 0) {
            return;
        }
        var fileToBeRemoved = this.acFiles.filter(function (x) { return x.fileName == file.fileName; })[0];
        fileToBeRemoved.fileStatus = ac_fileuploader_model_1.fileStatusEnum.deleted;
    };
    AcFileUploader.prototype.cancelUpload = function (file) {
        if (!this.acFiles || this.acFiles.length == 0) {
            return;
        }
        var fileToBeRemoved = this.acFiles.filter(function (x) { return x.fileName == file.fileName; })[0];
        var index = this.acFiles.indexOf(fileToBeRemoved);
        if (index != -1) {
            this.acFiles.splice(index, 1);
            this.filesToBeCancelled.push(fileToBeRemoved);
        }
    };
    AcFileUploader.prototype.sendFile = function (acFile) {
        var _this = this;
        return Observable_1.Observable.create(function (observer) {
            if (!acFile) {
                observer.next(false);
            }
            var self = _this;
            var start = 0;
            var end = Math.min(_this.maxChunkSize, acFile.fileSize);
            var chunkIdIncrementor = 1;
            var retryCount = 0;
            var isLastChunk = acFile.fileSize <= _this.maxChunkSize;
            var chunkIdList = "";
            var requiredChunkIdDigits = 4;
            if (acFile.fileSize > _this.maxChunkSize) {
                var expectedLength = String(Math.round(acFile.fileSize / _this.maxChunkSize)).length;
                if (expectedLength > requiredChunkIdDigits) {
                    requiredChunkIdDigits = (Math.floor(expectedLength / 4) * 4) +
                        ((expectedLength % 4) > 0 ? 4 : 0);
                }
            }
            var fileChunk;
            //let httpRequestOptions = new RequestOptions({ headers: new Headers({ 'Content-Type': 'multipart/form-data' }) });
            var uploadedFileSize = 0;
            var sendNextChunk = function () {
                var chunkId = self.padLeft(chunkIdIncrementor, requiredChunkIdDigits);
                if (chunkIdList.split(',').indexOf(chunkId) == -1) {
                    if (!chunkIdList || chunkIdList == "") {
                        chunkIdList += chunkId;
                    }
                    else {
                        chunkIdList += "," + chunkId;
                    }
                }
                //Preparing form data
                fileChunk = new FormData();
                if (acFile.file.slice) {
                    fileChunk.append('name', acFile.fileName);
                    fileChunk.append('chunkId', chunkId);
                    fileChunk.append('isLastChunk', isLastChunk);
                    fileChunk.append('contentType', acFile.contentType);
                    if (isLastChunk) {
                        fileChunk.append('chunkIdList', chunkIdList);
                    }
                    if (self.tempContainerPath) {
                        fileChunk.append('tempContainerPath', self.tempContainerPath);
                    }
                    fileChunk.append('autoCommitEnabled', self.autoCommitEnabled);
                    fileChunk.append('fileChunk', acFile.file.slice(start, end, acFile.contentType));
                }
                var xhr = new XMLHttpRequest();
                xhr.onreadystatechange = function () {
                    if (xhr.readyState === XMLHttpRequest.DONE) {
                        if (xhr.status === 200) {
                            var response = void 0;
                            if (xhr.response) {
                                response = JSON.parse(xhr.response);
                                if (response.azureUri) {
                                    acFile.fileUri = response.azureUri;
                                }
                                if (!self.tempContainerPath && response.tempContainerPath) {
                                    self.tempContainerPath = response.tempContainerPath;
                                }
                            }
                            if (isLastChunk) {
                                acFile.chunksIdList = chunkIdList;
                                acFile.progress = "0%";
                                acFile.file = null;
                                observer.next(true);
                                observer.complete();
                                return;
                            }
                            ++chunkIdIncrementor;
                            // note: calculate uploadedFileSize before new value of start and end.
                            uploadedFileSize += Math.min(self.maxChunkSize, end - start);
                            start = (chunkIdIncrementor - 1) * self.maxChunkSize;
                            end = Math.min(chunkIdIncrementor * self.maxChunkSize, acFile.fileSize);
                            //uploadedFileSize += Math.min(self.maxChunkSize, end - start);
                            isLastChunk = end == acFile.fileSize;
                            var fileToBeCancelled = null;
                            if (self.filesToBeCancelled) {
                                fileToBeCancelled = self.filesToBeCancelled.filter(function (x) { return x.fileName === acFile.fileName; })[0];
                            }
                            //Do not proceed if the user requested to cancel upload
                            if (!fileToBeCancelled) {
                                sendNextChunk();
                            }
                            else {
                                var index = self.filesToBeCancelled.indexOf(fileToBeCancelled);
                                if (index != -1) {
                                    self.filesToBeCancelled.splice(index, 1);
                                }
                            }
                        }
                        else {
                            ++retryCount;
                            if (retryCount < self.maxRetryCount) {
                                setTimeout(sendNextChunk, self.retryAfter * 1000);
                                return;
                            }
                            observer.error("Some problem occurred while file upload. So, upload not completed");
                        }
                    }
                };
                xhr.upload.onprogress = function (event) {
                    // calculate acutal fileSize that was uploaded, exclude size of header content.
                    var currentUploaded = event.loaded;
                    if (currentUploaded > Number(end - start)) {
                        currentUploaded = Number(end - start);
                    }
                    acFile.progress = Math.round((uploadedFileSize + currentUploaded) / acFile.fileSize * 100) + "%";
                    //acFile.progress = Math.round((uploadedFileSize + event.loaded) / acFile.fileSize * 100) + "%";
                };
                xhr.open('POST', self.uploadApiPath, true);
                xhr.send(fileChunk);
            };
            //To invoke function to send first chunk
            sendNextChunk();
        });
    };
    AcFileUploader.prototype.formatBytes = function (bytes, decimals) {
        if (bytes == 0)
            return '0 Byte';
        var k = 1000;
        //var dm = decimals + 1 || 3;
        var sizes = ['Bytes', 'kb', 'mb', 'gb', 'tb', 'pb', 'eb', 'zb', 'yb'];
        var i = Math.floor(Math.log(bytes) / Math.log(k));
        return parseFloat((bytes / Math.pow(k, i)).toFixed(decimals)) + ' ' + sizes[i];
    };
    AcFileUploader.prototype.padLeft = function (number, numberOfDigits, padWith) {
        return Array(numberOfDigits - String(number).length + 1).join(padWith || '0') + number;
    };
    AcFileUploader.prototype.checkAttachment = function () {
        return this.acFiles.length > 0;
    };
    __decorate([
        core_1.Input('total-size-limit'), 
        __metadata('design:type', Number)
    ], AcFileUploader.prototype, "totalSizeLimit", void 0);
    __decorate([
        //default: 10MB
        core_1.Input('accepted-file-types'), 
        __metadata('design:type', String)
    ], AcFileUploader.prototype, "acceptedFileTypes", void 0);
    __decorate([
        core_1.Input('max-chunk-size'), 
        __metadata('design:type', Number)
    ], AcFileUploader.prototype, "maxChunkSize", void 0);
    __decorate([
        //Default: 256KB
        core_1.Input('multi-file-browser'), 
        __metadata('design:type', Boolean)
    ], AcFileUploader.prototype, "multifileBrowser", void 0);
    __decorate([
        core_1.Input('show-upload-btn'), 
        __metadata('design:type', Boolean)
    ], AcFileUploader.prototype, "showUploadButton", void 0);
    __decorate([
        core_1.Input('show-delete-btn'), 
        __metadata('design:type', Boolean)
    ], AcFileUploader.prototype, "showDeleteButton", void 0);
    __decorate([
        core_1.Input('upload-api-path'), 
        __metadata('design:type', String)
    ], AcFileUploader.prototype, "uploadApiPath", void 0);
    __decorate([
        core_1.Input('delete-api-path'), 
        __metadata('design:type', String)
    ], AcFileUploader.prototype, "deleteApiPath", void 0);
    __decorate([
        core_1.Input('max-retry-count'), 
        __metadata('design:type', Number)
    ], AcFileUploader.prototype, "maxRetryCount", void 0);
    __decorate([
        core_1.Input('retry-wait-time-in-sec'), 
        __metadata('design:type', Number)
    ], AcFileUploader.prototype, "retryAfter", void 0);
    __decorate([
        core_1.Input('existing-files'), 
        __metadata('design:type', Array)
    ], AcFileUploader.prototype, "existingFiles", void 0);
    __decorate([
        core_1.Input('can-attach'), 
        __metadata('design:type', Boolean)
    ], AcFileUploader.prototype, "canAttach", void 0);
    __decorate([
        core_1.Input('auto-commit'), 
        __metadata('design:type', Boolean)
    ], AcFileUploader.prototype, "autoCommitEnabled", void 0);
    __decorate([
        core_1.Output('fileUploadCompletedEvent'), 
        __metadata('design:type', Object)
    ], AcFileUploader.prototype, "fileUploadCompletedEvent", void 0);
    __decorate([
        core_1.Output('unsupportedAttachmentsFoundEvent'), 
        __metadata('design:type', Object)
    ], AcFileUploader.prototype, "unsupportedAttachmentsFoundEvent", void 0);
    __decorate([
        core_1.Output('maxAttachmentSizeReachedEvent'), 
        __metadata('design:type', Object)
    ], AcFileUploader.prototype, "maxAttachmentSizeReachedEvent", void 0);
    AcFileUploader = __decorate([
        core_1.Component({
            selector: 'ac-fileuploader',
            templateUrl: 'app/lib/aclibs/ac-fileuploader/ac-fileuploader.component.html',
            styleUrls: ['app/lib/aclibs/ac-fileuploader/ac-fileuploader.component.css'],
            exportAs: 'acFileUploader'
        }), 
        __metadata('design:paramtypes', [http_1.Http])
    ], AcFileUploader);
    return AcFileUploader;
}());
exports.AcFileUploader = AcFileUploader;
