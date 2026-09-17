import { Component, ViewChild } from "@angular/core";
import { FiltersComponent } from "../filters/filters.component";
import {
  MatPaginator,
  MatPaginatorModule,
  PageEvent,
} from "@angular/material/paginator";
import { FiltersCompanies } from "../../../models/filters/filtersCompanies";
import { CompaniesService } from "../../../services/companies.service";
import { ResponseCompany } from "../../../models/company";
import { CompanySummaryComponent } from "../company-summary/company-summary.component";
import { CommonModule } from "@angular/common";
@Component({
  selector: "app-list",
  standalone: true,
  imports: [
    MatPaginatorModule,
    FiltersComponent,
    CompanySummaryComponent,
    CommonModule,
  ],
  templateUrl: "./list.component.html",
  styleUrl: "./list.component.css",
})
export class ListComponent {
  filters = new FiltersCompanies();

  constructor(private companiesService: CompaniesService) {}

  pageOptions = { actual: 1, size: 10 };

  totalCompanies = 0;
  companies: ResponseCompany[] = [];

  pageChange($event: PageEvent) {
    this.pageOptions.actual = $event.pageIndex + 1;
    this.pageOptions.size = $event.pageSize;
    this.searchCompanies();
  }

  ngOnInit() {
    this.searchCompanies();
  }

  applyFilters() {
    this.resetPagination();
    this.searchCompanies();
  }

  searchCompanies() {
    this.companiesService
      .getCompanies(this.filters, this.pageOptions)
      .subscribe((companies) => {
        console.log(companies);
        this.companies = companies.data?.companies ?? [];
        this.totalCompanies = companies.data?.totalCompanies ?? 0;
      });
  }

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  private resetPagination() {
    this.paginator.firstPage();
  }
}
