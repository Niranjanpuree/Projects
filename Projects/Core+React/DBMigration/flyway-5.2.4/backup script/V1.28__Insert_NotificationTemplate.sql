GO
INSERT [dbo].[NotificationTemplate] ([NotificationTemplateGuid], [Keys], [NotificationTypeGuid], [Subject], [Message],
[IsActive], [Priority], [IsRecurring], [RecurringInterval], [UserInteraction], [CreatedOn], [CreatedBy]) VALUES
(N'eceb13d9-0044-4368-a8b3-04cc16c45500', N'ProjectModification.Edit', N'133fab36-1bae-4278-af92-2835578f5021',
N'[{CONTRACT_NUMBER}] :A Task Order Modification has been updated.', N'
<style>
    * {
        margin: 0;
        padding: 0;
    }

    ul {
        list-style-position: inside;
    }
</style>
<table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
    <tr>
        <td>
            <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                <tr>
                    <td>
                        Dear<b style="color:#00649B">
                            {RECEIVER_DISPLAY_NAME}
                        </b>,
                        <br>
                        <br>

                        <p style="line-height:1.6;">{SUBMITTED_NAME} has added a Mod in the Contract Management System (CMS). You have been identified as someone who needs to know this. You can see the contract details by clicking the button below. If you have any questions, please contact {SUBMITTED_NAME} for more information.</p>
                        <br>
                        <table style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                            <tr>
                                <td>
                                    <table style="border-collapse: collapse;width:100%">
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Name:</td>
                                            <td style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_TITLE}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Number:</td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_NUMBER}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Task Order
                                                Number:</td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {TASKORDER_NUMBER}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Project Number:</td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {PROJECT_NUMBER}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Modification Title:</td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {MODIFICATION_TITLE}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Modification Number:</td>
                                            <td style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {MODIFICATION_NUMBER}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Description:</td>
                                            <td style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_DESCRIPTION}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding:0; font-size: 12px;line-height: 1.5;">
                                                <div style="margin-top:20px; padding:20px; border-top:1px solid #eee;">
                                                    <div><span style="color:#aaa;">Additional Message:</span></div>
                                                    <div>
                                                        {ADDITIONAL_MESSAGE}
                                                    </div>
                                                </div>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td colspan="2" style="font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;">
                                                <span style="display:block; padding-bottom:5px;">Details of the updated task order Mod</span><a
                                                    style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                    href="{LINK}">view detail</a></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="font-size: 12px;line-height: 1.5; background:#f5f5f5; padding:20px; border-top:1px solid #eee;">
                                                    <div><span style="color:#aaa;">Additional people notified:</span></div>
                                                    <div>
                                                        {ADDITIONAL_RECIPIENT}
                                                    </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>', 1, 0, 0, 0, 1, CAST(N'2012-12-12T00:00:00.000' AS DateTime),
N'133fab36-1bae-4278-af92-2835578f5021')
INSERT [dbo].[NotificationTemplate] ([NotificationTemplateGuid], [Keys], [NotificationTypeGuid],
[Subject], [Message],
[IsActive], [Priority], [IsRecurring], [RecurringInterval], [UserInteraction], [CreatedOn],
[CreatedBy]) VALUES
(N'aece671f-569d-4e17-b2bc-0dc7a46a4604', N'ContractCloseOut.Notify',
N'133fab36-1bae-4278-af92-2835578f5021',
N'[{PROJECT_NUMBER}] : The Contract need to be closed.', N'<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>Document</title>
</head>

<body>
    <style>
        * {
            margin: 0;
            padding: 0;
        }

        ul {
            list-style-position: inside;
        }
    </style>
    <table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
        <tr>
            <td>
                <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                    <tr>
                        <td> Dear<b style="color:#00649B"> {RECEIVER_NAME} </b>, <br> <br>
                            <p style="line-height:1.6;"> {SUBMITTED_NAME} has started a contract closeout in the Contract Management System (CMS). You have been identified as someone who needs to provide information as part of the closeout process. You can see the contract details by clicking the button below. If you have any questions, please contact {SUBMITTED_NAME} for more information.</p>
                            <br>
                            <table
                                style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                                <tr>
                                    <td>
                                        <table style="border-collapse: collapse;width:100%">
                                            <tbody>
                                                <tr>
                                                    <td style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;" colspan="2">
                                                        The Contract Close out form is need to be filled by answering some of the required questions.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Title:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {CONTRACT_TITLE}
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Number:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {CONTRACT_NUMBER}
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Project Number:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {PROJECT_NUMBER}
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Type:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {CONTRACT_TYPE}
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Description:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {CONTRACT_DESCRIPTION}
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5;">
                                                        <div
                                                            style="margin-top:20px; padding:20px; border-top:1px solid #eee;">
                                                            <div><span style="color:#aaa;">Additional Message:</span></div>
                                                            <div>
                                                                {ADDITIONAL_MESSAGE}
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;"
                                                        colspan="2"> <span
                                                            style="display:block; padding-bottom:5px;"></span><a
                                                            style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                            href="{Redirect_Url}"> Go To Contract
                                                            Details </a>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"
                                                        style="background:#f5f5f5; padding:20px; border-top:1px solid #eee; font-size: 12px;line-height: 1.5;">
                                                            <div><span style="color:#aaa;">Additional
                                                                    people notified:</span></div>
                                                            <div>{NOTIFY_OTHER}
                                                            </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>

</html>', 1, 0, 0, 0, 1, CAST(N'2019-06-06T00:00:00.000' AS DateTime),
N'133fab36-1bae-4278-af92-2835578f5021')
INSERT [dbo].[NotificationTemplate] ([NotificationTemplateGuid], [Keys], [NotificationTypeGuid],
[Subject], [Message],
[IsActive], [Priority], [IsRecurring], [RecurringInterval], [UserInteraction], [CreatedOn],
[CreatedBy]) VALUES
(N'1c97717a-c098-48f6-b32d-1879dadbeedf', N'Project.Notify',
N'133fab36-1bae-4278-af92-2835578f5021',
N'[{CONTRACT_NUMBER}] : The Notification About Contract.', N'<style>
    * {
        margin: 0;
        padding: 0;
    }

    ul {
        list-style-position: inside;
    }
</style>
<table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
    <tr>
        <td>
            <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                <tr>
                    <td>
                        Dear<b style="color:#00649B">
                            {RECEIVER_DISPLAY_NAME}
                        </b>,
                        <br>
                        <br>

                        <p style="line-height:1.6;">{SUBMITTED_NAME} wants to notify you about a contract in the Contract Management System (CMS). You can see their note below, and can see the contract details by clicking the button below. If you have any questions, please contact {SUBMITTED_NAME} for more information.</p>
                        <br>
                        <table
                            style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                            <tr>
                                <td>

                                    <table style="border-collapse: collapse;width:100%">
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Name:</td>
                                            <td width="200"
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_TITLE}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Number:</td>
                                            <td width="200"
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_NUMBER}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Project Number:</td>
                                            <td width="200"
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {PROJECT_NUMBER}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract
                                                Description:</td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_DESCRIPTION}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="2" style="padding:0; font-size: 12px;line-height: 1.5;">
                                                <div style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                    <div><span style="color:#aaa;">Additional Message:</span></div>
                                                    <div>
                                                        {ADDITIONAL_MESSAGE}
                                                    </div>
                                                </div>
                                            </td>

                                        </tr>

                                        <tr>
                                            <td style="text-align:center; padding-top:30px" colspan="2">
                                                <span style="display:block; padding-bottom:5px;">Details
                                                    of the Updated Contract:</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"
                                                style="font-size: 14px;line-height: 2.5; color:#fff; text-align:center; padding-bottom:30px; padding-top:5px;">
                                                <table cellpadding="0" cellspacing="0" border="0" style="width:100%">
                                                    <tr>
                                                        <td></td>
                                                        <td style="background:#00649B; text-align:center; border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; height:40px;"
                                                            width="200px">
                                                            <a style="padding:10px 0; color:#fff; display:block;  text-decoration: none;"
                                                                href="{LINK}">view detail</a></td>
                                                        <td></td>
                                                    </tr>
                                                </table>

                                            </td>

                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding:0; font-size: 12px;line-height: 1.5; margin-top:20px; padding:20px; border-top:1px solid #eee;">
                                                    <div><span style="color:#aaa;">Additional people notified:</span></div>
                                                    <div>
                                                        {ADDITIONAL_RECIPIENT}
                                                    </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>', 1, 0, 0, 0, 1, CAST(N'2012-12-12T00:00:00.000' AS DateTime),
