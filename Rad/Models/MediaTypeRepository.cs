using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Rad.Models.Domian
{
    public class MediaTypeRepository : SqlRepository<MediaType>
    {
        public MediaTypeRepository(MyDbContext context)
            : base(context)
        {
        }

        public override IQueryable<MediaType> GetAll()
        {
            return EfDbSet;
        }

        public override async Task<MediaType> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.MediaTypeId == (int)id);
        }
    }
}