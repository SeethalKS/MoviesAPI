using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Entities;

namespace MoviesAPI.Controllers
{
    [Route("api/genres")]  //[Route("api/[controller]")]
    public class GenresController
    {
        [HttpGet]
        public List<Genre> Get()
        {
            var repository = new InMemoryRepository();
            var genres = repository.GetAllGenres();
            return genres;
        }

        [HttpGet]
        public Genre? Get(int id)
        {
            var repository = new InMemoryRepository();
            var genre = repository.GetById(id);
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
