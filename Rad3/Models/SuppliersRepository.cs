using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad3.Models.Domian
{
    public class SuppliersRepository : SqlRepository<Suppliers>, ISuppliersRepository
    {
        public SuppliersRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<Suppliers> GetAll()
        {
            return EfDbSet
                ;
        }

        public override async Task<Suppliers> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.SupplierId == (int)id);
        }

        public IEnumerable<Suppliers> GetForSuppliers(int id)
        {
            return GetAll().Where(o => o.SupplierId == id).ToList();
        }

        public async Task Insert(Suppliers supplier)
        {
            await EfDbSet.AddAsync(supplier);
        }

        public async Task Update(Suppliers supplier)
        {
            var entry = Context.Entry(supplier);
            if (entry.State == EntityState.Detached)
            {
                var attachedSuppliers = await GetById(supplier.SupplierId);
                if (attachedSuppliers != null)
                {
                    Context.Entry(attachedSuppliers).CurrentValues.SetValues(supplier);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Suppliers supplier)
        {
            EfDbSet.Remove(supplier);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface ISuppliersRepository
    {
        Task Insert(Suppliers supplier);
        Task Update(Suppliers supplier);
        void Delete(Suppliers supplier);
        void Save();
    }
}
