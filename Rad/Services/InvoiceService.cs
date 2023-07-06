using Rad.Models.Domian;
using GridMvc.Server;
using GridShared;
using GridShared.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly DbContextOptions<MyDbContext> _options;

        public InvoiceService(DbContextOptions<MyDbContext> options)
        {
            _options = options;
        }

        public ItemsDTO<Invoice> GetInvoiceGridRows(Action<IGridColumnCollection<Invoice>> columns,
                                                    QueryDictionary<StringValues> query)
        {
            using (var context = new MyDbContext(_options))
            {
                var repository = new InvoiceRepository(context);

                var server = new GridServer<Invoice>(repository.GetAll(), new QueryCollection(query),
                        true, "invoiceGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true, true)
                ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public async Task<Invoice> Get(params object[] keys)
        {
            using (var context = new MyDbContext(_options))
            {
                int invoiceId;
                int.TryParse(keys[0].ToString(), out invoiceId);
                var repository = new InvoiceRepository(context);
                return await repository.GetById(invoiceId);
            }
        }

        public async Task Insert(Invoice item)
        {
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var repository = new InvoiceRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Invoice item)
        {
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    decimal unitPrice = 0;

                    foreach (InvoiceLine invoiceLine in item.InvoiceLines)
                        unitPrice += (invoiceLine.Quantity * invoiceLine.UnitPrice);

                    item.Total = unitPrice;

                    var repository = new InvoiceRepository(context);
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
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var invoice = await Get(keys);
                    var repositoryLine = new InvoiceLineService(_options);

                    foreach (InvoiceLine invoiceLine in invoice.InvoiceLines)
                        await repositoryLine.Delete(invoiceLine.InvoiceLineId);

                    invoice.InvoiceLines = null;

                    var repository = new InvoiceRepository(context);
                    repository.Delete(invoice);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the Invoice");
                }
            }
        }
    }

    public interface IInvoiceService : ICrudDataService<Invoice>
    {
         ItemsDTO<Invoice> GetInvoiceGridRows(Action<IGridColumnCollection<Invoice>> columns,
                                              QueryDictionary<StringValues> query);
    }
}
