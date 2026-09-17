import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject, Observable } from "rxjs";
import { Router } from "@angular/router";

@Injectable({
  providedIn: "root",
})
export class AuthenticationService {
  private TOKEN_KEY = "token";
  private isLoggedInSubject = new BehaviorSubject<boolean>(false);

  isLoggedIn$ = this.isLoggedInSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  public authenticate(email: string, password: string): Observable<any> {
    const url = `${environment.apiUrl}sessions`;
    const body = { email, password };

    return new Observable((observer) => {
      this.http.post(url, body).subscribe(
        (response: any) => {
          if (response && response.token) {
            localStorage.setItem(this.TOKEN_KEY, response.token);
            observer.next(response);
            this.isLoggedInSubject.next(true);
            this.router.navigate(["/home"]);
          } else {
            observer.error("Invalid response: Token missing");
          }
          observer.complete();
        },
        (error) => {
          observer.error(error);
        }
      );
    });
  }

  public logout(): void {
    //TODO: Implementar el logout en el servidor
    localStorage.removeItem(this.TOKEN_KEY);
    this.isLoggedInSubject.next(false);
    this.router.navigate(["/login"]);
  }

  public isUserLogged() {
    return !!localStorage.getItem(this.TOKEN_KEY);
  }

  checkLoginStatus(): void {
    const token = localStorage.getItem(this.TOKEN_KEY);
    this.isLoggedInSubject.next(!!token);
  }

  public getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }
}
