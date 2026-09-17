import { Routes } from "@angular/router";
import { authGuard } from "./guards/auth.guard";
import { loginGuard } from "./guards/login.guard";
import { Role } from "./models/role";
import { LoginComponent } from "./views/login/login.component";
import { HomeComponent } from "./views/home/home.component";
import { RegisterComponent } from "./views/register/register.component";
import { UsersComponent } from "./views/users/users.component";
import { MyCompanyComponent } from "./views/my-company/my-company.component";
import { DeviesComponent } from "./views/devies/devies.component";
import { CompaniesComponent } from "./views/companies/companies.component";
import { HomesComponent } from "./views/homes/homes.component";

export const routes: Routes = [
  { path: "login", component: LoginComponent, canActivate: [loginGuard] },
  { path: "register", component: RegisterComponent, canActivate: [loginGuard] },
  { path: "home", component: HomeComponent, canActivate: [authGuard] },
  { path: "users", component: UsersComponent, data: { roles: [Role.Admin] } },
  {
    path: "my-company",
    component: MyCompanyComponent,
    data: { roles: [Role.CompanyOwner] },
  },
  { path: "devices", component: DeviesComponent, canActivate: [authGuard] },
  {
    path: "companies",
    component: CompaniesComponent,
    canActivate: [authGuard],
  },
  { path: "homes", component: HomesComponent, canActivate: [authGuard] },
  { path: "", redirectTo: "/login", pathMatch: "full" },
  { path: "**", redirectTo: "/login" },
];
