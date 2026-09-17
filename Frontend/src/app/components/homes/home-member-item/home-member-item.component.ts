import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { ResponseGetHomeMember } from "../../../models/home";
import { MatCardModule } from "@angular/material/card";
import { MatIconModule } from "@angular/material/icon";
import { MatTooltipModule } from "@angular/material/tooltip";
@Component({
  selector: "app-home-member-item",
  standalone: true,
  imports: [MatCardModule, MatIconModule, MatTooltipModule, CommonModule],
  templateUrl: "./home-member-item.component.html",
  styleUrl: "./home-member-item.component.css",
})
export class HomeMemberItemComponent {
  @Input() member!: ResponseGetHomeMember;

  grants() {
    const { isNotifiable, listDevices, listUsers } = this.member;

    let grants = [];
    if (isNotifiable) {
      grants.push({
        value: "notificable",
        active: true,
        description: "This user can receive notifications",
        icon: "notifications",
        color: "yellow",
      });
    } else {
      grants.push({
        value: "notificable",
        active: false,
        description: "This user can't receive notifications",
        icon: "notifications_off",
        color: "gray",
      });
    }

    if (listDevices) {
      grants.push({
        value: "listDevices",
        active: true,
        description: "This user can list devices",
        icon: "devices",
        color: "green",
      });
    } else {
      grants.push({
        value: "listDevices",
        active: false,
        description: "This user can't list devices",
        icon: "devices_off",
        color: "gray",
      });
    }

    if (listUsers) {
      grants.push({
        value: "listUsers",
        active: true,
        description: "This user can list users",
        icon: "people",
        color: "blue",
      });
    } else {
      grants.push({
        value: "listUsers",
        active: false,
        description: "This user can't list users",
        icon: "people_off",
        color: "gray",
      });
    }

    return grants;
  }
}
