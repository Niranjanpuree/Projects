import * as React from "react";
import * as ReactDOM from "react-dom";
import { Remote } from "../../Common/Remote/Remote";
import { NotificationSummary } from "../NotificationSummary/NotificationSummary";
declare var window: any;
declare var $: any;

interface IMessageListProps {
    parentDomId: string;
    notificationMessageGuid: any;
    notificationMessage?: any;
    onMessageSelect: Function;
}

interface IMessageListState {
    notificationList: any[];
    notificationProps: any;
    showIndividualSummary: boolean;
    searchValue: string;
}

export class MessageList extends React.Component<IMessageListProps, IMessageListState> {
    dataList: any[] = [];
    sender: any = this;

    constructor(props: any) {
        super(props);

        this.onSearchChange = this.onSearchChange.bind(this);
        this.individualNotificationClick = this.individualNotificationClick.bind(this);

        this.state = {
            notificationList: [],
            showIndividualSummary: false,
            notificationProps: '',
            searchValue: '',
        }
    }

    getDesktopNotifications(searchValue: string) {
        Remote.get("/Notification/GetDesktopNotification?searchValue=" + searchValue,
            (response: any) => {
                this.setState({
                    notificationList: response,
                });
            },
            (err: any) => { window.Dialog.alert(err) });
    }

    componentDidMount() {
        this.getDesktopNotifications(this.state.searchValue || '');
    }

    onSearchChange(e: any) {
        this.setState({
            searchValue: e.target.value,
        });
    }

    onSearchClick(sender: any) {
        this.getDesktopNotifications(this.state.searchValue || '');
        sender.getDesktopNotifications(sender.state.searchValue);
        ReactDOM.unmountComponentAtNode(document.getElementById('singleMessageEntity'));
    }

    renderMessageList() {
        return <React.Fragment>
            <div className="inbox-header border-bottom">
                <div className="input-group">
                    <div className="input-group-prepend">
                        <div className="input-group-text"><i onClick={() => this.onSearchClick(this)} className="k-icon k-i-search"></i></div>
                    </div>
                    <input type="text" className="form-control" id="inlineFormInputGroup" onChange={this.onSearchChange} value={this.state.searchValue} placeholder="Search" />
                </div>
            </div>
            <ul className="list-unstyled mb-0 inbox-list">
                {this.state.notificationList.map((value, index) => {
                    if (value.notificationMessageGuid === this.props.notificationMessageGuid) {
                        return <li key={index} className={value.userResponse == true ? "notification-read py-3 border-bottom active" : "py-3 border-bottom active"}>

                            <div onClick={() => this.individualNotificationClick(value)
                            } className="notification-item individualNotification">
                                <p className="notification-subject">
                                    {value.subject}
                                </p>
                                <p className="notification-message">
                                    {value.message}
                                </p>
                                <p className="notification-message-additional">
                                    {value.additionalMessage}
                                </p>
                                <div className="notification-item-footer">
                                    <span className="notification-item-created">
                                    <i className="k-icon k-i-user"></i>{value.createdByName}
                                    </span>
                                    <span className="notification-item-date">
                                        <i className="k-icon k-i-clock"></i>{value.createdOnFormatDateTime}
                                    </span>
                                </div>
                            </div>
                        </li>
                    } else {

                        return <li key={index} className={value.userResponse === true
                            ? "notification-read py-3 border-bottom "
                            : "py-3 border-bottom"}>

                            <div onClick={() => this.individualNotificationClick(value)
                            } className="notification-item individualNotification">
                                <p className="notification-subject">
                                    {value.subject}
                                </p>
                                <p className="notification-message">
                                    {value.message}
                                </p>
                                <p className="notification-message-additional">
                                    {value.additionalMessage}
                                </p>
                                <div className="notification-item-footer">
                                    <span className="notification-item-created">
                                    <i className="k-icon k-i-user"></i>{value.createdByName}
                                    </span>
                                    <span className="notification-item-date">
                                        <i className="k-icon k-i-clock"></i>{value.createdOnFormatDateTime}
                                    </span>
                                </div>
                            </div>
                        </li>
                    }
                })}
            </ul>
        </React.Fragment>;
    }

    render() {
        
        return this.renderMessageList();
    }

    individualNotificationClick(notificationProps: any) {
        this.props.onMessageSelect(notificationProps);

        //refresh the notification count if user read the message..

        let sender = this;
        Remote.postData("/Notification/EditUserResponseByNotificationMessageId", notificationProps.notificationMessageGuid, (data: any) => { this.getDesktopNotifications(this.state.searchValue || ''); sender.forceUpdate(); }, (error: any) => { });

        //re render notification icon if user clicked the message ....
        if (document.getElementById('notificationSummary')) {
            ReactDOM.unmountComponentAtNode(document.getElementById('notificationSummary'));
            ReactDOM.render(<NotificationSummary parentDomId="notificationSummary" />, document.getElementById("notificationSummary"));
        }
    }

}

