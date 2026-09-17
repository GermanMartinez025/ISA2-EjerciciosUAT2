import { Component, inject } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterOutlet } from "@angular/router";
import { SidebarComponent } from "./components/shared/sidebar/sidebar.component";
import { AuthenticationService } from "./services/authentication.service";

@Component({
  selector: "app-root",
  standalone: true,
  imports: [CommonModule, RouterOutlet, SidebarComponent],
  templateUrl: "./app.component.html",
  styleUrl: "./app.component.css",
})
export class AppComponent {
  title = "Frontend";
  private authService = inject(AuthenticationService);
  
  isLoggedIn = false;
  
  ngOnInit(): void {
    this.authService.checkLoginStatus();
    this.authService.isLoggedIn$.subscribe((status) => {
      this.isLoggedIn = status;
    });
  }
}
