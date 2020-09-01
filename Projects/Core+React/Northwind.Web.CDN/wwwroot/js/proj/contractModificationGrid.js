$(document).ready(function () {

    var gridId = $("#ContractModificationGrid");

    InitialLoad();
    function InitialLoad() {
        LoadGrid();
        $("#FileToUpload").kendoUpload();
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
            ids.push(selectedItem.contractModificationGuid);
        });
        return ids;
    };

    // Section grid..
    function LoadGrid(columns) {
        var searchValue = $("#SearchValue").val() || '';
        var dataUrl = "/ContractModification/Get?ContractGuid=" + $("#ContractGuid").val() + "&SearchValue=" + searchValue + '&sortField=ModificationNumber&sortDirection=asc';

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
                        id: "contractModificationGuid",
                        fields: {
                            //                            contractNumber: { type: "string" },
                            //                            projectNumber: { type: "string" },
                            modificationNumber: { type: "string" },
                            contractTitle: { type: "string" },
                            modificationTitle: { type: "string" },
                            effectiveDate: { type: "string" },
                            enteredDate: { type: "string" },
                            popStart: { type: "string" },
                            popEnd: { type: "string" },
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
                //                { field: "contractNumber", title: "Contract Number", width: 150 },
                //                { field: "projectNumber", title: "Project Number", width: 150 },
                { field: "modificationNumber", title: "Modification Number", width: 150 },
                { field: "contractTitle", title: "Contract Title", width: 150 },
                { field: "modificationTitle", title: "Modification Title", width: 150 },
                { field: "effectiveDate", title: "POP Start", template: "#= kendo.toString(kendo.parseDate(effectiveDate, 'yyyy-MM-dd'), 'yyyy-MM-dd') #", width: 150 },
                { field: "enteredDate", title: "POP Start", template: "#= kendo.toString(kendo.parseDate(enteredDate, 'yyyy-MM-dd'), 'yyyy-MM-dd') #", width: 150 },
                { field: "popStart", title: "POP Start", template: "#= kendo.toString(kendo.parseDate(popStart, 'yyyy-MM-dd'), 'yyyy-MM-dd') #", width: 150 },
                { field: "popEnd", title: "POP End", template: "#= kendo.toString(kendo.parseDate(popEnd, 'yyyy-MM-dd'), 'yyyy-MM-dd') #", width: 150 },
                { field: "awardAmount", title: "Award Amount", width: 150 },
                { field: "updatedOn", title: "Updated On", template: "#= kendo.toString(kendo.parseDate(updatedOn, 'yyyy-MM-dd'), 'yyyy-MM-dd') #", width: 150 },
                { field: "isActiveStatus", title: "Status", width: 250, locked: false, lockable: false }
            ],
            dataBound: function () {
                totalHeader = $('.k-header-column-menu').length;
                var grid = $(gridId).data("kendoGrid");

                $('.k-header-column-menu').each(function () {
                    if ($(this).parent().attr("data-index") != parseInt(totalHeader)) {
                        var dataIndex = $(this).parent().attr("data-index");
                        grid.thead.find("[data-index = " + dataIndex + "]>.k-header-column-menu").remove();
                    }
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
                        data.url = "/ContractModification/Details/" + e.item.options.data.contractModificationGuid;
                        var options = {
                            title: 'Details of Contract Modification : ' + e.item.options.data.modificationNumber,
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
                    title: "Edit Contract Modification",
                    event: "onClick",
                    icon: 'pencil',
                    onClick: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var contractModificationGuid = e.item.options.data.contractModificationGuid;
                        data.url = "/ContractModification/Edit/" + contractModificationGuid;
                        data.submitURL = "/ContractModification/Edit";
                        var options = {
                            title: 'Edit Contract Modification : ' + e.item.options.data.modificationNumber,
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
                        var id = e.item.options.data.contractModificationGuid;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  delete ContractModification?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/ContractModification/Delete', ids);
                            },
                            cancel: function (e) {
                            }
                        });
                    }
                },
                {
                    title: "Disable cm",
                    event: "onDisable",
                    icon: 'cancel',
                    onDisable: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.contractModificationGuid;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  disable ContractModification?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/ContractModification/Disable', ids);
                            },
                            cancel: function (e) {
                            }
                        });
                    }
                },
                {
                    title: "Enable CM",
                    event: "onEnable",
                    icon: 'check',
                    onEnable: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.contractModificationGuid;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  enable ContractModification?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/ContractModification/Enable', ids);
                            },
                            cancel: function (e) {
                            }
                        });
                    }
                }
            ]
        }).data("kendoGrid");
    } // end of grid..



    $(document).on("click", "#DisableContractModification", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("No rows selected !!");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  disable <b>" + ids.length / 2 + "</b> ContractModification?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/ContractModification/Disable', ids);
            },
            cancel: function (e) {
            }
        });
    });


    $(document).on("click", "#EnableContractModification", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("No rows selected !!");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  enable <b>" + ids.length / 2 + "</b> ContractModification?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/ContractModification/Enable', ids);
            },
            cancel: function (e) {
            }
        });
    });

    $(document).on("click", "#DeleteContractModification", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("No rows selected !!");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  delete <b>" + ids.length / 2 + "</b> ContractModification?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/ContractModification/Delete', ids);
            },
            cancel: function (e) {
            }
        });
    });

    $(document).on("click", "#idAddContractMod", function () {
        var data = {}
        var contractGuid = $('#ContractGuid').val();
        var ContractNumber = $('.getContractNumber').attr("id");
        data.url = "/ContractModification/add?ContractGuid=" + contractGuid;
        data.submitURL = "/ContractModification/add";
        var options = {
            title: 'Add Contract Modification for : ' + ContractNumber,
            height: '85%',
            events: [
                {
                    text: "Save",
                    primary: true,
                    action: function (e, values) {
                        $("#loadingFileUpload").show();
                        window.uploaderMod.onSubmitFiles(values.contractGuid, values.uploadPath, false, '', 'ContractModification', values.resourceId);
                    }
                },
                {
                    text: "Cancel",
                    action: function (e) {
                    }
                }
            ]
        };
        //var dialog = $("#dialog").data("kendoDialog");
        //dialog.close();
        Dialog.openDialogSubmit(data, options);
    });

    function onSorting(arg) {
        var searchValue = $("#SearchValue").val() || '';
        var dataUrl = "/ContractModification/Get?ContractGuid=" + $("#ContractGuid").val() + "&SearchValue=" + searchValue + '&sortField=' + arg.sort.field + '&sortDirection=' + (arg.sort.dir == null ? "asc" : arg.sort.dir);

        var grid = $(gridId).data("kendoGrid");
        grid.dataSource.options.transport.read.url = dataUrl;
        ReloadGrid();
    }

});// end of document..