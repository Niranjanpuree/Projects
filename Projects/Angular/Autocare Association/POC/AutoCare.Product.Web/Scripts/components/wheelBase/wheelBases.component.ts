import { Component}                           from "@angular/core";
import { WheelBaseService }                 from "./wheelBase.service";

@Component({
    selector: 'wheelBases-component',
    template: `<router-outlet></router-outlet>`,
    providers: [WheelBaseService]
})

export class WheelBasesComponent {
}