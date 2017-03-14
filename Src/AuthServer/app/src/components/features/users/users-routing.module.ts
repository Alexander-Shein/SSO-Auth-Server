import {NgModule} from '@angular/core';
import {RouterModule} from '@angular/router';
import {DashboardPageComponent} from './dashboard/dashboard-page.component';
import {UserSettingsResolver} from './services/user-settings-resolver.service';
import {ChangePasswordPageComponent} from './password/change-password-page.component';
import {AddPasswordPageComponent} from './password/add-password-page.component';
import {ManageExternalProvidersPageComponent} from './external-providers/manage-external-providers-page.component';
import {ExternalProvidersSettingsResolver} from './services/external-providers-settings-resolver.service';

const appRoutes = [
    {
        path: 'dashboard',
        component: DashboardPageComponent,
        resolve: {
            userSettings: UserSettingsResolver
        }
    },
    {path: 'dashboard/change-password', component: ChangePasswordPageComponent},
    {path: 'dashboard/add-password', component: AddPasswordPageComponent},
    {
        path: 'dashboard/manage-external-providers',
        component: ManageExternalProvidersPageComponent,
        resolve: {
            externalProvidersSettings: ExternalProvidersSettingsResolver
        }
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(appRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class UsersRoutingModule {}