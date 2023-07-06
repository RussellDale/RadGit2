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
    public class EmployeeTerritoriesService : IEmployeeTerritoriesService
    {
        private readonly DbContextOptions<dbContext> _options;

        public EmployeeTerritoriesService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<EmployeeTerritories> GetTerritoriesGridRowId(Action<IGridColumnCollection<EmployeeTerritories>> columns,
                                                                     QueryDictionary<StringValues> query,
                                                                     int Id)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new EmployeeTerritoriesRepository(context);

                var server = new GridServer<EmployeeTerritories>(repository.GetAll().Where(c => c.EmployeeId == Id),
                              new QueryCollection(query),
                              false, "territoriesGridId", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, false, true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }
        public ItemsDTO<EmployeeTerritories> GetEmployeeGridRowId(Action<IGridColumnCollection<EmployeeTerritories>> columns,
                                                                           QueryDictionary<StringValues> query,
                                                                           string Id)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new EmployeeTerritoriesRepository(context);

                var server = new GridServer<EmployeeTerritories>(repository.GetAll().Where(c => c.TerritoryId == Id),
                              new QueryCollection(query),
                              false, "employeeGridId", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, false, true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public async Task<EmployeeTerritories> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new EmployeeTerritoriesRepository(context);
                return await repository.GetById(keys);
            }
        }

        public async Task Insert(EmployeeTerritories item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new EmployeeTerritoriesRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(EmployeeTerritories item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new EmployeeTerritoriesRepository(context);
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
                    var employeeTerritories = await Get(keys);
                    var repository = new EmployeeTerritoriesRepository(context);
                    repository.Delete(employeeTerritories);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the OrderDetails");
                }
            }
        }
    }

    public interface IEmployeeTerritoriesService : ICrudDataService<EmployeeTerritories>
    {
        ItemsDTO<EmployeeTerritories> GetTerritoriesGridRowId(Action<IGridColumnCollection<EmployeeTerritories>> columns,
                                                              QueryDictionary<StringValues> query, int Id);
        ItemsDTO<EmployeeTerritories> GetEmployeeGridRowId(Action<IGridColumnCollection<EmployeeTerritories>> columns,
                                                           QueryDictionary<StringValues> query, string Id);
    }
}
