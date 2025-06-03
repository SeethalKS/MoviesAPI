import { Component, inject } from '@angular/core';
import { MoviesListComponent } from "../movies/movies-list/movies-list.component";
import { MoviesService } from '../movies/movies.service';

@Component({
  selector: 'app-landing-page',
  imports: [MoviesListComponent],
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.css'
})
export class LandingPageComponent {

  upcomingReleasesMovies :any;
  inTheatresMovies:any;

  moviesService = inject(MoviesService)
  constructor(){
    this.moviesService.getLanding().subscribe(response => {
     this.upcomingReleasesMovies = response.upcomingReleases;
     this.inTheatresMovies = response.inTheaters;
    });
  }
}
