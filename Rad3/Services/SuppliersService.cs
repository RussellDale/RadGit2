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
    public class SuppliersService : ISuppliersService
    {
        private readonly DbContextOptions<dbContext> _options;

        public SuppliersService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<Suppliers> GetSuppliersGridRow(Action<IGridColumnCollection<Suppliers>> columns,
                                                       QueryDictionary<StringValues> query)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new SuppliersRepository(context);

                var server = new GridServer<Suppliers>(repository.GetAll(), new QueryCollection(query),
                              false, "suppliersGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, false, true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public IEnumerable<SelectItem> GetAllSuppliers()
        {
            using (var context = new dbContext(_options))
            {
                SuppliersRepository repository = new SuppliersRepository(context);
                return repository.GetAll()
                     .Select(r => new SelectItem(r.SupplierId.ToString(), r.SupplierId.ToString() + " - " + r.CompanyName))
                                               .ToList();
            }
        }

        public async Task<Suppliers> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                int supplierID;
                int.TryParse(keys[0].ToString(), out supplierID);
                var repository = new SuppliersRepository(context);
                return await repository.GetById(supplierID);
            }
        }

        public async Task Insert(Suppliers item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new SuppliersRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Suppliers item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new SuppliersRepository(context);
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
                    var supplier = await Get(keys);
                    var repository = new SuppliersRepository(context);
                    repository.Delete(supplier);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the suppliers");
                }
            }
        }
    }

    public interface ISuppliersService : ICrudDataService<Suppliers>
    {
        ItemsDTO<Suppliers> GetSuppliersGridRow(Action<IGridColumnCollection<Suppliers>> columns,
                                                QueryDictionary<StringValues> query);
        IEnumerable<SelectItem> GetAllSuppliers();
    }
}
