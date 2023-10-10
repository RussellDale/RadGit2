using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad.Models.Domian
{
    public class InvoiceRepository : SqlRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(MyDbContext context)
            : base(context)
        {
        }

        public override IQueryable<Invoice> GetAll()
        {
            return EfDbSet
                .Include("InvoiceLines")
                .Include("InvoiceLines.Track")
                .Include("InvoiceLines.Track.Album")
                .Include("InvoiceLines.Track.Album.Artist")
                .Include("Customer")
                ;
        }

        public override async Task<Invoice> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.InvoiceId == (int)id);
        }

        public IEnumerable<Invoice> GetForInvoice(int id)
        {
            return GetAll().Where(o => o.InvoiceId == id).ToList();
        }

        public async Task Insert(Invoice invoice)
        {
            await EfDbSet.AddAsync(invoice);
        }

        public async Task Update(Invoice invoice)
        {
            var entry = Context.Entry(invoice);
            if (entry.State == EntityState.Detached)
            {
                var attachedInvoice = await GetById(invoice.InvoiceId);
                if (attachedInvoice != null)
                {
                    Context.Entry(attachedInvoice).CurrentValues.SetValues(invoice);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Invoice invoice)
        {
            EfDbSet.Remove(invoice);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IInvoiceRepository
    {
        Task Insert(Invoice invoice);
        Task Update(Invoice invoice);
        void Delete(Invoice invoice);
        void Save();
    }
}