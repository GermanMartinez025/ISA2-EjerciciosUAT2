export class FilterUsers {
  public fullName: string[];
  public roles: string[];

  constructor(fullName?: string[], roles?: string[]) {
    this.fullName = fullName ?? [];
    this.roles = roles ?? [];
  }
}
