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
    public class T1Service : IT1Service
    {
        private readonly DbContextOptions<dbContext> _options;

        public T1Service(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<T1> GetCharactersGridRow(Action<IGridColumnCollection<T1>> columns,
                                                         QueryDictionary<StringValues> query)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new T1Repository(context);

                var server = new GridServer<T1>(repository.GetAll(), new QueryCollection(query),
                              false, "charactersGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, false, true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }
        public async Task<T1> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                int Id;
                int.TryParse(keys[0].ToString(), out Id);
                var repository = new T1Repository(context);
                return await repository.GetById(Id);
            }
        }

        public async Task Insert(T1 item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new T1Repository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(T1 item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new T1Repository(context);
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
                    var repository = new T1Repository(context);
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

    public interface IT1Service : ICrudDataService<T1>
    {
        ItemsDTO<T1> GetCharactersGridRow(Action<IGridColumnCollection<T1>> columns,
                                          QueryDictionary<StringValues> query);
    }
}
