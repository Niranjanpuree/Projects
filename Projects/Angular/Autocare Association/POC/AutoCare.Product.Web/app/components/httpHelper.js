"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var http_1 = require('@angular/http');
require('rxjs/Rx');
var Observable_1 = require('rxjs/Observable');
var HttpHelper = (function () {
    function HttpHelper(_http) {
        this._http = _http;
    }
    HttpHelper.prototype.getToken = function (url) {
        return this._http.get(url)
            .map(function (res) {
            return res.text();
        })
            .catch(this.handleError);
    };
    HttpHelper.prototype.get = function (url) {
        var _this = this;
        return this._http.get(url)
            .map(function (res) {
            return _this.extractData(res);
        })
            .catch(this.handleError);
    };
    HttpHelper.prototype.post = function (url, body, options, isHttpVerbPut) {
        options = options || null;
        isHttpVerbPut = isHttpVerbPut || false;
        var _requestOptions = null;
        var _isJsonRequest = false;
        var _contentTypeKey = 'Content-Type';
        var _contentTypeJson = 'application/json';
        //add content-type to application/json as default
        if (!options) {
            var headers = this.addContentTypeInHeader(_contentTypeJson);
            _requestOptions = this.getRequestOptions(headers);
            _isJsonRequest = true;
        }
        else {
            if (!options.headers) {
                var headers = this.addContentTypeInHeader(_contentTypeJson);
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
    };
    HttpHelper.prototype.put = function (url, body, options) {
        options = options || null;
        var _requestOptions = null;
        var _isJsonRequest = false;
        var _contentTypeKey = 'Content-Type';
        var _contentTypeJson = 'application/json';
        //add content-type to application/json as default
        if (!options) {
            var headers = this.addContentTypeInHeader(_contentTypeJson);
            _requestOptions = this.getRequestOptions(headers);
            _isJsonRequest = true;
        }
        else {
            if (!options.headers) {
                var headers = this.addContentTypeInHeader(_contentTypeJson);
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
    };
    //put(url, data) {
    //    return this._http.put(url, data)
    //        .map(this.extractData)
    //        .catch(this.handleError);
    //}
    HttpHelper.prototype.addContentTypeInHeader = function (value) {
        return new http_1.Headers({ 'Content-Type': value });
    };
    HttpHelper.prototype.isJsonContentType = function (options) {
        if (options.headers.get('Content-Type') == 'application/json') {
            return true;
        }
        else {
            return false;
        }
    };
    HttpHelper.prototype.getRequestOptions = function (headers) {
        return new http_1.RequestOptions({
            headers: headers
        });
    };
    HttpHelper.prototype.extractData = function (res) {
        if (res.status < 200 || res.status >= 300) {
            throw new Error('Bad response status: ' + res.status);
        }
        if (res.status == 401) {
            throw Observable_1.Observable.throw('You are not authorized. status: ' + res.status);
        }
        var body = res.json();
        return body || {};
    };
    HttpHelper.prototype.handleError = function (error) {
        var errorMessage = error._body || 'Server error';
        return Observable_1.Observable.throw(errorMessage);
    };
    HttpHelper.prototype.isJson = function (data) {
        try {
            JSON.parse(data);
            return true;
        }
        catch (e) {
            return false;
        }
    };
    HttpHelper.prototype.delete = function (url) {
        var _this = this;
        return this._http.delete(url)
            .map(function (res) { return _this.extractData(res); })
            .catch(this.handleError);
    };
    HttpHelper = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http])
    ], HttpHelper);
    return HttpHelper;
}());
exports.HttpHelper = HttpHelper;
