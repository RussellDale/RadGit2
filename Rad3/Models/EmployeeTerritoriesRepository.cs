using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Rad3.Models.Domian
{
    public class EmployeeTerritoriesRepository : SqlRepository<EmployeeTerritories>, IEmployeeTerritoriesRepository
    {
        public EmployeeTerritoriesRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<EmployeeTerritories> GetAll()
        { 
            return EfDbSet
                .Include("Employee")
                .Include("Territory")
                .Include("Territory.Region")
                ;
        }
        public override async Task<EmployeeTerritories> GetById(object id)
        {
            EmployeeTerritories p = GetById1((object[])id);

            return await GetAll().SingleOrDefaultAsync(
                 c => c.EmployeeId   == p.EmployeeId &&
                      c.TerritoryId == p.TerritoryId);
        }

        private EmployeeTerritories GetById1(object[] id)
        {
            int employeeId;
            int.TryParse(id[0].ToString(), out employeeId);
            string territoriesId = id[1].ToString();

            EmployeeTerritories p = new EmployeeTerritories();

            p.EmployeeId = employeeId;
            p.TerritoryId = territoriesId;

            return p;
        }


        public IEnumerable<EmployeeTerritories> GetForEmployeeTerritories(int id)
        {
            return GetAll().Where(o => o.EmployeeId == id).ToList();
        }

        public async Task Insert(EmployeeTerritories employeeTerritories)
        {
            await EfDbSet.AddAsync(employeeTerritories);
        }

        public async Task Update(EmployeeTerritories employeeTerritories)
        {
            var entry = Context.Entry(employeeTerritories);
            if (entry.State == EntityState.Detached)
            {
                var id = new object[2];
                id[0] = employeeTerritories.EmployeeId;
                id[1] = employeeTerritories.TerritoryId;             

                var attachedOrderDetails = await GetById(id);
                if (attachedOrderDetails != null)
                {
                    Context.Entry(attachedOrderDetails).CurrentValues.SetValues(employeeTerritories);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(EmployeeTerritories employeeTerritories)
        {
            EfDbSet.Remove(employeeTerritories);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IEmployeeTerritoriesRepository
    {
        Task Insert(EmployeeTerritories employeeTerritories);
        Task Update(EmployeeTerritories employeeTerritories);
        void Delete(EmployeeTerritories employeeTerritories);
        void Save();
    }
}
