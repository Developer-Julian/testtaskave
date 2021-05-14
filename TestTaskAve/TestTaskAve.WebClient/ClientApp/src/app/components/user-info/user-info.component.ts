import { LoginService } from './../../services/login.service';
import { HttpProxyService } from './../../services/http-proxy.service';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.scss'],
})
export class UserInfoComponent {
  public form: FormGroup = new FormGroup({
    login: new FormControl('', [Validators.required]),
  });

  constructor(
    private httpProxy: HttpProxyService,
    private loginService: LoginService,
    private router: Router
  ) {}

  public submit(): void {
    if (!this.form.valid) {
      return;
    }

    this.httpProxy.connect(this.form.controls.login.value).subscribe(
      (_) => {
        this.loginService.login(this.form.controls.login.value);
        this.router.navigate(['/chat']);
      },
      (error) => {
        console.error(error);
      }
    );
  }
}
