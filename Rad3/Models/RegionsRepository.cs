using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad3.Models.Domian
{
    public class RegionsRepository : SqlRepository<Regions>, IRegionsRepository
    {
        public RegionsRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<Regions> GetAll()
        {
            return EfDbSet
  //              .Include("Products")
                ;
        }

        public override async Task<Regions> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.RegionId == (int)id);
        }

        public IEnumerable<Regions> GetForRegions(int id)
        {
            return GetAll().Where(o => o.RegionId == id).ToList();
        }

        public async Task Insert(Regions regions)
        {
            await EfDbSet.AddAsync(regions);
        }

        public async Task Update(Regions regions)
        {
            var entry = Context.Entry(regions);
            if (entry.State == EntityState.Detached)
            {
                var attachedRegions = await GetById(regions.RegionId);
                if (attachedRegions != null)
                {
                    Context.Entry(attachedRegions).CurrentValues.SetValues(regions);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Regions regions)
        {
            EfDbSet.Remove(regions);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IRegionsRepository
    {
        Task Insert(Regions regions);
        Task Update(Regions regions);
        void Delete(Regions regions);
        void Save();
    }
}
