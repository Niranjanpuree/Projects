"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class DialogEvents {
    static registerEvent(component, event) {
        DialogEvents.events.push({
            component: component,
            event: event
        });
    }
    static getEvent(component) {
        for (let i in this.events) {
            if (DialogEvents.events[i].component == component) {
                return DialogEvents.events[i].event;
            }
        }
    }
    static onClick(e, component) {
        DialogEvents.getEvent(component)(e);
    }
}
DialogEvents.events = [];
DialogEvents.buttons = [];
exports.DialogEvents = DialogEvents;
window.DialogEvents = DialogEvents;
//# sourceMappingURL=Events.js.map