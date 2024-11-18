import { Component } from '@angular/core';
import {StatusViewComponent} from "../status-view/status-view.component";
import { AsyncPipe } from "@angular/common";
import {Observable} from "rxjs";
import {Device, Legend, ZoneUse} from "../model/models";
import {PrasennsaRealtimeService} from "../device-status/prasennsa-realtime.service";
import {DataSharingService} from "../app/data-sharing.service";

@Component({
  selector: 'app-zone-view',
  standalone: true,
  imports: [StatusViewComponent, AsyncPipe],
  templateUrl: './zone-view.component.html',
  styleUrl: './zone-view.component.css'
})
export class ZoneViewComponent {

  zones$:Observable<ZoneUse[]>;
  legends: Legend[] = [{color:'green',text:'No Call or Announcement in progress'},{color:'blue',text:'Call or Announcement in progress'}];

  constructor(protected prassensaService:PrasennsaRealtimeService,protected dataSharingService:DataSharingService) {
    //this.prassensaService.connect(this.dataSharingService.formValue);
    this.zones$ = this.prassensaService.zones$;
  }
}