N'133fab36-1bae-4278-af92-2835578f5021')
INSERT [dbo].[NotificationTemplate] ([NotificationTemplateGuid], [Keys],
[NotificationTypeGuid], [Subject], [Message],
[IsActive], [Priority], [IsRecurring], [RecurringInterval],
[UserInteraction], [CreatedOn], [CreatedBy]) VALUES
(N'96eeaa8a-0b57-4e2f-ac65-1b9708701999',
N'RevenueRecognition.ContractReview',
N'133fab36-1bae-4278-af92-2835578f5021',
N'[{PROJECT_NUMBER}] : A Revenue Recognition has been added.', N'<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>Document</title>
</head>

<body>
    <style>
        * {
            margin: 0;
            padding: 0;
        }

        ul {
            list-style-position: inside;
        }
    </style>
    <table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
        <tr>
            <td>
                <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                    <tr>
                        <td> Dear<b style="color:#00649B">
                                {RECEIVER_NAME} </b>, <br> <br>
                            <p style="line-height:1.6;"> A revenue
                                recognition form has been submitted
                                for the review.</p> <br>
                            <table
                                style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                                <tr>
                                    <td>
                                        <table style="border-collapse: collapse;width:100%">
                                            <tbody>
                                                <tr>
                                                    <td style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;"
                                                        colspan="2"> {REQUESTEDBY_NAME} has submitted the revenue recognition form for review. Please complete the remaining steps.
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Project Number:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {PROJECT_NUMBER}
                                                    </td>
                                                </tr>


                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Title:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {TITLE} </td>
                                                </tr>

                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Type: </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {CONTRACT_TYPE}
                                                    </td>
                                                </tr>


                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5;">
                                                        <div
                                                            style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                            <div><span style="color:#aaa;">Additional Message:</span>
                                                            </div>
                                                            <div>
                                                                {ADDITIONAL_MESSAGE}
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;"
                                                        colspan="2"> <span
                                                            style="display:block; padding-bottom:5px;"></span><a
                                                            style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                            href="{Redirect_Url}">
                                                            Go To Contract
                                                            Details </a>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5; background:#f5f5f5; padding:20px; border-top:1px solid #eee;">
                                                            <div><span style="color:#aaa;">Additional people notified:</span>
                                                            </div>
                                                            <div>
                                                                {NOTIFY_OTHER}
                                                            </div>
                                                    </td>
                                                </tr>

                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>

</html>', 1, 0, 0, 0, 1, CAST(N'2012-12-12T00:00:00.000' AS DateTime),
N'133fab36-1bae-4278-af92-2835578f5021')
INSERT [dbo].[NotificationTemplate] ([NotificationTemplateGuid], [Keys],
[NotificationTypeGuid], [Subject], [Message],
[IsActive], [Priority], [IsRecurring], [RecurringInterval],
[UserInteraction], [CreatedOn], [CreatedBy]) VALUES
(N'56d39cc2-b836-448a-8284-2284220c0bc2',
N'RevenueRecognition.TaskOrderCreate',
N'133fab36-1bae-4278-af92-2835578f5021', N'[{PROJECT_NUMBER}] : New
Revenue Recognition Form Is Required.', N'<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>Document</title>
</head>

<body>
    <style>
        * {
            margin: 0;
            padding: 0;
        }

        ul {
            list-style-position: inside;
        }
    </style>
    <table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
        <tr>
            <td>
                <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                    <tr>
                        <td> Dear<b style="color:#00649B">
                                {RECEIVER_NAME} </b>, <br> <br>
                            <p style="line-height:1.6;"> A new
                                revenue recognition form is required
                                for [{PROJECT_NUMBER}:{TITLE}]
                                because of the following
                                change.</p> <br>
                            <table
                                style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                                <tr>
                                    <td>

                                        <table style="border-collapse: collapse;width:100%">
                                            <tbody>
                                                <tr>
                                                    <td style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;"
                                                        colspan="2">
                                                        {REQUESTEDBY_NAME}
                                                        updated
                                                        [{PROJECT_NUMBER}:{CONTRACT_TITLE}].The
                                                        total award amount
                                                        for this task order
                                                        now exceeds the
                                                        threshold of
                                                        {ThresholdAward_Amount}
                                                        set for Contract
                                                        type
                                                        [{CONTRACT_TYPE}].
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Project Number:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {PROJECT_NUMBER}
                                                    </td>
                                                </tr>


                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Task Order Title:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {TITLE} </td>
                                                </tr>

                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Type: </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {CONTRACT_TYPE}
                                                    </td>
                                                </tr>


                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5;">
                                                        <div
                                                            style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                            <div><span style="color:#aaa;">Additional Message:</span>
                                                            </div>
                                                            <div>
                                                                {ADDITIONAL_MESSAGE}
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;"
                                                        colspan="2"> <span
                                                            style="display:block; padding-bottom:5px;"></span><a
                                                            style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                            href="{Redirect_Url}">
                                                            Go To Contract
                                                            Details </a>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5; background:#f5f5f5; padding:20px; border-top:1px solid #eee;">
                                                            <div><span style="color:#aaa;">Additional people notified:</span>
                                                            </div>
                                                            <div>
                                                                {NOTIFY_OTHER}
                                                        </div>
                                                    </td>
                                                </tr>

                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>

</html>', 1, 0, 0, 0, 1, CAST(N'2012-12-12T00:00:00.000' AS DateTime),
N'133fab36-1bae-4278-af92-2835578f5021')
INSERT [dbo].[NotificationTemplate] ([NotificationTemplateGuid], [Keys],
[NotificationTypeGuid], [Subject], [Message],
[IsActive], [Priority], [IsRecurring], [RecurringInterval],
[UserInteraction], [CreatedOn], [CreatedBy]) VALUES
(N'cf7cdd12-1799-4db0-a7a3-2bae49aaa43b',
N'ContractModification.Create', N'133fab36-1bae-4278-af92-2835578f5021',
N'[{CONTRACT_NUMBER}] :A Contract Modification has been added.', N'
<style>
    * {
        margin: 0;
        padding: 0;
    }

    ul {
        list-style-position: inside;
    }
</style>
<table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
    <tr>
        <td>
            <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                <tr>
                    <td>
                        Dear<b style="color:#00649B">
                            {RECEIVER_DISPLAY_NAME}
                        </b>,
                        <br>
                        <br>

                        <p style="line-height:1.6;">{SUBMITTED_NAME} has added a Mod in the Contract Management System (CMS). You have been identified as someone who needs to know this. You can see the contract details by clicking the button below. If you have any questions, please contact {SUBMITTED_NAME} for more information.</p>
                        <br>
                        <table
                            style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                            <tr>
                                <td>

                                    <table style="border-collapse: collapse;width:100%">
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Name:</td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_TITLE}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Number:</td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_NUMBER}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Project Number:</td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {PROJECT_NUMBER}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Mod Number:</td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {MODIFICATION_NUMBER}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Mod Title:</td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {MODIFICATION_TITLE}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Description:</td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_DESCRIPTION}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding:0; font-size: 12px;line-height: 1.5;">
                                                <div style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                    <div><span style="color:#aaa;">Additional Message:</span>
                                                    </div>
                                                    <div>
                                                        {ADDITIONAL_MESSAGE}
                                                    </div>
                                                </div>
                                            </td>

                                        </tr>

                                        <tr>
                                            <td colspan="2"
                                                style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;">
                                                <span style="display:block; padding-bottom:5px;"></span><a
                                                    style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                    href="{LINK}">view
                                                    details</a></td>

                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding:0; font-size: 12px;line-height: 1.5; background:#f5f5f5; padding:20px; border-top:1px solid #eee;">
                                                    <div><span style="color:#aaa;">Additional people notified:</span>
                                                    </div>
                                                    <div>
                                                        {ADDITIONAL_RECIPIENT}
                                                    </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>', 1, 0, 0, 0, 1,
