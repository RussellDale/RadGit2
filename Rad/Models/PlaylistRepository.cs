using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad.Models.Domian
{
    public class PlaylistRepository : SqlRepository<Playlist>, IPlaylistRepository
    {
        public PlaylistRepository(MyDbContext context)
            : base(context)
        {
        }

        public override IQueryable<Playlist> GetAll()
        {
            return EfDbSet
                .Include("PlaylistTracks")
                .Include("PlaylistTracks.Track")
                .Include("PlaylistTracks.Track.Album")
                .Include("PlaylistTracks.Track.Album.Artist")
                ;
        }

        public override async Task<Playlist> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.PlaylistId == (int)id);
        }

        public IEnumerable<Playlist> GetForPlaylist(int id)
        {
            return GetAll().Where(o => o.PlaylistId == id).ToList();
        }

        public async Task Insert(Playlist playlist)
        {
            await EfDbSet.AddAsync(playlist);
        }

        public async Task Update(Playlist playlist)
        {
            var entry = Context.Entry(playlist);
            if (entry.State == EntityState.Detached)
            {
                var attachedPlaylist = await GetById(playlist.PlaylistId);
                if (attachedPlaylist != null)
                {
                    Context.Entry(attachedPlaylist).CurrentValues.SetValues(playlist);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Playlist playlist)
        {
            EfDbSet.Remove(playlist);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IPlaylistRepository
    {
        Task Insert(Playlist playlist);
        Task Update(Playlist playlist);
        void Delete(Playlist playlist);
        void Save();
    }
}