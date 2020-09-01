import { Injectable }     from '@angular/core';
import { CanActivate, Router,
    ActivatedRouteSnapshot,
    RouterStateSnapshot }    from '@angular/router';
import { AuthenticationService } from './authentication.service';
import { JwtHelper } from './jwtHelper';
import { SharedService } from "./shared/shared.service";
import {ITokenModel} from "./shared/shared.model";

@Injectable()
export class AuthorizeService implements CanActivate {
    private authenticationService: AuthenticationService;
    private router: Router;
    private _jwtHelper: JwtHelper;// = new JwtHelper();
    private _sharedService: SharedService;
    
    private token: string = null;

    constructor(authenticationService: AuthenticationService, sharedService: SharedService,
        router: Router) {
        this.authenticationService = authenticationService;
        this.router = router;
        this._jwtHelper = new JwtHelper();
        this._sharedService = sharedService;
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (this.authenticationService.isLoggedIn()) { return true; }
        this.authenticationService.getToken().subscribe(tokenValue => {
            this.token = tokenValue;
            if (this.token) {
                try {
                    if (this._jwtHelper.decodeToken(this.token) &&
                        !this._jwtHelper.isTokenExpired(this.token)) {
                        sessionStorage.setItem('jwtToken', this.token);
                        // set token on shared.service
                        this._sharedService.setTokenModel(this._jwtHelper.decodeToken(this.token) as ITokenModel);
                        //check if ie
                        if ((/MSIE 9/i.test(navigator.userAgent) || (/MSIE 10/i.test(navigator.userAgent)) || /rv:11.0/i.test(navigator.userAgent))) { 
                            location.href = "dashboard";
                        }
                        else {
                            this.router.navigateByUrl('dashboard');
                        }
                        return; 
                    }
                }
                catch (e) {
                    throw e;
                }
            } 

            // Store the attempted URL for redirecting
            this.authenticationService.redirectUrl = state.url;
            // Navigate to the login page
            this.authenticationService.login();
                //this.router.navigate(['account/login']);
        });

        return false;
    }
}