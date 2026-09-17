import { Component, Inject, Input } from "@angular/core";
import { CommonModule } from "@angular/common";
import { MAT_DIALOG_DATA } from "@angular/material/dialog";
@Component({
  selector: "app-more-photos",
  standalone: true,
  imports: [CommonModule],
  templateUrl: "./more-photos.component.html",
  styleUrl: "./more-photos.component.css",
})
export class MorePhotosComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: { photos: string[] }) {}

  photos: string[] = [];

  ngOnInit() {
    this.photos = this.data.photos;
  }

  selectPhoto(photo: number) {
    this.selectedPhoto = photo;
  }
  selectedPhoto = 0;
}
