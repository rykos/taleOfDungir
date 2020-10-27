import { AuthenticationService } from './../_services/authentication.service';
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private authenticationService: AuthenticationService, private router: Router) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(catchError(err => {
      if (err === 401) {
        if (this.authenticationService.currentUserValue) {
          this.authenticationService.logout();
          location.reload();
        }
        this.router.navigate(['/login', { queryParams: { returnUrl: location.pathname } }]);
      }
      console.log(err);
      return throwError(err);
    }));
  }
}
