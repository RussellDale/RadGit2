using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad.Models.Domian
{
    public class AlbumRepository : SqlRepository<Album>, IAlbumRepository
    {
        public AlbumRepository(MyDbContext context)
            : base(context)
        {
        }

        public override IQueryable<Album> GetAll()
        {
            return EfDbSet.Include("Artist");
        }

        public override async Task<Album> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.AlbumId == (int)id);
        }

        public IEnumerable<Album> GetForAlbum(int id)
        {
            return GetAll().Where(o => o.ArtistId == id).ToList();
        }

        public async Task Insert(Album album)
        {
            await EfDbSet.AddAsync(album);
        }

        public async Task Update(Album album)
        {
            var entry = Context.Entry(album);
            if (entry.State == EntityState.Detached)
            {
                var attachedAlbum = await GetById(album.AlbumId);
                if (attachedAlbum != null)
                {
                    Context.Entry(attachedAlbum).CurrentValues.SetValues(album);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Album album)
        {
            EfDbSet.Remove(album);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IAlbumRepository
    {
        Task Insert(Album album);
        Task Update(Album album);
        void Delete(Album album);
        void Save();
    }
}