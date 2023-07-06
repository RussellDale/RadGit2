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

namespace Rad3.Services
{
    public class ProductsService : IProductsService
    {
        private readonly DbContextOptions<dbContext> _options;

        public ProductsService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<Products> GetProductsGridRow(Action<IGridColumnCollection<Products>> columns,
                                                     QueryDictionary<StringValues> query)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new ProductsRepository(context);

                var server = new GridServer<Products>(repository.GetAll(), new QueryCollection(query),
                              false, "productsGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, false, true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public IEnumerable<SelectItem> GetAllProducts()
        {
            using (var context = new dbContext(_options))
            {
                ProductsRepository repository = new ProductsRepository(context);
                return repository.GetAll()
                     .Select(r => new SelectItem(r.ProductId.ToString(), r.ProductId.ToString() + " - " + r.ProductName))
                                               .ToList();
            }
        }

        public async Task<Products> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                int productID;
                int.TryParse(keys[0].ToString(), out productID);
                var repository = new ProductsRepository(context);
                return await repository.GetById(productID);
            }
        }

        public async Task Insert(Products item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new ProductsRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Products item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new ProductsRepository(context);
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
                    var product = await Get(keys);
                    var repository = new ProductsRepository(context);
                    repository.Delete(product);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the Products");
                }
            }
        }
    } 

    public interface IProductsService : ICrudDataService<Products>
    {
        ItemsDTO<Products> GetProductsGridRow(Action<IGridColumnCollection<Products>> columns,
                                              QueryDictionary<StringValues> query);
        IEnumerable<SelectItem> GetAllProducts();
    } 
}
