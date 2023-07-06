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
    public class RegionsService : IRegionsService
    {
        private readonly DbContextOptions<dbContext> _options;

        public RegionsService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<Regions> GetRegionsGridRow(Action<IGridColumnCollection<Regions>> columns,
                                                         QueryDictionary<StringValues> query)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new RegionsRepository(context);

                var server = new GridServer<Regions>(repository.GetAll(), new QueryCollection(query),
                              false, "regionsGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, false, true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public IEnumerable<SelectItem> GetAllRegions()
        {
            using (var context = new dbContext(_options))
            {
                RegionsRepository repository = new RegionsRepository(context);
                return repository.GetAll()
                     .Select(r => new SelectItem(r.RegionId.ToString(), r.RegionId.ToString() + " - " + r.RegionDescription))
                                               .ToList();
            }
        }

        public async Task<Regions> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                int regionsID;
                int.TryParse(keys[0].ToString(), out regionsID);
                var repository = new RegionsRepository(context);
                return await repository.GetById(regionsID);
            }
        }

        public async Task Insert(Regions item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new RegionsRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Regions item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new RegionsRepository(context);
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
                    var regions = await Get(keys);
                    var repository = new RegionsRepository(context);
                    repository.Delete(regions);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the regions");
                }
            }
        }
    }

    public interface IRegionsService : ICrudDataService<Regions>
    {
        ItemsDTO<Regions> GetRegionsGridRow(Action<IGridColumnCollection<Regions>> columns,
                                            QueryDictionary<StringValues> query);
        IEnumerable<SelectItem> GetAllRegions();
    }
}
