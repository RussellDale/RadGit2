using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad2.Models.Domian
{
    public class CourseAssignmentRepository : SqlRepository<CourseAssignment>, ICourseAssignmentRepository
    {
        public CourseAssignmentRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<CourseAssignment> GetAll()
        {
            return EfDbSet
                .Include("Course")
                .Include("Course.Department")
                .Include("Instructor")
                .Include("Instructor.OfficeAssignment")
                ;
        }

        public override async Task<CourseAssignment> GetById(object id)
        {
            CourseAssignment p = GetById1((object[])id);

            return await GetAll().SingleOrDefaultAsync(c => (c.InstructorId == p.InstructorId && c.CourseId == p.CourseId));
        }

        private CourseAssignment GetById1(object[] id)
        {
            int instructorId, courseId;

            int.TryParse(id[0].ToString(), out courseId);
            int.TryParse(id[1].ToString(), out instructorId);

            CourseAssignment p = new CourseAssignment();

            p.InstructorId = instructorId;
            p.CourseId = courseId;

            return p;
        }

        public IEnumerable<CourseAssignment> GetForInstructorId(int id)
        {
            return GetAll().Where(o => o.InstructorId == id).ToList();
        }
        public IEnumerable<CourseAssignment> GetForCourseId(int id)
        {
            return GetAll().Where(o => o.CourseId == id).ToList();
        }

        public async Task Insert(CourseAssignment courseAssignment)
        {
            await EfDbSet.AddAsync(courseAssignment);
        }

        public async Task Update(CourseAssignment courseAssignment)
        {
            var entry = Context.Entry(courseAssignment);
            if (entry.State == EntityState.Detached)
            {
                var attachedCourseAssignment = await GetById(courseAssignment.InstructorId);
                if (attachedCourseAssignment != null)
                {
                    Context.Entry(attachedCourseAssignment).CurrentValues.SetValues(courseAssignment);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(CourseAssignment courseAssignment)
        {
            EfDbSet.Remove(courseAssignment);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface ICourseAssignmentRepository
    {
        Task Insert(CourseAssignment courseAssignment);
        Task Update(CourseAssignment courseAssignment);
        void Delete(CourseAssignment courseAssignment);
        void Save();
    }
}
