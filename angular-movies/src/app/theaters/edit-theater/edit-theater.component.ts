import { Component, Input, numberAttribute } from '@angular/core';
import { TheatreCreationDTO, TheatreDTO } from '../theaters.models';
import { TheatersFormComponent } from "../theaters-form/theaters-form.component";

@Component({
  selector: 'app-edit-theater',
  imports: [TheatersFormComponent],
  templateUrl: './edit-theater.component.html',
  styleUrl: './edit-theater.component.css'
})
export class EditTheaterComponent {

    @Input({transform: numberAttribute})
    id!:number;
    

    model:TheatreDTO = {name:'Acropolis',id:1,latitude:25.242144255536182,longitude: 51.44236570481855};

    saveChanges(theater:TheatreCreationDTO){
      console.log('editing the theater',theater)
    }
}
