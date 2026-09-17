import { Component, inject } from "@angular/core";
import { CommonModule } from "@angular/common";
import {
  ReactiveFormsModule,
  FormControl,
  FormGroup,
  Validators,
} from "@angular/forms";
import { RouterModule, Router } from "@angular/router";

import { MatButtonModule } from "@angular/material/button";
import { MatCardModule } from "@angular/material/card";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatIconModule } from "@angular/material/icon";
import { MatInput } from "@angular/material/input";
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from "@angular/material/snack-bar";

import { AuthenticationService } from "../../services/authentication.service";

@Component({
  selector: "login-form",
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInput,
    MatButtonModule,
    MatIconModule,
    RouterModule,
  ],
  templateUrl: "./login-form.component.html",
  styleUrls: ["./login-form.component.css"],
})
export class LoginFormComponent {
  // Services
  private authService = inject(AuthenticationService);
  private router = inject(Router);
  private _snackBar = inject(MatSnackBar);

  // Snackbar configuration
  horizontalPosition: MatSnackBarHorizontalPosition = "start";
  verticalPosition: MatSnackBarVerticalPosition = "bottom";
  duration = 5000;

  private snackBarConfig = {
    horizontalPosition: this.horizontalPosition,
    verticalPosition: this.verticalPosition,
    duration: this.duration,
  };

  // Form group
  form = new FormGroup({
    email: new FormControl("", [Validators.email, Validators.required]),
    password: new FormControl("", Validators.required),
  });

  // State
  message = "";
  hidden = true;

  // Methods
  submit() {
    if (this.form.valid) {
      const email = this.form.value.email ?? "";
      const password = this.form.value.password ?? "";

      this.authService.authenticate(email, password).subscribe(
        () => {
          // Handle success logic here if needed
        },
        (error) => {
          const { executionSuccessful, message } = error.error;

          if (!executionSuccessful) {
            this.message = message;
          } else {
            console.error("Error", error);
            this.message = "An error occurred. Please try again later.";
          }

          this.openSnackBar();
        }
      );
    }
  }

  openSnackBar() {
    this._snackBar.open(this.message, "Close", this.snackBarConfig);
  }

  toggleHide() {
    this.hidden = !this.hidden;
  }
}
