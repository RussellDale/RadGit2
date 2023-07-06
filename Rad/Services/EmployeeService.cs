using Rad.Models.Domian;
using GridMvc.Server;
using GridShared;
using GridShared.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DbContextOptions<MyDbContext> _options;

        public EmployeeService(DbContextOptions<MyDbContext> options)
        {
            _options = options;
        }

        public ItemsDTO<Employee> GetEmployeeGridRow(Action<IGridColumnCollection<Employee>> columns,
                                              QueryDictionary<StringValues> query)
        {
            using (var context = new MyDbContext(_options))
            {
                var repository = new EmployeeRepository(context);

                var server = new GridServer<Employee>(repository.GetAll(), new QueryCollection(query),
                        false, "employeeGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }
        public IEnumerable<SelectItem> GetAllEmployee()
        {
            using (var context = new MyDbContext(_options))
            {
                EmployeeRepository repository = new EmployeeRepository(context);
                return repository.GetAll()
                    .Select(r => new SelectItem(r.EmployeeId.ToString(), r.EmployeeId.ToString() + " - "
                        + r.FirstName + " " + r.LastName))
                    .ToList();
            }
        }

        public async Task<Employee> Get(params object[] keys)
        {
            using (var context = new MyDbContext(_options))
            {
                int employeeId;
                int.TryParse(keys[0].ToString(), out employeeId);
                var repository = new EmployeeRepository(context);
                return await repository.GetById(employeeId);
            }
        }

        public async Task Insert(Employee item)
        {
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var repository = new EmployeeRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Employee item)
        {
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var repository = new EmployeeRepository(context);
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
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var employee = await Get(keys);
                    var repository = new EmployeeRepository(context);
                    repository.Delete(employee);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the employee");
                }
            }
        }
    }

 
    public interface IEmployeeService : ICrudDataService<Employee>
    {
        ItemsDTO<Employee> GetEmployeeGridRow(Action<IGridColumnCollection<Employee>> columns,
                                       QueryDictionary<StringValues> query);
        IEnumerable<SelectItem> GetAllEmployee();
    }
}
