$(document).ready(function () {

    var gridId = $("#ProjectModificationGrid");

    InitialLoad();
    function InitialLoad() {
        $(".ProjectModificationPanel").kendoPanelBar({
            expandMode: "single"
        });
        LoadGrid();
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
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'X-CSRF-TOKEN': token,
                'RequestVerificationToken': reqValToken
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
            ids.push(selectedItem.projectModificationGuid);
        });
        return ids;
    };

    // Section grid..
    function LoadGrid() {
        var searchValue = $("#SearchValue").val() || '';
        var dataUrl = "/ProjectModification/Get?ProjectGuid=" + $("#ProjectGuid").val() + "&SearchValue=" + searchValue + '&sortField=ModificationNumber&sortDirection=asc';
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
            sort: onSorting,
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
            dataBound: function () { new GridExtention().loadGridMenu($(gridId)) },
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
                        data.url = "/ProjectModification/Details/" + e.item.options.data.projectModificationGuid;
                        var options = {
                            title: 'Details of ' + e.item.options.data.projectModificationGuid,
                            events: [
                                {
                                },
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
                    event: "onClick",
                    icon: 'pencil',
                    onClick: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var projectModificationGuid = e.item.options.data.projectModificationGuid;
                        data.url = "/ProjectModification/Edit/" + projectModificationGuid;
                        data.submitURL = "/ProjectModification/Edit";
                        var options = {
                            title: 'Edit Project Modification : ' + e.item.options.data.modificationNumber,
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
                    event: "onDisable",
                    icon: 'cancel',
                    onDisable: function (e) {
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
                    event: "onEnable",
                    icon: 'check',
                    onEnable: function (e) {
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
        }).data("kendoGrid");
    } // end of grid..

    /// set column menu in only last column header.. ///
    totalHeader = $('.k-header-column-menu').length;
    var grid = $(gridId).data("kendoGrid");

    $('.k-header-column-menu').each(function () {
        if ($(this).parent().attr("data-index") != parseInt(totalHeader + 1)) {
            var dataIndex = $(this).parent().attr("data-index");
            grid.thead.find("[data-index = " + dataIndex + "]>.k-header-column-menu").remove();
        }
    });

    $(document).on("click", "#DisableProjectModification", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("No rows selected !!");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  disable <b>" + ids.length / 2 + "</b> ProjectModification?",
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
            text: "Are you sure you want to  enable <b>" + ids.length / 2 + "</b> ProjectModification?",
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
            text: "Are you sure you want to  delete <b>" + ids.length / 2 + "</b> ProjectModification?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/ProjectModification/Delete', ids);
            },
            cancel: function (e) {
            }
        });
    });

    function onSorting(arg) {
        var searchValue = $("#SearchValue").val() || '';
        var dataUrl = "/ProjectModification/Get?ProjectGuid=" + $("#ProjectGuid").val() + "&SearchValue=" + searchValue + '&sortField=' + arg.sort.field + '&sortDirection=' + (arg.sort.dir == null ? "asc" : arg.sort.dir);

        var grid = $(gridId).data("kendoGrid");
        grid.dataSource.options.transport.read.url = dataUrl;
        ReloadGrid();
    }
    $(document).on("click", "#idAddProjectMod", function () {
        var data = {}
        var projectGuid = $('#ContractGuid').val();
        var projectNumber = $('.getProjectNumber').attr("id");
        data.url = "/projectModification/add?projectGuid=" + projectGuid;
        data.submitURL = "/projectModification/add";

        var options = {
            title: 'Add Task Order Modification For : ' + projectNumber,
            height: '85%',
            events: [
                {
                    text: "Save",
                    primary: true,
                    action: function (e, values) {
                        //                        window.location.href = "/Project/Details/" + projectGuid;

                        $("#loadingFileUpload").show();
                        //window.uploaderMod.onSubmitFiles(values.resourceId, values.uploadPath);
                        window.uploaderMod.onSubmitFiles(values.contractGuid, values.uploadPath, false, '', 'ContractModification', values.resourceId);
                        //    , () => {
                        //    $("#loadingFileUpload").hide();
                        //    projectAndModule.refresh();
                        //});
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
});// end of document..