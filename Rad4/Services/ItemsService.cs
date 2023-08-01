using Rad4.Models.Domian;
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

namespace Rad4.Services
{
    public class ItemsService : IItemsService
    {
        private readonly DbContextOptions<dbContext> _options;

        public ItemsService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<Items> GetItemsGridRow(Action<IGridColumnCollection<Items>> columns,
                                               QueryDictionary<StringValues> query)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new ItemsRepository(context);

                var server = new GridServer<Items>(repository.GetAll(), new QueryCollection(query),
                              false, "ItemsGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, false, true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }
        public async Task<Items> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                int Id;
                int.TryParse(keys[0].ToString(), out Id);
                var repository = new ItemsRepository(context);
                return await repository.GetById(Id);
            }
        }

        public async Task Insert(Items item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new ItemsRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Items item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new ItemsRepository(context);
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
                    var Id = await Get(keys);
                    var repository = new ItemsRepository(context);
                    repository.Delete(Id);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the category");
                }
            }
        }
    }

    public interface IItemsService : ICrudDataService<Items>
    {
        ItemsDTO<Items> GetItemsGridRow(Action<IGridColumnCollection<Items>> columns,
                                        QueryDictionary<StringValues> query);
    }
}
