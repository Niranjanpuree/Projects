var gridMenu = (function () {
    return {
        loadGridMenu: function (grid) {
            var ui = grid.data("kendoGrid")
            var items = []
            for (index in ui.getOptions().events) {
                var item = ui.getOptions().events[index]
                items.push({
                    type: "button",
                    text: item.title,
                    overflow: "always",
                    click: eval("ui.getOptions().events[" + index + "]." + item.event)
                })
            }

            grid.find(".gridToolBar").each(function (rowIndex) {
                var titems = []
                for (d in items) {
                    titems.push(items[d])
                    titems[titems.length - 1].data = ui._data[rowIndex]
                }
                $(this).kendoToolBar({ items: titems });
            })
        }

    }
})();



var dialogView = (function () {
    return {
        openDialog: function (data, options) {
            var tOptions = options;
            tOptions.width = tOptions.width || '50%';
            tOptions.height = tOptions.height || '50%';
            tOptions.closable = tOptions.closable || true;
            tOptions.actions = [
                {
                    text: tOptions.events[0].text,
                    primary: tOptions.events[0].primary,
                    action: function (e) {
                        if (!$('#dialog').find("form").valid()) {
                            return false;
                        }
                        var $form = $('#dialog').find("form").eq(0);
                        var value = dialogView.getFormData($form);
                        tOptions.events[0].action(e, value)
                    }
                },
                {
                    text: tOptions.events[1].text,
                    primary: tOptions.events[1].primary,
                    action: function (e) {
                        tOptions.events[1].action(e, null)
                    }
                }
            ];
            if (data.url != "") {
                $.ajax({
                    url: data.url,
                    type: 'get',
                    dataType: 'html',
                    success: function (d) {
                        $('#loading').hide();
                        $("#dialog").find(".content").html(d);
                        $("#dialog").find(".content").find("input, textarea").each(function (e) {
                            $(this).attr("name", $(this).attr("asp-for"))
                        })
                        $("#dialog").kendoDialog(options);
                        $("#dialog").css({ "display": "block" })
                        var dialog = $("#dialog").data("kendoDialog");
                        dialog.open();
                    }
                });
            }
            else {
                $("#dialog").kendoDialog(options);
                $("#dialog").find(".content").html(JSON.stringify(data))
                $("#dialog").css({ "display": "block" })
                var dialog = $("#dialog").data("kendoDialog");
                dialog.open();
            }
        },
        openDialogSubmit: function (data, options) {
            var tOptions = options;
            tOptions.width = tOptions.width || '50%';
            tOptions.height = tOptions.height || '50%';
            tOptions.closable = tOptions.closable || true;
            tOptions.actions = [
                {
                    text: tOptions.events[0].text,
                    primary: tOptions.events[0].primary,
                    action: function (e) {
                        if (!$('#dialog').find("form").valid()) {
                            return false;
                        }
                        var $form = $('#dialog').find("form").eq(0);
                        var value = dialogView.getFormData($form);
                        data.type = options.type || "post";
                        if (data.submitURL) {
                            $.ajax({
                                headers: {
                                    'Accept': 'application/json',
                                    'Content-Type': 'application/json'
                                },
                                type: data.type,
                                url: data.submitURL,
                                dataType: 'json',
                                data: JSON.stringify(value),
                                success: function (ee) {
                                    tOptions.events[0].action(e, ee);
                                    var dialog = $("#dialog").data("kendoDialog");
                                    dialog.close();
                                },
                                error: function (ee) {
                                    var message = "";
                                    for (i in ee.responseJSON) {
                                        if (message != "")
                                            message += "<br>";
                                        message += ee.responseJSON[i];
                                    }
                                    alert(message);
                                }
                            });
                            return false;
                        }
                        else {
                            tOptions.events[0].action(e, value)
                        }
                    }
                },
                {
                    text: tOptions.events[1].text,
                    primary: tOptions.events[1].primary,
                    action: function (e) {
                        tOptions.events[1].action(e, null)
                    }
                }
            ];
            if (data.url != "") {
                $.ajax({
                    url: data.url,
                    type: 'get',
                    dataType: 'html',
                    success: function (d) {
                        $('#loading').hide();
                        $("#dialog").find(".content").html(d);
                        $("#dialog").find(".content").find("input, textarea").each(function (e) {
                            $(this).attr("name", $(this).attr("asp-for"))
                        })
                        $("#dialog").kendoDialog(options);
                        $("#dialog").css({ "display": "block" })
                        var dialog = $("#dialog").data("kendoDialog");
                        dialog.open();
                    }
                });
            }
            else {
                $("#dialog").kendoDialog(options);
                $("#dialog").find(".content").html(JSON.stringify(data))
                $("#dialog").css({ "display": "block" })
                var dialog = $("#dialog").data("kendoDialog");
                dialog.open();
            }
        },
        getFormData: function ($form) {
            var unindexed_array = $form.serializeArray();
            var indexed_array = {};

            $.map(unindexed_array, function (n, i) {
                indexed_array[n['name']] = n['value'];
            });

            return indexed_array;
        }
    };
})();


function window_activate() {
    alert("activated")
}



function confirm(options) {
    $("<div id='confirm_1'></div>").appendTo($("body"));
    $("#confirm_1").kendoDialog({ content: options.text, title: options.title, width: '30%', actions: [{ text: "Yes", primary: true, action: function (e) { $("#confirm_1").remove(); options.ok(e); } }, { text: "No", primary: false, action: function (e) { $("#confirm_1").remove(); options.cancel(e); } }] })
}

function alert(text, title) {
    $("<div id='alert_1'></div>").appendTo($("body"));
    title = title || "Alert";
    $("#alert_1").kendoDialog({ content: text, title: title, width:'30%', actions: [{ text: "Ok", primary: true, action: function (e) { $("#alert_1").remove(); } }] })
}