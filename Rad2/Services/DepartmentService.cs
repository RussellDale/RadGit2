﻿using Rad2.Models.Domian;
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
    public class DepartmentService : IDepartmentService
    {
        private readonly DbContextOptions<dbContext> _options;

        public DepartmentService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<Department> GetDepartmentGridRow(Action<IGridColumnCollection<Department>> columns,
                                                         QueryDictionary<StringValues> query, bool isName, string name)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new DepartmentRepository(context);
                
                IQueryable<Department> department = isName == true ? 
                 repository.GetAll().Where(c => c.Name == name ) : 
                 repository.GetAll();
                
                var server = new GridServer<Department>(department, new QueryCollection(query),
                        false, "departmentGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, false, true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public IEnumerable<SelectItem> GetAllDepartment()
        {
            using (var context = new dbContext(_options))
            {
                DepartmentRepository repository = new DepartmentRepository(context);
                return repository.GetAll()
                     .Select(r => new SelectItem(r.DepartmentId.ToString(), r.DepartmentId.ToString() + " - " + r.Name))
                                               .ToList();
            }
        }

        public async Task<Department> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                int departmentID;
                int.TryParse(keys[0].ToString(), out departmentID);
                var repository = new DepartmentRepository(context);
                return await repository.GetById(departmentID);
            }
        }

        public async Task Insert(Department item)
        {
            using (var context = new dbContext(_options))
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
            using (var context = new dbContext(_options))
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
            using (var context = new dbContext(_options))
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
                                                  QueryDictionary<StringValues> query, bool isName, string name);
        IEnumerable<SelectItem> GetAllDepartment();
    }
}