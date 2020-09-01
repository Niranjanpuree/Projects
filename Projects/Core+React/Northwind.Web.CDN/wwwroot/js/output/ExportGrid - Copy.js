var ExportGrid = /** @class */ (function () {
    function ExportGrid() {
    }
    ExportGrid.exportToCSVDialog = function (kendogrd, dialog) {
        var datasource = kendogrd.dataSource;
        var columns = [];
        $.ajax({
            type: 'get',
            url: "../export",
            dataType: 'html',
            success: function (data) {
                $("#dialog").find(".content").html(data);
                var form = $($("#dialog").find(".content").html());
                form = form.eq(0);
                var parent = form.find(".parent");
                parent = parent.eq(0);
                var content;
                var rows = "";
                form.find(".parent").children().each(function () {
                    content = $("<div></div>").append($(this)).html();
                    $(this).remove();
                });
                if (datasource._data.length > 0) {
                    for (var i in datasource._data[0]) {
                        if (typeof (eval("datasource._data[0]." + i)) == "string") {
                            columns.push(i);
                            var con = $(content);
                            con.find("input").eq(0).attr("id", i).attr("name", i);
                            con.find("label").eq(0).attr("for", i).html(i.replace(/([A-Z])/g, ' $1'));
                            rows += $("<div></div>").append(con).html();
                        }
                    }
                }
                form.find(".parent").eq(0).html(rows);
                dialog.open(form.html(), {
                    events: [
                        {
                            text: "Export",
                            primary: true,
                            action: function (e, values) {
                                var selected = [];
                                for (var i in values) {
                                    if (i != "__RequestVerificationToken") {
                                        selected.push(i);
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
    ExportGrid.exportToPDFDialog = function (kendogrd, dialog) {
        var datasource = kendogrd.dataSource;
        var columns = [];
        $.ajax({
            type: 'get',
            url: "../export",
            dataType: 'html',
            success: function (data) {
                $("#dialog").find(".content").html(data);
                var form = $($("#dialog").find(".content").html());
                form = form.eq(0);
                var parent = form.find(".parent");
                parent = parent.eq(0);
                var content;
                var rows = "";
                form.find(".parent").children().each(function () {
                    content = $("<div></div>").append($(this)).html();
                    $(this).remove();
                });
                if (datasource._data.length > 0) {
                    for (var i in datasource._data[0]) {
                        if (typeof (eval("datasource._data[0]." + i)) == "string") {
                            columns.push(i);
                            var con = $(content);
                            con.find("input").eq(0).attr("id", i).attr("name", i);
                            con.find("label").eq(0).attr("for", i).html(i.replace(/([A-Z])/g, ' $1'));
                            rows += $("<div></div>").append(con).html();
                        }
                    }
                }
                form.find(".parent").eq(0).html(rows);
                dialog.open(form.html(), {
                    events: [
                        {
                            text: "Export",
                            primary: true,
                            action: function (e, values) {
                                var selected = [];
                                for (var i in values) {
                                    if (i != "__RequestVerificationToken") {
                                        selected.push(i);
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
        var gridColumns = [];
        for (var i in columns) {
            gridColumns.push({
                field: columns[i],
                title: columns[i].replace(/([A-Z])/g, ' $1')
            });
        }
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
        var gridColumns = [];
        var data = [];
        for (var i in columns) {
            gridColumns.push({
                field: columns[i],
                title: columns[i].replace(/([A-Z])/g, ' $1')
            });
        }
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