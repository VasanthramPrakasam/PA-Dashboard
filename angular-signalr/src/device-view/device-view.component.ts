import {Component, ElementRef, QueryList, ViewChildren} from '@angular/core';
import {PrasennsaRealtimeService} from "../device-status/prasennsa-realtime.service";
import {Observable} from "rxjs";
import {ConnectedState, Device, Legend} from "../model/models";
import {DataSharingService} from "../app/data-sharing.service";
import {StatusViewComponent} from "../status-view/status-view.component";
import {AsyncPipe, JsonPipe, KeyValuePipe} from "@angular/common";

@Component({
  selector: 'app-device-view',
  standalone: true,
  imports: [StatusViewComponent, AsyncPipe, JsonPipe, KeyValuePipe],
  templateUrl: './device-view.component.html',
  styleUrl: './device-view.component.css'
})
export class DeviceViewComponent {

  device$:Observable<Device[]> ;

  legends: Legend[] = [{ color: 'red', text: 'Fault' },{ color: 'green', text: 'Connected' }]

  @ViewChildren(`button`)
  btns: QueryList<ElementRef<HTMLButtonElement>>;

  expanded = false;

  constructor(protected prassensaService:PrasennsaRealtimeService,protected dataSharingService:DataSharingService) {
    //this.prassensaService.connect(this.dataSharingService.formValue);
    this.device$ = this.prassensaService.devices$;
  }

  getGroupColor(devices:Device[]):string{
    return this.getColor(devices.map(dev => dev.isConnected ?? ConnectedState.Connected).reduce((previousValue, currentValue) => currentValue > previousValue ? currentValue : previousValue, ConnectedState.Connected));
  }

  getColor(type:ConnectedState){
    return type === 0 ? 'green' : type === 1 ? 'red' : 'orange';
  }

  expandAllAccordians(e: any) {
    e?.preventDefault();

    let current = this.expanded;
    this.btns.filter(btn => {
      if (current) {
        return !btn.nativeElement.classList.contains('collapsed');
      } else {
        return btn.nativeElement.classList.contains('collapsed');
      }
    }
    ).forEach(btn => btn.nativeElement.click());

    this.expanded = !current;

  }

  getDeviceGroups(devices:Device[]) {
    return this.groupBy(devices, (device:Device) => device.name?.substring(0,device.name?.indexOf("-")));
  }

  groupBy(array:any[], keyMapper: any){
    const returnValue = {};
    array.reduce((acc, curr) => {
      let key;
      if (keyMapper instanceof Function){
          key = keyMapper(curr);
      }
      const value = acc[key] ?? [];
      value.push(curr);
      acc[key] = value;
      return acc;
    }, returnValue);
    return returnValue;
  }

  getDevicesForGrp(grp:string,devices:Device[]){
    return devices.filter(dev => dev.name?.includes(grp));
  }

}
