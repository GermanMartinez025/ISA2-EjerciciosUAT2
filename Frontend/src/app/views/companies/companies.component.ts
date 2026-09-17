import { Component } from "@angular/core";
import { ListComponent } from "../../components/companies/list/list.component";
@Component({
  selector: "app-companies",
  standalone: true,
  imports: [ListComponent],
  templateUrl: "./companies.component.html",
  styleUrl: "./companies.component.css",
})
export class CompaniesComponent {}