CAST(N'2012-12-12T00:00:00.000' AS DateTime), N'133fab36-1bae-4278-af92-2835578f5021') INSERT [dbo].[NotificationTemplate] ([NotificationTemplateGuid], [Keys], [NotificationTypeGuid], [Subject], [Message], [IsActive], [Priority], [IsRecurring], [RecurringInterval], [UserInteraction], [CreatedOn], [CreatedBy]) VALUES (N'3850ddca-610a-48c6-8112-42643f1487fa', N'RevenueRecognition.ContractModUpdate', N'133fab36-1bae-4278-af92-2835578f5021', N'[{PROJECT_NUMBER}] : New Revenue Recognition Form Is Required.', N'
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>Document</title>
</head>

<body>
    <style>
        * {
            margin: 0;
            padding: 0;
        }

        ul {
            list-style-position: inside;
        }
    </style>
    <table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
        <tr>
            <td>
                <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                    <tr>
                        <td> Dear<b style="color:#00649B">
                                {RECEIVER_NAME}
                            </b>, <br> <br>
                            <p style="line-height:1.6;"> A new revenue recognition form is required for [{PROJECT_NUMBER}:{TITLE}] because of the following change.</p>
                            <br>
                            <table
                                style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                                <tr>
                                    <td>

                                        <table style="border-collapse: collapse;width:100%">
                                            <tbody>
                                                <tr>
                                                    <td style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;"
                                                        colspan="2"> {REQUESTEDBY_NAME} updated [{PROJECT_NUMBER}:{TITLE}]. The total award amount for this contract now exceeds the threshold of {ThresholdAward_Amount} set for Contract type {Contract_type}.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Project Number:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {PROJECT_NUMBER}
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Title:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {TITLE}
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Type:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {CONTRACT_TYPE}
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Modification Number:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {MOD_NUMBER}
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5;">
                                                        <div
                                                            style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                            <div>
                                                                <span style="color:#aaa;">Additional Message:</span>
                                                            </div>
                                                            <div>
                                                                {ADDITIONAL_MESSAGE}
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;"
                                                        colspan="2">
                                                        <span style="display:block; padding-bottom:5px;"></span><a
                                                            style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                            href="{Redirect_Url}">
                                                            Go To Contract Details
                                                        </a>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5; background:#f5f5f5; padding:20px; border-top:1px solid #eee">
                                                            <div>
                                                                <span style="color:#aaa;">Additional people notified:</span>
                                                            </div>
                                                            <div>
                                                                {NOTIFY_OTHER}
                                                            </div>
                                                    </td>
                                                </tr>

                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>

</html>', 1, 0, 0, 0, 1,
CAST(N'2012-12-12T00:00:00.000' AS DateTime), N'133fab36-1bae-4278-af92-2835578f5021') INSERT [dbo].[NotificationTemplate] ([NotificationTemplateGuid], [Keys], [NotificationTypeGuid], [Subject], [Message], [IsActive], [Priority], [IsRecurring], [RecurringInterval], [UserInteraction], [CreatedOn], [CreatedBy]) VALUES (N'43588caf-7da5-4f8e-bc71-51f26b981e24', N'JobRequest.Notify', N'133fab36-1bae-4278-af92-2835578f5021', N'[{CONTRACT_NUMBER}] :The Notification About Job Request.', N'<style>
    * {
        margin: 0;
        padding: 0;
    }

    ul {
        list-style-position: inside;
    }
</style>
<table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
    <tr>
        <td>
            <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                <tr>
                    <td>
                        Dear<b style="color:#00649B">
                            {RECEIVER_NAME}
                        </b>,
                        <br>
                        <br>

                        <p style="line-height:1.6;"> {SUBMITTED_NAME} has submitted a new Job Request Form in the Contract Management System (CMS). You have been identified as someone who needs to add additional information to complete the submission process. You can see the contract details by clicking the button below. If you have any questions, please contact {SUBMITTED_NAME} for more information.
                        </p>
                        <br>

                        <table
                            style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                            <tr>
                                <td>

                                    <table style="border-collapse: collapse;width:100%">
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Name:
                                            </td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_TITLE}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Number:
                                            </td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_NUMBER}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Project Number:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {PROJECT_NUMBER}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Task Order Number:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {TASK_ORDERNUMBER}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td
                                                style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Key Personnel:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {KEY_PERSONNELLIST}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Description:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_DESCRIPTION}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="2" style="padding:0; font-size: 12px;line-height: 1.5;">
                                                <div style="margin-top:20px; padding:20px; border-top:1px solid #eee;">
                                                    <div>
                                                        <span style="color:#aaa;">Additional Message:</span>
                                                    </div>
                                                    <div>
                                                        {ADDITIONAL_MESSAGE}
                                                    </div>
                                                </div>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td colspan="2"
                                                style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;">
                                                <span style="display:block; padding-bottom:5px;"></span><a
                                                    style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                    href="{JOBREQUEST_URL}">view details</a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding:0; font-size: 12px;line-height: 1.5; background:#f5f5f5; padding:20px; border-top:1px solid #eee;">
                                                    <div>
                                                        <span style="color:#aaa;">This email has been copied to:</span>
                                                    </div>
                                                    <div style="margin-bottom:20px;">
                                                        {NOTIFY_TO}
                                                    </div>
                                                    <div>
                                                        <span style="color:#aaa;">Additional people notified:</span>
                                                    </div>
                                                    <div>
                                                        {NOTIFY_OTHER}
                                                    </div>
                                            </td>

                                        </tr>

                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>', 1, 0, 0, 0, 1, CAST(N'2012-06-06T00:00:00.000' AS DateTime), N'03daa37f-eb04-479e-a3aa-012fa95c24e3') INSERT [dbo].[NotificationTemplate] ([NotificationTemplateGuid], [Keys], [NotificationTypeGuid], [Subject], [Message], [IsActive], [Priority], [IsRecurring], [RecurringInterval], [UserInteraction], [CreatedOn], [CreatedBy]) VALUES (N'2c125fd9-45a2-4ad5-9059-618aab99d3e7', N'RevenueRecognition.ContractUpdate', N'133fab36-1bae-4278-af92-2835578f5021', N'[{PROJECT_NUMBER}] : New Revenue Recognition Form Is Required', N'
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>
        Document
    </title>
</head>

<body>
    <style>
        * {
            margin: 0;
            padding: 0;
        }

        ul {
            list-style-position: inside;
        }
    </style>
    <table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
        <tr>
            <td>
                <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                    <tr>
                        <td> Dear<b style="color:#00649B">
                                {RECEIVER_NAME}
                            </b>,
                            <br>
                            <br>
                            <p style="line-height:1.6;"> A new revenue recognition form is required for [{PROJECT_NUMBER}:{TITLE}] because of the following change.
                            </p>
                            <br>
                            <table
                                style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                                <tr>
                                    <td>

                                        <table style="border-collapse: collapse;width:100%">
                                            <tbody>
                                                <tr>
                                                    <td style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;"
                                                        colspan="2"> {REQUESTEDBY_NAME} updated [{PROJECT_NUMBER}:{TITLE}]. The total award amount for this contract now exceeds the threshold of {ThresholdAward_Amount} set for Contract type {CONTRACT_TYPE}.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Project Number:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {PROJECT_NUMBER}
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Title:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {TITLE}
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Type:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {CONTRACT_TYPE}
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5;">
                                                        <div
                                                            style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                            <div>
                                                                <span style="color:#aaa;">Additional Message:</span>
                                                            </div>
                                                            <div>
                                                                {ADDITIONAL_MESSAGE}
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;"
                                                        colspan="2">
                                                        <span style="display:block; padding-bottom:5px;"></span><a
                                                            style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                            href="{Redirect_Url}"> Go To Contract Details
                                                        </a>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5; background:#f5f5f5; padding:20px; border-top:1px solid #eee;">
                                                            <div>
                                                                <span style="color:#aaa;">Additional people notified:</span>
                                                            </div>
                                                            <div>
                                                                {NOTIFY_OTHER}
                                                            </div>
                                                    </td>
                                                </tr>

                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>

</html>', 1, 0, 0, 0, 1, CAST(N'2012-12-12T00:00:00.000' AS DateTime), N'133fab36-1bae-4278-af92-2835578f5021') INSERT [dbo].[NotificationTemplate] ([NotificationTemplateGuid], [Keys], [NotificationTypeGuid], [Subject], [Message], [IsActive], [Priority], [IsRecurring], [RecurringInterval], [UserInteraction], [CreatedOn], [CreatedBy]) VALUES (N'786cc657-16fd-4175-acc8-6bbd2cfefd87', N'Project.Edit', N'133fab36-1bae-4278-af92-2835578f5021', N'[{CONTRACT_NUMBER}] : A Task Order has been updated.', N'<style>
    * {
        margin: 0;
        padding: 0;
    }

    ul {
        list-style-position: inside;
    }
