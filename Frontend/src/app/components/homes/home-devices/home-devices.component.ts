import { Component, Input } from "@angular/core";
import { ResponseHomeDevice } from "../../../models/home";
import { CommonModule } from "@angular/common";
import { HomeDeviceItemComponent } from "../home-device-item/home-device-item.component";
@Component({
  selector: "app-home-devices",
  standalone: true,
  imports: [CommonModule, HomeDeviceItemComponent],
  templateUrl: "./home-devices.component.html",
  styleUrl: "./home-devices.component.css",
})
export class HomeDevicesComponent {
  @Input() devices: ResponseHomeDevice[] = [];
}
