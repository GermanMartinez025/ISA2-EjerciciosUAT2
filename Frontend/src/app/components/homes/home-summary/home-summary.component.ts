import { Component, Input } from "@angular/core";
import { ResponseGetHome } from "../../../models/home";
import { MatCardModule } from "@angular/material/card";
import { MatIconModule } from "@angular/material/icon";
@Component({
  selector: "app-home-summary",
  standalone: true,
  imports: [MatCardModule, MatIconModule],
  templateUrl: "./home-summary.component.html",
  styleUrl: "./home-summary.component.css",
})
export class HomeSummaryComponent {
  @Input() home!: ResponseGetHome;
  @Input() membersCount!: number;
  @Input() devicesCount!: number;
}
