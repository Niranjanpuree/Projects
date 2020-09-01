import { ErrorHandler } from '@angular/core';
import {ToastsManager} from "../lib/aclibs/ng2-toastr/ng2-toastr";

export class CustomExceptionHandler implements ErrorHandler {

    //constructor(private toastr: ToastsManager) {
    //}
    //pushkar: cannot use ToastsManager due to the cyclic dependency error
    //Cannot instantiate cyclic dependency! ApplicationRef_: in NgModule AppModule

    handleError(error: any): void {
        console.log(error.stack);

        if (error.rejection) {
            console.log(error.rejection.stack);
        }

        //if (this.toastr) {
        //    this.toastr.error(error.message || String(error));
        //}
    }
}