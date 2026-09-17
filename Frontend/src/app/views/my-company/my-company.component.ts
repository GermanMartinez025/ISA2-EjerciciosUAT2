import { Component } from "@angular/core";
import { CommonModule } from "@angular/common";
import { MatTabsModule } from "@angular/material/tabs";
import { ResponseCompany } from "./../../models/company";
import { CompaniesService } from "../../services/companies.service";
import { CompanyFormComponent } from "../../components/companies/company-form/company-form.component";
import { CompanySummaryComponent } from "../../components/companies/company-summary/company-summary.component";
import { FiltersDevice } from "../../models/filters/filtersDevice";
import { ListComponent } from "../../components/devices/list/list.component";

@Component({
  selector: "app-my-company",
  standalone: true,
  imports: [
    CommonModule,
    MatTabsModule,
    CompanyFormComponent,
    CompanySummaryComponent,
    ListComponent,
  ],
  templateUrl: "./my-company.component.html",
  styleUrl: "./my-company.component.css",
})
export class MyCompanyComponent {
  constructor(private companiesService: CompaniesService) {}

  company?: ResponseCompany;

  ngOnInit() {
    this.getMyCompany();
  }

  onCompanyCreated() {
    this.getMyCompany();
  }

  getMyCompany() {
    this.companiesService.getMyCompany().subscribe({
      next: (response) => {
        this.company = response.data;

        this.setFilters();
      },
    });
  }

  setFilters() {
    if (this.company) {
      this.filters.companyName = [this.company.companyName];
    }
  }

  filters = new FiltersDevice();
}