</style>
<table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
    <tr>
        <td>
            <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                <tr>
                    <td>
                        Dear<b style="color:#00649B">
                            {RECEIVER_DISPLAY_NAME}
                        </b>,
                        <br>
                        <br>

                        <p style="line-height:1.6;">
                            {SUBMITTED_NAME} has submitted a new Job Request Form in the Contract Management System (CMS). You have been identified as someone who needs to add additional information to complete the submission process. You can see the contract details by clicking the button below. If you have any questions, please contact {SUBMITTED_NAME} for more information.
                        </p>
                        <br>
                        <table
                            style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                            <tr>
                                <td>

                                    <table style="border-collapse: collapse;width:100%">
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Name:
                                            </td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_TITLE}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Number:
                                            </td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_NUMBER}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Task
                                                Order
                                                Number:
                                            </td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {TASKORDER_NUMBER}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Project Number:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {PROJECT_NUMBER}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Awarding
                                                Agency:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {AWARDING_AGENCY}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Funding
                                                Agency:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {FUNDING_AGENCY}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract
                                                Desctiption:
                                            </td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_DESCRIPTION}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding:0; font-size: 12px;line-height: 1.5;">
                                                <div style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                    <div>
                                                        <span style="color:#aaa;">Additional Message:</span>
                                                    </div>
                                                    <div>
                                                        {ADDITIONAL_MESSAGE}
                                                    </div>
                                                </div>
                                            </td>

                                        </tr>

                                        <tr>
                                            <td colspan="2"
                                                style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;">
                                                <span style="display:block; padding-bottom:5px;">Details of the updated Task order</span><a style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;" href="{LINK}">view detail</a>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding:0; font-size: 12px;line-height: 1.5;">
                                                <div
                                                    style="background:#f5f5f5; padding:20px; border-top:1px solid #eee;">
                                                    <div>
                                                        <span style="color:#aaa;">Additional people notified:</span>
                                                    </div>
                                                    <div>
                                                        {ADDITIONAL_RECIPIENT}
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
', 1, 0, 0, 0, 1, CAST(N'2012-12-12T00:00:00.000' AS DateTime), N'133fab36-1bae-4278-af92-2835578f5021') INSERT [dbo].[NotificationTemplate] ([NotificationTemplateGuid], [Keys], [NotificationTypeGuid], [Subject], [Message], [IsActive], [Priority], [IsRecurring], [RecurringInterval], [UserInteraction], [CreatedOn], [CreatedBy]) VALUES (N'fe7e8801-ed88-4d8d-9d9a-7673fdd93751', N'Contract.Create', N'133fab36-1bae-4278-af92-2835578f5021', N'[{CONTRACT_NUMBER}] :A Contract has been added.', N'
<style>
    * {
        margin: 0;
        padding: 0;
    }

    ul {
        list-style-position: inside;
    }
</style>
<table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
    <tr>
        <td>
            <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                <tr>
                    <td>
                        Dear<b style="color:#00649B">
                            {RECEIVER_DISPLAY_NAME}
                        </b>,
                        <br>
                        <br>

                        <p style="line-height:1.6;"> {SUBMITTED_NAME} has added a new contract in Contract Management System (CMS). You have been identified as someone who would need to know this. You can see the contract details by clicking the button below. If you have any questions, please contact {SUBMITTED_NAME} for more information.
                        </p>
                        <br>
                        <table
                            style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                            <tr>
                                <td>

                                    <table style="border-collapse: collapse;width:100%">
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Name:
                                            </td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_TITLE}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Number:
                                            </td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_NUMBER}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Project Number:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {PROJECT_NUMBER}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Awarding
                                                Agency:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {AWARDING_AGENCY}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Funding
                                                Agency:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {FUNDING_AGENCY}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract
                                                Description:
                                            </td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_DESCRIPTION}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding:0; font-size: 12px;line-height: 1.5;">
                                                <div style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                    <div>
                                                        <span style="color:#aaa;">Additional Message:</span>
                                                    </div>
                                                    <div>
                                                        {ADDITIONAL_MESSAGE}
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"
                                                style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;">
                                                <span style="display:block; padding-bottom:5px;"></span><a
                                                    style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                    href="{LINK}">view
                                                    details</a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding:0; font-size: 12px;line-height: 1.5;">
                                                <div
                                                    style="background:#f5f5f5; padding:20px; border-top:1px solid #eee;">
                                                    <div>
                                                        <span style="color:#aaa;">Additional people notified:</span>
                                                    </div>
                                                    <div>
                                                        {ADDITIONAL_RECIPIENT}
                                                    </div>
                                                </div>
                                            </td>

                                        </tr>

                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
',
1,
0,
0,
0,
1,
CAST(N'2012-12-12T00:00:00.000'
AS
DateTime),
N'133fab36-1bae-4278-af92-2835578f5021')
INSERT
[dbo].[NotificationTemplate]
([NotificationTemplateGuid],
[Keys],
[NotificationTypeGuid],
[Subject],
[Message],
[IsActive],
[Priority],
[IsRecurring],
[RecurringInterval],
[UserInteraction],
[CreatedOn],
[CreatedBy])
VALUES
(N'6238e019-a69c-45d9-a173-7708a0d07d2d',
N'ContractModification.Edit',
N'133fab36-1bae-4278-af92-2835578f5021',
N'[{CONTRACT_NUMBER}]
:
A
Contract
Modification
has
been
updated.',
N'
<style>
    * {
        margin: 0;
        padding: 0;
    }

    ul {
        list-style-position: inside;
    }
</style>
<table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
    <tr>
        <td>
            <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                <tr>
                    <td>
                        Dear<b style="color:#00649B">
                            {RECEIVER_DISPLAY_NAME}
                        </b>,
                        <br>
                        <br>

                        <p style="line-height:1.6;"> {SUBMITTED_NAME} has updated a Mod in the Contract Management System (CMS). You have been identified as someone who needs to know this. You can see the contract details by clicking the button below. If you have any questions, please contact {SUBMITTED_NAME} for more information.
                        </p>
                        <br>
                        <table
                            style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                            <tr>
                                <td>

                                    <table style="border-collapse: collapse;width:100%">
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract
                                                Title:
                                            </td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_TITLE}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Number:
                                            </td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_NUMBER}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Project Number:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {PROJECT_NUMBER}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Mod
                                                Number:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {MODIFICATION_NUMBER}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Mod
                                                Title:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {MODIFICATION_TITLE}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract
                                                Description:
                                            </td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_DESCRIPTION}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding:0; font-size: 12px;line-height: 1.5;">
                                                <div style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                    <div>
                                                        <span style="color:#aaa;">Additional Message:</span>
                                                    </div>
                                                    <div>
                                                        {ADDITIONAL_MESSAGE}
                                                    </div>
                                                </div>
                                            </td>

                                        </tr>

                                        <tr>
                                            <td colspan="2"
                                                style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;">
                                                <span style="display:block; padding-bottom:5px;"></span><a
                                                    style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                    href="{LINK}">view
                                                    details</a>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding:0; font-size: 12px;line-height: 1.5;">
                                                <div
                                                    style="background:#f5f5f5; padding:20px; border-top:1px solid #eee;">
                                                    <div>
                                                        <span style="color:#aaa;">Additional people notified:</span>
                                                    </div>
                                                    <div>
                                                        {ADDITIONAL_RECIPIENT}
                                                    </div>
                                                </div>
                                            </td>

                                        </tr>

                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
