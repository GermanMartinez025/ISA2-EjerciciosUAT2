import { CanActivateFn } from "@angular/router";
import { AuthenticationService } from "../services/authentication.service";
import { inject } from "@angular/core";

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthenticationService);

  if (authService.isUserLogged()) {
    return true;
  }

  return false;
};
