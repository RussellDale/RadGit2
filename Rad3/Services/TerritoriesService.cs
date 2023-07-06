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
    public class TerritoriesService : ITerritoriesService
    {
        private readonly DbContextOptions<dbContext> _options;

        public TerritoriesService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<Territories> GetTerritoriesGridRow(Action<IGridColumnCollection<Territories>> columns,
                                                           QueryDictionary<StringValues> query)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new TerritoriesRepository(context);

                var server = new GridServer<Territories>(repository.GetAll(), new QueryCollection(query),
                                false, "territoriesGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, false, true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public IEnumerable<SelectItem> GetAllTerritories()
        {
            using (var context = new dbContext(_options))
            {
                TerritoriesRepository repository = new TerritoriesRepository(context);
                return repository.GetAll()
                     .Select(r => new SelectItem(r.TerritoryId.ToString(), r.TerritoryId.ToString() + " - " + 
                                                 r.Region.RegionDescription + " - " +
                                                 r.TerritoryDescription))
                                               .ToList();
            }
        }

        public async Task<Territories> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new TerritoriesRepository(context);
                return await repository.GetById(keys);
            }
        }

        public async Task Insert(Territories item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new TerritoriesRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Territories item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new TerritoriesRepository(context);
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
                    var territories = await Get(keys);
                    var repository = new TerritoriesRepository(context);
                    repository.Delete(territories);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the territories");
                }
            }
        }
    }

    public interface ITerritoriesService : ICrudDataService<Territories>
    {
        ItemsDTO<Territories> GetTerritoriesGridRow(Action<IGridColumnCollection<Territories>> columns,
                                                    QueryDictionary<StringValues> query);
        IEnumerable<SelectItem> GetAllTerritories();
    }
}
