import { Injectable } from '@angular/core';
import { BehaviorSubject, filter, Observable } from "rxjs";
import { Device, DeviceFault, ZoneUse } from "../model/models";
import * as signalR from '@microsoft/signalr';
import { DataSharingService } from "../app/data-sharing.service";
import { environment } from "../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class PrasennsaRealtimeService {
  private hubConnection?: signalR.HubConnection;
  private devicesSubject = new BehaviorSubject<Device[]>([]);
  devices$: Observable<Device[]> = this.devicesSubject.asObservable();
  private zonesSubject = new BehaviorSubject<ZoneUse[]>([]);
  zones$: Observable<ZoneUse[]> = this.zonesSubject.asObservable();

  constructor(protected dataSharingService: DataSharingService) {
    this.connect();
    this.disconnect();
  }

  connect() {

    this.dataSharingService.loginform$.pipe(
      filter(Boolean)
    ).subscribe(login => {
      console.log('login-form', login);
      const baseUrl = `${environment.API_URL}:${environment.PORT}`;
      this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(`ws://${baseUrl}/notifications?userName=${login.userName}&password=${login.password}&ip=${login.ipOrHostName}`,
          {
            withCredentials: sessionStorage.getItem('token') != null,
            accessTokenFactory: () => {
              let token = sessionStorage.getItem('token');
              return token ?? '';
            },
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets,
          }
        )
        .build();

      this.hubConnection
        .start()
        .then(() => console.log('Connected to SignalR hub'))
        .catch(err => console.error('Error connecting to SignalR hub:', err));

      this.hubConnection.on('SendDevices', (devices: Device[]) => {
        console.log('devices', devices);
        this.devicesSubject.next(devices);
      });

      this.hubConnection.on('SendFaultState', (args: string) => {
        console.log('fault state recieved', args);
      });

      this.hubConnection.on('SendZoneState', (args: string) => {
        console.log('zone state recieved', args);
      });

      this.hubConnection.on('CallStateChanged', (args: string) => {
        console.log('call state recieved', args);
      });

      this.hubConnection.on('SendZones', (args: ZoneUse[]) => {
        console.log('zones recieved', args);
        this.zonesSubject.next(args);
      });

      this.hubConnection.on('SendZoneInUse', (args: ZoneUse[]) => {
        console.log('zones recieved....', args);
        const allZones = args.map(z => z.name);
        const inUse = args[0].inUse;
        const zones = this.zonesSubject.value;
        zones.filter(zone => allZones.includes(zone.name)).forEach(z => z.inUse = inUse);
        this.zonesSubject.next(zones);
      });

      this.hubConnection.on('SendDevicesFault', (args: Device[]) => {
        console.log('device update recieved....', args);
        const allDevices = args.map(a => a.name);
        const updatedState = args[0].isConnected;
        const prevDevices = this.devicesSubject.value;
        prevDevices.filter(dev => allDevices.findIndex(name => !!name && dev.name?.includes(name)) > -1).forEach(d => {
          d.isConnected = updatedState;
          d.displayFault = `${args[0]?.fault?.length} fault(s)`;
        });
        this.devicesSubject.next(prevDevices);
      });

      //SendDeviceFault
      this.hubConnection.on('SendDeviceFault', (args: Device) => {
        console.log('device fault receieved....', args);
        const prevDevices = this.devicesSubject.value;
        prevDevices.filter(dev => dev.name == args?.name).forEach(d => {
          d.fault = args.fault;
          const count = args.fault?.length ?? 0;
          d.displayFault = count > 0 ? `${count} fault(s)` : '';
          d.isConnected = args.isConnected;
        });
        this.devicesSubject.next(prevDevices);
      });
    })

  }

  disconnect() {
    this.dataSharingService.logOut$.pipe(filter(Boolean)).
      subscribe(_ => {
        this.hubConnection?.stop().then(
          value => console.log('disconnected'),
          error => console.error('error', error)
        );
      })
  }

}
