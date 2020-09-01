$(document).ready(function () {

    var gridId = $("#OfficeGrid");

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
        ajaxGet("/GridFields/office").then(function (data) {
            var arrData = data.sort(function (a, b) {
                return a.orderIndex - b.orderIndex
            });;
            var columns = [];
            columns.push({ selectable: true, width: "50px", locked: true, lockable: true });
            columns.push({ name: 'open', width: 30, click: open, template: "<div class='gridToolBar'></div>" });
            for (i in arrData) {
                if (i == arrData.length - 1) {
                    columns.push({
//                        ...arrData[i],
                        field: arrData[i].fieldName,
                        title: arrData[i].fieldLabel,
                        locked: false,
                        lockable: false
                    });
                }
                else {
                    columns.push({
//                        ...arrData[i],
                        field: arrData[i].fieldName,
                        title: arrData[i].fieldLabel
                    });
                }

            }
            LoadGrid(columns);
        })
    }

    //Reload grid..
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
            ids.push(selectedItem.officeGuid);
        });
        return ids;
    };

    // Section grid..
    function LoadGrid(columns) {
        var dataUrl = "/Admin/Office/Get?SearchValue=" + $("#SearchValue").val() + '&sortField=OfficeCode&sortDirection=asc';
        $($(gridId)).kendoGrid({
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
                        id: "officeGuid",
                        fields: columns.fieldLabel
                    }
                },
                sort: {
                    field: "officeCode",
                },
                serverPaging: true,
                pageSize: 20
            },

            height: 650,
            scrollable: true,
            sortable: true,
            sort: onSorting,
            resizable: true,
            columnMenu: {
                sortable: false
            },
            filterable: false,
            pageable: {
                input: true,
                numeric: true
            },
            columns: columns,
            dataBound: function () {
                var totalheader = $(".k-header-column-menu").length;
                $(".k-header-column-menu").each(function (i)
                {
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
                        data.url = "/Admin/Office/Detail/" + e.item.options.data.officeGuid;
                        var options = {
                            title: 'Detail of ' + e.item.options.data.officeName,
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
                        var id = e.item.options.data.officeGuid;
                        data.url = "/Admin/Office/Edit/" + e.item.options.data.officeGuid;
                        data.submitURL = "/Admin/Office/Edit";
                        var options = {
                            title: 'Edit Office ' + e.item.options.data.officeName,
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
                        var id = e.item.options.data.officeGuid;
                        var officeName = e.item.options.data.officeName;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  delete office " + officeName + " ?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/Admin/Office/Delete', ids);
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
                        var id = e.item.options.data.officeGuid;
                        var officeName = e.item.options.data.officeName;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to disable office " + officeName + " ?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/Admin/Office/Disable', ids);
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
                        var id = e.item.options.data.officeGuid;
                        var officeName = e.item.options.data.officeName;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to enable office " + officeName + " ?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/Admin/Office/Enable', ids);
                            },
                            cancel: function (e) {
                            }
                        });

                    }
                }
            ]

        }).data("kendoGrid");
    } // end of grid..

    $("#addNewOffice").on('click',
        function (evt) {
            var data = {}
            data.url = "/Admin/Office/Add/";
            data.submitURL = "/Admin/Office/Add/";
            var options = {
                title: 'Add Office',
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
                        secondary: true,
                        action: function (e) {
                        }
                    }
                ]
            };
            Dialog.openDialogSubmit(data, options);
        });

    // Section texbox Search //
    $('#btnSearch').click(function (e) {
        var searchvalue = $("#SearchValue").val();
        window.location.href = "/Admin/Office/Index?SearchValue=" + searchvalue;
    });

    $('#clearSearch').click(function () {
        $("#SearchValue").val('');
        window.location.href = "/Admin/Office/Index";
    });

    $(document).on("click", "#DisableOffice", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("Please select item/s first before disabling.");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  disable " + ids.length / 2 + " office(s)?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/Admin/Office/Disable', ids);
            },
            cancel: function (e) {
            }
        });
    });


    $(document).on("click", "#EnableOffice", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("Please select item/s first before enabling.");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  enable " + ids.length / 2 + " office(s)?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/Admin/Office/Enable', ids);
            },
            cancel: function (e) {
            }
        });
    });

    $(document).on("click", "#DeleteOffice", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("Please select item/s first before deleting.");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  delete " + ids.length / 2 + " office(s)?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/Admin/Office/Delete', ids);
            },
            cancel: function (e) {
            }
        });
    });

    function onSorting(arg) {
        var dataUrl = "/Admin/Office/Get?SearchValue=" + $("#SearchValue").val() + '&sortField=' + arg.sort.field + '&sortDirection=' + (arg.sort.dir == null ? "asc" : arg.sort.dir);
        var grid = $(gridId).data("kendoGrid");
        grid.dataSource.options.transport.read.url = dataUrl;
        ReloadGrid();
    }
    $(document).on("change",
        ".drpCountry",
        function (event) {
            var id = $(this).attr('id');
            switch (id) {
                case "drpCountryPhysical":
                    getStatesByCountryId('PhysicalCountryId', $('#drpCountryPhysical').val());
                    break;
                case "drpCountryMailing":
                    getStatesByCountryId('MailingCountryId', $('#drpCountryMailing').val());
                    break;
            }
        });
    
    function getStatesByCountryId(element, value) {
        var selectToId = element.split('')[0] + 'StateId';
        $.ajax({
            dataType: 'json',
            type: "GET",
            url: "Office/GetStatesByCountryId?countryId=" + value,
            success: function (data) {
                if (data.data.length > 0) {
                    $("#" + selectToId).html("");
                    var markup = "<option value>--Select--</option>";
                    for (var x = 0; x < data.data.length; x++) {
                        markup += "<option value=" + data.data[x].keys + ">" + data.data[x].values + "</option>";
                    }
                    $("#" + selectToId).html(markup).show();
                    $("#" + selectToId).show();
                    $("#" + selectToId).get(0).selectedIndex = 0;

                } else {
                    $("#" + selectToId).html("<option value=''>No record found </option>");
                }
            }
        });
    }
});// end of document..