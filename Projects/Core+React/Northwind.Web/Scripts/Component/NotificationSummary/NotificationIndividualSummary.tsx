import * as React from "react";
import * as ReactDOM from "react-dom";
import { Remote } from "../../Common/Remote/Remote";
import { Popup } from '@progress/kendo-react-popup';
declare var window: any;
declare var $: any;

//interface INotificationIndividualSummaryProps {
//    notificationProps: any;
//}
//
//interface INotificationIndividualSummaryState {
//}

//export class NotificationIndividualSummary extends React.Component<INotificationIndividualSummaryProps, INotificationIndividualSummaryState> {
//    constructor(props: any) {
//        super(props);
//    }
//
//    render() {
//        return <div>
//            <p>{this.props.notificationProps.subject}</p>
//            <p>{this.props.notificationProps.message}</p>
//            <p>{this.props.notificationProps.additionalMessage}</p>
//            <span className="notification-list-date">
//                <i className="k-icon k-i-clock"></i>{this.props.notificationProps.createdOn}
//            </span>
//        </div>
//    }
//}
export function NotificationIndividualSummary(props: any) {
    console.log(props);
    return <div>
        <p>{props.notificationProps.subject}</p>
        <p>{props.notificationProps.message}</p>
        <p>{props.notificationProps.additionalMessage}</p>
        <span className="notification-list-date">
            <i className="k-icon k-i-clock"></i>{props.notificationProps.createdOnFormatDateTime}
        </span>
        <p>{props.notificationProps.createdByName}</p>
    </div>
}

export function NotificationIndividualSummaryCreatePortal(props: any) {
    return ReactDOM.createPortal(<div>
        <p>{props.notificationProps.subject}</p>
        <p>{props.notificationProps.message}</p>
        <p>{props.notificationProps.additionalMessage}</p>
        <span className="notification-list-date">
            <i className="k-icon k-i-clock"></i>{props.notificationProps.createdOnFormatDateTime}
        </span>
        <p>{props.notificationProps.createdByName}</p>
    </div>,
        document.getElementById(props.domToRender));
}

