"use strict";
var Toast = (function () {
    function Toast(type, message, title) {
        this.type = type;
        this.message = message;
        this.title = title;
        this.autoDismiss = true;
        this.enableHTML = false;
    }
    return Toast;
}());
exports.Toast = Toast;
//# sourceMappingURL=toast.js.map