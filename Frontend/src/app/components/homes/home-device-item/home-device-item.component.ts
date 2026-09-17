import { Component, inject, Input } from "@angular/core";
import { ResponseHomeDevice } from "../../../models/home";
import { MatCardModule } from "@angular/material/card";
import { MatDialog } from "@angular/material/dialog";
import { MorePhotosComponent } from "../../devices/more-photos/more-photos.component";
import { CommonModule } from "@angular/common";
import { MatIconModule } from "@angular/material/icon";
@Component({
  selector: "app-home-device-item",
  standalone: true,
  imports: [MatCardModule, MatIconModule, CommonModule],
  templateUrl: "./home-device-item.component.html",
  styleUrl: "./home-device-item.component.css",
})
export class HomeDeviceItemComponent {
  @Input() device!: ResponseHomeDevice;

  dialog = inject(MatDialog);

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
