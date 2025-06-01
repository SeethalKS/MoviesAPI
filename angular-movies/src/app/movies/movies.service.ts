import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Observable } from 'rxjs';
import { MovieCreationDTO, MoviesPostGetDTO } from './movies.models';

@Injectable({
  providedIn: 'root'
})
export class MoviesService {

  constructor() { }
  private http = inject(HttpClient);//create dependency injection
  private baseURL = environment.apiURL + "/movies";

  public postGet():Observable<MoviesPostGetDTO>{
    return this.http.get<MoviesPostGetDTO>(`${this.baseURL}/postget`);
  }

  public create(movie:MovieCreationDTO){

    const formData = this.buildFormData(movie);
    return this.http.post(this.baseURL,formData);
  }
  private buildFormData(movie:MovieCreationDTO):FormData{
    const formData = new FormData();
    formData.append('title',movie.title);

    if (movie.releaseDate){
      formData.append('releaseData',movie.releaseDate.toISOString().split('T')[0]);
    }

    if(movie.poster)
    {
      formData.append('poster',movie.poster);
    }

    if(movie.trailer){
      formData.append('trailer',movie.trailer);
    }

    formData.append('genresIds',JSON.stringify(movie.genresIds));
    formData.append('theatersIds',JSON.stringify(movie.theatersIds));
    formData.append('actors',JSON.stringify(movie.actors));

    return formData;
  }
}
