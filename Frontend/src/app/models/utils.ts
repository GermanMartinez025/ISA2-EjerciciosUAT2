import { FormControl } from "@angular/forms";

export interface Option {
  value: string;
  label: string;
  icon: string;
}

export class Field {
  key: string;
  label: string;
  type: string;
  control: FormControl;
  show: Function;
  cols: number;
  rows: number;
  value: Function = () => this.control.value;
  action: Function = () => {};
  items: Option[] = [];

  constructor(
    key: string,
    label: string,
    type: string,
    control: FormControl,
    show?: Function,
    cols?: number,
    rows?: number,
    value?: Function,
    action?: Function,
    options?: Option[]
  ) {
    this.key = key;
    this.label = label;
    this.type = type;
    this.control = control;
    this.show = show ?? (() => true);
    this.cols = cols ?? 12;
    this.rows = rows ?? 1;
    this.value = value ?? (() => this.control.value);
    this.action = action ?? (() => {});
    this.items = options ?? [];
  }
}
