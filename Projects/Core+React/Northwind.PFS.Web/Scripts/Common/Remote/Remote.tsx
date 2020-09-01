declare var $: any;
import 'whatwg-fetch'

export class Remote
{

    static post(url: string, form: any, callback: any, error: any)
    {
        let data: any;

        data = JSON.stringify(Remote.serializeToJson(form));

        fetch(url, { credentials: 'include', method: 'post', body: JSON.stringify(data), headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } })
            .then((response) =>
            {
                if (response.status == 200)
                    return response.json();
                else
                    return { error: true, statusText: response.statusText, originalMessage: response }
            })
            .catch((reason: any) => { Remote.onError(reason, error); })
            .then((result) =>
            {
                if (!result.error)
                    callback(result);
                else {
                    Remote.onError(result, error);
                }
            });

    }

    static async postAsync(url: string, form: any)
    {
        let data: any;
        data = JSON.stringify(Remote.serializeToJson(form));
        return await fetch(url, { credentials: 'same-origin', method: 'post', body: JSON.stringify(data), headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } })
    }

    static postData(url: string, data: any, callback: any, error: any)
    {
        fetch(url, { credentials: 'include', method: 'post', body: JSON.stringify(data), headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } })
            .then((response) =>
            {
                if (response.status == 200)
                    return response.json();
                else
                    return { error: true, statusText: response.statusText, originalMessage: response }
            })
            .catch((reason: any) => { Remote.onError(reason, error); })
            .then((result) =>
            {
                if (result && !result.error)
                    callback(result);
                else {
                    Remote.onError(result, error);
                }
            });
    }

    static async postDataAsync(url: string, data: any)
    {
        return fetch(url, { credentials: 'same-origin', method: 'post', body: JSON.stringify(data), headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } })
    }

    static postPlainFormData(url: string, data: any, callback: any, error: any)
    {
        fetch(url, { credentials: 'include', method: 'post', body: data })
            .then((response) =>
            {
                if (response.status == 200)
                    return response;
                else
                    return { error: true, statusText: response.statusText, originalMessage: response }
            })
            .catch((reason: any) => { Remote.onError(reason, error); })
            .then((result: any) =>
            {
                if (result.ok)
                    callback(result);
                else {
                    Remote.onError(result, error);
                }
            });
    }

    static postDataText(url: string, data: any, callback: any, error: any)
    {
        fetch(url, { credentials: 'include', method: 'post', body: JSON.stringify(data), headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } })
            .then((response: any) =>
            {
                if (response.status == 200)
                    return response.text();
                else
                    return { error: true, statusText: response.statusText, originalMessage: response }
            })
            .catch((reason: any) => { Remote.onError(reason, error); })
            .then((result) =>
            {
                if (!result.error)
                    callback(result);
                else {
                    Remote.onError(result, error);
                }
            });
    }

    static get(url: string, callback: any, error: any)
    {
        var dt = new Date().getTime();
        var url = url.indexOf("?") > 0 ? url + "&dt=" + dt : url + "?dt=" + dt;
        fetch(url, { credentials: 'same-origin', headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } })
            .then((response) =>
            {
                if (response.status == 200)
                    return response.json();
                else
                    return { error: true, statusText: response.statusText, originalMessage: response }
            })
            .catch((reason: any) => { Remote.onError(reason, error); })
            .then((result) =>
            {
                if (result && !result.error)
                    callback(result);
                else {
                    Remote.onError(result, error);
                }
            });
    }

    static async getAsync(url: string)
    {
        var dt = new Date().getTime();
        var url = url.indexOf("?") > 0 ? url + "&dt=" + dt : url + "?dt=" + dt;
        return await fetch(url, { credentials: 'same-origin', headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } });
    }

    static getText(url: string, callback: any, error: any)
    {

        fetch(url, { credentials: 'include', headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } })
            .then((response: any) =>
            {
                if (response.status == 200)
                    return response.text();
                else
                    return { error: true, statusText: response.statusText, originalMessage: response }
            })
            .catch((reason: any) => { Remote.onError(reason, error); })
            .then((result: any) =>
            {
                if (!result.error)
                    callback(result);
                else {
                    Remote.onError(result, error);
                }
            });
    }

    static serializeToJson(serializer: any)
    {

        var data = serializer.serialize().split("&");
        var obj: any = {};
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

    static onError(data: any, callback: any)
    {
        if (data.originalMessage && data.originalMessage.json) {
            data.originalMessage.json().then((e: any) =>
            {
                if (e[""] && e[""]["errors"] && e[""]["errors"][0]["errorMessage"]) {
                    callback(e[""]["errors"][0]["errorMessage"]);
                    return;
                }
                
            })
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

    static download(url: any, callback: any, error: any)
    {
        return $.fileDownload(url).done(callback).fail(error);
    }
}