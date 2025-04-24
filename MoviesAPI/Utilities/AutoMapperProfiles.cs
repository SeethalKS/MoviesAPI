using AutoMapper;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Utilities
{
    public class AutoMapperProfiles :Profile
    {
        public AutoMapperProfiles() {
            ConfigureGenres();
        }
        private void ConfigureGenres()
        {
            CreateMap<GenreCreationDTO,Genre> ();
            CreateMap<Genre,GenreDTO> ();
        }
    }
}
