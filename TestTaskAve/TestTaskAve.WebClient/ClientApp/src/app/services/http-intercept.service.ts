import {
  HttpHandler,
  HttpEvent,
  HttpRequest,
  HttpInterceptor,
  HttpResponse,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { tap, catchError, map } from 'rxjs/operators';

@Injectable()
export class InterceptService implements HttpInterceptor {
  constructor(private router: Router) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return this.handle(req.clone(), next);
  }

  private handle(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      map((event: HttpEvent<any>) => {
        if (event instanceof HttpResponse) {
          return event;
        }

        return next.handle(req);
      }),
      catchError((err: any) => {
        let statusText: string = err.statusText;

        if (err.status === 500) {
          statusText = 'An error occurred on the server';
        } else {
          statusText = err.status + ' ' + err.statusText;
        }

        console.warn(statusText, err);

        this.router.navigate(['/error']);

        return throwError(statusText);
      })
    ) as Observable<HttpEvent<any>>;
  }
}
