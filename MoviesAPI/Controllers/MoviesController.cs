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
        [HttpGet("landing")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<LandingDTO>> Get()
        {
            var today = DateTime.Now;
            var top = 6;

            var upcomingReleases = await context.Movies
                .Where(m => m.ReleaseDate > today)
                .OrderBy(m => m.ReleaseDate)
                .Take(top)
                .ProjectTo<MovieDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            var inTheaters = await context.Movies
                .Where(m=>m.MoviesTheaters.Select(mt=>mt.MovieId).Contains(m.Id))
                .OrderBy(m=>m.ReleaseDate)
                .Take(top)
                .ProjectTo<MovieDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
            var result = new LandingDTO();
            result.InTheaters = inTheaters;
            result.UpcomingReleases = upcomingReleases;
            return result;
        }



        [HttpGet("{id:int}",Name = "GetMovieById")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<MovieDetailsDTO>> Get(int id)
        {
          var movie = await context.Movies
                .ProjectTo<MovieDetailsDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(m=>m.Id==id);
            if (movie is null)
            {
                return NotFound();
            }
            return movie;
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

        [HttpGet("putget/{id:int}")]
        public async Task<ActionResult<MoviesPutGetDTO>> PutGet(int id)
        {
            var movie = await context.Movies
                .ProjectTo<MovieDetailsDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(m =>m.Id == id);
            if(movie is null)
            {
                return NotFound();
            }
            var selectedGenresIds = movie.Genres.Select(g=>g.Id).ToList();
            var nonSelectedGenres = await context.Genres.Where(g => !selectedGenresIds.Contains(g.Id))
                .ProjectTo<GenreDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            var selectedTheaterIds = movie.Theaters.Select(t => t.Id).ToList();
            var nonSelectedTheaters = await context.Theaters.Where(t=> !selectedTheaterIds.Contains(t.Id))
                                           .ProjectTo<TheaterDTO>(mapper.ConfigurationProvider)
                                           .ToListAsync();

            var response = new MoviesPutGetDTO();
            response.Movie = movie;
            response.SelectedGenres = movie.Genres;
            response.NonSelectedGenres = nonSelectedGenres;
            response.SelectedTheaters = movie.Theaters;
            response.NonSelectedTheaters= nonSelectedTheaters;
            response.Actors = movie.Actors;

            return response;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] MovieCreationDTO movieCreationDTO)
        {
            var movie = await context.Movies
                .Include(p=>p.MoviesActors)
                .Include(p=>p.MoviesGenres)
                .Include(p=>p.MoviesTheaters)
                .FirstOrDefaultAsync(m=>m.Id == id);
            if(movie is null)
            {
                return NotFound();
            }
            movie = mapper.Map(movieCreationDTO, movie);
            if(movieCreationDTO.Poster != null)
            {
                movie.Poster = await fileStorage.Edit(movie.Poster,
                    container, movieCreationDTO.Poster);
            }
            AssignActorsOrder(movie);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
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
