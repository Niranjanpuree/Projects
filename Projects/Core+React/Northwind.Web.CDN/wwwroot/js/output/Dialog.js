var Dialog = /** @class */ (function () {
    function Dialog() {
    }
    Dialog.open = function (data, options) {
        var tOptions = options;
        tOptions.width = tOptions.width || '50%';
        tOptions.height = tOptions.height || '50%';

        $('html').addClass('htmlClass');

        tOptions.closable = tOptions.closable || true;
        tOptions.actions = [];
        tOptions.events.map(function (item, index) {
            tOptions.actions.push({
                text: item.text,
                primary: item.primary,
                action: function (e) {
                    if (item.validationRequired || item.primary) {
                        if (!$('#dialog').find("form").valid()) {
                            return false;
                        }
                        var $form = $('#dialog').find("form").eq(0);
                        var value = Dialog.getFormData($form);
                        item.action(e, value);
                    }
                    else {
                        item.action(e, null);
                    }
                }
            })
        });
        $("#dialog").kendoDialog(options);
        $("#dialog").find(".content").html(data);
        $("#dialog").css({ "display": "block" });
        var dialog = $("#dialog").data("kendoDialog");
        dialog.open().center();

    };
    Dialog.openDialog = function (data, options) {
        $('html').addClass('htmlClass');
        var tOptions = options;
        tOptions.width = tOptions.width || '70%';
        tOptions.height = tOptions.height || '50%';
        tOptions.closable = tOptions.closable || true;
        tOptions.actions = [];
        tOptions.events.map(function (item, data) {
            tOptions.actions.push({
                text: item.text,
                primary: item.primary,
                action: function (e) {
                    if ((item.requireValidation || item.primary) && (item.requireValidation !== false)) {
                        if (!$('#dialog').find("form").valid()) {
                            return false;
                        }
                        var $form = $('#dialog').find("form").eq(0);
                        var value = Dialog.getFormData($form);
                        item.action(e, value);
                    }
                    else {
                        item.action(e, null);
                    }
                }
            })
        })
        if (data.url != "") {
            $.ajax({
                beforeSend: function (xhr) {
                    xhr.withCredentials = true;
                },
                url: data.url,
                type: 'get',
                dataType: 'html',
                success: function (d) {
                    $('#loading').hide();
                    $("#dialog").find(".content").html(d);
                    $("#dialog").find("form").attr("onsubmit", "return false");
                    $("#dialog").find("[data-val-required]").each(function (e) {
                        $(this).attr("required='required'");
                    });
                    $("#dialog").kendoDialog(options);
                    $("#dialog").css({ "display": "block" });
                    var dialog = $("#dialog").data("kendoDialog");

                    $(".hideDivIfValueNotFound").each(function (e) {
                        if (!this.id) {
                            $(this).hide();
                        }
                    });
                    dialog.open().center();
                }
            });
        }
        else {
            $("#dialog").kendoDialog(options);
            $("#dialog").find(".content").html(JSON.stringify(data));
            $("#dialog").css({ "display": "block" });
            var dialog = $("#dialog").data("kendoDialog");
            dialog.open().center();
        }
    };
    Dialog.openDialogSubmit = function (data, options) {
        $('html').addClass('htmlClass');
        $('#loading').show();
        var tOptions = options;
        tOptions.width = tOptions.width || '70%';
        tOptions.height = tOptions.height || '50%';
        tOptions.closable = tOptions.closable || true;
        tOptions.actions = [];
        tOptions.events.map(function (item, index) {
            tOptions.actions.push({
                text: item.text,
                primary: item.primary,
                action: function (e) {
                    if ((item.requireValidation || item.primary) && (item.requireValidation !== false)) {
                        if (!$('#dialog').find("form").valid()) {
                            $('#dialog').find("form").find("input, textarea").each(function (e) {
                                var id = $(this).attr("name");
                                $("#" + id + "_error").html($(this).attr("data-val-required"));
                            });
                            return false;
                        }
                        var $form = $('#dialog').find("form").eq(0);
                        var encType = $form.attr("enctype");
                        var value = Dialog.getFormData($form);
                        if (data.submitURL) {
                            var token = getCookieValue('X-CSRF-TOKEN');
                            var reqValToken = getCookieValue('RequestVerificationToken');
                            $('#loading').show();
                            $.ajax({
                                headers: {
                                    'Accept': 'application/json',
                                    'Content-Type': 'application/json',
                                    'X-CSRF-TOKEN': token,
                                    'RequestVerificationToken': reqValToken
                                },
                                beforeSend: function (xhr) {
                                    xhr.withCredentials = true;
                                },
                                type: 'post',
                                url: data.submitURL,
                                dataType: 'json',
                                data: JSON.stringify(value),
                                success: function (ee) {
                                    if (item.action) {
                                        item.action(e, ee);
                                    }
                                    var dialog = $("#dialog").data("kendoDialog");
                                    $('#loading').hide();
                                    $('html').removeClass('htmlClass');
                                    dialog.close();
                                },
                                error: function (ee) {
                                    var message = "";
                                    for (var i in ee.responseJSON) {
                                        if (message !== "")
                                            message += "<br>";
                                        message += ee.responseJSON[i];
                                    }
                                    $('#loading').hide();
                                    Dialog.alert(message);
                                }
                            });
                            return false;
                        }
                        else {
                            item.action(e, value);
                        }
                    }
                    else {
                        item.action(e, null);
                    }
                }
            });
        });

        if (data.url !== "") {
            $.ajax({
                beforeSend: function (xhr) {
                    xhr.withCredentials = true;
                },
                url: data.url,
                type: 'get',
                dataType: 'html',
                success: function (d) {
                    $('#loading').hide();
                    $("#dialog").find(".content").html(d);
                    $("#dialog").find("form").attr("onsubmit", "return false");
                    $("#dialog").find("[data-val-required]").each(function (e) {
                        $(this).attr("required", "required");
                    });
                    $("#dialog").kendoDialog(options);
                    $("#dialog").css({ "display": "block" });
                    var dialog = $("#dialog").data("kendoDialog");

                    $(".hideDivIfValueNotFound").each(function (e) {
                        if (!this.id) {
                            $(this).hide();
                        }
                    });
                    $('#loading').hide();
                    dialog.open().center();
                }
            });
        }
        else {
            $("#dialog").kendoDialog(options);
            $("#dialog").find(".content").html(JSON.stringify(data));
            $("#dialog").css({ "display": "block" });
            var dialog = $("#dialog").data("kendoDialog");
            $('#loading').hide();
            dialog.open().center();
        }
    };
    Dialog.openDialogGridSubmit = function (data, options) {
        $('html').addClass('htmlClass');
        $('#loading').show();
        var tOptions = options;
        tOptions.width = tOptions.width || '75%';
        tOptions.height = tOptions.height || '75%';
        tOptions.closable = tOptions.closable || true;
        tOptions.actions = [];
        tOptions.events.map(function (item, index) {
            tOptions.actions.push({
                text: item.text,
                primary: item.primary,
                action: function (e) {
                    if (item.validationRequired || item.primary) {
                        if (!$('#dialog').find("form").valid()) {
                            $('#dialog').find("form").find("input, textarea").each(function (e) {
                                var id = $(this).attr("name");
                                $("#" + id + "_error").html($(this).attr("data-val-required"));
                            });
                            return false;
                        }
                        var $form = $('#dialog').find("form").eq(0);
                        var encType = $form.attr("enctype");
                        var value = Dialog.getFormData($form);
                        var formData = new FormData();
                        var isFilePost = false;
                        if (encType && encType == "multipart/form-data") {
                            for (i in value) {
                                formData.append(i, value[i]);
                            }
                            var files = $($($form).find('input[type=file]')[0]).prop("files");

                            for (var i = 0; i < files.length; i++) {
                                formData.append($($form).find('input[type=file]')[0].name, files[i]);
                            }
                            isFilePost = true;
                        }

                        if (data.submitURL) {
                            $('#loading').show();
                            var token = getCookieValue('X-CSRF-TOKEN');
                            var reqValToken = getCookieValue('RequestVerificationToken');
                            if (isFilePost) {

                                var pbar = $('#progressBar');

                                $(pbar).removeClass('progressBarInactive');

                                $(pbar).width(0).addClass('progressBarActive');

                                var currentProgress = 0;
                                function trackUploadProgress(e) {
                                    if (e.lengthComputable) {
                                        currentProgress = (e.loaded / e.total) * 100; // uploaded in percent
                                        $(pbar).width(currentProgress + '%');

                                        if (currentProgress == 100)
                                            console.log('Progress : 100%');
                                    }
                                }

                                $.ajax({
                                    headers: {
                                        'X-CSRF-TOKEN': token,
                                        'RequestVerificationToken': reqValToken
                                    },
                                    beforeSend: function (xhr) {
                                        xhr.withCredentials = true;
                                    },
                                    contentType: false,
                                    processData: false,
                                    type: 'post',
                                    url: data.submitURL,
                                    data: formData,
                                    success: function (ee) {
                                        if (item.action) {
                                            item.action(e, ee);
                                        }
                                        var dialog = $("#dialog").data("kendoDialog");
                                        $('#loading').hide();
                                        $('html').removeClass('htmlClass');
                                        dialog.close();
                                    },
                                    xhr: function () {
                                        // Custom XMLHttpRequest
                                        var appXhr = $.ajaxSettings.xhr();

                                        // Check if upload property exists, if "yes" then upload progress can be tracked otherwise "not"
                                        if (appXhr.upload) {
                                            // Attach a function to handle the progress of the upload
                                            appXhr.upload.addEventListener('progress', trackUploadProgress, false);
                                        }
                                        return appXhr;
                                    },
                                    error: function (ee) {
                                        var message = "";

                                        var resultMessage = ee || "";
                                        if (resultMessage == '')
                                            message += 'File size might be too high..';

                                        for (var i in ee.responseJSON) {
                                            if (message != "")
                                                message += "<br>";
                                            message += ee.responseJSON[i];
                                        }
                                        $(pbar).width(0).removeClass('progressBarActive');
                                        $('#loading').hide();
                                        Dialog.alert(message);
                                    }
                                });
                            }
                            else {
                                $.ajax({
                                    beforeSend: function (xhr) {
                                        xhr.withCredentials = true;
                                    },
                                    headers: {
                                        'Accept': 'application/json',
                                        'Content-Type': 'application/json',
                                        'X-CSRF-TOKEN': token,
                                        'RequestVerificationToken': reqValToken
                                    },
                                    type: 'post',
                                    url: data.submitURL,
                                    dataType: 'json',
                                    data: JSON.stringify(value),
                                    success: function (ee) {
                                        if (item.action) {
                                            item.action(e, ee);
                                        }
                                        var dialog = $("#dialog").data("kendoDialog");
                                        $('#loading').hide();
                                        $('html').removeClass('htmlClass');
                                        dialog.close();
                                    },
                                    error: function (ee) {
                                        var message = "";
                                        for (var i in ee.responseJSON) {
                                            if (message != "")
                                                message += "<br>";
                                            message += ee.responseJSON[i];
                                        }
                                        $('#loading').hide();
                                        Dialog.alert(message);
                                    }
                                });
                            }

                            return false;
                        }
                        else {
                            item.action(e, value);
                        }
                    }
                    else {
                        item.action(e, null);
                    }
                }
            });
        });

        if (data.url != "") {
            $.ajax({
                beforeSend: function (xhr) {
                    xhr.withCredentials = true;
                },
                url: data.url,
                type: 'get',
                dataType: 'html',
                success: function (d) {
                    $('#loading').hide();
                    $("#dialog").find(".content").html(d);
                    $("#dialog").find("form").attr("onsubmit", "return false");
                    $("#dialog").find("[data-val-required]").each(function (e) {
                        $(this).attr("required", "required");
                    });
                    $("#dialog").kendoDialog(options);
                    $("#dialog").css({ "display": "block" });
                    var dialog = $("#dialog").data("kendoDialog");

                    $(".hideDivIfValueNotFound").each(function (e) {
                        if (!this.id) {
                            $(this).hide();
                        }
                    });
                    $('#loading').hide();
                    dialog.open().center();
                },
                error: function (ee) {
                    var message = "";
                    message = "Something Went Wrong !!";
                    $('#loading').hide();
                    Dialog.alert(message);
                }
            });
        }
        else {
            $("#dialog").kendoDialog(options);
            $("#dialog").find(".content").html(JSON.stringify(data));
            $("#dialog").css({ "display": "block" });
            var dialog = $("#dialog").data("kendoDialog");
            $('#loading').hide();
            dialog.open().center();
        }
    };

    Dialog.openDialogNestedListSubmit = function (data, options) {
        $('#loading').show();
        $('html').addClass('htmlClass');
        var tOptions = options;
        tOptions.width = tOptions.width || '75%';
        tOptions.height = tOptions.height || '75%';
        tOptions.closable = tOptions.closable || true;
        tOptions.actions = [];
        tOptions.events.map(function (item, index) {
            tOptions.actions.push({
                text: item.text,
                primary: item.primary,
                action: function (e) {
                    if (item.validationRequired || item.primary) {

                        if (!$('#dialog').find("form").valid()) {
                            $('#dialog').find("form").find("input, textarea").each(function (e) {
                                var id = $(this).attr("name");
                                $("#" + id + "_error").html($(this).attr("data-val-required"));
                            });
                            return false;
                        }

                        var $form = $('#dialog').find("form").eq(0);
                        var serializedFormData = $form.serialize();

                        var fileForm = new FormData();

                        var formData = serializedFormData.split("&");
                        for (var i in formData) {
                            var decodedModel = (decodeURIComponent(formData[i]).replace(/%5B/g, '[').replace(/%5D/g, ']'));
                            var keyVal = decodedModel.split("=")
                            if (keyVal.length > 1) {
                                fileForm.append(keyVal[0], decodeURIComponent(keyVal[1]));
                            }
                        }
                        var token = getCookieValue('X-CSRF-TOKEN');
                        var reqValToken = getCookieValue('RequestVerificationToken');

                        if (data.submitURL) {
                            var token = getCookieValue('X-CSRF-TOKEN');
                            var reqValToken = getCookieValue('RequestVerificationToken');
                            $('#loading').show();
                            $.ajax({
                                headers: {
                                    'X-CSRF-TOKEN': token,
                                    'RequestVerificationToken': reqValToken
                                },
                                type: 'post',
                                contentType: false,
                                processData: false,
                                url: data.submitURL,
                                data: fileForm,
                                beforeSend: function (xhr) {
                                    xhr.withCredentials = true;
                                    $('#loading').show();
                                },
                                success: function (ee) {
                                    if (item.action) {
                                        item.action(e, ee);
                                        $('html').removeClass('htmlClass');
                                    }
                                    var dialog = $("#dialog").data("kendoDialog");
                                    $('#loading').hide();
                                    dialog.close();
                                },
                                error: function (ee) {
                                    var message = "";
                                    for (var i in ee.responseJSON) {
                                        if (message != "")
                                            message += "<br>";
                                        message += ee.responseJSON[i];
                                    }
                                    $('#loading').hide();
                                    Dialog.alert(message);
                                }
                            });
                            return false;
                        }
                        else {
                            item.action(e, value);
                        }
                    }
                    else {
                        item.action(e, null);
                    }
                }
            });
        });

        if (data.url != "") {
            $.ajax({
                beforeSend: function (xhr) {
                    xhr.withCredentials = true;
                },
                url: data.url,
                type: 'get',
                dataType: 'html',
                success: function (d) {
                    $('#loading').hide();
                    $("#dialog").find(".content").html(d);
                    $("#dialog").find("form").attr("onsubmit", "return false");
                    $("#dialog").find("[data-val-required]").each(function (e) {
                        $(this).attr("required", "required");
                    });
                    $("#dialog").kendoDialog(options);
                    $("#dialog").css({ "display": "block" });
                    var dialog = $("#dialog").data("kendoDialog");

                    $(".hideDivIfValueNotFound").each(function (e) {
                        if (!this.id) {
                            $(this).hide();
                        }
                    });
                    $('#loading').hide();
                    dialog.open().center();
                },
                error: function (ee) {
                    var message = "";
                    message = "Something Went Wrong !!";
                    $('#loading').hide();
                    Dialog.alert(message);
                }
            });
        }
        else {
            $("#dialog").kendoDialog(options);
            $("#dialog").find(".content").html(JSON.stringify(data));
            $("#dialog").css({ "display": "block" });
            var dialog = $("#dialog").data("kendoDialog");
            $('#loading').hide();
            dialog.open().center();
        }
    };
    Dialog.getFormData = function (serializer) {
        var data = serializer.serialize().split("&");
        var obj = {};
        for (var key in data) {
            if (obj[data[key].split("=")[0]]) {
                if (typeof (obj[data[key].split("=")[0]]) == "string") {
                    let val = obj[data[key].split("=")[0]];
                    obj[data[key].split("=")[0]] = [];
                    obj[data[key].split("=")[0]].push(val);
                }
                obj[data[key].split("=")[0]].push(decodeURIComponent(data[key].split("=")[1]));
            }
            else {
                obj[data[key].split("=")[0]] = decodeURIComponent(data[key].split("=")[1]);
            }
        }
        return obj;
    };
    Dialog.confirm = function (options) {
        $("<div id='confirm_1'></div>").appendTo($("body"));
        $("#confirm_1").kendoDialog({
            content: options.text, title: options.title, width: '30%', actions: [{
                text: "Yes", primary: true, action: function (e) {
                    $("#confirm_1").remove(); options.ok(e, options.sender || null);
                }
            }, { text: "No", primary: false, action: function (e) { $("#confirm_1").remove(); options.cancel(e); } }]
        });
    };
    Dialog.alert = function (text, title, callback) {
        if ($.trim(text) === "" || !text)
            return;
        if (title === void 0) { title = ""; }
        $("<div id='alert_1'></div>").appendTo($("body"));
        title = title || "Alert";
        $("#alert_1").kendoDialog({
            content: text, title: title, width: '30%', actions: [{
                text: "Ok", primary: true, action: function (e) {
                    $("#alert_1").remove(); if (callback) { callback() }
                }
            }]
        });
    };
    return Dialog;
}());

