var ExportGrid = /** @class */ (function () {
    function ExportGrid() {
    }
    ExportGrid.exportToCSVDialog = function (kendogrd, dialog, resource) {
        var datasource = kendogrd.dataSource;
        var columns = [];
        $.ajax({
            type: 'get',
            url: "../export/" + resource,
            dataType: 'html',
            success: function (data) {
                dialog.open(data, {
                    title: "Export to Excel",
                    events: [
                        {
                            text: "Export",
                            primary: true,
                            action: function (e, values) {
                                var selected = [];
                                if (typeof (values.selectedOptions) == "string") {
                                    var index = values.fieldNames.indexOf(values.selectedOptions);
                                    selected.push({
                                        title: values.fieldLabels[index],
                                        field: ExportGrid.formatFieldName(values.selectedOptions)
                                    });
                                }
                                else {
                                    for (var i in values.selectedOptions) {
                                        if (i != "__RequestVerificationToken") {
                                            var index = values.fieldNames.indexOf(values.selectedOptions[i]);
                                            selected.push({
                                                title: values.fieldLabels[index],
                                                field: ExportGrid.formatFieldName(values.selectedOptions[i])
                                            });
                                        }
                                    }
                                }
                                ExportGrid.exportToCSV(kendogrd, selected);
                            }
                        },
                        {
                            text: "Cancel",
                            primary: false,
                            action: function (e, values) {
                            }
                        }
                    ]
                });
            }
        });
    };
    ExportGrid.exportToPDFDialog = function (kendogrd, dialog, resource) {
        var datasource = kendogrd.dataSource;
        var columns = [];
        $.ajax({
            type: 'get',
            url: "../export/" + resource,
            dataType: 'html',
            success: function (data) {                
                dialog.open(data, {
                    title: "Export to PDF",
                    events: [
                        {
                            text: "Export",
                            primary: true,
                            action: function (e, values) {
                                var selected = [];
                                if (typeof (values.selectedOptions) == "string") {
                                    var index = values.fieldNames.indexOf(values.selectedOptions);
                                    selected.push({
                                        title: values.fieldLabels[index],
                                        field: ExportGrid.formatFieldName(values.selectedOptions)
                                    });
                                }
                                else {
                                    for (var i in values.selectedOptions) {
                                        if (i != "__RequestVerificationToken") {
                                            var index = values.fieldNames.indexOf(values.selectedOptions[i]);
                                            selected.push({
                                                title: values.fieldLabels[index],
                                                field: ExportGrid.formatFieldName(values.selectedOptions[i])
                                            });
                                        }
                                    }
                                }
                                ExportGrid.exportToPDF(kendogrd, selected);
                            }
                        },
                        {
                            text: "Cancel",
                            primary: false,
                            action: function (e, values) {
                            }
                        }
                    ]
                });
            }
        });
    };
    ExportGrid.exportToCSV = function (kendogrd, columns) {
        var dataSource = kendogrd.dataSource;
        var data = [];
        var selectedIDs = [];
        for (var i in kendogrd._selectedIds) {
            selectedIDs.push(i);
        }
        var gridColumns = columns;
        for (var i = 0; i < kendogrd.dataSource._data.length; i++) {
            if (selectedIDs.indexOf(kendogrd.dataSource._data[i].id) >= 0) {
                data.push(kendogrd.dataSource._data[i]);
            }
        }
        var grid = $("<div id='pdfGrid'></div>");
        grid.kendoGrid({
            toolbar: ["excel"],
            dataSource: data,
            height: 550,
            sortable: true,
            pageable: {
                refresh: true,
                pageSizes: true,
                buttonCount: 5
            },
            columns: gridColumns
        });
        grid.data("kendoGrid").saveAsExcel();
    };
    ExportGrid.exportToPDF = function (kendogrd, columns) {
        var gridColumns = columns;
        var data = [];
        var selectedIDs = [];
        for (var i in kendogrd._selectedIds) {
            selectedIDs.push(i);
        }
        for (var i = 0; i < kendogrd.dataSource._data.length; i++) {
            if (selectedIDs.indexOf(kendogrd.dataSource._data[i].id) >= 0) {
                data.push(kendogrd.dataSource._data[i]);
            }
        }
        var grid = $("<div id='pdfGrid'></div>");
        grid.kendoGrid({
            toolbar: ["pdf"],
            pdf: {
                allPages: true,
                avoidLinks: true,
                paperSize: "A4",
                margin: { top: "2cm", left: "1cm", right: "1cm", bottom: "1cm" },
                landscape: true,
                repeatHeaders: true,
                template: $("#page-template").html(),
                scale: 0.8
            },
            dataSource: data,
            height: 550,
            sortable: true,
            pageable: {
                refresh: true,
                pageSizes: true,
                buttonCount: 5
            },
            columns: gridColumns
        });
        grid.data("kendoGrid").saveAsPDF();
    };
    ExportGrid.formatFieldName = function (s) {
        return s.substring(0,1).toLowerCase()+s.substring(1);
    };
    ExportGrid.base64 = function (s) {
        return window.btoa(unescape(encodeURIComponent(s)));
    };
    ExportGrid.format = function (s, c) {
        return s.replace(/{(\w+)}/g, function (m, p) {
            return c[p];
        });
    };
    return ExportGrid;
}());