
import {Injectable} from '@angular/core';
import {IRecentChange} from './recentChange.model';

@Injectable()
export class RecentChangeService {

    recentChanges: IRecentChange[] = [];

    getRecentChanges() {
        this.recentChanges.push({
            table: "Make",
            newValue: "Olive",
            previousValue: "Maroon",
            rowId: "132139",
            approve: "09-09-2016",
            changeType: "Add"  
        });
        this.recentChanges.push({
            table: "Make",
            newValue: "Olive",
            previousValue: "Maroon",
            rowId: "132139",
            approve: "09-09-2016",
            changeType: "Update"
        });
        this.recentChanges.push({
            table: "Make",
            newValue: "Olive",
            previousValue: "Maroon",
            rowId: "132139",
            approve: "09-09-2016",
            changeType: "Delete"
        });
        this.recentChanges.push({
            table: "Make",
            newValue: "Olive",
            previousValue: "Maroon",
            rowId: "132139",
            approve: "09-09-2016",
            changeType: "Update"
        });
        this.recentChanges.push({
            table: "Make",
            newValue: "Olive",
            previousValue: "Maroon",
            rowId: "132139",
            approve: "09-09-2016",
            changeType: "Delete"
        });
        this.recentChanges.push({
            table: "Make",
            newValue: "Olive",
            previousValue: "Maroon",
            rowId: "132139",
            approve: "09-09-2016",
            changeType: "Add"
        });
        this.recentChanges.push({
            table: "Make",
            newValue: "Olive",
            previousValue: "Maroon",
            rowId: "132139",
            approve: "09-09-2016",
            changeType: "Delete"
        });

        return this.recentChanges;

    }

}