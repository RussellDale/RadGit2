using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad3.Models.Domian
{
    public class EmployeesRepository : SqlRepository<Employees>, IEmployeesRepository
    {
        public EmployeesRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<Employees> GetAll()
        {
            return EfDbSet
  //              .Include("Products")
                ;
        }

        public override async Task<Employees> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.EmployeeId == (int)id);
        }

        public IEnumerable<Employees> GetForEmployees(int id)
        {
            return GetAll().Where(o => o.EmployeeId == id).ToList();
        }

        public async Task Insert(Employees employee)
        {
            await EfDbSet.AddAsync(employee);
        }

        public async Task Update(Employees employee)
        {
            var entry = Context.Entry(employee);
            if (entry.State == EntityState.Detached)
            {
                var attachedEmployees = await GetById(employee.EmployeeId);
                if (attachedEmployees != null)
                {
                    Context.Entry(attachedEmployees).CurrentValues.SetValues(employee);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Employees employee)
        {
            EfDbSet.Remove(employee);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IEmployeesRepository
    {
        Task Insert(Employees employee);
        Task Update(Employees employee);
        void Delete(Employees employee);
        void Save();
    }
}
