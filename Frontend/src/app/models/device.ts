import { Option } from "./utils";
export class ResponseDevice {
  public id: number;
  public name: string;
  public model: string;
  public description: string;
  public photos: string[];
  public companyName: string;
  public deviceType: string;
  public mainPhoto: string;

  constructor(
    id: number,
    name: string,
    modelNumber: string,
    description: string,
    photos: string[],
    companyName: string,
    deviceType: string,
    mainPhoto: string
  ) {
    this.id = id;
    this.name = name;
    this.model = modelNumber;
    this.description = description;
    this.photos = photos;
    this.companyName = companyName;
    this.deviceType = deviceType;
    this.mainPhoto = mainPhoto;
  }
}

export interface GetDevicesResponse {
  devices: ResponseDevice[];
  totalDevices: number;
  actualPage: number;
  totalPages: number;
}

export class CreateDeviceRequest {
  public name: string;
  public modelNumber: string;
  public description: string;
  public photos: string[];
  public type?: DeviceType;
  public outsideEnvironment?: boolean;
  public movementDetection?: boolean;
  public personDetection?: boolean;
  public mainPhoto?: string;

  constructor(
    name: string,
    modelNumber: string,
    description: string,
    photos: string | string[],
    type: string,
    mainPhoto: string | undefined,
    outsideEnvironment: boolean,
    movementDetection: boolean,
    personDetection: boolean
  ) {
    this.name = name;
    this.modelNumber = modelNumber;
    this.description = description;
    this.photos = Array.isArray(photos) ? photos : [photos];
    this.type = type as DeviceType;
    this.mainPhoto = mainPhoto || this.photos[0];
    this.outsideEnvironment = outsideEnvironment;
    this.movementDetection = movementDetection;
    this.personDetection = personDetection;
  }
}

export enum DeviceType {
  Camera = "Camera",
  Sensor = "Sensor",
  MotionSensor = "Motion Sensor",
  SmartLamp = "Smart Lamp",
}

export class DeviceTypeOption {
  public icon: string;
  public label: string;

  constructor(icon: string, label: string) {
    this.icon = icon;
    this.label = label;
  }
}

export const deviceTypes = {
  Camera: new DeviceTypeOption("photo_camera", "Camera"),
  Sensor: new DeviceTypeOption("window", "Sensor"),
  MotionSensor: new DeviceTypeOption("sensors", "Motion Sensor"),
  SmartLamp: new DeviceTypeOption("lightbulb", "Smart Lamp"),
};

export const deviceTypesOptions: Option[] = Object.entries(deviceTypes).map(
  ([key, value]) => ({
    value: key,
    label: value.label,
    icon: value.icon,
  })
);
