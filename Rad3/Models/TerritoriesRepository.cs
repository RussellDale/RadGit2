using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad3.Models.Domian
{
    public class TerritoriesRepository : SqlRepository<Territories>, ITerritoriesRepository
    {
        public TerritoriesRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<Territories> GetAll()
        {
            return EfDbSet
                .Include("Region")
                ;
        }

        public override async Task<Territories> GetById(object id)
        {
            Territories p = GetById1((object[])id);

            return await GetById2(p.TerritoryId);
        }

        private Territories GetById1(object[] id)
        {
            string territoryId = id[0].ToString();

            Territories p = new Territories();

            p.TerritoryId = territoryId;

            return p;
        }

        private async Task<Territories> GetById2(string territoryId)
        {
            return await GetAll().SingleOrDefaultAsync(
                 c => c.TerritoryId == territoryId);
        }

        public IEnumerable<Territories> GetForTerritories(string id)
        {
            return GetAll().Where(o => o.TerritoryId == id).ToList();
        }

        public async Task Insert(Territories territories)
        {
            await EfDbSet.AddAsync(territories);
        }

        public async Task Update(Territories territories)
        {
            var entry = Context.Entry(territories);
            if (entry.State == EntityState.Detached)
            {
                var attachedTerritories = await GetById2(territories.TerritoryId);
                if (attachedTerritories != null)
                {
                    Context.Entry(attachedTerritories).CurrentValues.SetValues(territories);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Territories territories)
        {
            EfDbSet.Remove(territories);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface ITerritoriesRepository
    {
        Task Insert(Territories territories);
        Task Update(Territories territories);
        void Delete(Territories territories);
        void Save();
    }
}
