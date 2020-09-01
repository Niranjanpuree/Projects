$(document).ready(function () {

    var gridId = $("#CustomerGrid");

    var advancedSearchLink = $("#advancedSearchLink");
    var dialog = $("#advancedSearchDialog");

    advancedSearchLink.on('click', function (evt) {
        dialog.data('kendoDialog').content = $('#advancedSearchDialogContent');
        dialog.data('kendoDialog').open();
        evt.preventDefault;
    });

    dialog.kendoDialog({
        width: "50%",
        title: "Advanced Search",
        visible: false,
        closable: true,
        modal: true,
        content: "",
        actions: [
            { text: 'Apply', primary: true, action: onEnable },
            { text: 'Cancel' }
        ]
    });
    function onEnable(e) {

        var ob = {
            SearchVms: [
                { "AttributeName": 'CustomerName', "Operator": 'StringEquals', "FirstValue": 'sds' }
            ]
        };

        $.ajax({
            url: '/Admin/Customer/AdvanceSearch',
            type: "POST",
            data: { lstSearch: ob },
            success: function () {
                location.reload();
            },
            error: function (xhr, status, error) { alert('Error:'); }
        });
    }

    var ajaxGet = function (url) {
        return $.ajax({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            dataType: 'json',
            url: url,
            type: "GET"
        });
    }

    InitialLoad();
    function InitialLoad() {
        ajaxGet("/GridFields/Customer").then(function (data) {
            var arrData = data.sort(function (a, b) {
                return a.orderIndex - b.orderIndex
            });;
            var columns = [];
            columns.push({ selectable: true, width: "50px", locked: true, lockable: true });
            columns.push({ name: 'open', width: 30, click: open, template: "<div class='gridToolBar'></div>" });
            for (i in arrData) {
                if (i == arrData.length - 1) {
                    columns.push({
                        field: arrData[i].fieldName,
                        title: arrData[i].fieldLabel,
                        sortable: arrData[i].isSortable,
                        filterable: arrData[i].isFilterable,
                        locked: false,
                        lockable: false
                    });
                }
                else {
                    columns.push({
                        field: arrData[i].fieldName,
                        title: arrData[i].fieldLabel,
                        sortable: arrData[i].isSortable,
                        filterable: arrData[i].isFilterable
                    });
                }

            }
            LoadGrid(columns);
        })

    }
    ///.............................................///

    var ReloadGrid = function () {
        $(gridId).data('kendoGrid').dataSource.read();
        $(gridId).data('kendoGrid').refresh();
    }

    //Generic ajax post for commands..
    var ajaxPost = function (url, data) {

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
            url: url,
            type: "POST",
            data: JSON.stringify(data),
            success: function (values) {
                $.notify(values.message, values.status);
                ReloadGrid();
            },
            error: function (values) {
                $.notify(values.message, values.status);
            }
        });
    }

    var selectedIds = function () {
        var ids = [];
        var entityGrid = $(gridId).data("kendoGrid");
        var rows = entityGrid.select();
        rows.each(function (index, row) {
            var selectedItem = entityGrid.dataItem(row);
            ids.push(selectedItem.customerGuid);
        });
        return ids;
    };

    // Section Table List/Grid //
    function LoadGrid(columns) {
        var dataUrl = "/Admin/customer/Get?searchValue=" + $("#SearchValue").val() + '&sortField=CustomerCode&sortDirection=asc';
        $(gridId).kendoGrid({
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
                        id: "customerGuid",
                        fields: columns.fieldLabel
                    }
                },
                serverPaging: true,
                pageSize: 20

            },
            columnMenu: {
                sortable: false
            },
            height: 650,
            scrollable: true,
            sortable: true,
            reorderable: true,
            sort: onSorting,
            resizable: true,
            filterable: false,
            pageable: {
                input: true,
                numeric: true
            },
            columns: columns,
            dataBound: function () {
                var totalheader = $(".k-header-column-menu").length;
                $(".k-header-column-menu").each(function (i) {
                    $(this).remove();
                    //if (i < parseInt(totalheader - 1)) {
                    //    $(this).remove();
                    //}
                });
                new GridExtention().loadGridMenu($(gridId));
            },
            editable: "",
            progress: false,
            events: [
                {
                    title: "Details",
                    event: "onView",
                    icon: 'info',
                    onView: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        data.url = "/Admin/customer/Detail/" + e.item.options.data.customerGuid;
                        var options = {
                            title: 'Details of ' + e.item.options.data.displayName,
                            events: [
                                {
                                    text: "Cancel",
                                    action: function (e) {
                                    }
                                }
                            ]
                        };
                        Dialog.openDialogSubmit(data, options);
                    }
                },
                {
                    title: "Edit",
                    event: "onView",
                    icon: 'pencil',
                    onView: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.customerGuid;
                        data.url = "/Admin/customer/Edit/" + e.item.options.data.customerGuid;
                        data.submitURL = "/Admin/customer/Edit";
                        var options = {
                            title: 'Edit Customer ' + e.item.options.data.displayName,
                            events: [
                                {
                                    text: "Save",
                                    primary: true,
                                    action: function (e, values) {
                                        $.notify(values.message, values.status);
                                        ReloadGrid();
                                    }
                                },
                                {
                                    text: "Cancel",
                                    action: function (e) {
                                    }
                                }
                            ]
                        };
                        Dialog.openDialogSubmit(data, options);
                    }
                },
                {
                    title: "Delete",
                    event: "onDelete",
                    icon: 'delete',
                    onDelete: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.customerGuid;
                        var customerName = e.item.options.data.customerName;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  delete customer " + customerName + " ?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/Admin/Customer/Delete', ids);
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
                        var id = e.item.options.data.customerGuid;
                        var customerName = e.item.options.data.customerName;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  disable customer " + customerName + " ?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/Admin/Customer/Disable', ids);
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
                        var id = e.item.options.data.customerGuid;
                        var customerName = e.item.options.data.customerName;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  enable customer " + customerName + " ?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/Admin/Customer/Enable', ids);
                            },
                            cancel: function (e) {
                            }
                        });

                    }
                },
                {
                    title: "Manage Contact",
                    event: "onManageContact",
                    icon: 'user',
                    onManageContact: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.customerGuid;
                        data.url = "/admin/CustomerContact/index?SearchValue=" + "" + '&customerGuid=' + e.item.options.data.customerGuid;
                        data.submitURL = "/Admin/customer/Edit";
                        var options = {
                            title: 'Manage contact for customer ' + e.item.options.data.displayName,
                            events: [
                                {
                                    text: "Cancel",
                                    action: function (e) {
                                    }
                                }
                            ]
                        };
                        Dialog.openDialogGridSubmit(data, options);
                    }
                }
            ]

        }).data("kendoGrid");
    }
    ///..........................................///

    $("#addNewCustomer").on('click',
        function (evt) {
            var data = {}
            data.url = "/Admin/customer/Add/";
            data.submitURL = "/Admin/customer/Add/";
            var options = {
                title: 'Add New Customer',
                events: [
                    {
                        text: "Save & Add New Contact",
                        primary: true,
                        action: function (e, values) {
                            addContacts(values)
                            $.notify(values.message, values.status);
                            ReloadGrid();
                        }
                    },
                    {
                        text: "Save",
                        primary: true,
                        action: function (e, values) {
                            $.notify(values.message, values.status);
                            ReloadGrid();
                        }
                    },
                    {
                        text: "Cancel",
                        secondary: true,
                        action: function (e) {
                        }
                    }
                ]
            };
            Dialog.openDialogSubmit(data, options);
            setTimeout(function () {

            }, 1000)
        });

    // add new contacts
    function addContacts(values) {
        var data = {
            url: '/admin/customercontact/Add?customerGuid=' + values.customer.customerGuid,
            submitURL: '/admin/customercontact/Add/' + values.customer.customerGuid
        };
        var options = {
            title: 'Add New Contacts For ' + values.customer.customerName,
            events: [
                {
                    text: "Save & Add New Contact",
                    primary: true,
                    action: function (e, v) {
                        $.notify(v.message, v.status);
                        addContacts(values);

                    }
                },
                {
                    text: "Save",
                    primary: true,
                    action: function (e, v) {
                        $.notify(v.message, v.status);
                    }
                },
                {
                    text: "Cancel",
                    action: function (e) {
                    }
                }
            ]
        };
        Dialog.openDialogSubmit(data, options);
    }
    // Section texbox Search //
    $('#btnSearch').click(function (e) {
        var searchvalue = $("#SearchValue").val();
        window.location.href = "/Admin/customer?SearchValue=" + searchvalue;
    });

    $('#clearSearch').click(function () {
        $("#SearchValue").val('');
        window.location.href = "/Admin/customer/Index";
    });

    $(document).on("click", "#DisableCustomer", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("Please select item/s first before disabling.");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  disable " + ids.length / 2 + " customer(s)?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/Admin/Customer/Disable', ids);
            },
            cancel: function (e) {
            }
        });
    });

    $(document).on("click", "#EnableCustomer", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("Please select item/s first before enabling.");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure to  enable " + ids.length / 2 + " customer(s)?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/Admin/Customer/Enable', ids);
            },
            cancel: function (e) {
            }
        });
    });

    $(document).on("click", "#DeleteCustomer", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("Please select item/s first before deleting.");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  delete " + ids.length / 2 + " customer(s)?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/Admin/Customer/Delete', ids);
            },
            cancel: function (e) {
            }
        });
    });

    function onSorting(arg) {
        var dataUrl = "/Admin/customer/Get?SearchValue=" + $("#SearchValue").val() + '&sortField=' + arg.sort.field + '&sortDirection=' + (arg.sort.dir == null ? "asc" : arg.sort.dir);
        var grid = $(gridId).data("kendoGrid");
        grid.dataSource.options.transport.read.url = dataUrl;
        grid.dataSource.read();
        grid.refresh();
    }

});// end of document..