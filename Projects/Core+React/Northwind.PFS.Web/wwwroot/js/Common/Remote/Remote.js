"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
require("whatwg-fetch");
class Remote {
    static post(url, form, callback, error) {
        let data;
        data = JSON.stringify(Remote.serializeToJson(form));
        fetch(url, { credentials: 'include', method: 'post', body: JSON.stringify(data), headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } })
            .then((response) => {
            if (response.status == 200)
                return response.json();
            else
                return { error: true, statusText: response.statusText, originalMessage: response };
        })
            .catch((reason) => { Remote.onError(reason, error); })
            .then((result) => {
            if (!result.error)
                callback(result);
            else {
                Remote.onError(result, error);
            }
        });
    }
    static postAsync(url, form) {
        return __awaiter(this, void 0, void 0, function* () {
            let data;
            data = JSON.stringify(Remote.serializeToJson(form));
            return yield fetch(url, { credentials: 'same-origin', method: 'post', body: JSON.stringify(data), headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } });
        });
    }
    static postData(url, data, callback, error) {
        fetch(url, { credentials: 'include', method: 'post', body: JSON.stringify(data), headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } })
            .then((response) => {
            if (response.status == 200)
                return response.json();
            else
                return { error: true, statusText: response.statusText, originalMessage: response };
        })
            .catch((reason) => { Remote.onError(reason, error); })
            .then((result) => {
            if (result && !result.error)
                callback(result);
            else {
                Remote.onError(result, error);
            }
        });
    }
    static postDataAsync(url, data) {
        return __awaiter(this, void 0, void 0, function* () {
            return fetch(url, { credentials: 'same-origin', method: 'post', body: JSON.stringify(data), headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } });
        });
    }
    static postPlainFormData(url, data, callback, error) {
        fetch(url, { credentials: 'include', method: 'post', body: data })
            .then((response) => {
            if (response.status == 200)
                return response;
            else
                return { error: true, statusText: response.statusText, originalMessage: response };
        })
            .catch((reason) => { Remote.onError(reason, error); })
            .then((result) => {
            if (result.ok)
                callback(result);
            else {
                Remote.onError(result, error);
            }
        });
    }
    static postDataText(url, data, callback, error) {
        fetch(url, { credentials: 'include', method: 'post', body: JSON.stringify(data), headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } })
            .then((response) => {
            if (response.status == 200)
                return response.text();
            else
                return { error: true, statusText: response.statusText, originalMessage: response };
        })
            .catch((reason) => { Remote.onError(reason, error); })
            .then((result) => {
            if (!result.error)
                callback(result);
            else {
                Remote.onError(result, error);
            }
        });
    }
    static get(url, callback, error) {
        var dt = new Date().getTime();
        var url = url.indexOf("?") > 0 ? url + "&dt=" + dt : url + "?dt=" + dt;
        fetch(url, { credentials: 'same-origin', headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } })
            .then((response) => {
            if (response.status == 200)
                return response.json();
            else
                return { error: true, statusText: response.statusText, originalMessage: response };
        })
            .catch((reason) => { Remote.onError(reason, error); })
            .then((result) => {
            if (result && !result.error)
                callback(result);
            else {
                Remote.onError(result, error);
            }
        });
    }
    static getAsync(url) {
        var url;
        return __awaiter(this, void 0, void 0, function* () {
            var dt = new Date().getTime();
            url = url.indexOf("?") > 0 ? url + "&dt=" + dt : url + "?dt=" + dt;
            return yield fetch(url, { credentials: 'same-origin', headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } });
        });
    }
    static getText(url, callback, error) {
        fetch(url, { credentials: 'include', headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } })
            .then((response) => {
            if (response.status == 200)
                return response.text();
            else
                return { error: true, statusText: response.statusText, originalMessage: response };
        })
            .catch((reason) => { Remote.onError(reason, error); })
            .then((result) => {
            if (!result.error)
                callback(result);
            else {
                Remote.onError(result, error);
            }
        });
    }
    static serializeToJson(serializer) {
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
    }
    static onError(data, callback) {
        if (data.originalMessage && data.originalMessage.json) {
            data.originalMessage.json().then((e) => {
                if (e[""] && e[""]["errors"] && e[""]["errors"][0]["errorMessage"]) {
                    callback(e[""]["errors"][0]["errorMessage"]);
                    return;
                }
            });
        }
        let message = "";
        if (data && data.responseJSON && data.responseJSON.Message) {
            message += data.responseJSON.Message + "<br/>";
        }
        else if (data) {
            for (let i in data.responseJSON) {
                if (data.responseJSON[i] != undefined && data.responseJSON[i].errors != undefined && data.responseJSON[i].errors.length > 0) {
                    message += data.responseJSON[i].errors[0].errorMessage + "<br/>";
                }
            }
        }
        if (message == "" && data)
            message = data.statusText;
        callback(message);
    }
    static download(url, callback, error) {
        return $.fileDownload(url).done(callback).fail(error);
    }
}
exports.Remote = Remote;
//# sourceMappingURL=Remote.js.map