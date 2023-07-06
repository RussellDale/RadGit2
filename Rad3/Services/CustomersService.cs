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
    public class CustomersService : ICustomersService
    {
        private readonly DbContextOptions<dbContext> _options;

        public CustomersService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<Customers> GetCustomersGridRow(Action<IGridColumnCollection<Customers>> columns,
                                                       QueryDictionary<StringValues> query)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new CustomersRepository(context);

                var server = new GridServer<Customers>(repository.GetAll(), new QueryCollection(query),
                              false, "customersGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, false, true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public IEnumerable<SelectItem> GetAllCustomers()
        {
            using (var context = new dbContext(_options))
            {
                CustomersRepository repository = new CustomersRepository(context);
                return repository.GetAll()
                     .Select(r => new SelectItem(r.CustomerId.ToString(), r.CustomerId.ToString() + " - " + r.CompanyName))
                                               .ToList();
            }
        }

        public async Task<Customers> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new CustomersRepository(context);
                return await repository.GetById(keys);
            }
        }

        public async Task Insert(Customers item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new CustomersRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Customers item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new CustomersRepository(context);
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
                    var customers = await Get(keys);
                    var repository = new CustomersRepository(context);
                    repository.Delete(customers);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the Customers");
                }
            }
        }
    }

    public interface ICustomersService : ICrudDataService<Customers>
    {
        ItemsDTO<Customers> GetCustomersGridRow(Action<IGridColumnCollection<Customers>> columns,
                                                QueryDictionary<StringValues> query);
        IEnumerable<SelectItem> GetAllCustomers();
    }
}
