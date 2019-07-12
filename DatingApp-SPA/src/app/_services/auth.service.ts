import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = 'https://localhost:44303/api/auth/';

  constructor(private http: HttpClient) {}

  login(model: any) {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');

    return this.http
      .post(this.baseUrl + 'login', model, {
        headers
      })
      .pipe(
        map((response: any) => {
          const user = response;
          if (user) {
            localStorage.setItem('token', user.token);
          }
        })
      );
  }
}
