export class FiltersDevice {
  public deviceType: string[];
  public companyName: string[];
  public deviceModel: string[];
  public deviceName: string[];

  constructor(
    deviceType?: string[],
    companyName?: string[],
    deviceModel?: string[],
    deviceName?: string[]
  ) {
    this.deviceType = deviceType ?? [];
    this.companyName = companyName ?? [];
    this.deviceModel = deviceModel ?? [];
    this.deviceName = deviceName ?? [];
  }
}
