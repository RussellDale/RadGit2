using Ra.Models.Domian;
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

namespace Ra.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly DbContextOptions<MyDbContext> _options;

        public DepartmentService(DbContextOptions<MyDbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<Department> GetDepartmentGridRow(Action<IGridColumnCollection<Department>> columns,
                                                         QueryDictionary<StringValues> query)
        {
            using (var context = new MyDbContext(_options))
            {
                var repository = new DepartmentRepository(context);

                var server = new GridServer<Department>(repository.GetAll(), new QueryCollection(query),
                        false, "departmentGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public IEnumerable<SelectItem> GetAllDepartment()
        {
            using (var context = new MyDbContext(_options))
            {
                DepartmentRepository repository = new DepartmentRepository(context);
                return repository.GetAll()
                     .Select(r => new SelectItem(r.InstructorID.ToString(), r.InstructorID.ToString() + " - "
                       + ""))
                    .ToList();
            }
        }

        public async Task<Department> Get(params object[] keys)
        {
            using (var context = new MyDbContext(_options))
            {
                int departmentID;
                int.TryParse(keys[0].ToString(), out departmentID);
                var repository = new DepartmentRepository(context);
                return await repository.GetById(departmentID);
            }
        }

        public async Task Insert(Department item)
        {
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var repository = new DepartmentRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Department item)
        {
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var repository = new DepartmentRepository(context);
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
                    var department = await Get(keys);
                    var repository = new DepartmentRepository(context);
                    repository.Delete(department);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the employee");
                }
            }
        }
    }

    public interface IDepartmentService : ICrudDataService<Department>
    {
        ItemsDTO<Department> GetDepartmentGridRow(Action<IGridColumnCollection<Department>> columns,
                                                  QueryDictionary<StringValues> query);
        IEnumerable<SelectItem> GetAllDepartment();
    }
}