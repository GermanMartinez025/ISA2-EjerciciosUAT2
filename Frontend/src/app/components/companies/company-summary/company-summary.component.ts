import { Component, Input } from "@angular/core";
import { MatCardModule } from "@angular/material/card";
import { MatIconModule } from "@angular/material/icon";
import { ResponseCompany } from "../../../models/company";
@Component({
  selector: "app-company-summary",
  standalone: true,
  imports: [MatCardModule, MatIconModule],
  templateUrl: "./company-summary.component.html",
  styleUrl: "./company-summary.component.css",
})
export class CompanySummaryComponent {
  @Input() company!: ResponseCompany;
}
