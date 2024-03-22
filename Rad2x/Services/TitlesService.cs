using Rad2.Models.Domian;
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

namespace Rad2.Services
{
    public class TitlesService : ITitlesService
    {
        private readonly DbContextOptions<dbContext> _options;

        public TitlesService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<Titles> GetTitlesGridRow(Action<IGridColumnCollection<Titles>> columns,
                                                 QueryDictionary<StringValues> query, 
                                                 int Id)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new TitlesRepository(context);

                var server = new GridServer<Titles>(repository.GetForTitles2(Id), new QueryCollection(query),
                        false, "titlesGrid" + Id, columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public IEnumerable<SelectItem> GetAllTitles()
        {
            using (var context = new dbContext(_options))
            {
                TitlesRepository repository = new TitlesRepository(context);
                return repository.GetAll()//.Where(x => x.Id == x.Id2)
                     .Select(r => new SelectItem(r.Id.ToString(),
                        r.Id.ToString()  + " - " +
                        r.Id2.ToString() + " - " +
                        r.Title))
                    .ToList();
            }
        }

        public async Task<Titles> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                int titlesID;
                int.TryParse(keys[0].ToString(), out titlesID);
                var repository = new TitlesRepository(context);
                return await repository.GetById(titlesID);
            }
        }

        public async Task Insert(Titles item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new TitlesRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Titles item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new TitlesRepository(context);
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
                    var titles = await Get(keys);
                    var repository = new TitlesRepository(context);

                    IEnumerable<Titles> IEnumTitles = repository.GetForTitles2(titles.Id);

                    if(IEnumTitles.Count() == 0)
                       repository.Delete(titles);
                    else
                      throw new GridException("Error deleting the Titles - subTitles");

                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the Titles");
                }
            }
        }
    }

    public interface ITitlesService : ICrudDataService<Titles>
    {
        ItemsDTO<Titles> GetTitlesGridRow(Action<IGridColumnCollection<Titles>> columns,
                                          QueryDictionary<StringValues> query,
                                          int Id2);
        IEnumerable<SelectItem> GetAllTitles();
    }
}
