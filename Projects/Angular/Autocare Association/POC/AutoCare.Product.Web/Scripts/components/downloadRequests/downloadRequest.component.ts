import {Component, OnInit} from '@angular/core';
import {IDownloadRequest} from './downloadRequest.model';
import {DownloadRequestService} from './downloadRequest.service';

@Component({
    selector: 'download-requests',
    templateUrl: 'app/templates/downloadRequests/downloadRequests.html',
    providers: [DownloadRequestService]
})

export class DownloadRequestComponent implements OnInit {

    constructor(private _downloadRequestService: DownloadRequestService) { }

    errorMessage: string;

    downloadRequests: IDownloadRequest[];

    ngOnInit() {
        this.getDownloadRequests();
    }

    getDownloadRequests() {
        this.downloadRequests = this._downloadRequestService.getDownloadRequests();

        //this._downloadRequestService.getDownloadRequests()
        //    .subscribe(
        //    downloadRequests => this.downloadRequests = downloadRequests,
        //    error => this.errorMessage = <any>error
        //);
    }
}