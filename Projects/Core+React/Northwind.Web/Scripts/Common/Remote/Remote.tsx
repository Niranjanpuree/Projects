declare var $: any;
import 'whatwg-fetch'



export class Remote
{
    static post(url: string, form: any, callback: any, error: any)
    {
        let data: any;
        let token = Remote.getCookieValue('X-CSRF-TOKEN');
        let reqValToken = Remote.getCookieValue('RequestVerificationToken');
        data = JSON.stringify(Remote.serializeToJson(form));

        fetch(url, {
            credentials: 'same-origin', method: 'post', body: JSON.stringify(data), headers: {
                'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache', 'X-CSRF-TOKEN': token, 'RequestVerificationToken': reqValToken  } })
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
        let token = Remote.getCookieValue('X-CSRF-TOKEN');
        let reqValToken = Remote.getCookieValue('RequestVerificationToken');
        data = JSON.stringify(Remote.serializeToJson(form));
        return await fetch(url, {
            credentials: 'same-origin', method: 'post', body: JSON.stringify(data), headers: {
                'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache', 'X-CSRF-TOKEN': token, 'RequestVerificationToken': reqValToken  } })            
    }

    static postData(url: string, data: any, callback: any, error: any)
    {
        let token = Remote.getCookieValue('X-CSRF-TOKEN');
        let reqValToken = Remote.getCookieValue('RequestVerificationToken');
        fetch(url, {
            credentials: 'same-origin', method: 'post', body: JSON.stringify(data), headers: {
                'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache', 'X-CSRF-TOKEN': token, 'RequestVerificationToken': reqValToken } })
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
        let token = Remote.getCookieValue('X-CSRF-TOKEN');
        let reqValToken = Remote.getCookieValue('RequestVerificationToken');
        return fetch(url, {
            credentials: 'same-origin', method: 'post', body: JSON.stringify(data), headers: {
                'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache', 'X-CSRF-TOKEN': token, 'RequestVerificationToken': reqValToken } })
    }

    static postPlainFormData(url: string, data: any, callback: any, error: any)
    {
        let token = Remote.getCookieValue('X-CSRF-TOKEN');
        let reqValToken = Remote.getCookieValue('RequestVerificationToken');
        fetch(url, {
            credentials: 'same-origin', method: 'post', body: data, headers: {
                'X-CSRF-TOKEN': token, 'RequestVerificationToken': reqValToken } })
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

    static postPlainFormDataAsync(url: string, data: any) {
        let token = Remote.getCookieValue('X-CSRF-TOKEN');
        let reqValToken = Remote.getCookieValue('RequestVerificationToken');
        return fetch(url, {
            credentials: 'same-origin', method: 'post', body: data, headers: {
                'X-CSRF-TOKEN': token, 'RequestVerificationToken': reqValToken
            }
        })
    }

    static postDataText(url: string, data: any, callback: any, error: any)
    {
        let token = Remote.getCookieValue('X-CSRF-TOKEN');
        let reqValToken = Remote.getCookieValue('RequestVerificationToken');
        fetch(url, {
            credentials: 'same-origin', method: 'post', body: JSON.stringify(data), headers: {
                'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache', 'X-CSRF-TOKEN': token, 'RequestVerificationToken': reqValToken } })
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
                    throw response;
                    //return { error: true, statusText: response.statusText, originalMessage: response }
            })
            .catch((reason: any) => { Remote.onError(reason, error); })
            .then((result) =>
            {
                if (result && !result.error && !result.message)
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

        fetch(url, { credentials: 'same-origin', headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } })
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

    static async getTextAsync(url: string) {
        return await fetch(url, { credentials: 'same-origin', headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Cache-Control': 'no-cache' } })
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

        if (data && data.message && message === "") {
            message = data.message;
        }

        if (message == "" && data)
            message = data.statusText;

        callback(message);
    }

    static download(url: any, callback: any, error: any)
    {
        return $.fileDownload(url).done(callback).fail(error);
    }

    static async parseErrorMessage(a: any) {
        try {
            let m = await a.json();
            if (m[""]) {
                return m[""]["errors"][0]["errorMessage"];
            }
            else {
                return m["Message"];
            }

        } catch (e) {
            return a.statusText;
        }
    }

    static getCookieValue(cname: string) {
        var name = cname + "=";
        var ca = document.cookie.split(';');

        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1);
            if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
        }

        return "";
    }
}

