using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad2.Models.Domian
{
    public class InstructorRepository : SqlRepository<Instructor>, IInstructorRepository
    {
        public InstructorRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<Instructor> GetAll()
        {
            return EfDbSet
                .Include("OfficeAssignment")
                .Include("CourseAssignment")
                ;
        }

        public override async Task<Instructor> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.Id == (int)id);
        }

        public IEnumerable<Instructor> GetForInstructor(int id)
        {
            return GetAll().Where(o => o.Id == id).ToList();
        }
        public async Task Insert(Instructor instructor)
        {
            await EfDbSet.AddAsync(instructor);
        }

        public async Task Update(Instructor instructor)
        {
            var entry = Context.Entry(instructor);
            if (entry.State == EntityState.Detached)
            {
                var attachedInstructor = await GetById(instructor.Id);
                if (attachedInstructor != null)
                {
                    Context.Entry(attachedInstructor).CurrentValues.SetValues(instructor);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Instructor instructor)
        {
            EfDbSet.Remove(instructor);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IInstructorRepository
    {
        Task Insert(Instructor instructor);
        Task Update(Instructor instructor);
        void Delete(Instructor instructor);
        void Save();
    }
}
