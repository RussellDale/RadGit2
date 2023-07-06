using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Rad3.Models.Domian
{
    public class CustomersRepository : SqlRepository<Customers>, ICustomersRepository
    {
        public CustomersRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<Customers> GetAll()
        {
            return EfDbSet
                ;
        }

        public override async Task<Customers> GetById(object id)
        {
            Customers p = GetById1((object[])id);

            return await GetById2(p.CustomerId);
        }

        private Customers GetById1(object[] id)
        {
            string customerId = id[0].ToString();

            Customers p = new Customers();

            p.CustomerId = customerId;

            return p;
        }

        private async Task<Customers> GetById2(string customerId)
        {
            return await GetAll().SingleOrDefaultAsync(
                 c => c.CustomerId == customerId);
        }

        public IEnumerable<Customers> GetForCustomers(string id)
        {
            return GetAll().Where(o => o.CustomerId == id).ToList();
        }

        public async Task Insert(Customers customer)
        {
            await EfDbSet.AddAsync(customer);
        }

        public async Task Update(Customers customer)
        {
            var entry = Context.Entry(customer);
            if (entry.State == EntityState.Detached)
            {
                var attachedCustomers = await GetById2(customer.CustomerId);
                if (attachedCustomers != null)
                {
                    Context.Entry(attachedCustomers).CurrentValues.SetValues(customer);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Customers customer)
        {
            EfDbSet.Remove(customer);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface ICustomersRepository
    {
        Task Insert(Customers customer);
        Task Update(Customers customer);
        void Delete(Customers customer);
        void Save();
    }
}
