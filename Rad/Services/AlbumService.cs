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
    public class AlbumService : IAlbumService
    {
        private readonly DbContextOptions<MyDbContext> _options;

        public AlbumService(DbContextOptions<MyDbContext> options)
        {
            _options = options;
        }

        public ItemsDTO<Album> GetAlbumId(Action<IGridColumnCollection<Album>> columns,
                                          QueryDictionary<StringValues> query, int withPaging,
                                          int artistId)
        {
            using (var context = new MyDbContext(_options))
            {
                AlbumRepository repository = new AlbumRepository(context);
                IGridServer<Album> server = new GridServer<Album>(repository.GetForAlbum(artistId), new QueryCollection(query),
                    true, "albumGrid", columns, 10)
                        .WithPaging(withPaging)
                        .Sortable()
                        ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public IEnumerable<SelectItem> GetAllAlbum()
        {
            using (var context = new MyDbContext(_options))
            {
                AlbumRepository repository = new AlbumRepository(context);
                return repository.GetAll()
//                    .Select(r => new SelectItem(r.AlbumId.ToString(), r.AlbumId.ToString() + " - "
                     .Select(r => new SelectItem(r.ArtistId.ToString(), r.ArtistId.ToString() + " - "
                       + r.Title))
                    .ToList();
            }
        }

        public IEnumerable<Album> GetForAlbum(int id)
        {
            using (var context = new MyDbContext(_options))
            {
                AlbumRepository repository = new AlbumRepository(context);
                return repository.GetForAlbum(id);
            }
        }

        public async Task<Album> Get(params object[] keys)
        {
            using (var context = new MyDbContext(_options))
            {
                int albumId;
                int.TryParse(keys[0].ToString(), out albumId);
                var repository = new AlbumRepository(context);
                return await repository.GetById(albumId);
            }
        }

        public async Task Insert(Album item)
        {
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var repository = new AlbumRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Album item)
        {
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var repository = new AlbumRepository(context);
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
                    var album = await Get(keys);
                    var repository = new AlbumRepository(context);
                    repository.Delete(album);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the employee");
                }
            }
        }
    }

    public interface IAlbumService : ICrudDataService<Album>
    {
        ItemsDTO<Album> GetAlbumId(Action<IGridColumnCollection<Album>> columns,
                                          QueryDictionary<StringValues> query, int withPaging,
                                          int artistId);
        IEnumerable<SelectItem> GetAllAlbum();

        IEnumerable<Album> GetForAlbum(int id);
    }
}
