$(document).ready(function () {

    var gridId = $("#RegionGrid");

    var ajaxGet = function (url) {
        var dt = new Date().getTime();
        var url = url.indexOf("?") > 0 ? url + "&dt=" + dt : url + "?dt=" + dt;


        return $.ajax({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            cache: false,
            dataType: 'json',
            url: url,
            type: "GET"
        });
    }
    InitialLoad();

    function InitialLoad() {
        ajaxGet("/GridFields/region").then(function (data) {
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
            ids.push(selectedItem.regionGuid);
        });
        return ids;
    };

    // Section Table List/Grid //
    function LoadGrid(columns) {
        var dt = (new Date()).getTime();
        var dataUrl = "/Admin/Region/Get?SearchValue=" + $("#SearchValue").val() + '&sortField=regionCode&sortDirection=asc&dt=' + dt;

        $(gridId).kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        contentType: "application/json",
                        url: dataUrl,
                        dataType: "json",
                        cache: false
                    },
                },
                batch: true,
                schema: {
                    data: "data",
                    total: "total",
                    model: {
                        id: "regionGuid",
                        fields: columns.fieldLabel
                    }
                },
                serverPaging: true,
                pageSize: 20,

            },
            height: 650,
            scrollable: true,
            sortable: true,
            reorderable: true,
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
                $(".k-header-column-menu").each(function (i) {
                    $(this).remove();
                    //if (i < parseInt(totalheader - 1))
                    //{
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
                        data.url = "/Admin/Region/Detail/" + e.item.options.data.regionGuid;
                        var options = {
                            title: 'Details of ' + e.item.options.data.regionName,
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
                        var id = e.item.options.data.regionGuid;
                        data.url = "/Admin/Region/Edit/" + e.item.options.data.regionGuid;
                        data.submitURL = "/Admin/Region/Edit";
                        var options = {
                            title: 'Edit Region ' + e.item.options.data.regionName,
                            events: [
                                {
                                    text: "Save",
                                    primary: true,
                                    action: function (e, values) {
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
                        var id = e.item.options.data.regionGuid;
                        var regionName = e.item.options.data.regionName;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  delete region " + regionName + " ?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/Admin/Region/Delete', ids);
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
                        var id = e.item.options.data.regionGuid;
                        var regionName = e.item.options.data.regionName;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  disable region " + regionName + " ?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/Admin/Region/Disable', ids);
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
                        var id = e.item.options.data.regionGuid;
                        var regionName = e.item.options.data.regionName;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  enable region " + regionName + " ?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/Admin/Region/Enable', ids);
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

    $("#addNewRegion").on('click',
        function (evt) {
            var data = {}
            data.url = "/Admin/Region/Add/";
            data.submitURL = "/Admin/Region/Add/";
            var options = {
                title: 'Add New Region',
                events: [
                    {
                        text: "Save",
                        primary: true,
                        action: function (e, values) {
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
        });

    // Section texbox Search //
    $('#btnSearch').click(function (e) {
        var searchvalue = $("#SearchValue").val();
        window.location.href = "/Admin/Region/Index?SearchValue=" + searchvalue;
    });

    $('#clearSearch').click(function () {
        $("#SearchValue").val('');
        window.location.href = "/Admin/Region/Index";
    });

    $(document).on("click", "#DisableRegion", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("Please select item/s first before disabling.");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  disable " + ids.length / 2 + " region(s)?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/Admin/Region/Disable', ids);
            },
            cancel: function (e) {
            }
        });
    });

    $(document).on("click", "#EnableRegion", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("Please select item/s first before enabling.");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  enable " + ids.length / 2 + " region(s)?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/Admin/Region/Enable', ids);
            },
            cancel: function (e) {
            }
        });
    });

    $(document).on("click", "#DeleteRegion", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("Please select item/s first before deleting.");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  delete " + ids.length / 2 + " region(s)?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/Admin/Region/Delete', ids);
            },
            cancel: function (e) {
            }
        });
    });

    function onSorting(arg) {
        var dataUrl = "/Admin/Region/Get?SearchValue=" + $("#SearchValue").val() + '&sortField=' + arg.sort.field + '&sortDirection=' + (arg.sort.dir == null ? "asc" : arg.sort.dir);
        var grid = $(gridId).data("kendoGrid");
        grid.dataSource.options.transport.read.url = dataUrl;
        grid.dataSource.read();
        grid.refresh();
    }

});// end of document..