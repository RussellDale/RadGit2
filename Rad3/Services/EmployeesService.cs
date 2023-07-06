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
    public class EmployeesService : IEmployeesService
    {
        private readonly DbContextOptions<dbContext> _options;

        public EmployeesService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<Employees> GetEmployeesGridRow(Action<IGridColumnCollection<Employees>> columns,
                                                       QueryDictionary<StringValues> query)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new EmployeesRepository(context);

                var server = new GridServer<Employees>(repository.GetAll(), new QueryCollection(query),
                              false, "employeesGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, false, true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }
        public ItemsDTO<Employees> GetEmployeesIdGridRow(Action<IGridColumnCollection<Employees>> columns,
                                                         QueryDictionary<StringValues> query, int Id)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new EmployeesRepository(context);

                var server = new GridServer<Employees>(repository.GetAll().Where(c => c.EmployeeId == Id),
                                                       new QueryCollection(query), false, "employeesIdGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, false, true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public IEnumerable<SelectItem> GetAllEmployees()
        {
            using (var context = new dbContext(_options))
            {
                EmployeesRepository repository = new EmployeesRepository(context);
                return repository.GetAll()
                     .Select(r => new SelectItem(r.EmployeeId.ToString(), r.EmployeeId.ToString() + " - " 
                               + r.FirstName + " " + r.LastName))
                              .ToList();
            }
        }

        public async Task<Employees> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                int employeeID;
                int.TryParse(keys[0].ToString(), out employeeID);
                var repository = new EmployeesRepository(context);
                return await repository.GetById(employeeID);
            }
        }

        public async Task Insert(Employees item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new EmployeesRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Employees item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new EmployeesRepository(context);
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
                    var employee = await Get(keys);
                    var repository = new EmployeesRepository(context);
                    repository.Delete(employee);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the Employees");
                }
            }
        }
    }

    public interface IEmployeesService : ICrudDataService<Employees>
    {
        ItemsDTO<Employees> GetEmployeesGridRow(Action<IGridColumnCollection<Employees>> columns,
                                                QueryDictionary<StringValues> query);
        ItemsDTO<Employees> GetEmployeesIdGridRow(Action<IGridColumnCollection<Employees>> columns,
                                                  QueryDictionary<StringValues> query, int Id);
        IEnumerable<SelectItem> GetAllEmployees();
    }
}
