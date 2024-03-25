using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad2.Models.Domian
{
    public class TitlesRepository : SqlRepository<Titles>, ITitlesRepository
    {
        public TitlesRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<Titles> GetAll()
        {
            return EfDbSet
                ;
        }

        public override async Task<Titles> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.Id == (int)id);
        }

        public IEnumerable<Titles> GetForTitles(int id)
        {
            return GetAll().Where(o => o.Id == id).ToList();
        }


        public IEnumerable<Titles> GetForTitles2(int Id)
        {
            return GetAll().Where(o => o.Id2 == Id).ToList();
        }

        public async Task Insert(Titles titles)
        {
            await EfDbSet.AddAsync(titles);
        }

        public async Task Update(Titles titles)
        {
            var entry = Context.Entry(titles);
            if (entry.State == EntityState.Detached)
            {
                var attachedTitles = await GetById(titles.Id);
                if (attachedTitles != null)
                {
                    Context.Entry(attachedTitles).CurrentValues.SetValues(titles);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Titles titles)
        {
            EfDbSet.Remove(titles);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface ITitlesRepository
    {
        Task Insert(Titles titles);
        Task Update(Titles titles);
        void Delete(Titles titles);
        void Save();
    }
}
