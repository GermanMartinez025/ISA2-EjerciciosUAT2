import { Component } from "@angular/core";
import { ListComponent } from "../../components/devices/list/list.component";
@Component({
  selector: "app-devies",
  standalone: true,
  imports: [ListComponent],
  templateUrl: "./devies.component.html",
  styleUrl: "./devies.component.css",
})
export class DeviesComponent {}
