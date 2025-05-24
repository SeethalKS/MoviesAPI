using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Utilities;
using System.Linq.Expressions;

namespace MoviesAPI.Controllers
{
    public class CustomBaseController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public CustomBaseController(ApplicationDbContext context,IMapper mapper)
        {

            this.context = context;
            this.mapper = mapper;
        }
        protected async Task<List<TDTO>> Get<TEntity,TDTO>(PaginationDTO pagination,
            Expression<Func<TEntity,object>> orderBy)
            where TEntity : class
        {
            var queryable = context.Set<TEntity>().AsQueryable();
            await HttpContext.InsertPaginationParametersInHeader(queryable);
            return await queryable
                .OrderBy(orderBy)
                .Paginate(pagination)
                .ProjectTo<TDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        protected async Task<ActionResult<TDTO>> Get<TEntity,TDTO>(int id)
            where TEntity : class
            where TDTO : IId
        {
            var enity = await context.Set<TEntity>()
                .ProjectTo<TDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e=>e.Id == id);
            if(enity is null)
            {
                return NotFound();
            }
            return enity;


        }
    }
}
