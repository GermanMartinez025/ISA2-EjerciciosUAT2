import { Component, EventEmitter, inject, Output } from "@angular/core";
import {
  FormControl,
  FormGroup,
  FormsModule,
  Validators,
} from "@angular/forms";
import { CommonModule } from "@angular/common";
import { MatInputModule } from "@angular/material/input";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatCardModule } from "@angular/material/card";
import { ReactiveFormsModule } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatGridListModule } from "@angular/material/grid-list";
import { BehaviorSubject } from "rxjs";
import { MatDialog } from "@angular/material/dialog";
import { RegisterDialogProfileImageComponent } from "../../register-form/register-dialog-profile-image/register-dialog-profile-image.component";
import { CompaniesService } from "../../../services/companies.service";
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from "@angular/material/snack-bar";

@Component({
  selector: "app-company-form",
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    MatInputModule,
    MatFormFieldModule,
    MatCardModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatGridListModule,
  ],
  templateUrl: "./company-form.component.html",
  styleUrl: "./company-form.component.css",
})
export class CompanyFormComponent {
  @Output() created = new EventEmitter();

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

  private imageSubject = new BehaviorSubject<string>("/assets/user.jpg");
  image$ = this.imageSubject.asObservable();

  constructor(
    private dialog: MatDialog,
    private companiesService: CompaniesService
  ) {
    this.form.get("logoType")?.valueChanges.subscribe((value) => {
      this.imageSubject.next(value || "/assets/user.jpg");
    });
  }

  submit() {
    if (this.form.valid) {
      this.companiesService.createCompany(this.form.value).subscribe({
        next: ({ message }) => {
          this.openSnackBar(message);
          this.created.emit();
        },
        error: ({ message }) => this.openSnackBar(message),
      });
    }
  }

  form = new FormGroup({
    rut: new FormControl("", [Validators.required]),
    name: new FormControl("", Validators.required),
    logoType: new FormControl("", [
      Validators.required,
      Validators.pattern("https?://.+"),
    ]),
  });

  openDialogImage() {
    const dialogRef = this.dialog.open(RegisterDialogProfileImageComponent, {
      data: { oldUrl: this.form.get("logoType")?.value },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.saveImage(result);
      }
    });
  }

  saveImage(url: string) {
    this.form.get("logoType")?.setValue(url);
  }

  openSnackBar(message: string) {
    this._snackBar.open(message, "Close", this.snackBarConfig);
  }
}
