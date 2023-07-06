using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Rad3.Models.Domian
{
    public class OrderDetailsRepository : SqlRepository<OrderDetails>, IOrderDetailsRepository
    {
        public OrderDetailsRepository(dbContext context)
            : base(context)
        {
        }

        public override IQueryable<OrderDetails> GetAll()
        {
            return EfDbSet
                .Include("Product")
                ;
        }

        public override async Task<OrderDetails> GetById(object id)
        {
            OrderDetails p = GetById1((object[])id);

            return await GetAll().SingleOrDefaultAsync(
                 c => c.OrderId   == p.OrderId &&
                      c.ProductId == p.ProductId);
        }

        private OrderDetails GetById1(object[] id)
        {
            int orderId, productId;

            int.TryParse(id[0].ToString(), out orderId);
            int.TryParse(id[1].ToString(), out productId);

            OrderDetails p = new OrderDetails();

            p.OrderId = orderId;
            p.ProductId = productId;

            return p;
        }


        public IEnumerable<OrderDetails> GetForOrderDetails(int id)
        {
            return GetAll().Where(o => o.OrderId == id).ToList();
        }

        public async Task Insert(OrderDetails orderDetail)
        {
            await EfDbSet.AddAsync(orderDetail);
        }

        public async Task Update(OrderDetails orderDetail)
        {
            var entry = Context.Entry(orderDetail);
            if (entry.State == EntityState.Detached)
            {
                var id = new object[2];
                id[0] = orderDetail.OrderId;
                id[1] = orderDetail.ProductId;             

                var attachedOrderDetails = await GetById(id);
                if (attachedOrderDetails != null)
                {
                    Context.Entry(attachedOrderDetails).CurrentValues.SetValues(orderDetail);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(OrderDetails orderDetail)
        {
            EfDbSet.Remove(orderDetail);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IOrderDetailsRepository
    {
        Task Insert(OrderDetails orderDetail);
        Task Update(OrderDetails orderDetail);
        void Delete(OrderDetails orderDetail);
        void Save();
    }
}
