import {
  ActivatedRouteSnapshot,
  CanActivate,
  GuardResult,
  MaybeAsync,
  Router,
  RouterStateSnapshot
} from '@angular/router';
import {Injectable} from "@angular/core";
import {of} from "rxjs";

@Injectable({
  providedIn:'root'
})
export class LoginGuard implements CanActivate{

  constructor(protected router:Router) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): MaybeAsync<GuardResult> {
    const currentUrl = this.router.url;
    if (currentUrl.includes('login')){
      return of(true);
    }
    const loggedIn = sessionStorage.getItem('logged-in');
    if (!!loggedIn){
      return of(true);
    }
    return this.router.createUrlTree(['login']);
  }


}
