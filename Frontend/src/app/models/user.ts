import { Role } from "./role";

interface IUser {
  Firstname: string;
  Lastname: string;
  Email: string;
  Password: string;
  ProfilePhoto?: string;
}

export class User implements IUser {
  Firstname: string;
  Lastname: string;
  Email: string;
  Password: string;
  ProfilePhoto?: string | undefined;
  public Roles: Role[];

  constructor(
    firstname: string,
    lastname: string,
    email: string,
    password: string,
    role: string[]
  ) {
    this.Firstname = firstname;
    this.Lastname = lastname;
    this.Email = email;
    this.Password = password;
    this.Roles = Role.Roles.filter((r) => role.includes(r.value));
  }
}

export class UserLogin {
  Email: string;
  Password: string;

  constructor(email: string, password: string) {
    this.Email = email;
    this.Password = password;
  }
}

export class UserCreate implements IUser {
  Firstname: string;
  Lastname: string;
  Email: string;
  Password: string;
  ProfilePhoto?: string;
  Role: string;

  constructor(
    firstname: string | undefined | null,
    lastname: string | undefined | null,
    email: string | undefined | null,
    password: string | undefined | null,
    role: string | undefined | null,
    profilePhoto: string | undefined | null
  ) {
    this.Firstname = firstname || "";
    this.Lastname = lastname || "";
    this.Email = email || "";
    this.Password = password || "";
    this.Role = role || "";
    this.ProfilePhoto = profilePhoto || undefined;
  }
}


export class ResponseUser {
  id: number;
  fullName: string;
  email: string;
  profilePhoto?: string | undefined;
  roles: string[];
  creationDate: string;

  constructor(
    id: number,
    fullName: string,
    email: string,
    Roles: string[],
    creationDate: string,
    profilePhoto?: string
  ) {
    this.id = id;
    this.fullName = fullName;
    this.email = email;
    this.roles = Roles;
    this.creationDate = creationDate;
    this.profilePhoto = profilePhoto || "/assets/user.jpg";
  }

  getRoles(): Role[] {
    return this.roles
      ? Role.Roles.filter((r) => this.roles.includes(r.value))
      : [];
  }
}

export interface GetUsersResponse {
  users: ResponseUser[];
  totalUsers: number;
  actualPage: number;
  totalPages: number;
}
