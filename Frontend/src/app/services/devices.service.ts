import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { map, Observable } from "rxjs";
import { FiltersDevice } from "../models/filters/filtersDevice";
import { GenericResponse } from "../models/genericResponse";
import {
  CreateDeviceRequest,
  GetDevicesResponse,
  ResponseDevice,
} from "../models/device";
import { AuthenticationService } from "./authentication.service";

@Injectable({
  providedIn: "root",
})
export class DevicesService {
  constructor(
    private http: HttpClient,
    private authService: AuthenticationService
  ) {}

  private buildHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({ Authorization: `${token}` });
  }

  create(
    createDeviceRequest: CreateDeviceRequest
  ): Observable<GenericResponse<ResponseDevice>> {
    const url = `${environment.apiUrl}devices`;
    const headers = this.buildHeaders();
    return this.http
      .post<GenericResponse<ResponseDevice>>(url, createDeviceRequest, {
        headers,
      })
      .pipe(map((response) => response));
  }

  public getSupportedDevices(): Observable<string[]> {
    const url = `${environment.apiUrl}devices/supported`;
    return this.http
      .get<{
        executionSuccessful: boolean;
        data: { deviceTypes: string[] };
        message: string;
      }>(url)
      .pipe(
        map((response) => {
          return response.executionSuccessful ? response.data.deviceTypes : [];
        })
      );
  }

  getDevices(
    filters: FiltersDevice,
    pagination: any
  ): Observable<GenericResponse<GetDevicesResponse>> {
    const url = `${environment.apiUrl}devices`;
    const query = this.buildQueryParams(filters, pagination);

    return this.http.get<GenericResponse<GetDevicesResponse>>(
      `${url}?${query}`
    );
  }

  buildQueryParams(filters: FiltersDevice, pagination: any) {
    const filterParams = Object.entries(filters)
      .filter(([_, value]) => Array.isArray(value) && value.length > 0)
      .map(([key, values]) =>
        (values as string[]).map((value) => `${key}=${value}`).join("&")
      )
      .join("&");

    const paginationParams = `page=${pagination.actual}&pageSize=${pagination.size}`;
    return `${filterParams}&${paginationParams}`;
  }

  public deviceIconsMap: { [key: string]: string } = {
    Camera: "photo_camera",
    Sensor: "window",
    MotionSensor: "sensors",
    SmartLamp: "lightbulb",
  };

  public deviceLabelsMap: { [key: string]: string } = {
    Camera: "Camera",
    Sensor: "Sensor",
    MotionSensor: "Motion Sensor",
    SmartLamp: "Smart Lamp",
  };
}
