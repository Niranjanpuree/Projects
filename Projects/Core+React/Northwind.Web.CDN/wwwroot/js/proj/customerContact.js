$(document).ready(function () {
    InitialLoad();
    function InitialLoad() {
        LoadGrid();
    }
    ///.............................................///
    var ReloadGrid = function () {
        $('#CustomerContactGrid').data('kendoGrid').dataSource.read();
        $('#CustomerContactGrid').data('kendoGrid').refresh();
    }
    // Section Table List/Grid //
    function LoadGrid() {
        var searchVal = $(".searchCustomerContact").val() || '';
        var dataUrl = "/Admin/CustomerContact/Get?searchValue=" + searchVal + '&CustomerGuid=' + $('#CustomerGuid').val() + '&sortField=FirstName&sortDirection=asc';
        $("#CustomerContactGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        contentType: "application/json",
                        url: dataUrl,
                        dataType: "json"
                    }
                },
                batch: true,
                schema: {
                    data: "data",
                    total: "total",
                    model: {
                        id: "contactGuid",
                        fields: {
                            contactTypeName: { type: "string" },
                            fullName: { type: "string" },
                            ContactNumber: { type: "string" },
                            EmailAddress: { type: "string" },
                            IsActiveStatus: { type: "string" },
                        }
                    }
                },
                serverPaging: true,
                pageSize: 20

            },
            height: 490,
            scrollable: true,
            sortable: true,
            reorderable: true,
            sort: onSorting,
            resizable: true,
            columnMenu: false,
            filterable: false,
            pageable: {
                input: true,
                numeric: true
            },
            columns: [
                { selectable: true, width: "50px", locked: true, lockable: true },
                { name: 'open', width: 30, click: open, template: "<div class='gridToolBar'></div>" },
                { field: "contactTypeName", title: "Contact Type", width: 300 },
                { field: "fullName", title: "Full Name", width: 200 },
                { field: "contactNumbers", title: "Contact Number", width: 225 },
                { field: "emailAddress", title: "Email Address", width: 225 },
                { field: "isActiveStatus", title: "Status", width: 100, locked: false, lockable: false }
            ],
            dataBound: function () { new GridExtention().loadGridMenu($("#CustomerContactGrid")) },
            editable: "",
            progress: false,
            events: [
                {
                    title: "Details",
                    event: "onView",
                    icon: 'info',
                    onView: function (e) {
                        var customerguid = $('#CustomerGuid').val();
                        var CustomerName = $('#CustomerName').val();
                        var data = {}
                        data.url = "/admin/CustomerContact/Detail/" + e.item.options.data.contactGuid;
                        data.submitURL = "/admin/CustomerContact/Detail";
                        var options = {
                            title: 'Detail info of : ' + e.item.options.data.fullName,
                            events: [
                                {
                                    text: "Back to list",
                                    id: customerguid,
                                    action: function (e) {
                                        values = {
                                            customerContact: { customerguid: customerguid, searchvalue: " ", customerName: CustomerName }
                                        }
                                        reloadGridModal(values)
                                    }
                                }
                            ]
                        };
                        var dialog = $("#dialog").data("kendoDialog");
                        dialog.close();
                        Dialog.openDialogGridSubmit(data, options);
                    }
                },
                {
                    title: "Edit",
                    event: "onView",
                    icon: 'pencil',
                    onView: function (e) {
                        var customerguid = $('#CustomerGuid').val();
                        var CustomerName = $('#CustomerName').val();
                        var data = {}
                        data.url = "/admin/CustomerContact/Edit/" + e.item.options.data.contactGuid;
                        data.submitURL = "/admin/CustomerContact/Edit";
                        var options = {
                            title: 'Edit the info of ' + e.item.options.data.fullName,
                            events: [
                                {
                                    text: "Save",
                                    primary: true,
                                    action: function (e, values) {
                                        reloadGridModal(values)
                                    }
                                },
                                {
                                    text: "Back to list",
                                    id: customerguid,
                                    action: function (e) {
                                        values = {
                                            customerContact: { customerguid: customerguid, searchvalue: "", customerName: CustomerName }
                                        }
                                        reloadGridModal(values)
                                    }
                                }
                            ]
                        };
                        var dialog = $("#dialog").data("kendoDialog");
                        dialog.close();
                        Dialog.openDialogGridSubmit(data, options);
                    }
                },
                {
                    title: "Delete",
                    event: "onDelete",
                    icon: 'delete',
                    onDelete: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.contactGuid;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  delete contact " + data.data.fullName + " ?",
                            title: "Confirm",
                            ok: function (e) {
                                var token = getCookieValue('X-CSRF-TOKEN');
                                var reqValToken = getCookieValue('RequestVerificationToken');
                                $.ajax({
                                    headers: {
                                        'X-CSRF-TOKEN': token,
                                        'RequestVerificationToken': reqValToken,
                                        'Accept': 'application/json',
                                        'Content-Type': 'application/json'
                                    },
                                    dataType: 'json',
                                    url: '/Admin/CustomerContact/Delete',
                                    type: "POST",
                                    data: JSON.stringify(ids),
                                    success: function (values) {
                                        $.notify(values.message, values.status);
                                        ReloadGrid();
                                    },
                                    error: function (e, values) {
                                        Dialog.alert(values.message);
                                    }
                                });
                            },
                            cancel: function (e) {
                            }
                        });

                    }
                },
                {
                    title: "Disable",
                    event: "onDisable",
                    icon: 'cancel',
                    onDisable: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        if (!data.data.isActive) {
                            Dialog.alert("The status has already been set inactive!!");
                            return false;
                        }
                        var id = e.item.options.data.contactGuid;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  disable contact " + data.data.fullName + " ?",
                            title: "Confirm",
                            ok: function (e) {
                                var token = getCookieValue('X-CSRF-TOKEN');
                                var reqValToken = getCookieValue('RequestVerificationToken');

                                $.ajax({
                                    headers: {
                                        'X-CSRF-TOKEN': token,
                                        'RequestVerificationToken': reqValToken,
                                        'Accept': 'application/json',
                                        'Content-Type': 'application/json'
                                    },
                                    dataType: 'json',
                                    url: '/Admin/CustomerContact/Disable',
                                    type: "POST",
                                    data: JSON.stringify(ids),
                                    success: function (values) {
                                        $.notify(values.message, values.status);
                                        ReloadGrid();
                                    },
                                    error: function (e, values) {
                                        Dialog.alert(values.message);
                                    }
                                });
                            },
                            cancel: function (e) {
                            }
                        });

                    }
                },
                {
                    title: "Enable",
                    event: "onEnable",
                    icon: 'check',
                    onEnable: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        if (data.data.isActive) {
                            Dialog.alert("The status has already been set active!!");
                            return false;
                        }
                        var id = e.item.options.data.contactGuid;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  enable contact " + data.data.fullName + " ?",
                            title: "Confirm",
                            ok: function (e) {
                                var token = getCookieValue('X-CSRF-TOKEN');
                                var reqValToken = getCookieValue('RequestVerificationToken');
                                $.ajax({
                                    headers: {
                                        'X-CSRF-TOKEN': token,
                                        'RequestVerificationToken': reqValToken,
                                        'Accept': 'application/json',
                                        'Content-Type': 'application/json'
                                    },
                                    dataType: 'json',
                                    url: '/Admin/CustomerContact/Enable',
                                    type: "POST",
                                    data: JSON.stringify(ids),
                                    success: function (values) {
                                        $.notify(values.message, values.status);
                                        ReloadGrid();
                                    },
                                    error: function (e, values) {
                                        Dialog.alert(values.message);
                                    }
                                });
                            },
                            cancel: function (e) {
                            }
                        });

                    }
                }
            ]

        }).data("kendoGrid");
    }
    ///..........................................///

    $("#addNewCustomerContact").on('click',
        function (evt) {
            var customerguid = $('#CustomerGuid').val();
            var CustomerName = $('#CustomerName').val();
            var data = {}
            data.url = "/Admin/CustomerContact/Add?customerGuid=" + customerguid;
            data.submitURL = "/Admin/CustomerContact/Add/";
            var options = {
                title: 'Add New Contact ' + CustomerName,
                events: [
                    {
                        text: "Save",
                        primary: true,
                        action: function (e, values) {
                            reloadGridModal(values)
                        }
                    },
                    {
                        text: "Back to list",
                        id: customerguid,
                        action: function (e) {
                            values = {
                                customerContact: { customerguid: customerguid, searchvalue: "", customerName: CustomerName }
                            }
                            reloadGridModal(values)
                        }
                    }
                ]
            };
            var dialog = $("#dialog").data("kendoDialog");
            dialog.close();
            Dialog.openDialogGridSubmit(data, options);
        });
    function reloadGridModal(values) {
        var searchVal = values.customerContact.searchvalue;
        if (searchVal == 'undefined' || searchVal == null) {
            searchVal = "";
        }
        var url = '/admin/customercontact/Index?searchValue=' + searchVal + '&CustomerGuid=' + values.customerContact.customerguid;
        var data = {
            url: url
        };
        var options = {
            title: 'Manage contact for ' + values.customerContact.customerName,
            events: [
                {
                    text: "Cancel",
                    action: function (e) {
                    }
                }
            ]
        };
        var dialog = $("#dialog").data("kendoDialog");
        dialog.close();
        Dialog.openDialogGridSubmit(data, options);
    }
    // Section texbox Search //
    $('#btnContactSearch').click(function () {
        var customerguid = $('#CustomerGuid').val();
        var customerName = $('#CustomerName').val();
        var searchvalue = $('.searchCustomerContact').val();
        values = {
            customerContact: { customerguid: customerguid, searchvalue: searchvalue, customerName: customerName }
        }
        reloadGridModal(values)
    });

    $('#clearContactSearch').click(function () {
        $(".searchCustomerContact").val('');
        var customerguid = $('#CustomerGuid').val();
        var customerName = $('#CustomerName').val();
        var searchvalue = $('.searchCustomerContact').val();
        values = {
            customerContact: { customerguid: customerguid, searchvalue: searchvalue, customerName: customerName }
        }
        reloadGridModal(values)
    });

    /// set column menu in only last column header.. ///
    totalHeader = $('.k-header-column-menu').length;
    var grid = $("#CustomerContactGrid").data("kendoGrid");

    $('.k-header-column-menu').each(function () {
        if ($(this).parent().attr("data-index") != parseInt(totalHeader + 1)) {
            var dataIndex = $(this).parent().attr("data-index");
            grid.thead.find("[data-index = " + dataIndex + "]>.k-header-column-menu").remove();
        }
    });

    $('#ExportToPdf').click(function () {
        var ids = [];
        var entityGrid = $("#CustomerContactGrid").data("kendoGrid");
        var rows = entityGrid.select();
        rows.each(function (index, row) {
            var selectedItem = entityGrid.dataItem(row);
            ids.push(selectedItem.contactGuid);
        });
    });

    $('#DisableCustomerContact').click(function () {
        var ids = [];
        var entityGrid = $("#CustomerContactGrid").data("kendoGrid");
        var rows = entityGrid.select();
        rows.each(function (index, row) {
            var selectedItem = entityGrid.dataItem(row);
            ids.push(selectedItem.contactGuid);
        });
        if (ids <= 0) {
            Dialog.alert("Please select item/s first before disabling.");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to disable " + ids.length / 2 + " contact(s)?",
            title: "Confirm",
            ok: function (e) {
                var token = getCookieValue('X-CSRF-TOKEN');
                var reqValToken = getCookieValue('RequestVerificationToken');
                $.ajax({
                    headers: {
                        'X-CSRF-TOKEN': token,
                        'RequestVerificationToken': reqValToken,
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    dataType: 'json',
                    url: '/Admin/CustomerContact/Disable',
                    type: "POST",
                    data: JSON.stringify(ids),
                    success: function (values) {
                        $.notify(values.message, values.status);
                        ReloadGrid();
                    },
                    error: function (values) {
                        Dialog.alert(values.message);
                    }
                });
            },
            cancel: function (e) {
            }
        });
    });

    $('#EnableCustomerContact').click(function () {
        var ids = [];
        var entityGrid = $("#CustomerContactGrid").data("kendoGrid");
        var rows = entityGrid.select();
        rows.each(function (index, row) {
            var selectedItem = entityGrid.dataItem(row);
            ids.push(selectedItem.contactGuid);
        });
        if (ids <= 0) {
            Dialog.alert("Please select item/s first before enabling.");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to enable " + ids.length / 2 + " contact(s)?",
            title: "Confirm",
            ok: function (e) {
                var token = getCookieValue('X-CSRF-TOKEN');
                var reqValToken = getCookieValue('RequestVerificationToken');
                $.ajax({
                    headers: {
                        'X-CSRF-TOKEN': token,
                        'RequestVerificationToken': reqValToken,
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    dataType: 'json',
                    url: '/Admin/CustomerContact/Enable',
                    type: "POST",
                    data: JSON.stringify(ids),
                    success: function (values) {
                        $.notify(values.message, values.status);
                        ReloadGrid();
                    },
                    error: function (values) {
                        Dialog.alert(values.message);
                    }
                });
            },
            cancel: function (e) {
            }
        });
    });

    $('#DeleteCustomerContact').click(function () {
        var ids = [];
        var entityGrid = $("#CustomerContactGrid").data("kendoGrid");
        var rows = entityGrid.select();
        rows.each(function (index, row) {
            var selectedItem = entityGrid.dataItem(row);
            ids.push(selectedItem.contactGuid);
        });
        if (ids <= 0) {
            Dialog.alert("Please select item/s first before deleting.");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to delete " + ids.length / 2 + " contact(s)?",
            title: "Confirm",
            ok: function (e) {
                var token = getCookieValue('X-CSRF-TOKEN');
                var reqValToken = getCookieValue('RequestVerificationToken');
                $.ajax({
                    headers: {
                        'X-CSRF-TOKEN': token,
                        'RequestVerificationToken': reqValToken,
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    dataType: 'json',
                    url: '/Admin/CustomerContact/Delete',
                    type: "POST",
                    data: JSON.stringify(ids),
                    success: function (values) {
                        $.notify(values.message, values.status);
                        ReloadGrid();
                    },
                    error: function (values) {
                        Dialog.alert(values.message);
                    }
                });
            },
            cancel: function (e) {
            }
        });
    });

    function onSorting(arg) {
        var searchValue = $(".searchCustomerContact").val() || '';
        var dataUrl = "/Admin/CustomerContact/Get?SearchValue=" + searchValue + '&customerGuid=' + $("#CustomerGuid").val() + '&sortField=' + arg.sort.field + '&sortDirection=' + (arg.sort.dir == null ? "asc" : arg.sort.dir);
        var grid = $("#CustomerContactGrid").data("kendoGrid");
        grid.dataSource.options.transport.read.url = dataUrl;
        grid.dataSource.read();
        grid.refresh();
    }

});// end of document..