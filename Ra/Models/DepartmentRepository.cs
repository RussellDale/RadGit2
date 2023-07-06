using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ra.Models.Domian
{
    public class DepartmentRepository : SqlRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(MyDbContext context)
            : base(context)
        {
        }

        public override IQueryable<Department> GetAll()
        {
            return EfDbSet;
        }

        public override async Task<Department> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.DepartmentID == (int)id);
        }

        public IEnumerable<Department> GetForDepartment(int id)
        {
            return GetAll().Where(o => o.DepartmentID == id).ToList();
        }

        public async Task Insert(Department department)
        {
            await EfDbSet.AddAsync(department);
        }

        public async Task Update(Department department)
        {
            var entry = Context.Entry(department);
            if (entry.State == EntityState.Detached)
            {
                var attachedDepartment = await GetById(department.DepartmentID);
                if (attachedDepartment != null)
                {
                    Context.Entry(attachedDepartment).CurrentValues.SetValues(department);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Department department)
        {
            EfDbSet.Remove(department);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IDepartmentRepository
    {
        Task Insert(Department department);
        Task Update(Department department);
        void Delete(Department department);
        void Save();
    }
}
