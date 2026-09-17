import { Component } from "@angular/core";
import { ResponseGetHome } from "../../models/home";
import { HomesService } from "../../services/homes.service";
import { CommonModule } from "@angular/common";
import { MatButtonModule } from "@angular/material/button";
import { UsersService } from "./../../services/users.service";
import { HomeDetailComponent } from "./../../components/homes/home-detail/home-detail.component";
import { HomesToolbarComponent } from "../../components/homes/homes-toolbar/homes-toolbar.component";
@Component({
  selector: "app-homes",
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    HomeDetailComponent,
    HomesToolbarComponent,
  ],
  templateUrl: "./homes.component.html",
  styleUrl: "./homes.component.css",
})
export class HomesComponent {
  constructor(
    private homeService: HomesService,
    private UsersService: UsersService
  ) {}

  homes: ResponseGetHome[] = [];
  selectedHome: ResponseGetHome | null = null;
  isHomeUser = true;

  ngOnInit() {
    this.getHomes();
  }

  getHomes() {
    this.homeService.getHomes().subscribe(
      ({ data }) => (this.homes = data?.homes ?? []),
      ({ message }) => {
        if (message == "Unauthorized") this.isHomeUser = false;
      }
    );
  }

  convertToHomeUser() {
    this.UsersService.addAsHomeUser().subscribe(
      () => (this.isHomeUser = true),
      ({ message }) => console.error(message)
    );
  }
}
