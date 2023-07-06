using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad.Models.Domian
{
    public class PlaylistTrackRepository : SqlRepository<PlaylistTrack>, IPlaylistTrackRepository
    {
        public PlaylistTrackRepository(MyDbContext context)
            : base(context)
        {
        }

        public override IQueryable<PlaylistTrack> GetAll()
        {
            return EfDbSet
                .Include("Playlist")
                .Include("Track")
                .Include("Track.MediaType")
                .Include("Track.Genre")
                .Include("Track.Album")
                .Include("Track.Album.Artist")
                ;
        }

        public override async Task<PlaylistTrack> GetById(object id)
        {
            PlaylistTrack p = GetById1((object[])id);

            return await GetAll().SingleOrDefaultAsync(c => (c.PlaylistId == p.PlaylistId && c.TrackId == p.TrackId));
        }

        private PlaylistTrack GetById1(object[] id)
        {
            int playlistId, playlistTrackId;

            int.TryParse(id[0].ToString(), out playlistId);
            int.TryParse(id[1].ToString(), out playlistTrackId);

            PlaylistTrack p = new PlaylistTrack();

            p.PlaylistId = playlistId;
            p.TrackId = playlistTrackId;

            return p;
        }

        public IEnumerable<PlaylistTrack> GetForPlaylistId(int id)
        {
            return GetAll().Where(o => o.PlaylistId == id).ToList();
        }

        public async Task Insert(PlaylistTrack playlistTrack)
        {
            await EfDbSet.AddAsync(playlistTrack);
        }

        public async Task Update(PlaylistTrack playlistTrack)
        {
            var entry = Context.Entry(playlistTrack);
            if (entry.State == EntityState.Detached)
            {
                var attachedPlaylistTrack = await GetById(playlistTrack.PlaylistId);
                if (attachedPlaylistTrack != null)
                {
                    Context.Entry(attachedPlaylistTrack).CurrentValues.SetValues(playlistTrack);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(PlaylistTrack playlistTrack)
        {
            EfDbSet.Remove(playlistTrack);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface IPlaylistTrackRepository
    {
        Task Insert(PlaylistTrack playlistTrack);
        Task Update(PlaylistTrack playlistTrack);
        void Delete(PlaylistTrack playlistTrack);
        void Save();
    }
}