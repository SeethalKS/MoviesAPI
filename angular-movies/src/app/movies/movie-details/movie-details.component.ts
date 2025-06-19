import { Component, inject, Input, numberAttribute, OnInit } from '@angular/core';
import { MovieDTO } from '../movies.models';
import { MoviesService } from '../movies.service';
import {MatChipsModule} from '@angular/material/chips';
import { LoadingComponent } from "../../shared/components/loading/loading.component";
import { RouterLink } from '@angular/router';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { MapComponent } from "../../shared/components/map/map.component";
import { Coordinate } from '../../shared/components/map/Coordinate.model';

@Component({
  selector: 'app-movie-details',
  standalone:true,
  imports: [LoadingComponent, MatChipsModule, RouterLink, MapComponent],
  templateUrl: './movie-details.component.html',
  styleUrl: './movie-details.component.css'
})
export class MovieDetailsComponent implements OnInit {
  
  @Input({transform: numberAttribute})
  id!: number;
  movie!: MovieDTO;
  trailerURL!: SafeResourceUrl;
  sanitizer = inject(DomSanitizer);

  moviesService = inject(MoviesService);
  coordinates:Coordinate[]=[];
  
  ngOnInit(): void {
    this.moviesService.getById(this.id).subscribe(movie =>{
      this.movie =movie;
      movie.releaseDate = new Date(movie.releaseDate);
      this.trailerURL = this.transformYoutubeURLToEmbed(movie.trailer);
      if(movie.theatres){
        this.coordinates = movie.theatres.map(theater =>{
        return {latitude:theater.latitude,longitude:theater.longitude,text:theater.name}

      });
      }
         
    });
  }
 transformYoutubeURLToEmbed(url:string): SafeResourceUrl | string{
  if(!url){
    return '';
  }
  // https://youtu.be/evEDtHqrlVk?si=bbroaiKr-nZwgAgM

  let videoId = url.split('v=')[1];
  let ampersandPosition = videoId.indexOf('&');
  if(ampersandPosition !== -1)
  {
    videoId = videoId.substring(0,ampersandPosition);
  }
    return this.sanitizer.bypassSecurityTrustResourceUrl(`https://www.youtube.com/embed/${videoId}`);
 }
}
