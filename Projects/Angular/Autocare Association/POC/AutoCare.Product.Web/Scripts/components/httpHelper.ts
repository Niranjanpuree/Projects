
import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import 'rxjs/Rx';
import {Observable} from 'rxjs/Observable';

@Injectable()
export class HttpHelper {

    constructor(private _http: Http) { }

    getToken<T>(url: string): Observable<string> {
        return this._http.get(url)
            .map((res: Response) => {
                return res.text();
            })
            .catch(this.handleError);
    }

    get<T>(url: string): Observable<T> {
        return this._http.get(url)
            .map((res: Response) => {
                return <T>this.extractData(res);
            })
            .catch(this.handleError);
    }

    post(url: string, body: any, options?: any, isHttpVerbPut?: any) {
        options = options || null;
        isHttpVerbPut = isHttpVerbPut || false;
        let _requestOptions: any = null;
        let _isJsonRequest = false;
        let _contentTypeKey = 'Content-Type';
        let _contentTypeJson = 'application/json';
        //add content-type to application/json as default
        if (!options) {
            let headers = this.addContentTypeInHeader(_contentTypeJson);
            _requestOptions = this.getRequestOptions(headers);
            _isJsonRequest = true;
        }
        else {
            if (!options.headers) {
                let headers = this.addContentTypeInHeader(_contentTypeJson);
                options.headers = this.getRequestOptions(headers);
                _isJsonRequest = true;
            }
            _requestOptions = options;
            if (this.isJsonContentType(options)) {
                _isJsonRequest = true;
            }
        }

        // check if the body is json stringified or not
        //if the option's header is 'application/json'
        if (_isJsonRequest) {
            if (!this.isJson(body)) {
                //parse to json
                body = JSON.stringify(body);
            }
        }

        if (isHttpVerbPut) {
            return this._http.put(url, body, _requestOptions)
                .map(this.extractData)
                .catch(this.handleError);
        }
        else {
            return this._http.post(url, body, _requestOptions)
                .map(this.extractData)
                .catch(this.handleError);
        }
    }

    put(url: string, body: any, options?: any) {
        options = options || null
        let _requestOptions: any = null;
        let _isJsonRequest = false;
        let _contentTypeKey = 'Content-Type';
        let _contentTypeJson = 'application/json';
        //add content-type to application/json as default
        if (!options) {
            let headers = this.addContentTypeInHeader(_contentTypeJson);
            _requestOptions = this.getRequestOptions(headers);
            _isJsonRequest = true;
        }
        else {
            if (!options.headers) {
                let headers = this.addContentTypeInHeader(_contentTypeJson);
                options.headers = this.getRequestOptions(headers);
                _isJsonRequest = true;
            }
            _requestOptions = options;
            if (this.isJsonContentType(options)) {
                _isJsonRequest = true;
            }
        }

        // check if the body is json stringified or not
        //if the option's header is 'application/json'
        if (_isJsonRequest) {
            if (!this.isJson(body)) {
                //parse to json
                body = JSON.stringify(body);
            }
        }

        return this._http.put(url, body, _requestOptions)
            .map(this.extractData)
            .catch(this.handleError);
    }
    //put(url, data) {
    //    return this._http.put(url, data)
    //        .map(this.extractData)
    //        .catch(this.handleError);
    //}

    private addContentTypeInHeader(value: any) {
        return new Headers({ 'Content-Type': value });
    }

    private isJsonContentType(options: any) {
        if (options.headers.get('Content-Type') == 'application/json') {
            return true;
        }
        else {
            return false;
        }
    }

    private getRequestOptions(headers: any) {
        return new RequestOptions({
            headers: headers
        });
    }

    private extractData(res: Response) {
        if (res.status < 200 || res.status >= 300) {
            throw new Error('Bad response status: ' + res.status);
        }

        if (res.status == 401) {//Unauthorized
            throw Observable.throw('You are not authorized. status: ' + res.status);
        }

        let body = res.json();
        return body || {};
    }

    private handleError(error: any) {
        let errorMessage = error._body || 'Server error';
        return Observable.throw(errorMessage);
    }

    private isJson(data: any) {
        try {
            JSON.parse(data);
            return true;
        }
        catch (e) {
            return false;
        }
    }

    delete(url: string): Observable<Response> {
        return this._http.delete(url)
            .map((res: Response) => this.extractData(res))
            .catch(this.handleError);
    }

}