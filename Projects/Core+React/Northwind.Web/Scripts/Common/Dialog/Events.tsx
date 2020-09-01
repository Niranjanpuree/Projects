declare var window: any 

export interface IButtonParameter {
    text: string;
    primary: boolean;
    action: any,
    requireValidation: boolean
}

export class DialogEvents {
    static events: any[] = []
    static buttons: IButtonParameter[] = []

    static registerEvent(component: any, event: any) {
        DialogEvents.events.push({
            component: component,
            event: event
        })
    }

    static getEvent(component: any) {
        for (let i in this.events) {
            if (DialogEvents.events[i].component == component) {
                return DialogEvents.events[i].event;
            }
        }
    }

    static onClick(e: any, component: any) {
        DialogEvents.getEvent(component)(e);
    }
    
}

window.DialogEvents = DialogEvents