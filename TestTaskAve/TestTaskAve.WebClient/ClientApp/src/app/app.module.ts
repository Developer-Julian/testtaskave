import { LoginService } from './services/login.service';
import { UserInfoComponent } from './components/user-info/user-info.component';
import { ChatComponent } from './components/chat/chat.component';
import { InterceptService } from './services/http-intercept.service';
import { HttpProxyService } from './services/http-proxy.service';
import { MaterialModule } from './material.module';
import { ErrorComponent } from './components/error/error.component';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent,
    ChatComponent,
    UserInfoComponent,
    ErrorComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    MaterialModule,
    ReactiveFormsModule,
    HttpClientModule,
  ],
  providers: [
    LoginService,
    HttpProxyService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: InterceptService,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
