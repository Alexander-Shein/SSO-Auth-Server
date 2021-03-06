import {Component, Input} from "@angular/core";
import {LogIn} from "../models/log-in";
import {AppVm} from "../../business/apps/models/app-vm";
import {AuthBaseComponent} from "../auth-base.component";
import {ActivatedRoute, Router} from "@angular/router";
import {AuthenticationService} from "../services/authentication.service";
import {SpinnerService} from "../../../common/spinner/services/spinner.service";
import {LogInResult} from "../models/log-in-result";
import {Error} from "../../../common/error";


@Component({
    selector: 'au-log-in-password',
    styleUrls: ['../auth.scss'],
    template: `

        <form class="row mt-1" #logInForm="ngForm" (submit)="onSubmit()">
            <div class="col-12">
                <div class="row">
                    <div class="col">
                        <md-input-container class="w-100">
                            <input
                                    autofocus
                                    mdInput
                                    placeholder="Password"
                                    type="{{password.show ? 'text' : 'password'}}"
                                    name="password"
                                    maxlength="100"
                                    minlength="6"
                                    required
                                    [(ngModel)]="logIn.password"
                                    #password="ngModel">
                            <md-hint align="end">{{password.value?.length || 0}} / 100</md-hint>
                            <md-hint *ngIf="password.errors && (password.dirty || password.touched)" style="color: red;">
                                <span [hidden]="!password.errors.required" >
                                    Password is required.
                                </span>
                                <span [hidden]="!password.errors.minlength || password.errors.required">
                                    Min length is 6 characters for password.
                                </span>
                            </md-hint>
                            <md-hint *ngIf="isInvalidPasswordAttempt()" style="color: red;">
                                <span>Invalid log in attempt.</span>
                            </md-hint>
                            <au-show-hide-password [password]="password"></au-show-hide-password>
                        </md-input-container>
                    </div>
                </div>
                <div class="row mt-1">
                    <div class="col">
                        <button
                                class="w-100"
                                [disabled]="!logInForm.form.valid || isInvalidPasswordAttempt()"
                                md-raised-button color="primary">log in</button>
                    </div>
                </div>
                <div class="row mt-1" *ngIf="app.isRememberLogInEnabled" style="font-size: 12px;">
                    <div class="col text-left">
                        <div>
                            <md-checkbox
                                    
                                    color="primary"
                                    name="remember"
                                    [(ngModel)]="logIn.rememberLogIn">
                                <span>stay logged in</span>
                            </md-checkbox>
                            <!--<md-icon style="position: absolute; margin-left: 3px;"
                                     mdTooltip="Protect your account. Uncheck if using public/shared device."
                                     [mdTooltipPosition]="'above'">info_outline</md-icon>-->
                        </div>
                    </div>
                    <div class="col text-right">
                        <a
                                [routerLink]="['/forgot-password', {userName: logIn.userName}]"
                                [queryParams]="{redirectUrl: redirectUrl}"
                                style="vertical-align: middle;">Forgot password?</a>
                    </div>
                </div>
            </div>
        </form>
    `
})
export class LogInPasswordComponent extends AuthBaseComponent {

    constructor(
        route: ActivatedRoute,
        router: Router,
        private authenticationService: AuthenticationService,
        private spinnerService: SpinnerService
    ) {
        super(route, router);
    }

    @Input()
    public logIn: LogIn;

    @Input()
    public app: AppVm;

    public isInvalidPasswordAttempt(): boolean {

        for (let passwordAttempt of this.invalidPasswordAttempts) {
            if (passwordAttempt === this.logIn.password) {
                return true;
            }
        }

        return false;
    }

    private invalidPasswordAttempts: string[] = [];

    public onSubmit() {
        this.spinnerService.show();

        this.authenticationService
            .logIn(this.logIn)
            .subscribe((result: LogInResult) => {

                if (result.requiresTwoFactor) {
                    this.router
                        .navigate(['/two-factor', {
                            rememberLogIn: this.logIn.rememberLogIn
                        }], {
                            queryParams: {
                                redirectUrl: this.redirectUrl
                            }
                        });
                    return;
                }

                this.redirectAfterLogin();
            }, (errors: Error[]) => {

                for (let error of errors) {
                    if (error.code === 1) {
                        this.invalidPasswordAttempts.push(this.logIn.password);
                        break;
                    }
                }

                this.spinnerService.hide();
            });
    }

}