//@Ashish Pokhrel
//2017

(function ($) {
    var defaults =
    {
        Focus: true,
        copyEvents: false,
    };
    $.fn.AddNew = function (options) {
        var config = $.extend({
            callback: function (a) { }
        }, defaults, options);
        var i = 1;
        //Value of Hidden Id
        var hiddenid = '';
        var customDatepickerid = '';
        var ThisID = $(this).attr('id');

        //Counts the last row number as starts from (0)
        var ID = $("#" + ThisID + ">tr:last").attr('data-i');
        if (ID == undefined) {
            console.error("'data-i' attribure not provided to TR.");
        }
        if ($("#" + ThisID + ">tr:last").css('display') == 'none') {
            $("#" + ThisID + ">tr:last").css('display', 'flex');

            config.callback.call(this, $("#" + ThisID + ">tr:last"));

            $("#" + ThisID + ">tr:last").find("input").each(function () {
                if ($(this).attr('type') == 'hidden') {
                }
                else {
                    $(this).val('');
                }
            });
            return;
        }

        var NewR = $("#" + ThisID + ">tr:last").clone(config.copyEvents).attr('data-i', parseInt(ID) + 1).css('display', 'flex');
        NewR.find("input").each(function () {
            $(this).attr({
                'id': function (_, id) {
                    try {

                        var ids = id.replace(ID, parseInt(ID) + 1);
                        var parentlabel = $(this).parent().find("label").attr("for", ids);
                        return id.replace(ID, parseInt(ID) + 1);
                    }
                    catch (err) {
                        console.info("ID not provoded for input element");
                    }
                },
                'name': function (_, name) {
                    try {
                        return name.replace(ID, parseInt(ID) + 1);
                    }
                    catch (err) {
                        console.info("Name not provoded for input element");
                    }
                }
            });
            //if ($(this).attr('type') == 'radio') {
            //    var id = $(this).attr('id');
            //}
            if ($(this).attr('type') == 'text') {
                $(this).attr({
                    'aria-owns': function (_, name) {
                        try { return name.replace(ID, parseInt(ID) + 1); }
                        catch (err) { }
                    }
                });
                if ($(this).hasClass('customdatepickers')) {
                    var id = $(this).attr('id');
                    customDatepickerid = id;
                }
            }
            if ($(this).attr('type') == 'hidden') {
                var id = $(this).attr('id');
                hiddenid = id;
            }
            if ($(this).attr('type') == 'radio') {
                $(this).attr({
                    'value': function (_, value) {
                        try { return value.replace(value, value); }
                        catch (err) { }
                    }
                });
            }
            else {
                $(this).val('');
            }
        });

        NewR.find("div").each(function () {
            $(this).attr({
                'id': function (_, id) {
                    try {
                        return id.replace(ID, parseInt(ID) + 1);
                    }
                    catch (err) {

                    }
                }
            });
        });
        NewR.find("tbody").each(function () {
            $(this).attr({
                'id': function (_, id) {
                    try {
                        return id.replace(ID, parseInt(ID) + 1);
                    }
                    catch (err) {

                    }
                }
            });
        });

        NewR.find("a").each(function () {
            $(this).attr({
                'data-target': function (_, id) {
                    try {
                        return id.replace(ID, parseInt(ID) + 1);
                    }
                    catch (err) {
                    }
                }
            });
        });


        NewR.find("select").each(function () {
            $(this).attr({
                'id': function (_, id) {
                    try {
                        return id.replace(ID, parseInt(ID) + 1);
                    }
                    catch (err) {
                        console.info("ID not provoded for DropDown List");
                    }
                },
                'name': function (_, name) {
                    try {
                        return name.replace(ID, parseInt(ID) + 1);
                    }
                    catch (err) {
                        console.info("Name not provoded for DropDown List");
                    }
                }
            });
            if ($(this).attr('type') == 'hidden') {
            }
            else {
                $(this).val('');
            }
        });
        NewR.find("span").each(function () {
            $(this).attr({
                'data-valmsg-for': function (_, name) {
                    try { return name.replace(ID, parseInt(ID) + 1); }
                    catch (err) { }
                },
                'aria-controls': function (_, name) {
                    try { return name.replace(ID, parseInt(ID) + 1); }
                    catch (err) { }
                }
            });
        }).end();
        NewR.appendTo(this);
        config.callback.call(this, NewR);
        i++;
        $("#" + ThisID + ">tr:last").find('[id$=_Id]').each(function () {
            $(this).val('0');
        });
        if (config.Focus == true) {
            $('#' + ThisID + '>tr:last ').each(function () {
                $($(this)).find('td:first').each(function () {
                    $(this).find('input').each(function () {
                        $(this).focus();
                    });
                });
            });
        }
        if (hiddenid.length > 0) {
            //Set the Value of Hidden Id as NUll
            $('#' + hiddenid).val(null);
        }
        if (customDatepickerid.length > 0) {
            $('#' + customDatepickerid).kendoDatePicker();
        }
    }
}(jQuery));