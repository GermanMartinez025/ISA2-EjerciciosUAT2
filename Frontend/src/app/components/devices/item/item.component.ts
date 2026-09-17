import { Component, Input } from "@angular/core";
import { CommonModule } from "@angular/common";
import { MatCardModule } from "@angular/material/card";
import { MatIconModule } from "@angular/material/icon";
import { MatTooltipModule } from "@angular/material/tooltip";
import { ResponseDevice } from "../../../models/device";
import { DevicesService } from "../../../services/devices.service";
import { MatDialog } from "@angular/material/dialog";
import { MorePhotosComponent } from "../more-photos/more-photos.component";

@Component({
  selector: "app-item",
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule, MatTooltipModule],
  templateUrl: "./item.component.html",
  styleUrl: "./item.component.css",
})
export class ItemComponent {
  @Input() device!: ResponseDevice;
  @Input() showCompany = false;

  constructor(
    private devicesService: DevicesService,
    private dialog: MatDialog
  ) {}

  getDeviceIcon(deviceType: string): string {
    return this.devicesService.deviceIconsMap[deviceType];
  }

  getDeviceLabel(deviceType: string): string {
    return this.devicesService.deviceLabelsMap[deviceType];
  }

  secondaryPhotos(): string[] {
    return this.device.photos.filter(
      (photo) => photo !== this.device.mainPhoto
    );
  }

  showMorePhotos() {
    this.dialog.open(MorePhotosComponent, {
      data: {
        photos: this.secondaryPhotos(),
      },
      width: "80%",
      height: "fit-content",
    });
  }
}
