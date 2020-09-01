import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import 'rxjs/Rx';
import {Observable} from 'rxjs/Observable';
import {Observer} from 'rxjs/Observer';
import { AcFile, fileStatusEnum } from './ac-fileuploader.model';

@Component({
    selector: 'ac-fileuploader',
    templateUrl: 'app/lib/aclibs/ac-fileuploader/ac-fileuploader.component.html',
    styleUrls: ['app/lib/aclibs/ac-fileuploader/ac-fileuploader.component.css'],
    exportAs: 'acFileUploader'
})
export class AcFileUploader implements OnInit {

    @Input('total-size-limit') totalSizeLimit: number = 10485760;//default: 10MB
    @Input('accepted-file-types') acceptedFileTypes: string = ".xls,.xlsx, .doc, .docx, .pdf, .jpg, .png, .txt, .jpeg";
    @Input('max-chunk-size') maxChunkSize: number = 256 * 1024; //Default: 256KB
    @Input('multi-file-browser') multifileBrowser: boolean = true;
    @Input('show-upload-btn') showUploadButton: boolean = false;
    @Input('show-delete-btn') showDeleteButton: boolean = false;
    @Input('upload-api-path') uploadApiPath: string;
    @Input('delete-api-path') deleteApiPath: string;
    @Input('max-retry-count') maxRetryCount: number = 3;
    @Input('retry-wait-time-in-sec') retryAfter: number = 5;
    @Input('existing-files') existingFiles: AcFile[] = [];
    @Input('can-attach') canAttach: boolean = true;
    //If this flag is false, then file will not be stored in the Azure until manually calling commit to putblocklist
    @Input('auto-commit') autoCommitEnabled: boolean = false;

    //Emits file information for uploaded files. Invoked for each file upload
    @Output('fileUploadCompletedEvent') fileUploadCompletedEvent = new EventEmitter<AcFile>();
    //Emits the event with list of files that are not in accepted file types
    @Output('unsupportedAttachmentsFoundEvent') unsupportedAttachmentsFoundEvent = new EventEmitter<string>();
    //Emits the event with list of files that are not accepted because the total size reached threshold
    @Output('maxAttachmentSizeReachedEvent') maxAttachmentSizeReachedEvent = new EventEmitter<string>();

    private acFiles: AcFile[] = [];
    private deletedFiles: AcFile[] = [];
    private maxSizeLimit: number = 10485760; //10MB
    private filesSizeSoFar: number = 0;
    private filesToBeCancelled: AcFile[] = [];
    private fileStatusEnum: any = fileStatusEnum;
    private tempContainerPath: string;
    private fileInputId: string;

    constructor(private http: Http) {
        this.fileInputId = 'id-' + Math.random().toString(36).substr(2, 16);
    }

    ngOnInit(): void {
        if (this.canAttach === true) {
            if (!this.uploadApiPath || this.uploadApiPath === "") {
                throw Error("Api path required to upload the file");
            }
        }
       
        if (this.existingFiles) {
            this.acFiles = this.existingFiles.slice();
            for (let file of this.acFiles) {
                file.fileStatus = this.fileStatusEnum.uploaded;
                file.formattedSize = this.formatBytes(file.fileSize, 2);
            }
        }
    }  

    // wrapper function to set existingfiles.
    public setAcFiles(paramFiles?: AcFile[]) {
        
        if (paramFiles) {
            this.acFiles = paramFiles;
        } else {
            this.acFiles = this.existingFiles;//.slice();
        }
    }
    
    public reset(shouldDeleteTempContainer?: boolean) {
        this.acFiles = [];
        this.filesSizeSoFar = 0;
        this.filesToBeCancelled = [];

        if (shouldDeleteTempContainer !== false) {

            this.deleteTempContainer();
        }
        this.tempContainerPath = "";
    }

    public getAcFiles(): AcFile[] {
        // loop through this.acfiles
        let cloneAcFiles: AcFile[] = [];
        this.acFiles.forEach(item => {
            let itemAcFile: AcFile = this.clone(item);
            var file = item.file;
            itemAcFile.file = file;
            cloneAcFiles.push(itemAcFile);
        });
        return cloneAcFiles;
    }

