using Rad.Models.Domian;
using GridMvc.Server;
using GridShared;
using GridShared.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Rad.Services
{
    public class TrackService : ITrackService
    {
        private readonly DbContextOptions<MyDbContext> _options;

        public TrackService(DbContextOptions<MyDbContext> options)
        {
            _options = options;
        }

        public ItemsDTO<Track> GetTrack(Action<IGridColumnCollection<Track>> columns,
                                          QueryDictionary<StringValues> query, int withPaging,
                                          int albumId)
        {
            using (var context = new MyDbContext(_options))
            {
                var repository = new TrackRepository(context);

                var server = new GridServer<Track>(repository.GetForTrack(albumId), new QueryCollection(query),
                        false, "tracksGrid" + albumId.ToString(), columns, 10)
                            .WithPaging(withPaging)
                            .Sortable()
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }

        public IEnumerable<SelectItem> GetAllTrack()
        {
            using (var context = new MyDbContext(_options))
            {
                TrackRepository repository = new TrackRepository(context);
                return repository.GetAll()
//                    .Select(r => new SelectItem(r.TrackId.ToString(), r.TrackId.ToString() + " - "
                    .Select(r => new SelectItem(r.AlbumId.ToString(), r.AlbumId.ToString() + " - "
                        + r.Name))
                    .ToList();
            }
        }

        public IEnumerable<Track> GetForTrack(int id)
        {
            using (var context = new MyDbContext(_options))
            {
                TrackRepository repository = new TrackRepository(context);
                return repository.GetForTrack(id);
            }
        }

        public async Task<Track> Get(params object[] keys)
        {
            using (var context = new MyDbContext(_options))
            {
                int trackId;
                int.TryParse(keys[0].ToString(), out trackId);
                var repository = new TrackRepository(context);
                return await repository.GetById(trackId);
            }
        }

        public async Task Insert(Track item)
        {
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var repository = new TrackRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(Track item)
        {
            using (var context = new MyDbContext(_options))
            {
                try
                {
                    var repository = new TrackRepository(context);
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
                    var track = await Get(keys);
                    var repository = new TrackRepository(context);
                    repository.Delete(track);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the employee");
                }
            }
        }
    }

 
    public interface ITrackService : ICrudDataService<Track>
    {
        ItemsDTO<Track> GetTrack(Action<IGridColumnCollection<Track>> columns,
                                 QueryDictionary<StringValues> query, int withPaging, int albumId);
        IEnumerable<SelectItem> GetAllTrack();
        IEnumerable<Track> GetForTrack(int id);
    }
}
