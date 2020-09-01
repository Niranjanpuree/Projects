import * as React from 'react';
import { ToastContainer, toast } from 'react-toastify';
//import 'react-toastify/dist/ReactToastify.css';

interface INotificationToastProps
{
    messageType: any;
    messageText: any;
}
export class NotificationToast extends React.Component<INotificationToastProps, {}>
{
    constructor(props: any)
    {
        super(props);
    }

    componentDidUpdate(prevProps: any, prevState: any, snapshot: any)
    {
        if (prevProps.messageType !== this.props.messageType)
        {
            return true;
        }
        return false;
    }

    notify = (messageType: any, messageText: any) =>
    {

        switch (messageType)
        {
            case "Success":
                toast.success(messageText + " !", {
                    position: toast.POSITION.TOP_CENTER,
                    autoClose: 3000
                });
                break;
            case "Error":
                toast.error(messageText + " !", {
                    position: toast.POSITION.TOP_LEFT,
                    autoClose: 500
                });
                break;
            case "Warn":
                toast.warn(messageText + " !", {
                    position: toast.POSITION.BOTTOM_LEFT,
                    autoClose: 500
                });
                break;
            case "Info":
                toast.info(messageText + " !", {
                    position: toast.POSITION.BOTTOM_CENTER,
                    autoClose: 500
                });
                break;
            case "Custom":
                toast("Custom Style Notification with css class!", {
                    position: toast.POSITION.BOTTOM_RIGHT,
                    className: 'foo-bar',
                    autoClose: 500
                });
                break;
            default:
                toast("Default Notification !");
        }
    };

    render()
    {
        return (
            <div>
                {this.notify(this.props.messageType, this.props.messageText)}
                <ToastContainer />
            </div>
        );
    }
}