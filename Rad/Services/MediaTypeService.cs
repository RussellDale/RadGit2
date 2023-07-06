using Rad.Models.Domian;
using GridShared;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace Rad.Services
{
    public class MediaTypeService : IMediaTypeService
    {
        private readonly DbContextOptions<MyDbContext> _options;

        public MediaTypeService(DbContextOptions<MyDbContext> options)
        {
            _options = options;
        }

        public IEnumerable<SelectItem> GetAllMediaType()
        {
            using (var context = new MyDbContext(_options))
            {
                MediaTypeRepository repository = new MediaTypeRepository(context);
                return repository.GetAll()
                    .Select(r => new SelectItem(r.MediaTypeId.ToString(), r.MediaTypeId.ToString() + " - "
                        + r.Name))
                    .ToList();
            }
        }

    }

    public interface IMediaTypeService
    {
        IEnumerable<SelectItem> GetAllMediaType();
    }
}
