import { Component, EventEmitter, inject, Input, Output } from "@angular/core";
import {
  ReactiveFormsModule,
  FormControl,
  FormGroup,
  Validators,
} from "@angular/forms";
import { MatInput } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import { MatCardModule } from "@angular/material/card";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatIconModule } from "@angular/material/icon";
import { CommonModule } from "@angular/common";
import { MatGridListModule } from "@angular/material/grid-list";
import { MatSelectModule } from "@angular/material/select";
import { Role } from "../../../models/role";
import { ResponseUser, UserCreate } from "../../../models/user";
import { UsersService } from "../../../services/users.service";
import {
  ActivatedRoute,
  NavigationEnd,
  Router,
  RouterModule,
} from "@angular/router";
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from "@angular/material/snack-bar";
import { RegisterDialogProfileImageComponent } from "../register-dialog-profile-image/register-dialog-profile-image.component";
import { MatDialog } from "@angular/material/dialog";
import { BehaviorSubject } from "rxjs";

@Component({
  selector: "app-register-user-form",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    CommonModule,
    MatFormFieldModule,
    MatInput,
    MatButtonModule,
    MatIconModule,
    MatGridListModule,
    MatSelectModule,
    RouterModule,
  ],
  templateUrl: "./register-user-form.component.html",
  styleUrls: ["./register-user-form.component.css"],
})
export class RegisterUserFormComponent {
  // Snackbar configuration
  private _snackBar = inject(MatSnackBar);
  horizontalPosition: MatSnackBarHorizontalPosition = "start";
  verticalPosition: MatSnackBarVerticalPosition = "bottom";
  duration = 5000;
  private snackBarConfig = {
    horizontalPosition: this.horizontalPosition,
    verticalPosition: this.verticalPosition,
    duration: this.duration,
  };

  // Reactive image property
  private imageSubject = new BehaviorSubject<string>("/assets/user.jpg");
  image$ = this.imageSubject.asObservable();

  // Input property
  @Input() title: string = "Register";

  // Output property
  @Output() create = new EventEmitter<ResponseUser>();

  // State
  roles: Role[] = [];
  currentRoute: string = "register";
  hidden: boolean = true;

  // Reactive form
  form = new FormGroup({
    firstName: new FormControl("", Validators.required),
    lastName: new FormControl("", Validators.required),
    email: new FormControl("", [Validators.required, Validators.email]),
    password: new FormControl("", Validators.required),
    role: new FormControl("", Validators.required),
    profilePicture: new FormControl("", Validators.pattern("https?://.+")),
  });

  constructor(
    private usersService: UsersService,
    private router: Router,
    private dialog: MatDialog
  ) {
    // React to changes in the profilePicture field
    this.form.get("profilePicture")?.valueChanges.subscribe((value) => {
      this.imageSubject.next(value || "/assets/user.jpg");
    });
  }

  // Lifecycle hooks
  ngOnInit() {
    this.subscribeToRoute();
    this.setRoles();
  }

  // Form logic
  submit() {
    if (this.form.valid) {
      const user = new UserCreate(
        this.form.get("firstName")?.value,
        this.form.get("lastName")?.value,
        this.form.get("email")?.value,
        this.form.get("password")?.value,
        this.form.get("role")?.value,
        this.form.get("profilePicture")?.value
      );

      this.usersService.createUser(user).subscribe((result) => {
        if (result.executionSuccessful) {
          if (this.currentRoute === "register") {
            this.router.navigate(["/login"]);
          } else {
            this.openSnackBar(result.message);
            if (result.data) this.create.emit(result.data);
          }
        } else {
          this.openSnackBar(result.message);
        }
      });
    }
  }

  // Dialog logic
  openDialogImage() {
    const dialogRef = this.dialog.open(RegisterDialogProfileImageComponent, {
      data: { oldUrl: this.form.get("profilePicture")?.value },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.saveImage(result);
      }
    });
  }

  saveImage(url: string) {
    this.form.get("profilePicture")?.setValue(url);
  }

  // Helper methods
  toggleHide() {
    this.hidden = !this.hidden;
  }

  getRole(role: string | null | undefined): Role | null {
    return this.roles.find((r) => r.value === role) ?? null;
  }

  openSnackBar(message: string) {
    this._snackBar.open(message, "Close", this.snackBarConfig);
  }

  subscribeToRoute() {
    this.currentRoute = this.router.url.split("/")[1];
  }

  setRoles() {
    if (this.currentRoute === "register") {
      this.roles.push(Role.HomeUser);
    } else if (this.currentRoute === "users") {
      this.roles.push(Role.Admin, Role.CompanyOwner);
    }

    if (this.roles.length === 1) {
      this.form.get("role")?.setValue(this.roles[0].value);
    }
  }
}
