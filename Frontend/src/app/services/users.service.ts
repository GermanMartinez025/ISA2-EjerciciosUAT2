import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { GetUsersResponse, ResponseUser, UserCreate } from "../models/user";
import { GenericResponse } from "../models/genericResponse";
import { catchError, map, Observable, throwError } from "rxjs";
import { AuthenticationService } from "./authentication.service";
import { FilterUsers } from "../models/filters/filtersUser";

@Injectable({
  providedIn: "root",
})
export class UsersService {
  private url = `${environment.apiUrl}users`;

  constructor(
    private http: HttpClient,
    private authService: AuthenticationService
  ) {}

  private buildHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({ Authorization: `${token}` });
  }

  private buildQueryParams(filters: FilterUsers, pagination: any): string {
    const filterParams = Object.entries(filters)
      .filter(([_, value]) => Array.isArray(value) && value.length > 0)
      .map(([key, values]) =>
        (values as string[]).map((value) => `${key}=${value}`).join("&")
      )
      .join("&");

    const paginationParams = `page=${pagination.actual}&pageSize=${pagination.size}`;
    return `${filterParams}&${paginationParams}`;
  }

  public getUsers(
    filters: FilterUsers,
    pagination: any
  ): Observable<GenericResponse<any>> {
    const headers = this.buildHeaders();
    const query = this.buildQueryParams(filters, pagination);

    return this.http
      .get<GenericResponse<GetUsersResponse | null>>(`${this.url}?${query}`, {
        headers,
      })
      .pipe(
        map((response) => {
          if (!response.data) {
            return {
              ...response,
              data: {
                users: [],
                totalUsers: 0,
                actualPage: 0,
                totalPages: 0,
              },
            };
          }

          const transformedUsers = response.data.users.map(
            (user) =>
              new ResponseUser(
                user.id,
                user.fullName,
                user.email,
                user.roles,
                user.creationDate,
                user.profilePhoto
              )
          );

          return {
            ...response,
            data: {
              ...response.data,
              users: transformedUsers,
            },
          };
        }),
        catchError((error) => throwError(() => error.error))
      );
  }

  public getUser(id: string): Observable<GenericResponse<ResponseUser>> {
    return this.http
      .get<GenericResponse<ResponseUser>>(`${this.url}/${id}`)
      .pipe(catchError((error) => throwError(() => error.error)));
  }

  public createUser(
    user: UserCreate
  ): Observable<GenericResponse<ResponseUser>> {
    const headers = this.buildHeaders();
    return this.http
      .post<GenericResponse<ResponseUser>>(this.url, user, { headers })
      .pipe(catchError((error) => throwError(() => error.error)));
  }

  public deleteUser(
    user: ResponseUser
  ): Observable<GenericResponse<{ id: number }>> {
    const headers = this.buildHeaders();
    return this.http
      .delete<GenericResponse<{ id: number }>>(`${this.url}/${user.id}`, {
        headers,
      })
      .pipe(catchError((error) => throwError(() => error.error)));
  }

  addAsHomeUser(): Observable<GenericResponse<any>> {
    const headers = this.buildHeaders();
    return this.http
      .put<GenericResponse<any>>(
        `${this.url}`,
        { role: "HomeUser" },
        { headers }
      )
      .pipe(catchError((error) => throwError(() => error.error)));
  }
}
