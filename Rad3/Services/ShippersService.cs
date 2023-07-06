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
    public class ShippersService : IShippersService
    {
        private readonly DbContextOptions<dbContext> _options;

        public ShippersService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<Shippers> GetShippersGridRow(Action<IGridColumnCollection<Shippers>> columns,
                                                     QueryDictionary<StringValues> query)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new ShippersRepository(context);

                var server = new GridServer<Shippers>(repository.GetAll(), new QueryCollection(query),
                              false, "shippersGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, false, true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public IEnumerable<SelectItem> GetAllShippers()
        {
            using (var context = new dbContext(_options))
            {
                ShippersRepository repository = new ShippersRepository(context);
                return repository.GetAll()
                     .Select(r => new SelectItem(r. ShipperId.ToString(), r. ShipperId.ToString() + " - " + r.CompanyName))
                                               .ToList();
            }
        }

        public async Task<Shippers> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                int shipperID;
                int.TryParse(keys[0].ToString(), out shipperID);
                var repository = new ShippersRepository(context);
                return await repository.GetById(shipperID);
            }
        }

        public async Task Insert(Shippers item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new ShippersRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Shippers item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new ShippersRepository(context);
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
                    var shipper = await Get(keys);
                    var repository = new ShippersRepository(context);
                    repository.Delete(shipper);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the shippers");
                }
            }
        }
    }

    public interface IShippersService : ICrudDataService<Shippers>
    {
        ItemsDTO<Shippers> GetShippersGridRow(Action<IGridColumnCollection<Shippers>> columns,
                                              QueryDictionary<StringValues> query);
        IEnumerable<SelectItem> GetAllShippers();
    }
}
