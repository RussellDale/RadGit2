using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad2.Models.Domian
{
    public class EnrollmentRepository : SqlRepository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<Enrollment> GetAll()
        {
            return EfDbSet
                .Include("Student")
                .Include("Course")
                .Include("Course.Department")
                ; 
        }

        public override async Task<Enrollment> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.EnrollmentId == (int)id);
        }

        public IEnumerable<Enrollment> GetForEnrollment(int id)
        {
            return GetAll().Where(o => o.EnrollmentId == id).ToList();
        }
        public IEnumerable<Enrollment> GetForStudent(int id)
        {
            return GetAll().Where(o => o.StudentId == id).ToList();
        }
        public async Task Insert(Enrollment Enrollment)
        {
            await EfDbSet.AddAsync(Enrollment);
        }

        public async Task Update(Enrollment Enrollment)
        {
            var entry = Context.Entry(Enrollment);
            if (entry.State == EntityState.Detached)
            {
                var attachedEnrollment = await GetById(Enrollment.EnrollmentId);
                if (attachedEnrollment != null)
                {
                    Context.Entry(attachedEnrollment).CurrentValues.SetValues(Enrollment);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Enrollment Enrollment)
        {
            EfDbSet.Remove(Enrollment);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IEnrollmentRepository
    {
        Task Insert(Enrollment Enrollment);
        Task Update(Enrollment Enrollment);
        void Delete(Enrollment Enrollment);
        void Save();
    }
}
