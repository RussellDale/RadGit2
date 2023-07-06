using Rad.Models.Domian;
using GridMvc.Server;
using GridShared;
using GridShared.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly DbContextOptions<MyDbContext> _options;

        public CustomerService(DbContextOptions<MyDbContext> options)
        {
            _options = options;
        }

        public ItemsDTO<Customer> GetCustomerGridRow(Action<IGridColumnCollection<Customer>> columns,
                                                     QueryDictionary<StringValues> query)
        {
            using (var context = new MyDbContext(_options))
            {
                var repository = new CustomerRepository(context);

                var server = new GridServer<Customer>(repository.GetAll(), new QueryCollection(query),
                        false, "customerGrid", columns, 10)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public IEnumerable<SelectItem> GetAllCustomer()
        {
            using (var context = new MyDbContext(_options))
            {
                CustomerRepository repository = new CustomerRepository(context);
                return repository.GetAll()
                    .Select(r => new SelectItem(r.CustomerId.ToString(), r.CustomerId.ToString() + " - "
                        + r.Email))
                    .ToList();
            }
        }

        public async Task<Customer> Get(params object[] keys)
        {
            using (var context = new MyDbContext(_options))
            {
                int customerId;
                int.TryParse(keys[0].ToString(), out customerId);
                var repository = new CustomerRepository(context);
                return await repository.GetById(customerId);
            }
        }

        public async Task Insert(Customer item)
        {
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var repository = new CustomerRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Customer item)
        {
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var repository = new CustomerRepository(context);
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
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var customer = await Get(keys);
                    var repository = new CustomerRepository(context);
                    repository.Delete(customer);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the employee");
                }
            }
        }
    }

 
    public interface ICustomerService : ICrudDataService<Customer>
    {
        ItemsDTO<Customer> GetCustomerGridRow(Action<IGridColumnCollection<Customer>> columns,
                                              QueryDictionary<StringValues> query);
        IEnumerable<SelectItem> GetAllCustomer();
    }
}
