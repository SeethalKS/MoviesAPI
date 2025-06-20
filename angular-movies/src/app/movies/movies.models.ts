import { ActorAutoCompleteDTO } from "../actors/actors.models";
import { GenreDTO } from "../genres/genres.models";
import { TheatreDTO } from "../theaters/theaters.models";

export interface MovieDTO{
    id:number,
    title:string;
    releaseDate:Date;
    trailer:string;
    poster?:string;
    genres?:GenreDTO[];
    theatres?:TheatreDTO[];
    actors?:ActorAutoCompleteDTO[];
}
export interface MovieCreationDTO{
    title:string;
    releaseDate:Date;
    trailer:string;
    poster?:File;
    genresIds?:number[];
    theatersIds?:number[];
    actors?: ActorAutoCompleteDTO[];
}
export interface MoviesPostGetDTO{
    genres :GenreDTO[];
    theaters:TheatreDTO[];
}
export interface MoviesPutGetDTO{
    movie: MovieDTO;
    selectedGenres: GenreDTO[];
    nonSelectedGenres:GenreDTO[];
    selectedTheaters: TheatreDTO[];
    nonSelectedTheaters:TheatreDTO[];
    actors:ActorAutoCompleteDTO[];
}

export interface LandingDTO{
    upcomingReleases: MovieDTO[];
    inTheaters:MovieDTO[];
}