export class GenericResponse<T> {
  public executionSuccessful: boolean;
  public data?: T;
  public message: string;

  constructor(executionSuccessful: boolean, data: T, message: string) {
    this.executionSuccessful = executionSuccessful;
    this.data = data;
    this.message = message;
  }
}
