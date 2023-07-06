using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad3.Models.Domian
{
    public class ShippersRepository : SqlRepository<Shippers>, IShippersRepository
    {
        public ShippersRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<Shippers> GetAll()
        {
            return EfDbSet
                ;
        }

        public override async Task<Shippers> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.ShipperId == (int)id);
        }

        public IEnumerable<Shippers> GetForShippers(int id)
        {
            return GetAll().Where(o => o.ShipperId == id).ToList();
        }

        public async Task Insert(Shippers shippers)
        {
            await EfDbSet.AddAsync(shippers);
        }

        public async Task Update(Shippers shippers)
        {
            var entry = Context.Entry(shippers);
            if (entry.State == EntityState.Detached)
            {
                var attachedShippers = await GetById(shippers.ShipperId);
                if (attachedShippers != null)
                {
                    Context.Entry(attachedShippers).CurrentValues.SetValues(shippers);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Shippers shippers)
        {
            EfDbSet.Remove(shippers);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IShippersRepository
    {
        Task Insert(Shippers shippers);
        Task Update(Shippers shippers);
        void Delete(Shippers shippers);
        void Save();
    }
}
