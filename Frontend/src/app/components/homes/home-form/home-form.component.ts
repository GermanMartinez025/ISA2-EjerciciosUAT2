import { Field } from "./../../../models/utils";
import { Component, inject } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { HomesService } from "../../../services/homes.service";
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
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from "@angular/material/snack-bar";
import { RequestCreateHome } from "../../../models/home";


@Component({
  selector: "app-home-form",
  standalone: true,
  templateUrl: "./home-form.component.html",
  styleUrls: ["./home-form.component.css"],
  imports: [MatCardModule, MatFormFieldModule, CommonModule, ReactiveFormsModule, MatInputModule, MatButtonModule, MatGridListModule],
})
export class HomeFormComponent {
  private snackBar = inject(MatSnackBar);
  private dialogRef = inject(MatDialogRef<HomeFormComponent>);
  private homesService = inject(HomesService);

  request: RequestCreateHome = {
    name: "",
    address:
    {
      mainStreet: "",
      doorNumber: 0,
    },
    geolocation:
    {
      latitude: 0,
      longitude: 0,
    },
    maxMembers: 1,
  }

  constructor () {
    this.form.patchValue(this.request);
  }


  submit() {
    if (this.form.valid) {
      var request = new RequestCreateHome(this.request.name, this.request.address, this.request.geolocation, this.request.maxMembers);

      this.homesService.create(request).subscribe({
        next: ({ message }) => {
          this.openSnackBar(message);
          //this.created.emit();
        },
        error: ({ message }) => this.openSnackBar(message),
      });
    }
  }

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

  openSnackBar(message: string) {
    this._snackBar.open(message, "Close", this.snackBarConfig);
  }


  fields: Field[] = [
    new Field(
      "name",
      "Name",
      "input",
      new FormControl(this.request.name, Validators.required),
      undefined,
      12
    ),
    new Field(
      "doorNumber",
      "Door Number",
      "input",
      new FormControl(this.request.address.doorNumber, Validators.required),
      undefined,
      4
    ),
    new Field(
      "mainStreet",
      "Main Street",
      "input",
      new FormControl(this.request.address.mainStreet, Validators.required),
      undefined,
      8
    ),
    new Field(
      "latitude",
      "Latitude",
      "input",
      new FormControl(this.request.geolocation.latitude, Validators.required),
      undefined,
      6
    ),
    new Field(
      "longitude",
      "Longitude",
      "input",
      new FormControl(this.request.geolocation.longitude, Validators.required),
      undefined,
      6
    ),
    new Field(
      "maxMembers",
      "Max Members",
      "input",
      new FormControl(this.request.maxMembers, Validators.required),
      undefined,
      12
    ),
  ];

  form = new FormGroup(
    this.fields.reduce((acc: { [key: string]: FormControl }, field) => {
      acc[field.key] = field.control;
      return acc;
    }, {})
  );

  cancel() {
    this.dialogRef.close(false);
  }

  ngOnInit() {
    this.form.valueChanges.subscribe((formValues) => {
      const { mainStreet, doorNumber, latitude, longitude, ...rest } =
        formValues;
      this.request = {
        ...this.request,
        geolocation: {
          latitude: latitude,
          longitude: longitude,
        },
        address: {
          mainStreet: mainStreet,
          doorNumber: doorNumber,
        },
        ...rest,
      };
    });
  }
}
