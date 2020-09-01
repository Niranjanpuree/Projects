$(document).ready(function () {

    var gridId = $("#ContractGrid");

    var ajaxGet = function (url) {
        return $.ajax({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            dataType: 'json',
            url: url,
            Type: "GET"
        });
    }

    InitialLoad();
    function InitialLoad() {
        ajaxGet("/GridFields/Contract").then(function (data) {
            var arrData = data.sort(function (a, b) {
                return a.orderIndex - b.orderIndex;
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
                } else {
                    columns.push({
                        //                        ...arrData[i],
                        field: arrData[i].fieldName,
                        title: arrData[i].fieldLabel
                    });
                }

            }
            LoadGrid(columns);
        });
    }

    //Reload grid..
    var ReloadGrid = function () {
        $(gridId).data('kendoGrid').dataSource.read();
        $(gridId).data('kendoGrid').refresh();
    }

    //Generic ajax post for commands..
    var ajaxPost = function (url, data) {
        $.ajax({
            headers: {
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
            ids.push(selectedItem.contractGuid);
        });
        return ids;
    };

    // Section grid..
    function LoadGrid(columns) {
        var dataUrl = "/contract/Get?SearchValue=" + $("#SearchValue").val() + '&FilterList=' + $("#idFilterList").val() + '&ShowHideTaskOrder=' + $("#ShowHideTaskOrder").is(":checked") + '&sortField=contractNumber&sortDirection=asc';
        $($(gridId)).kendoGrid({
            resource: 'Contract',
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
                        id: "contractGuid",
                        fields: columns.fieldLabel
                    }
                },
                serverPaging: true,
                pageSize: 20
            },
            height: 650,
            scrollable: true,
            sortable: true,
            sort: onSorting,
            resizable: true,
            columnMenu: true,
            filterable: false,
            pageable: {
                input: true,
                numeric: true
            },
            columns: columns,
            dataBound: function (e) {
                var totalheader = $(".k-header-column-menu").length;
                $(".k-header-column-menu").each(function (i) {
                    if (i < parseInt(totalheader - 1)) {
                        $(this).remove();
                    }
                });
                new GridExtention().loadGridMenuContractWithProjectMenu($(gridId));
                IconForContractAndProject();
            },
            editable: "",
            progress: false,
            events: [
                {
                    title: "Details",
                    event: "onClick",
                    objectType: "contract",
                    icon: 'info',
                    onClick: function (e) {
                        window.location.href = "/contract/Details/" + e.item.options.data.contractGuid;
                    }
                },
                {
                    title: "Edit",
                    event: "onClick",
                    objectType: "contract",
                    icon: 'pencil',
                    onClick: function (e) {
                        window.location.href = "/contract/Edit/" + e.item.options.data.contractGuid;
                    }
                },
                {
                    title: "Delete",
                    event: "onDelete",
                    objectType: "contract",
                    icon: 'delete',
                    onDelete: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.contractGuid;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  delete contract?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/Contract/Delete', ids);
                            },
                            cancel: function (e) {
                            }
                        });
                    }
                },
                {
                    title: "Disable",
                    event: "onDisable",
                    objectType: "contract",
                    icon: 'cancel',
                    onDisable: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.contractGuid;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  disable contract?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/Contract/Disable', ids);
                            },
                            cancel: function (e) {
                            }
                        });
                    }
                },
                {
                    title: "Enable",
                    event: "onEnable",
                    objectType: "contract",
                    icon: 'check',
                    onEnable: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.contractGuid;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  enable contract?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/Contract/Enable', ids);
                            },
                            cancel: function (e) {
                            }
                        });
                    }
                },
                {
                    title: "Add Mod",
                    event: "onCreate",
                    objectType: "contract",
                    icon: 'plus',
                    onCreate: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var contractGuid = e.item.options.data.contractGuid;
                        data.url = "/ContractModification/_add?ContractGuid=" + contractGuid;
                        data.submitURL = "/ContractModification/_add";

                        var options = {
                            title: 'Add Contract Modification For : ' + e.item.options.data.contractNumber,
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
                    title: "Add Task Order",
                    event: "addProject",
                    objectType: "contract",
                    //                    filterBy :"idiq",
                    icon: 'plus',
                    addProject: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var contractGuid = e.item.options.data.contractGuid;
                        window.location.href = "/project/add?ContractGuid=" + contractGuid;
                    }
                },
                {
                    title: "Details",
                    event: "onClick",
                    objectType: "project",
                    icon: 'info',
                    onClick: function (e) {
                        window.location.href = "/Project/Details/" + e.item.options.data.contractGuid;
                    }
                },
                {
                    title: "Edit",
                    event: "onClick",
                    objectType: "project",
                    icon: 'pencil',
                    onClick: function (e) {
                        window.location.href = "/Project/Edit/" + e.item.options.data.contractGuid;
                    }
                },
                {
                    title: "Delete",
                    event: "onDelete",
                    objectType: "project",
                    icon: 'delete',
                    onDelete: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.contractGuid;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  delete Project?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/Project/Delete', ids);
                            },
                            cancel: function (e) {
                            }
                        });
                    }
                },
                {
                    title: "Disable",
                    event: "onDisable",
                    objectType: "project",
                    icon: 'cancel',
                    onDisable: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.contractGuid;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  disable Project?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/Project/Disable', ids);
                            },
                            cancel: function (e) {
                            }
                        });
                    }
                },
                {
                    title: "Enable",
                    event: "onEnable",
                    objectType: "project",
                    icon: 'check',
                    onEnable: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.contractGuid;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  enable Project?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/Project/Enable', ids);
                            },
                            cancel: function (e) {
                            }
                        });
                    }
                },
                {
                    title: "Add Mod",
                    event: "onCreate",
                    objectType: "project",
                    icon: 'plus',
                    onCreate: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var projectGuid = e.item.options.data.contractGuid;
                        data.url = "/projectModification/_add?projectGuid=" + projectGuid;
                        data.submitURL = "/projectModification/_add";

                        var options = {
                            title: 'Add Task Order Modification For:' + e.item.options.data.projectNumber,
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
                }
            ]
        }).data("kendoGrid");
    } // end of grid..

    $("#addNewContract").on('click',
        function (e) {
            window.location.href = "/contract/Add";
        }
    );

    // Section texbox Search //
    $('#btnSearch').click(function (e) {
        var searchvalue = $("#SearchValue").val();
        window.location.href = "/contract/Index?SearchValue=" + searchvalue + '&ShowHideTaskOrder=' + $("#ShowHideTaskOrder").is(":checked");
    });

    $(document).on("click", "#DisableContract", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("No rows selected !!");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  disable <b>" + ids.length / 2 + "</b> contract?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/Contract/Disable', ids);
                ajaxPost('/Project/Disable', ids);
            },
            cancel: function (e) {
            }
        });
    });

    $(document).on("click", "#EnableContract", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("No rows selected !!");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  enable <b>" + ids.length / 2 + "</b> contract?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/Contract/Enable', ids);
                ajaxPost('/Project/Enable', ids);
            },
            cancel: function (e) {
            }
        });
    });

    $(document).on("click", "#DeleteContract", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("No rows selected !!");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  delete <b>" + ids.length / 2 + "</b> contract?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/Contract/Delete', ids);
                ajaxPost('/Project/Delete', ids);
            },
            cancel: function (e) {
            }
        });
    });

    function onSorting(arg) {
        var dataUrl = "/Contract/Get?SearchValue=" + $("#SearchValue").val() + '&FilterList=' + $("#idFilterList").val() + '&ShowHideTaskOrder=' + $("#ShowHideTaskOrder").is(":checked") + '&sortField=' + arg.sort.field + '&sortDirection=' + (arg.sort.dir == null ? "asc" : arg.sort.dir);
        var grid = $(gridId).data("kendoGrid");
        grid.dataSource.options.transport.read.url = dataUrl;
        ReloadGrid();
    }

    $(document).on("change", "#idFilterList", function () {
        var searchvalue = $("#SearchValue").val();
        var FilterList = $(this).val();
        window.location.href = "/contract/Index?SearchValue=" + searchvalue + '&FilterList=' + FilterList;

    });

    $("#ShowHideTaskOrder").change(function () {
        var searchText = $("#SearchValue").val();
        $(this).parent().parent().remove();
        var newSearchText = searchText.replace($(this).parent().parent().attr('id'), '');

        if ($(this).is(":checked")) {
            window.location.href = "/contract/Index?SearchValue=" + newSearchText + '&ShowHideTaskOrder=true';
        }
        else {
            window.location.href = "/contract/Index?SearchValue=" + newSearchText + '&ShowHideTaskOrder=false';
        }
    });
    $(document).on('click', '.btnSearchPills', function () {
        var searchText = $("#SearchValue").val();
        $(this).parent().parent().remove();
        var newSearchText = searchText.replace($(this).parent().parent().attr('id'), '');
        window.location.href = "/contract/Index?SearchValue=" + $.trim(newSearchText) + '&ShowHideTaskOrder=' + $("#ShowHideTaskOrder").is(":checked");
    });
    function IconForContractAndProject() {
        $("#ContractGrid").find("tr").each(function () {
            var text = $(this).find("td[role=gridcell]").eq(1).html();
            var td = $(this).find("td[role=gridcell]").eq(1);
            if (text && text.indexOf("(C)") >= 0) {
                td.addClass("project-icon");
                text = text.replace("(C)", "");
                td.html(text);
            }
            else if (text && text.indexOf("(P)") >= 0) {
                td.addClass("contract-icon");
                text = text.replace("(P)", "");
                td.html(text);
            }
        });
    }
});// end of document..