import { Component } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import {IMainHeader} from "./shared.model";
import { SharedService} from "./shared.service";

@Component({
    selector: 'subHeader-comp',
    templateUrl: 'app/templates/shared/subHeader.component.html',
})
export class SubHeaderComponent {
    selectedMainHeaderMenu: IMainHeader = [];
    router: Router;
    route: ActivatedRoute;
    activevehicle = false;
    activesystem = false;

    constructor(route: ActivatedRoute, private sharedService: SharedService, router: Router) {
        this.router = router;
        this.route = route;
    }

    isVehiclesActive() {
        this.router.events.subscribe(event => {
            if ((<any>event.constructor).name === 'NavigationEnd') {
                //let routeConfig = this.router.routerState.firstChild(this.route);
                //if (routeConfig) {
                //    let activeTab = routeConfig.snapshot.data['activeTab'];
                //    //console.log(activeTab);
                //    return this.activevehicle= activeTab == "Vehicles";
                //} 
            }
        });
    }

    isSystemsActive() {
        this.router.events.subscribe(event => {
            if ((<any>event.constructor).name === 'NavigationEnd') {
                //let routeConfig = this.router.routerState.firstChild(this.route);
                //if (routeConfig) {
                //    let activeTab = routeConfig.snapshot.data['activeTab'];
                //    //console.log(activeTab);
                //    return this.activesystem=activeTab == "Systems";
                //} 
            }
        });
    }
}