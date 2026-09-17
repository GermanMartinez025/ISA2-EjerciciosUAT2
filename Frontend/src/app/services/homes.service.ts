import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { GenericResponse } from "../models/genericResponse";
import { catchError, map, Observable, throwError } from "rxjs";
import { AuthenticationService } from "./authentication.service";
import {
  ResponseGetHomes,
  ResponseGetMembers,
  RequestCreateHome,
  ResponseHomeDevices,
  ResponseAddUserToHome,
} from "../models/home";

@Injectable({
  providedIn: "root",
})
export class HomesService {
  constructor(
    private http: HttpClient,
    private authService: AuthenticationService
  ) {}

  private buildHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({ Authorization: `${token}` });
  }

  public getHomes(): Observable<GenericResponse<ResponseGetHomes>> {
    const headers = this.buildHeaders();

    return this.http
      .get<GenericResponse<ResponseGetHomes>>(`${environment.apiUrl}homes`, {
        headers,
      })
      .pipe(
        map((response) => response),
        catchError((error) => throwError(() => error.error))
      );
  }

  public getHomeMembers(
    id: number
  ): Observable<GenericResponse<ResponseGetMembers>> {
    const headers = this.buildHeaders();

    return this.http
      .get<GenericResponse<ResponseGetMembers>>(
        `${environment.apiUrl}homes/${id}/members`,
        {
          headers,
        }
      )
      .pipe(
        map((response) => response),
        catchError((error) => throwError(() => error.error))
      );
  }

  public create(
    createHomeRequest: RequestCreateHome
  ): Observable<GenericResponse<ResponseGetHomes>> {
    const url = `${environment.apiUrl}homes`;
    const headers = this.buildHeaders();
    return this.http
      .post<GenericResponse<ResponseGetHomes>>(url, createHomeRequest, {
        headers,
      })
      .pipe(map((response) => response));
  }

  public getHomeDevices(
    id: number,
    roomName?: string
  ): Observable<GenericResponse<ResponseHomeDevices>> {
    const headers = this.buildHeaders();

    return this.http
      .get<GenericResponse<ResponseHomeDevices>>(
        `${environment.apiUrl}homes/${id}/devices`,
        {
          headers,
          params: roomName ? { roomName } : {},
        }
      )
      .pipe(
        map((response) => response),
        catchError((error) => throwError(() => error.error))
      );
  }

  public getRooms(
    id: number
  ): Observable<GenericResponse<ResponseHomeDevices>> {
    const headers = this.buildHeaders();

    return this.http
      .get<GenericResponse<ResponseHomeDevices>>(
        `${environment.apiUrl}homes/${id}/rooms`,
        {
          headers,
        }
      )
      .pipe(
        map((response) => response),
        catchError((error) => throwError(() => error.error))
      );
  }
      
  addHomeMember(
    id: number,
    email: string
  ): Observable<GenericResponse<ResponseAddUserToHome>> {
    const headers = this.buildHeaders();
    return this.http.post<GenericResponse<ResponseAddUserToHome>>(
      `${environment.apiUrl}homes/${id}/members`,
      { email },
      { headers }
    );
  }
}
