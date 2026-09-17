import { Component, Inject, Input } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { HomesService } from "../../../services/homes.service";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatCardModule } from "@angular/material/card";
import { ReactiveFormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";

@Component({
  selector: "app-home-member-add",
  standalone: true,
  imports: [
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    MatCardModule,
    ReactiveFormsModule,
    CommonModule,
  ],
  templateUrl: "./home-member-add.component.html",
  styleUrl: "./home-member-add.component.css",
})
export class HomeMemberAddComponent {
  form = new FormGroup({
    email: new FormControl("", [Validators.required, Validators.email]),
  });
  constructor(
    private dialogRef: MatDialogRef<HomeMemberAddComponent>,
    private homeService: HomesService,
    @Inject(MAT_DIALOG_DATA) public data: { homeId: number }
  ) {}

  addMember() {
    if (this.form.valid) {
      const homeId = this.data.homeId;
      const email = this.form.get("email")?.value as string;
      this.homeService.addHomeMember(homeId, email).subscribe(
        () => this.dialogRef.close(true),
        ({ message }) => console.error(message)
      );
    }
  }
}
