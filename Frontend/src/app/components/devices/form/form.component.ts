import { Field } from "./../../../models/utils";
import {
  afterNextRender,
  Component,
  EventEmitter,
  inject,
  Injector,
  Output,
  ViewChild,
} from "@angular/core";
import {
  CreateDeviceRequest,
  DeviceType,
  deviceTypesOptions,
} from "../../../models/device";
import { DevicesService } from "../../../services/devices.service";
import { MatDialog } from "@angular/material/dialog";
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from "@angular/material/snack-bar";
import { BehaviorSubject } from "rxjs";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { RegisterDialogProfileImageComponent } from "../../register-form/register-dialog-profile-image/register-dialog-profile-image.component";
import { MatCardModule } from "@angular/material/card";
import { MatFormFieldModule } from "@angular/material/form-field";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule } from "@angular/forms";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import { MatGridListModule } from "@angular/material/grid-list";
import { MatButtonToggleModule } from "@angular/material/button-toggle";
import { MatIconModule } from "@angular/material/icon";
import { MatTooltipModule } from "@angular/material/tooltip";
import { MatSlideToggleModule } from "@angular/material/slide-toggle";
import { CdkTextareaAutosize } from "@angular/cdk/text-field";
import {
  MatChipEditedEvent,
  MatChipInputEvent,
  MatChipsModule,
} from "@angular/material/chips";
import { COMMA, ENTER } from "@angular/cdk/keycodes";
@Component({
  selector: "app-form",
  standalone: true,
  imports: [
    MatCardModule,
    MatFormFieldModule,
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatGridListModule,
    MatButtonToggleModule,
    MatIconModule,
    MatTooltipModule,
    MatSlideToggleModule,
    MatChipsModule,
  ],
  templateUrl: "./form.component.html",
  styleUrl: "./form.component.css",
})
export class FormComponent {
  constructor(
    private dialog: MatDialog,
    private devicesService: DevicesService,
    private _injector: Injector
  ) {
    this.form.patchValue(this.request);

    this.form.get("logoType")?.valueChanges.subscribe((value) => {
      this.imageSubject.next(value || "/assets/user.jpg");
    });
  }

  @Output() created = new EventEmitter();

  private _snackBar = inject(MatSnackBar);

  // Snackbar configuration
  horizontalPosition: MatSnackBarHorizontalPosition = "start";
  verticalPosition: MatSnackBarVerticalPosition = "bottom";
  duration = 5000;

  private snackBarConfig = {
    horizontalPosition: this.horizontalPosition,
    verticalPosition: this.verticalPosition,
    duration: this.duration,
  };

  private imageSubject = new BehaviorSubject<string>("/assets/user.jpg");
  image$ = this.imageSubject.asObservable();

  request: CreateDeviceRequest = {
    name: "",
    modelNumber: "",
    description: "",
    photos: [
    ],
    type: undefined,
    outsideEnvironment: false,
    movementDetection: false,
    personDetection: false,
    mainPhoto: "",
  };

  submit() {
    if (this.form.valid) {
      this.devicesService.create(this.request).subscribe({
        next: ({ message }) => {
          this.openSnackBar(message);
          this.created.emit();
        },
        error: ({ message }) => this.openSnackBar(message),
      });
    }
  }
  fields: Field[] = [
    new Field(
      "photos",
      "Photo",
      "image",
      new FormControl(this.request.photos, [
        Validators.required,
        Validators.pattern("https?://.+"),
      ]),
      undefined,
      4,
      2,
      () => this.request.mainPhoto,
      () => this.openDialogImage()
    ),
    new Field(
      "name",
      "Name",
      "input",
      new FormControl(this.request.name, Validators.required),
      undefined,
      8
    ),
    new Field(
      "modelNumber",
      "Model Number",
      "input",
      new FormControl(this.request.modelNumber, Validators.required),
      undefined,
      8
    ),
    new Field(
      "description",
      "Description",
      "textarea",
      new FormControl(this.request.description, Validators.required)
    ),
    new Field(
      "type",
      "Device Type",
      "buttonGroup",
      new FormControl<DeviceType | undefined>(
        this.request.type,
        Validators.required
      ),
      undefined,
      undefined,
      undefined,
      undefined,
      undefined,
      deviceTypesOptions
    ),
    new Field(
      "outsideEnvironment",
      "Outside Environment",
      "switch",
      new FormControl(this.request.outsideEnvironment),
      () => this.request.type === DeviceType.Camera,
      4
    ),
    new Field(
      "movementDetection",
      "Movement Detection",
      "switch",
      new FormControl(this.request.movementDetection),
      () => this.request.type === DeviceType.Camera,
      4
    ),
    new Field(
      "personDetection",
      "Person Detection",
      "switch",
      new FormControl(this.request.personDetection),
      () => this.request.type === DeviceType.Camera,
      4
    ),
    //chips photos
    new Field(
      "photos",
      "Photos",
      "chips",
      new FormControl(this.request.photos, [
        Validators.required,
        Validators.pattern("https?://.+"),
      ]),
      undefined,
      12,
      1,
      undefined,
      () => this.openDialogImage()
    ),
  ];
  fieldsToShow = () => this.fields.filter((field) => field.show());

  form = new FormGroup(
    this.fields.reduce((acc: { [key: string]: FormControl }, field) => {
      acc[field.key] = field.control;
      return acc;
    }, {})
  );

  openDialogImage() {
    const dialogRef = this.dialog.open(RegisterDialogProfileImageComponent, {
      data: { oldUrl: this.request.photos },
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.saveImage(result);
      }
    });
  }

  saveImage(url: string) {
    this.form.get("photos")?.setValue([...this.request.photos, url]);
    this.request.mainPhoto = url;
  }

  openSnackBar(message: string) {
    this._snackBar.open(message, "Close", this.snackBarConfig);
  }

  ngOnInit() {
    this.form.valueChanges.subscribe((formValues) => {
      this.request = {
        ...this.request,
        ...formValues,
      };
    });
  }

  @ViewChild("autosize") autosize!: CdkTextareaAutosize;

  triggerResize() {
    afterNextRender(
      () => {
        this.autosize.resizeToFitContent(true);
      },
      {
        injector: this._injector,
      }
    );
  }

  readonly addOnBlur = true;
  separatorKeysCodes = [ENTER, COMMA];

  addItem($event: MatChipInputEvent, type: string) {
    const key = type as keyof CreateDeviceRequest;
    const value = $event.value.trim();

    if (!value) {
      return;
    }

    if (Array.isArray(this.request[key])) {
      (this.request[key] as string[]).push(value);
    }

    $event.chipInput.clear();
  }
  removeItem(item: string, type: string) {
    const key = type as keyof CreateDeviceRequest;

    let filter = Array.isArray(this.request[key])
      ? (this.request[key] as string[])
      : [];

    const index = filter.indexOf(item);

    if (index < 0) {
      return;
    }

    if (this.request.mainPhoto === item) {
      this.request.mainPhoto = this.request.photos[0];
    }

    filter.splice(index, 1);
  }

  editItem(item: string, $event: MatChipEditedEvent, type: string) {
    const key = type as keyof CreateDeviceRequest;
    const value = $event.value.trim();

    if (!value) {
      this.removeItem(item, type);
      return;
    }

    let filter = Array.isArray(this.request[key])
      ? (this.request[key] as string[])
      : [];

    const index = filter.indexOf(item);
    filter[index] = value;
  }
}
