import * as React from "react";
import * as ReactDOM from "react-dom";
import { Remote } from "../../Common/Remote/Remote";
declare var window: any;
declare var $: any;

interface INotificationSummaryProps {
    parentDomId: string,
    siteUrl?: string
}

interface INotificationSummaryState {
    notificationList: any[];
    unreadNotificationList: any[];
    notificationProps: any;
}

export class NotificationSummary extends React.Component<INotificationSummaryProps, INotificationSummaryState> {
    dataList: any[] = [];

    constructor(props: any) {
        super(props);


        this.handleClick = this.handleClick.bind(this);
        this.individualNotificationClick = this.individualNotificationClick.bind(this);


        this.state = {
            notificationList: [],
            unreadNotificationList: [],
            notificationProps: '',
        }
    }

    componentDidUpdate(prevProps: any, prevState: any, snapshot?: any): void {
        let sender = this;
        setTimeout(function () {
            sender.getDesktopNotification();
        }, 30000);
    }

    getDesktopNotification() {
        let sender = this;
        Remote.get("/Notification/GetUnReadDesktopNotification",
            (response: any) => {
                sender.dataList = response;
                sender.setState({
                    unreadNotificationList: response,
                });
            },
            (err: any) => { window.Dialog.alert(err) });
    }

    componentDidMount() {
        this.getDesktopNotification();
    }

    render() {
        return <React.Fragment>

            <a className="dropdown-toggle" href="#" role="button" data-toggle="dropdown" onClick={this.handleClick} id="notificationSummary">
                <i className="menu-icon material-icons">notifications</i>
                <span className={this.state.unreadNotificationList.length === 0 ? "d-none" : "notification-count"} >{this.state.unreadNotificationList.length}</span>
            </a>
            <div className="dropdown-menu dropdown-menu-right" aria-labelledby="notificationSummary">
                <div className="notification-list">
                    <ul className="list-unstyled">
                        {this.state.unreadNotificationList.map((value, index) => {
                            return <li key={index}>
                                <span onClick={() => this.individualNotificationClick(value)} className="notification-item individualNotification" >
                                    <p className="notification-subject">
                                        {value.subject}
                                    </p>
                                    <p className="notification-message-additional">
                                        {value.additionalMessage}
                                    </p>
                                    <div className="notification-item-footer">
                                        <span className="notification-item-created">
                                            {value.createdByName}
                                        </span>
                                        <span className="notification-item-date">
                                            <i className="k-icon k-i-clock"></i>{value.createdOnFormatDateTime}
                                        </span>
                                    </div>
                                </span>
                            </li>
                        })}
                        {this.state.unreadNotificationList.length === 0 &&
                            <li className="notification-empty p-3 text-center">
                                <i className="material-icons text-muted">notifications_off</i>
                                <p className="mb-0">There isn't any new notification.</p>
                            </li>
                        }
                    </ul>
                    <div className="card-footer p-0"><a href={(this.props.siteUrl || '') + "/admin/inbox"} className="btn-block btn btn-sm btn-light p-2 rounded-bottom text-primary">View All</a></div>
                </div>
            </div>

        </React.Fragment>
    }

    handleClick() {
        this.setState({
            notificationList: this.dataList
        });
    }

    refresh() { }

    individualNotificationClick(notificationProps: any) {
        Remote.postData("/Notification/EditUserResponseByNotificationMessageId", notificationProps.notificationMessageGuid, (data: any) => { }, (error: any) => { });
        window.location.href = this.props.siteUrl + "/Admin/Inbox?notificationMessageGuid=" + notificationProps.notificationMessageGuid;
    }
}