using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad.Models.Domian
{
    public class InvoiceLineRepository : SqlRepository<InvoiceLine>, IInvoiceLineRepository
    {
        public InvoiceLineRepository(MyDbContext context)
            : base(context)
        {
        }

        public override IQueryable<InvoiceLine> GetAll()
        {
            return EfDbSet
                .Include("Invoice")
                .Include("Track")
                .Include("Track.Genre")
                .Include("Track.MediaType")
                .Include("Track.Album")
                .Include("Track.Album.Artist")
                ;
        }

        public override async Task<InvoiceLine> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.InvoiceLineId == (int)id);
        }

        public IEnumerable<InvoiceLine> GetForInvoiceLine(int id)
        {
            return GetAll().Where(o => o.InvoiceId == id).ToList();
        }

        public async Task Insert(InvoiceLine invoiceLine)
        {
            await EfDbSet.AddAsync(invoiceLine);
        }

        public async Task Update(InvoiceLine invoiceLine)
        {
            var entry = Context.Entry(invoiceLine);
            if (entry.State == EntityState.Detached)
            {
                var attachedInvoiceLine = await GetById(invoiceLine.InvoiceLineId);
                if (attachedInvoiceLine != null)
                {
                    Context.Entry(attachedInvoiceLine).CurrentValues.SetValues(invoiceLine);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(InvoiceLine invoiceLine)
        {
            EfDbSet.Remove(invoiceLine);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IInvoiceLineRepository
    {
        Task Insert(InvoiceLine invoiceLine);
        Task Update(InvoiceLine invoiceLine);
        void Delete(InvoiceLine invoiceLine);
        void Save();
    }
}