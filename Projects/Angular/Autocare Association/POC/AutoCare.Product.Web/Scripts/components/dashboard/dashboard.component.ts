import {Component, OnInit} from '@angular/core';
import {IDashboardPreference} from './dashboardPreference.model';
import {DownloadRequestService} from '../downloadRequests/downloadRequest.service';
import { AcFileUploader } from '../../lib/aclibs/ac-fileuploader/ac-fileuploader';

@Component({
    selector: 'dashboard',
    templateUrl: 'app/templates/dashboard/dashboard.html',
    providers: [DownloadRequestService]
})

export class DashboardComponent implements OnInit {

    constructor(private _downloadRequestService: DownloadRequestService
    ) { }

    dashboardPreferences: IDashboardPreference[] = [];

    ngOnInit() {
        this.getDashboardPreferences();
        //debugger;
        //var p = document.getElementById('myDashboard');
        //for (var i = 0; i < this.dashboardPreferences.length; i++) {
        //   // var n = document.registerElement(this.dashboardPreferences[i].htmlSelector);
        //   // document.body.appendChild(n);
        //}
    }

    getDashboardPreferences() {
        //get dashboard preferences
        this.dashboardPreferences.push({
            name: 'Search',
            htmlSelector: 'search'
        });
        this.dashboardPreferences.push({
            name: 'Download Request',
            htmlSelector: 'download-requests'
        });
        this.dashboardPreferences.push({
            name: 'My Change Request',
            htmlSelector: 'my-change-requests'
        });
        this.dashboardPreferences.push({
            name: 'Recent Changes',
            htmlSelector: 'recent-changes'
        });
    }

}