using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MoviesAPI.Entities;

namespace MoviesAPI.Controllers
{
    [Route("api/genres")]  //[Route("api/[controller]")]
    [ApiController]
    public class GenresController:ControllerBase
    {
        [HttpGet] //api/genres
        [HttpGet("all-genres")] //api/genres/all-genres
        [HttpGet("/all-of-the-genres")] // /all-of-the-genres
        public List<Genre> Get()
        {
            var repository = new InMemoryRepository();
            var genres = repository.GetAllGenres();
            return genres;
        }

        [HttpGet("{id:int}")] //api/genres/500
        [OutputCache]
        public async Task<ActionResult<Genre>> Get(int id)
        {
            var repository = new InMemoryRepository();
            var genre = await repository.GetById(id);
            if(genre is null)
            {
                return NotFound();
            }
            return genre;
        }
        [HttpGet("{name}")] //api/genres/action
        [OutputCache]
        public async Task<ActionResult<Genre>> Get(string name, [FromQuery] int id)
        {
            return new Genre {Id = id, Name = name };
        }
        [HttpPost]
        public ActionResult<Genre> Post([FromBody] Genre genre)
        {
            genre.Id = 3;
            return genre;
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