    private clone(source): any {
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

    public cleanupAllTempContainers(): Observable<boolean>{

        return Observable.create((observer: Observer<boolean>) => {

            if (!this.acFiles || this.acFiles.length == 0) {
                observer.next(true);
                observer.complete();
                return;
            }

            let tempContainers = [];
            for (let acFile of this.acFiles) {
                if (acFile.tempContainerName) {
                    tempContainers.push(acFile.tempContainerName);
                }
            }

            let index = 0;
            let cleanupCompleted = false;
            let deleteNextTempContainer = function (tempContainerName: string) {
                this.http.post(this.deleteApiPath, JSON.stringify({ containerName: tempContainerName }),
                    new RequestOptions({ headers: new Headers({ 'Content-Type': 'application/json' }) }))
                    .map(res => {
                        ++index;
                        if (index < tempContainers.length) {
                            deleteNextTempContainer(tempContainers[index]);
                        } else {
                            cleanupCompleted = true;
                        }
                    })
                    .subscribe();
            }

            let checkCleanupCompleted = function () {
                if (cleanupCompleted) {
                    observer.next(true);
                    observer.complete();
                    return;
                }

                setTimeout(checkCleanupCompleted, 100);
            }

            checkCleanupCompleted();
        });
        
    }

    private deleteTempContainer() {
        if (this.tempContainerPath) {
            this.http.post(this.deleteApiPath, JSON.stringify({ containerName: this.tempContainerPath }),
                new RequestOptions({ headers: new Headers({ 'Content-Type': 'application/json' }) }))
                .map(res => { })
                .subscribe();
        }
    }

    public uploadAttachments(): Observable<AcFile[]> {
        return Observable.create((observer: Observer<AcFile[]>) => {
            try {
                this.uploadFiles();
                let self = this;

                let checkFileUploadCompleted = function () {
                    if (!self.getNextFileToUpload()) {
                        let uploadedFiles = self.acFiles.filter(file => file.fileStatus == self.fileStatusEnum.uploaded);
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
                    } else {
                        setTimeout(checkFileUploadCompleted, 100);
                    }
                }

                checkFileUploadCompleted();
            } catch (e) {
                observer.error(e);
            }
        });
      
    }

    public deleteAttachments(): Observable<AcFile[]> {
        return Observable.create((observer: Observer<AcFile[]>) => {
            try {
                this.deleteFiles();

                let self = this;
                let checkFileDeleteCompleted = function () {
                    let filesMarkedToBeDeleted = self.acFiles.filter(file => file.fileStatus == self.fileStatusEnum.deleted);

                    if (!filesMarkedToBeDeleted || filesMarkedToBeDeleted.length == 0) {
                        observer.next(self.deletedFiles);
                        observer.complete();
                        return;
                    } else {
                        setTimeout(checkFileDeleteCompleted, 100);
                    }
                }

                checkFileDeleteCompleted();

            } catch (e) {
                observer.error(e);
            }
        });
    }

    public getFilesMarkedToDelete() {
        let filesToBeDeleted = this.acFiles.filter(file => file.fileStatus == this.fileStatusEnum.deleted);

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
    }

    private deleteFiles() {
        let filesMarkedToBeDeleted = this.acFiles.filter(file => file.fileStatus == this.fileStatusEnum.deleted);

        if (!filesMarkedToBeDeleted || filesMarkedToBeDeleted.length == 0) {
            return;
        }

        let filesToBeDeleted = [];

        for (let file of filesMarkedToBeDeleted) {
            filesToBeDeleted.push({
                containerName: file.containerName,
                fileName: file.fileName,
                directoryPath: file.directoryPath
            })
        }

        let self = this;
        this.http.post(this.deleteApiPath, JSON.stringify(filesToBeDeleted),
            new RequestOptions({ headers: new Headers({ 'Content-Type': 'application/json' }) }))
            .map(res => {
                for (let file of filesMarkedToBeDeleted) {
                    let index = self.acFiles.indexOf(file);
                    if (index != -1) {
                        self.acFiles.splice(index, 1);
                        self.deletedFiles.push(file);
                    }
                    file.fileStatus = self.fileStatusEnum.deleted;
                }
            })
            .subscribe();

    }

    private uploadFiles() {
        if (!this.acFiles || this.acFiles.length == 0) {
            return;
        }

        let self = this;
        let uploadFile = function () {
            if (self.acFiles.length == 0) {
                return;
            }

            let fileToBeUploaded = <AcFile>self.getNextFileToUpload();
            if (!fileToBeUploaded) {
                return;
            }

            self.sendFile(fileToBeUploaded).subscribe(
                uploaded => {
                    if (uploaded) {
                        fileToBeUploaded.fileStatus = fileStatusEnum.uploaded;
                        fileToBeUploaded.containerName = self.tempContainerPath;
                        fileToBeUploaded.tempContainerName = self.tempContainerPath;
                        let nextFileToBeUploaded = self.getNextFileToUpload();
                        if (!nextFileToBeUploaded) {
                            return;
                        }

                        uploadFile();
                    }
                },
                error => {
                    fileToBeUploaded.fileStatus = fileStatusEnum.failed;

                    let nextFileToBeUploaded = self.getNextFileToUpload();
                    if (!nextFileToBeUploaded) {
                        return;
                    }
                }
            );
        };

        uploadFile();
    }

    private getNextFileToUpload(): AcFile {
        if (!this.acFiles || this.acFiles.length == 0) {
            return null;
        }

        let pendingFiles = this.acFiles.filter(function (file) {
            return file.fileStatus === fileStatusEnum.pending;
        });

        if (!pendingFiles || pendingFiles.length == 0) {
            return null;
        }

        return pendingFiles[0];
    }

    private onFilesSelected(e) {
        let selectedFiles: FileList = (<HTMLInputElement>e.target).files;
        if (!selectedFiles || selectedFiles.length == 0) {
            return;
        }

        let sizeExceededFiles: string = "";
        let notAcceptedFiles: string = "";

        for (var i = 0; i < selectedFiles.length; i++) {
            let currentFile = selectedFiles.item(i);

            let duplicateFile = this.acFiles.filter(acFile => acFile.fileName == currentFile.name)[0];

            if (!duplicateFile) {
                let fileExtn = currentFile.name.split('.').pop();

                if (this.acceptedFileTypes.indexOf(fileExtn) == -1) {
                    if (notAcceptedFiles) {
                        notAcceptedFiles += ", ";
                    }
                    notAcceptedFiles += currentFile.name
                } else if (this.filesSizeSoFar + currentFile.size > this.maxSizeLimit) {
                    if (sizeExceededFiles) {
                        sizeExceededFiles += ", ";
                    }
                    sizeExceededFiles += currentFile.name
                }
                else {
                    this.filesSizeSoFar += currentFile.size;

                    this.acFiles.push(<AcFile>{
                        file: currentFile,
                        fileName: currentFile.name,
                        fileSize: currentFile.size,
                        formattedSize: this.formatBytes(currentFile.size, 2),
                        contentType: currentFile.type,
                        fileExtension: fileExtn,
                        fileStatus: fileStatusEnum.pending,
                        canDelete: true,
                    });
                }
            }
        }

        if (sizeExceededFiles) {
            this.maxAttachmentSizeReachedEvent.emit(sizeExceededFiles);
        }

        if (notAcceptedFiles) {
            this.unsupportedAttachmentsFoundEvent.emit(notAcceptedFiles);
        }

        e.target.value = "";
    }

    private removeAttachment(file: AcFile) {   
        if (!this.acFiles || this.acFiles.length == 0) {
            return;
        }

        let fileToBeRemoved = this.acFiles.filter(x => x.fileName == file.fileName)[0];
        fileToBeRemoved.fileStatus = fileStatusEnum.deleted;
    }

    private cancelUpload(file: AcFile) {
        if (!this.acFiles || this.acFiles.length == 0) {
            return;
        }

        let fileToBeRemoved = this.acFiles.filter(x => x.fileName == file.fileName)[0];
        let index = this.acFiles.indexOf(fileToBeRemoved);
        if (index != -1) {
            this.acFiles.splice(index, 1);
            this.filesToBeCancelled.push(fileToBeRemoved);
        }
    }

    private sendFile(acFile: AcFile): Observable<boolean> {
        return Observable.create((observer: Observer<boolean>) => {
            if (!acFile) {
                observer.next(false);
            }

            let self = this;
            let start = 0;
            let end = Math.min(this.maxChunkSize, acFile.fileSize);
            let chunkIdIncrementor = 1;
            let retryCount = 0;
            let isLastChunk = acFile.fileSize <= this.maxChunkSize;
            let chunkIdList = "";

            let requiredChunkIdDigits = 4;
            if (acFile.fileSize > this.maxChunkSize) {
                let expectedLength = String(Math.round(acFile.fileSize / this.maxChunkSize)).length;
                if (expectedLength > requiredChunkIdDigits) {
                    requiredChunkIdDigits = (Math.floor(expectedLength / 4) * 4) +
                        ((expectedLength % 4) > 0 ? 4 : 0);
                }
            }

            let fileChunk: FormData;
            //let httpRequestOptions = new RequestOptions({ headers: new Headers({ 'Content-Type': 'multipart/form-data' }) });
            let uploadedFileSize: number = 0;
            let sendNextChunk = function () {
                let chunkId = self.padLeft(chunkIdIncrementor, requiredChunkIdDigits);

                if (chunkIdList.split(',').indexOf(chunkId) == -1) {
                    if (!chunkIdList || chunkIdList == "") {
                        chunkIdList += chunkId;
                    } else {
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

                let xhr: XMLHttpRequest = new XMLHttpRequest();

                xhr.onreadystatechange = () => {
                    if (xhr.readyState === XMLHttpRequest.DONE) {
                        if (xhr.status === 200) {
                            let response: any;
                            if (xhr.response) {
                                response = <any>JSON.parse(xhr.response);
                                if (response.azureUri) {
                                    acFile.fileUri = response.azureUri;
                                }

                                if (!self.tempContainerPath && response.tempContainerPath) {
                                    self.tempContainerPath = response.tempContainerPath;
                                }
                            }

                            if (isLastChunk) {
                                acFile.chunksIdList = chunkIdList;
                                acFile.progress = "0%"
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

                            let fileToBeCancelled: AcFile = null;
                            if (self.filesToBeCancelled) {
                                fileToBeCancelled = self.filesToBeCancelled.filter(x => x.fileName === acFile.fileName)[0];
                            }

                            //Do not proceed if the user requested to cancel upload
                            if (!fileToBeCancelled) {
                                sendNextChunk();
                            } else {
                                let index = self.filesToBeCancelled.indexOf(fileToBeCancelled);
                                if (index != -1) {
                                    self.filesToBeCancelled.splice(index, 1);
                                }
                            }
                        } else {
                            ++retryCount;
                            if (retryCount < self.maxRetryCount) {
                                setTimeout(sendNextChunk, self.retryAfter * 1000);
                                return;
                            }

                            observer.error("Some problem occurred while file upload. So, upload not completed");
                        }
                    }
                };

                xhr.upload.onprogress = (event) => {
                    // calculate acutal fileSize that was uploaded, exclude size of header content.
                    let currentUploaded: number = event.loaded;
                    if (currentUploaded > Number(end - start)) {
                        currentUploaded = Number(end - start);
                    }
                    acFile.progress = Math.round((uploadedFileSize + currentUploaded) / acFile.fileSize * 100) + "%";
                    //acFile.progress = Math.round((uploadedFileSize + event.loaded) / acFile.fileSize * 100) + "%";
                };

                xhr.open('POST', self.uploadApiPath, true);
                xhr.send(fileChunk);
            }

            //To invoke function to send first chunk
            sendNextChunk();
        });
    }

    private formatBytes(bytes, decimals) {
        if (bytes == 0) return '0 Byte';
        var k = 1000;
        //var dm = decimals + 1 || 3;
        var sizes = ['Bytes', 'kb', 'mb', 'gb', 'tb', 'pb', 'eb', 'zb', 'yb'];
        var i = Math.floor(Math.log(bytes) / Math.log(k));
        return parseFloat((bytes / Math.pow(k, i)).toFixed(decimals)) + ' ' + sizes[i];
    }

    private padLeft(number, numberOfDigits, padWith?) {
        return Array(numberOfDigits - String(number).length + 1).join(padWith || '0') + number;
    }

    public checkAttachment(): boolean {
        return this.acFiles.length > 0;
    }
}