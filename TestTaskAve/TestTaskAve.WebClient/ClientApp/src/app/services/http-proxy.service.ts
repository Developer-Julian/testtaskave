import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class HttpProxyService {
  private chatApi = '/v1/chat';
  constructor(public http: HttpClient) {}

  public connect(userName: string): Observable<any> {
    return this.http.post<any>(`${this.chatApi}/connect`, {
      userName,
    });
  }

  public sendMessage(userName: string, message: string): Observable<any> {
    return this.http.post<any>(`${this.chatApi}`, { userName, message });
  }

  public disconnect(userName: string): Observable<any> {
    return this.http.post<any>(`${this.chatApi}/disconnect`, {
      userName,
    });
  }
}
