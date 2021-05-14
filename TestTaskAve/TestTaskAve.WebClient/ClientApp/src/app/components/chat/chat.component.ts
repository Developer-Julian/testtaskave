import { LoginService } from './../../services/login.service';
import { HttpProxyService } from './../../services/http-proxy.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import {
  HttpTransportType,
  HubConnection,
  HubConnectionBuilder,
} from '@microsoft/signalr';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
})
export class ChatComponent implements OnInit, OnDestroy {
  public messages: string[] = [];
  public userMessage: string = '';
  private connection: HubConnection | null = {} as HubConnection;
  constructor(
    private httpProxyService: HttpProxyService,
    private loginService: LoginService
  ) {}

  async ngOnDestroy(): Promise<void> {
    if (this.connection) {
      this.connection.stop();
      this.connection = null;
    }

    await this.httpProxyService
      .disconnect(this.loginService.getUserName())
      .toPromise()
      .then();
  }

  ngOnInit(): void {
    this.initWebSocket();
  }

  private initWebSocket(): void {
    this.connection = new HubConnectionBuilder().withUrl('/chathub').build();

    this.connection.on('messageReceived', (message: string) => {
      this.messages.push(message);
    });

    this.connection.start();
  }

  public sendMessage(): void {
    this.httpProxyService
      .sendMessage(this.loginService.getUserName(), this.userMessage)
      .subscribe(
        (_) => {
          this.messages.push(this.userMessage);
          this.userMessage = '';
        },
        (error) => {
          console.error(error);
        }
      );
  }
}
