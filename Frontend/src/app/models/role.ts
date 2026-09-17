export class Role {
  public name: string;
  public value: string;
  public description: string;
  public icon: string;

  private constructor(
    name: string,
    value: string,
    description: string,
    icon: string
  ) {
    this.name = name;
    this.value = value;
    this.description = description;
    this.icon = icon;
  }

  public static Admin = new Role(
    "Admin",
    "Admin",
    "This user can manage all users",
    "shield person"
  );
  public static CompanyOwner = new Role(
    "Company Owner",
    "CompanyOwner",
    "This user can manage his company",
    "star"
  );
  public static HomeUser = new Role(
    "Home User",
    "HomeUser",
    "This user can create and manage their own homes and devices",
    "person"
  );

  public static Roles = [Role.Admin, Role.CompanyOwner, Role.HomeUser];
}
