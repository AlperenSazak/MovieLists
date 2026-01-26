using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DatabaseContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }

        // 🔥 YENİ DbSet'ler
        public DbSet<Comment> Comments { get; set; }
        public DbSet<MovieLike> MovieLikes { get; set; }
        public DbSet<WatchLater> WatchLaterMovies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
            });

            // Movie Configuration
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.TmdbId).IsRequired();
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Movies)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 🔥 YENİ - Comment Configuration
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasOne(e => e.Movie)
                      .WithMany(m => m.Comments)
                      .HasForeignKey(e => e.MovieId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Comments)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.NoAction); // Cascade conflict önlemek için
            });

            // 🔥 YENİ - MovieLike Configuration
            modelBuilder.Entity<MovieLike>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasOne(e => e.Movie)
                      .WithMany(m => m.Likes)
                      .HasForeignKey(e => e.MovieId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Likes)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.NoAction); // Cascade conflict önlemek için

                // Bir kullanıcı bir filme sadece 1 kez beğeni/beğenmeme yapabilir
                entity.HasIndex(e => new { e.UserId, e.MovieId }).IsUnique();
            });

            // WatchLater Configuration
            modelBuilder.Entity<WatchLater>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.TmdbId).IsRequired();

                entity.HasOne(e => e.User)
                      .WithMany(u => u.WatchLaterMovies)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Bir kullanıcı aynı filmi birden fazla kez ekleyemesin
                entity.HasIndex(e => new { e.UserId, e.TmdbId }).IsUnique();
            });
        }
    }
}