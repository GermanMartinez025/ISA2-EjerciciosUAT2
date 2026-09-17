import { Component, EventEmitter, inject, Input, Output } from "@angular/core";
import {
  FormControl,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from "@angular/forms";
import { CommonModule } from "@angular/common";
import { MatButtonModule } from "@angular/material/button";
import { MatDialogModule, MatDialogRef } from "@angular/material/dialog";
import { MatInput } from "@angular/material/input";
import { MatFormFieldModule } from "@angular/material/form-field";

@Component({
  selector: "app-register-dialog-profile-image",
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatDialogModule,
    MatInput,
    MatFormFieldModule,
    ReactiveFormsModule,
  ],
  templateUrl: "./register-dialog-profile-image.component.html",
  styleUrls: ["./register-dialog-profile-image.component.css"],
})
export class RegisterDialogProfileImageComponent {
  // Inputs
  @Input() oldUrl: string | null | undefined;

  // Outputs
  @Output() urlChange = new EventEmitter<string>();

  // Dialog reference
  readonly dialogRef = inject(
    MatDialogRef<RegisterDialogProfileImageComponent>
  );

  // Reactive form
  form = new FormGroup({
    url: new FormControl("", [
      Validators.pattern(
        /^(?:https?:\/\/.*\.(?:png|jpg|jpeg|gif|bmp|webp|svg))?$/i
      ),
    ]),
  });

  constructor() {
    // Initialize form with old URL if available
    this.form.get("url")?.setValue(this.oldUrl || "");
  }

  // Methods
  saveImage() {
    const url = this.form.get("url")?.value || "";
    this.urlChange.emit(url);
    this.dialogRef.close(url);
  }

  closeDialog() {
    this.dialogRef.close();
  }
}
