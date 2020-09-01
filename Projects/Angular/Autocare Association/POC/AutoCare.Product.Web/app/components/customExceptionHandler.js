"use strict";
var CustomExceptionHandler = (function () {
    function CustomExceptionHandler() {
    }
    //constructor(private toastr: ToastsManager) {
    //}
    //pushkar: cannot use ToastsManager due to the cyclic dependency error
    //Cannot instantiate cyclic dependency! ApplicationRef_: in NgModule AppModule
    CustomExceptionHandler.prototype.handleError = function (error) {
        console.log(error.stack);
        if (error.rejection) {
            console.log(error.rejection.stack);
        }
        //if (this.toastr) {
        //    this.toastr.error(error.message || String(error));
        //}
    };
    return CustomExceptionHandler;
}());
exports.CustomExceptionHandler = CustomExceptionHandler;
