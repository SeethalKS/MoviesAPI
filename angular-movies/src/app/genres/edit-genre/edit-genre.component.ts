import { Component, inject, Input, numberAttribute, OnInit } from '@angular/core';
import { GenreCreationDTO, GenreDTO } from '../genres.models';
import { GenresFormComponent } from "../genres-form/genres-form.component";
import { GenresService } from '../genres.service';
import { LoadingComponent } from "../../shared/components/loading/loading.component";
import { extractErrors } from '../../shared/functions/extractErrors';
import { DisplayErrorsComponent } from "../../shared/components/display-errors/display-errors.component";
import { Router } from '@angular/router';

@Component({
  selector: 'app-edit-genre',
  imports: [GenresFormComponent, LoadingComponent, DisplayErrorsComponent],
  templateUrl: './edit-genre.component.html',
  styleUrl: './edit-genre.component.css'
})
export class EditGenreComponent implements OnInit {

  @Input({transform: numberAttribute})
  id!:number;
  model?:GenreDTO;
  genresService = inject(GenresService);
  errors: string[]=[];
  router = inject(Router);

  ngOnInit(): void {
    this.genresService.getById(this.id).subscribe(genre =>{
      this.model = genre;
    })
  }

  saveChanges(genre:GenreCreationDTO){
   this.genresService.update(this.id,genre).subscribe({
    next:() => {
       this.router.navigate(['/genres']);
    },
    error:err => {
      const errors = extractErrors(err);
      this.errors = errors;
    }
   })
  }
}
