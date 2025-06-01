using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private readonly IFileStorage fileStorage;
        private const string cacheTag = "movies";
        private readonly string container = "movies";
        public MoviesController(ApplicationDbContext context, IMapper mapper,
            IOutputCacheStore outputCacheStore, IFileStorage fileStorage)
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.fileStorage = fileStorage;
        }
        [HttpGet("{id:int}",Name = "GetMovieById")]
        public IActionResult Get(int id)
        {
          throw new NotImplementedException();
        }
        [HttpGet("postget")]
        public async Task<ActionResult<MoviesPostGetDTO>> PostGet()
        {
            var genres = await context.Genres.OrderBy(g=>g.Name).ProjectTo<GenreDTO>(mapper.ConfigurationProvider).ToListAsync();
            var theatres = await context.Theaters.OrderBy(t=>t.Name).ProjectTo<TheaterDTO>(mapper.ConfigurationProvider).ToListAsync();
            return new MoviesPostGetDTO { Theaters = theatres, Genres = genres };
        }

        [HttpPost]

        public async Task<CreatedAtRouteResult> Post([FromForm] MovieCreationDTO movieCreationDTO)
            {
            var movie = mapper.Map<Movie>(movieCreationDTO);
            if (movieCreationDTO.Poster is not null)
            {
                var url = await fileStorage.Store(container,movieCreationDTO.Poster);
                movie.Poster = url;
            }
            AssignActorsOrder(movie);
            context.Add(movie);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            var movieDTO = mapper.Map<MovieDTO>(movie);
            return CreatedAtRoute("GetMovieById", new {id = movieDTO.Id},movieDTO);
            }
        private void AssignActorsOrder(Movie movie)
        {
            if(movie.MoviesActors is not null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i;
                }
            }
        }
    }
}
