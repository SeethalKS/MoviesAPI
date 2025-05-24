using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Utilities;

namespace MoviesAPI.Controllers
{
    [Route("api/genres")]  //[Route("api/[controller]")]
    [ApiController]
    public class GenresController:CustomBaseController
    {
        private readonly IOutputCacheStore outputCacheStore;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private const string cacheTag = "genres";
        public GenresController(IOutputCacheStore outputCacheStore,ApplicationDbContext context
            ,IMapper mapper)
            :base(context,mapper)
        { 
            this.outputCacheStore = outputCacheStore;
            this.context = context;
            this.mapper = mapper;
        }
       
        
        [HttpGet] //api/genres
        
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<GenreDTO>> Get([FromQuery] PaginationDTO pagination)
        {
            return await Get<Genre, GenreDTO>(pagination, orderBy: g => g.Name);
        }

        [HttpGet("{id:int}",Name ="GetGenreById")] //api/genres/500
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<GenreDTO>> Get(int id)
        {
            return await Get<Genre,GenreDTO>(id);
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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] GenreCreationDTO genreCreationDTO)
        { 
            var genreExists = await context.Genres.AnyAsync(g => g.Id == id);
            if(!genreExists) {
                return NotFound();
            }
            var genre = mapper.Map<Genre>(genreCreationDTO);
            genre.Id = id;

            context.Update(genre);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag,default);
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) 
        { 
            var deletedRecords = await context.Genres.Where(g=>g.Id== id).ExecuteDeleteAsync();
            if(deletedRecords ==0)
            {
                return NotFound();
            }
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }
    }
}
