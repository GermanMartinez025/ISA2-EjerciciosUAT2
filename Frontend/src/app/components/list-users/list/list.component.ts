import { Component, EventEmitter, Input, Output } from "@angular/core";
import { CardComponent } from "../card/card.component";
import { ResponseUser } from "../../../models/user";
import { CommonModule } from "@angular/common";

@Component({
  selector: "app-list",
  standalone: true,
  imports: [CardComponent, CommonModule],
  templateUrl: "./list.component.html",
  styleUrl: "./list.component.css",
})
export class ListComponent {
  @Input() users!: ResponseUser[];
  @Output() deleteUser = new EventEmitter<ResponseUser>();

  delete(user: ResponseUser) {
    this.deleteUser.emit(user);
  }
}
