import { Component}                           from "@angular/core";
import { MfrBodyCodeService }                from "./mfrBodyCode.service";

@Component({
    selector: 'mfrBodyCodes-component',
    template: `<router-outlet></router-outlet>`,
    providers: [MfrBodyCodeService]
})

export class MfrBodyCodesComponent {
}