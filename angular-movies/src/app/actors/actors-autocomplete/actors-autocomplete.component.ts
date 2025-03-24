import { Component, OnInit } from '@angular/core';
import { ActorAutoCompleteDTO } from '../actors.model';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import {MatTableModule} from '@angular/material/table';


@Component({
  selector: 'app-actors-autocomplete',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule,MatTableModule,FormsModule,MatIconModule,
    MatAutocompleteModule],
  templateUrl: './actors-autocomplete.component.html',
  styleUrl: './actors-autocomplete.component.css'
})
export class ActorsAutocompleteComponent implements OnInit {

actors:ActorAutoCompleteDTO[]=[
  {id: 1, name: 'Tom Holland', character: '', picture: 'https://upload.wikimedia.org/wikipedia/commons/thumb/3/3c/Tom_Holland_by_Gage_Skidmore.jpg/330px-Tom_Holland_by_Gage_Skidmore.jpg'},
    {id: 2, name: 'Tom Hanks', character: '', picture: 'https://upload.wikimedia.org/wikipedia/commons/thumb/a/a9/Tom_Hanks_TIFF_2019.jpg/220px-Tom_Hanks_TIFF_2019.jpg' },
    {id: 3, name: 'Samuel L. Jackson', character: '', picture: 'https://upload.wikimedia.org/wikipedia/commons/thumb/2/29/SamuelLJackson.jpg/250px-SamuelLJackson.jpg' }
]
actorsOriginal = this.actors;

actorsSelected:ActorAutoCompleteDTO[]=[];

control = new FormControl();
ngOnInit(): void {
  this.control.valueChanges.subscribe(value=>{
    this.actors = this.actorsOriginal;
    this.actors = this.actors.filter(actor=>actor.name.indexOf(value)!== -1);
  })
}
}
