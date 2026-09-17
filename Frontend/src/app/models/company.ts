export class ResponseCompany {
  public id: number;
  public companyName: string;
  public ownerFullName: string;
  public ownerEmail: string;
  public companyRut: string;
  public companyLogoType: string;
  public totalDevices: number;

  constructor(
    id: number,
    companyName: string,
    ownerFullName: string,
    ownerEmail: string,
    companyRut: string,
    companyLogoType: string,
    totalDevices: number
  ) {
    this.id = id;
    this.companyName = companyName;
    this.ownerFullName = ownerFullName;
    this.ownerEmail = ownerEmail;
    this.companyRut = companyRut;
    this.companyLogoType = companyLogoType;
    this.totalDevices = totalDevices;
  }
}

export interface GetCompaniesResponse {
  companies: ResponseCompany[];
  totalCompanies: number;
  actualPage: number;
  totalPages: number;
}
