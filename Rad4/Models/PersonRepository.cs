using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad4.Models.Domian
{
    public class PersonRepository : SqlRepository<Person>, IPersonRepository
    {
        public PersonRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<Person> GetAll()
        {
            return EfDbSet;
        }

        public override async Task<Person> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.FirstName == (string)id);
        }

        public IEnumerable<Person> GetForAlbum(string id)
        {
            return GetAll().Where(o => o.FirstName == id).ToList();
        }

        public async Task Insert(Person person)
        {
            await EfDbSet.AddAsync(person);
        }

        public async Task Update(Person person)
        {
            var entry = Context.Entry(person);
            if (entry.State == EntityState.Detached)
            {
                var attachedAlbum = await GetById(person.FirstName);
                if (attachedAlbum != null)
                {
                    Context.Entry(attachedAlbum).CurrentValues.SetValues(person);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Person person)
        {
            EfDbSet.Remove(person);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IPersonRepository
    {
        Task Insert(Person person);
        Task Update(Person person);
        void Delete(Person person);
        void Save();
    }
}