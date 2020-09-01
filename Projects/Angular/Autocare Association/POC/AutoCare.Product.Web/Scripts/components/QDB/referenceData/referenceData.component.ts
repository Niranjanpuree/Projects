import {Component} from '@angular/core';
import {ActivatedRoute, Router, NavigationEnd} from '@angular/router';

@Component({
    selector: 'qdb-reference-data',
    templateUrl: 'app/templates/QDB/referenceData/referenceData.component.html',
})
export class QDBReferenceDataComponent {
    activeSubMenu: string = '';
    activeSubMenuGroup: string = '';
    router: Router;
    route: ActivatedRoute;
    finalUrl: string;
    isChildClicked: boolean = false;
    isMenuExpanded: boolean = true;
    constructor(route: ActivatedRoute, router: Router) {
        this.route = route;
        this.router = router;
        this.router.events.subscribe(event => {
            if ((<any>event.constructor).name === 'NavigationEnd') {
                let navEndEvent = <NavigationEnd>event;
                this.finalUrl = navEndEvent.urlAfterRedirects;
                //let referenceDataRouteConfig = this.router.routerState.firstChild(this.route);

                //if (referenceDataRouteConfig) {
                //    let activeRouteConfig = this.router.routerState.firstChild(referenceDataRouteConfig);
                //    this.activeSubMenu = activeRouteConfig.snapshot.data['activeSubMenuTab'];
                //    this.activeSubMenuGroup = activeRouteConfig.snapshot.data['activeSubMenuGroup'];
                //    this.isChildClicked = true;
                //}
            }
        });
    }

    toggleSubMenuGroupActive(subMenuGroup, activeSubMenuGroup) {
        if (this.activeSubMenuGroup == subMenuGroup && this.isChildClicked == false) {
            this.activeSubMenuGroup = '';
        }
        else {
            this.activeSubMenuGroup = subMenuGroup;
        }
        this.isChildClicked = false;
    }

    toggleLeftMenu() {
        if (this.isMenuExpanded) {
            this.isMenuExpanded = false;
        }
        else {
            this.isMenuExpanded = true;
        }
    }
}