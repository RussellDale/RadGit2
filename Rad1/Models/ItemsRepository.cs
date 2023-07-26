using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad1.Models.Domian
{
    public class ItemsRepository : SqlRepository<Items>, IItemsRepository
    {
        public ItemsRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<Items> GetAll()
        {
            return EfDbSet;
        }

        public override async Task<Items> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.Id == (int)id);
        }

        public IEnumerable<Items> GetForAlbum(int id)
        {
            return GetAll().Where(o => o.Id == id).ToList();
        }

        public async Task Insert(Items items)
        {
            await EfDbSet.AddAsync(items);
        }

        public async Task Update(Items items)
        {
            var entry = Context.Entry(items);
            if (entry.State == EntityState.Detached)
            {
                var attachedAlbum = await GetById(items.Id);
                if (attachedAlbum != null)
                {
                    Context.Entry(attachedAlbum).CurrentValues.SetValues(items);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Items items)
        {
            EfDbSet.Remove(items);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IItemsRepository
    {
        Task Insert(Items items);
        Task Update(Items items);
        void Delete(Items items);
        void Save();
    }
}