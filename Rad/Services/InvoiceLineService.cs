using Rad.Models.Domian;
using GridMvc.Server;
using GridShared;
using GridShared.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad.Services
{
    public class InvoiceLineService : IInvoiceLineService
    {
        private readonly DbContextOptions<MyDbContext> _options;

        public InvoiceLineService(DbContextOptions<MyDbContext> options)
        {
            _options = options;
        }

        public ItemsDTO<InvoiceLine> GetInvoiceLineId(Action<IGridColumnCollection<InvoiceLine>> columns,
                                                      QueryDictionary<StringValues> query,
                                                      int invoiceId)
        {
            using (var context = new MyDbContext(_options))
            {
                var repository = new InvoiceLineRepository(context);

                var server = new GridServer<InvoiceLine>(repository.GetForInvoiceLine(invoiceId), new QueryCollection(query),
                        false, "invoiceLineGrid" + invoiceId.ToString(), columns, 10)
                            .WithPaging(10)
                            .Sortable()
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }



        public async Task<InvoiceLine> Get(params object[] keys)
        {
            using (var context = new MyDbContext(_options))
            {
                int invoiceLineId;
                int.TryParse(keys[0].ToString(), out invoiceLineId);
                var repository = new InvoiceLineRepository(context);
                return await repository.GetById(invoiceLineId);
            }
        }

        public async Task Insert(InvoiceLine item)
        {
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var repository = new InvoiceLineRepository(context);
                    await repository.Insert(item);
                    repository.Save();

                    InvoiceLine(item);
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(InvoiceLine item)
        {
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var repository = new InvoiceLineRepository(context);
                    await repository.Update(item);
                    repository.Save();

                    InvoiceLine(item);
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
                    InvoiceLine item = await Get(keys);
                    var repository = new InvoiceLineRepository(context);
                    repository.Delete(item);
                    repository.Save();

                    InvoiceLine(item);
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the InvoiceLine");
                }
            }
        }

        private async void InvoiceLine(InvoiceLine item)
        {
            using (var context = new MyDbContext(_options))
            {
                var servicesInvoice = new InvoiceService(_options);
                var invoice = await servicesInvoice.Get(item.InvoiceId);
                await servicesInvoice.Update(invoice);
            }     
            
        }
    }

    public interface IInvoiceLineService : ICrudDataService<InvoiceLine>
    {
       public ItemsDTO<InvoiceLine> GetInvoiceLineId(Action<IGridColumnCollection<InvoiceLine>> columns,
                                              QueryDictionary<StringValues> query,
                                              int InvoiceLineId);
    }
}
