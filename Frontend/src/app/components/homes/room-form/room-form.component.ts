import { Field } from "./../../../models/utils";
import { Component, inject, Input, OnInit, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { RoomsService } from "../../../services/rooms.service";
import {
  FormControl,
  FormGroup,
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
import { RequestCreateRoom } from "../../../models/room";

@Component({
  selector: "app-room-form",
  standalone: true,
  templateUrl: "./room-form.component.html",
  styleUrls: ["./room-form.component.css"],
  imports: [MatCardModule, MatFormFieldModule, CommonModule, ReactiveFormsModule, MatInputModule, MatButtonModule, MatGridListModule],
})
export class RoomFormComponent implements OnInit {
  @Input() homeId!: number;

  private snackBar = inject(MatSnackBar);
  private dialogRef = inject(MatDialogRef<RoomFormComponent>);
  private roomService = inject(RoomsService);

  request: RequestCreateRoom = {
    homeId: 0,
    name: "",
  }

  constructor (@Inject(MAT_DIALOG_DATA) public data: { homeId: number }) {
    this.request.homeId = data.homeId;
    this.form.patchValue(this.request);
  }

  ngOnInit() {
    this.form.valueChanges.subscribe((formValues) => {
      this.request = {
        ...this.request,
        ...formValues,
      };
    });
  }

  submit() {
    if (this.form.valid) {
      var request = new RequestCreateRoom(this.request.name, this.request.homeId);

      this.roomService.create(request.homeId, request).subscribe({
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
}