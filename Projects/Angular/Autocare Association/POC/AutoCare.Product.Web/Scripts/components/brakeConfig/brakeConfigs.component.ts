import { Component}                           from "@angular/core";
import { BrakeConfigService }                 from "./brakeConfig.service";
import { BrakeTypeService }                   from "../brakeType/brakeType.service";
import { BrakeSystemService }                 from "../brakeSystem/brakeSystem.service";
import { BrakeABSService }                    from "../brakeABS/brakeABS.service";

@Component({
    selector: 'brakeConfigs-component',
    template: `<router-outlet></router-outlet>`,
    providers: [BrakeConfigService, BrakeTypeService, BrakeSystemService, BrakeABSService]
})

export class BrakeConfigsComponent {
}