var Dialog1 = /** @class */ (function () {
    function Dialog1() {
    }
    Dialog1.openDialog = function (data, options) {
        $('html').addClass('htmlClass');
        var tOptions = options;
        tOptions.width = tOptions.width || '70%';
        tOptions.height = tOptions.height || '50%';
        tOptions.closable = tOptions.closable || true;
        tOptions.actions = [];
        tOptions.events.map(function (item, data) {
            tOptions.actions.push({
                text: item.text,
                primary: item.primary,
                action: function (e) {
                    if ((item.requireValidation || item.primary) && (item.requireValidation !== false)) {
                        if (!$('#dialog1').find("form").valid()) {
                            return false;
                        }
                        var $form = $('#dialog1').find("form").eq(0);
                        var value = Dialog.getFormData($form);
                        item.action(e, value);
                    }
                    else {
                        item.action(e, null);
                    }
                    $('html').removeClass('htmlClass');
                }
            })
        })
        if (data.url != "") {
            $.ajax({
                beforeSend: function (xhr) {
                    xhr.withCredentials = true;
                },
                url: data.url,
                type: 'get',
                dataType: 'html',
                success: function (d) {
                    $('#loading').hide();
                    $("#dialog1").find(".content").html(d);
                    $("#dialog1").find("form").attr("onsubmit", "return false");
                    $("#dialog1").find("[data-val-required]").each(function (e) {
                        $(this).attr("required='required'");
                    });
                    $("#dialog1").kendoDialog(options);
                    $("#dialog1").css({ "display": "block" });
                    var dialog1 = $("#dialog1").data("kendoDialog");

                    $(".hideDivIfValueNotFound").each(function (e) {
                        if (!this.id) {
                            $(this).hide();
                        }
                    });
                    $('html').removeClass('htmlClass');
                    dialog1.open().center();
                }
            });
        }
        else {
            $("#dialog1").kendoDialog(options);
            $("#dialog1").find(".content").html(JSON.stringify(data));
            $("#dialog1").css({ "display": "block" });
            var dialog1 = $("#dialog1").data("kendoDialog");
            dialog1.open().center();
        }
    };

    return Dialog1;
}());