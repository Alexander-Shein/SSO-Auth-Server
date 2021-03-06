import {Injectable} from "@angular/core";
import {Resolve, RouterStateSnapshot, ActivatedRouteSnapshot} from "@angular/router";
import {ExternalProvidersService} from "./external-providers.service";
import {ExternalProvider} from "../models/external-provider";
import {Observable} from "rxjs/Observable";


@Injectable()
export class ExternalProvidersResolver implements Resolve<ExternalProvider[]> {

    public reject:(p1:Error)=>void;

    constructor(private externalProvidersService: ExternalProvidersService) {}

    public resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<ExternalProvider[]> {
        return this.externalProvidersService.getAll();
    }

}