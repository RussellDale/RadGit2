using Rad.Models.Domian;
using GridShared;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace Rad.Services
{
    public class GenreService : IGenreService
    {
        private readonly DbContextOptions<MyDbContext> _options;

        public GenreService(DbContextOptions<MyDbContext> options)
        {
            _options = options;
        }

        public IEnumerable<SelectItem> GetAllGenre()
        {
            using (var context = new MyDbContext(_options))
            {
                GenreRepository repository = new GenreRepository(context);
                return repository.GetAll()
                    .Select(r => new SelectItem(r.GenreId.ToString(), r.GenreId.ToString() + " - "
                        + r.Name))
                    .ToList();
            }
        }

    }

    public interface IGenreService
    {
        IEnumerable<SelectItem> GetAllGenre();
    }
}
