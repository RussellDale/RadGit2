using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad.Models.Domian
{
    public class CustomerRepository : SqlRepository<Customer>, ICustomerRepository
    {
        private PlaylistTrackRepository PlaylistTrack { get; set; }

        public CustomerRepository(MyDbContext context)
            : base(context)
        {
        }

        public override IQueryable<Customer> GetAll()
        {
            return EfDbSet.Include("Employee");
        }

        public override async Task<Customer> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.CustomerId == (int)id);
        }

        public async Task Insert(Customer customer)
        {
            await EfDbSet.AddAsync(customer);
        }

        public async Task Update(Customer customer)
        {
            var entry = Context.Entry(customer);
            if (entry.State == EntityState.Detached)
            {
                var attachedCustomer = await GetById(customer.CustomerId);
                if (attachedCustomer != null)
                {
                    Context.Entry(attachedCustomer).CurrentValues.SetValues(customer);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Customer customer)
        {
            EfDbSet.Remove(customer);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface ICustomerRepository
    {
        Task Insert(Customer customer);
        Task Update(Customer customer);
        void Delete(Customer customer);
        void Save();
    }
}