using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad1.Models.Domian
{
    public class CategoriesRepository : SqlRepository<Categories>, ICategoriesRepository
    {
        public CategoriesRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<Categories> GetAll()
        {
            return EfDbSet
                .Include("Products")
                ;
        }

        public override async Task<Categories> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.CategoryId == (int)id);
        }

        public IEnumerable<Categories> GetForCategories(int id)
        {
            return GetAll().Where(o => o.CategoryId == id).ToList();
        }

        public async Task Insert(Categories categories)
        {
            await EfDbSet.AddAsync(categories);
        }

        public async Task Update(Categories categories)
        {
            var entry = Context.Entry(categories);
            if (entry.State == EntityState.Detached)
            {
                var attachedCategories = await GetById(categories.CategoryId);
                if (attachedCategories != null)
                {
                    Context.Entry(attachedCategories).CurrentValues.SetValues(categories);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Categories categories)
        {
            EfDbSet.Remove(categories);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface ICategoriesRepository
    {
        Task Insert(Categories categories);
        Task Update(Categories categories);
        void Delete(Categories categories);
        void Save();
    }
}
