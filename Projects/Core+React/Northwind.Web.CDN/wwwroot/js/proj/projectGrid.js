$(document).ready(function () {

    var gridId = $("#ProjectGrid");

    InitialLoad();
    function InitialLoad() {
        LoadGrid();

        /// set column menu in only last column header.. ///
        totalHeader = $('.k-header-column-menu').length;
        var grid = $(gridId).data("kendoGrid");

        $('.k-header-column-menu').each(function () {
            if ($(this).parent().attr("data-index") != parseInt(totalHeader + 1)) {
                var dataIndex = $(this).parent().attr("data-index");
                grid.thead.find("[data-index = " + dataIndex + "]>.k-header-column-menu").remove();
            }
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
            ids.push(selectedItem.projectGuid);
        });
        return ids;
    };

    // Section grid..
    function LoadGrid() {
        var searchValue = $("#SearchValue").val() || '';
        var dataUrl = "/Project/Get?ContractGuid=" + $("#ContractGuid").val() + "&SearchValue=" + searchValue + '&sortField=ProjectNumber&sortDirection=asc';
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
                        id: "projectGuid",
                        fields: {
                            projectNumber: { type: "string" },
                            contractTitle: { type: "string" },
                            projectTitle: { type: "string" },
                            awardAmount: { type: "number" },
                            pOPStart: { type: "date" },
                            pOPEnd: { type: "date" },
                            updatedOn: { type: "date" },
                            isActiveStatus: { type: "string" }
                        }
                    }
                },
                serverPaging: true,
                pageSize: 10
            },
            height: 650,
            scrollable: true,
            sortable: true,
            sort: onSortingProject,
            resizable: true,
            detailInit: detailInit,
            //            dataBound: function () { this.expandRow(this.tbody.find("tr.k-master-row").first()); new GridExtention().loadGridMenu($(gridId)) },
            dataBound: function () {
                var dataSource = this.dataSource;
                this.element.find('tr.k-master-row').each(function () {
                    var row = $(this);
                    var data = dataSource.getByUid(row.data('uid'));

                    $.ajax({
                        dataType: 'json',
                        url: "/Project/HasChild?projectGuid=" + data.id,
                        type: "GET",
                        success: function (data) {
                            if (data.data == false) {
                                row.find('.k-hierarchy-cell a').css({ opacity: 0.0, cursor: 'default' }).click(function (e) { e.stopImmediatePropagation(); return false; });
                            }
                        },
                        error: function (values) {
                            $.notify(values.message, values.status);
                        }
                    });
                });
                new GridExtention().loadGridMenu($(gridId));
            },
            columnMenu: true,
            filterable: false,
            pageable: {
                input: true,
                numeric: true
            },
            columns: [
                { selectable: true, width: "35px", locked: false, lockable: true },
                { name: 'open', width: 30, click: open, template: "<div class='gridToolBar'></div>" },
                { field: "projectNumber", title: "Project Number", width: 150 },
                { field: "contractTitle", title: "Contract Detail", width: 250 },
                { field: "projectTitle", title: "Project Title", width: 250 },
                { field: "awardAmount", title: "Award Amount", width: 150 },
                { field: "pOPStart", title: "Period of Performance Start", template: "#= kendo.toString(kendo.parseDate(periodofperformancestart, 'yyyy-MM-dd'), 'yyyy-MM-dd') #", width: 250 },
                { field: "pOPEnd", title: "Period of Performance End", template: "#= kendo.toString(kendo.parseDate(periodofperformanceend, 'yyyy-MM-dd'), 'yyyy-MM-dd') #", width: 250 },
                { field: "updatedOn", title: "Updated On", template: "#= kendo.toString(kendo.parseDate(updatedOn, 'yyyy-MM-dd'), 'yyyy-MM-dd') #", width: 150 },
                { field: "isActiveStatus", title: "Status", width: 250, locked: false, lockable: false }
            ],
            editable: "",
            progress: false,
            events: [
                {
                    title: "Details",
                    event: "onClick",
                    icon: 'info',
                    onClick: function (e) {
                        window.location.href = "/Project/Detail/" + e.item.options.data.projectGuid;
                    }
                },
                {
                    title: "Edit",
                    event: "onClick",
                    icon: 'pencil',
                    onClick: function (e) {
                        window.location.href = "/Project/Edit/" + e.item.options.data.projectGuid;
                    }
                },
                {
                    title: "Delete",
                    event: "onDelete",
                    icon: 'delete',
                    onDelete: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.projectGuid;
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
                    icon: 'cancel',
                    onDisable: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.projectGuid;
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
                    icon: 'check',
                    onEnable: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.projectGuid;
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
                    title: "Add Task Order Modification",
                    event: "onCreate",
                    icon: 'plus',
                    onCreate: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var projectGuid = e.item.options.data.projectGuid;
                        data.url = "/projectModification/add?projectGuid=" + projectGuid;
                        data.submitURL = "/projectModification/add";

                        var options = {
                            title: 'Add Task Order Modification :' + e.item.options.data.projectNumber + ' ' + e.item.options.data.projectTitle,
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

    function detailInit(e) {
        var searchValue = $("#SearchValue").val() || '';
        var dataUrl = "/ProjectModification/Get?ProjectGuid=" + e.data.projectGuid + "&SearchValue=" + searchValue + '&sortField=ModificationNumber&sortDirection=asc';
        $("<div class='projectDetailsGrid'/>").appendTo(e.detailCell).kendoGrid({
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
                        id: "projectModificationGuid",
                        fields: {
                            projectNumber: { type: "string" },
                            modificationNumber: { type: "string" },
                            contractTitle: { type: "string" },
                            modificationTitle: { type: "string" },
                            popStart: { type: "date" },
                            popEnd: { type: "date" },
                            awardAmount: { type: "string" },
                            updatedOn: { type: "date" },
                            isActiveStatus: { type: "string" }
                        }
                    }
                },
                serverPaging: true,
                pageSize: 10
            },
            height: 650,
            scrollable: true,
            sortable: true,
            resizable: true,
            columnMenu: true,
            filterable: false,
            pageable: {
                input: true,
                numeric: true
            },
            columns: [
                { selectable: true, width: "35px", locked: true, lockable: true },
                { name: 'open', width: 30, click: open, template: "<div class='gridToolBar'></div>" },
                { field: "projectNumber", title: "Project Number", width: 150 },
                { field: "modificationNumber", title: "Modification Number", width: 150 },
                { field: "contractTitle", title: "Contract Title", width: 150 },
                { field: "modificationTitle", title: "Modification Title", width: 150 },
                { field: "popStart", title: "POP Start", template: "#= kendo.toString(kendo.parseDate(popStart, 'yyyy-MM-dd'), 'yyyy-MM-dd') #", width: 150 },
                { field: "popEnd", title: "POP End", template: "#= kendo.toString(kendo.parseDate(popEnd, 'yyyy-MM-dd'), 'yyyy-MM-dd') #", width: 150 },
                { field: "awardAmount", title: "Award Amount", width: 150 },
                { field: "updatedOn", title: "Updated On", template: "#= kendo.toString(kendo.parseDate(updatedOn, 'yyyy-MM-dd'), 'yyyy-MM-dd') #", width: 150 },
                { field: "isActiveStatus", title: "Status", width: 250, locked: false, lockable: false }
            ],
            dataBound: function (e) {
                new GridExtention().loadGridMenu(e.sender.element);
            },
            editable: "",
            progress: false,
            events: [
                {
                    title: "Details",
                    event: "onViewProjectMod",
                    icon: 'info',
                    onViewProjectMod: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        data.url = "/ProjectModification/Details/" + e.item.options.data.projectModificationGuid;
                        var options = {
                            title: 'Details of Project :' + e.item.options.data.projectTitle + ' , Modification Number :' + e.item.options.data.modificationNumber,
                            events: [
                                {
                                    text: "Cancel",
                                    action: function (e) {
                                    }
                                }
                            ]
                        };
                        Dialog.openDialog(data, options);
                    }
                },
                {
                    title: "Edit Project Modification",
                    event: "onEditProjectMod",
                    icon: 'pencil',
                    onEditProjectMod: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var projectModificationGuid = e.item.options.data.projectModificationGuid;
                        data.url = "/ProjectModification/Edit/" + projectModificationGuid;
                        data.submitURL = "/ProjectModification/Edit";
                        var options = {
                            title: 'Edit of Project :' + e.item.options.data.projectTitle + ' , Modification Number :' + e.item.options.data.modificationNumber,
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
                    event: "onDeleteProjectMod",
                    icon: 'delete',
                    onDeleteProjectMod: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.projectModificationGuid;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  delete ProjectModification?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/ProjectModification/Delete', ids);
                            },
                            cancel: function (e) {
                            }
                        });
                    }
                },
                {
                    title: "Disable",
                    event: "onDisableProjectMod",
                    icon: 'cancel',
                    onDisableProjectMod: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.projectModificationGuid;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  disable ProjectModification?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/ProjectModification/Disable', ids);
                            },
                            cancel: function (e) {
                            }
                        });
                    }
                },
                {
                    title: "Enable",
                    event: "onEnableProjectMod",
                    icon: 'check',
                    onEnableProjectMod: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.projectModificationGuid;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  enable ProjectModification?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/ProjectModification/Enable', ids);
                            },
                            cancel: function (e) {
                            }
                        });
                    }
                }
            ]
        });
    } // end of grid..

    $("#addNewProject").on('click',
        function (e) {
            window.location.href = "/Project/Add";
        }
    );

    // Section texbox Search //
    $('#btnSearch').click(function (e) {
        var searchvalue = $("#SearchValue").val();
        window.location.href = "/Project/Index?SearchValue=" + searchvalue;
    });

    $(document).on("click", "#DisableProject", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("No rows selected !!");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  disable " + ids.length / 2 + " Project?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/Project/Disable', ids);
            },
            cancel: function (e) {
            }
        });
    });


    $(document).on("click", "#EnableProject", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("No rows selected !!");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  enable " + ids.length / 2 + " Project?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/Project/Enable', ids);
            },
            cancel: function (e) {
            }
        });
    });

    $(document).on("click", "#DeleteProject", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("No rows selected !!");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  delete " + ids.length / 2 + " Project?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/Project/Delete', ids);
            },
            cancel: function (e) {
            }
        });
    });

    function onSortingProject(arg) {
        var searchValue = $("#SearchValue").val() || '';
        var dataUrl = "/Project/Get?ContractGuid=" + $("#ContractGuid").val() + "&SearchValue=" + searchValue + '&sortField=' + arg.sort.field + '&sortDirection=' + (arg.sort.dir == null ? "asc" : arg.sort.dir);

        var grid = $(gridId).data("kendoGrid");
        grid.dataSource.options.transport.read.url = dataUrl;
        ReloadGrid();
    }

    $(document).on("click", "#DisableProjectModification", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("No rows selected !!");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  disable " + ids.length / 2 + " ProjectModification?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/ProjectModification/Disable', ids);
            },
            cancel: function (e) {
            }
        });
    });

    $(document).on("click", "#EnableProjectModification", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("No rows selected !!");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  enable " + ids.length / 2 + " ProjectModification?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/ProjectModification/Enable', ids);
            },
            cancel: function (e) {
            }
        });
    });

    $(document).on("click", "#DeleteProjectModification", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("No rows selected !!");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  delete " + ids.length / 2 + " ProjectModification?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/ProjectModification/Delete', ids);
            },
            cancel: function (e) {
            }
        });
    });
});// end of document..
