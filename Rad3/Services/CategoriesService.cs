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
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Rad3.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly DbContextOptions<dbContext> _options;

        public CategoriesService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<Categories> GetCategoriesGridRow(Action<IGridColumnCollection<Categories>> columns,
                                                         QueryDictionary<StringValues> query)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new CategoriesRepository(context);

                var server = new GridServer<Categories>(repository.GetAll(), new QueryCollection(query),
                              false, "categoriesGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, false, true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public IEnumerable<SelectItem> GetAllCategories()
        {
            using (var context = new dbContext(_options))
            {
                CategoriesRepository repository = new CategoriesRepository(context);
                return repository.GetAll()
                     .Select(r => new SelectItem(r.CategoryId.ToString(), r.CategoryId.ToString() + " - " + r.CategoryName))
                                               .ToList();
            }
        }

        public async Task<Categories> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                int categoryID;
                int.TryParse(keys[0].ToString(), out categoryID);
                var repository = new CategoriesRepository(context);
                return await repository.GetById(categoryID);
            }
        }

        public async Task Insert(Categories item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new CategoriesRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Categories item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new CategoriesRepository(context);
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
                    var category = await Get(keys);
                    var repository = new CategoriesRepository(context);
                    repository.Delete(category);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the category");
                }
            }
        }
    }

    public interface ICategoriesService : ICrudDataService<Categories>
    {
        ItemsDTO<Categories> GetCategoriesGridRow(Action<IGridColumnCollection<Categories>> columns,
                                                  QueryDictionary<StringValues> query);
        IEnumerable<SelectItem> GetAllCategories();
    }
}
