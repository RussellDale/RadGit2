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
using System.Collections.Immutable;

namespace Rad2.Services
{
    public class CourseAssignmentService : ICourseAssignmentService
    {
        private readonly DbContextOptions<dbContext> _options;

        public CourseAssignmentService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<CourseAssignment> GetCourseAssignmentGridRow(Action<IGridColumnCollection<CourseAssignment>> columns,
                                                                     QueryDictionary<StringValues> query,
                                                                     int instructorId)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new CourseAssignmentRepository(context);

                var server = new GridServer<CourseAssignment>(repository.GetForInstructorId(instructorId), new QueryCollection(query),
                        true, "CourseAssignmentGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public ItemsDTO<CourseAssignment> GetCourseAssignmentIdGridRow(Action<IGridColumnCollection<CourseAssignment>> columns,
                                                                       QueryDictionary<StringValues> query,
                                                                       int courseId)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new CourseAssignmentRepository(context);

                var server = new GridServer<CourseAssignment>(repository.GetForCourseId(courseId), new QueryCollection(query),
                        true, "CourseAssignmentGridId", columns)
                            .WithPaging(10)
                            .Sortable()
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public async Task<CourseAssignment> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new CourseAssignmentRepository(context);
                return await repository.GetById(keys);
            }
        }

        public async Task Insert(CourseAssignment item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new CourseAssignmentRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(CourseAssignment item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new CourseAssignmentRepository(context);
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
                    var CourseAssignment = await Get(keys);
                    var repository = new CourseAssignmentRepository(context);
                    repository.Delete(CourseAssignment);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the office assignment");
                }
            }
        }
    }

    public interface ICourseAssignmentService : ICrudDataService<CourseAssignment>
    {
        ItemsDTO<CourseAssignment> GetCourseAssignmentGridRow(Action<IGridColumnCollection<CourseAssignment>> columns,
                                                              QueryDictionary<StringValues> query, int instructorId);

        ItemsDTO<CourseAssignment> GetCourseAssignmentIdGridRow(Action<IGridColumnCollection<CourseAssignment>> columns,
                                                                QueryDictionary<StringValues> query,
                                                                int courseId);
    }
}