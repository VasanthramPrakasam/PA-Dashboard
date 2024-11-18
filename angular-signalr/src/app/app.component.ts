import {Component} from '@angular/core';
import {Router, RouterOutlet} from '@angular/router';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {BehaviorSubject} from "rxjs";
import {AsyncPipe} from "@angular/common";
import {DataSharingService} from "./data-sharing.service";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FormsModule, ReactiveFormsModule, AsyncPipe],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'angular-signalr-demo';

  loggedIn$:BehaviorSubject<boolean>;
  constructor(protected router:Router,protected dataSharingService:DataSharingService) {
    this.loggedIn$ = this.dataSharingService.loggedIn$;
  }


  onSubmit() {
    this.dataSharingService.setLoggedIn(true);
    this.router.navigateByUrl("/device-status").then(_ => console.log("redirected"));
  }
}
