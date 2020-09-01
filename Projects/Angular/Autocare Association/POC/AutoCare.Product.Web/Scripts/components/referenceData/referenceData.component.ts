import {Component} from '@angular/core';
import {ActivatedRoute, Router, NavigationEnd} from '@angular/router';
import {SharedService} from '../shared/shared.service';

@Component({
    selector: 'reference-data',
    templateUrl: 'app/templates/referenceData/referenceData.component.html',
})

export class ReferenceDataComponent {
    activeSubMenu: string = '';
    activeSubMenuGroup: string = '';
    router: Router;
    route: ActivatedRoute;
    finalUrl: string;
    isChildClicked: boolean = false;
    isMenuExpanded: boolean = true;
    constructor(route: ActivatedRoute, router: Router, private sharedService: SharedService) {
        this.route = route;
        this.router = router;
        this.router.events.subscribe(event => {
            if ((<any>event.constructor).name === 'NavigationEnd') {
                let navEndEvent = <NavigationEnd>event;
                this.finalUrl = navEndEvent.urlAfterRedirects;

                let referenceDataRoute = this.route.firstChild;

                if (referenceDataRoute) {
                    let activeRouteConfig = referenceDataRoute.firstChild;

                    referenceDataRoute.firstChild.data.subscribe(data => {
                        this.activeSubMenu = data['activeSubMenuTab'];
                        if (sharedService.referenceDataActiveSubMenuGroupSelected == '') {
                            this.activeSubMenuGroup = data['activeSubMenuGroup'];
                            this.sharedService.referenceDataActiveSubMenuGroupSelected = this.activeSubMenuGroup;
                        }
                        else {
                            this.activeSubMenuGroup = this.sharedService.referenceDataActiveSubMenuGroupSelected;
                        }
                    });
                    this.isChildClicked = true;
                }
            }
        });
    }

    toggleSubMenuGroupActive(subMenuGroup, event) {
        this.sharedService.referenceDataActiveSubMenuGroupSelected = subMenuGroup;
        if (this.activeSubMenuGroup == subMenuGroup && this.isChildClicked == false) {
            if (event.target.innerText == subMenuGroup) {
                this.activeSubMenuGroup = '';
            }
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
    redirectToSystem(menuName: string) {
        if (menuName != null) {
            this.sharedService.systemMenubarSelected = menuName //Assumes that same name in system and reference data
            this.router.navigate(["/system/search"]);

        }

    }
}