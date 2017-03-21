import {Component} from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import {NotificationsService} from "angular2-notifications";
import {SpinnerService} from "../../../common/spinner/services/spinner.service";
import {AuthBaseComponent} from "../auth-base.component";
import {PhonesService} from "./services/phones.service";
import {NewPhone} from "./models/new-phone";


@Component({
    selector: 'au-add-phone',
    templateUrl: './add-phone-page.component.html',
    styleUrls: ['../auth.scss', './add-phone-page.component.scss']
})
export class AddPhonePageComponent extends AuthBaseComponent {

    constructor(
        private phonesService: PhonesService,
        route: ActivatedRoute,
        router: Router,
        notificationsService: NotificationsService,
        spinnerService: SpinnerService
    ) {
        super(route, router, notificationsService, spinnerService);
    }

    public im: NewPhone = new NewPhone();
    public isCodeSent: boolean = false;

    public sendCode(): void {
        this.phonesService
            .sendVerificationCode(this.im)
            .then(() => {
                this.isCodeSent = true;
                this.spinnerService.hide();
            })
            .catch((error: any) => this.handleError(error));
    }

    public verifyCode(): void {
        this.spinnerService.show();

        this.phonesService
            .add(this.im)
            .then(() => this.redirectAfterLogin())
            .catch((error: any) => this.handleError(error));
    }
}