',
1,
0,
0,
0,
1,
CAST(N'2012-12-12T00:00:00.000'
AS
DateTime),
N'133fab36-1bae-4278-af92-2835578f5021')
INSERT
[dbo].[NotificationTemplate]
([NotificationTemplateGuid],
[Keys],
[NotificationTypeGuid],
[Subject],
[Message],
[IsActive],
[Priority],
[IsRecurring],
[RecurringInterval],
[UserInteraction],
[CreatedOn],
[CreatedBy])
VALUES
(N'23de6364-2d0c-4bdb-8afd-856912f75bc4',
N'RevenueRecognition.TaskOrderReview',
N'133fab36-1bae-4278-af92-2835578f5021',
N'[{PROJECT_NUMBER}]
:
New
Revenue
Recognition
has
been
added.',
N'
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>
        Document
    </title>
</head>

<body>
    <style>
        * {
            margin: 0;
            padding: 0;
        }

        ul {
            list-style-position: inside;
        }
    </style>
    <table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
        <tr>
            <td>
                <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                    <tr>
                        <td> Dear<b style="color:#00649B">
                                {RECEIVER_NAME}
                            </b>,
                            <br>
                            <br>
                            <p style="line-height:1.6;"> A revenue recognition form has been submitted for the review.
                            </p>
                            <br>
                            <table
                                style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                                <tr>
                                    <td>

                                        <table style="border-collapse: collapse;width:100%">
                                            <tbody>
                                                <tr>
                                                    <td style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;"
                                                        colspan="2"> {REQUESTEDBY_NAME} has submitted the revenue recognition form for review. Please complete the remaining steps.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Project Number:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {PROJECT_NUMBER}
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;"> Task Order Title:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {TASKORDER_TITLE}
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Type:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {CONTRACT_TYPE}
                                                    </td>
                                                </tr>


                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5;">
                                                        <div
                                                            style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                            <div>
                                                                <span style="color:#aaa;">Additional Message:</span>
                                                            </div>
                                                            <div>
                                                                {ADDITIONAL_MESSAGE}
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;"
                                                        colspan="2">
                                                        <span style="display:block; padding-bottom:5px;"></span><a
                                                            style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                            href="{Redirect_Url}">
                                                            Go To Contract Details
                                                        </a>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5; background:#f5f5f5; padding:20px; border-top:1px solid #eee;">
                                                            <div>
                                                                <span style="color:#aaa;">Additional people notified:</span>
                                                            </div>
                                                            <div>
                                                                {NOTIFY_OTHER}
                                                            </div>
                                                    </td>
                                                </tr>

                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>

</html>
',
1,
0,
0,
0,
1,
CAST(N'2012-12-12T00:00:00.000'
AS
DateTime),
N'133fab36-1bae-4278-af92-2835578f5021')
INSERT
[dbo].[NotificationTemplate]
([NotificationTemplateGuid],
[Keys],
[NotificationTypeGuid],
[Subject],
[Message],
[IsActive],
[Priority],
[IsRecurring],
[RecurringInterval],
[UserInteraction],
[CreatedOn],
[CreatedBy])
VALUES
(N'01c08d52-32eb-4385-a307-88c27e033ca9',
N'RevenueRecognition.TaskOrderUpdate',
N'133fab36-1bae-4278-af92-2835578f5021',
N'[{PROJECT_NUMBER}]
:
New
Revenue
Recognition
Form
Is
Required.',
N'
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>
        Document
    </title>
</head>

<body>
    <div>
        <style>
            * {
                margin: 0;
                padding: 0;
            }

            ul {
                list-style-position: inside;
            }
        </style>
        <table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
            <tr>
                <td>
                    <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                        <tr>
                            <td> Dear<b style="color:#00649B">
                                    {RECEIVER_NAME}
                                </b>,
                                <br>
                                <br>
                                <p style="line-height:1.6;"> A new revenue recognition form is required for [{PROJECT_NUMBER}:{TITLE}] because of the following change.
                                </p>
                                <br>
                                <table
                                    style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                                    <tr>
                                        <td>

                                            <table style="border-collapse: collapse;width:100%">
                                                <tbody>
                                                    <tr>
                                                        <td style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;"
                                                            colspan="2"> {REQUESTEDBY_NAME} updated [{PROJECT_NUMBER}:{TITLE}]. The total award amount for this task order now exceeds the threshold of {ThresholdAward_Amount} set for Contract type {CONTRACT_TYPE}.
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td width="200"
                                                            style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                            Project Number:
                                                        </td>
                                                        <td
                                                            style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                            {PROJECT_NUMBER}
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td width="200"
                                                            style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                            Task Order Title:
                                                        </td>
                                                        <td
                                                            style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                            {TITLE}
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td width="200"
                                                            style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                            Contract Type:
                                                        </td>
                                                        <td
                                                            style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                            {CONTRACT_TYPE}
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td colspan="2"
                                                            style="padding:0; font-size: 12px;line-height: 1.5;">
                                                            <div
                                                                style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                                <div>
                                                                    <span style="color:#aaa;">Additional
                                                                        Message:</span>
                                                                </div>
                                                                <div>
                                                                    {ADDITIONAL_MESSAGE}
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;"
                                                            colspan="2">
                                                            <span style="display:block; padding-bottom:5px;"></span><a
                                                                style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                                href="{Redirect_Url}">
                                                                Go To Contract Details
                                                            </a>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"
                                                            style="padding:0; font-size: 12px;line-height: 1.5;">
                                                            <div
                                                                style="background:#f5f5f5; padding:20px; border-top:1px solid #eee;">

                                                                <div>
                                                                    <span style="color:#aaa;">Additional people notified:</span>
                                                                </div>
                                                                <div>
                                                                    {NOTIFY_OTHER}
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table> </td>
                        </tr>
                    </tbody>
                </table> </td>
            </tr>
        </tbody>
    </table>
    </div>
</body>

</html>
',
1,
0,
0,
0,
1,
CAST(N'2012-12-12T00:00:00.000'
AS
DateTime),
N'133fab36-1bae-4278-af92-2835578f5021')
INSERT
[dbo].[NotificationTemplate]
([NotificationTemplateGuid],
[Keys],
[NotificationTypeGuid],
[Subject],
[Message],
[IsActive],
[Priority],
[IsRecurring],
[RecurringInterval],
[UserInteraction],
[CreatedOn],
[CreatedBy])
VALUES
(N'e9a8162d-e328-4c7f-884e-8bc0b11515fb',
N'RevenueRecognition.TaskOrderModUpdate',
N'133fab36-1bae-4278-af92-2835578f5021',
N'[{PROJECT_NUMBER}]
:
New
Revenue
Recognition
Is
Required.',
N'
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>
        Document
    </title>
</head>

<body>
    <style>
        * {
            margin: 0;
            padding: 0;
        }

        ul {
            list-style-position: inside;
        }
    </style>
    <table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
        <tr>
            <td>
                <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                    <tr>
                        <td> Dear<b style="color:#00649B">
                                {RECEIVER_NAME}
                            </b>,
                            <br>
                            <br>
                            <p style="line-height:1.6;"> A new revenue recognition form is required for [{PROJECT_NUMBER}:{TITLE}] because of the following change.
                            </p>
                            <br>
                            <table
                                style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                                <tr>
                                    <td>

                                        <table style="border-collapse: collapse;width:100%">
                                            <tbody>
                                                <tr>
                                                    <td style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;"
                                                        colspan="2"> {REQUESTEDBY_NAME} updated [{PROJECT_NUMBER}:{TITLE}], the total award amount for this task order now exceeds the threshold of {ThresholdAward_Amount} set for Contract type {Contract_type}.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Project Number:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {PROJECT_NUMBER}
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Task
                                                        Order
                                                        Title:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {TITLE}
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Type:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {CONTRACT_TYPE}
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;"> Task Order Modification Number:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {MOD_NUMBER}
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5;">
                                                        <div
                                                            style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                            <div>
                                                                <span style="color:#aaa;">Additional Message:</span>
                                                            </div>
                                                            <div>
                                                                {ADDITIONAL_MESSAGE}
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;"
                                                        colspan="2">
                                                        <span style="display:block; padding-bottom:5px;"></span><a
                                                            style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                            href="{Redirect_Url}">
                                                            Go To Contract Details
                                                        </a>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5; background:#f5f5f5; padding:20px; border-top:1px solid #eee;">
                                                            <div>
                                                                <span style="color:#aaa;">Additional people notified:</span>
                                                            </div>
                                                            <div>
                                                                {NOTIFY_OTHER}
                                                            </div>
                                                    </td>
                                                </tr>

                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>

</html>
',
1,
0,
0,
0,
1,
CAST(N'2012-12-12T00:00:00.000'
AS
DateTime),
N'133fab36-1bae-4278-af92-2835578f5021')
INSERT
[dbo].[NotificationTemplate]
([NotificationTemplateGuid],
[Keys],
[NotificationTypeGuid],
[Subject],
[Message],
[IsActive],
[Priority],
[IsRecurring],
[RecurringInterval],
[UserInteraction],
[CreatedOn],
[CreatedBy])
VALUES
(N'29517926-0e42-4ea9-b944-93d6d74936f8',
N'RevenueRecognition.ContractCreate',
N'133fab36-1bae-4278-af92-2835578f5021',
N'[{PROJECT_NUMBER}]
:
New
Revenue
Recognition
Form
Is
Required.',
N'
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>
        Document
    </title>
</head>

<body>

    <style>
        * {
            margin: 0;
            padding: 0;
        }

        ul {
            list-style-position: inside;
        }
    </style>
    <table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
        <tr>
            <td>
                <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                    <tr>
                        <td> Dear<b style="color:#00649B">
                                {RECEIVER_NAME}
                            </b>,
                            <br>
                            <br>
                            <p style="line-height:1.6;">
                                A
                                new
                                revenue
                                recognition
                                form
                                is
                                required
                                for
                                [{PROJECT_NUMBER}:{TITLE}]
                                because
                                of
                                the
                                following
                                change.
                            </p>
                            <br>
                            <table
                                style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                                <tr>
                                    <td>

                                        <table style="border-collapse: collapse;width:100%">
                                            <tbody>
                                                <tr>
                                                    <td style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;"
                                                        colspan="2"> {REQUESTEDBY_NAME} updated [{PROJECT_NUMBER}:{TITLE}] . The total award amount for this contract now exceeds the threshold of {ThresholdAward_Amount} set for Contract type {CONTRACT_TYPE}.
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Project Number:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {PROJECT_NUMBER}
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Title:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {TITLE}
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Type:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {CONTRACT_TYPE}
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5;">
                                                        <div
                                                            style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                            <div>
                                                                <span style="color:#aaa;">Additional Message:</span>
                                                            </div>
                                                            <div>
                                                                {ADDITIONAL_MESSAGE}
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;"
                                                        colspan="2">
                                                        <span style="display:block; padding-bottom:5px;"></span><a
                                                            style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                            href="{Redirect_Url}">
                                                            Go To Contract Details
                                                        </a>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5; background:#f5f5f5; padding:20px; border-top:1px solid #eee;">
                                                              <div>  <span style="color:#aaa;">Additional people notified:</span>
                                                            </div>
                                                            <div>
                                                                {NOTIFY_OTHER}
                                                            </div>
                                                    </td>
                                                </tr>

                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>

</html>
',
1,
0,
0,
0,
1,
CAST(N'2019-06-06T00:00:00.000'
AS
DateTime),
N'133fab36-1bae-4278-af92-2835578f5021')
INSERT
[dbo].[NotificationTemplate]
([NotificationTemplateGuid],
[Keys],
[NotificationTypeGuid],
[Subject],
[Message],
[IsActive],
[Priority],
[IsRecurring],
[RecurringInterval],
[UserInteraction],
[CreatedOn],
[CreatedBy])
VALUES
(N'fa53357e-7593-430e-85c2-99fce0cb0609',
N'ProjectModification.Create',
N'133fab36-1bae-4278-af92-2835578f5021',
N'[{CONTRACT_NUMBER}]
:A
Task
Order
Modification
has
been
added.',
N'
<style>
    * {
        margin: 0;
        padding: 0;
    }

    ul {
        list-style-position: inside;
    }
</style>
<table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
    <tr>
        <td>
            <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                <tr>
                    <td>
                        Dear<b style="color:#00649B">
                            {RECEIVER_DISPLAY_NAME}
                        </b>,
                        <br>
                        <br>

                        <p style="line-height:1.6;"> {SUBMITTED_NAME} has added a Mod in the Contract Management System (CMS). You have been identified as someone who needs to know this. You can see the contract details by clicking the button below. If you have any questions, please contact {SUBMITTED_NAME} for more information.
                        </p>
                        <br>
                        <table
                            style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                            <tr>
                                <td>

                                    <table style="border-collapse: collapse;width:100%">
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Name:
                                            </td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_TITLE}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Number:
                                            </td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_NUMBER}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Project Number:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {PROJECT_NUMBER}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Modification
                                                Title:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {MODIFICATION_TITLE}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Modification
                                                Number:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {MODIFICATION_NUMBER}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract
                                                Description:
                                            </td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_DESCRIPTION}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding:0; font-size: 12px;line-height: 1.5;">
                                                <div style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                    <div>
                                                        <span style="color:#aaa;">Additional Message:</span>
                                                    </div>
                                                    <div>
                                                        {ADDITIONAL_MESSAGE}
                                                    </div>
                                                </div>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td colspan="2"
                                                style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;">
                                                <span style="display:block; padding-bottom:5px;">Details of the added task order Mod</span><a
                                                    style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                    href="{LINK}">view
                                                    detail</a>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding:0; font-size: 12px;line-height: 1.5;">
                                                <div
                                                    style="background:#f5f5f5; padding:20px; border-top:1px solid #eee;">
                                                    <div>
                                                        <span style="color:#aaa;">Additional people notified:</span>
                                                    </div>
                                                    <div>
                                                        {ADDITIONAL_RECIPIENT}
                                                    </div>
                                                </div>
                                            </td>

                                        </tr>

                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
