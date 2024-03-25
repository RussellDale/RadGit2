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
    public class EnrollmentService : IEnrollmentService
    {
        private readonly DbContextOptions<dbContext> _options;

        public EnrollmentService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<Enrollment> GetEnrollmentGridRow(Action<IGridColumnCollection<Enrollment>> columns,
                                                 QueryDictionary<StringValues> query)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new EnrollmentRepository(context);

                var server = new GridServer<Enrollment>(repository.GetAll(), new QueryCollection(query),
                        false, "EnrollmentGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public ItemsDTO<Enrollment> GetEnrollmentIdGridRow(Action<IGridColumnCollection<Enrollment>> columns,
                                                           QueryDictionary<StringValues> query, int Id)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new EnrollmentRepository(context);

                var server = new GridServer<Enrollment>(repository.GetForStudent(Id), new QueryCollection(query),
                        false, "enrollmentGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }
        public IEnumerable<Enrollment> GetAllEnrollment(int id)
        {
            using (var context = new dbContext(_options))
            {
                EnrollmentRepository repository = new EnrollmentRepository(context);
                return repository.GetAll().Where(o => o.StudentId == id).ToList();
            }
        }

        public async Task<Enrollment> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                int EnrollmentID;
                int.TryParse(keys[0].ToString(), out EnrollmentID);
                var repository = new EnrollmentRepository(context);
                return await repository.GetById(EnrollmentID);
            }
        }

        public async Task Insert(Enrollment item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new EnrollmentRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Enrollment item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new EnrollmentRepository(context);
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
                    var Enrollment = await Get(keys);
                    var repository = new EnrollmentRepository(context);
                    repository.Delete(Enrollment);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the employee");
                }
            }
        }
    }

    public interface IEnrollmentService : ICrudDataService<Enrollment>
    {
        ItemsDTO<Enrollment> GetEnrollmentGridRow(Action<IGridColumnCollection<Enrollment>> columns,
                                                  QueryDictionary<StringValues> query);
        ItemsDTO<Enrollment> GetEnrollmentIdGridRow(Action<IGridColumnCollection<Enrollment>> columns,
                                                    QueryDictionary<StringValues> query, int Id);
        IEnumerable<Enrollment> GetAllEnrollment(int id);
    }
}