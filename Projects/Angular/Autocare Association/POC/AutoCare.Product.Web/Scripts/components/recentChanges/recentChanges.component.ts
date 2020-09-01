
import {Component, OnInit} from '@angular/core';
import {RecentChangeService} from './recentChange.service';

@Component({
    selector: 'recent-changes',
    templateUrl: 'app/templates/recentChanges/recentChanges.html',
    providers: [RecentChangeService]
})

export class RecentChangesComponent implements OnInit {

    constructor(private _recentChangeService: RecentChangeService) { }
    
    recentChanges = [];
    //isumm: ISummary = {
    //    countAdd: 0,
    //    countUpdate: 0,
    //    countDelete: 0
    //}
    changeTypeAddCount: number = 0;
    changeTypeUpdateCount: number = 0;
    changeTypeDeleteCount: number = 0;
    
    ngOnInit() {
        this.getRecentChanges();
    }

    getRecentChanges() {
        this.recentChanges = this._recentChangeService.getRecentChanges();    
        for (var i = 0; i < this.recentChanges.length; i++) {
            if (this.recentChanges[i].changeType == "Add") {
                this.changeTypeAddCount++;
            }
            else if (this.recentChanges[i].changeType == "Update") {
                this.changeTypeUpdateCount++;
            }
            else if (this.recentChanges[i].changeType == "Delete") {
                this.changeTypeDeleteCount++;
            }
        }

        //this.recentChanges.forEach(function (recentChange, index) {
        //    debugger;
        //    if (recentChange.changeType == "Add") {
        //        //this.changeTypeAddCount++;
                
        //    }
        //    else if (recentChange.changeType == "Update") {
        //        this.changeTypeEditCount++;
        //    }
        //    else if (recentChange.changeType == "Delete") {
        //        this.changeTypeDeleteCount++;
        //    }
        //})
    }
}

//export interface ISummary {
//    countAdd: number;
//    countUpdate: number;
//    countDelete: number;
//}