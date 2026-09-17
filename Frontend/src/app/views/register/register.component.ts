import { Component } from "@angular/core";
import { RegisterUserFormComponent } from "../../components/register-form/register-user-form/register-user-form.component";

@Component({
  selector: "app-register",
  standalone: true,
  imports: [RegisterUserFormComponent],
  templateUrl: "./register.component.html",
  styleUrl: "./register.component.css",
})
export class RegisterComponent {}
