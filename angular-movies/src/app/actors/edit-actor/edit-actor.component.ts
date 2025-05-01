import { Component, inject, Input, numberAttribute, OnInit } from '@angular/core';
import { ActorCreationDTO, ActorDTO } from '../actors.models';
import { ActorsFormComponent } from "../actors-form/actors-form.component";
import { ActorsService } from '../actors.service';
import { LoadingComponent } from "../../shared/components/loading/loading.component";
import { extractErrors } from '../../shared/functions/extractErrors';
import { DisplayErrorsComponent } from "../../shared/components/display-errors/display-errors.component";
import { Router } from '@angular/router';

@Component({
  selector: 'app-edit-actor',
  imports: [ActorsFormComponent, LoadingComponent, DisplayErrorsComponent],
  templateUrl: './edit-actor.component.html',
  styleUrl: './edit-actor.component.css'
})
export class EditActorComponent implements OnInit {
    

    @Input({transform: numberAttribute})
    id!:number;
    model?: ActorDTO;
    actorsService = inject(ActorsService);
    errors:string[]=[];
    router = inject(Router);

    ngOnInit(): void {
      this.actorsService.getById(this.id).subscribe(actor => {
        this.model = actor;
      })
    }

    saveChanges(actor:ActorCreationDTO){
      this.actorsService.update(this.id,actor).subscribe({
        next:()=>{
          this.router.navigate(['/actors']);
        },
        error:err=>{
          const errors = extractErrors(err);
          this.errors = errors;
        }
      })
    }
}
