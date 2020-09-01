import { Component, OnInit, ViewChild, Input,
    Output, EventEmitter }                      from "@angular/core";
import { SharedService }                        from "../shared/shared.service";

@Component({
    selector: "system-menubar",
    templateUrl: "app/templates/system/system-menubar.component.html",
})

export class SystemMenuBar {
    //private isSystemsMenuExpanded: boolean = true;
    private activeSubMenu: string = "";
    activeSubMenuGroup: string;//Brake as Default
    private isChildClicked: boolean = false;
    @Input("style") style: string[];
    @Input("isSystemsMenuExpanded") isSystemsMenuExpanded: boolean;
    @Output("onToggleMenuBarEvent") onToggleMenuBarEvent = new EventEmitter<boolean>();
    @Output("onSelectedSubMenuGroupEvent") onSelectedSubMenuGroupEvent = new EventEmitter<string>();

    constructor(private sharedService: SharedService) { }
    ngOnInit() {
        if (this.sharedService.systemMenubarSelected != null) {
            this.activeSubMenuGroup = this.sharedService.systemMenubarSelected;
        }
        else {
            this.activeSubMenuGroup = "Brake";//load brake system first
        }
    }
    private toggleSubMenuGroupActive(subMenuGroup, activeSubMenuGroup) {
        if (this.activeSubMenuGroup == subMenuGroup && this.isChildClicked == false) {
            this.activeSubMenuGroup = subMenuGroup;
        }
        else {
            this.activeSubMenuGroup = subMenuGroup;
            this.onSelectedSubMenuGroupEvent.emit(<string>subMenuGroup);
        }
        this.isChildClicked = false;
        var headerht = $('header').innerHeight();
        var navht = $('nav').innerHeight();
        var winht = $(window).height();
        var winwt = 960;
        $(".drawer-left").css('min-height', winht - headerht - navht);
        $(".drawer-left").css('width', winwt);
    }

    private ontoggleMenuBar() {
        this.isSystemsMenuExpanded = !this.isSystemsMenuExpanded;
        // call back
        this.onToggleMenuBarEvent.emit(this.isSystemsMenuExpanded);
    }
    
}


