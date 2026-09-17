import { Component } from "@angular/core";
import { CommonModule } from "@angular/common";
import { DevicesService } from "../../services/devices.service";
import { MatIconModule } from "@angular/material/icon";
import { MatTooltipModule } from "@angular/material/tooltip";
@Component({
  selector: "app-home",
  standalone: true,
  imports: [CommonModule, MatIconModule, MatTooltipModule],
  templateUrl: "./home.component.html",
  styleUrl: "./home.component.css",
})
export class HomeComponent {
  supportedDevices: string[] = [];
  constructor(private devicesService: DevicesService) {}

  ngOnInit(): void {
    this.devicesService.getSupportedDevices().subscribe((devices) => {
      this.supportedDevices = devices;
    });
  }

  getDeviceIcon(deviceType: string): string {
    return this.devicesService.deviceIconsMap[deviceType];
  }
}
