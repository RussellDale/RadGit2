using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad3.Models.Domian
{
    public class ProductsRepository : SqlRepository<Products>, IProductsRepository
    {
        public ProductsRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<Products> GetAll()
        {
            return EfDbSet
                .Include("Category")
                .Include("Supplier")
                ;
        }

        public override async Task<Products> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.ProductId == (int)id);
        }

        public IEnumerable<Products> GetForProducts(int id)
        {
            return GetAll().Where(o => o.ProductId == id).ToList();
        }

        public async Task Insert(Products product)
        {
            await EfDbSet.AddAsync(product);
        }

        public async Task Update(Products product)
        {
            var entry = Context.Entry(product);
            if (entry.State == EntityState.Detached)
            {
                var attachedProducts = await GetById(product.ProductId);
                if (attachedProducts != null)
                {
                    Context.Entry(attachedProducts).CurrentValues.SetValues(product);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Products product)
        {
            EfDbSet.Remove(product);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IProductsRepository
    {
        Task Insert(Products product);
        Task Update(Products product);
        void Delete(Products product);
        void Save();
    }
}
