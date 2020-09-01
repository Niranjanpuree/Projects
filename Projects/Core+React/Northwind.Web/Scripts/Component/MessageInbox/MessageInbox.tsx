import * as React from "react";
import * as ReactDOM from "react-dom";
import { Remote } from "../../Common/Remote/Remote";
import { NotificationSummary } from "../NotificationSummary/NotificationSummary";
import { SingleMessageEntity } from "./SingleMessageEntity";
declare var window: any;
declare var $: any;

interface IMessageInboxProps {
    parentDomId: string;
    notificationMessageGuid: any;
}

interface IMessageInboxState {
    notificationList: any[];
    notificationProps: any;
    showIndividualSummary: boolean;
    searchValue: string;
    notificationMessageGuid: any,
    showNotFoundMessage: boolean,
}

export class MessageInbox extends React.Component<IMessageInboxProps, IMessageInboxState> {
    dataList: any[] = [];
    sender: any = this;

    constructor(props: any) {
        super(props);

        this.onSearchChange = this.onSearchChange.bind(this);
        this.onSearchClick = this.onSearchClick.bind(this);
        this.individualNotificationClick = this.individualNotificationClick.bind(this);

        this.state = {
            notificationList: [],
            showIndividualSummary: false,
            notificationProps: '',
            searchValue: '',
            notificationMessageGuid: this.props.notificationMessageGuid,
            showNotFoundMessage: false,
        }
    }

    getDesktopNotifications(searchValue: string) {
        let sender = this;
        Remote.get("/Notification/GetDesktopNotification?searchValue=" + searchValue,
            (response: any) => {
                sender.setState({
                    notificationList: response,
                    showNotFoundMessage: response.length > 0 ? false : true,
                });
            },
            (err: any) => { window.Dialog.alert(err) });
    }
    async getInitialMessage(searchValue: string) {
        let sender = this;
        let response = await Remote.getAsync("/Admin/Inbox/GetMessageByNotificationMessageId?notificationMessageGuid=" + this.props.notificationMessageGuid);

        if (response.ok) {
            let data = await response.json();
            sender.setState({
                notificationProps: data,
            });
        } else {
            let message = await Remote.parseErrorMessage(response);
            window.Dialog.alert(message)
        }
    }

    componentDidMount() {
        this.getDesktopNotifications(this.state.searchValue || '');
        this.getInitialMessage(this.state.searchValue || '');
    }

    onSearchChange(e: any) {
        this.setState({
            searchValue: e.target.value,
        });
    }

    onSearchClick() {
        this.getDesktopNotifications(this.state.searchValue || '');
        this.setState({
            notificationProps: '',
        });
    }

    renderMessageList() {
        return <div>
            <div className="inbox-container">
                <div className="row">
                    <div className="inbox-list-container col-md-4 col-lg-3">
                        <div className="inbox-header border-bottom">
                            <div className="input-group search-form-r">
                                <input type="text" className="form-control" id="inlineFormInputGroup" onChange={
                                    this.onSearchChange} value={this.state.searchValue} placeholder="Search" />
                                <div className="input-group-append">
                                    <div className="input-group-text"><a href="javascript:void(0)" onClick={this.onSearchClick} className="k-icon k-i-search"></a></div>
                                </div>

                            </div>
                        </div>
                        <ul className="list-unstyled mb-0 inbox-list">
                            {this.state.notificationList.map((value, index) => {
                                if (value.notificationMessageGuid === this.state.notificationMessageGuid) {
                                    return <li key={index} className={value.userResponse == true
                                        ? "notification-read py-3 border-bottom active"
                                        : "py-3 border-bottom showActiveMessage"}>

                                        <div onClick={() => this.individualNotificationClick(value)
                                        } className="notification-item individualNotification">
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
                                        </div>
                                    </li>
                                }
                            })}
                            {this.state.showNotFoundMessage && <li><div className="not-found-message"><i className="material-icons">sms_failed </i><h6>No results found</h6><small>It seems we can’t find any results based on your search.</small></div></li>}
                        </ul>
                    </div>
                    <div className="col-sm-8 col-lg-9">
                        {this.state.notificationProps !== '' && <SingleMessageEntity notificationProps={this.state.notificationProps} />}
                    </div>
                </div>
            </div>
        </div>;
    }

    render() {
        return this.renderMessageList();
    }

    individualNotificationClick(notificationProps: any) {

        this.setState({
            notificationProps: notificationProps,
            notificationMessageGuid: notificationProps.notificationMessageGuid,
        });

        //refresh the notification count if user read the message..

        let sender = this;
        Remote.postData("/Notification/EditUserResponseByNotificationMessageId", notificationProps.notificationMessageGuid, (data:
            any) => {
            this.getDesktopNotifications(this.state.searchValue || '');
            sender.forceUpdate();
        }, (error: any) => { });

        //re render notification icon if user clicked the message ....
        if (document.getElementById('notificationSummary')) {
            ReactDOM.unmountComponentAtNode(document.getElementById('notificationSummary'));
            ReactDOM.render(<NotificationSummary parentDomId="notificationSummary" />, document.getElementById("notificationSummary"));
        }
    }

}

