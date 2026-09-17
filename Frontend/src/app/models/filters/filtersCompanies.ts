export class FiltersCompanies {
  companyNames: string[];
  ownerFullNames: string[];

  constructor(companyNames?: string[], ownerFullNames?: string[]) {
    this.companyNames = companyNames ?? [];
    this.ownerFullNames = ownerFullNames ?? [];
  }
}
