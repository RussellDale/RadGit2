using Rad.Models.Domian;
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

namespace Rad.Services
{
    public class PlaylistTrackService : IPlaylistTrackService
    {
        private readonly DbContextOptions<MyDbContext> _options;

        public PlaylistTrackService(DbContextOptions<MyDbContext> options)
        {
            _options = options;
        }

        public ItemsDTO<PlaylistTrack> GetPlaylistTrackGridRows(Action<IGridColumnCollection<PlaylistTrack>> columns,
                                                                QueryDictionary<StringValues> query,
                                                                int playlist)
        {
            using (var context = new MyDbContext(_options))
            {
                PlaylistTrackRepository repository = new PlaylistTrackRepository(context);
                IGridServer<PlaylistTrack> server = new GridServer<PlaylistTrack>(repository.GetForPlaylistId(playlist), new QueryCollection(query),
                    true, "playlistTrackGrid", columns, 10)
                        .Sortable(true)
                        .WithPaging(10)
                        .Searchable(true)
                        ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public async Task<PlaylistTrack> Get(params object[] keys)
        {
            using (var context = new MyDbContext(_options))
            {
                var repository = new PlaylistTrackRepository(context);
                return await repository.GetById(keys);
            }
        }

        public async Task Insert(PlaylistTrack item)
        {
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var repository = new PlaylistTrackRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(PlaylistTrack item)
        {
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var repository = new PlaylistTrackRepository(context);
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
                    var playlistTrack = await Get(keys);
                    var repository = new PlaylistTrackRepository(context);
                    repository.Delete(playlistTrack);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the employee");
                }
            }
        }
    }

    public interface IPlaylistTrackService : ICrudDataService<PlaylistTrack>
    {
        ItemsDTO<PlaylistTrack> GetPlaylistTrackGridRows(Action<IGridColumnCollection<PlaylistTrack>> columns,
            QueryDictionary<StringValues> query, int Playlist);
    }
}
