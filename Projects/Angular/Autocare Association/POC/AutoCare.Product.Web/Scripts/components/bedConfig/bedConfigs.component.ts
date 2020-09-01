import { Component}                           from "@angular/core";
import { BedConfigService }                 from "./bedConfig.service";
import { BedTypeService }                   from "../bedType/bedType.service";
import { BedLengthService }                 from "../bedLength/bedLength.service";

@Component({
    selector: 'bedConfigs-component',
    template: `<router-outlet></router-outlet>`,
    providers: [BedConfigService, BedTypeService, BedLengthService]
})

export class BedConfigsComponent {
}