',
1,
0,
0,
0,
1,
CAST(N'2012-12-12T00:00:00.000'
AS
DateTime),
N'133fab36-1bae-4278-af92-2835578f5021')
INSERT
[dbo].[NotificationTemplate]
([NotificationTemplateGuid],
[Keys],
[NotificationTypeGuid],
[Subject],
[Message],
[IsActive],
[Priority],
[IsRecurring],
[RecurringInterval],
[UserInteraction],
[CreatedOn],
[CreatedBy])
VALUES
(N'52ed23fe-6363-42c2-9564-9e58adc78fea',
N'RevenueRecognition.ContractCompleted',
N'133fab36-1bae-4278-af92-2835578f5021',
N'[{PROJECT_NUMBER}]
:
A
Revenue
Recognition
has
been
reviewed.',
N'
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>
        Document
    </title>
</head>

<body>
    <style>
        * {
            margin: 0;
            padding: 0;
        }

        ul {
            list-style-position: inside;
        }
    </style>
    <table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
        <tr>
            <td>
                <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                    <tr>
                        <td> Dear<b style="color:#00649B">
                                {RECEIVER_NAME}
                            </b>,
                            <br>
                            <br>
                            <p style="line-height:1.6;"> A revenue recognition remaining steps is completed.
                            </p>
                            <br>
                            <table
                                style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                                <tr>
                                    <td>

                                        <table style="border-collapse: collapse;width:100%">
                                            <tbody>
                                                <tr>
                                                    <td style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;"
                                                        colspan="2"> The reamining steps of revenue recognition has been completed.
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Project Number:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {PROJECT_NUMBER}
                                                    </td>
                                                </tr>


                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Title:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {TITLE}
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Type:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {CONTRACT_TYPE}
                                                    </td>
                                                </tr>


                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5;">
                                                        <div
                                                            style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                            <div>
                                                                <span style="color:#aaa;">Additional Message:</span>
                                                            </div>
                                                            <div>
                                                                {ADDITIONAL_MESSAGE}
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;"
                                                        colspan="2">
                                                        <span style="display:block; padding-bottom:5px;"></span><a
                                                            style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                            href="{Redirect_Url}">
                                                            Go
                                                            To
                                                            Contract
                                                            Details
                                                        </a>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5; background:#f5f5f5; padding:20px; border-top:1px solid #eee;">
                                                            <div>
                                                                <span style="color:#aaa;">Additional people notified:</span>
                                                            </div>
                                                            <div>
                                                                {NOTIFY_OTHER}
                                                            </div>
                                                    </td>
                                                </tr>

                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>

</html>
',
1,
0,
0,
0,
1,
CAST(N'2019-06-27T00:00:00.000'
AS
DateTime),
N'133fab36-1bae-4278-af92-2835578f5021')
INSERT
[dbo].[NotificationTemplate]
([NotificationTemplateGuid],
[Keys],
[NotificationTypeGuid],
[Subject],
[Message],
[IsActive],
[Priority],
[IsRecurring],
[RecurringInterval],
[UserInteraction],
[CreatedOn],
[CreatedBy])
VALUES
(N'94f63b22-946d-4ecd-be7a-dc1ee1caf431',
N'Contract.Notify',
N'133fab36-1bae-4278-af92-2835578f5021',
N'[{CONTRACT_NUMBER}]
:
The
Notification
About
Contract.
',
N'
<style>
    * {
        margin: 0;
        padding: 0;
    }

    ul {
        list-style-position: inside;
    }
</style>
<table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
    <tr>
        <td>
            <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                <tr>
                    <td>
                        Dear<b style="color:#00649B">
                            {RECEIVER_DISPLAY_NAME}
                        </b>,
                        <br>
                        <br>

                        <p style="line-height:1.6;"> {SUBMITTED_NAME} wants to notify you about a contract in the Contract Management System (CMS). You can see their note below, and can see the contract details by clicking the button below. If you have any questions, please contact {SUBMITTED_NAME} for more information.
                        </p>
                        <br>
                        <table
                            style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                            <tr>
                                <td>

                                    <table style="border-collapse: collapse;width:100%">
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Name:
                                            </td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_TITLE}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Number:
                                            </td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_NUMBER}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Project Number:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {PROJECT_NUMBER}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract
                                                Description:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_DESCRIPTION}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="2" style="padding:0; font-size: 12px;line-height: 1.5;">
                                                <div style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                    <div>
                                                        <span style="color:#aaa;">Additional Message:</span>
                                                    </div>
                                                    <div>
                                                        {ADDITIONAL_MESSAGE}
                                                    </div>
                                                </div>
                                            </td>

                                        </tr>


                                        <tr>
                                            <td colspan="2"
                                                style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;">
                                                <span style="display:block; padding-bottom:5px;"></span><a
                                                    style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                    href="{LINK}">view
                                                    details</a>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding:0; font-size: 12px;line-height: 1.5;">
                                                <div style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                    <div>
                                                        <span style="color:#aaa;">Additional people notified:</span>
                                                    </div>
                                                    <div>
                                                        {ADDITIONAL_RECIPIENT}
                                                    </div>
                                                </div>
                                            </td>

                                        </tr>

                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
