using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad2.Models.Domian
{
    public class StudentRepository : SqlRepository<Student>, IStudentRepository
    {
        public StudentRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<Student> GetAll()
        {
            return EfDbSet
                .Include("Enrollment")
                ;
        }

        public override async Task<Student> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.Id == (int)id);
        }

        public IEnumerable<Student> GetForStudent(int id)
        {
            return GetAll().Where(o => o.Id == id).ToList();
        }

        public async Task Insert(Student student)
        {
            await EfDbSet.AddAsync(student);
        }

        public async Task Update(Student student)
        {
            var entry = Context.Entry(student);
            if (entry.State == EntityState.Detached)
            {
                var attachedInstructor = await GetById(student.Id);
                if (attachedInstructor != null)
                {
                    Context.Entry(attachedInstructor).CurrentValues.SetValues(student);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Student student)
        {
            EfDbSet.Remove(student);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IStudentRepository
    {
        Task Insert(Student student);
        Task Update(Student student);
        void Delete(Student student);
        void Save();
    }
}
