using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad2.Models.Domian
{
    public class OfficeAssignmentRepository : SqlRepository<OfficeAssignment>, IOfficeAssignmentRepository
    {
        public OfficeAssignmentRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<OfficeAssignment> GetAll()
        {
            return EfDbSet
                .Include("Instructor")
                ;
        }

        public override async Task<OfficeAssignment> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.InstructorId == (int)id);
        }

        public IEnumerable<OfficeAssignment> GetForOfficeAssignment(int id)
        {
            return GetAll().Where(o => o.InstructorId == id).ToList();
        }

        public async Task Insert(OfficeAssignment OfficeAssignment)
        {
            await EfDbSet.AddAsync(OfficeAssignment);
        }

        public async Task Update(OfficeAssignment OfficeAssignment)
        {
            var entry = Context.Entry(OfficeAssignment);
            if (entry.State == EntityState.Detached)
            {
                var attachedOfficeAssignment = await GetById(OfficeAssignment.InstructorId);
                if (attachedOfficeAssignment != null)
                {
                    Context.Entry(attachedOfficeAssignment).CurrentValues.SetValues(OfficeAssignment);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(OfficeAssignment OfficeAssignment)
        {
            EfDbSet.Remove(OfficeAssignment);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IOfficeAssignmentRepository
    {
        Task Insert(OfficeAssignment OfficeAssignment);
        Task Update(OfficeAssignment OfficeAssignment);
        void Delete(OfficeAssignment OfficeAssignment);
        void Save();
    }
}