',
1,
0,
0,
0,
1,
CAST(N'2012-12-12T00:00:00.000'
AS
DateTime),
N'133fab36-1bae-4278-af92-2835578f5021')
INSERT
[dbo].[NotificationTemplate]
([NotificationTemplateGuid],
[Keys],
[NotificationTypeGuid],
[Subject],
[Message],
[IsActive],
[Priority],
[IsRecurring],
[RecurringInterval],
[UserInteraction],
[CreatedOn],
[CreatedBy])
VALUES
(N'94f63b22-946d-4ecd-be7a-dc1ee1caf432',
N'Project.Create',
N'133fab36-1bae-4278-af92-2835578f5021',
N'[{CONTRACT_NUMBER}]
:A
Task
Order
has
been
added.',
N'
<style>
    * {
        margin: 0;
        padding: 0;
    }

    ul {
        list-style-position: inside;
    }
</style>
<table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
    <tr>
        <td>
            <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                <tr>
                    <td>
                        Dear<b style="color:#00649B">
                            {RECEIVER_DISPLAY_NAME}
                        </b>,
                        <br>
                        <br>

                        <p style="line-height:1.6;"> {SUBMITTED_NAME} has submitted a new Job Request Form in the Contract Management System (CMS). You have been identified as someone who needs to add additional information to complete the submission process. You can see the contract details by clicking the button below. If you have any questions, please contact {SUBMITTED_NAME} for more information.
                        </p>
                        <br>
                        <table
                            style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                            <tr>
                                <td>

                                    <table style="border-collapse: collapse;width:100%">
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Number:
                                            </td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_NUMBER}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Task
                                                Order
                                                Number:
                                            </td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {TASKORDER_NUMBER}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Project Number:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {PROJECT_NUMBER}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Awarding
                                                Agency:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {AWARDING_AGENCY}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Funding
                                                Agency:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {FUNDING_AGENCY}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="200" style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract
                                                Description:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_DESCRIPTION}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding:0; font-size: 12px;line-height: 1.5;">
                                                <div style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                    <div>
                                                        <span style="color:#aaa;">Additional Message:</span>
                                                    </div>
                                                    <div>
                                                        {ADDITIONAL_MESSAGE}
                                                    </div>
                                                </div>
                                            </td>

                                        </tr>

                                        <tr>
                                            <td colspan="2"
                                                style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;">
                                                <span style="display:block; padding-bottom:5px;"></span><a
                                                    style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                    href="{LINK}">view
                                                    details</a>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td colspan="2" style="padding:0; font-size: 12px;line-height: 1.5;">
                                                <div
                                                    style="background:#f5f5f5; padding:20px; border-top:1px solid #eee;">
                                                    <div>
                                                        <span style="color:#aaa;">Additional people notified:</span>
                                                    </div>
                                                    <div>
                                                        {ADDITIONAL_RECIPIENT}
                                                    </div>
                                                </div>
                                            </td>

                                        </tr>

                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
