import { Component, EventEmitter, Input, Output } from "@angular/core";
import { MatCardModule } from "@angular/material/card";
import { ResponseUser } from "../../../models/user";
import { CommonModule } from "@angular/common";
import { MatIconModule } from "@angular/material/icon";
import { MatTooltipModule } from "@angular/material/tooltip";
import { MatButtonModule } from "@angular/material/button";
@Component({
  selector: "app-card",
  standalone: true,
  imports: [
    MatCardModule,
    CommonModule,
    MatIconModule,
    MatTooltipModule,
    MatButtonModule,
  ],
  templateUrl: "./card.component.html",
  styleUrl: "./card.component.css",
})
export class CardComponent {
  @Input() user!: ResponseUser;

  @Output() delete = new EventEmitter();

  deleteUser() {
    this.delete.emit();
  }

  constructor() {}
}
