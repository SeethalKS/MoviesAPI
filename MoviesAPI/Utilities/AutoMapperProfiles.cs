using AutoMapper;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using NetTopologySuite.Geometries;

namespace MoviesAPI.Utilities
{
    public class AutoMapperProfiles :Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory) {
            ConfigureGenres();
            ConfigureActors();
            ConfigureTheaters(geometryFactory);
        }
        private void ConfigureTheaters(GeometryFactory geometryFactory)
        {
            CreateMap<Theater, TheaterDTO>()
                .ForMember(x => x.Latitude, x => x.MapFrom(p => p.Location.Y))
                .ForMember(x => x.Longitude, x => x.MapFrom(p => p.Location.X));
            CreateMap<TheatreCreationDTO,Theater>()
                .ForMember(entity => entity.Location,dto => dto.MapFrom(p=>
                geometryFactory.CreatePoint(new Coordinate(p.Latitude, p.Longitude))));
        }
        private void ConfigureActors()
        {
            CreateMap<ActorCreationDTO, Actor>()
                .ForMember(x => x.Picture, options => options.Ignore());

            CreateMap<Actor, ActorDTO>();
        }
        private void ConfigureGenres()
        {
            CreateMap<GenreCreationDTO,Genre> ();
            CreateMap<Genre,GenreDTO> ();
        }
    }
}
