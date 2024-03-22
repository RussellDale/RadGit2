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
    public class StudentService : IStudentService
    {
        private readonly DbContextOptions<dbContext> _options;

        public StudentService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<Student> GetStudentGridRow(Action<IGridColumnCollection<Student>> columns,
                                                   QueryDictionary<StringValues> query)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new StudentRepository(context);

                var server = new GridServer<Student>(repository.GetAll(), new QueryCollection(query),
                        false, "instructorGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public IEnumerable<SelectItem> GetAllStudent()
        {
            using (var context = new dbContext(_options))
            {
                StudentRepository repository = new StudentRepository(context);
                return repository.GetAll()
                     .Select(r => new SelectItem(r.Id.ToString(), r.Id.ToString() + " - "
                       + r.FirstName + " " + r.LastName))
                    .ToList();
            }
        }

        public async Task<Student> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                int studentID;
                int.TryParse(keys[0].ToString(), out studentID);
                var repository = new StudentRepository(context);
                return await repository.GetById(studentID);
            }
        }

        public async Task Insert(Student item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new StudentRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Student item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new StudentRepository(context);
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
                    var student = await Get(keys);
                    var repository = new StudentRepository(context);
                    repository.Delete(student);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the employee");
                }
            }
        }
    }

    public interface IStudentService : ICrudDataService<Student>
    {
        ItemsDTO<Student> GetStudentGridRow(Action<IGridColumnCollection<Student>> columns,
                                            QueryDictionary<StringValues> query);
        IEnumerable<SelectItem> GetAllStudent();
    }
}