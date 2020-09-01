import { Component}                           from "@angular/core";
import { DriveTypeService }                from "./driveType.service";

@Component({
    selector: 'driveTypes-component',
    template: `<router-outlet></router-outlet>`,
    providers: [DriveTypeService]
})

export class DriveTypesComponent {
}