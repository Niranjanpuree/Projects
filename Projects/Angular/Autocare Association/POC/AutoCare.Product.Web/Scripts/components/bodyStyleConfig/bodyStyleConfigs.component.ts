import { Component}                           from "@angular/core";
import { BodyStyleConfigService }                 from "./bodyStyleConfig.service";
import { BodyTypeService }                   from "../bodyType/bodyType.service";
import { BodyNumDoorsService }                 from "../bodyNumDoors/bodyNumDoors.service";

@Component({
    selector: 'bodyStyleConfigs-component',
    template: `<router-outlet></router-outlet>`,
    providers: [BodyStyleConfigService, BodyTypeService, BodyNumDoorsService]
})

export class BodyStyleConfigsComponent {
}