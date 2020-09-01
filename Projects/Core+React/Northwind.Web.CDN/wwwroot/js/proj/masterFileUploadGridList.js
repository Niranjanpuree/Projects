$(document).ready(function () {
    var uploadGrid = document.getElementsByClassName("MasterFileUploadGridList");
    var gridName = uploadGrid[0].getAttribute("data-gridname");
    var controller = uploadGrid[0].getAttribute("data-controller");
    var idvalue = uploadGrid[0].getAttribute("data-idvalue");
    var guid = uploadGrid[0].getAttribute("data-guid");
    var fields = uploadGrid[0].getAttribute("data-fields");
    var titles = uploadGrid[0].getAttribute("data-titles");
    var editProp = uploadGrid[0].getAttribute("data-editable");
    var downloadGrid = uploadGrid[0].getAttribute("data-downloadgrid");
    var fileUploadName = uploadGrid[0].getAttribute("data-path");
    var addRow = uploadGrid[0].getAttribute("data-addrow");

    var gridId = $(gridName);

    var fieldArr = fields.split("|");
    var titleArr = titles.split("|");

    InitialLoad();

    function InitialLoad() {
        LoadGrid();
    }

    var Path = fileUploadName.split("/");
    var fileName = Path[Path.length - 1]

    //Reload grid..
    var ReloadGrid = function () {
        $(gridId).data('kendoGrid').dataSource.read();
        $(gridId).data('kendoGrid').refresh();
    }

    // Section grid..
    function LoadGrid() {
        var dataUrl = "/" + controller + "/Get?id=" + $(idvalue).val();
        var columns = generateColumns();
        var reqfields = generateFields();
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
                        id: guid,
                        fields: reqfields
                    }
                }
            },
            height: 650,
            scrollable: true,
            resizable: true,
            sortable: true,
            sort: onSorting,
            columnMenu: true,
            navigatable: true,
            filterable: false,
            columns: columns,
            dataBound: function () { new GridExtention().loadGridMenu($(gridId)) },
            progress: false,
            events: [
            ],
            editable: parseInt(editProp) == 1 ? true : false

        }).data("kendoGrid");

        function generateFields() {
            var reqfields = {};
            for (var i = 0; i < fieldArr.length; i++) {
                eval("reqfields.field" + i + " = { " + fieldArr[i] + ": { type: \"string\" }}");
            }
            return reqfields;
        }

        function generateColumns() {
            var columns = [];
            for (var i = 0; i < fieldArr.length; i++) {
                columns.push({ field: fieldArr[i], title: titleArr[i], width: 100 })
            }
            if (editProp == 1)
                columns.push({
                    command: [{
                        name: "Delete", click: function (e1) {
                            var sender = this;
                            Dialog.confirm({
                                text: "Are you sure you want to delete this?", title: "Delete confirmation", sender: e1,
                                ok: function (e, e1) {
                                    var tr = $(e1.target).closest("tr");
                                    var data = sender.dataItem(tr);
                                    var dataSource = $($(gridId)).data("kendoGrid").dataSource;
                                    dataSource.remove(data);
                                },
                                cancel: function (e) {
                                }
                            })
                        }
                    }], title: "", width: 50
                });
            return columns;
        }

        $(downloadGrid).on('click', function (e) {
            var displayedData = gridId.data().kendoGrid.dataSource.view();
            var displayedDataAsJSON = JSON.stringify(displayedData);
            var url = "/project/" + controller;
            $.ajax({
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                dataType: 'json',
                url: url,
                type: "POST",
                data: JSON.stringify(displayedDataAsJSON),
                success: function (values) {
                    window.location.href = "/project/DownloadDocument?filePath=" + fileUploadName + "&&fileName=" + fileName;
                },
                error: function (values) {
                    $.notify(values.message, values.status);
                }
            });
        });

        $(addRow).on('click', function (e) {
            var dataGrid = $($(gridId)).data("kendoGrid");
            var dataSource = dataGrid.dataSource;
            dataSource.add({ projectGuid: $(idvalue).val() })
            var content = dataGrid.content;
            content.scrollTop(content[0].scrollHeight);
        });
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

    function onSorting(arg) {
        var dataUrl = "/project/" + controller + "?id=" + $(idvalue).val() + '&sortField=' + arg.sort.field + '&sortDirection=' + (arg.sort.dir == null ? "asc" : arg.sort.dir);
        var grid = $(gridId).data("kendoGrid");
        grid.dataSource.options.transport.read.url = dataUrl;
        grid.dataSource.read();
        grid.refresh();
    }

});// end of document..
