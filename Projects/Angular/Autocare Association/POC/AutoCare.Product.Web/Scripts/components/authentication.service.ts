import { Injectable } from '@angular/core';
import { ConstantsWarehouse } from './constants-warehouse';
import {HttpHelper} from './httpHelper';
import { JwtHelper } from './jwtHelper';
import { RuntimeCompiler} from '@angular/compiler';

@Injectable()
export class AuthenticationService {
    private _jwtHelper = new JwtHelper();

    constructor(private _httpHelper: HttpHelper, private _runtimeCompiler: RuntimeCompiler) {
        this._jwtHelper = new JwtHelper();
        this._runtimeCompiler.clearCache();
    }

    // store the URL so we can redirect after logging in
    redirectUrl: string;


    getToken() {
        //jQuery.ajax({
        //    url: 'http://localhost:60797/account/gettoken',
        //    success: function (result) {
        //        debugger;
        //        if (result.isOk == false) alert(result.message);
        //    },
        //    async: false
        //});
        return this._httpHelper.getToken<string>('account/gettoken');
    }

    login() {
        location.href = 'account/login?ReturnUrl=' + window.btoa(this.redirectUrl);
    }

    logout() {
        sessionStorage.removeItem('jwtToken');
        //this._httpHelper.post('/Account/LogOff', null);
        //location.href = 'account/logoff?ReturnUrl=' + window.btoa('account/login');
    }

    isLoggedIn() {
        let token = sessionStorage.getItem('jwtToken');
        if (!token) {
            return false;
        }

        if (this._jwtHelper.isTokenExpired(token)) {
            sessionStorage.removeItem('jwtToken');
            return false;
        }

        return true;
    }
}
