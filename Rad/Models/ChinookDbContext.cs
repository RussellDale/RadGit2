using Microsoft.EntityFrameworkCore;

namespace Rad.Models.Domian
{
    public class ChinookDbContext : RadShared.Data.SharedDbContext<ChinookDbContext>
    {

        public ChinookDbContext(DbContextOptions<ChinookDbContext> options)
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