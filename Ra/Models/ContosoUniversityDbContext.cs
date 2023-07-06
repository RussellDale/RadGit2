using Microsoft.EntityFrameworkCore;

namespace Ra.Models.Domian
{
    public class ContosoUniversityDbContext : RadShared.Data.SharedDbContext<ContosoUniversityDbContext>
    {

        public ContosoUniversityDbContext(DbContextOptions<ContosoUniversityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
/*
            modelBuilder.Entity<Artist>();
            modelBuilder.Entity<Album>();
            modelBuilder.Entity<Track>();
            modelBuilder.Entity<MediaType>();
            modelBuilder.Entity<Genre>();
            modelBuilder.Entity<Playlist>();
            modelBuilder.Entity<PlaylistTrack>();
*/
            base.OnModelCreating(modelBuilder);
        }
/*
        public DbSet<Artist>        Artist        { get; set; }
        public DbSet<Album>         Album         { get; set; }
        public DbSet<Track>         Track         { get; set; }
        public DbSet<MediaType>     MediaType     { get; set; }
        public DbSet<Genre>         Genre         { get; set; }
        public DbSet<Playlist>      Playlist      { get; set; }
        public DbSet<PlaylistTrack> PlaylistTrack { get; set; }
*/
    }
}
