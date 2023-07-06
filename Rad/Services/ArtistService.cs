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
    public class ArtistService : IArtistService
    {
        private readonly DbContextOptions<MyDbContext> _options;

        public ArtistService(DbContextOptions<MyDbContext> options)
        {
            _options = options;
        }

        public ItemsDTO<Artist> GetArtistGridRows(Action<IGridColumnCollection<Artist>> columns,
                                                  QueryDictionary<StringValues> query, int withPaging)
        {
            using (var context = new MyDbContext(_options))
            {
                ArtistRepository repository = new ArtistRepository(context);
                IGridServer<Artist> server = new GridServer<Artist>(repository.GetAll(), new QueryCollection(query),
                    true, "artistGrid", columns)
                        .WithPaging(withPaging)
                        .Searchable(true)
                        .Sortable(true)
                        ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public IEnumerable<SelectItem> GetAllArtist()
        {
            using (var context = new MyDbContext(_options))
            {
                ArtistRepository repository = new ArtistRepository(context);
                return repository.GetAll()
                    .Select(r => new SelectItem(r.ArtistId.ToString(), r.ArtistId.ToString() + " - "
                        + r.Name))
                    .ToList();
            }
        }

        public async Task<Artist> Get(params object[] keys)
        {
            using (var context = new MyDbContext(_options))
            {
                int artistId;
                int.TryParse(keys[0].ToString(), out artistId);
                var repository = new ArtistRepository(context);
                return await repository.GetById(artistId);
            }
        }

        public async Task Insert(Artist item)
        {
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var repository = new ArtistRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Artist item)
        {
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var repository = new ArtistRepository(context);
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
                    var artist = await Get(keys);
                    var repository = new ArtistRepository(context);
                    repository.Delete(artist);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the employee");
                }
            }
        }
    }

    public interface IArtistService : ICrudDataService<Artist>
    {
        ItemsDTO<Artist> GetArtistGridRows(Action<IGridColumnCollection<Artist>> columns,
                                           QueryDictionary<StringValues> query, int withPaging);
        IEnumerable<SelectItem> GetAllArtist();
    }
}
