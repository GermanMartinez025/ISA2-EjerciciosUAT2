export class RequestCreateRoom {
    name: string;
    homeId: number;

    constructor(Name: string, HomeId: number) {
        this.name = Name;
        this.homeId = HomeId;
    }
}

export class ResponseRooms {
    name: string;
    homeId: number;

    constructor(Name: string, HomeId: number) {
        this.name = Name;
        this.homeId = HomeId;
    }

  }