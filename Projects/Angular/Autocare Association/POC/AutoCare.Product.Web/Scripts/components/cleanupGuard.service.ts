import { Injectable }    from '@angular/core';
import { CanDeactivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable }    from 'rxjs/Observable';

export interface IComponentGuard {
    cleanupComponent: () => Observable<boolean> | boolean;
}

@Injectable()
export class cleanupGuardService implements CanDeactivate<IComponentGuard> {
    canDeactivate(component: IComponentGuard, route: ActivatedRouteSnapshot, state: RouterStateSnapshot):
        Observable<boolean> | boolean {
        return component.cleanupComponent ? component.cleanupComponent() : true;
    }
}
