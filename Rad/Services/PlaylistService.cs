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
    public class PlaylistService : IPlaylistService
    {
        private readonly DbContextOptions<MyDbContext> _options;

        public PlaylistService(DbContextOptions<MyDbContext> options)
        {
            _options = options;
        }

        public ItemsDTO<Playlist> GetPlaylistGridRows(Action<IGridColumnCollection<Playlist>> columns,
            QueryDictionary<StringValues> query)
        {
            using (var context = new MyDbContext(_options))
            {
                PlaylistRepository repository = new PlaylistRepository(context);
                IGridServer<Playlist> server = new GridServer<Playlist>(repository.GetAll(),
                    new QueryCollection(query), true, "playlistGrid", columns, 10)
                        .Sortable(true)
                        .WithPaging(10)
                        .Searchable(true)
                        ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public async Task<Playlist> Get(params object[] keys)
        {
            using (var context = new MyDbContext(_options))
            {
                int PlaylistId;
                int.TryParse(keys[0].ToString(), out PlaylistId);
                var repository = new PlaylistRepository(context);
                return await repository.GetById(PlaylistId);
            }
        }

        public async Task Insert(Playlist item)
        {
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var repository = new PlaylistRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Playlist item)
        {
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var repository = new PlaylistRepository(context);
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
                    var Playlist = await Get(keys);
                    var repository = new PlaylistRepository(context);
                    repository.Delete(Playlist);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the employee");
                }
            }
        }
    }

    public interface IPlaylistService : ICrudDataService<Playlist>
    {
        ItemsDTO<Playlist> GetPlaylistGridRows(Action<IGridColumnCollection<Playlist>> columns,
            QueryDictionary<StringValues> query);
    }
}
