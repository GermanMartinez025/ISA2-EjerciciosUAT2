import { Component, EventEmitter, Input, Output } from "@angular/core";
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatSelectModule } from "@angular/material/select";
import { MatIconModule } from "@angular/material/icon";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
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
import { FiltersDevice } from "../../../models/filters/filtersDevice";
import { DevicesService } from "../../../services/devices.service";

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
  deviceTypes: string[] = [];

  @Input() filters: FiltersDevice = new FiltersDevice();
  @Input() showCreateButton = false;
  @Input() showCompanyFilter = false;
  @Output() apply = new EventEmitter();
  @Output() create = new EventEmitter();

  constructor(private devicesService: DevicesService) {
    this.devicesService.getSupportedDevices().subscribe((deviceTypes) => {
      this.deviceTypes = deviceTypes;
    });
  }

  selectDeviceType($event: MatButtonToggleChange) {
    this.filters.deviceType = $event.value;
  }
  createDevice() {
    this.create.emit();
  }

  getDeviceTypeIcon(deviceType: string): string {
    return this.devicesService.deviceIconsMap[deviceType];
  }

  getDeviceTypeTooltip(deviceType: string): string {
    return this.devicesService.deviceLabelsMap[deviceType];
  }

  readonly addOnBlur = true;
  separatorKeysCodes = [ENTER, COMMA];

  addItem($event: MatChipInputEvent, type: keyof FiltersDevice) {
    const value = $event.value.trim();

    if (!value) {
      return;
    }

    if (Array.isArray(this.filters[type])) {
      (this.filters[type] as string[]).push(value);
    }

    $event.chipInput.clear();
  }
  removeItem(item: string, type: keyof FiltersDevice) {
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
    type: keyof FiltersDevice
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
      value: "companyName" as keyof FiltersDevice,
      placeholder: "Company",
      name: "Companies",
      show: () => this.showCompanyFilter,
    },
    {
      value: "deviceName" as keyof FiltersDevice,
      placeholder: "Device",
      name: "Devices",
      show: () => true,
    },
    {
      value: "deviceModel" as keyof FiltersDevice,
      placeholder: "Model",
      name: "Models",
      show: () => true,
    },
  ];

  filtersMultiSelectToShow = () =>
    this.filtersMultiSelect.filter((f) => f.show());
}
