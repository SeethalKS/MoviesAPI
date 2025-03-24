import { Component } from '@angular/core';
import { TheatreCreationDTO } from '../theaters.models';
import { TheatersFormComponent } from "../theaters-form/theaters-form.component";

@Component({
  selector: 'app-create-theater',
  imports: [TheatersFormComponent],
  templateUrl: './create-theater.component.html',
  styleUrl: './create-theater.component.css'
})
export class CreateTheaterComponent {

  saveChanges(theater:TheatreCreationDTO){
    console.log('creating the theater',theater);
  }
}
