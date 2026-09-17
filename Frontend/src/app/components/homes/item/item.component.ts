import { Component, Input } from "@angular/core";
import { ResponseGetHome } from "../../../models/home";
import { MatCardModule } from "@angular/material/card";
@Component({
  selector: "app-item",
  standalone: true,
  imports: [MatCardModule],
  templateUrl: "./item.component.html",
  styleUrl: "./item.component.css",
})
export class ItemComponent {
  @Input() home!: ResponseGetHome;
  Json = JSON;
}
