import { UserInfoComponent } from './components/user-info/user-info.component';
import { ChatComponent } from './components/chat/chat.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ErrorComponent } from './components/error/error.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: '/user-info', pathMatch: 'full' },
  {
    path: 'user-info',
    component: UserInfoComponent,
  },
  { path: 'chat', component: ChatComponent, canActivate: [AuthGuard] },
  { path: 'error', component: ErrorComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
