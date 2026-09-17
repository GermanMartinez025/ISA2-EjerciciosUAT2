import { Component, EventEmitter, Input, Output } from "@angular/core";
import { CommonModule } from "@angular/common";
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatIconModule } from "@angular/material/icon";
import { FormsModule } from "@angular/forms";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import {
  MatChipEditedEvent,
  MatChipInputEvent,
  MatChipsModule,
} from "@angular/material/chips";
import { COMMA, ENTER } from "@angular/cdk/keycodes";
import { FiltersCompanies } from "../../../models/filters/filtersCompanies";

@Component({
  selector: "app-filters",
  standalone: true,
  imports: [
    CommonModule,
    MatToolbarModule,
    MatFormFieldModule,
    MatIconModule,
    FormsModule,
    MatInputModule,
    MatButtonModule,
    MatChipsModule,
  ],
  templateUrl: "./filters.component.html",
  styleUrl: "./filters.component.css",
})
export class FiltersComponent {
  @Input() filters: FiltersCompanies = new FiltersCompanies();
  @Output() apply = new EventEmitter();

  readonly addOnBlur = true;
  separatorKeysCodes = [ENTER, COMMA];

  addItem($event: MatChipInputEvent, type: keyof FiltersCompanies) {
    const value = $event.value.trim();

    if (!value) {
      return;
    }

    if (Array.isArray(this.filters[type])) {
      (this.filters[type] as string[]).push(value);
    }

    $event.chipInput.clear();
  }
  removeItem(item: string, type: keyof FiltersCompanies) {
    let filter = Array.isArray(this.filters[type])
      ? (this.filters[type] as string[])
      : [];

    const index = filter.indexOf(item);

    if (index < 0) {
      return;
    }

    filter.splice(index, 1);
  }

  editItem(
    item: string,
    $event: MatChipEditedEvent,
    type: keyof FiltersCompanies
  ) {
    const value = $event.value.trim();

    if (!value) {
      this.removeItem(item, type);
      return;
    }

    let filter = Array.isArray(this.filters[type])
      ? (this.filters[type] as string[])
      : [];

    const index = filter.indexOf(item);
    filter[index] = value;
  }

  filtersMultiSelect = [
    {
      value: "companyNames" as keyof FiltersCompanies,
      placeholder: "Companys",
      name: "Companies",
      show: () => true,
    },
    {
      value: "ownerFullNames" as keyof FiltersCompanies,
      placeholder: "Owners",
      name: "Owners",
      show: () => true,
    },
  ];

  filtersMultiSelectToShow = () =>
    this.filtersMultiSelect.filter((filter) => filter.show());
}
