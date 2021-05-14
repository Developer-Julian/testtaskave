import { Injectable } from '@angular/core';

@Injectable()
export class LoginService {
  private userName: string = '';

  public isLogged(): boolean {
    return !!this.userName;
  }

  public login(userName: string): void {
    this.userName = userName;
  }

  public getUserName(): string {
    return this.userName;
  }
}
