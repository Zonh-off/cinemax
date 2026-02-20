using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastucture.Data;

public class StoreContext(DbContextOptions<StoreContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<City> Cities => Set<City>();
    public DbSet<Cinema> Cinemas => Set<Cinema>();
    public DbSet<Hall> Halls => Set<Hall>();
    public DbSet<Seat> Seats => Set<Seat>();
    public DbSet<SeatType> SeatTypes => Set<SeatType>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Showtime> Showtimes => Set<Showtime>();
    public DbSet<ShowtimeSeatPrice> ShowtimeSeatPrices => Set<ShowtimeSeatPrice>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        foreach (var property in modelBuilder.Model.GetEntityTypes()
                    .SelectMany(t => t.GetProperties())
                    .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(18,2)");
        }
                
        modelBuilder.Entity<City>(b =>
        {
            b.Property(x => x.Name).HasMaxLength(200);
            b.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Cinema>(b =>
        {
            b.Property(x => x.Name).HasMaxLength(200);
            b.Property(x => x.Address).HasMaxLength(500);

            b.HasOne(x => x.City)
               .WithMany(x => x.Cinemas)
               .HasForeignKey(x => x.CityId)
               .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => new { x.CityId, x.Name }).IsUnique();
        });

        modelBuilder.Entity<Hall>(b =>
        {
            b.Property(x => x.Name).HasMaxLength(200);

            b.HasOne(x => x.Cinema)
               .WithMany(x => x.Halls)
               .HasForeignKey(x => x.CinemaId)
               .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(x => new { x.CinemaId, x.Name }).IsUnique();
        });

        modelBuilder.Entity<SeatType>(b =>
        {
            b.Property(x => x.Name).HasMaxLength(100);
            b.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Seat>(b =>
        {
            b.HasOne(x => x.Hall)
               .WithMany(x => x.Seats)
               .HasForeignKey(x => x.HallId)
               .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.SeatType)
               .WithMany()
               .HasForeignKey(x => x.SeatTypeId)
               .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => new { x.HallId, x.Row, x.Number }).IsUnique();
        });

        modelBuilder.Entity<Movie>(b =>
        {
            b.Property(x => x.Title).HasMaxLength(300);
            b.Property(x => x.PosterPath).HasMaxLength(500);

            b.HasIndex(x => x.ExternalId).IsUnique();
        });

        modelBuilder.Entity<Showtime>(b =>
        {
            b.HasOne(x => x.Movie)
               .WithMany()
               .HasForeignKey(x => x.MovieId)
               .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Hall)
               .WithMany(x => x.Showtimes)
               .HasForeignKey(x => x.HallId)
               .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(x => new { x.MovieId, x.StartTime });
            b.HasIndex(x => new { x.HallId, x.StartTime });
        });

        modelBuilder.Entity<ShowtimeSeatPrice>(b =>
        {
            b.HasKey(x => new { x.ShowtimeId, x.SeatTypeId });

            b.Property(x => x.Price).HasColumnType("decimal(18,2)");

            b.HasOne(x => x.Showtime)
               .WithMany(x => x.Prices)
               .HasForeignKey(x => x.ShowtimeId)
               .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.SeatType)
               .WithMany()
               .HasForeignKey(x => x.SeatTypeId)
               .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<Order>(b =>
        {
            b.HasOne(x => x.User)
               .WithMany()
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Showtime)
               .WithMany()
               .HasForeignKey(x => x.ShowtimeId)
               .OnDelete(DeleteBehavior.Restrict);

            b.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<OrderTicket>(b =>
        {
            b.HasOne(x => x.Order)
               .WithMany(x => x.Tickets)
               .HasForeignKey(x => x.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Showtime)
               .WithMany()
               .HasForeignKey(x => x.ShowtimeId)
               .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Seat)
               .WithMany()
               .HasForeignKey(x => x.SeatId)
               .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.SeatType)
               .WithMany()
               .HasForeignKey(x => x.SeatTypeId)
               .OnDelete(DeleteBehavior.Restrict);

            b.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");

            b.HasIndex(x => new { x.ShowtimeId, x.SeatId }).IsUnique();

            b.HasIndex(x => x.TicketCode).IsUnique();
        });
    }
}