using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Rad.Models.Domian
{
    public class GenreRepository : SqlRepository<Genre>
    {
        public GenreRepository(MyDbContext context)
            : base(context)
        {
        }

        public override IQueryable<Genre> GetAll()
        {
            return EfDbSet;
        }

        public override async Task<Genre> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.GenreId == (int)id);
        }
    }
}