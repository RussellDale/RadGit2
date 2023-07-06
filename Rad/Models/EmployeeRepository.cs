using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad.Models.Domian
{
    public class EmployeeRepository : SqlRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(MyDbContext context)
            : base(context)
        {
        }

        public override IQueryable<Employee> GetAll()
        {
            return EfDbSet;
        }

        public override async Task<Employee> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.EmployeeId == (int)id);
        }

        public IEnumerable<Employee> GetForEmployee(int id)
        {
            return GetAll().Where(o => o.EmployeeId == id).ToList();
        }

        public async Task Insert(Employee employee)
        {
            await EfDbSet.AddAsync(employee);
        }

        public async Task Update(Employee employee)
        {
            var entry = Context.Entry(employee);
            if (entry.State == EntityState.Detached)
            {
                var attachedTrack = await GetById(employee.EmployeeId);
                if (attachedTrack != null)
                {
                    Context.Entry(attachedTrack).CurrentValues.SetValues(employee);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Employee employee)
        {
            EfDbSet.Remove(employee);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IEmployeeRepository
    {
        Task Insert(Employee employee);
        Task Update(Employee employee);
        void Delete(Employee employee);
        void Save();
    }
}