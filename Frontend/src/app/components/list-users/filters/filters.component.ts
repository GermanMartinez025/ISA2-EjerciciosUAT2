import { Component, EventEmitter, Input, Output } from "@angular/core";
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatSelectModule } from "@angular/material/select";
import { MatIconModule } from "@angular/material/icon";
import { Role } from "../../../models/role";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import { FilterUsers } from "../../../models/filters/filtersUser";
import {
  MatChipEditedEvent,
  MatChipInputEvent,
  MatChipsModule,
} from "@angular/material/chips";
import { COMMA, ENTER } from "@angular/cdk/keycodes";
import {
  MatButtonToggleChange,
  MatButtonToggleModule,
} from "@angular/material/button-toggle";
import { MatTooltipModule } from "@angular/material/tooltip";

@Component({
  selector: "app-filters",
  standalone: true,
  imports: [
    MatToolbarModule,
    MatFormFieldModule,
    MatSelectModule,
    MatIconModule,
    CommonModule,
    FormsModule,
    MatInputModule,
    MatButtonModule,
    MatChipsModule,
    MatButtonToggleModule,
    MatTooltipModule,
  ],
  templateUrl: "./filters.component.html",
  styleUrl: "./filters.component.css",
})
export class FiltersComponent {
  readonly addOnBlur = true;
  separatorKeysCodes = [ENTER, COMMA];

  constructor() {}
  @Input() filters: FilterUsers = new FilterUsers();
  @Output() apply = new EventEmitter();

  applyFilters() {
    this.apply.emit();
  }

  roles: Role[] = Role.Roles;

  getRole(role: string | null | undefined): Role | null {
    return this.roles.find((r) => r.value === role) ?? null;
  }

  addName($event: MatChipInputEvent) {
    const value = $event.value.trim();

    if (!value) {
      return;
    }

    this.filters.fullName.push(value);

    $event.chipInput!.clear();
  }
  removeName(item: string) {
    const index = this.filters.fullName.indexOf(item);
    if (index < 0) {
      return;
    }

    this.filters.fullName.splice(index, 1);
  }

  editName(item: string, $event: MatChipEditedEvent) {
    const value = $event.value.trim();

    if (!value) {
      this.removeName(item);
      return;
    }

    const index = this.filters.fullName.indexOf(item);
    this.filters.fullName[index] = value;
  }

  selectRoles($event: MatButtonToggleChange) {
    this.filters.roles = $event.value;
  }

  @Output() create = new EventEmitter();

  createUser() {
    this.create.emit();
  }
}
