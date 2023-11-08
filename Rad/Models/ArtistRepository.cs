using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Rad.Models.Domian
{
    public class ArtistRepository : SqlRepository<Artist>, IArtistRepository
    {
        public ArtistRepository(MyDbContext context)
            : base(context)
        {
        }

        public override IQueryable<Artist> GetAll()
        {
            return EfDbSet
//                .Include(c => c.Albums)
                //                .Include("Artist.Album")
                //                .Include("Track.Album")
                //                .Include("Track")
                ;
        }

        public override async Task<Artist> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.ArtistId == (int)id);
        }

        public async Task Insert(Artist artist)
        {
            await EfDbSet.AddAsync(artist);
        }

        public async Task Update(Artist artist)
        {
            var entry = Context.Entry(artist);
            if (entry.State == EntityState.Detached)
            {
                var attachedArtist = await GetById(artist.ArtistId);
                if (attachedArtist != null)
                {
                    Context.Entry(attachedArtist).CurrentValues.SetValues(artist);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Artist artist)
        {
            EfDbSet.Remove(artist);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IArtistRepository
    {
        Task Insert(Artist artist);
        Task Update(Artist artist);
        void Delete(Artist artist);
        void Save();
    }
}