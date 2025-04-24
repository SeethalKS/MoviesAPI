using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Controllers
{
    [Route("api/genres")]  //[Route("api/[controller]")]
    [ApiController]
    public class GenresController:ControllerBase
    {
        private readonly IOutputCacheStore outputCacheStore;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private const string cacheTag = "genres";
        public GenresController(IOutputCacheStore outputCacheStore,ApplicationDbContext context
            ,IMapper mapper)
        { 
            this.outputCacheStore = outputCacheStore;
            this.context = context;
            this.mapper = mapper;
        }
       
        
        [HttpGet] //api/genres
        
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<GenreDTO>> Get()
        {
            return await context.Genres.ProjectTo<GenreDTO>(mapper.ConfigurationProvider).ToListAsync(); ;
        }

        [HttpGet("{id:int}",Name ="GetGenreById")] //api/genres/500
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<Genre>> Get(int id)
        {
            
           throw new NotImplementedException();
        }
        
        [HttpPost]
        public async Task<CreatedAtRouteResult> Post([FromBody] GenreCreationDTO genreCreationDTO)
        {
            var genre = mapper.Map<Genre>(genreCreationDTO);
            context.Add(genre);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            var genreDTO = mapper.Map<GenreDTO>(genre);
            return CreatedAtRoute("GetGenreById",new {id=genreDTO.Id},genreDTO);
        
        }

        [HttpPut]
        public void Put()
        { 
        }
        [HttpDelete]
        public void Delete() 
        { 
        }
    }
}
