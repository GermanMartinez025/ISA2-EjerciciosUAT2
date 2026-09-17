import { Component, EventEmitter, inject, Input, Output } from "@angular/core";
import { ResponseGetHomeMember } from "../../../models/home";
import { HomeMemberItemComponent } from "../home-member-item/home-member-item.component";
import { CommonModule } from "@angular/common";
import { MatButtonModule } from "@angular/material/button";
import { MatIconModule } from "@angular/material/icon";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { HomeMemberAddComponent } from "../home-member-add/home-member-add.component";
@Component({
  selector: "app-home-members",
  standalone: true,
  imports: [
    HomeMemberItemComponent,
    CommonModule,
    MatButtonModule,
    MatIconModule,
  ],
  templateUrl: "./home-members.component.html",
  styleUrl: "./home-members.component.css",
})
export class HomeMembersComponent {
  @Input() homeId!: number;
  @Input() members: ResponseGetHomeMember[] = [];
  @Output() reloadMembers = new EventEmitter<void>();

  dialog = inject(MatDialog);

  addMember() {
    const dialogRef = this.dialog.open(HomeMemberAddComponent, {
      data: { homeId: this.homeId },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.reloadMembers.emit();
      }
    });
  }
}
