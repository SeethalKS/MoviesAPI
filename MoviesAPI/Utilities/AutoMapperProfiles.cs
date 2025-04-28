using AutoMapper;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Utilities
{
    public class AutoMapperProfiles :Profile
    {
        public AutoMapperProfiles() {
            ConfigureGenres();
            ConfigureActors();
        }
        private void ConfigureActors()
        {
            CreateMap<ActorCreationDTO, Actor>()
                .ForMember(x => x.Picture, options => options.Ignore());
        }
        private void ConfigureGenres()
        {
            CreateMap<GenreCreationDTO,Genre> ();
            CreateMap<Genre,GenreDTO> ();
        }
    }
}
