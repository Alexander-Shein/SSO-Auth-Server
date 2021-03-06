import {Injectable} from "@angular/core";
import {Resolve, RouterStateSnapshot, ActivatedRouteSnapshot} from "@angular/router";
import {User} from "../models/user";
import {UsersService} from "./users.service";
import {Observable} from "rxjs";
import {AuthenticationService} from "./authentication.service";


@Injectable()
export class CurrentUserResolver implements Resolve<User> {

    public reject:(p1:Error)=>void;

    public constructor(
        private authenticationService: AuthenticationService,
        private usersService: UsersService) {}

    public resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<User> {

        if (this.authenticationService.isLoggedIn()) {
            return this.usersService.getUser();
        } else {
            return Observable.of<User>(null);
        }

    }

}