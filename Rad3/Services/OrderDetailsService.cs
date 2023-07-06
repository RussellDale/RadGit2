using Rad3.Models.Domian;
using GridCore.Server;
using GridMvc.Server;
using GridShared;
using GridShared.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad3.Services
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private readonly DbContextOptions<dbContext> _options;

        public OrderDetailsService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<OrderDetails> GetOrderDetailsGridRow(Action<IGridColumnCollection<OrderDetails>> columns,
                                                             QueryDictionary<StringValues> query,
                                                             int Id)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new OrderDetailsRepository(context);

                var server = new GridServer<OrderDetails>(repository.GetAll().Where(c => c.OrderId == Id),
                              new QueryCollection(query),
                              false, "orderDetailsGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, false, true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public IEnumerable<SelectItem> GetAllOrderDetails()
        {
            using (var context = new dbContext(_options))
            {
                OrderDetailsRepository repository = new OrderDetailsRepository(context);
                return repository.GetAll()
                     .Select(r => new SelectItem(r.OrderId.ToString(), r.OrderId.ToString()))
                                               .ToList();
            }
        }

        public async Task<OrderDetails> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new OrderDetailsRepository(context);
                return await repository.GetById(keys);
            }
        }

        public async Task Insert(OrderDetails item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new OrderDetailsRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(OrderDetails item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new OrderDetailsRepository(context);
                    await repository.Update(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException(e);
                }
            }
        }

        public async Task Delete(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var orderDetail = await Get(keys);
                    var repository = new OrderDetailsRepository(context);
                    repository.Delete(orderDetail);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the OrderDetails");
                }
            }
        }
    }

    public interface IOrderDetailsService : ICrudDataService<OrderDetails>
    {
        ItemsDTO<OrderDetails> GetOrderDetailsGridRow(Action<IGridColumnCollection<OrderDetails>> columns,
                                          QueryDictionary<StringValues> query, int Id);
        IEnumerable<SelectItem> GetAllOrderDetails();
    }
}
