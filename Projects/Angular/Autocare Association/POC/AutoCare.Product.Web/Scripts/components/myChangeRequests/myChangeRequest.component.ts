
import {Component, OnInit} from '@angular/core';
import {MyChangeRequestService} from './myChangeRequest.service';


@Component({
    selector: 'my-change-requests',
    templateUrl: 'app/templates/myChangeRequests/myChangeRequests.html',
    providers: [MyChangeRequestService]
})


export class MyChangeRequestComponent implements OnInit {

    constructor(private _myChangeRequestService: MyChangeRequestService) { }

    myChangeRequests = [];

    ngOnInit() {
        this.getMyChangeRequests();
    }

    getMyChangeRequests() {
        this.myChangeRequests = this._myChangeRequestService.getMyChangeRequests();
    }

}