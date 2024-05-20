namespace Tour_Booking.Data;

using Microsoft.EntityFrameworkCore;
using Tour_Booking.Models;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<Destination> Destinations { get; set; }
    public DbSet<Asset> Assets { get; set; }



}