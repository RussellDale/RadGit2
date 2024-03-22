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
    public class CourseService : ICourseService
    {
        private readonly DbContextOptions<dbContext> _options;

        public CourseService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<Course> GetCourseGridRow(Action<IGridColumnCollection<Course>> columns,
                                                 QueryDictionary<StringValues> query)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new CourseRepository(context);

                var server = new GridServer<Course>(repository.GetAll(), new QueryCollection(query),
                        false, "courseGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }
        public ItemsDTO<Course> GetCourseIdGridRow(Action<IGridColumnCollection<Course>> columns,
                                                   QueryDictionary<StringValues> query, int departmentId)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new CourseRepository(context);

                var server = new GridServer<Course>(repository.GetForDepartment(departmentId), new QueryCollection(query),
                        false, "courseGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }
/*
        public ItemsDTO<Course> GetCourseInstructorIdGridRow(Action<IGridColumnCollection<Course>> columns,
                                                             QueryDictionary<StringValues> query, int instructorId)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new CourseRepository(context);

                var server = new GridServer<Course>(repository.GetForInstructor(instructorId), new QueryCollection(query),
                        false, "courseGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }
*/
        public IEnumerable<SelectItem> GetAllCourse()
        {
            using (var context = new dbContext(_options))
            {
                CourseRepository repository = new CourseRepository(context);

                return repository.GetAll()
                     .Select(r => new SelectItem(r.CourseId.ToString(), r.CourseId.ToString() + " - "
                       + r.Title))
                    .ToList();
            }
        }
        public IEnumerable<SelectItem> GetAllDepartmentId(int departmentId)
        {
            using (var context = new dbContext(_options))
            {
                CourseRepository repository = new CourseRepository(context);

                return repository.GetForDepartment(departmentId)
                     .Select(r => new SelectItem(r.CourseId.ToString(), r.CourseId.ToString() + " - "
                       + r.Title))
                    .ToList();
            }
        }
        public IEnumerable<Course> GetForCourse(int id)
        {
            using (var context = new dbContext(_options))
            {
                CourseRepository repository = new CourseRepository(context);
                return repository.GetForDepartment(id).OrderBy(r => r.Title);
            }
        }

        public async Task<Course> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                int CourseID;
                int.TryParse(keys[0].ToString(), out CourseID);
                var repository = new CourseRepository(context);
                return await repository.GetById(CourseID);
            }
        }

        public async Task Insert(Course item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new CourseRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Course item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new CourseRepository(context);
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
                    var Course = await Get(keys);
                    var repository = new CourseRepository(context);
                    repository.Delete(Course);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the employee");
                }
            }
        }
    }

    public interface ICourseService : ICrudDataService<Course>
    {
        ItemsDTO<Course> GetCourseGridRow(Action<IGridColumnCollection<Course>> columns,
                                                  QueryDictionary<StringValues> query);
        ItemsDTO<Course> GetCourseIdGridRow(Action<IGridColumnCollection<Course>> columns,
                                            QueryDictionary<StringValues> query, int departmentId);
        IEnumerable<SelectItem> GetAllCourse();
        IEnumerable<SelectItem> GetAllDepartmentId(int departmentId);
        IEnumerable<Course> GetForCourse(int id);
    }
}