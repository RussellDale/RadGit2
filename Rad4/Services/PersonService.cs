using Rad4.Models.Domian;
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

namespace Rad4.Services
{
    public class PersonService : IPersonService
    {
        private readonly DbContextOptions<dbContext> _options;

        public PersonService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<Person> GetPersonGridRow(Action<IGridColumnCollection<Person>> columns,
                                                 QueryDictionary<StringValues> query)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new PersonRepository(context);

                var server = new GridServer<Person>(repository.GetAll(), new QueryCollection(query),
                              false, "personGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, false, true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }
        public async Task<Person> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                int Id;
                int.TryParse(keys[0].ToString(), out Id);
                var repository = new PersonRepository(context);
                return await repository.GetById(Id);
            }
        }

        public async Task Insert(Person item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new PersonRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Person item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new PersonRepository(context);
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
                    var Id = await Get(keys);
                    var repository = new PersonRepository(context);
                    repository.Delete(Id);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the category");
                }
            }
        }
    }

    public interface IPersonService : ICrudDataService<Person>
    {
        ItemsDTO<Person> GetPersonGridRow(Action<IGridColumnCollection<Person>> columns,
                                          QueryDictionary<StringValues> query);
    }
}
