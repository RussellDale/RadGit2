using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rad.Models.Domian
{
    public class TrackRepository : SqlRepository<Track>, ITrackRepository
    {
        private PlaylistTrackRepository PlaylistTrack { get; set; }

        public TrackRepository(MyDbContext context)
            : base(context)
        {
            PlaylistTrack = new PlaylistTrackRepository(context);
        }

        public override IQueryable<Track> GetAll()
        {
            return EfDbSet
                .Include("Album")
                .Include("Album.Artist")
                .Include("MediaType")
                .Include("Genre")
                ;
        }

        public override async Task<Track> GetById(object id)
        {
            return await GetAll().SingleOrDefaultAsync(c => c.TrackId == (int)id);
        }

        public IEnumerable<Track> GetForTrack(int id)
        {
            return GetAll().Where(o => o.AlbumId == id).ToList();
        }

        public async Task Insert(Track track)
        {
            await EfDbSet.AddAsync(track);
        }

        public async Task Update(Track track)
        {
            var entry = Context.Entry(track);
            if (entry.State == EntityState.Detached)
            {
                var attachedTrack = await GetById(track.TrackId);
                if (attachedTrack != null)
                {
                    Context.Entry(attachedTrack).CurrentValues.SetValues(track);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }
        }

        public void Delete(Track track)
        {
            EfDbSet.Remove(track);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }

    public interface ITrackRepository
    {
        Task Insert(Track track);
        Task Update(Track track);
        void Delete(Track track);
        void Save();
    }
}