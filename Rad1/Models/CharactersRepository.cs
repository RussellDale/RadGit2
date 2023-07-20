using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad1.Models.Domian
{
    public class CharactersRepository : SqlRepository<Characters>, ICharactersRepository
    {
        public CharactersRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<Characters> GetAll()
        {
            return EfDbSet;
        }

        public override async Task<Characters> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.Id == (int)id);
        }

        public IEnumerable<Characters> GetForAlbum(int id)
        {
            return GetAll().Where(o => o.Id == id).ToList();
        }

        public async Task Insert(Characters characters)
        {
            await EfDbSet.AddAsync(characters);
        }

        public async Task Update(Characters characters)
        {
            var entry = Context.Entry(characters);
            if (entry.State == EntityState.Detached)
            {
                var attachedAlbum = await GetById(characters.Id);
                if (attachedAlbum != null)
                {
                    Context.Entry(attachedAlbum).CurrentValues.SetValues(characters);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Characters characters)
        {
            EfDbSet.Remove(characters);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface ICharactersRepository
    {
        Task Insert(Characters characters);
        Task Update(Characters characters);
        void Delete(Characters characters);
        void Save();
    }
}