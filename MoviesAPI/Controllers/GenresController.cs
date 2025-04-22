using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Entities;

namespace MoviesAPI.Controllers
{
    [Route("api/genres")]  //[Route("api/[controller]")]
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

        [HttpGet("{id}")] //api/genres/500
        public ActionResult<Genre> Get(int id)
        {
            var repository = new InMemoryRepository();
            var genre = repository.GetById(id);
            if(genre is null)
            {
                return NotFound();
            }
            return genre;
        }
        [HttpPost]
        public void Post()
        {

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
