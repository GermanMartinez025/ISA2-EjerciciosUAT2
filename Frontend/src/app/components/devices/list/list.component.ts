import { Component, Input, ViewChild } from "@angular/core";
import {
  MatPaginator,
  MatPaginatorModule,
  PageEvent,
} from "@angular/material/paginator";
import { CommonModule } from "@angular/common";
import { FiltersDevice } from "../../../models/filters/filtersDevice";
import { ResponseDevice } from "../../../models/device";
import { DevicesService } from "../../../services/devices.service";
import { ItemComponent } from "../item/item.component";
import { FiltersComponent } from "../filters/filters.component";
import { MatDialog } from "@angular/material/dialog";
import { FormComponent } from "../form/form.component";

@Component({
  selector: "app-list",
  standalone: true,
  imports: [CommonModule, MatPaginatorModule, ItemComponent, FiltersComponent],
  templateUrl: "./list.component.html",
  styleUrl: "./list.component.css",
})
export class ListComponent {
  @Input() showCreateButton = false;
  @Input() showCompany = false;
  @Input() filters: FiltersDevice = new FiltersDevice();

  pageOptions = { actual: 1, size: 12 };
  totalDevices = 0;
  devices: ResponseDevice[] = [];

  constructor(
    private devicesService: DevicesService,
    private dialog: MatDialog
  ) {}
  ngOnInit() {
    this.searchDevices();
  }

  pageChange($event: PageEvent) {
    this.pageOptions.actual = $event.pageIndex + 1;
    this.pageOptions.size = $event.pageSize;
    this.searchDevices();
  }
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  applyFilters() {
    this.resetPagination();
    this.searchDevices();
  }

  private resetPagination() {
    this.paginator.firstPage();
  }

  searchDevices() {
    this.devicesService.getDevices(this.filters, this.pageOptions).subscribe({
      next: (response) => {
        this.devices = response.data?.devices ?? [];
        this.totalDevices = response.data?.totalDevices ?? 0;
      },
    });
  }

  createDevice() {
    const dialogRef = this.dialog.open(FormComponent, {
      width: "33%",
      maxWidth: "100%",
    });

    dialogRef.componentInstance.created.subscribe(() => {
      this.searchDevices();
    });
  }
}
