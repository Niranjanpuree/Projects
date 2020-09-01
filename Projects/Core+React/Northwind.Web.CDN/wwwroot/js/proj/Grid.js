$(document).ready(function () {
    $("#grid").kendoGrid({
        dataSource: {
            type: "odata",
            transport: {
                read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Customers"
            },
            pageSize: 20,
            schema: {
                model: {
                    id: "uid"
                }
            }
        },
        height: 550,
        groupable: true,
        sortable: true,
        pageable: {
            refresh: true,
            pageSizes: true,
            buttonCount: 5
        },
        columns: [{selectable: true},{
            template: "<div class='customer-photo'" +
                "style='background-image: url(https://demos.telerik.com/kendo-ui/content/web/Customers/#:data.CustomerID#.jpg);'></div>" +
                "<div class='customer-name'>#: ContactName #</div>",
            field: "ContactName",
            title: "Contact Name",
            width: 240
        }, {
            field: "ContactTitle",
            title: "Contact Title"
        }, {
            field: "CompanyName",
            title: "Company Name"
        }, {
            field: "Country",
            width: 150
        }, { name: 'open', click: open, template: "<div class='gridToolBar'></div>" }
        ],
        dataBound: function () { new GridExtention().loadGridMenu($("#grid")) },
        events: [
            {
                title: "View",
                event: "onView",
                onView: function (e) {
                    var data = {}
                    data.data = e.item.options.data;
                    data.url = "Grid/Add";
                    var options = {
                        title: 'Delete Confirmation',
                        events: [
                            {
                                text: "Save",
                                primary: true,
                                action: function (e, values) {
                                    Dialog.alert("OK")
                                }
                            },
                            {
                                text: "Ok",
                                primary: true,
                                action: function (e, values) {
                                    Dialog.alert("OK")
                                }
                            },
                            {
                                text: "Cancel",
                                action: function (e) {
                                    Dialog.alert("Cancel")
                                }
                            }
                        ]
                    };
                    Dialog.openDialog(data, options);
                }
            },
            {
                title: "Edit",
                event: "onEdit",
                onEdit: function (e) {
                    var data = {}
                    data.data = e.item.options.data;
                    data.url = "Grid/Edit";
                    data.submitURL = "Grid/Add";
                    var options = {
                        title: 'Delete Confirmation',
                        events: [
                            {
                                text: "Ok",
                                primary: true,
                                action: function (e, values) {
                                    Dialog.alert("OK")
                                }
                            },
                            {
                                text: "Cancel",
                                action: function (e) {
                                    Dialog.alert("Cancel")
                                }
                            }
                        ]
                    };
                    //Dialog.openDialogSubmit(data, options);
                }
            },
            {
                title: "Delete",
                event: "onDelete",
                onDelete: function (e) {
                    Dialog.confirm({
                        text: "Confirm it?",
                        title: "Confirm",
                        ok: function (e) {
                            Dialog.alert("success")
                        },
                        cancel: function (e) {
                            Dialog.alert("cancel")
                        }
                    })
                }
            }
        ]

    });
});
