import * as React from "react";

export function SingleMessageEntity(props: any) {
    const { subject, message, additionalMessage, createdOnFormatDateTime } = props.notificationProps;
    return (<div>
        <div className="inbox-detail-container">
            <div className="inbox-detail-header">
                <h5>{subject}</h5>
            </div>
            <div className="inbox-detail">
                <div dangerouslySetInnerHTML={{ __html: message}}/>
            </div>
        </div>
    </div>);
}