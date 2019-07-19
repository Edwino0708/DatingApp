import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = environment.baseUrl;
  decodedToken: any;
  jwtHelper = new JwtHelperService();

  constructor(private http: HttpClient) {}

  login(model: any) {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.http.post(this.baseUrl + 'auth/login', model, { headers }).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          console.log(this.decodedToken);
        }
      })
    );
  }

  register(model: any) {
    var headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.http.post(this.baseUrl + 'auth/Register', model, { headers });
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
}