',
1,
0,
0,
0,
1,
CAST(N'2012-12-12T00:00:00.000'
AS
DateTime),
N'133fab36-1bae-4278-af92-2835578f5021')
INSERT
[dbo].[NotificationTemplate]
([NotificationTemplateGuid],
[Keys],
[NotificationTypeGuid],
[Subject],
[Message],
[IsActive],
[Priority],
[IsRecurring],
[RecurringInterval],
[UserInteraction],
[CreatedOn],
[CreatedBy])
VALUES
(N'46e9f891-dc18-4bb8-ade4-dc2145a9903a',
N'Contract.Edit',
N'133fab36-1bae-4278-af92-2835578f5021',
N'[{CONTRACT_NUMBER}]
:
A
Contract
has
been
updated.',
N'
<style>
    * {
        margin: 0;
        padding: 0;
    }

    ul {
        list-style-position: inside;
    }
</style>
<table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
    <tr>
        <td>
            <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                <tr>
                    <td>
                        Dear<b style="color:#00649B">
                            {RECEIVER_DISPLAY_NAME}
                        </b>,
                        <br>
                        <br>

                        <p style="line-height:1.6;">
                            {SUBMITTED_NAME} has updated a contract in the Contract Management System (CMS).
You have been identified as someone who would need to know this. You can see the contract details by clicking the button below. If you have any questions, please contact {SUBMITTED_NAME} for more information.
                        </p>
                        <br>
                        <table
                            style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                            <tr>
                                <td>

                            <tr>
                                <td>
                                    <table style="border-collapse: collapse;width:100%">
                                        <tr>
                                            <td
                                                style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Contract Number:
                                            </td>
                                            <td
                                                style="vertical-align: top; padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {CONTRACT_NUMBER}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td
                                                style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Project Number:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {PROJECT_NUMBER}
                                            </td>
                                        </tr>


                                        <tr>
                                            <td
                                                style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Awarding
                                                Agency:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {AWARDING_AGENCY}
                                            </td>
                                        </tr>

                                        <tr>
                                            <td
                                                style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Funding
                                                Agency:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {FUNDING_AGENCY}
                                            </td>
                                        </tr>


                                        <tr>
                                            <td
                                                style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                Status:
                                            </td>
                                            <td
                                                style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                {STATUS}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align:center; padding-top:30px" colspan="2">
                                                <span style="display:block; padding-bottom:5px;">Details
                                                    of
                                                    the
                                                    Updated
                                                    Contract:</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"
                                                style="font-size: 14px;line-height: 2.5; color:#fff; text-align:center; padding-bottom:30px; padding-top:5px;">
                                                <table cellpadding="0" cellspacing="0" border="0" style="width:100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td style="background:#00649B; text-align:center; border-radius:4px; text-decoration: none; letter-spacing: 1px; color:#fff; text-transform: uppercase; height:40px;"
                                                            width="200">
                                                            <a style="padding: 0; color:#fff; display:block; text-decoration: none;"
                                                                href="{LINK}">view
                                                                detail</a>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>

                                            </td>

                                        </tr>
                                        <tr>
                                            <td colspan="2"
                                                style="font-size: 12px;line-height: 1.5; background:#f5f5f5; padding:20px; border-top:1px solid #eee;">
                                                <div style="color:#aaa; font-weight: bold">
                                                    Additional people notified:
                                                </div>
                                                <div>
                                                    {ADDITIONAL_RECIPIENT}
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2"
                                                style="background:#f5f5f5; padding:20px; border-top:1px solid #eee; font-size: 12px;line-height: 1.5;">
                                                <div style="color:#aaa; font-weight: bold;">
                                                    Additional
                                                    Message:
                                                </div>
                                                <div>
                                                    {ADDITIONAL_MESSAGE}
                                                </div>
                                            </td>

                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
',
1,
0,
0,
0,
1,
CAST(N'2012-12-12T00:00:00.000'
AS
DateTime),
N'133fab36-1bae-4278-af92-2835578f5021')
INSERT
[dbo].[NotificationTemplate]
([NotificationTemplateGuid],
[Keys],
[NotificationTypeGuid],
[Subject],
[Message],
[IsActive],
[Priority],
[IsRecurring],
[RecurringInterval],
[UserInteraction],
[CreatedOn],
[CreatedBy])
VALUES
(N'cb0c4a0f-1df3-4624-94bd-e4682c214cc9',
N'RevenueRecognition.TaskOrderModCreate',
N'133fab36-1bae-4278-af92-2835578f5021',
N'[{PROJECT_NUMBER}]
:
New
Revenue
Recognition
Form
Is
Required.',
N'
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>
        Document
    </title>
</head>

<body>
    <style>
        * {
            margin: 0;
            padding: 0;
        }

        ul {
            list-style-position: inside;
        }
    </style>
    <table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
        <tr>
            <td>
                <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                    <tr>
                        <td> Dear<b style="color:#00649B">
                                {RECEIVER_NAME}
                            </b>,
                            <br>
                            <br>
                            <p style="line-height:1.6;"> A new revenue recognition form is required for [{PROJECT_NUMBER}:{TITLE}] because of the following change.
                            </p>
                            <br>
                            <table
                                style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                                <tr>
                                    <td>

                                        <table style="border-collapse: collapse;width:100%">
                                            <tbody>
                                                <tr>
                                                    <td style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;"
                                                        colspan="2"> {REQUESTEDBY_NAME} updated [{PROJECT_NUMBER}:{TITLE}]. The total award amount for this task order now exceeds the threshold of {THRESHOLD_AMOUNT} set for Contract type {CONTRACT_TYPE}.
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Project Number:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {PROJECT_NUMBER}
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Task
                                                        Order
                                                        Title:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {TITLE}
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Type:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {CONTRACT_TYPE}
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Task
                                                        Order
                                                        Modification
                                                        Number:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {MOD_NUMBER}
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5;">
                                                        <div
                                                            style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                            <div>
                                                                <span style="color:#aaa;">Additional Message:</span>
                                                            </div>
                                                            <div>
                                                                {ADDITIONAL_MESSAGE}
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;"
                                                        colspan="2">
                                                        <span style="display:block; padding-bottom:5px;"></span><a
                                                            style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                            href="{Redirect_Url}">
                                                            Go
                                                            To
                                                            Contract
                                                            Details
                                                        </a>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5; background:#f5f5f5; padding:20px; border-top:1px solid #eee;">
                                                            <div>
                                                                <span style="color:#aaa;">Additional people notified:</span>
                                                            </div>
                                                            <div>
                                                                {NOTIFY_OTHER}
                                                            </div>
                                                    </td>
                                                </tr>

                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>

</html>
',
1,
0,
0,
0,
1,
CAST(N'2012-12-12T00:00:00.000'
AS
DateTime),
N'133fab36-1bae-4278-af92-2835578f5021')
INSERT
[dbo].[NotificationTemplate]
([NotificationTemplateGuid],
[Keys],
[NotificationTypeGuid],
[Subject],
[Message],
[IsActive],
[Priority],
[IsRecurring],
[RecurringInterval],
[UserInteraction],
[CreatedOn],
[CreatedBy])
VALUES
(N'5dfab408-6eaa-494c-8dc2-f06fbeea477d',
N'RevenueRecognition.TaskOrderCompleted',
N'133fab36-1bae-4278-af92-2835578f5021',
N'[{PROJECT_NUMBER}]
:
A
Revenue
Recognition
has
been
reviewed.',
N'
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>
        Document
    </title>
</head>

<body>
    <div>
        <style>
            * {
                margin: 0;
                padding: 0;
            }

            ul {
                list-style-position: inside;
            }
        </style>
        <table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
            <tr>
                <td>
                    <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                        <tr>
                            <td> Dear<b style="color:#00649B">
                                    {RECEIVER_NAME}
                                </b>,
                                <br>
                                <br>
                                <p style="line-height:1.6;"> A revenue recognition remaining steps is completed.
                                </p>
                                <br>
                                <table
                                    style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                                    <tr>
                                        <td>

                                            <table style="border-collapse: collapse;width:100%">
                                                <tbody>
                                                    <tr>
                                                        <td style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;"
                                                            colspan="2"> The reamining steps of revenue recognition has been completed.
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td width="200"
                                                            style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                            Project
                                                            Number:
                                                        </td>
                                                        <td
                                                            style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                            {PROJECT_NUMBER}
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td width="200"
                                                            style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                            Task
                                                            Order
                                                            Number:
                                                        </td>
                                                        <td
                                                            style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                            {TASKORDER_NUMBER}
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td width="200"
                                                            style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                            Task
                                                            Order
                                                            Title:
                                                        </td>
                                                        <td
                                                            style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                            {TASKORDER_TITLE}
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td width="200"
                                                            style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                            Contract
                                                            Type:
                                                        </td>
                                                        <td
                                                            style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                            {CONTRACT_TYPE}
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td colspan="2"
                                                            style="padding:0; font-size: 12px;line-height: 1.5;">
                                                            <div
                                                                style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                                <div>
                                                                    <span style="color:#aaa;">Additional
                                                                        Message:</span>
                                                                </div>
                                                                <div>
                                                                    {ADDITIONAL_MESSAGE}
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;"
                                                            colspan="2">
                                                            <span style="display:block; padding-bottom:5px;"></span><a
                                                                style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                                href="{Redirect_Url}">
                                                                Go To Contract Details
                                                            </a>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"
                                                            style="padding:0; font-size: 12px;line-height: 1.5;">
                                                            <div
                                                                style="background:#f5f5f5; padding:20px; border-top:1px solid #eee;">

                                                                <div>
                                                                    <span style="color:#aaa;">Additional
                                                                        people
                                                                        notified:</span>
                                                                </div>
                                                                <div>
                                                                    {NOTIFY_OTHER}
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>

                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>

                                </tbody>
                            </table> </td>
                        </tr>

                    </tbody>
                </table> </td>
            </tr>

        </tbody>
    </table>
    </div>
</body>

</html>
',
1,
0,
0,
0,
1,
CAST(N'2012-12-12T00:00:00.000'
AS
DateTime),
N'133fab36-1bae-4278-af92-2835578f5021')
INSERT
[dbo].[NotificationTemplate]
([NotificationTemplateGuid],
[Keys],
[NotificationTypeGuid],
[Subject],
[Message],
[IsActive],
[Priority],
[IsRecurring],
[RecurringInterval],
[UserInteraction],
[CreatedOn],
[CreatedBy])
VALUES
(N'4f9ecc95-a7fd-4d2f-92d3-f7a23261b53b',
N'RevenueRecognition.ContractModCreate',
N'133fab36-1bae-4278-af92-2835578f5021',
N'[{PROJECT_NUMBER}]
:
New
Revenue
Recognition
Form
Is
Required.',
N'
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>
        Document
    </title>
</head>

<body>
    <style>
        * {
            margin: 0;
            padding: 0;
        }

        ul {
            list-style-position: inside;
        }
    </style>
    <table style="background: #eee; width:100%; color:#777; padding: 50px 15px; font-size: 14px;">
        <tr>
            <td>
                <table style="max-width:600px; width:100%; margin:30px auto;" align="center">
                    <tr>
                        <td> Dear<b style="color:#00649B">
                                {RECEIVER_NAME}
                            </b>,
                            <br>
                            <br>
                            <p style="line-height:1.6;"> A new revenue recognition form is required for [{PROJECT_NUMBER}:{TITLE}] because of the following change.
                            </p>
                            <br>
                            <table
                                style="width: 100%;margin: 0 auto; overflow:hidden; padding-top:10px;border: 1px solid #eee; background: #fff;line-height: 1.5;border-radius: 5px;">
                                <tr>
                                    <td>

                                        <table style="border-collapse: collapse;width:100%">
                                            <tbody>
                                                <tr>
                                                    <td style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;"
                                                        colspan="2"> {REQUESTEDBY_NAME} updated [{PROJECT_NUMBER}:{TITLE}] , the total award amount for this contract now exceeds the threshold of {ThresholdAward_Amount} set for Contract type {CONTRACT_TYPE}.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Project Number:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {PROJECT_NUMBER}
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Title:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {CONTRACT_TITLE}
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Type:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {CONTRACT_TYPE}
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td width="200"
                                                        style="vertical-align: top; padding:8px 8px 8px 20px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        Contract Modification Number:
                                                    </td>
                                                    <td
                                                        style="vertical-align: top;padding:8px 20px 8px 8px; font-size: 14px;line-height: 1.5; color:#444;">
                                                        {MOD_NUMBER}
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5;">
                                                        <div
                                                            style="margin-top:20px; padding:20px; border-top:1px solid #eee;">

                                                            <div>
                                                                <span style="color:#aaa;">Additional Message:</span>
                                                            </div>
                                                            <div>
                                                                {ADDITIONAL_MESSAGE}
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="padding:8px 20px; font-size: 14px;line-height: 2.5; text-align:center; padding-bottom:30px; padding-top:30px;"
                                                        colspan="2">
                                                        <span style="display:block; padding-bottom:5px;"></span><a
                                                            style="padding:10px 50px; background:#00649B;border-radius:4px; letter-spacing: 1px; color:#fff; text-transform: uppercase; text-decoration: none;"
                                                            href="{Redirect_Url}"> Go To Contract Details
                                                        </a>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2"
                                                        style="padding:0; font-size: 12px;line-height: 1.5; background:#f5f5f5; padding:20px; border-top:1px solid #eee;">
                                                            <div>
                                                                <span style="color:#aaa;">Additional people notified:</span>
                                                            </div>
                                                            <div>
                                                                {NOTIFY_OTHER}
                                                            </div>
                                                    </td>
                                                </tr>

                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>

</html>
',
1,
0,
0,
0,
1,
CAST(N'2012-12-12T00:00:00.000'
AS
DateTime),
N'133fab36-1bae-4278-af92-2835578f5021')