
import {Injectable} from '@angular/core';
import {IMyChangeRequest} from './myChangeRequest.model';

@Injectable()
export class MyChangeRequestService {

    myChangeRequests: IMyChangeRequest[] = [];

    getMyChangeRequests() {

        this.myChangeRequests.push({
            id: 1592,
            date: "09-09-2016",
            table: "Make",
            rowId: "1321345589",
            status: "Pending",
            reviewed: "",
            changeType: "Add"
        });
        this.myChangeRequests.push({
            id: 1592,
            date: "09-09-2016",
            table: "Break Type",
            rowId: "1235813213",
            status: "Pending",
            reviewed: "",
            changeType: "Update"
        });
        this.myChangeRequests.push({
            id: 1592,
            date: "09-09-2016",
            table: "Break Config",
            rowId: "1321345589",
            status: "Pending",
            reviewed: "",
            changeType: "Delete"
        });

        return this.myChangeRequests;

    }

}