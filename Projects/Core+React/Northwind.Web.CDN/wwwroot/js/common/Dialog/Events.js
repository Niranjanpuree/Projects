"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var DialogEvents = /** @class */ (function () {
    function DialogEvents() {
    }
    DialogEvents.registerEvent = function (component, event) {
        DialogEvents.events.push({
            component: component,
            event: event
        });
    };
    DialogEvents.getEvent = function (component) {
        for (var i in this.events) {
            if (DialogEvents.events[i].component == component) {
                return DialogEvents.events[i].event;
            }
        }
    };
    DialogEvents.onClick = function (e, component) {
        DialogEvents.getEvent(component)(e);
    };
    DialogEvents.events = [];
    DialogEvents.buttons = [];
    return DialogEvents;
}());
exports.DialogEvents = DialogEvents;
window.DialogEvents = DialogEvents;
//# sourceMappingURL=Events.js.map