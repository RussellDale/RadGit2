using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad4.Models.Domian
{
    public class T1Repository : SqlRepository<T1>, ICharactersRepository
    {
        public T1Repository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<T1> GetAll()
        {
            return EfDbSet;
        }

        public override async Task<T1> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.Id == (int)id);
        }

        public IEnumerable<T1> GetForAlbum(int id)
        {
            return GetAll().Where(o => o.Id == id).ToList();
        }

        public async Task Insert(T1 t1)
        {
            await EfDbSet.AddAsync(t1);
        }

        public async Task Update(T1 t1)
        {
            var entry = Context.Entry(t1);
            if (entry.State == EntityState.Detached)
            {
                var attachedAlbum = await GetById(t1.Id);
                if (attachedAlbum != null)
                {
                    Context.Entry(attachedAlbum).CurrentValues.SetValues(t1);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(T1 t1)
        {
            EfDbSet.Remove(t1);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface ICharactersRepository
    {
        Task Insert(T1 t1);
        Task Update(T1 t1);
        void Delete(T1 t1);
        void Save();
    }
}