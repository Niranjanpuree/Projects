import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from "../authentication.service";
import { SharedService } from "./shared.service";
import {IMainHeader, ITokenModel} from "./shared.model";
import { Router, ActivatedRoute } from "@angular/router";

@Component({
    selector: 'mainHeader-comp',
    templateUrl: 'app/templates/shared/mainHeader.component.html',
    providers: [AuthenticationService],
})

export class MainHeaderComponent {
    private token: ITokenModel;
    selectedMainHeaderMenu: IMainHeader = [];
    constructor(private authenticationService: AuthenticationService, private sharedService: SharedService, private route: ActivatedRoute, private router: Router) {
    }

    ngOnInit() {
        this.token = this.sharedService.getTokenModel();
        this.selectedMainHeaderMenu.selectedMainHeaderMenuItem = "VCDB";
    }

    onLogout() {
        this.authenticationService.logout();
    }

    onMainHeaderMenuClick(mainHeaderMenuItem) {
        this.selectedMainHeaderMenu.selectedMainHeaderMenuItem = mainHeaderMenuItem;
        this.sharedService.selectedMenuHeaderItem.selectedMainHeaderMenuItem = mainHeaderMenuItem;
        this.router.navigateByUrl('dashboard');
    }
}