import {Routes} from '@angular/router';
import {DeviceStatusComponent} from "../device-status/device-status.component";
import {LoginComponent} from "../login/login.component";
import {LoginGuard} from "../login/login.guard";
import {DeviceViewComponent} from "../device-view/device-view.component";
import {ZoneViewComponent} from "../zone-view/zone-view.component";

export const routes: Routes = [
  {
    path:'device-status',
    component:DeviceViewComponent,
    canActivate:[LoginGuard]
  },
  {
    path:'zone-status',
    component:ZoneViewComponent,
    canActivate:[LoginGuard]
  },
  {
    path:'login',
    component:LoginComponent,
    //canActivate:() => true;
  },
  {
    path:'test',
    component:DeviceStatusComponent
  },
  {
    path:'',
    component:LoginComponent
  },
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full'
  }
];
