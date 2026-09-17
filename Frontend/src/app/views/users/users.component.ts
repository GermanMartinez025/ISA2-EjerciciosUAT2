import { Component, inject, ViewChild } from "@angular/core";
import {
  MatPaginator,
  MatPaginatorModule,
  PageEvent,
} from "@angular/material/paginator";
import { MatDialog } from "@angular/material/dialog";
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from "@angular/material/snack-bar";
import { ListComponent } from "../../components/list-users/list/list.component";
import { FiltersComponent } from "../../components/list-users/filters/filters.component";
import { RegisterUserFormComponent } from "../../components/register-form/register-user-form/register-user-form.component";
import { ResponseUser } from "../../models/user";
import { FilterUsers } from "../../models/filters/filtersUser";
import { UsersService } from "../../services/users.service";

@Component({
  selector: "app-users",
  standalone: true,
  imports: [MatPaginatorModule, ListComponent, FiltersComponent],
  templateUrl: "./users.component.html",
  styleUrl: "./users.component.css",
})
export class UsersComponent {
  filters = new FilterUsers();
  pageOptions = { actual: 1, size: 10 };
  totalUsers = 0;
  users: ResponseUser[] = [];
  private _snackBar = inject(MatSnackBar);
  readonly dialog = inject(MatDialog);
  dialogs = { create: RegisterUserFormComponent };
  private snackBarConfig = {
    horizontalPosition: "start" as MatSnackBarHorizontalPosition,
    verticalPosition: "bottom" as MatSnackBarVerticalPosition,
    duration: 5000,
  };

  constructor(private usersService: UsersService) {}

  ngOnInit() {
    this.searchUsers();
  }

  filterChange() {
    this.resetPagination();
    this.searchUsers();
  }

  pageChange($event: PageEvent) {
    this.pageOptions.actual = $event.pageIndex + 1;
    this.pageOptions.size = $event.pageSize;
    this.searchUsers();
  }

  searchUsers() {
    this.usersService.getUsers(this.filters, this.pageOptions).subscribe({
      next: (response) => {
        this.users = response.data?.users ?? [];
        this.totalUsers = response.data?.totalUsers ?? 0;
      },
      error: (error) => this.openSnackBar(`Error: ${error.message}`),
    });
  }

  openDialog(dialog: keyof typeof this.dialogs): void {
    const dialogToOpen = this.dialogs[dialog];
    const dialogRef = this.dialog.open(dialogToOpen, { width: "900%" });

    dialogRef.componentInstance.create.subscribe(
      (user: ResponseUser | undefined) => {
        if (user) {
          this.searchUsers();
          dialogRef.close();
        }
      }
    );
  }

  deleteUser($event: ResponseUser) {
    this.usersService.deleteUser($event).subscribe({
      next: ({ message, executionSuccessful }) => {
        this.openSnackBar(message);
        if (executionSuccessful) this.searchUsers();
      },
      error: (error) => this.openSnackBar(`Error: ${error.message}`),
    });
  }

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  private resetPagination() {
    this.paginator.firstPage();
  }

  openSnackBar(message: string) {
    this._snackBar.open(message, "Close", this.snackBarConfig);
  }
}
