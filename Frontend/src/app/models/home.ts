export class ResponseGetHome {
  id: number;
  name: string;
  mainStreet: string;
  doorNumber: number;
  latitude: number;
  longitude: number;
  maxMembers: number;
  owner: string;
  static schema = {
    id: {
      label: "Id",
    },
    name: {
      label: "Name",
    },
    mainStreet: {
      label: "Main Street",
    },
    doorNumber: {
      label: "Door Number",
    },
    latitude: {
      label: "Latitude",
    },
    longitude: {
      label: "Longitude",
    },
    maxMembers: {
      label: "Max Members",
    },
    owner: {
      label: "Owner",
    },
  };

  constructor(
    Id: number,
    Name: string,
    MainStreet: string,
    DoorNumber: number,
    Latitude: number,
    Longitude: number,
    MaxMembers: number,
    Owner: string
  ) {
    this.id = Id;
    this.name = Name;
    this.mainStreet = MainStreet;
    this.doorNumber = DoorNumber;
    this.latitude = Latitude;
    this.longitude = Longitude;
    this.maxMembers = MaxMembers;
    this.owner = Owner;
  }
}

export class ResponseGetHomes {
  homes: ResponseGetHome[];

  constructor(homes: ResponseGetHome[]) {
    this.homes = homes;
  }
}

export class ResponseGetHomeMember {
  firstName: string;
  lastName: string;
  email: string;
  profilePhoto: string;
  isNotifiable: boolean;
  listDevices: boolean;
  listUsers: boolean;
  static schema = {
    firstName: {
      label: "First Name",
    },
    lastName: {
      label: "Last Name",
    },
    email: {
      label: "Email",
    },
    profilePhoto: {
      label: "Profile Photo",
    },
    isNotifiable: {
      label: "Is Notifiable",
    },
    listDevices: {
      label: "List Devices",
    },
    listUsers: {
      label: "List Users",
    },
  };

  constructor(
    FirstName: string,
    LastName: string,
    Email: string,
    ProfilePhoto: string,
    IsNotifiable: boolean,
    ListDevices: boolean,
    ListUsers: boolean
  ) {
    this.firstName = FirstName;
    this.lastName = LastName;
    this.email = Email;
    this.profilePhoto = ProfilePhoto;
    this.isNotifiable = IsNotifiable;
    this.listDevices = ListDevices;
    this.listUsers = ListUsers;
  }
}

export class ResponseGetMembers {
  members: ResponseGetHomeMember[];

  constructor(members: ResponseGetHomeMember[]) {
    this.members = members;
  }
}
export class RequestCreateHome {
    name: string;
    address: Address;
    geolocation: Geolocation;
    maxMembers: number;
  
    constructor(
      name: string,
      address: Address,
      geolocation: Geolocation,
      maxMembers: number
    ) {
      this.name = name;
      this.address = address;
      this.geolocation = geolocation;
      this.maxMembers = maxMembers;
    }
}
  
  
  export class Address {
    mainStreet: string;
    doorNumber: number;
  
    constructor(mainStreet: string, doorNumber: number) {
      this.mainStreet = mainStreet;
      this.doorNumber = doorNumber;
    }
  }
  
  export class Geolocation {
    latitude: number;
    longitude: number;
  
    constructor(latitude: number, longitude: number) {
      this.latitude = latitude;
      this.longitude = longitude;
    }
  }


/**
 * ResponseHomeDevice
 *  public int Id { get; set; }
    public string Name { get; set; }
    public string ModelNumber { get; set; }
    public List<string> Photo { get; set; }
    public StateType State { get; set; }
    public int HomeId { get; set; }
 */

export class ResponseHomeDevice {
  id: number;
  name: string;
  modelNumber: string;
  photos: string[];
  state: StateType;
  homeId: number;
  mainPhoto: string;

  constructor(
    Id: number,
    Name: string,
    ModelNumber: string,
    Photos: string[],
    State: StateType,
    HomeId: number,
    MainPhoto: string
  ) {
    this.id = Id;
    this.name = Name;
    this.modelNumber = ModelNumber;
    this.photos = Photos;
    this.state = State;
    this.homeId = HomeId;
    this.mainPhoto = MainPhoto;
  }

  static schema = {
    id: {
      label: "Id",
    },
    name: {
      label: "Name",
    },
    modelNumber: {
      label: "Model Number",
    },
    photo: {
      label: "Photo",
    },
    state: {
      label: "State",
    },
    homeId: {
      label: "Home Id",
    },
    mainPhoto: {
      label: "Main Photo",
    },
  };
}

export enum StateType {
  Online = "Online",
  Offline = "Offline",
}

export class ResponseHomeDevices {
  devices: ResponseHomeDevice[];

  constructor(devices: ResponseHomeDevice[]) {
    this.devices = devices;
  }
}

export class ResponseAddUserToHome {
  email: string;
  homeId: number;

  constructor(Email: string, HomeId: number) {
    this.email = Email;
    this.homeId = HomeId;
  }
}
