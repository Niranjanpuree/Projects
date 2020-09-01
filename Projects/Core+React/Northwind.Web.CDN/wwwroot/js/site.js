// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $("#ExportToCSV").on("click", function () {
        var rel = $(this).attr("rel");
        var resource = $(this).attr("data-resource");
        var grid = $(rel).data("kendoGrid")
        var hasValue = false;
        for (var i in grid._selectedIds) {
            hasValue = true;
            break;
        }
        if (hasValue == false) {
            Dialog.alert("Please select item/s first before exporting.")
        }
        else {
            ExportGrid.exportToCSVDialog(grid, Dialog, resource)
        }
        
    })

    $("#ExportToPdf").on("click", function () {
        var rel = $(this).attr("rel");
        var resource = $(this).attr("data-resource");
        var grid = $(rel).data("kendoGrid")
        var hasValue = false;
        for (var i in grid._selectedIds) {
            hasValue = true;
            break;
        }
        if (hasValue == false) {
            Dialog.alert("Please select item/s first before exporting.")
        }
        else {
            ExportGrid.exportToPDFDialog(grid, Dialog, resource)
        }
    })
});

function numberWithCommas(x)
{
    x = x.replace(/\,/g, '')
    if (!x || isNaN(x))
        x = 0;
    var amt = parseFloat(x + "").toFixed(2);
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}


function formatUSDate(x) {
    var d = new Date(x);
    if (d && d.getDate()) {
        return padNumber(d.getMonth() + 1) + "/" + padNumber(d.getDate()) + "/" + d.getFullYear();
    }
    return x;
}

function formatUSDatetime(x)
{
    var d = new Date(x);
    if (d && d.getDate()) {
        return padNumber(d.getMonth() + 1) + "/" + padNumber(d.getDate()) + "/" + d.getFullYear() + " " + padNumber(d.getHours()) + ":" + padNumber(d.getMinutes()) + ":" + padNumber(d.getSeconds());
    }
    return x;
}

function padNumber(num) {
    if (num > 9)
        return num + "";
    else
        return "0" + num;
}

function getTodayDate() {
    return formatUSDate((new Date()).getTime());
}


function getCookieValue(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');

    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ') c = c.substring(1);
        if (c.indexOf(name) === 0) return c.substring(name.length, c.length);
    }

    return "";
}


function serializeToJson(serializer) {
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

    if (obj && obj['__RequestVerificationToken']) {
        obj['__RequestVerificationToken'] = undefined;
    }

    return obj;
}

function ajaxPost(url, data, onSuccess, onError) {
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
        success: onSuccess,
        error: onError
    });
}

function isIE() {
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))  // If Internet Explorer, return version number
    {
        return true;
    }
    else  // If another browser, return 0
    {
        return false;
    }
}