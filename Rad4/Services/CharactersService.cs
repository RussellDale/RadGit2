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
    public class CharactersService : ICharactersService
    {
        private readonly DbContextOptions<dbContext> _options;

        public CharactersService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<Characters> GetCharactersGridRow(Action<IGridColumnCollection<Characters>> columns,
                                                         QueryDictionary<StringValues> query)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new CharactersRepository(context);

                var server = new GridServer<Characters>(repository.GetAll(), new QueryCollection(query),
                              false, "charactersGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, false, true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }
        public async Task<Characters> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                int Id;
                int.TryParse(keys[0].ToString(), out Id);
                var repository = new CharactersRepository(context);
                return await repository.GetById(Id);
            }
        }

        public async Task Insert(Characters item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new CharactersRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Characters item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new CharactersRepository(context);
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
                    var repository = new CharactersRepository(context);
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

    public interface ICharactersService : ICrudDataService<Characters>
    {
        ItemsDTO<Characters> GetCharactersGridRow(Action<IGridColumnCollection<Characters>> columns,
                                                  QueryDictionary<StringValues> query);
    }
}
