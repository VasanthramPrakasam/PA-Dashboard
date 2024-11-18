import {Component, Input} from '@angular/core';
import {Observable} from "rxjs";
import {Device, Legend, ZoneUse} from "../model/models";
import {AsyncPipe} from "@angular/common";
import {Router, RouterLink} from "@angular/router";
import {DataSharingService} from "../app/data-sharing.service";

@Component({
  selector: 'app-status-view',
  standalone: true,
  imports: [
    AsyncPipe,
    RouterLink
  ],
  templateUrl: './status-view.component.html',
  styleUrl: './status-view.component.css'
})
export class StatusViewComponent {

  device$:Observable<Device[] | undefined> ;
  zones$:Observable<ZoneUse[]>;

  @Input() legends:Legend[] = [];

  constructor(protected router:Router, protected dataSharingService:DataSharingService) {

  }

  isCurrentPage(route:string):boolean{
    return this.router.url === route;
  }

  ngOnInit() {
    //this.zones$.subscribe(console.log);
  }

  logOut($event:any){
    $event?.stopPropagation();
    this.dataSharingService.setLoggedOut(true);
    this.router.navigateByUrl('/').then(_ => {
      window.location.reload();
    });
  }

}
