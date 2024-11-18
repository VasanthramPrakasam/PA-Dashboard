import {Component, OnInit} from '@angular/core';
import {BehaviorSubject, map, Observable} from "rxjs";
import {Device} from "../model/models";
import { AsyncPipe } from "@angular/common";
import {PrasennsaRealtimeService} from "./prasennsa-realtime.service";

@Component({
  selector: 'app-device-status',
  standalone: true,
  imports: [
    AsyncPipe
],
  templateUrl: './device-status.component.html',
  styleUrl: './device-status.component.css'
})
export class DeviceStatusComponent implements OnInit{

  device$:Observable<Device[] | undefined> ;
  constructor(protected prassensaService:PrasennsaRealtimeService) {
    this.device$ = this.prassensaService.devices$;
  }



  ngOnInit() {
  }

}
