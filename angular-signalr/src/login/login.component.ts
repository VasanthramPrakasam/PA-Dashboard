import {Component, ElementRef, ViewChild} from '@angular/core';
import {AsyncPipe, NgOptimizedImage} from "@angular/common";
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validator, Validators} from "@angular/forms";
import {BehaviorSubject, first} from "rxjs";
import {Router} from "@angular/router";
import {DataSharingService} from "../app/data-sharing.service";
import {environment} from "../environments/environment";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-login',
  standalone: true,
    imports: [
    AsyncPipe,
    FormsModule,
    ReactiveFormsModule,
    NgOptimizedImage
],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  pwdVisible = false;

  loggedIn$:BehaviorSubject<boolean>;

  loginForm: FormGroup;

  @ViewChild('pwd') passWord! : ElementRef<HTMLInputElement>;

  error = false;

  errorMsg = "Error logging in user, try again...";

  constructor(protected router:Router,protected dataSharingService:DataSharingService,protected fb: FormBuilder,
              protected httpClient:HttpClient) {
    this.loggedIn$ = this.dataSharingService.loggedIn$;
    this.loginForm = this.fb.group({
      userName: ['', [Validators.required]],
      password: ['', [Validators.required]],
      ipOrHostName: ['', [Validators.required]],
    });
  }


  onSubmit() {
    const baseUrl = environment.API_URL;
    const loginUrl = `http://${baseUrl}:${environment.PORT}/login`;
    this.httpClient.post(loginUrl, {
      user:this.loginForm.value['userName'],
      pwd:this.loginForm.value['password'],
      ip:this.loginForm.value['ipOrHostName']
    }).pipe(first()).subscribe((response:any) => {
      console.log(response);
      const loginResponse = response?.response;
      if (loginResponse === "OIERROR_OK"){
        console.log('response',response);
        this.dataSharingService.setLoggedIn(true);
        this.dataSharingService.formValue = this.loginForm.value;
        this.dataSharingService.loginform$.next(this.loginForm.value);
        sessionStorage.setItem("logged-in","true");
        this.router.navigateByUrl("/device-status");
      } else if (loginResponse === "OIERROR_BAD_CREDENTIALS"){
        this.error = true;
        this.errorMsg = "Bad credentials";
      }else{
        this.error = true;
      }
    });
  }

  toggleVisibility(){
    const type = this.passWord.nativeElement.type;
    if (type === 'password'){
      this.pwdVisible = true;
      this.passWord.nativeElement.type = 'text';
    }else {
      this.pwdVisible = false;
      this.passWord.nativeElement.type = 'password';
    }
  }

}
