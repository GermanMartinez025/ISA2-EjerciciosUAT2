import { Component, Input } from "@angular/core";
import {
  ResponseGetHome,
  ResponseGetHomeMember,
  ResponseHomeDevice,
} from "../../../models/home";
import { ResponseRooms } from "../../../models/room";
import { HomeSummaryComponent } from "../home-summary/home-summary.component";
import { MatTabsModule } from "@angular/material/tabs";
import { HomeMembersComponent } from "../home-members/home-members.component";
import { HomesService } from "../../../services/homes.service";
import { HomeDevicesComponent } from "../home-devices/home-devices.component";
import { RoomFormComponent } from "../room-form/room-form.component";
import { MatDialog } from "@angular/material/dialog";
import { MatButtonModule } from "@angular/material/button";

@Component({
  selector: "app-home-detail",
  standalone: true,
  imports: [
    HomeSummaryComponent,
    MatTabsModule,
    HomeMembersComponent,
    HomeDevicesComponent,
    RoomFormComponent,
    MatButtonModule,
  ],
  templateUrl: "./home-detail.component.html",
  styleUrl: "./home-detail.component.css",
})
export class HomeDetailComponent {
  private _home!: ResponseGetHome;

  @Input()
  set home(value: ResponseGetHome) {
    this._home = value;
    if (this._home) {
      this.getHomeMembers();
      this.getHomeDevices();
    }
  }

  get home(): ResponseGetHome {
    return this._home;
  }

  members: ResponseGetHomeMember[] = [];
  devices: ResponseHomeDevice[] = [];
  rooms: ResponseRooms[] = [];

  constructor(private homeService: HomesService, private dialog: MatDialog) {}

  getHomeMembers() {
    this.homeService.getHomeMembers(this._home.id).subscribe(
      ({ data }) => (this.members = data?.members ?? []),
      ({ message }) => console.error(message)
    );
  }

  getHomeDevices() {
    this.homeService.getHomeDevices(this._home.id).subscribe(
      ({ data }) => (this.devices = data?.devices ?? []),
      ({ message }) => console.error(message)
    );
  }

  openCreateRoomDialog() {
    const dialogRef = this.dialog.open(RoomFormComponent, {
      width: '400px',
      data: { homeId: this._home.id }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Handle the result if needed
        console.log('Room created successfully!');
      }
    });
  }
  
  reloadMembers() {
    this.getHomeMembers();
  }
}
