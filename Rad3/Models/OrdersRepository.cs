using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad3.Models.Domian
{
    public class OrdersRepository : SqlRepository<Orders>, IOrdersRepository
    {
        public OrdersRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<Orders> GetAll()
        {
            return EfDbSet
                .Include("Employee")
                .Include("Customer")
                .Include("ShipViaNavigation")
                ;
        }

        public override async Task<Orders> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.OrderId == (int)id);
        }

        public IEnumerable<Orders> GetForOrders(int id)
        {
            return GetAll().Where(o => o.OrderId == id).ToList();
        }

        public async Task Insert(Orders order)
        {
            await EfDbSet.AddAsync(order);
        }

        public async Task Update(Orders order)
        {
            var entry = Context.Entry(order);
            if (entry.State == EntityState.Detached)
            {
                var attachedOrders = await GetById(order.OrderId);
                if (attachedOrders != null)
                {
                    Context.Entry(attachedOrders).CurrentValues.SetValues(order);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Orders order)
        {
            EfDbSet.Remove(order);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IOrdersRepository
    {
        Task Insert(Orders order);
        Task Update(Orders order);
        void Delete(Orders order);
        void Save();
    }
}
