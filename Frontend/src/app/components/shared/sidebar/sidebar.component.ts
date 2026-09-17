import { Component, inject } from "@angular/core";
import { CommonModule } from "@angular/common";
import { MatDrawerMode, MatSidenavModule } from "@angular/material/sidenav";
import { NavigationEnd, Router, RouterModule } from "@angular/router";
import { ToolbarComponent } from "../toolbar/toolbar.component";
import { MatListModule } from "@angular/material/list";
import { MatDividerModule } from "@angular/material/divider";
import { MatIconModule } from "@angular/material/icon";

@Component({
  selector: "app-sidebar",
  standalone: true,
  imports: [
    MatSidenavModule,
    CommonModule,
    RouterModule,
    ToolbarComponent,
    MatListModule,
    MatDividerModule,
    MatIconModule,
  ],
  templateUrl: "./sidebar.component.html",
  styleUrl: "./sidebar.component.css",
})
export class SidebarComponent {
  opened = false;
  mode = "over" as MatDrawerMode;
  currentPath: string = "";

  toggleSidebar() {
    this.opened = !this.opened;
  }
  openedChange($event: boolean) {
    this.opened = $event;
  }

  sections = [
    {
      title: "Users",
      items: [{ name: "Users", path: "/users", icon: "people" }],
    },
    {
      title: "Company",
      items: [
        { name: "Companies", path: "/companies", icon: "business" },
        { name: "My Company", path: "/my-company", icon: "business" },
      ],
    },
    {
      title: "Devices",
      items: [{ name: "Devices", path: "/devices", icon: "devices" }],
    },
    {
      title: "Homes",
      items: [{ name: "Homes", path: "/homes", icon: "home" }],
    },
  ];

  subscribeToRoute() {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.currentPath = event.url;
      }
    });
  }

  constructor(private router: Router) {
    this.subscribeToRoute();
  }
}
