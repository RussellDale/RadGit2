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
    public class InstructorService : IInstructorService
    {
        private readonly DbContextOptions<dbContext> _options;

        public InstructorService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<Instructor> GetInstructorGridRow(Action<IGridColumnCollection<Instructor>> columns,
                                                         QueryDictionary<StringValues> query, bool isName, string name)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new InstructorRepository(context);

                IQueryable<Instructor> instructor = isName == true ? 
                 repository.GetAll().Where(c => c.FirstName + " " + c.LastName == name ) : 
                 repository.GetAll();
                
                var server = new GridServer<Instructor>(instructor, new QueryCollection(query),
                        false, "instructorGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public IEnumerable<SelectItem> GetAllInstructor()
        {
            using (var context = new dbContext(_options))
            {
                InstructorRepository repository = new InstructorRepository(context);
                return repository.GetAll()
                     .Select(r => new SelectItem(r.Id.ToString(), r.Id.ToString() + " - "
                       + r.FirstName + " " + r.LastName))
                    .ToList();
            }
        }

        public async Task<Instructor> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                int instructorID;
                int.TryParse(keys[0].ToString(), out instructorID);
                var repository = new InstructorRepository(context);
                return await repository.GetById(instructorID);
            }
        }

        public async Task Insert(Instructor item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new InstructorRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Instructor item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new InstructorRepository(context);
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
                    var instructor = await Get(keys);
                    var repository = new InstructorRepository(context);
                    repository.Delete(instructor);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the employee");
                }
            }
        }
    }

    public interface IInstructorService : ICrudDataService<Instructor>
    {
        ItemsDTO<Instructor> GetInstructorGridRow(Action<IGridColumnCollection<Instructor>> columns,
                                                  QueryDictionary<StringValues> query, bool isName, string name);
        IEnumerable<SelectItem> GetAllInstructor();
    }
}