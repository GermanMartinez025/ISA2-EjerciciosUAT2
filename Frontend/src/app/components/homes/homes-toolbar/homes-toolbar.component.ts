import { Component, EventEmitter, Input, Output } from "@angular/core";
import { ResponseGetHome } from "../../../models/home";
import { MatIconModule } from "@angular/material/icon";
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatButtonModule } from "@angular/material/button";
import { FormControl, FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatAutocompleteModule } from "@angular/material/autocomplete";
import { MatInputModule } from "@angular/material/input";
import { MatFormFieldModule } from "@angular/material/form-field";
import { HomeFormComponent } from "../../homes/home-form/home-form.component";
import { MatDialog } from "@angular/material/dialog";


@Component({
  selector: "app-homes-toolbar",
  standalone: true,
  imports: [
    MatIconModule,
    MatToolbarModule,
    MatButtonModule,
    FormsModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    MatInputModule,
    MatFormFieldModule,
  ],
  templateUrl: "./homes-toolbar.component.html",
  styleUrl: "./homes-toolbar.component.css",
})
export class HomesToolbarComponent {
  constructor (private dialog: MatDialog) {}
  createHome() {
    const dialogRef = this.dialog.open(HomeFormComponent, {
      width: "500px",
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log("Home created successfully.");
      }
    });
  }

  @Input() homes: ResponseGetHome[] = [];
  @Output() homeSelected = new EventEmitter<ResponseGetHome>();

  formControl = new FormControl();

  homeToDisplay(home: ResponseGetHome) {
    return home ? `${home.name} - ${home.mainStreet} ${home.doorNumber}` : "";
  }
}
