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
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Rad3.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly DbContextOptions<dbContext> _options;

        public OrdersService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<Orders> GetOrdersGridRow(Action<IGridColumnCollection<Orders>> columns,
                                                 QueryDictionary<StringValues> query)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new OrdersRepository(context);

                var server = new GridServer<Orders>(repository.GetAll().Include(r => r.OrderDetails), new QueryCollection(query),
                              false, "OrdersGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, false, true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }
        public ItemsDTO<Orders> GetOrdersEGridRow(Action<IGridColumnCollection<Orders>> columns,
                                                 QueryDictionary<StringValues> query, int Id)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new OrdersRepository(context);

                var server = new GridServer<Orders>(repository.GetAll()
                                                              .Include(r => r.OrderDetails)
                                                              .Where(c => c.EmployeeId == Id), 
                                                    new QueryCollection(query),
                              false, "OrdersGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, false, true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public ItemsDTO<Orders> GetOrdersCGridRow(Action<IGridColumnCollection<Orders>> columns,
                                                  QueryDictionary<StringValues> query, string Id)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new OrdersRepository(context);

                var server = new GridServer<Orders>(repository.GetAll()
                                                              .Include(r => r.OrderDetails)
                                                              .Where(c => c.CustomerId == Id),
                                                    new QueryCollection(query),
                              false, "OrdersGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, false, true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public IEnumerable<SelectItem> GetAllOrders()
        {
            using (var context = new dbContext(_options))
            {
                OrdersRepository repository = new OrdersRepository(context);
                return repository.GetAll()
                     .Select(r => new SelectItem(r.OrderId.ToString(), r.OrderId.ToString()))
                                               .ToList();
            }
        }

        public async Task<Orders> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                int orderID;
                int.TryParse(keys[0].ToString(), out orderID);
                var repository = new OrdersRepository(context);
                return await repository.GetById(orderID);
            }
        }

        public async Task Insert(Orders item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new OrdersRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Orders item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new OrdersRepository(context);
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
                    var order = await Get(keys);
                    var repository = new OrdersRepository(context);
                    repository.Delete(order);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the Orders");
                }
            }
        }
    }

    public interface IOrdersService : ICrudDataService<Orders>
    {
        ItemsDTO<Orders> GetOrdersGridRow(Action<IGridColumnCollection<Orders>> columns,
                                          QueryDictionary<StringValues> query);
        ItemsDTO<Orders> GetOrdersEGridRow(Action<IGridColumnCollection<Orders>> columns,
                                           QueryDictionary<StringValues> query, int Id);
        ItemsDTO<Orders> GetOrdersCGridRow(Action<IGridColumnCollection<Orders>> columns,
                                           QueryDictionary<StringValues> query, string Id);

        IEnumerable<SelectItem> GetAllOrders();
    }
}
