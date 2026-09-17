import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { GenericResponse } from "../models/genericResponse";
import { catchError, map, Observable, throwError } from "rxjs";
import { AuthenticationService } from "./authentication.service";
import {
  RequestCreateRoom,
  ResponseRooms
} from "../models/room";

@Injectable({
  providedIn: "root",
})
export class RoomsService {
  constructor(
    private http: HttpClient,
    private authService: AuthenticationService
  ) {}

  private buildHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({ Authorization: `${token}` });
  }

  public create(
    homeId: number,
    createRoomRequest: RequestCreateRoom
  ): Observable<GenericResponse<ResponseRooms>> {
    const url = `${environment.apiUrl}homes/${homeId}/rooms`;
    const headers = this.buildHeaders();
    return this.http
      .post<GenericResponse<ResponseRooms>>(url, createRoomRequest, {
        headers,
      })
      .pipe(map((response) => response));
  }

}

