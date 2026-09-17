import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { GenericResponse } from "../models/genericResponse";
import { catchError, map, Observable, throwError } from "rxjs";
import { AuthenticationService } from "./authentication.service";
import { GetCompaniesResponse, ResponseCompany } from "../models/company";
import { FiltersCompanies } from "../models/filters/filtersCompanies";

@Injectable({
  providedIn: "root",
})
export class CompaniesService {
  constructor(
    private http: HttpClient,
    private authService: AuthenticationService
  ) {}

  private url = `${environment.apiUrl}companies`;

  private buildHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({ Authorization: `${token}` });
  }

  public getMyCompany(): Observable<GenericResponse<ResponseCompany>> {
    const headers = this.buildHeaders();
    return this.http
      .get<GenericResponse<ResponseCompany>>(`${this.url}/me`, { headers })
      .pipe(
        map((response) => response),
        catchError((error) => throwError(() => error.error))
      );
  }

  public createCompany(
    company: any
  ): Observable<GenericResponse<ResponseCompany>> {
    const headers = this.buildHeaders();
    return this.http
      .post<GenericResponse<ResponseCompany>>(this.url, company, { headers })
      .pipe(
        map((response) => response),
        catchError((error) => throwError(() => error.error))
      );
  }

  getCompanies(
    filters: FiltersCompanies,
    pagination: any
  ): Observable<GenericResponse<GetCompaniesResponse>> {
    const url = `${environment.apiUrl}companies`;
    const query = this.buildQueryParams(filters, pagination);
    const headers = this.buildHeaders();

    return this.http.get<GenericResponse<GetCompaniesResponse>>(
      `${url}?${query}`,
      { headers }
    );
  }

  private buildQueryParams(filters: FiltersCompanies, pagination: any): string {
    const filterParams = Object.entries(filters)
      .filter(([_, value]) => Array.isArray(value) && value.length > 0)
      .map(([key, values]) =>
        (values as string[]).map((value) => `${key}=${value}`).join("&")
      )
      .join("&");

    const paginationParams = `page=${pagination.actual}&pageSize=${pagination.size}`;
    return `${filterParams}&${paginationParams}`;
  }
}
