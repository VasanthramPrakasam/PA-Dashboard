import { Injectable } from '@angular/core';
import {BehaviorSubject} from "rxjs";
import {LoginForm} from "../model/models";

@Injectable({
  providedIn: 'root'
})
export class DataSharingService {

  loggedIn$ = new BehaviorSubject(false);

  logOut$:BehaviorSubject<boolean> = new BehaviorSubject(false);

  formValue:LoginForm = {userName:'',password:'',ipOrHostName:''};

  loginform$ = new BehaviorSubject<LoginForm | undefined>(undefined);

  constructor() { }

  setLoggedIn(value:boolean){
    this.loggedIn$.next(value);
  }

  setLoggedOut(value:boolean){
    this.logOut$.next(value);
  }

}
