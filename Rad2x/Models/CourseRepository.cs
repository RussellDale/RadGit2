using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad2.Models.Domian
{
    public class CourseRepository : SqlRepository<Course>, ICourseRepository
    {
        public CourseRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<Course> GetAll()
        {
            return EfDbSet
                .Include("Department")
                .Include("Enrollment")
                .Include("CourseAssignment")
                ;
        }

        public override async Task<Course> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.CourseId == (int)id);
        }

        public IEnumerable<Course> GetForCourse(int id)
        {
            return GetAll().Where(o => o.CourseId == id).ToList();
        }

        public IEnumerable<Course> GetForDepartment(int id)
        {
            return GetAll().Where(o => o.DepartmentId == id).ToList();
        }

        public async Task Insert(Course Course)
        {
            await EfDbSet.AddAsync(Course);
        }

        public async Task Update(Course Course)
        {
            var entry = Context.Entry(Course);
            if (entry.State == EntityState.Detached)
            {
                var attachedCourse = await GetById(Course.CourseId);
                if (attachedCourse != null)
                {
                    Context.Entry(attachedCourse).CurrentValues.SetValues(Course);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Course Course)
        {
            EfDbSet.Remove(Course);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface ICourseRepository
    {
        Task Insert(Course Course);
        Task Update(Course Course);
        void Delete(Course Course);
        void Save();
    }
}
